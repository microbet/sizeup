(function () {
    sizeup.core.namespace('sizeup.views.competition');
    sizeup.views.competition.competition = function (opts) {


        var defaults = {
            itemsPerPage: 10,
            slideTime: 500,
            dblClickZoom: 18,
            mapRadius:100
        };
        var me = {};
        
        me.opts = $.extend(true, defaults, opts);

        me.data = {};
        me.container = $('#competition');
        var dataLayer = new sizeup.core.data();
        var templates = new sizeup.core.templates(me.container);

        me.activePickerIndex = null;
        me.activeContentIndex = null;


        me.competitor = {};
        me.buyer = {};
        me.supplier = {};

        me.data.competitor =
        {
            industries: {},
            pickerIndustries: {},
            primaryIndustry: me.opts.CurrentIndustry.Id,
            businesses: {
                items: {},
                markers: {},
                infoWindow: null,
                isStale: true
            }
        };
        me.data.buyer =
        {
            industries: {},
            pickerIndustries: {},
            businesses: {
                items: {},
                markers: {},
                infoWindow: null,
                isStale: true
            }
        };
        me.data.supplier = 
        {
            industries: {},
            pickerIndustries: {},
            businesses: {
                items: {},
                markers: {},
                infoWindow: null,
                isStale: true
            }
        };

        var notifier = new sizeup.core.notifier(function () { init(); });
        dataLayer.isAuthenticated(notifier.getNotifier(function (data) { me.isAuthenticated = data; }));
        dataLayer.getCityBoundingBox({id: opts.CurrentCity.Id}, notifier.getNotifier(function (data) { 
            me.data.cityBoundingBox = new sizeup.maps.latLngBounds();
            me.data.cityBoundingBox.extend(new sizeup.maps.latLng({lat: data[0].Lat, lng: data[0].Lng}));
            me.data.cityBoundingBox.extend(new sizeup.maps.latLng({lat: data[1].Lat, lng: data[1].Lng}));
        }));

        var params = jQuery.bbq.getState();

        if (params.competitor) {
            dataLayer.getIndustries({ ids: typeof params.competitor == 'object' ? params.competitor : [params.competitor] }, notifier.getNotifier(function (data) { me.data.competitor.industries = data; }));
        }
        if (params.buyer) {
            dataLayer.getIndustries({ ids: typeof params.buyer == 'object' ? params.buyer : [params.buyer] }, notifier.getNotifier(function (data) { me.data.buyer.industries = data; }));
        }
        if (params.supplier) {
            dataLayer.getIndustries({ ids: typeof params.supplier == 'object' ? params.supplier : [params.supplier] }, notifier.getNotifier( function (data) { me.data.supplier.industries = data; }));
        }
      

        var init = function () {
            initGeneral('competitor');
            initGeneral('supplier');
            initGeneral('buyer');

            showTab('competitor');
            activateTab('competitor');
            showContent('competitor');

            if (params.buyer) {
                showTab('buyer');
                hideQuestion('buyer');
            }
            if (params.supplier) {
                showTab('supplier');
                hideQuestion('supplier');
            }
            updateMaps();
        };
       
        var initGeneral = function (index) {
            me[index].tab = me.container.find('#tabs .' + index).removeClass('hidden').hide();
            me[index].tab.click(function () { tabClicked(index); });

            me[index].question = me.container.find('#questions .' + index);
            me[index].question.click(function () { questionClicked(index); });

            me[index].content = {};
            me[index].content.container = me.container.find('.content.' + index).removeClass('hidden').hide();
            me[index].content.loader = me[index].content.container.find('.loading').removeClass('hidden').hide();
            me[index].content.businessList = me[index].content.container.find('.businessList');

            me[index].content.businessList.find('a').live('mouseover', function () {
                var a = $(this);
                var id = a.attr('data-id');
                me.data[index].businesses.markers[id].triggerEvent('mouseover');
            }).live('mouseout', function () {
                var a = $(this);
                var id = a.attr('data-id');
                me.data[index].businesses.markers[id].triggerEvent('mouseout');
            }).live('click', function () {
                var a = $(this);
                var id = a.attr('data-id');
                me.data[index].businesses.markers[id].triggerEvent('click');
            }).live('dblclick', function () {
                var a = $(this);
                var id = a.attr('data-id');
                me.data[index].businesses.markers[id].triggerEvent('dblclick');
            });



            me[index].content.pager = new sizeup.controls.pager({
                container: me[index].content.container.find('.pager'),
                templates: templates,
                templateId: index + 'Pager',
                onUpdate: function (data) { pagerOnUpdate(index, data); }
            });
            me[index].content.pager.hide();

            me[index].content.changer = me[index].content.container.find('.change');
            me[index].content.changer.click(function () { changeClicked(index); });

            me[index].picker = {};
            me[index].picker.container = me.container.find('.picker.' + index).removeClass('hidden').hide();
            me[index].picker.list = me.container.find('.industryList');
            me[index].picker.list.find('a').on('click', function () {
                var a = $(this);
                var id = a.attr('data-id');
                a.parent().remove();
                industryRemoved(index, id);
            });

            me[index].picker.textbox = me[index].picker.container.find('.pickerInput');
            me[index].picker.cancel = me[index].picker.container.find('.cancel');
            me[index].picker.cancel.click(function () { cancelClicked(index); });

            me[index].picker.submit = me[index].picker.container.find('.submit');
            me[index].picker.submit.click(function () { submitClicked(index); });


            me[index].picker.selector = new sizeup.controls.industrySelector({
                textbox: me[index].picker.textbox,
                onChange: function (item) { industryPicked(index, item); }
            });


            me[index].content.map = new sizeup.maps.businessMap({
                container: me[index].content.container.find('.mapContent'),
                radius: me.opts.mapRadius,
                cityId: me.opts.CurrentCity.Id,
                primaryIndex: index
            });
            me[index].content.map.fitBounds(me.data.cityBoundingBox);
        };


        var activateTab = function (index) {
            me['competitor'].tab.removeClass('active');
            me['buyer'].tab.removeClass('active');
            me['supplier'].tab.removeClass('active');
            me[index].tab.addClass('active');
        };


        var showTab = function (index) {
            me[index].tab.show();
        };

        var hideTab = function (index) {
            me[index].tab.hide();
        };

        var showQuestion = function (index) {
            me[index].question.show();
        };

        var hideQuestion = function (index) {
            me[index].question.hide();
        };


        var showPicker = function (index) {
            me[index].picker.container.show();
            me.activePickerIndex = index;
            setupPicker(index);
        };

        var slideInPicker = function (index, callback) {
            me[index].picker.container.show(
               "slide",
               { direction: "right" },
               me.opts.slideTime,
           callback);
            me.activePickerIndex = index;
            setupPicker(index);
        };

        var slideOutPicker = function (index, callback) {
            me[index].picker.container.hide(
              "slide",
              { direction: "right" },
              me.opts.slideTime,
          callback);
            me.activePickerIndex = null;
        };


        var showContent = function (index) {
            if (me.data[index].businesses.isStale) {
                loadBusinesses(index);
            }
            me[index].content.container.show();
            me[index].content.map.triggerEvent('resize');
            me.activeContentIndex = index;
        };

        var hideContent = function (index) {
            me[index].content.container.hide();
        };

        var slideInContent = function (index, callback) {
            if (me.data[index].businesses.isStale) {
                loadBusinesses(index);
            }
            me[index].content.container.show(
               "slide",
               { direction: "left" },
               me.opts.slideTime,
               function () {
                   me[index].content.map.triggerEvent('resize');
                   if (callback) {
                       callback();
                   }
               }
           );
           me.activeContentIndex = index;
        };

        var slideOutContent = function (index, callback) {
            me[index].content.container.hide(
                "slide",
                { direction: "left" },
                me.opts.slideTime,
            callback);
        };

        var tabClicked = function (index) {
            if (me.activePickerIndex != null) {
                slideOutPicker(me.activePickerIndex, function () {
                    slideInContent(index);
                });
            }
            else {
                hideContent(me.activeContentIndex);
                showContent(index);
            }
            activateTab(index);
        };

        var changeClicked = function (index) {
            slideOutContent(index, function () {
                slideInPicker(index);
            });
        };

        var cancelClicked = function (index) {
            slideOutPicker(index, function () {
                slideInContent(me.activeContentIndex);
            });
        };

        var submitClicked = function (index) {
            if (me.data[index].businesses.isStale) {
                me.data[index].industries = me.data[index].pickerIndustries;
                var ids = extractIds(me.data[index].industries);
                var obj = {};
                obj[index] = ids;
                jQuery.bbq.pushState(obj);
                me[index].content.pager.gotoPage(1);
            }
            slideOutPicker(index, function () {
                slideInContent(index);
            });
            showTab(index);
            activateTab(index);
            hideQuestion(index);
            bindListIndustries(index);
            updateMaps();
        };

        var questionClicked = function (index) {
            if (me.activePickerIndex != null) {
                slideOutPicker(me.activePickerIndex, function () {
                    slideInPicker(index);
                });
            }
            else {
                slideOutContent(me.activeContentIndex, function () {
                    slideInPicker(index);
                });
            }
        };

        var updateMaps = function () {
            var obj = {
                competitorIndustryIds: getIndustryIds('competitor'),
                buyerIndustryIds: getIndustryIds('buyer'),
                supplierIndustryIds: getIndustryIds('supplier')
            };
            me['competitor'].content.map.setIndustryIds(obj);
            me['buyer'].content.map.setIndustryIds(obj);
            me['supplier'].content.map.setIndustryIds(obj);
        };


        var setupPicker = function (index) {
            me.data[index].pickerIndustries = me.data[index].industries;
            me[index].picker.list.empty();
            for (var x in me.data[index].pickerIndustries) {
                bindPickerIndustry(index, me.data[index].pickerIndustries[x]);
            }
        };

        var bindPickerIndustry = function (index, item) {
            var i = templates.bind(templates.get(index + 'PickerItem'), item);
            me[index].picker.list.append(i);
        };

        var industryPicked = function (index, item) {
            if(!me.data[index].pickerIndustries[item.Id]){
                me.data[index].pickerIndustries[item.Id] = item;
                bindPickerIndustry(index, item);
                me.data[index].businesses.isStale = true;
            }
            me[index].picker.selector.setSelection(null);
        };

        var industryRemoved = function (index, id) {
            me.data[index].businesses.isStale = true;
            delete me.data[index].pickerIndustries[id];
        };

        var pagerOnUpdate = function (index, data) {
            loadBusinesses(index);
        };

        var extractIds = function (array) {
            var ar = new Array();
            for (var x in array) {
                ar.push(array[x].Id);
            }
            return ar;
        };

        var getIndustryIds = function (index) {
            var ar = new Array();
            ar = ar.concat(extractIds(me.data[index].industries));
            if (me.data[index].primaryIndustry) {
                ar.push(me.data[index].primaryIndustry);
            }
            return ar;
        };


        var bindListIndustries = function (index) {

        };

        var loadBusinesses = function (index) {
            me[index].content.loader.show();
            me[index].content.businessList.hide();
            me[index].content.pager.hide();
            var pagerData = me[index].content.pager.getPageData();
            dataLayer.getBusinessesByIndustry({
                industryIds: getIndustryIds(index),
                cityId: me.opts.CurrentCity.Id,
                itemCount: me.opts.itemsPerPage,
                page: pagerData.page
            }, function (data) {
                me.data[index].businesses.isStale = false;
                bindBusinesses(index, data);
                me[index].content.loader.hide();
            });
        };

        var bindBusinesses = function (index, data) {
            me[index].content.pager.setState(data);
            if (data.Count > me.opts.itemsPerPage && me.isAuthenticated) {
                me[index].content.pager.show();
            }
            else {
                me[index].content.pager.hide();
            }
            me.data[index].businesses.items = {};
            clearMarkers(index);
            var viewBounds = new sizeup.maps.latLngBounds();
            var html = '';
            for (var x = 0; x < data.Items.length;x++) {
                var template = templates.get(index + 'BusinessItem');
                html = html + templates.bind(template, { index: x + 1, business: data.Items[x] });

                var marker = createMarker(index, data.Items[x], x + 1);
                me.data[index].businesses.items[data.Items[x].Id] = data.Items[x];
                me.data[index].businesses.markers[data.Items[x].Id] = marker;
                viewBounds.extend(marker.getPosition());
            };
            if (data.Count == 0) {
                viewBounds = me.data.cityBoundingBox;
            }
            me[index].content.map.fitBounds(viewBounds);
            me[index].content.businessList.html(html);
            me[index].content.businessList.show();
        };

        var createMarker = function (index, business, label) {
            var marker = new sizeup.maps.imageMarker({
                position: new sizeup.maps.latLng({ lat: business.Lat, lng: business.Lng }),
                section: index,
                index: label
            });


            //wrap these in inner funcs so we can pass along the index and id and jank
            marker.bindEvent('click', function () {
                createInfoWindow(index, business, marker);
                
            });
            marker.bindEvent('dblclick', markerDblClicked);
            me[index].content.map.addMarker(marker);
            return marker;
        };



        var createInfoWindow = function (index, business, marker) {
            var content = templates.bind(templates.get(index + 'InfoWindow'), business);
            if (me.data[index].businesses.infoWindow) {
                me.data[index].businesses.infoWindow.close();
            }
            me.data[index].businesses.infoWindow = new sizeup.maps.infoWindow({
                content: content
            });
            me.data[index].businesses.infoWindow.open(me[index].content.map, marker);
        };

       
        var markerDblClicked = function () { }


        var clearMarkers = function (index) {
            for (var x in me.data[index].businesses.markers) {
                me[index].content.map.removeMarker(me.data[index].businesses.markers[x]);
            }
            me.data[index].businesses.markers = {};
        };


        var publicObj = {

        };
        return publicObj;
        
    };
})();