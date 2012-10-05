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

        me.data = {
            activeIndex: 'competitor',
            competitor: {
                pageData:null,
                businesses:{},
                industries: {},
                primaryIndustry: me.opts.CurrentInfo.CurrentIndustry
            },
            buyer: {
                pageData: null,
                businesses: {},
                industries: {}
            },
            supplier: {
                pageData: null,
                businesses: {},
                industries: {}
            },
            consumerExpenditures: {
                activeOverlays: []
            }
        };
        me.container = $('#competition');
        var dataLayer = new sizeup.core.data();
        var templates = new sizeup.core.templates(me.container);

       


        var notifier = new sizeup.core.notifier(function () { init(); });
        dataLayer.isAuthenticated(notifier.getNotifier(function (data) { me.isAuthenticated = data; }));
        dataLayer.getPlaceBoundingBox({id: opts.CurrentInfo.CurrentPlace.Id}, notifier.getNotifier(function (data) { 
            me.data.cityBoundingBox = new sizeup.maps.latLngBounds();
            me.data.cityBoundingBox.extend(new sizeup.maps.latLng({lat: data[0].Lat, lng: data[0].Lng}));
            me.data.cityBoundingBox.extend(new sizeup.maps.latLng({lat: data[1].Lat, lng: data[1].Lng}));
        }));

        var params = jQuery.bbq.getState();

        var insertIndustries = function (index, data) {
            for (var x in data) {
                me.data[index].industries[data[x].Id] = data[x];
            }
        };

        if (params.competitor) {
            dataLayer.getIndustries({ ids: typeof params.competitor == 'object' ? params.competitor : [params.competitor] }, notifier.getNotifier(function (data) { insertIndustries('competitor', data); }));
        }
        if (params.buyer) {
            dataLayer.getIndustries({ ids: typeof params.buyer == 'object' ? params.buyer : [params.buyer] }, notifier.getNotifier(function (data) { insertIndustries('buyer', data); }));
        }
        if (params.supplier) {
            dataLayer.getIndustries({ ids: typeof params.supplier == 'object' ? params.supplier : [params.supplier] }, notifier.getNotifier(function (data) { insertIndustries('supplier', data); }));
        }
      


        var init = function () {
            me.content = {};

            me.content.container = me.container.find('.content.container');
            me.content.loader = me.content.container.find('.loading').removeClass('hidden').hide();
            me.content.noResults = me.content.container.find('.noResults').removeClass('hidden').hide();
            me.content.businessList = me.content.container.find('.businessList').removeClass('hidden').hide();
            me.content.industryList = me.content.container.find('.industryList');

  
            me.content.map = new sizeup.maps.map({
                container: me.container.find('.mapWrapper.container .map')
            });
            me.content.map.fitBounds(me.data.cityBoundingBox);
    




            me.content.ConsumerExpenditure = {
                menuContent: me.content.container.find('.mapWrapper.container .consumerExpenditurePicker').hide().removeClass('hidden'),
                startOver: me.content.container.find('.mapWrapper.container .consumerExpenditurePicker .startOver'),
                menu: me.content.container.find('.mapWrapper.container .menu'),
                selectionList: me.content.container.find('.mapWrapper.container .consumerExpenditurePicker .selection'),
                childList: me.content.container.find('.mapWrapper.container .consumerExpenditurePicker .children')
            };

            $('body').click(function (e) {
                if (!$(e.target).parents().is(me.content.ConsumerExpenditure.menuContent) && me.content.ConsumerExpenditure.menuContent.is(':visible')) {
                    me.content.ConsumerExpenditure.menuContent.hide();
                }
            });


            var loadCsVariables = function (parentId) {
                me.content.ConsumerExpenditure.childList.empty();
                //toggle load icon
                dataLayer.getConsumerExpenditureVariables({ parentId: parentId }, function (data) {
                    var html = '';
                    for (var x in data) {
                        html = html + templates.bind(templates.get('consumerExpenditureListItem'), data[x]);
                    }
                    me.content.ConsumerExpenditure.childList.html(html);
                });
            };

            var setHeatmap = function (id) {

                for (var x in me.data.consumerExpenditures.activeOverlays) {
                    me.content.map.removeOverlay(me.data.consumerExpenditures.activeOverlays[x]);
                }
                me.data.consumerExpenditures.activeOverlays = [];

                if (id != null) {
                    me.data.consumerExpenditures.activeOverlays.push(new sizeup.maps.overlay({
                        tileUrl: '/tiles/consumerExpenditures/zip/',
                        tileParams: {
                            colors: [
                                        '#F5F500',
                                        '#F5CC00',
                                        '#F5A300',
                                        '#F57A00',
                                        '#F55200',
                                        '#F52900',
                                        '#F50000'
                            ].join(','),
                            variableId: id,
                            //boundingEntityId: 'co222'
                            boundingEntityId: 'm755'
                        },
                        minZoom: 10,
                        maxZoom: 20
                    }));

                    me.data.consumerExpenditures.activeOverlays.push(new sizeup.maps.overlay({
                        tileUrl: '/tiles/consumerExpenditures/county/',
                        tileParams: {
                            colors: [
                                        '#F5F500',
                                        '#F5CC00',
                                        '#F5A300',
                                        '#F57A00',
                                        '#F55200',
                                        '#F52900',
                                        '#F50000'
                            ].join(','),
                            variableId: id,
                            boundingEntityId: 's5'
                        },
                        minZoom: 0,
                        maxZoom: 9
                    }));


                    for (var x in me.data.consumerExpenditures.activeOverlays) {
                        me.content.map.addOverlay(me.data.consumerExpenditures.activeOverlays[x]);
                    }
                }
            };

            me.content.ConsumerExpenditure.menu.click(function (e) {
                me.content.ConsumerExpenditure.menuContent.toggle();
                e.stopPropagation();
            });

    
            me.content.ConsumerExpenditure.selectionList.delegate('a', 'click', function (e) {
                var a = $(this);
                var item = a.parent();
                item.nextAll().remove();
                var id = a.attr('data-id');
                setHeatmap(id);
                loadCsVariables(id);
                e.stopPropagation();
            });

            me.content.ConsumerExpenditure.childList.delegate('a', 'click', function (e) {
                var a = $(this);
                a.addClass('inverse');
                var item = a.parent();
                item.remove();
                var id = a.attr('data-id');
                me.content.ConsumerExpenditure.selectionList.append(item);
                setHeatmap(id);
                loadCsVariables(id);
                e.stopPropagation();
            });

            me.content.ConsumerExpenditure.startOver.click(function (e) {
                me.content.ConsumerExpenditure.selectionList.empty();
                loadCsVariables(null);
                setHeatmap(null);

                e.stopPropagation();
            });

            loadCsVariables(null);





            me.content.pager = new sizeup.controls.pager({
                container: me.content.container.find('.pager').removeClass('hidden').hide(),
                templates: templates,
                templateId: 'pager',
                onUpdate: function (data) { pagerOnUpdate(data); }
            });


            me.content.signinPanel = {
                container: me.content.container.find('.signinWrapper').removeClass('hidden').hide(),
                toggle: me.content.container.find('.signinWrapper .signinToggle'),
                control: new sizeup.views.shared.signin({
                    container: me.content.container.find('.signinWrapper .signinPanel'),
                    toggle: me.content.container.find('.signinWrapper .signinToggle')
                })
            };
            me.content.signinPanel.templateText = me.content.signinPanel.toggle.html();



            me.filters = {};
            me.filters.container = me.container.find('.filters.container');//.removeClass('hidden').hide();
           

            me.filters.picker = new sizeup.controls.industrySelector({
                textbox: me.filters.container.find('.pickerInput'),
                onChange: function (item) { industryPicked(item); }
            });



            me.addIndustries = me.container.find('.change');
            me.addIndustries.click(addIndustryClicked);


            me.data[me.data.activeIndex].pageData = me.content.pager.getPageData();
            loadBusinesses();
        };

         
       
        var addIndustryClicked = function () {
            var item = $(this);
            var type = item.attr('data-type');
            showFilters();

        };


        var showFilters = function (e) {
            me.filters.container.slideToggle(me.opts.slideTime);
        };



        var pushUrlState = function () {

            var getIds = function (data) {
                var ids = [];
                for (var x in data) {
                    ids.push(data[x].Id);
                }
                return ids;
            };

            var data = {};
            var competitors = getIds(me.data.competitor.industries);
            var suppliers = getIds(me.data.supplier.industries);
            var buyers = getIds(me.data.buyer.industries);
            if (competitors.length > 0) {
                data.competitor = competitors;
            }
            if (suppliers.length > 0) {
                data.supplier = suppliers;
            }
            if (buyers.length > 0) {
                data.buyer = buyers;
            }

            jQuery.bbq.pushState(data);
        };


        var industryPicked = function (data) {
            me.data[me.data.activeIndex].industries[data.Id] = data;
            var element = me.content.industryList.find('.item[data-id="' + data.Id + '"]');
            if (element.length > 0) {
                element.addClass('highlight', 250, function () {
                    element.removeClass('highlight', 1000);
                });
            }
            else {
                pushUrlState();
                bindIndustryList();
                loadBusinesses();
            }
        };


        var loadBusinesses = function () {
            var industries = $.extend({}, me.data[me.data.activeIndex].industries);
            if (me.data[me.data.activeIndex].primaryIndustry) {
                industries[me.data[me.data.activeIndex].primaryIndustry.Id] = me.data[me.data.activeIndex].primaryIndustry;
            }
            var ids = [];
            for (var x in industries) {
                ids.push(industries[x].Id);
            }

            me.content.loader.show();
            me.content.businessList.hide();
            me.content.noResults.hide();
            me.content.pager.getContainer().hide();


            dataLayer.getBusinessesByIndustry({
                industryIds: ids,
                placeId: me.opts.CurrentInfo.CurrentPlace.Id,
                itemCount: me.opts.itemsPerPage,
                page: me.data[me.data.activeIndex].pageData.page
            }, function (data) {
                me.data[me.data.activeIndex].businesses = data;
                bindBusinesses();
                me.content.loader.hide();
                me.content.businessList.show();
            });
        };













        var bindIndustryList = function () {
            var data = me.data[me.data.activeIndex].industries;
            var html = '';
            for (var x in data) {
                html = html + templates.bind(templates.get('industryItem'), data[x]);
            }
            me.content.industryList.html(html);
        };


        var bindBusinesses = function () {
            new sizeup.core.analytics().competitionTabLoaded({ tab: me.data.activeIndex });
            var data = me.data[me.data.activeIndex].businesses;
            me.content.pager.setState(data);
            if (data.Count == 0) {
                me.content.noResults.show();
                me.content.signinPanel.container.hide();
            }
            else {
                me.content.noResults.hide();
                if (!me.isAuthenticated) {
                    me.content.signinPanel.container.show();
                    me.content.signinPanel.toggle.html(templates.bind(me.content.signinPanel.templateText, me.content.pager.getPageData()));
                }
            }
            
            if (data.Count > me.opts.itemsPerPage && me.isAuthenticated) {
                me.content.pager.getContainer().show();
            }
            else {
                me.content.pager.getContainer().hide();
            }




            var html = '';
            for (var x = 0; x < data.Items.length; x++) {
                var template = templates.get('businessItem');
                html = html + templates.bind(template, { index: x + 1, business: data.Items[x] });

                //var marker = createMarker(index, data.Items[x], x + 1);
                //me.data[index].businesses.items[data.Items[x].Id] = data.Items[x];
                //me.data[index].businesses.markers[data.Items[x].Id] = marker;
                //viewBounds.extend(marker.getPosition());
            };
            me.content.businessList.html(html);






            /*
            if (me.data[index].signinPanel.templateText) {
                me[index].content.signinPanel.toggle.html(templates.bind(me.data[index].signinPanel.templateText, me[index].content.pager.getPageData()));
                me[index].content.signinPanel.toggle.show();
            }

        
            me.data[index].businesses.items = {};
            clearMarkers(index);
            var viewBounds = new sizeup.maps.latLngBounds();
       
           
            if (data.Count == 0) {
                viewBounds = me.data.cityBoundingBox;
                me[index].content.noResults.show();
                me[index].content.signinPanel.toggle.hide();
                me[index].content.pager.hide();
            }
            me[index].content.map.fitBounds(viewBounds);
           */
           



        };

        var pagerOnUpdate = function (data) {
            me.data[me.data.activeIndex].pageData = data;

            //
        };

        /*

        var initGeneral = function (index) {
            me[index].tab = me.container.find('#tabs .' + index).removeClass('hidden').hide();
            me[index].tab.click(function () { tabClicked(index); });

            me[index].question = me.container.find('#questions .' + index);
            me[index].question.click(function () { questionClicked(index); });

            me[index].content = {};
            me[index].content.container = me.container.find('.content.' + index).removeClass('hidden').hide();
            me[index].content.loader = me[index].content.container.find('.loading').removeClass('hidden').hide();
            me[index].content.noResults = me[index].content.container.find('.noResults').removeClass('hidden').hide();
            me[index].content.businessList = me[index].content.container.find('.businessList');
            me[index].content.industryList = me[index].content.container.find('.industryList');
            me[index].content.industryMessage = me[index].content.container.find('.industryMessage');

            me[index].content.businessList.delegate('a', 'mouseover', function () {
                var a = $(this);
                var id = a.attr('data-id');
                me.data[index].businesses.markers[id].triggerEvent('mouseover');
            }).delegate('a', 'mouseout', function () {
                var a = $(this);
                var id = a.attr('data-id');
                me.data[index].businesses.markers[id].triggerEvent('mouseout');
            }).delegate('a', 'click', function () {
                var a = $(this);
                var id = a.attr('data-id');
                me.data[index].businesses.markers[id].triggerEvent('click');
            }).delegate('a', 'dblclick', function () {
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

            me[index].content.signinPanel = {
                toggle: me[index].content.container.find('.signinWrapper .signinToggle').hide().removeClass('hidden'),
                control: new sizeup.views.shared.signin({
                    container: me[index].content.container.find('.signinWrapper .signinPanel'),
                    toggle: me[index].content.container.find('.signinWrapper .signinToggle')
                })
            };

            me.data[index].signinPanel.templateText = me[index].content.signinPanel.toggle.html();


            me[index].content.changer = me[index].content.container.find('.change');
            me[index].content.changer.click(function () { changeClicked(index); });

            me[index].picker = {};
            me[index].picker.container = me.container.find('.picker.' + index).removeClass('hidden').hide();
            me[index].picker.list = me[index].picker.container.find('.industryList');
            me[index].picker.list.delegate('a', 'click', function () {
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
                cityId: me.opts.CurrentInfo.CurrentPlace.City.Id,
                primaryIndex: index
            });

            me[index].content.map.fitBounds(me.data.cityBoundingBox);
            me[index].content.map.addEventListener('click', function (e) { mapClicked(index, { lat: e.latLng.lat(), lng: e.latLng.lng() }); });

            bindIndustryList(index);
        };


        var activateTab = function (index) {
            me['competitor'].tab.removeClass('active');
            me['buyer'].tab.removeClass('active');
            me['supplier'].tab.removeClass('active');
            me['supplier'].content.container.hide();
            me['buyer'].content.container.hide();
            me['competitor'].content.container.hide();
            me[index].content.container.show();
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

            me[index].picker.container.slideDown(me.opts.slideTime, callback);
            me.activePickerIndex = index;
            setupPicker(index);
        };

        var slideOutPicker = function (index, callback) {
            me[index].picker.container.hide(
              "slide",
              { direction: "right" },
              me.opts.slideTime,
          callback);

            me[index].picker.container.slideUp(me.opts.slideTime, callback);
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
            if (callback) {
                callback();
            }
            me[index].content.container.show();
            me[index].content.map.triggerEvent('resize');
           me.activeContentIndex = index;
        };

        var slideOutContent = function (index, callback) {
            me[index].content.container.hide(
                "slide",
                { direction: "left" },
                me.opts.slideTime,
            callback);

            if (callback) {
                callback();
            }
        };

        var getActiveIds = function (index) {
            var activeIndexes = me[index].content.map.getActiveIndexes();
            var ids = [];
            for (var x in activeIndexes) {
                if (activeIndexes[x]) {
                    ids = ids.concat(getIndustryIds(x));
                }
            }
            return ids;
        };

        var mapClicked = function (index, latLng) {
            var ids = getActiveIds(index);
            dataLayer.getBusinessAt({ lat: latLng.lat, lng: latLng.lng, industryIds: ids }, function (data) { createPin(index, data); });
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
                bindIndustryList(index);
                updateMaps();           
            }
            slideOutPicker(index, function () {
                slideInContent(index, function () {
                    showTab(index);
                    activateTab(index);
                    hideQuestion(index);
                });
            });

           
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
            clearPins('competitor');
            clearPins('buyer');
            clearPins('supplier');
            me['competitor'].content.map.setIndustryIds(obj);
            me['buyer'].content.map.setIndustryIds(obj);
            me['supplier'].content.map.setIndustryIds(obj);
        };


        var setupPicker = function (index) {
            new sizeup.core.analytics().competitionIndustryPickerClicked({ tab: index });

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


        var bindIndustryList = function (index) {           
            var html = '';
            for (var x in me.data[index].industries) {
                html = html + templates.bind(templates.get(index + 'IndustryItem'), me.data[index].industries[x]);
            }
            if (me.data[index].industries.length > 0) {
                me[index].content.industryMessage.show();
            }
            else {
                me[index].content.industryMessage.hide();
            }
            me[index].content.industryList.html(html);
        };

        var loadBusinesses = function (index) {
            me[index].content.loader.show();
            me[index].content.businessList.hide();
            me[index].content.pager.hide();
            var pagerData = me[index].content.pager.getPageData();
            dataLayer.getBusinessesByIndustry({
                industryIds: getIndustryIds(index),
                placeId: me.opts.CurrentInfo.CurrentPlace.Id,
                itemCount: me.opts.itemsPerPage,
                page: pagerData.page
            }, function (data) {
                me.data[index].businesses.isStale = false;
                bindBusinesses(index, data);
                me[index].content.loader.hide();
            });
        };

        var bindBusinesses = function (index, data) {
            new sizeup.core.analytics().competitionTabLoaded({ tab: index });

            me[index].content.noResults.hide();
            me[index].content.pager.setState(data);
            if (me.data[index].signinPanel.templateText) {
                me[index].content.signinPanel.toggle.html(templates.bind(me.data[index].signinPanel.templateText, me[index].content.pager.getPageData()));
                me[index].content.signinPanel.toggle.show();
            }

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
                me[index].content.noResults.show();
                me[index].content.signinPanel.toggle.hide();
                me[index].content.pager.hide();
            }
            me[index].content.map.fitBounds(viewBounds);
            me[index].content.businessList.html(html);
            me[index].content.businessList.show();
        };

        var getPinColor = function (index, id) {
            var color = '';
            var activeIndexes = me[index].content.map.getActiveIndexes();
            if (activeIndexes['supplier']) {
                var ids = getIndustryIds('supplier');
                for (var x in ids) {
                    if (ids[x] == id) {
                        color = me.data['supplier'].color;
                    }
                }
            }
            if (activeIndexes['buyer']) {
                ids = getIndustryIds('buyer');
                for (var x in ids) {
                    if (ids[x] == id) {
                        color = me.data['buyer'].color;
                    }
                }
            }
            if (activeIndexes['competitor']) {
                ids = getIndustryIds('competitor');
                for (var x in ids) {
                    if (ids[x] == id) {
                        color = me.data['competitor'].color;
                    }
                }
            }
            return color;
        };

        var createPin = function(index, business){
            var marker = new sizeup.maps.imagePin({
                position: new sizeup.maps.latLng({ lat: business.Lat, lng: business.Lng }),
                color: getPinColor(index, business.IndustryId)
            });
            me.data[index].businesses.pins[business.Id] = { business: business, pin: marker };
            me[index].content.map.addMarker(marker);
            createInfoWindow(index, business, marker, true);
            marker.bindEvent('click', function () {
                createInfoWindow(index, business, marker, true);

            });
        };

        var createMarker = function (index, business, label) {
            var marker = new sizeup.maps.imageMarker({
                position: new sizeup.maps.latLng({ lat: business.Lat, lng: business.Lng }),
                section: index,
                index: label
            });
            marker.bindEvent('click', function () {
                createInfoWindow(index, business, marker);
                
            });
            marker.bindEvent('dblclick', function () {
                me[index].content.map.setZoom(28);
                createInfoWindow(index, business, marker);
            });
            me[index].content.map.addMarker(marker);
            return marker;
        };



        var createInfoWindow = function (index, business, marker, isPin) {
            var content = templates.bind(templates.get(index + 'InfoWindow'), business);
            var jContent = $(content);
            if (me.data[index].businesses.infoWindow) {
                me.data[index].businesses.infoWindow.close();
            }
            me.data[index].businesses.infoWindow = new sizeup.maps.infoWindow({
                content: jContent.get(0)
            });
            me.data[index].businesses.infoWindow.open(me[index].content.map, marker);

            jContent.find('.tools .zoom').click(function (e) {
                e.stopPropagation();
                me[index].content.map.setCenter(new sizeup.maps.latLng({ lat: business.Lat, lng: business.Lng }));
                me[index].content.map.setZoom(24);
            });

            if (isPin) {
                jContent.find('.tools .remove').click(function (e) {
                    e.stopPropagation();
                    removePin(index, business.Id);
                });
            }
            else {
                jContent.find('.tools .remove').remove();
            }
        };

        var clearMarkers = function (index) {
            for (var x in me.data[index].businesses.markers) {
                me[index].content.map.removeMarker(me.data[index].businesses.markers[x]);
            }
            me.data[index].businesses.markers = {};
        };

        var collectAllIndustryIds = function () {
            var ids = [];
            ids = ids.concat(getIndustryIds('competitor'));
            ids = ids.concat(getIndustryIds('buyer'));
            ids = ids.concat(getIndustryIds('supplier'));
            return ids;
        };

        var clearPins = function (index) {
            var ids = collectAllIndustryIds();
            for (var x in me.data[index].businesses.pins) {
                for (var y in ids) {
                    if (me.data[index].businesses.pins[x].business.IndustryId == ids[y]) {
                        me[index].content.map.removeMarker(me.data[index].businesses.pins[x].pin);
                        delete me.data[index].businesses.pins[x];
                    }
                }
            }
        };

        var removePin = function(index, id){
            me[index].content.map.removeMarker(me.data[index].businesses.pins[id].pin);
            delete me.data[index].businesses.pins[id];
        };
        */

        var publicObj = {

        };
        return publicObj;
        
    };
})();



