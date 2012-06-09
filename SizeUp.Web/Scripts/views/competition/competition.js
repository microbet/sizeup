(function () {
    sizeup.core.namespace('sizeup.views.competition');
    sizeup.views.competition.competition = function (opts) {


        var defaults = {
            itemsPerPage: 10,
            slideTime: 500
        };
        var me = {};
        
        me.opts = $.extend(true, defaults, opts);

        me.data = {};
        me.container = $('#competition');
        var dataLayer = new sizeup.core.data();
        var templates = new sizeup.core.templates(me.container);

        var notifier = new sizeup.core.notifier(function () { init(); });
        dataLayer.isAuthenticated(notifier.getNotifier(function (data) { me.isAuthenticated = data; }));

        me.activePickerIndex = null;
        me.activeContentIndex = null;


        me.competitor = {};
        me.buyer = {};
        me.supplier = {};

        me.data.competitor =
        {
            industries: [1245],
            pickerIndustries: [],
            primaryIndustry: opts.CurrentIndustry.Id,
            businesses: {
                items: {},
                isStale: true
            }
        };
        me.data.buyer =
        {
            industries: [],
            pickerIndustries: [],
            businesses: {
                items: {},
                isStale: true
            }
        };
        me.data.supplier = 
        {
            industries: [],
            pickerIndustries: [],
            businesses: {
                items: {},
                isStale: true
            }
        };

      

        var init = function () {
            initGeneral('competitor');
            initGeneral('supplier');
            initGeneral('buyer');

            showTab('competitor');
            activateTab('competitor');
            showContent('competitor');
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
            me[index].picker.textbox = me[index].picker.container.find('.pickerInput');
            me[index].picker.cancel = me[index].picker.container.find('.cancel');
            me[index].picker.cancel.click(function () { cancelClicked(index); });

            me[index].picker.submit = me[index].picker.container.find('.submit');
            me[index].picker.submit.click(function () { submitClicked(index); });


            me[index].picker.selector = new sizeup.controls.industrySelector({
                textbox: me[index].picker.textbox,
                onChange: function (item) { industryPicked(item); }
            });


            me[index].content.map = new sizeup.maps.businessMap({
                container: me[index].content.container.find('.map')
            });
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
        };

        var slideInPicker = function (index, callback) {
            me[index].picker.container.show(
               "slide",
               { direction: "right" },
               me.opts.slideTime,
           callback);
            me.activePickerIndex = index;
        };

        var slideOutPicker = function (index, callback) {
            me[index].picker.container.hide(
              "slide",
              { direction: "right" },
              me.opts.slideTime,
          callback);
            me.activePickerIndex = null;
        };


        var setupPicker = function (index) {

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
            //do setting of ids to proper place
            //test to see if data isd now stale
            me.data[index].businesses.IsStale = true;
            slideOutPicker(index, function () {
                slideInContent(index);
            });
            showTab(index);
            activateTab(index);
            hideQuestion(index);
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

        var industryPicked = function (index, item) {

        };

        var industryRemoved = function(index, item){

        };

        var pagerOnUpdate = function (index, data) {
            loadBusinesses(index);
        };

        var getIndustryIds = function (index) {
            var ar = new Array();
            ar = ar.concat(me.data[index].industries);
            if (me.data[index].primaryIndustry) {
                ar.push(me.data[index].primaryIndustry);
            }
            return ar;
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
            me.data[index].businesses.items = data.Items;
            var html = '';
            for (var x = 0; x < data.Items.length;x++) {
                var template = templates.get(index + 'BusinessItem');
                html = html + templates.bind(template, { index: x + 1, business: data.Items[x] });

                createMarker(index, data.Items[x], x + 1);
            };
            me[index].content.businessList.html(html);
            me[index].content.businessList.show();
        };

        var createMarker = function (index, business, label) {
            var marker = new sizeup.maps.imageMarker({
                position: new sizeup.maps.latLng({ lat: business.Lat, lng: business.Lng }),
                section: index,
                index: label
            });
            me[index].content.map.addMarker(marker);
        };


        var publicObj = {

        };
        return publicObj;
        
    };
})();