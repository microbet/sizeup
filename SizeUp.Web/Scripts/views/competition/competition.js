(function () {
    sizeup.core.namespace('sizeup.views.competition');
    sizeup.views.competition.competition = function (opts) {


        var defaults = {
            itemsPerPage: 10,
            dblClickZoom: 18,
            mapRadius: 100,
            mapFilterZoomThreshold: 12,
            maxAutoZoom: 16
        };
        var me = {};
        
        me.opts = $.extend(true, defaults, opts);

        me.data = {
            activeIndex: 'competitor',
            businessOverlay: null,
            infoWindow: null,
            activeMapFilter: 'all',
            currentMapFilter: 'all', 
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
                currentSelection: null,
                legend: null
            },
            businessListXHR: null
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
        }
        if (params.activeTab) {
            me.data.activeIndex = params.activeTab;
        }
        if (params.activeMapFilter) {
            me.data.activeMapFilter = params.activeMapFilter;
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
                container: me.container.find('.mapControls.container'),
                filter: me.container.find('.mapControls.container .mapFilter').hide().removeClass('hidden'),
                filterItems: me.container.find('.mapControls.container .mapFilter input[name=mapFilter]'),
                consumerExpenditures: me.container.find('.mapControls.container .consumerExpenditures')
            };
            me.content.mapControls.filter.find('.zoomMessage').hide();

            me.content.map = new sizeup.maps.map({
                container: me.container.find('.mapWrapper.container .map')
            });
            me.content.map.fitBounds(me.data.cityBoundingBox);

 

            me.content.questions = {
                buyer: me.content.container.find('.tabs .buyerQuestion'),
                supplier: me.content.container.find('.tabs .supplierQuestion')
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
                childList: me.content.container.find('.mapControls.container .consumerExpenditurePicker .children'),
                loading: me.content.container.find('.mapControls.container .consumerExpenditurePicker .loading').hide().removeClass('hidden')
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
            me.content.map.addEventListener('zoom_changed', mapZoomUpdated);

            
            me.content.questions.buyer.find('a').click(buyerQuestionClicked);
            me.content.questions.supplier.find('a').click(supplierQuestionClicked);

            me.content.tabs.buyer.find('a').click(function () { activateTab('buyer'); });
            me.content.tabs.supplier.find('a').click(function () { activateTab('supplier'); });
            me.content.tabs.competitor.find('a').click(function () { activateTab('competitor'); });

            me.container.find('.map').delegate('.ceType', 'click', consumerExpenditureTypeChanged);

            me.content.mapControls.filterItems.click(mapFilterClicked);

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

            
            setMapFilter(me.data.activeMapFilter);
          
         
            if (me.data.consumerExpenditure.currentSelection != null) {
                loadConsumerExpenditureSelection(me.data.consumerExpenditure.currentSelection);
                setHeatmap(me.data.consumerExpenditure.currentSelection);
                getLegendData();
            }
            else {
                loadConsumerExpenditureVariables(me.data.consumerExpenditure.rootId);
            }
        };

         
        //////event actions//////////////////
     
        var consumerExpenditureTypeChanged = function (e) {
            var target = $(e.target);
            var rootId = target.attr('data-value');

            new sizeup.core.analytics().consumerExpenditureTypeChanged({ label: rootId });

            if (me.data.consumerExpenditure.rootId != rootId) {
                me.container.find('.map .ceType').removeClass('active');
               target.addClass('active');

                me.data.consumerExpenditure.rootId = rootId;
                dataLayer.getConsumerExpenditureVariableCrosswalk({ id: me.data.consumerExpenditure.currentSelection }, function (data) {
                    me.data.consumerExpenditure.currentSelection = data.Id;
                    loadConsumerExpenditureSelection(me.data.consumerExpenditure.currentSelection);
                    setHeatmap(me.data.consumerExpenditure.currentSelection);
                    getLegendData();
                    pushUrlState();
                });
            }
        };

        var mapZoomUpdated = function () {
            checkMapFilterZoom();          
            getLegendData();
        };

        var mapFilterClicked = function (e) {
            var target = $(e.target);
            me.data.oldMapFilter = null;
            me.data.activeMapFilter = target.attr('data-index');
            pushUrlState();
            setBusinessOverlay();
        };
        
        var buyerQuestionClicked = function () {
            var doActivate = !me.content.tabs.buyer.is(':visible');
            showTab('buyer');
            if (doActivate) {
                activateTab('buyer');
            }
        };

        var supplierQuestionClicked = function () {
            var doActivate = !me.content.tabs.supplier.is(':visible');
            showTab('supplier');
            if (doActivate) {
                activateTab('supplier');
            }
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
            loadConsumerExpenditureVariables(me.data.consumerExpenditure.rootId);
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
            new sizeup.core.analytics().consumerExpenditureSelected({ label: a.html() });
            me.data.consumerExpenditure.currentSelection = id;
            setHeatmap(id);
            getLegendData();
            loadConsumerExpenditureVariables(id);
            pushUrlState();
            e.stopPropagation();
        };

        var consumerExpenditureChildListItemClicked = function (e) {
            var a = $(this);
            var item = a.parent();
            item.remove();
            var id = a.attr('data-id');
            new sizeup.core.analytics().consumerExpenditureSelected({ label: a.html() });
            var hasChildren = a.attr('data-hasChildren');
            if (hasChildren == "false") {
                me.content.ConsumerExpenditure.menuContent.toggle();
            }

            me.data.consumerExpenditure.currentSelection = id;
            me.content.ConsumerExpenditure.selectionList.append(item);
            setHeatmap(id);
            getLegendData();
            loadConsumerExpenditureVariables(id);
            pushUrlState();
            e.stopPropagation();
        };

        var pagerOnUpdate = function (data) {
            me.data[me.data.activeIndex].pageData = data;
            loadBusinesses();
        };

        var industryPicked = function (data) {
            me.picker.setSelection(null);
            if (data != null) {
                me.data[me.data.activeIndex].industries[data.Id] = data;
                var element = me.content.industryList.find('.item[data-id="' + data.Id + '"]');

                if (element.length > 0) {
                    element.addClass('highlight', 250, function () {
                        element.removeClass('highlight', 1000);
                    });
                }
                else {
                    trackIndustry(data.Id);
                    me.data[me.data.activeIndex].pageData = me.content.pager.gotoPage(1);
                    checkMapFilter();
                    pushUrlState();
                    bindIndustryList();
                    loadBusinesses();
                    setBusinessOverlay();
                }
            }
        };

        var trackIndustry = function (id) {
            var params = { primaryIndustryId: me.opts.CurrentInfo.CurrentIndustry.Id, relatedIndustryId: id, placeId: me.opts.CurrentInfo.CurrentPlace.Id };
            if (me.data.activeIndex == 'competitor') {
                new sizeup.core.analytics().relatedCompetitor(params);
            }
            else if (me.data.activeIndex == 'buyer') {
                new sizeup.core.analytics().relatedBuyer(params);
            }
            else if (me.data.activeIndex == 'supplier') {
                new sizeup.core.analytics().relatedSupplier(params);
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
            checkMapFilter();
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
       
        var showLegend = function () {
            if (me.data.consumerExpenditure.legend != null) {
                me.content.map.setLegend(me.data.consumerExpenditure.legend);
            }
        };

        var hideLegned = function () {
            me.content.map.clearLegend();
        };

        var setLegend = function (data) {
            me.data.consumerExpenditure.legend = new sizeup.maps.legend({
                templates: templates,
                title: data.title,
                items: data.items,
                colors: [
                '#F5F500',
                '#F5CC00',
                '#F5A300',
                '#F57A00',
                '#F55200',
                '#F52900',
                '#F50000'
                ],
                format: function (val) { return '$' + sizeup.util.numbers.format.abbreviate(val); }
            });
            me.content.map.setLegend(me.data.consumerExpenditure.legend);


            me.container.find('.map .ceType').removeClass('active');
            me.container.find('.map .ceType[data-value=' + me.data.consumerExpenditure.rootId + ']').addClass('active');
        };

        
        var setMapFilter = function (index) {
            me.data.activeMapFilter = index;
            me.content.mapControls.filter.find('input[data-index=' + index + ']').attr('checked', 'checked');
            pushUrlState();
            setBusinessOverlay();
        };

        var checkMapFilter = function () {
            if (getIndustryIdArray('buyer').length > 0 || getIndustryIdArray('supplier').length > 0) {
                me.content.mapControls.filter.show();
            }
            else {
                me.content.mapControls.filter.hide();
            }

            if (getIndustryIdArray('buyer').length > 0) {
                me.content.mapControls.filter.find('.buyer').show();
            } else {
                var item = me.content.mapControls.filter.find('.buyer').hide();
                if (item.find('input').is(':checked')) {
                    me.data.oldMapFilter = null;
                    setMapFilter('competitor');
                }
            }

            if (getIndustryIdArray('supplier').length > 0) {
                me.content.mapControls.filter.find('.supplier').show();
            } else {
                var item = me.content.mapControls.filter.find('.supplier').hide();
                if (item.find('input').is(':checked')) {
                    me.data.oldMapFilter = null;
                    setMapFilter('competitor');
                }
            }
        };

        var checkMapFilterZoom = function () {
            var z = me.content.map.getZoom();
            var allBox = me.content.mapControls.filter.find('input[data-index=all]');
            if (z < me.opts.mapFilterZoomThreshold) {
               
                me.content.mapControls.filter.find('.zoomMessage').show();
                allBox.attr('disabled', 'disabled');
                if (me.data.activeMapFilter == 'all') {
                    me.data.oldMapFilter = 'all';
                    setMapFilter(me.data.activeIndex);
                }
            }
            else if (z >= me.opts.mapFilterZoomThreshold) {
     
                allBox.removeAttr('disabled');
                me.content.mapControls.filter.find('.zoomMessage').hide();
                if (me.data.oldMapFilter != null) {
                    setMapFilter(me.data.oldMapFilter);
                }
            }
        };

        var getIndustryIdArray = function (index) {
            var industries = $.extend({}, me.data[index].industries);
            if (me.data[index].primaryIndustry) {
                industries[me.data[index].primaryIndustry.Id] = me.data[index].primaryIndustry;
            }
            var ids = new Array();
            for (var x in industries) {
                ids.push(industries[x].Id);
            }
            return ids;
        };

       
        var showTab = function (tabIndex) {
            me.content.tabs[tabIndex].show();
            me.content.questions[tabIndex].hide();
        };

        var activateTab = function (tabIndex) {
            abortLoadBusinesses();

            new sizeup.core.analytics().competitionTabLoaded({ tab: tabIndex });
            for (var x in me.content.tabs) {
                me.content.tabs[x].removeClass('active');
            };
            me.content.tabs[tabIndex].addClass('active');
            me.data.activeIndex = tabIndex; 

            me.container.removeClass('competitor').removeClass('supplier').removeClass('buyer').addClass(me.data.activeIndex);

            var ids = getIndustryIdArray(me.data.activeIndex);

            checkMapFilter();
            bindIndustryList();
            loadBusinesses();
            setBusinessOverlay();
            pushUrlState();
        };

        var loadConsumerExpenditureSelection = function (id) {

            dataLayer.getConsumerExpenditureVariablePath({ id: id }, function (data) {
                var html = '';
                //removes the root node as we arent using that
                data.shift();
                for (var x in data) {
                    html = html + templates.bind(templates.get('consumerExpenditureListItem'), data[x]);
                }
                me.content.ConsumerExpenditure.selectionList.html(html);
            });
            loadConsumerExpenditureVariables(id);
        };

        var loadConsumerExpenditureVariables = function (parentId) {
            me.content.ConsumerExpenditure.childList.empty();
            me.content.ConsumerExpenditure.loading.show();
            dataLayer.getConsumerExpenditureVariables({ parentId: parentId }, function (data) {
                me.content.ConsumerExpenditure.loading.hide();
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
                    opacity: 0.7,
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
                        boundingEntityId: 'co' + me.opts.CurrentInfo.CurrentPlace.County.Id
                    },
                    minZoom: 11,
                    maxZoom: 32
                }));


                if (me.opts.CurrentInfo.CurrentPlace.Metro.Id != null) {

                    me.data.consumerExpenditure.activeOverlays.push(new sizeup.maps.overlay({
                        tileUrl: '/tiles/consumerExpenditures/county/',
                        opacity: 0.7,
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
                            boundingEntityId: 'm' + me.opts.CurrentInfo.CurrentPlace.Metro.Id
                        },
                        minZoom: 8,
                        maxZoom: 10
                    }));

                    me.data.consumerExpenditure.activeOverlays.push(new sizeup.maps.overlay({
                        tileUrl: '/tiles/consumerExpenditures/county/',
                        opacity: 0.7,
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
                            boundingEntityId: 's' + me.opts.CurrentInfo.CurrentPlace.State.Id
                        },
                        minZoom: 5,
                        maxZoom: 7
                    }));
                }
                else {
                    me.data.consumerExpenditure.activeOverlays.push(new sizeup.maps.overlay({
                        tileUrl: '/tiles/consumerExpenditures/county/',
                        opacity: 0.7,
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
                            boundingEntityId: 's' + me.opts.CurrentInfo.CurrentPlace.State.Id
                        },
                        minZoom: 5,
                        maxZoom: 10
                    }));
                }


                me.data.consumerExpenditure.activeOverlays.push(new sizeup.maps.overlay({
                    tileUrl: '/tiles/consumerExpenditures/state/',
                    opacity: 0.7,
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
                        variableId: id
                    },
                    minZoom: 0,
                    maxZoom: 4
                }));


                for (var x in me.data.consumerExpenditure.activeOverlays) {
                    me.content.map.addOverlay(me.data.consumerExpenditure.activeOverlays[x], 0);
                }
                showLegend();
            }
            else {
                me.data.consumerExpenditure.legend = null;
                hideLegned();
            }
        };

        var getLegendData = function () {
            var z = me.content.map.getZoom();
            var ceType = me.data.consumerExpenditure.rootId == 1 ? 'Totals' : 'Averages';
            if (me.data.consumerExpenditure.currentSelection != null ) {
                me.data.consumerExpenditure.legendZoomLevel = z;
                var data = {
                    title: '',
                    items:[]
                };
                var notify = new sizeup.core.notifier(function () {
                    setLegend(data);
                });
                var itemsNotify = notify.getNotifier(function (d) { data.items = d; });

                if (z <= 32 && z >= 11) {

                    var titleNotify = notify.getNotifier(function (d) { data.title = 'Consumer Expenditure ' + ceType + ' for ' + d.Description + ' by zip code in ' + me.opts.CurrentInfo.CurrentPlace.County.Name + ', ' +  me.opts.CurrentInfo.CurrentPlace.State.Name; });


                    dataLayer.getConsumerExpenditureBandsByZip(
                            { bands: 7, variableId: me.data.consumerExpenditure.currentSelection, boundingEntityId: 'co' + me.opts.CurrentInfo.CurrentPlace.County.Id },
                            itemsNotify
                    );
                }


                if (me.opts.CurrentInfo.CurrentPlace.Metro.Id != null) {
                    if (z <= 10 && z >= 8) {

                        var titleNotify = notify.getNotifier(function (d) { data.title = 'Consumer Expenditure ' + ceType + ' for ' + d.Description + ' by county in ' + me.opts.CurrentInfo.CurrentPlace.Metro.Name; });



                        dataLayer.getConsumerExpenditureBandsByCounty(
                           { bands: 7, variableId: me.data.consumerExpenditure.currentSelection, boundingEntityId: 'm' + me.opts.CurrentInfo.CurrentPlace.Metro.Id },
                           itemsNotify);
                    }

                    if (z <= 8 && z >= 5) {

                        var titleNotify = notify.getNotifier(function (d) { data.title = 'Consumer Expenditure ' + ceType + ' for ' + d.Description + ' by county in ' + me.opts.CurrentInfo.CurrentPlace.State.Name; });



                        dataLayer.getConsumerExpenditureBandsByCounty(
                           { bands: 7, variableId: me.data.consumerExpenditure.currentSelection, boundingEntityId: 's' + me.opts.CurrentInfo.CurrentPlace.State.Id },
                           itemsNotify);
                    }
                }
                else {
                    if (z <= 10 && z >= 5) {

                        var titleNotify = notify.getNotifier(function (d) { data.title = 'Consumer Expenditure ' + ceType + ' for ' + d.Description + ' by county in ' + me.opts.CurrentInfo.CurrentPlace.State.Name; });



                        dataLayer.getConsumerExpenditureBandsByCounty(
                            { bands: 7, variableId: me.data.consumerExpenditure.currentSelection, boundingEntityId: 's' + me.opts.CurrentInfo.CurrentPlace.State.Id },
                            itemsNotify);
                    }
                }

                
                if (z <= 4 && z >= 0) {

                    var titleNotify = notify.getNotifier(function (d) { data.title = 'Consumer Expenditure ' + ceType + ' for ' + d.Description + ' by state in USA' });


                    dataLayer.getConsumerExpenditureBandsByState(
                            { bands: 7, variableId: me.data.consumerExpenditure.currentSelection },
                            itemsNotify);
                }


                dataLayer.getConsumerExpenditureVariable({ id: me.data.consumerExpenditure.currentSelection }, titleNotify);


            }
        };
 

        var setBusinessOverlay = function () {
            me.content.map.removeOverlay(me.data.businessOverlay);
   
            var state = jQuery.bbq.getState();
            var data = {};
            data.competitorIndustryIds = getIndustryIdArray('competitor');
            data.supplierIndustryIds = getIndustryIdArray('supplier');
            data.buyerIndustryIds = getIndustryIdArray('buyer');

            var filterValue = me.data.activeMapFilter;

            if (data.supplierIndustryIds.length == 0 || !(filterValue == 'all' || filterValue == 'supplier')) {
                delete data.supplierIndustryIds;
            }
            if (data.buyerIndustryIds.length == 0 || !(filterValue == 'all' || filterValue == 'buyer')) {
                delete data.buyerIndustryIds;
            }
            if (!(filterValue == 'all' || filterValue == 'competitor')) {
                delete data.competitorIndustryIds;
            }

            me.data.businessOverlay = new sizeup.maps.overlay({
                tileUrl: '/tiles/businesses/',
                tileParams: data
            });
            me.content.map.addOverlay(me.data.businessOverlay, 1);
        };



        var pushUrlState = function () {

            var data = {};
            var competitors = getIndustryIdArray('competitor');
            competitors.splice($.inArray(me.data.competitor.primaryIndustry.Id, competitors), 1);
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
                data.consumerExpenditureVariable = me.data.consumerExpenditure.currentSelection;
            }
            if (me.data.consumerExpenditure.rootId != null) {
                data.rootId = me.data.consumerExpenditure.rootId;
            }
            if (me.data.activeMapFilter != null) {
                data.activeMapFilter = me.data.activeMapFilter;
            }
            data.activeTab = me.data.activeIndex;


            jQuery.bbq.pushState(data, 2);
        };

        var abortLoadBusinesses = function () {
            if (me.data.businessListXHR != null) {
                me.data.businessListXHR.abort();
            }
            me.content.loader.hide();
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

            if (ids.length > 0) {

                me.content.loader.show();
                me.content.businessList.hide();
                me.content.noResults.hide();
                me.content.pager.getContainer().hide();
                me.content.addIndustries.hide();

                if (me.data.businessListXHR != null) {
                    me.data.businessListXHR.abort();
                }
                me.data.businessListXHR = dataLayer.getBusinessesByIndustry({
                    industryIds: ids,
                    placeId: me.opts.CurrentInfo.CurrentPlace.Id,
                    itemCount: me.opts.itemsPerPage,
                    page: me.data[me.data.activeIndex].pageData.page
                }, function (data) {
                    me.data[me.data.activeIndex].businesses = data;
                    me.data.businessListXHR = null;
                    bindBusinesses();
                    bindBusinessMarkers();
                    me.content.loader.hide();
                    me.content.businessList.show();
                });
            }
            else {
                me.content.addIndustries.show();
                me.content.pager.getContainer().hide();
                me.content.signinPanel.container.hide();
                clearIndustryList();
                clearBusinessList();
                clearMarkers();
                pushUrlState();
            }
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
                html = html + templates.bind(template, { index: me.data.activeIndex, number: x + 1, business: data.Items[x] });
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
            var bounds = new sizeup.maps.latLngBounds();
            for (var x in data.Items) {
                var marker = createMarker(data.Items[x], index);
                me.data.markers[data.Items[x].Id] = marker;
                me.content.map.addMarker(marker);
                bounds.extend(marker.getPosition());
                index = index + 1;
            }
            if (data.Items.length > 0) {
                me.content.map.fitBounds(bounds);
            }
            if(me.content.map.getZoom() > me.opts.maxAutoZoom){
                me.content.map.setZoom(me.opts.maxAutoZoom);
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
      

        var publicObj = {

        };
        return publicObj;
        
    };
})();



