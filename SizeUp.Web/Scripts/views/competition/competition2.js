(function () {
    sizeup.core.namespace('sizeup.views.competition');
    sizeup.views.competition.competition = function (opts) {


        var defaults = {
            itemsPerPage: 10,
            dblClickZoom: 18,
            mapRadius:100
        };
        var me = {};
        
        me.opts = $.extend(true, defaults, opts);

        me.data = {
            activeIndex: 'competitor',
            businessOverlay: null,
            infoWindow: null,
            pins: {},
            markers:{},
            competitor: {
                pageData:null,
                businesses:{},
                industries: {},
                primaryIndustry: me.opts.CurrentInfo.CurrentIndustry,
                pinColor: 'FF5522'
            },
            buyer: {
                pageData: null,
                businesses: {},
                industries: {},
                pinColor: '66EE00'
            },
            supplier: {
                pageData: null,
                businesses: {},
                industries: {},
                pinColor: '11AAFF'
            },
            consumerExpenditure: {
                activeOverlays: [],
                rootId: 1,
                currentSelection:null
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
        if (params.rootId) {
            me.data.consumerExpenditure.rootId = params.rootId;
        }
        if (params.consumerExpenditureVariable) {
            me.data.consumerExpenditure.currentSelection = params.consumerExpenditureVariable;
            //fire request for path
        }
        if (params.activeTab) {
            me.data.activeIndex = params.activeTab;
        }
      


        var init = function () {
            me.content = {};

            me.content.container = me.container.find('.content.container');
            me.content.loader = me.content.container.find('.loading').removeClass('hidden').hide();
            me.content.noResults = me.content.container.find('.noResults').removeClass('hidden').hide();
            me.content.addIndustries = me.content.container.find('.addIndustries').removeClass('hidden').hide();
            me.content.businessList = me.content.container.find('.businessList').removeClass('hidden').hide();
            me.content.industryList = me.content.container.find('.industryList');

  
            me.content.businessList
                .delegate('a', 'mouseover', businessItemMouseOver)
                .delegate('a', 'mouseout', businessItemMouseOut)
                .delegate('a', 'click', businessItemClicked)
                .delegate('a', 'dblclick', businessItemDoubleClicked);


            me.content.mapControls = {
                container: me.container.find('.mapControls.container').hide().removeClass('hidden'),
                filter: me.container.find('.mapControls.container .mapFilter').hide().removeClass('hidden'),
                consumerExpenditures: me.container.find('.mapControls.container .consumerExpenditures').hide().removeClass('hidden')
            };

            me.content.map = new sizeup.maps.map({
                container: me.container.find('.mapWrapper.container .map')
            });
            me.content.map.fitBounds(me.data.cityBoundingBox);

            me.content.questions = {
                buyer: me.container.find('.questions .buyers'),
                supplier: me.container.find('.questions .suppliers'),
                consumer: me.container.find('.questions .consumers')
            };

            me.content.tabs = {
                competitor: me.content.container.find('.tabs .competitors'),
                supplier: me.content.container.find('.tabs .suppliers').hide().removeClass('hidden'),
                buyer: me.content.container.find('.tabs .buyers').hide().removeClass('hidden')
            };

            me.content.ConsumerExpenditure = {
                menuContent: me.content.container.find('.mapControls.container .consumerExpenditurePicker').hide().removeClass('hidden'),
                startOver: me.content.container.find('.mapControls.container .consumerExpenditurePicker .startOver'),
                close: me.content.container.find('.mapControls.container .consumerExpenditurePicker .close'),
                menu: me.content.container.find('.mapControls.container .menu'),
                selectionList: me.content.container.find('.mapControls.container .consumerExpenditurePicker .selection'),
                childList: me.content.container.find('.mapControls.container .consumerExpenditurePicker .children')
            };


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

            me.picker = new sizeup.controls.industrySelector({
                textbox: me.container.find('.industryPicker .pickerInput'),
                onChange: function (item) { industryPicked(item); }
            });


            $('body').click(consumerExpenditureOnBodyClicked);
            me.content.ConsumerExpenditure.menu.click(consumerExpenditureMenuClicked);
            me.content.ConsumerExpenditure.selectionList.delegate('a', 'click', consumerExpenditureSelectionListItemClicked);
            me.content.ConsumerExpenditure.childList.delegate('a', 'click', consumerExpenditureChildListItemClicked);
            me.content.ConsumerExpenditure.startOver.click(consumerExpenditureStartOverClicked);
            me.content.ConsumerExpenditure.close.click(consumerExpenditureCloseClicked);

            me.content.industryList.delegate('a', 'click', removeIndustryClicked);
            me.content.map.addEventListener('click', function (e) { mapClicked({ lat: e.latLng.lat(), lng: e.latLng.lng() }); });

            me.content.questions.buyer.click(buyerQuestionClicked);
            me.content.questions.supplier.click(supplierQuestionClicked);
            me.content.questions.consumer.click(consumerExpenditureQuestionClicked);

            me.content.tabs.buyer.click(function () { activateTab('buyer'); });
            me.content.tabs.supplier.click(function () { activateTab('supplier'); });
            me.content.tabs.competitor.click(function () { activateTab('competitor'); });



            

            me.data.competitor.pageData = me.content.pager.getPageData();
            me.data.supplier.pageData = me.content.pager.getPageData();
            me.data.buyer.pageData = me.content.pager.getPageData();

           
            if (getIndustryIdArray('buyer').length > 0) {
                showTab('buyer');
            }
            else if(me.data.activeIndex == 'buyer'){
                me.data.activeIndex = 'competitor';
            }
            if (getIndustryIdArray('supplier').length > 0) {
                showTab('supplier');
            }
            else if (me.data.activeIndex == 'supplier') {
                me.data.activeIndex = 'competitor';
            }
            activateTab(me.data.activeIndex);
        };

         
        //////event actions//////////////////
     
        var buyerQuestionClicked = function () {
            me.content.questions.buyer.addClass('disabled');
            var doActivate = !me.content.tabs.buyer.is(':visible');
            showTab('buyer');
            if (doActivate) {
                activateTab('buyer');
            }
        };

        var supplierQuestionClicked = function () {
            me.content.questions.supplier.addClass('disabled');
            var doActivate = !me.content.tabs.supplier.is(':visible');
            showTab('supplier');
            if (doActivate) {
                activateTab('supplier');
            }
        };

        var consumerExpenditureQuestionClicked = function () {
            me.content.questions.consumer.addClass('disabled');
            showConsumerExpenditures();
        };

        var businessItemDoubleClicked = function () {
            var a = $(this);
            var id = a.attr('data-id');
            me.data[index].businesses.markers[id].triggerEvent('dblclick');
        };

        var businessItemClicked = function () {
            var a = $(this);
            var id = a.attr('data-id');
            me.data.markers[id].triggerEvent('click');
        };

        var businessItemMouseOver = function () {
            var a = $(this);
            var id = a.attr('data-id');
            me.data.markers[id].triggerEvent('mouseover');
        };

        var businessItemMouseOut = function () {
            var a = $(this);
            var id = a.attr('data-id');
            me.data.markers[id].triggerEvent('mouseout');
        };

        var consumerExpenditureOnBodyClicked = function (e) {
            if (!$(e.target).parents().is(me.content.ConsumerExpenditure.menuContent) && me.content.ConsumerExpenditure.menuContent.is(':visible')) {
                me.content.ConsumerExpenditure.menuContent.hide();
            }
        };

        var consumerExpenditureMenuClicked = function (e) {
            me.content.ConsumerExpenditure.menuContent.toggle();
            e.stopPropagation();
        };

        var consumerExpenditureStartOverClicked = function (e) {
            me.content.ConsumerExpenditure.selectionList.empty();
            me.data.consumerExpenditure.currentSelection = null;
            loadCsVariables(me.data.consumerExpenditure.rootId);
            setHeatmap(null);
            pushUrlState();
            e.stopPropagation();
        };

        var consumerExpenditureCloseClicked = function (e) {
            me.content.ConsumerExpenditure.menuContent.toggle();
            e.stopPropagation();
        };

        var consumerExpenditureSelectionListItemClicked = function (e) {
            var a = $(this);
            var item = a.parent();
            item.nextAll().remove();
            var id = a.attr('data-id');
            me.data.consumerExpenditure.currentSelection = id;
            setHeatmap(id);
            loadCsVariables(id);
            pushUrlState();
            e.stopPropagation();
        };

        var consumerExpenditureChildListItemClicked = function (e) {
            var a = $(this);
            var item = a.parent();
            item.remove();
            var id = a.attr('data-id');
            me.data.consumerExpenditure.currentSelection = id;
            me.content.ConsumerExpenditure.selectionList.append(item);
            setHeatmap(id);
            loadCsVariables(id);
            pushUrlState();
            e.stopPropagation();
        };

        var pagerOnUpdate = function (data) {
            me.data[me.data.activeIndex].pageData = data;
            loadBusinesses();
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
                me.data[me.data.activeIndex].pageData = me.content.pager.gotoPage(1);
                pushUrlState();
                bindIndustryList();
                loadBusinesses();
                setBusinessOverlay();
            }
        };

        var removeIndustryClicked = function (e) {
            var a = $(this);
            var id = a.attr('data-id');
            delete me.data[me.data.activeIndex].industries[id];
            me.data[me.data.activeIndex].pageData = me.content.pager.gotoPage(1);
            pushUrlState();
            bindIndustryList();
            loadBusinesses();
            setBusinessOverlay();
        };

        var mapClicked = function (latLng) {
            var getIds = function (data) {
                var ids = [];
                for (var x in data) {
                    ids.push(data[x].Id);
                }
                return ids;
            };

            var ids = [];
            var competitors = getIds(me.data.competitor.industries);
            var suppliers = getIds(me.data.supplier.industries);
            var buyers = getIds(me.data.buyer.industries);
            ids.push(me.data.competitor.primaryIndustry.Id);
            if (competitors.length > 0) {
                ids.push(competitors);
            }
            if (suppliers.length > 0) {
                ids.push(suppliers);
            }
            if (buyers.length > 0) {
                ids.push(buyers);
            }

            dataLayer.getBusinessAt({ lat: latLng.lat, lng: latLng.lng, industryIds: ids }, function (data) { createPin(data); });
        };

        
        //////////end event actions/////////////////////////////
        var getIndustryIdArray = function (index) {
            var industries = $.extend({}, me.data[index].industries);
            if (me.data[index].primaryIndustry) {
                industries[me.data[index].primaryIndustry.Id] = me.data[index].primaryIndustry;
            }
            var ids = [];
            for (var x in industries) {
                ids.push(industries[x].Id);
            }
            return ids;
        };

        var showConsumerExpenditures = function () {
            me.content.mapControls.container.show();
            me.content.mapControls.consumerExpenditures.show();
            loadCsVariables(me.data.consumerExpenditure.rootId);
        };

        var showMapFilters = function () {
            me.content.mapControls.container.show();
            me.content.mapControls.mapFilter.show();
        };

        var showTab = function (tabIndex) {
            me.content.tabs[tabIndex].show();
        };

        var activateTab = function (tabIndex) {
            for (var x in me.content.tabs) {
                me.content.tabs[x].removeClass('active');
            };
            me.content.tabs[tabIndex].addClass('active');
            me.data.activeIndex = tabIndex; 

            me.container.removeClass('competitor').removeClass('supplier').removeClass('buyer').addClass(me.data.activeIndex);

            var ids = getIndustryIdArray(me.data.activeIndex);

            if (ids.length > 0) {
                bindIndustryList();
                loadBusinesses();
                setBusinessOverlay();
                pushUrlState();
            }
            else {
                //show the you need to add industries text
                me.content.addIndustries.show();
                me.content.pager.getContainer().hide();
                me.content.signinPanel.container.hide();
                clearIndustryList();
                clearBusinessList();
                clearMarkers();
                pushUrlState();
            }
        };

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

            for (var x in me.data.consumerExpenditure.activeOverlays) {
                me.content.map.removeOverlay(me.data.consumerExpenditure.activeOverlays[x]);
            }
            me.data.consumerExpenditure.activeOverlays = [];

            if (id != null) {
                me.data.consumerExpenditure.activeOverlays.push(new sizeup.maps.overlay({
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

                me.data.consumerExpenditure.activeOverlays.push(new sizeup.maps.overlay({
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


                for (var x in me.data.consumerExpenditure.activeOverlays) {
                    me.content.map.addOverlay(me.data.consumerExpenditure.activeOverlays[x]);
                }
            }
        };


        var setBusinessOverlay = function () {
            me.content.map.removeOverlay(me.data.businessOverlay);
   
            var state = jQuery.bbq.getState();
            var data = {};
            data.competitorIndustryIds = getIndustryIdArray('competitor');
            data.supplierIndustryIds = getIndustryIdArray('supplier');
            data.buyerIndustryIds = getIndustryIdArray('buyer');


            if (data.supplierIndustryIds.length == 0) {
                delete data.supplierIndustryIds;
            }
            if (data.buyerIndustryIds.length == 0) {
                delete data.buyerIndustryIds;
            }

            me.data.businessOverlay = new sizeup.maps.overlay({
                tileUrl: '/tiles/businesses/',
                tileParams: data
            });
            me.content.map.addOverlay(me.data.businessOverlay);
        };



        var pushUrlState = function () {

            var data = {};
            var competitors = getIndustryIdArray('competitor');
            competitors.splice(competitors.indexOf(me.data.competitor.primaryIndustry.Id), 1);
            var suppliers = getIndustryIdArray('supplier');
            var buyers = getIndustryIdArray('buyer');
            if (competitors.length > 0) {
                data.competitor = competitors;
            }
            if (suppliers.length > 0) {
                data.supplier = suppliers;
            }
            if (buyers.length > 0) {
                data.buyer = buyers;
            }
            if (me.data.consumerExpenditure.currentSelection != null) {
                data.consumerExpenditure = me.data.consumerExpenditure.currentSelection;
            }
            data.activeTab = me.data.activeIndex;
 
            jQuery.bbq.pushState(data, 2);
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
            me.content.addIndustries.hide();

            dataLayer.getBusinessesByIndustry({
                industryIds: ids,
                placeId: me.opts.CurrentInfo.CurrentPlace.Id,
                itemCount: me.opts.itemsPerPage,
                page: me.data[me.data.activeIndex].pageData.page
            }, function (data) {
                me.data[me.data.activeIndex].businesses = data;
                bindBusinesses();
                bindBusinessMarkers();
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
            };
            me.content.businessList.html(html);
        };

        var clearIndustryList = function () {
            me.content.industryList.empty();
        };

        var clearBusinessList = function () {
            me.content.businessList.empty();
        };

        var bindBusinessMarkers = function () {
            var data = me.data[me.data.activeIndex].businesses;
            var index = 1;
            clearMarkers();
            for (var x in data.Items) {
                var marker = createMarker(data.Items[x], index);
                me.data.markers[data.Items[x].Id] = marker;
                me.content.map.addMarker(marker);
                index = index + 1;
            }
        };


        var createMarker = function (business, label) {
            var marker = new sizeup.maps.businessMarker({
                position: new sizeup.maps.latLng({ lat: business.Lat, lng: business.Lng }),
                section: me.data.activeIndex,
                index: label
            });
            marker.bindEvent('click', function () {
                createInfoWindow(business, marker);

            });
            marker.bindEvent('dblclick', function () {
                me.content.map.setZoom(28);
                createInfoWindow(business, marker);
            });
            return marker;
        };

        var getPinColor = function (business) {
            var color = '';
            var ids = getIndustryIdArray('supplier');
            for (var x in ids) {
                if (business.IndustryId == ids[x]) {
                    color = me.data.supplier.pinColor;
                }
            };

            ids = getIndustryIdArray('buyer');
            for (var x in ids) {
                if (business.IndustryId == ids[x]) {
                    color = me.data.buyer.pinColor;
                }
            };

            ids = getIndustryIdArray('competitor');
            for (var x in ids) {
                if (business.IndustryId == ids[x]) {
                    color = me.data.competitor.pinColor;
                }
            };
            return color;
        };

        var createPin = function (business) {
            if (me.data.pins[business.Id] == null) {
                var marker = new sizeup.maps.imagePin({
                    position: new sizeup.maps.latLng({ lat: business.Lat, lng: business.Lng }),
                    color: getPinColor(business)
                });
                me.data.pins[business.Id] = { business: business, pin: marker };
                me.content.map.addMarker(marker);
                createInfoWindowPin(business, marker);
                marker.bindEvent('click', function () {
                    createInfoWindowPin(business, marker);

                });
            }
        };

        var clearMarkers = function () {
            for (var x in me.data.markers) {
                me.content.map.removeMarker(me.data.markers[x]);
            }
            me.data.markers = {};
        };

        var removePin = function (id) {
            me.content.map.removeMarker(me.data.pins[id].pin);
            delete me.data.pins[id];
        };


        var createInfoWindow = function (business, marker) {
            var content = templates.bind(templates.get('infoWindow'), business);
            var jContent = $(content);
            if (me.data.infoWindow) {
                me.data.infoWindow.close();
            }
            me.data.infoWindow = new sizeup.maps.infoWindow({
                content: jContent.get(0)
            });
            me.data.infoWindow.open(me.content.map, marker);

            jContent.find('.tools .zoom').click(function (e) {
                e.stopPropagation();
                me.content.map.setCenter(new sizeup.maps.latLng({ lat: business.Lat, lng: business.Lng }));
                me.content.map.setZoom(24);
            });
            jContent.find('.tools .remove').remove();
        };

  
        var createInfoWindowPin = function (business, pin) {
            var content = templates.bind(templates.get('infoWindow'), business);
            var jContent = $(content);
            if (me.data.infoWindow) {
                me.data.infoWindow.close();
            }
            me.data.infoWindow = new sizeup.maps.infoWindow({
                content: jContent.get(0)
            });
            me.data.infoWindow.open(me.content.map, pin);

            jContent.find('.tools .zoom').click(function (e) {
                e.stopPropagation();
                me.content.map.setCenter(new sizeup.maps.latLng({ lat: business.Lat, lng: business.Lng }));
                me.content.map.setZoom(24);
            });
            
            jContent.find('.tools .remove').click(function (e) {
                e.stopPropagation();
                removePin(business.Id);
            });

        };
      

        /*

      

          

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



