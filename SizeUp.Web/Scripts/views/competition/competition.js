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
            restoredSession: false,
            activeIndex: 'competitor',
            businessOverlay: null,
            infoWindow: null,
            activeMapFilter: 'all',
            currentMapFilter: 'all',
            pins: {},
            markers: {},
            activePlace: me.opts.CurrentInfo.CurrentPlace,
            competitor: {
                pageData: null,
                businesses: {},
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
                legend: null,
                list: null,
                overlay: null
            },
            filters: {
                employees: {
                    competitor: {
                        min: null,
                        max: null
                    },
                    suppliers: {
                        min: null,
                        max: null
                    },
                    buyers: {
                        min: null,
                        max: null
                    }
                }

            },
            businessListXHR: null
        };
        me.container = $('#competition');
        var templates = new sizeup.core.templates(me.container);



        var setupNotifier = new sizeup.core.notifier(function () { setup(); });
        sizeup.core.profile.getCompetitionValues({ placeId: opts.CurrentInfo.CurrentPlace.Id, industryId: opts.CurrentInfo.CurrentIndustry.Id }, setupNotifier.getNotifier(function (data) {
            if (!jQuery.isEmptyObject(data)) {
                me.data.restoredSession = true;
            }
            $.extend(data, $.parseJSON(localStorage.getItem("cv-" + opts.CurrentInfo.CurrentPlace.Id + "-" + opts.CurrentInfo.CurrentIndustry.Id)));
            jQuery.bbq.pushState(data, 1);
        }));

        var setup = function () {
            var params = jQuery.bbq.getState();
            var notifier = new sizeup.core.notifier(function () { init(); });
            sizeup.api.data.getBoundingBox({ geographicLocationId: opts.CurrentInfo.CurrentPlace.Id }, notifier.getNotifier(function (data) {
                me.data.cityBoundingBox = new sizeup.maps.latLngBounds();
                me.data.cityBoundingBox.extend(new sizeup.maps.latLng({ lat: data.SouthWest.Lat, lng: data.SouthWest.Lng }));
                me.data.cityBoundingBox.extend(new sizeup.maps.latLng({ lat: data.NorthEast.Lat, lng: data.NorthEast.Lng }));
            }));

            if (params.consumerExpenditureVariable) {
                sizeup.api.data.getConsumerExpenditureVariable({ id: params.consumerExpenditureVariable }, notifier.getNotifier(function (data) {
                    me.data.consumerExpenditure.currentSelection = data;
                }));
            }



            var insertIndustries = function (index, data) {
                for (var x in data) {
                    me.data[index].industries[data[x].Id] = data[x];
                }
            };

            if (params.competitor) {
                sizeup.api.data.getIndustry({ id: params.competitor }, notifier.getNotifier(function (data) { insertIndustries('competitor', data); }));
            }
            if (params.buyer) {
                sizeup.api.data.getIndustry({ id: params.buyer }, notifier.getNotifier(function (data) { insertIndustries('buyer', data); }));
            }
            if (params.supplier) {
                sizeup.api.data.getIndustry({ id: params.supplier }, notifier.getNotifier(function (data) { insertIndustries('supplier', data); }));
            }
            if (params.rootId) {
                me.data.consumerExpenditure.rootId = params.rootId;
            }
            if (params.activeTab) {
                me.data.activeIndex = params.activeTab;
            }
            if (params.activeMapFilter) {
                me.data.activeMapFilter = params.activeMapFilter;
            }
        };



        var init = function () {
            me.content = {};

            me.content.container = me.container.find('.content.container');
            me.content.loader = me.content.container.find('.loading').removeClass('hidden').hide();
            me.content.noResults = me.content.container.find('.noResults').removeClass('hidden').hide();
            me.content.addIndustries = me.content.container.find('.addIndustries').removeClass('hidden').hide();
            me.content.businessList = me.content.container.find('.businessList').removeClass('hidden').hide();
            me.content.industryList = me.content.container.find('.industryList');
            me.content.businessListFootnote = me.content.container.find('.list.container .footNote').removeClass('hidden').hide();
            me.content.filters = {};
            me.content.filters.sliders = {};
            me.content.filters.labels = {};
            me.content.filters.sliders['employees'] = me.content.container.find('.employees');

            me.content.businessList
                .delegate('a', 'mouseover', businessItemMouseOver)
                .delegate('a', 'mouseout', businessItemMouseOut)
                .delegate('a', 'click', businessItemClicked)
                .delegate('a', 'dblclick', businessItemDoubleClicked);


            me.content.mapControls = {
                container: me.container.find('.mapControls.container'),
                filter: me.container.find('.mapControls.container .mapFilter').hide().removeClass('hidden'),
                filterItems: me.container.find('.mapControls.container .mapFilter input[name=mapFilter]'),
                consumerExpenditures: me.container.find('.mapControls.container .consumerExpenditures'),
                filterMessage: me.container.find('.mapControls.container .filterMessage').hide().removeClass('hidden')
            };
            me.content.mapControls.filter.find('.zoomMessage').hide();

            me.content.map = new sizeup.maps.map({
                container: me.container.find('.mapWrapper.container .map'),
                mapSettings: { panControl: false, zoomControl: true, zoomControlOptions: { style: google.maps.ZoomControlStyle.SMALL } }
            });
            me.content.map.fitBounds(me.data.cityBoundingBox);

            me.textAlternative = me.container.find('.mapWrapper .textAlternative');
            me.textAlternative.click(textAlternativeClicked);


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
                itemsPerPage: me.opts.IsAuthenticated ? me.opts.itemsPerPage : 3,
                onUpdate: function (data) { pagerOnUpdate(data); }
            });

            if (me.opts.IsCustomTools)
                me.content.pager.setItemsPerPage(10);

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

            me.sessionLoadedBox = new sizeup.controls.flashBox(
            {
                container: me.container.find('#sessionLoadedBox')
            });


            me.content.industryBox = me.container.find('#industryBox').hide().removeClass('hidden');
            me.content.changeIndustry = me.container.find('#changeIndustry');

            me.content.industrySelector = sizeup.controls.industrySelector({
                textbox: me.content.industryBox,
                revertToSelection: true,
                onChange: function (item) { onPrimaryIndustryChange(item); },
                onBlur: function () { industryBoxBlur(); }
            });

            me.content.placeBox = me.container.find('#placeBox').hide().removeClass('hidden');
            me.content.changePlace = me.container.find('#changePlace');

            me.content.placeSelector = sizeup.controls.placeSelector({
                textbox: me.content.placeBox,
                revertToSelection: true,
                onChange: function (item) { onPlaceChange(item); },
                onBlur: function () { placeBoxBlur(); }
            });




            me.content.industrySelector.setSelection(me.data.competitor.primaryIndustry);
            me.content.changeIndustry.click(changeIndustryClicked);

            me.content.placeSelector.setSelection(me.data.activePlace);
            me.content.changePlace.click(changePlaceClicked);


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

            $(window).bind('hashchange', function (e) { hashChanged(e); });


            me.data.competitor.pageData = me.content.pager.getPageData();
            me.data.supplier.pageData = me.content.pager.getPageData();
            me.data.buyer.pageData = me.content.pager.getPageData();


            if (getIndustryIdArray('buyer').length > 0) {
                showTab('buyer');
            }
            else if (me.data.activeIndex == 'buyer') {
                me.data.activeIndex = 'competitor';
            }
            if (getIndustryIdArray('supplier').length > 0) {
                showTab('supplier');
            }
            else if (me.data.activeIndex == 'supplier') {
                me.data.activeIndex = 'competitor';
            }
            activateTab(me.data.activeIndex);
            initFilterLabels();

            setMapFilter(me.data.activeMapFilter);
            if (me.data.consumerExpenditure.currentSelection != null) {
                loadConsumerExpenditureSelection(me.data.consumerExpenditure.currentSelection.Id);
                setHeatmap(me.data.consumerExpenditure.currentSelection.Id);
            }
            else {
                loadConsumerExpenditureVariables(me.data.consumerExpenditure.rootId);
            }

            if (me.data.restoredSession) {
                me.sessionLoadedBox.flash();
            }
        };


        //////event actions//////////////////

        var hashChanged = function (e) {
            var p = $.extend(true, { placeId: opts.CurrentInfo.CurrentPlace.Id, industryId: opts.CurrentInfo.CurrentIndustry.Id }, e.getState());
            localStorage.setItem("cv-" + opts.CurrentInfo.CurrentPlace.Id + "-" + opts.CurrentInfo.CurrentIndustry.Id, JSON.stringify(p));

            sizeup.core.profile.setCompetitionValues(p);
        };

        var consumerExpenditureTypeChanged = function (e) {
            var target = $(e.target);
            var rootId = target.attr('data-value');

            new sizeup.core.analytics().consumerExpenditureTypeChanged({ label: rootId });

            if (me.data.consumerExpenditure.rootId != rootId) {
                me.container.find('.map .ceType').removeClass('active');
                target.addClass('active');

                me.data.consumerExpenditure.rootId = rootId;
                sizeup.api.data.getConsumerExpenditureVariableCrosswalk({ id: me.data.consumerExpenditure.currentSelection.Id }, function (data) {
                    me.data.consumerExpenditure.currentSelection = data;
                    loadConsumerExpenditureSelection(me.data.consumerExpenditure.currentSelection.Id);
                    setHeatmap(me.data.consumerExpenditure.currentSelection.Id);
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

        var textAlternativeClicked = function () {
            var url = '/accessibility/competition/';
            window.open(jQuery.param.querystring(url, me.data.textAlternative), '_blank');
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
            me.textAlternative.hide();
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
            var callback = function () {
                setHeatmap(id);
                pushUrlState();
            };
            loadConsumerExpenditureSelection(id, callback);
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

            me.data.consumerExpenditure.currentSelection = getConsumerExpenditureFromList(id);
            me.content.ConsumerExpenditure.selectionList.append(item);
            setHeatmap(id);
            loadConsumerExpenditureVariables(id);
            pushUrlState();
            e.stopPropagation();
        };

        var getConsumerExpenditureFromList = function (id) {
            for (var x in me.data.consumerExpenditure.list) {
                if (me.data.consumerExpenditure.list[x].Id == id) {
                    return me.data.consumerExpenditure.list[x];
                }
            }
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
                    activateTab(me.data.activeIndex);

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
                ids = ids.concat(competitors);
            }
            if (suppliers.length > 0) {
                ids = ids.concat(suppliers);
            }
            if (buyers.length > 0) {
                ids = ids.concat(buyers);
            }

            sizeup.api.data.getBusinessAt({ lat: latLng.lat, lng: latLng.lng, industryIds: ids }, function (data) { createPin(data); });
        };




        var changeIndustryClicked = function () {
            me.content.changeIndustry.hide();
            me.content.industryBox.show();
            me.content.industryBox.focus();
        };

        var onPrimaryIndustryChange = function (i) {
            if (i.Id != me.data.competitor.primaryIndustry.Id) {
                me.content.changeIndustry.html(i.Name);
                var p = { industry: me.data.competitor.primaryIndustry.Name };
                new sizeup.core.analytics().competitionIndustryChanged(p);
                var params = getParameters();
                var url = document.location.pathname;
                url = url.replace(me.data.competitor.primaryIndustry.SEOKey, i.SEOKey);
                url = jQuery.param.querystring(url, jQuery.param.querystring(), 2);
                url = jQuery.param.fragment(url, params, 2);
                document.location = url;
            }
            else {
                me.content.changeIndustry.show();
                me.content.industryBox.hide();
            }
        };

        var industryBoxBlur = function () {
            me.content.changeIndustry.show();
            me.content.industryBox.hide();
        };


        var changePlaceClicked = function () {
            me.content.changePlace.hide();
            me.content.placeBox.show();
            me.content.placeBox.focus();
        };

        var onPlaceChange = function (i) {
            if (i.Id != me.data.activePlace.Id) {
                me.content.changePlace.html(i.City.Name + ', ' + i.State.Abbreviation);
                var p = { place: me.data.activePlace.City.Name + ', ' + me.data.activePlace.State.Abbreviation };
                new sizeup.core.analytics().competitionPlaceChanged(p);
                var params = getParameters();
                var url = document.location.href;
                url = url.substring(0, url.indexOf('competition'));
                url = url + 'competition/' + i.State.SEOKey + '/' + i.County.SEOKey + '/' + i.City.SEOKey + '/' + me.data.competitor.primaryIndustry.SEOKey;
                url = jQuery.param.querystring(url, jQuery.param.querystring(), 2);
                url = jQuery.param.fragment(url, params, 2);
                document.location = url;
            }
            else {
                me.content.changePlace.show();
                me.content.placeBox.hide();
            }
        };

        var placeBoxBlur = function () {
            me.content.changePlace.show();
            me.content.placeBox.hide();
        };

        var updateFilterLabel = function (attribute) {

            var v = me.content.filters.sliders[attribute][me.data.activeIndex] ? me.content.filters.sliders[attribute][me.data.activeIndex].getValue() : null;
            me.content.filters.labels[attribute].setValues(v);
            if (v == null && !me.content.filters.labels[attribute].getContainer().hasClass('fixed')) {
                me.content.filters.labels[attribute].hide();
            }
            else {
                me.content.filters.labels[attribute].show();
            }
        };

        var sliderChanged = function (attribute) {
            var ids = getIndustryIdArray(me.data.activeIndex);
            if (!ids || ids.length === 0) return;
            abortLoadBusinesses();
            me.content.loader.show();
            me.content.businessList.hide();
            me.content.businessListFootnote.hide();
            me.content.noResults.hide();
            me.content.pager.getContainer().hide();
            me.content.addIndustries.hide();
            updateFilterLabel(attribute);
            //var p = { attribute: attribute };
            me.data.filters[attribute][me.data.activeIndex] = me.content.filters.sliders[attribute][me.data.activeIndex] ? me.content.filters.sliders[attribute][me.data.activeIndex].getValue() : null;
            me.data.businessListXHR = sizeup.api.data.getBusinessesByIndustry({
                industryIds: ids,
                geographicLocationId: me.opts.CurrentInfo.CurrentPlace.Id,
                itemCount: me.data[me.data.activeIndex].pageData.itemsPerPage,
                page: me.data[me.data.activeIndex].pageData.page,
                employeesMin: me.data.filters.employees[me.data.activeIndex] ? me.data.filters.employees[me.data.activeIndex].min : null,
                employeesMax: me.data.filters.employees[me.data.activeIndex] ? me.data.filters.employees[me.data.activeIndex].max : null
            }, function (data) {
                me.data[me.data.activeIndex].businesses = data;
                me.data.businessListXHR = null;
                bindBusinesses();
                bindBusinessMarkers();
                me.content.loader.hide();
                me.content.businessList.show();
                me.content.businessListFootnote.show();
                checkMapFilter();
                bindIndustryList();
                loadBusinesses();
                setBusinessOverlay();
                pushUrlState();

            });


        };
        //////////end event actions/////////////////////////////        
        var initFilterLabels = function () {
            me.content.filters.labels['employees'] = new sizeup.controls.rangeLabel({
                container: me.container.find('#employeesLabel')
            });
        }

        var initSliders = function () {

            $.each(me.content.filters.sliders, function (i, e) {
                var sliderName = e[0].className;

                if (!me.content.filters.sliders[sliderName][me.data.activeIndex]) {
                    me.content.filters.sliders[sliderName][me.data.activeIndex] = new sizeup.controls.rangeSlider({
                        container: me.container.find('.' + sliderName),
                        label: me.container.find('.filters .' + sliderName + ' .valueLabel'),
                        range: [
                        { value: 1, label: '1' },
                        { value: 5, label: '5' },
                        { value: 10, label: '10' },
                        { value: 15, label: '15' },
                        { value: 20, label: '20' },
                        { value: 25, label: '25' },
                        { value: 30, label: '30' },
                        { value: 40, label: '40' },
                        { value: 50, label: '50' },
                        { value: 60, label: '60' },
                        { value: 70, label: '70' },
                        { value: 80, label: '80' },
                        { value: 90, label: '90' },
                        { value: 100, label: '100' }
                        ],
                        mode: 'range',
                        value: me.data.filters[sliderName][me.data.activeIndex],
                        onChange: function () { sliderChanged(sliderName) }
                    });
                } else {
                    if (me.data.filters[sliderName][me.data.activeIndex])
                        me.content.filters.sliders[sliderName][me.data.activeIndex].setParam([me.data.filters[sliderName][me.data.activeIndex].min], [me.data.filters[sliderName][me.data.activeIndex].max]);
                    else {
                        me.content.filters.sliders[sliderName][me.data.activeIndex].setParam(null, null);
                    }
                    sliderChanged(sliderName)
                }

            });
        };

        var getParameters = function () {
            var params = jQuery.bbq.getState();
            return params;
        };

        var showLegend = function () {
            if (me.data.consumerExpenditure.legend != null) {
                me.content.map.setLegend(me.data.consumerExpenditure.legend);
            }
        };

        var hideLegned = function () {
            me.content.map.clearLegend();
        };

        var setLegend = function (legend) {
            me.data.consumerExpenditure.legend = legend;
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
                me.content.mapControls.filterMessage.hide();
            }
            else {
                me.content.mapControls.filter.hide();
                me.content.mapControls.filterMessage.show();
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
            initSliders();
            checkMapFilter();
            bindIndustryList();
            loadBusinesses();
            setBusinessOverlay();
            pushUrlState();
            var zoom = me.content.map.getZoom();
            if (zoom >= me.opts.mapFilterZoomThreshold)
                tabIndex = 'all';
            setMapFilter(tabIndex);
        };

        var loadConsumerExpenditureSelection = function (id, callback) {

            sizeup.api.data.getConsumerExpenditureVariablePath({ id: id }, function (data) {
                var html = '';
                //removes the root node as we arent using that
                me.data.consumerExpenditure.currentSelection = data[data.length - 1];
                data.shift();
                for (var x in data) {
                    html = html + templates.bind(templates.get('consumerExpenditureListItem'), data[x]);
                }
                me.content.ConsumerExpenditure.selectionList.html(html);
                if (callback) {
                    callback();
                }
            });
            loadConsumerExpenditureVariables(id);
        };

        var loadConsumerExpenditureVariables = function (parentId) {
            me.content.ConsumerExpenditure.childList.empty();
            me.content.ConsumerExpenditure.loading.show();
            sizeup.api.data.getConsumerExpenditureVariables({ parentId: parentId }, function (data) {
                me.data.consumerExpenditure.list = data;
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



            var createOverlay = function () {
                if (id != null) {
                    var ceType = me.data.consumerExpenditure.rootId == 1 ? '(Total Households)' : '(Average Household)';
                    var title = 'Consumer Spending ' + ceType + ' for ' + me.data.consumerExpenditure.currentSelection.Description;
                    me.data.consumerExpenditure.overlay = new sizeup.maps.heatMapOverlays({
                        attribute: sizeup.api.tiles.overlayAttributes.heatmap.consumerExpenditures,
                        place: me.opts.CurrentInfo.CurrentPlace,
                        opacity: 0.7,
                        params: { variableId: me.data.consumerExpenditure.currentSelection.Id },
                        zoomExtent: me.data.zoomExtent,
                        attributeLabel: title,
                        format: function (val) { return '$' + sizeup.util.numbers.format.abbreviate(val); },
                        legendData: sizeup.api.data.getConsumerExpenditureBands,
                        templates: templates
                    });

                    me.data.consumerExpenditure.activeOverlays = me.data.consumerExpenditure.overlay.getOverlays();


                    for (var x in me.data.consumerExpenditure.activeOverlays) {
                        me.content.map.addOverlay(me.data.consumerExpenditure.activeOverlays[x], 0);
                    }
                    showLegend();
                    getLegendData();
                }
                else {
                    me.data.consumerExpenditure.legend = null;
                    hideLegned();
                }

            };


            if (me.data.zoomExtend == null) {
                sizeup.api.data.getZoomExtent({ placeId: me.opts.CurrentInfo.CurrentPlace.Id, width: me.content.map.getWidth() }, function (data) {
                    me.data.zoomExtent = data;
                    createOverlay();
                });
            }
            else {
                createOverlay();
            }
        };

        var getLegendData = function () {
            if (me.data.consumerExpenditure.currentSelection != null && me.data.consumerExpenditure.overlay != null) {
                var z = me.content.map.getZoom();
                me.data.consumerExpenditure.legendZoomLevel = z;
                var callback = function (legend) {
                    setLegend(legend);
                };

                me.data.consumerExpenditure.overlay.getLegend(z, callback);
                me.data.textAlternative = me.data.consumerExpenditure.overlay.getParams(z);
                me.textAlternative.show();
            } else {
                me.textAlternative.hide();
            }
        };


        var setBusinessOverlay = function () {
            me.content.map.removeOverlay(me.data.competitorOverlay);
            me.content.map.removeOverlay(me.data.supplierOverlay);
            me.content.map.removeOverlay(me.data.buyerOverlay);

            var competitorIndustryIds = getIndustryIdArray('competitor');
            var supplierIndustryIds = getIndustryIdArray('supplier');
            var buyerIndustryIds = getIndustryIdArray('buyer');
            var filterValue = me.data.activeMapFilter;

            if (supplierIndustryIds.length == 0 || !(filterValue == 'all' || filterValue == 'supplier')) {
                supplierIndustryIds = [];
            }
            if (buyerIndustryIds.length == 0 || !(filterValue == 'all' || filterValue == 'buyer')) {
                buyerIndustryIds = [];
            }
            if (!(filterValue == 'all' || filterValue == 'competitor')) {
                competitorIndustryIds = [];
            }

            me.data.competitorOverlay = new sizeup.maps.overlay({
                attribute: sizeup.api.tiles.overlayAttributes.businesses,
                tileParams: {
                    industryIds: competitorIndustryIds,
                    color: '#ff5522',
                    employeesMin: me.data.filters.employees['competitor'] ? me.data.filters.employees['competitor'].min : null,
                    employeesMax: me.data.filters.employees['competitor'] ? me.data.filters.employees['competitor'].max : null
                }
            });

            me.data.supplierOverlay = new sizeup.maps.overlay({
                attribute: sizeup.api.tiles.overlayAttributes.businesses,
                tileParams: {
                    industryIds: supplierIndustryIds,
                    color: '#11AAFF',
                    employeesMin: me.data.filters.employees['supplier'] ? me.data.filters.employees['supplier'].min : null,
                    employeesMax: me.data.filters.employees['supplier'] ? me.data.filters.employees['supplier'].max : null
                }
            });

            me.data.buyerOverlay = new sizeup.maps.overlay({
                attribute: sizeup.api.tiles.overlayAttributes.businesses,
                tileParams: {
                    industryIds: buyerIndustryIds,
                    color: '#66EE00',
                    employeesMin: me.data.filters.employees['buyer'] ? me.data.filters.employees['buyer'].min : null,
                    employeesMax: me.data.filters.employees['buyer'] ? me.data.filters.employees['buyer'].max : null
                }
            });

            me.content.map.addOverlay(me.data.competitorOverlay, 1);
            me.content.map.addOverlay(me.data.supplierOverlay, 1);
            me.content.map.addOverlay(me.data.buyerOverlay, 1);
        };



        var pushUrlState = function () {

            var data = {};
            var competitors = getIndustryIdArray('competitor');
            competitors.splice($.inArray(me.data.competitor.primaryIndustry.Id, competitors), 1);
            var suppliers = getIndustryIdArray('supplier');
            var buyers = getIndustryIdArray('buyer');
            if (competitors.length > 0) {
                data.competitor = competitors;
                if (me.data.filters.employees.competitor) {
                    data.competitorsEmployeesMin = me.data.filters.employees.competitor.min;
                    data.competitorsEmployeesMax = me.data.filters.employees.competitor.max;
                }
            }
            if (suppliers.length > 0) {
                data.supplier = suppliers;
                if (me.data.filters.employees.supplier) {
                    data.suppliersEmployeesMin = me.data.filters.employees.supplier.min;
                    data.suppliersEmployeesMax = me.data.filters.employees.supplier.max;
                }
                    
            }
            if (buyers.length > 0) {
                data.buyer = buyers;
                if (me.data.filters.employees.buyer) {
                    data.buyersEmployeesMin = me.data.filters.employees.buyer.min;
                    data.buyersEmployeesMax = me.data.filters.employees.buyer.max;
                }
            }
            if (me.data.consumerExpenditure.currentSelection != null) {
                data.consumerExpenditureVariable = me.data.consumerExpenditure.currentSelection.Id;
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
                me.content.businessListFootnote.hide();
                me.content.noResults.hide();
                me.content.pager.getContainer().hide();
                me.content.addIndustries.hide();

                if (me.data.businessListXHR != null) {
                    me.data.businessListXHR.abort();
                }
                me.data.businessListXHR = sizeup.api.data.getBusinessesByIndustry({
                    industryIds: ids,
                    geographicLocationId: me.opts.CurrentInfo.CurrentPlace.Id,
                    itemCount: me.data[me.data.activeIndex].pageData.itemsPerPage,
                    page: me.data[me.data.activeIndex].pageData.page,
                    employeesMin: me.data.filters.employees[me.data.activeIndex] ? me.data.filters.employees[me.data.activeIndex].min : null,
                    employeesMax: me.data.filters.employees[me.data.activeIndex] ? me.data.filters.employees[me.data.activeIndex].max : null
                }, function (data) {
                    me.data[me.data.activeIndex].businesses = data;
                    me.data.businessListXHR = null;
                    bindBusinesses();
                    bindBusinessMarkers();
                    me.content.loader.hide();
                    me.content.businessList.show();
                    me.content.businessListFootnote.show();
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
                me.content.businessListFootnote.show();
                if (!me.opts.IsAuthenticated && !me.opts.IsCustomTools) {
                    me.content.signinPanel.container.show();
                    me.content.signinPanel.toggle.html(templates.bind(me.content.signinPanel.templateText, me.content.pager.getPageData()));
                }
            }

            if (data.Count > me.opts.itemsPerPage && (me.opts.IsAuthenticated || me.opts.IsCustomTools)) {
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
            me.content.businessListFootnote.hide();
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
            if (me.content.map.getZoom() > me.opts.maxAutoZoom) {
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



