(function () {
    sizeup.core.namespace('sizeup.widget.views.bestIndustries');
    sizeup.widget.views.bestIndustries.bestIndustries = function (opts) {

        var me = {};
       
        me.opts = opts;
        me.data = {};

        me.container = $('#bestIndustries');
        var templates = new sizeup.core.templates(me.container);

        var init = function () {
            
            me.content = {};
            me.loader = me.container.find('.loading');

            me.content.container = me.container.find('.wrapper.bestIndustries').hide().removeClass('hidden');
            me.content.noResults = me.container.find('.wrapper.noResults').removeClass('hidden').hide();

            loadData();
        };



        var loadData = function () {
            var notifier = new sizeup.core.notifier(function () { bindData(); });
            me.data.bestIndustries = {};
            sizeup.api.data.getBestIndustries({ geographicLocationId: opts.CurrentInfo.CurrentPlace.City.Id, attribute: sizeup.api.attributes.TOTAL_REVENUE }, notifier.getNotifier(function (data) { me.data.bestIndustries.totalRevenue = data; }));
            sizeup.api.data.getBestIndustries({ geographicLocationId: opts.CurrentInfo.CurrentPlace.City.Id, attribute: sizeup.api.attributes.REVENUE_PER_CAPITA }, notifier.getNotifier(function (data) { me.data.bestIndustries.revenuePerCapita = data; }));
        };

        bindData = function () {
            var data = formatBestIndustries();
            me.content.bestIndustries = {};

            me.content.bestIndustries.wrapper = me.container.find('.wrapper.bestIndustries .report');
            me.content.bestIndustries.wrapper.html(templates.bind(templates.get('bestIndustries'), data));

            if (data.Industries.length > 0) {
                me.content.container.show();
            }
            else {
                me.content.noResults.show();
            }

            me.loader.remove();
        };


        var formatBestIndustries = function () {
            var temp = {};
            for (var x in me.data.bestIndustries) {
                for (var y in me.data.bestIndustries[x]) {

                    if (!temp[me.data.bestIndustries[x][y].Industry.Id]) {
                        temp[me.data.bestIndustries[x][y].Industry.Id] = {
                            Industry: me.data.bestIndustries[x][y].Industry
                        };
                    }
                    temp[me.data.bestIndustries[x][y].Industry.Id][x] = { rank: sizeup.util.numbers.format.ordinal(me.data.bestIndustries[x][y].Rank), badgeType: '' };
                    if (me.data.bestIndustries[x][y].Rank >= 1 && me.data.bestIndustries[x][y].Rank <= 10) {
                        temp[me.data.bestIndustries[x][y].Industry.Id][x].badgeType = 'top10';
                        temp[me.data.bestIndustries[x][y].Industry.Id][x].badgeName = 'Top 10';
                    }
                    else if (me.data.bestIndustries[x][y].Rank >= 11 && me.data.bestIndustries[x][y].Rank <= 50) {
                        temp[me.data.bestIndustries[x][y].Industry.Id][x].badgeType = 'top50';
                        temp[me.data.bestIndustries[x][y].Industry.Id][x].badgeName = 'Top 50';
                    }
                    else if (me.data.bestIndustries[x][y].Rank >= 51 && me.data.bestIndustries[x][y].Rank <= 100) {
                        temp[me.data.bestIndustries[x][y].Industry.Id][x].badgeType = 'top100';
                        temp[me.data.bestIndustries[x][y].Industry.Id][x].badgeName = 'Top 100';
                    }
                }
            }
            var data = [];
            for (var x in temp) {
                data.push(temp[x]);
            }
            data.sort(sort);
            return { Industries: data };
        };

        var sort = function (a, b) {
            if (a.Industry.Name < b.Industry.Name)
                return -1;
            if (a.Industry.Name > b.Industry.Name)
                return 1;
            return 0;
        };

        var publicObj = {

        };
        init();
        return publicObj;

    };
})();