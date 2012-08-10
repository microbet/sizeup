(function () {
    sizeup.core.namespace('sizeup.views.community');
    sizeup.views.community.communityWithIndustry = function (opts) {


        var defaults = {
            mapSettings: sizeup.maps.mapOptions.getDefaults(),
            styles: sizeup.maps.mapStyles.getDefaults()
        };
        var me = {};

        me.opts = $.extend(true, defaults, opts);

        me.data = {};
        me.container = $('#community');
        var dataLayer = new sizeup.core.data();
        var templates = new sizeup.core.templates(me.container);
        var notifier = new sizeup.core.notifier(function () { init(); });


        me.content = {};


        dataLayer.getCityCentroid({ id: opts.CurrentPlace.Id }, notifier.getNotifier(function (data) { me.data.CityCenter = new sizeup.maps.latLng({ lat: data.Lat, lng: data.Lng }); }));

        var init = function () {


            me.content.map = new sizeup.maps.heatMap({
                legendItemTemplate: templates.get('legendItem'),
                container: me.container.find('.map').removeClass('hidden').show()
            });
            me.content.map.setCenter(me.data.CityCenter);
            me.content.map.hideLegend();

            dataLayer.getAverageRevenueChart({ industryId: me.opts.CurrentIndustry.Id, placeId: me.opts.CurrentPlace.Id }, initAverageRevenueChart);
            dataLayer.getTotalRevenueChart({ industryId: me.opts.CurrentIndustry.Id, placeId: me.opts.CurrentPlace.Id }, initTotalRevenueChart);

            dataLayer.getAverageEmployeesChart({ industryId: me.opts.CurrentIndustry.Id, placeId: me.opts.CurrentPlace.Id }, initAverageEmployeesChart);
            dataLayer.getTotalEmployeesChart({ industryId: me.opts.CurrentIndustry.Id, placeId: me.opts.CurrentPlace.Id }, initTotalEmployeesChart);

            dataLayer.getAverageSalaryChart({ industryId: me.opts.CurrentIndustry.Id, placeId: me.opts.CurrentPlace.Id }, initAverageSalaryChart);
            dataLayer.getCostEffectivenessChart({ industryId: me.opts.CurrentIndustry.Id, placeId: me.opts.CurrentPlace.Id }, initCostEffectivenessChart);

            
        };

        var clearOverlays = function () {
            me.content.map.clearOverlays();
        };

        var setOverlays = function (overlays) {
            me.content.map.setOverlays(overlays);
        };

        var getAverageRevenueOverlays = function () {
            var overlays = [
                 {
                     tileUrl: "/tiles/AverageRevenue/state/",
                     legendSource: function (callback) {
                         dataLayer.getAverageRevenueBandsByState({
                             industryId: me.opts.CurrentIndustry.Id,
                             bands: 7
                         }, callback);
                     },
                     legendTitle: 'Average Business Annual Revenue by state in the USA',
                     legendFormat: function (val) { return '$' + sizeup.util.numbers.format.abbreviate(val); },
                     industryId: me.opts.CurrentIndustry.Id,
                     minZoom: 0,
                     maxZoom: 4,
                     colors: [
                         '#F5F500',
                         '#F5CC00',
                         '#F5A300',
                         '#F57A00',
                         '#F55200',
                         '#F52900',
                         '#F50000'
                     ]
                 },
                 {
                     tileUrl: "/tiles/AverageRevenue/county/",
                     legendSource: function (callback) {
                         dataLayer.getAverageRevenueBandsByCounty({
                             industryId: me.opts.CurrentIndustry.Id,
                             bands: 7,
                             boundingEntityId: 's' + me.opts.CurrentPlace.State.Id
                         }, callback);
                     },
                     legendTitle: 'Average Business Annual Revenue by county in ' + me.opts.CurrentPlace.State.Name,
                     legendFormat: function (val) { return '$' + sizeup.util.numbers.format.abbreviate(val); },
                     industryId: me.opts.CurrentIndustry.Id,
                     minZoom: 5,
                     maxZoom: 8,
                     boundingEntityId: 's' + me.opts.CurrentPlace.State.Id,
                     colors: [
                         '#F5F500',
                         '#F5CC00',
                         '#F5A300',
                         '#F57A00',
                         '#F55200',
                         '#F52900',
                         '#F50000'
                     ]
                 },
                {
                    tileUrl: "/tiles/AverageRevenue/county/",
                    legendSource: function (callback) {
                        dataLayer.getAverageRevenueBandsByCounty({
                            industryId: me.opts.CurrentIndustry.Id,
                            bands: 7,
                            boundingEntityId: me.opts.CurrentPlace.Metro ? 'm' + me.opts.CurrentPlace.Metro.Id : 's' + me.opts.CurrentPlace.State.Id
                        }, callback);
                    },
                    legendTitle: 'Average Business Annual Revenue by county in ' + (me.opts.CurrentPlace.Metro ? me.opts.CurrentPlace.Metro.Name + ' (Metro)' : me.opts.CurrentPlace.State.Name),
                    legendFormat: function (val) { return '$' + sizeup.util.numbers.format.abbreviate(val); },
                    industryId: me.opts.CurrentIndustry.Id,
                    minZoom: 9,
                    maxZoom: 11,
                    boundingEntityId: me.opts.CurrentPlace.Metro ? 'm' + me.opts.CurrentPlace.Metro.Id : 's' + me.opts.CurrentPlace.State.Id,
                    colors: [
                        '#F5F500',
                        '#F5CC00',
                        '#F5A300',
                        '#F57A00',
                        '#F55200',
                        '#F52900',
                        '#F50000'
                    ]
                },
                {
                    tileUrl: "/tiles/AverageRevenue/zip/",
                    legendSource: function (callback) {
                        dataLayer.getAverageRevenueBandsByZip({
                            industryId: me.opts.CurrentIndustry.Id,
                            bands: 7,
                            boundingEntityId: 'co' + me.opts.CurrentPlace.County.Id
                        }, callback);
                    },
                    legendTitle: 'Average Business Annual Revenue by ZIP code in ' + me.opts.CurrentPlace.County.Name + ', ' + me.opts.CurrentPlace.State.Abbreviation,
                    legendFormat: function (val) { return '$' + sizeup.util.numbers.format.abbreviate(val); },
                    industryId: me.opts.CurrentIndustry.Id,
                    minZoom: 12,
                    maxZoom: 32,
                    boundingEntityId: 'co' + me.opts.CurrentPlace.County.Id,
                    colors: [
                        '#F5F500',
                        '#F5CC00',
                        '#F5A300',
                        '#F57A00',
                        '#F55200',
                        '#F52900',
                        '#F50000'
                    ]
                }
            ];
            return overlays;
        };

        var getTotalRevenueOverlays = function () {
            var overlays = [
                  {
                      tileUrl: "/tiles/totalRevenue/state/",
                      legendSource: function (callback) {
                          dataLayer.getTotalRevenueBandsByState({
                              industryId: me.optsCurrentIndustry.Id,
                              bands: 7
                          }, callback);
                      },
                      legendTitle: 'Total Revenue by state in the USA',
                      legendFormat: function (val) { return '$' + sizeup.util.numbers.format.abbreviate(val); },
                      industryId: me.opts.CurrentIndustry.Id,
                      minZoom: 0,
                      maxZoom: 4,
                      colors: [
                          '#F5F500',
                          '#F5CC00',
                          '#F5A300',
                          '#F57A00',
                          '#F55200',
                          '#F52900',
                          '#F50000'
                      ]
                  },
                        {
                            tileUrl: "/tiles/totalRevenue/county/",
                            legendSource: function (callback) {
                                dataLayer.getTotalRevenueBandsByCounty({
                                    industryId: me.opts.CurrentIndustry.Id,
                                    bands: 7,
                                    boundingEntityId: 's' + me.opts.CurrentPlace.State.Id
                                }, callback);
                            },
                            legendTitle: 'Total Revenue by county in ' + me.opts.CurrentPlace.State.Name,
                            legendFormat: function (val) { return '$' + sizeup.util.numbers.format.abbreviate(val); },
                            industryId: me.opts.CurrentIndustry.Id,
                            minZoom: 5,
                            maxZoom: 8,
                            boundingEntityId: 's' + me.opts.CurrentPlace.State.Id,
                            colors: [
                                '#F5F500',
                                '#F5CC00',
                                '#F5A300',
                                '#F57A00',
                                '#F55200',
                                '#F52900',
                                '#F50000'
                            ]
                        },
                        {
                            tileUrl: "/tiles/totalRevenue/county/",
                            legendSource: function (callback) {
                                dataLayer.getTotalRevenueBandsByCounty({
                                    industryId: me.opts.CurrentIndustry.Id,
                                    bands: 7,
                                    boundingEntityId: me.opts.CurrentPlace.Metro ? 'm' + me.opts.CurrentPlace.Metro.Id : 's' + me.opts.CurrentPlace.State.Id
                                }, callback);
                            },
                            legendTitle: 'Total Revenue by county in ' + (me.opts.CurrentPlace.Metro ? me.opts.CurrentPlace.Metro.Name + ' (Metro)' : me.opts.CurrentPlace.State.Name),
                            legendFormat: function (val) { return '$' + sizeup.util.numbers.format.abbreviate(val); },
                            industryId: me.opts.CurrentIndustry.Id,
                            minZoom: 9,
                            maxZoom: 11,
                            boundingEntityId: me.opts.CurrentPlace.Metro ? 'm' + me.opts.CurrentPlace.Metro.Id : 's' + me.opts.CurrentPlace.State.Id,
                            colors: [
                                '#F5F500',
                                '#F5CC00',
                                '#F5A300',
                                '#F57A00',
                                '#F55200',
                                '#F52900',
                                '#F50000'
                            ]
                        },
                        {
                            tileUrl: "/tiles/totalRevenue/zip/",
                            legendSource: function (callback) {
                                dataLayer.getTotalRevenueBandsByZip({
                                    industryId: me.opts.CurrentIndustry.Id,
                                    bands: 7,
                                    boundingEntityId: 'co' + me.opts.CurrentPlace.County.Id
                                }, callback);
                            },
                            legendTitle: 'Total Revenue by ZIP code in ' + me.opts.CurrentPlace.County.Name + ', ' + me.opts.CurrentPlace.State.Abbreviation,
                            legendFormat: function (val) { return '$' + sizeup.util.numbers.format.abbreviate(val); },
                            industryId: me.opts.CurrentIndustry.Id,
                            minZoom: 12,
                            maxZoom: 32,
                            boundingEntityId: 'co' + me.opts.CurrentPlace.County.Id,
                            colors: [
                                '#F5F500',
                                '#F5CC00',
                                '#F5A300',
                                '#F57A00',
                                '#F55200',
                                '#F52900',
                                '#F50000'
                            ]
                        }
            ];
            return overlays;
        };

        var getAverageEmployeesOverlays = function () {
            var overlays = [
                  {
                      tileUrl: "/tiles/AverageEmployees/state/",
                      legendSource: function (callback) {
                          dataLayer.getAverageEmployeesBandsByState({
                              industryId: me.optsCurrentIndustry.Id,
                              bands: 7
                          }, callback);
                      },
                      legendTitle: 'Average Employees per business by state in the USA',
                      legendFormat: function (val) { return sizeup.util.numbers.format.abbreviate(val, 0); },
                      industryId: me.opts.CurrentIndustry.Id,
                      minZoom: 0,
                      maxZoom: 4,
                      colors: [
                          '#F5F500',
                          '#F5CC00',
                          '#F5A300',
                          '#F57A00',
                          '#F55200',
                          '#F52900',
                          '#F50000'
                      ]
                  },
                        {
                            tileUrl: "/tiles/AverageEmployees/county/",
                            legendSource: function (callback) {
                                dataLayer.getAverageEmployeesBandsByCounty({
                                    industryId: me.opts.CurrentIndustry.Id,
                                    bands: 7,
                                    boundingEntityId: 's' + me.opts.CurrentPlace.State.Id
                                }, callback);
                            },
                            legendTitle: 'Average Employees per business by county in ' + me.opts.CurrentPlace.State.Name,
                            legendFormat: function (val) { return sizeup.util.numbers.format.abbreviate(val, 0); },
                            industryId: me.opts.CurrentIndustry.Id,
                            minZoom: 5,
                            maxZoom: 8,
                            boundingEntityId: 's' + me.opts.CurrentPlace.State.Id,
                            colors: [
                                '#F5F500',
                                '#F5CC00',
                                '#F5A300',
                                '#F57A00',
                                '#F55200',
                                '#F52900',
                                '#F50000'
                            ]
                        },
                        {
                            tileUrl: "/tiles/AverageEmployees/county/",
                            legendSource: function (callback) {
                                dataLayer.getAverageEmployeesBandsByCounty({
                                    industryId: me.opts.CurrentIndustry.Id,
                                    bands: 7,
                                    boundingEntityId: me.opts.CurrentPlace.Metro ? 'm' + me.opts.CurrentPlace.Metro.Id : 's' + me.opts.CurrentPlace.State.Id
                                }, callback);
                            },
                            legendTitle: 'Average Employees per business by county in ' + (me.opts.CurrentPlace.Metro ? me.opts.CurrentPlace.Metro.Name + ' (Metro)' : me.opts.CurrentPlace.State.Name),
                            legendFormat: function (val) { return sizeup.util.numbers.format.abbreviate(val, 0); },
                            industryId: me.opts.CurrentIndustry.Id,
                            minZoom: 9,
                            maxZoom: 11,
                            boundingEntityId: me.opts.CurrentPlace.Metro ? 'm' + me.opts.CurrentPlace.Metro.Id : 's' + me.opts.CurrentPlace.State.Id,
                            colors: [
                                '#F5F500',
                                '#F5CC00',
                                '#F5A300',
                                '#F57A00',
                                '#F55200',
                                '#F52900',
                                '#F50000'
                            ]
                        },
                        {
                            tileUrl: "/tiles/AverageEmployees/zip/",
                            legendSource: function (callback) {
                                dataLayer.getAverageEmployeesBandsByZip({
                                    industryId: me.opts.CurrentIndustry.Id,
                                    bands: 7,
                                    boundingEntityId: 'co' + me.opts.CurrentPlace.County.Id
                                }, callback);
                            },
                            legendTitle: 'Average Employees per business by ZIP code in ' + me.opts.CurrentPlace.County.Name + ', ' + me.opts.CurrentPlace.State.Abbreviation,
                            legendFormat: function (val) { return sizeup.util.numbers.format.abbreviate(val, 0); },
                            industryId: me.opts.CurrentIndustry.Id,
                            minZoom: 12,
                            maxZoom: 32,
                            boundingEntityId: 'co' + me.opts.CurrentPlace.County.Id,
                            colors: [
                                '#F5F500',
                                '#F5CC00',
                                '#F5A300',
                                '#F57A00',
                                '#F55200',
                                '#F52900',
                                '#F50000'
                            ]
                        }
            ];
            return overlays;
        };

        var getTotalEmployeesOverlays = function () {
            var overlays = [
                  {
                      tileUrl: "/tiles/TotalEmployees/state/",
                      legendSource: function (callback) {
                          dataLayer.getTotalEmployeesBandsByState({
                              industryId: me.optsCurrentIndustry.Id,
                              bands: 7
                          }, callback);
                      },
                      legendTitle: 'Total Employees by state in the USA',
                      legendFormat: function (val) { return '$' + sizeup.util.numbers.format.abbreviate(val); },
                      industryId: me.opts.CurrentIndustry.Id,
                      minZoom: 0,
                      maxZoom: 4,
                      colors: [
                          '#F5F500',
                          '#F5CC00',
                          '#F5A300',
                          '#F57A00',
                          '#F55200',
                          '#F52900',
                          '#F50000'
                      ]
                  },
                        {
                            tileUrl: "/tiles/TotalEmployees/county/",
                            legendSource: function (callback) {
                                dataLayer.getTotalEmployeesBandsByCounty({
                                    industryId: me.opts.CurrentIndustry.Id,
                                    bands: 7,
                                    boundingEntityId: 's' + me.opts.CurrentPlace.State.Id
                                }, callback);
                            },
                            legendTitle: 'Total Employees by county in ' + me.opts.CurrentPlace.State.Name,
                            legendFormat: function (val) { return '$' + sizeup.util.numbers.format.abbreviate(val); },
                            industryId: me.opts.CurrentIndustry.Id,
                            minZoom: 5,
                            maxZoom: 8,
                            boundingEntityId: 's' + me.opts.CurrentPlace.State.Id,
                            colors: [
                                '#F5F500',
                                '#F5CC00',
                                '#F5A300',
                                '#F57A00',
                                '#F55200',
                                '#F52900',
                                '#F50000'
                            ]
                        },
                        {
                            tileUrl: "/tiles/TotalEmployees/county/",
                            legendSource: function (callback) {
                                dataLayer.getTotalEmployeesBandsByCounty({
                                    industryId: me.opts.CurrentIndustry.Id,
                                    bands: 7,
                                    boundingEntityId: me.opts.CurrentPlace.Metro ? 'm' + me.opts.CurrentPlace.Metro.Id : 's' + me.opts.CurrentPlace.State.Id
                                }, callback);
                            },
                            legendTitle: 'Total Employees by county in ' + (me.opts.CurrentPlace.Metro ? me.opts.CurrentPlace.Metro.Name + ' (Metro)' : me.opts.CurrentPlace.State.Name),
                            legendFormat: function (val) { return '$' + sizeup.util.numbers.format.abbreviate(val); },
                            industryId: me.opts.CurrentIndustry.Id,
                            minZoom: 9,
                            maxZoom: 11,
                            boundingEntityId: me.opts.CurrentPlace.Metro ? 'm' + me.opts.CurrentPlace.Metro.Id : 's' + me.opts.CurrentPlace.State.Id,
                            colors: [
                                '#F5F500',
                                '#F5CC00',
                                '#F5A300',
                                '#F57A00',
                                '#F55200',
                                '#F52900',
                                '#F50000'
                            ]
                        },
                        {
                            tileUrl: "/tiles/TotalEmployees/zip/",
                            legendSource: function (callback) {
                                dataLayer.getTotalEmployeesBandsByZip({
                                    industryId: me.opts.CurrentIndustry.Id,
                                    bands: 7,
                                    boundingEntityId: 'co' + me.opts.CurrentPlace.County.Id
                                }, callback);
                            },
                            legendTitle: 'Total Employees by ZIP code in ' + me.opts.CurrentPlace.County.Name + ', ' + me.opts.CurrentPlace.State.Abbreviation,
                            legendFormat: function (val) { return '$' + sizeup.util.numbers.format.abbreviate(val); },
                            industryId: me.opts.CurrentIndustry.Id,
                            minZoom: 12,
                            maxZoom: 32,
                            boundingEntityId: 'co' + me.opts.CurrentPlace.County.Id,
                            colors: [
                                '#F5F500',
                                '#F5CC00',
                                '#F5A300',
                                '#F57A00',
                                '#F55200',
                                '#F52900',
                                '#F50000'
                            ]
                        }
            ];
            return overlays;
        };

        var getAverageSalaryOverlays = function () {
            var overlays = [
                  {
                      tileUrl: "/tiles/averageSalary/state/",
                      legendSource: function (callback) {
                          dataLayer.getAverageSalaryBandsByState({
                              industryId: me.optsCurrentIndustry.Id,
                              bands: 7
                          }, callback);
                      },
                      legendTitle: 'Average Salary by state in the USA',
                      legendFormat: function (val) { return '$' + sizeup.util.numbers.format.abbreviate(val); },
                      industryId: me.opts.CurrentIndustry.Id,
                      minZoom: 0,
                      maxZoom: 4,
                      colors: [
                          '#F5F500',
                          '#F5CC00',
                          '#F5A300',
                          '#F57A00',
                          '#F55200',
                          '#F52900',
                          '#F50000'
                      ]
                  },
                        {
                            tileUrl: "/tiles/averageSalary/county/",
                            legendSource: function (callback) {
                                dataLayer.getAverageSalaryBandsByCounty({
                                    industryId: me.opts.CurrentIndustry.Id,
                                    bands: 7,
                                    boundingEntityId: 's' + me.opts.CurrentPlace.State.Id
                                }, callback);
                            },
                            legendTitle: 'Average Salary by county in ' + me.opts.CurrentPlace.State.Name,
                            legendFormat: function (val) { return '$' + sizeup.util.numbers.format.abbreviate(val); },
                            industryId: me.opts.CurrentIndustry.Id,
                            minZoom: 5,
                            maxZoom: 8,
                            boundingEntityId: 's' + me.opts.CurrentPlace.State.Id,
                            colors: [
                                '#F5F500',
                                '#F5CC00',
                                '#F5A300',
                                '#F57A00',
                                '#F55200',
                                '#F52900',
                                '#F50000'
                            ]
                        },
                        {
                            tileUrl: "/tiles/averageSalary/county/",
                            legendSource: function (callback) {
                                dataLayer.getAverageSalaryBandsByCounty({
                                    industryId: me.opts.CurrentIndustry.Id,
                                    bands: 7,
                                    boundingEntityId: me.opts.CurrentPlace.Metro ? 'm' + me.opts.CurrentPlace.Metro.Id : 's' + me.opts.CurrentPlace.State.Id
                                }, callback);
                            },
                            legendTitle: 'Average Salary by county in ' + (me.opts.CurrentPlace.Metro ? me.opts.CurrentPlace.Metro.Name + ' (Metro)' : me.opts.CurrentPlace.State.Name),
                            legendFormat: function (val) { return '$' + sizeup.util.numbers.format.abbreviate(val); },
                            industryId: me.opts.CurrentIndustry.Id,
                            minZoom: 9,
                            maxZoom: 32,
                            boundingEntityId: me.opts.CurrentPlace.Metro ? 'm' + me.opts.CurrentPlace.Metro.Id : 's' + me.opts.CurrentPlace.State.Id,
                            colors: [
                                '#F5F500',
                                '#F5CC00',
                                '#F5A300',
                                '#F57A00',
                                '#F55200',
                                '#F52900',
                                '#F50000'
                            ]
                        }
                        
            ];
            return overlays;
        };


        var getCostEffectivenessOverlays = function () {
            var overlays = [
                 {
                     tileUrl: "/tiles/CostEffectiveness/state/",
                     legendSource: function (callback) {
                         dataLayer.getCostEffectivenessBandsByState({
                             industryId: me.optsCurrentIndustry.Id,
                             bands: 7
                         }, callback);
                     },
                     legendTitle: 'Cost Effectiveness by state in the USA',
                     legendFormat: function (val) { return sizeup.util.numbers.format.round(val, 2); },
                     industryId: me.opts.CurrentIndustry.Id,
                     minZoom: 0,
                     maxZoom: 4,
                     colors: [
                         '#F5F500',
                         '#F5CC00',
                         '#F5A300',
                         '#F57A00',
                         '#F55200',
                         '#F52900',
                         '#F50000'
                     ]
                 },
                {
                    tileUrl: "/tiles/CostEffectiveness/county/",
                    legendSource: function (callback) {
                        dataLayer.getCostEffectivenessBandsByCounty({
                            industryId: me.opts.CurrentIndustry.Id,
                            bands: 7,
                            boundingEntityId: 's' + me.opts.CurrentPlace.State.Id
                        }, callback);
                    },
                    legendTitle: 'Cost Effectiveness by county in ' + me.opts.CurrentPlace.State.Name,
                    legendFormat: function (val) { return sizeup.util.numbers.format.round(val, 2); },
                    industryId: me.opts.CurrentIndustry.Id,
                    minZoom: 5,
                    maxZoom: 8,
                    boundingEntityId: 's' + me.opts.CurrentPlace.State.Id,
                    colors: [
                        '#F5F500',
                        '#F5CC00',
                        '#F5A300',
                        '#F57A00',
                        '#F55200',
                        '#F52900',
                        '#F50000'
                    ]
                },
                {
                    tileUrl: "/tiles/CostEffectiveness/county/",
                    legendSource: function (callback) {
                        dataLayer.getCostEffectivenessBandsByCounty({
                            industryId: me.opts.CurrentIndustry.Id,
                            bands: 7,
                            boundingEntityId: me.opts.CurrentPlace.Metro ? 'm' + me.opts.CurrentPlace.Metro.Id : 's' + me.opts.CurrentPlace.State.Id
                        }, callback);
                    },
                    legendTitle: 'Cost Effectiveness by county in ' + (me.opts.CurrentPlace.Metro ? me.opts.CurrentPlace.Metro.Name + ' (Metro)' : me.opts.CurrentPlace.State.Name),
                    legendFormat: function (val) { return sizeup.util.numbers.format.round(val, 2); },
                    industryId: me.opts.CurrentIndustry.Id,
                    minZoom: 9,
                    maxZoom: 32,
                    boundingEntityId: me.opts.CurrentPlace.Metro ? 'm' + me.opts.CurrentPlace.Metro.Id : 's' + me.opts.CurrentPlace.State.Id,
                    colors: [
                        '#F5F500',
                        '#F5CC00',
                        '#F5A300',
                        '#F57A00',
                        '#F55200',
                        '#F52900',
                        '#F50000'
                    ]
                }

            ];
            return overlays;
        };






        var formatChartData = function (data, indexes) {
            var formattedData = {};
            for (var x = 0; x < indexes.length; x++) {
                if (data[indexes[x]] != null) {
                    formattedData[indexes[x]] =
                    {
                        value: data[indexes[x]].Value,
                        label: indexes[x],
                        name: data[indexes[x]].Name,
                        color: '#0af'
                    };
                }
            }
            return formattedData;
        };

        var initAverageRevenueChart = function (data) {
            var container = me.container.find('#averageRevenue');
            container.find('.loading').remove();
            var indexes = ['City', 'County', 'Metro', 'State', 'Nation'];
            var formattedData = formatChartData(data, indexes);

            var chart = new sizeup.charts.barChart({
                valueFormat: function (val) { return '$' + sizeup.util.numbers.format.addCommas(Math.floor(val)); },
                bar: { height: 13, padding: 4 },
                gutters: { left: 50, top: 1 },
                fillRight: false,
                container: container.find('.chart'),
                title: null,
                bars: formattedData
            });
            chart.draw();


            container.find('.buttons .mapActivate').click(function () {
                setOverlays(getAverageRevenueOverlays());
            });

            container.find('.buttons .mapClear').click(function () {
                clearOverlays();
            });
        };

        var initTotalRevenueChart = function (data) {
            var container = me.container.find('#totalRevenue');
            container.find('.loading').remove();
            var indexes = ['City', 'County', 'Metro', 'State'];
            var formattedData = formatChartData(data, indexes);

            var chart = new sizeup.charts.barChart({
                valueFormat: function (val) { return '$' + sizeup.util.numbers.format.addCommas(Math.floor(val)); },
                bar: { height: 13, padding: 4 },
                gutters: { left: 50, top: 1 },
                fillRight: false,
                container: container.find('.chart'),
                title: null,
                bars: formattedData
            });
            chart.draw();


            container.find('.buttons .mapActivate').click(function () {
                setOverlays(getTotalRevenueOverlays());
            });

            container.find('.buttons .mapClear').click(function () {
                clearOverlays();
            });
            
        };

        var initAverageEmployeesChart = function (data) {
            var container = me.container.find('#averageEmployees');
            container.find('.loading').remove();
            var indexes = ['City', 'County', 'Metro', 'State', 'Nation'];
            var formattedData = formatChartData(data, indexes);

            var chart = new sizeup.charts.barChart({
                valueFormat: function (val) { return sizeup.util.numbers.format.addCommas(Math.floor(val)); },
                bar: { height: 13, padding: 4 },
                gutters: { left: 50, top: 1 },
                fillRight: false,
                container: container.find('.chart'),
                title: null,
                bars: formattedData
            });
            chart.draw();

            container.find('.buttons .mapActivate').click(function () {
                setOverlays(getAverageEmployeesOverlays());
            });

            container.find('.buttons .mapClear').click(function () {
                clearOverlays();
            });

        };

        var initTotalEmployeesChart = function (data) {
            var container = me.container.find('#totalEmployees');
            container.find('.loading').remove();
            var indexes = ['City', 'County', 'Metro', 'State'];
            var formattedData = formatChartData(data, indexes);

            var chart = new sizeup.charts.barChart({
                valueFormat: function (val) { return sizeup.util.numbers.format.addCommas(Math.floor(val)); },
                bar: { height: 13, padding: 4 },
                gutters: { left: 50, top: 1 },
                fillRight: false,
                container: container.find('.chart'),
                title: null,
                bars: formattedData
            });
            chart.draw();

            container.find('.buttons .mapActivate').click(function () {
                setOverlays(getTotalEmployeesOverlays());
            });

            container.find('.buttons .mapClear').click(function () {
                clearOverlays();
            });
        };


        var initAverageSalaryChart = function (data) {
            var container = me.container.find('#averageSalary');
            container.find('.loading').remove();
            var indexes = ['City', 'County', 'Metro', 'State', 'Nation'];
            var formattedData = formatChartData(data, indexes);

            var chart = new sizeup.charts.barChart({
                valueFormat: function (val) { return '$' + sizeup.util.numbers.format.addCommas(Math.floor(val)); },
                bar: { height: 13, padding: 4 },
                gutters: { left: 50, top: 1 },
                fillRight: false,
                container: container.find('.chart'),
                title: null,
                bars: formattedData
            });
            chart.draw();

            container.find('.buttons .mapActivate').click(function () {
                setOverlays(getAverageSalaryOverlays());
            });

            container.find('.buttons .mapClear').click(function () {
                clearOverlays();
            });
        };


        var initCostEffectivenessChart = function (data) {
            var container = me.container.find('#costEffectiveness');
            container.find('.loading').remove();
            var indexes = ['City', 'County', 'Metro', 'State', 'Nation'];
            var formattedData = formatChartData(data, indexes);

            var chart = new sizeup.charts.barChart({
                valueFormat: function (val) { return sizeup.util.numbers.format.round(val, 1); },
                bar: { height: 13, padding: 4 },
                gutters: { left: 50, top: 1 },
                fillRight: false,
                container: container.find('.chart'),
                title: null,
                bars: formattedData
            });
            chart.draw();

            container.find('.buttons .mapActivate').click(function () {
                setOverlays(getCostEffectivenessOverlays());
            });

            container.find('.buttons .mapClear').click(function () {
                clearOverlays();
            });
        };



        var publicObj = {

        };
        return publicObj;

    };
})();