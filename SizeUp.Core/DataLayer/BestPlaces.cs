using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SizeUp.Data;
using SizeUp.Core.DataLayer.Base;
using SizeUp.Core.DataLayer.Models;
using SizeUp.Core.DataLayer.Models.Base;
using SizeUp.Core.Extensions;

namespace SizeUp.Core.DataLayer
{
    public class BestPlaces : Base.Base
    {
        private static readonly long POPULATION_MIN = 100000;
        protected static IQueryable<Models.BestPlaces> FilterQuery(IQueryable<Models.BestPlaces> data, BestPlacesFilters filters)
        {
            if (filters.AverageRevenue != null)
            {
                if (filters.AverageRevenue.Min.HasValue)
                {
                    data = data.Where(i => i.AverageRevenue >= filters.AverageRevenue.Min);
                }
                if (filters.AverageRevenue.Max.HasValue)
                {
                    data = data.Where(i => i.AverageRevenue <= filters.AverageRevenue.Max);
                }
            }

            if (filters.TotalRevenue != null)
            {
                if (filters.TotalRevenue.Min.HasValue)
                {
                    data = data.Where(i => i.TotalRevenue >= filters.TotalRevenue.Min);
                }
                if (filters.TotalRevenue.Max.HasValue)
                {
                    data = data.Where(i => i.TotalRevenue <= filters.TotalRevenue.Max);
                }
            }

            if (filters.TotalEmployees != null)
            {
                if (filters.TotalEmployees.Min.HasValue)
                {
                    data = data.Where(i => i.TotalEmployees >= filters.TotalEmployees.Min);
                }
                if (filters.TotalEmployees.Max.HasValue)
                {
                    data = data.Where(i => i.TotalEmployees <= filters.TotalEmployees.Max);
                }
            }

            if (filters.AverageEmployees != null)
            {
                if (filters.AverageEmployees.Min.HasValue)
                {
                    data = data.Where(i => i.AverageEmployees >= filters.AverageEmployees.Min);
                }
                if (filters.TotalEmployees.Max.HasValue)
                {
                    data = data.Where(i => i.AverageEmployees <= filters.AverageEmployees.Max);
                }
            }

            if (filters.RevenuePerCapita != null)
            {
                if (filters.RevenuePerCapita.Min.HasValue)
                {
                    data = data.Where(i => i.RevenuePerCapita >= filters.RevenuePerCapita.Min);
                }
                if (filters.RevenuePerCapita.Max.HasValue)
                {
                    data = data.Where(i => i.RevenuePerCapita <= filters.RevenuePerCapita.Max);
                }
            }

            if (filters.HouseholdIncome != null)
            {
                if (filters.HouseholdIncome.Min.HasValue)
                {
                    data = data.Where(i => i.HouseholdIncome >= filters.HouseholdIncome.Min);
                }
                if (filters.HouseholdIncome.Max.HasValue)
                {
                    data = data.Where(i => i.HouseholdIncome <= filters.HouseholdIncome.Max);
                }
            }

            if (filters.HouseholdExpenditures != null)
            {
                if (filters.HouseholdExpenditures.Min.HasValue)
                {
                    data = data.Where(i => i.HouseholdExpenditures >= filters.HouseholdExpenditures.Min);
                }
                if (filters.HouseholdExpenditures.Max.HasValue)
                {
                    data = data.Where(i => i.HouseholdExpenditures <= filters.HouseholdExpenditures.Max);
                }
            }

            if (filters.MedianAge != null)
            {
                if (filters.MedianAge.Min.HasValue)
                {
                    data = data.Where(i => i.MedianAge >= filters.MedianAge.Min);
                }
                if (filters.MedianAge.Max.HasValue)
                {
                    data = data.Where(i => i.MedianAge <= filters.MedianAge.Max);
                }
            }

            if (filters.BachelorOrHigher != null)
            {
                var v = filters.BachelorOrHigher / 100.0d;
                data = data.Where(i => i.BachelorsDegreeOrHigher >= v);
            }

            if (filters.HighSchoolOrHigher != null)
            {
                var v = filters.HighSchoolOrHigher / 100.0d;
                data = data.Where(i => i.HighSchoolOrHigher >= v);
            }

            if (filters.WhiteCollarWorkers != null)
            {
                var v = filters.WhiteCollarWorkers / 100.0d;
                data = data.Where(i => i.WhiteCollarWorkers >= v);
            }

            if (filters.BlueCollarWorkers != null)
            {
                var v = filters.BlueCollarWorkers / 100.0d;
                data = data.Where(i => i.BlueCollarWorkers >= v);
            }

            if (filters.YoungEducated != null)
            {
                var v = filters.YoungEducated / 100.0d;
                data = data.Where(i => i.YoungEducated >= v);
            }

            if (filters.AirportsNearby != null)
            {
                data = data.Where(i => i.AirportsNearby >= filters.AirportsNearby);
            }

            if (filters.UniversitiesNearby != null)
            {
                data = data.Where(i => i.UniversitiesNearby >= filters.UniversitiesNearby);
            }

            if (filters.CommuteTime != null)
            {
                data = data.Where(i => i.CommuteTime <= filters.CommuteTime);
            }


            if (filters.Attribute == "totalRevenue")
            {
                data = data.Where(i => i.TotalRevenue != null && i.TotalRevenue > 0);
            }
            else if (filters.Attribute == "averageRevenue")
            {
                data = data.Where(i => i.AverageRevenue != null && i.AverageRevenue > 0);
            }
            else if (filters.Attribute == "revenuePerCapita")
            {
                data = data.Where(i => i.RevenuePerCapita != null && i.RevenuePerCapita > 0);
            }
            else if (filters.Attribute == "underservedMarkets")
            {
                data = data.Where(i => i.RevenuePerCapita != null && i.RevenuePerCapita > 0);
            }
            else if (filters.Attribute == "employeesPerCapita")
            {
                data = data.Where(i => i.EmployeesPerCapita != null && i.EmployeesPerCapita > 0);
            }
            else if (filters.Attribute == "householdIncome")
            {
                data = data.Where(i => i.HouseholdIncome != null && i.HouseholdIncome > 0);
            }
            else if (filters.Attribute == "whiteCollarWorkers")
            {
                data = data.Where(i => i.WhiteCollarWorkers != null && i.WhiteCollarWorkers > 0);
            }
            else if (filters.Attribute == "totalEmployees")
            {
                data = data.Where(i => i.TotalEmployees != null && i.TotalEmployees > 0);
            }
            else if (filters.Attribute == "averageEmployees")
            {
                data = data.Where(i => i.AverageEmployees != null && i.AverageEmployees > 0);
            }
            else if (filters.Attribute == "householdExpenditures")
            {
                data = data.Where(i => i.HouseholdExpenditures != null && i.HouseholdExpenditures > 0);
            }
            else if (filters.Attribute == "medianAge")
            {
                data = data.Where(i => i.MedianAge != null && i.MedianAge > 0);
            }
            else if (filters.Attribute == "bachelorsDegreeOrHigher")
            {
                data = data.Where(i => i.BachelorsDegreeOrHigher != null && i.BachelorsDegreeOrHigher > 0);
            }
            else if (filters.Attribute == "highSchoolOrHigher")
            {
                data = data.Where(i => i.HighSchoolOrHigher != null && i.HighSchoolOrHigher > 0);
            }
            else if (filters.Attribute == "blueCollarWorkers")
            {
                data = data.Where(i => i.BlueCollarWorkers != null && i.BlueCollarWorkers > 0);
            }
            else if (filters.Attribute == "airportsNearby")
            {
                data = data.Where(i => i.AirportsNearby != null && i.AirportsNearby > 0);
            }
            else if (filters.Attribute == "youngEducated")
            {
                data = data.Where(i => i.YoungEducated != null && i.YoungEducated > 0);
            }
            else if (filters.Attribute == "universitiesNearby")
            {
                data = data.Where(i => i.UniversitiesNearby != null && i.UniversitiesNearby > 0);
            }
            else if (filters.Attribute == "commuteTime")
            {
                data = data.Where(i => i.CommuteTime != null && i.CommuteTime > 0);
            }

            data = data.Where(i => i.Population >= POPULATION_MIN);

            switch (filters.Attribute)
            {
                case "totalRevenue":
                    data = data.OrderByDescending(i => i.TotalRevenue);
                    break;
                case "averageRevenue":
                    data = data.OrderByDescending(i => i.AverageRevenue);
                    break;
                case "revenuePerCapita":
                    data = data.OrderByDescending(i => i.RevenuePerCapita);
                    break;
                case "underservedMarkets":
                    data = data.OrderBy(i => i.RevenuePerCapita);
                    break;
                case "employeesPerCapita":
                    data = data.OrderByDescending(i => i.EmployeesPerCapita);
                    break;
                case "householdIncome":
                    data = data.OrderByDescending(i => i.HouseholdIncome);
                    break;
                case "whiteCollarWorkers":
                    data = data.OrderByDescending(i => i.WhiteCollarWorkers);
                    break;
                case "totalEmployees":
                    data = data.OrderByDescending(i => i.TotalEmployees);
                    break;
                case "averageEmployees":
                    data = data.OrderByDescending(i => i.AverageEmployees);
                    break;
                case "householdExpenditures":
                    data = data.OrderByDescending(i => i.HouseholdExpenditures);
                    break;
                case "medianAge":
                    data = data.OrderByDescending(i => i.MedianAge);
                    break;
                case "bachelorsDegreeOrHigher":
                    data = data.OrderByDescending(i => i.BachelorsDegreeOrHigher);
                    break;
                case "highSchoolOrHigher":
                    data = data.OrderByDescending(i => i.HighSchoolOrHigher);
                    break;
                case "blueCollarWorkers":
                    data = data.OrderByDescending(i => i.BlueCollarWorkers);
                    break;
                case "airportsNearby":
                    data = data.OrderByDescending(i => i.AirportsNearby);
                    break;
                case "youngEducated":
                    data = data.OrderByDescending(i => i.YoungEducated);
                    break;
                case "universitiesNearby":
                    data = data.OrderByDescending(i => i.UniversitiesNearby);
                    break;
                case "commuteTime":
                    data = data.OrderByDescending(i => i.CommuteTime);
                    break;
            }
            return data;
        }


        public static IQueryable<Models.BestPlacesOutput> Get(SizeUpContext context, long industryId, long? regionId, long? stateId, BestPlacesFilters filters, Granularity granularity)
        {
            IQueryable<Models.BestPlaces> data = new List<Models.BestPlaces>().AsQueryable(); //empty

            var center = Core.DataLayer.Geography.Centroid(context, granularity);
            var boundingBox = Core.DataLayer.Geography.BoundingBox(context, granularity);
            var geography =  center.Join(boundingBox, i => i.Key, o => o.Key, (o,i) => new {  Id = i.Key, BoundingBox = i.Value, Centroid = o.Value });
            var bands = context.Bands;


            if (granularity == Granularity.City)
            {
                var place = Core.DataLayer.Place.List(context);
                var industry = Core.DataLayer.Base.IndustryData.City(context).Where(i => i.IndustryId == industryId);
                var demographics = Core.DataLayer.Base.DemographicsData.City(context);
                if (regionId != null)
                {
                    demographics = demographics.Where(i => i.City.State.DivisionId == regionId);
                    industry = industry.Where(i => i.City.State.DivisionId == regionId);
                }
                if (stateId != null)
                {
                    demographics = demographics.Where(i => i.City.State.Id == stateId);
                    industry = industry.Where(i => i.City.State.Id == stateId);
                }

                data = geography.Join(industry, i => i.Id, o => o.CityId, (i, o) => new { Id = i.Id, BoundingBox = i.BoundingBox, Centroid = i.Centroid, IndustryData = o })
                    .Join(demographics, i => i.Id, o => o.CityId, (i, o) => new { Id = i.Id, BoundingBox = i.BoundingBox, Centroid = i.Centroid, IndustryData = i.IndustryData, Demographics = o })
                    .Select(i => new Models.BestPlaces
                    {
                        AirportsNearby = i.Demographics.AirPortsWithinHalfMile,
                        BachelorsDegreeOrHigher = i.Demographics.BachelorsOrHigherPercentage,
                        BlueCollarWorkers = i.Demographics.BlueCollarWorkersPercentage,
                        CommuteTime = i.Demographics.CommuteTime,
                        HighSchoolOrHigher = i.Demographics.HighSchoolOrHigherPercentage,
                        HouseholdExpenditures = i.Demographics.AverageHouseholdExpenditure,
                        HouseholdIncome = i.Demographics.MedianHouseholdIncome,
                        MedianAge = i.Demographics.MedianAge,
                        Population = i.Demographics.TotalPopulation,
                        UniversitiesNearby = i.Demographics.UniversitiesWithinHalfMile,
                        WhiteCollarWorkers = i.Demographics.WhiteCollarWorkersPercentage,
                        YoungEducated = i.Demographics.YoungEducatedPercentage,
                        AverageEmployees = i.IndustryData.AverageEmployees,
                        AverageRevenue = i.IndustryData.AverageRevenue,
                        RevenuePerCapita = i.IndustryData.RevenuePerCapita,
                        TotalEmployees = i.IndustryData.TotalEmployees,
                        TotalRevenue = i.IndustryData.TotalRevenue,
                        EmployeesPerCapita = i.IndustryData.EmployeesPerCapita,

                        AverageEmployeesBand = i.IndustryData.IndustryDataByCityBandMappings.Where(b => b.Band.Attribute.Name == IndustryAttribute.AverageEmployees).Select(b => new Band<double> { Min = (double)b.Band.Min.Value, Max = (double)b.Band.Max.Value }).FirstOrDefault(),
                        TotalEmployeesBand = i.IndustryData.IndustryDataByCityBandMappings.Where(b => b.Band.Attribute.Name == IndustryAttribute.TotalEmployees).Select(b => new Band<double> { Min = (double)b.Band.Min.Value, Max = (double)b.Band.Max.Value }).FirstOrDefault(),
                        EmployeesPerCapitaBand = i.IndustryData.IndustryDataByCityBandMappings.Where(b => b.Band.Attribute.Name == IndustryAttribute.EmployeesPerCapita).Select(b => new Band<double> { Min = (double)b.Band.Min.Value, Max = (double)b.Band.Max.Value }).FirstOrDefault(),
                        AverageRevenueBand = i.IndustryData.IndustryDataByCityBandMappings.Where(b => b.Band.Attribute.Name == IndustryAttribute.AverageRevenue).Select(b => new Band<double> { Min = (double)b.Band.Min.Value, Max = (double)b.Band.Max.Value }).FirstOrDefault(),
                        TotalRevenueBand = i.IndustryData.IndustryDataByCityBandMappings.Where(b => b.Band.Attribute.Name == IndustryAttribute.TotalRevenue).Select(b => new Band<double> { Min = (double)b.Band.Min.Value, Max = (double)b.Band.Max.Value }).FirstOrDefault(),
                        RevenuePerCapitaBand = i.IndustryData.IndustryDataByCityBandMappings.Where(b => b.Band.Attribute.Name == IndustryAttribute.RevenuePerCapita).Select(b => new Band<double> { Min = (double)b.Band.Min.Value, Max = (double)b.Band.Max.Value }).FirstOrDefault(),


                        Centroid = i.Centroid,
                        BoundingBox = i.BoundingBox,
                        Place = place.FirstOrDefault(p => p.City.Id == i.Id)
                    });
            }
            else if (granularity == Granularity.County)
            {
                var place = Core.DataLayer.Place.ListCounty(context);
                var industry = Core.DataLayer.Base.IndustryData.County(context).Where(i=>i.IndustryId == industryId);
                var demographics = Core.DataLayer.Base.DemographicsData.County(context);
                if (regionId != null)
                {
                    demographics = demographics.Where(i => i.County.State.DivisionId == regionId);
                    industry = industry.Where(i => i.County.State.DivisionId == regionId);
                }
                if (stateId != null)
                {
                    demographics = demographics.Where(i => i.County.State.Id == stateId);
                    industry = industry.Where(i => i.County.State.Id == stateId);
                }

                data = geography.Join(industry, i => i.Id, o => o.CountyId, (i, o) => new { Id = i.Id, BoundingBox = i.BoundingBox, Centroid = i.Centroid, IndustryData = o })
                    .Join(demographics, i => i.Id, o => o.CountyId, (i, o) => new { Id = i.Id, BoundingBox = i.BoundingBox, Centroid = i.Centroid, IndustryData = i.IndustryData, Demographics = o })
                    .Select(i => new Models.BestPlaces
                    {
                        AirportsNearby = i.Demographics.AirPortsWithinHalfMile,
                        BachelorsDegreeOrHigher = i.Demographics.BachelorsOrHigherPercentage,
                        BlueCollarWorkers = i.Demographics.BlueCollarWorkersPercentage,
                        CommuteTime = i.Demographics.CommuteTime,
                        HighSchoolOrHigher = i.Demographics.HighSchoolOrHigherPercentage,
                        HouseholdExpenditures = i.Demographics.AverageHouseholdExpenditure,
                        HouseholdIncome = i.Demographics.MedianHouseholdIncome,
                        MedianAge = i.Demographics.MedianAge,
                        Population = i.Demographics.TotalPopulation,
                        UniversitiesNearby = i.Demographics.UniversitiesWithinHalfMile,
                        WhiteCollarWorkers = i.Demographics.WhiteCollarWorkersPercentage,
                        YoungEducated = i.Demographics.YoungEducatedPercentage,
                        AverageEmployees = i.IndustryData.AverageEmployees,
                        AverageRevenue = i.IndustryData.AverageRevenue,
                        RevenuePerCapita = i.IndustryData.RevenuePerCapita,
                        TotalEmployees = i.IndustryData.TotalEmployees,
                        TotalRevenue = i.IndustryData.TotalRevenue,
                        EmployeesPerCapita = i.IndustryData.EmployeesPerCapita,

                        AverageEmployeesBand = i.IndustryData.IndustryDataByCountyBandMappings.Where(b => b.Band.Attribute.Name == IndustryAttribute.AverageEmployees).Select(b => new Band<double> { Min = (double)b.Band.Min.Value, Max = (double)b.Band.Max.Value }).FirstOrDefault(),
                        TotalEmployeesBand = i.IndustryData.IndustryDataByCountyBandMappings.Where(b => b.Band.Attribute.Name == IndustryAttribute.TotalEmployees).Select(b => new Band<double> { Min = (double)b.Band.Min.Value, Max = (double)b.Band.Max.Value }).FirstOrDefault(),
                        EmployeesPerCapitaBand = i.IndustryData.IndustryDataByCountyBandMappings.Where(b => b.Band.Attribute.Name == IndustryAttribute.EmployeesPerCapita).Select(b => new Band<double> { Min = (double)b.Band.Min.Value, Max = (double)b.Band.Max.Value }).FirstOrDefault(),
                        AverageRevenueBand = i.IndustryData.IndustryDataByCountyBandMappings.Where(b => b.Band.Attribute.Name == IndustryAttribute.AverageRevenue).Select(b => new Band<double> { Min = (double)b.Band.Min.Value, Max = (double)b.Band.Max.Value }).FirstOrDefault(),
                        TotalRevenueBand = i.IndustryData.IndustryDataByCountyBandMappings.Where(b => b.Band.Attribute.Name == IndustryAttribute.TotalRevenue).Select(b => new Band<double> { Min = (double)b.Band.Min.Value, Max = (double)b.Band.Max.Value }).FirstOrDefault(),
                        RevenuePerCapitaBand = i.IndustryData.IndustryDataByCountyBandMappings.Where(b => b.Band.Attribute.Name == IndustryAttribute.RevenuePerCapita).Select(b => new Band<double> { Min = (double)b.Band.Min.Value, Max = (double)b.Band.Max.Value }).FirstOrDefault(),


                        Centroid = i.Centroid,
                        BoundingBox = i.BoundingBox,
                        Place = place.FirstOrDefault(p => p.County.Id == i.Id)
                    });
            }
            else if (granularity == Granularity.Metro)
            {
                var place = Core.DataLayer.Place.ListMetro(context);
                var industry = Core.DataLayer.Base.IndustryData.Metro(context).Where(i=>i.IndustryId == industryId);
                var demographics = Core.DataLayer.Base.DemographicsData.Metro(context);
                if (regionId != null)
                {
                    demographics = demographics.Where(i => i.Metro.Counties.Any(co => co.State.DivisionId == regionId));
                    industry = industry.Where(i => i.Metro.Counties.Any(co => co.State.DivisionId == regionId));
                }
                if (stateId != null)
                {
                    demographics = demographics.Where(i => i.Metro.Counties.Any(co => co.State.Id == stateId));
                    industry = industry.Where(i => i.Metro.Counties.Any(co => co.State.Id == stateId));
                }

                data = geography.Join(industry, i => i.Id, o => o.MetroId, (i, o) => new { Id = i.Id, BoundingBox = i.BoundingBox, Centroid = i.Centroid, IndustryData = o })
                    .Join(demographics, i => i.Id, o => o.MetroId, (i, o) => new { Id = i.Id, BoundingBox = i.BoundingBox, Centroid = i.Centroid, IndustryData = i.IndustryData, Demographics = o })
                    .Select(i => new Models.BestPlaces
                    {
                        AirportsNearby = i.Demographics.AirPortsWithinHalfMile,
                        BachelorsDegreeOrHigher = i.Demographics.BachelorsOrHigherPercentage,
                        BlueCollarWorkers = i.Demographics.BlueCollarWorkersPercentage,
                        CommuteTime = i.Demographics.CommuteTime,
                        HighSchoolOrHigher = i.Demographics.HighSchoolOrHigherPercentage,
                        HouseholdExpenditures = i.Demographics.AverageHouseholdExpenditure,
                        HouseholdIncome = i.Demographics.MedianHouseholdIncome,
                        MedianAge = i.Demographics.MedianAge,
                        Population = i.Demographics.TotalPopulation,
                        UniversitiesNearby = i.Demographics.UniversitiesWithinHalfMile,
                        WhiteCollarWorkers = i.Demographics.WhiteCollarWorkersPercentage,
                        YoungEducated = i.Demographics.YoungEducatedPercentage,
                        AverageEmployees = i.IndustryData.AverageEmployees,
                        AverageRevenue = i.IndustryData.AverageRevenue,
                        RevenuePerCapita = i.IndustryData.RevenuePerCapita,
                        TotalEmployees = i.IndustryData.TotalEmployees,
                        TotalRevenue = i.IndustryData.TotalRevenue,
                        EmployeesPerCapita = i.IndustryData.EmployeesPerCapita,

                        AverageEmployeesBand = i.IndustryData.IndustryDataByMetroBandMappings.Where(b => b.Band.Attribute.Name == IndustryAttribute.AverageEmployees).Select(b => new Band<double> { Min = (double)b.Band.Min.Value, Max = (double)b.Band.Max.Value }).FirstOrDefault(),
                        TotalEmployeesBand = i.IndustryData.IndustryDataByMetroBandMappings.Where(b => b.Band.Attribute.Name == IndustryAttribute.TotalEmployees).Select(b => new Band<double> { Min = (double)b.Band.Min.Value, Max = (double)b.Band.Max.Value }).FirstOrDefault(),
                        EmployeesPerCapitaBand = i.IndustryData.IndustryDataByMetroBandMappings.Where(b => b.Band.Attribute.Name == IndustryAttribute.EmployeesPerCapita).Select(b => new Band<double> { Min = (double)b.Band.Min.Value, Max = (double)b.Band.Max.Value }).FirstOrDefault(),
                        AverageRevenueBand = i.IndustryData.IndustryDataByMetroBandMappings.Where(b => b.Band.Attribute.Name == IndustryAttribute.AverageRevenue).Select(b => new Band<double> { Min = (double)b.Band.Min.Value, Max = (double)b.Band.Max.Value }).FirstOrDefault(),
                        TotalRevenueBand = i.IndustryData.IndustryDataByMetroBandMappings.Where(b => b.Band.Attribute.Name == IndustryAttribute.TotalRevenue).Select(b => new Band<double> { Min = (double)b.Band.Min.Value, Max = (double)b.Band.Max.Value }).FirstOrDefault(),
                        RevenuePerCapitaBand = i.IndustryData.IndustryDataByMetroBandMappings.Where(b => b.Band.Attribute.Name == IndustryAttribute.RevenuePerCapita).Select(b => new Band<double> { Min = (double)b.Band.Min.Value, Max = (double)b.Band.Max.Value }).FirstOrDefault(),



                        Centroid = i.Centroid,
                        BoundingBox = i.BoundingBox,
                        Place = place.FirstOrDefault(p => p.Metro.Id == i.Id)
                    });
            }
            else if (granularity == Granularity.State)
            {
                var place = Core.DataLayer.Place.ListState(context);
                var industry = Core.DataLayer.Base.IndustryData.State(context).Where(i=>i.IndustryId == industryId);
                var demographics = Core.DataLayer.Base.DemographicsData.State(context);
                if (regionId != null)
                {
                    demographics = demographics.Where(i => i.State.DivisionId == regionId);
                    industry = industry.Where(i => i.State.DivisionId == regionId);
                }
                if (stateId != null)
                {
                    demographics = demographics.Where(i => i.State.Id == stateId);
                    industry = industry.Where(i => i.State.Id == stateId);
                }


                data = geography.Join(industry, i => i.Id, o => o.StateId, (i, o) => new { Id = i.Id, BoundingBox = i.BoundingBox, Centroid = i.Centroid, IndustryData = o })
                    .Join(demographics, i => i.Id, o => o.StateId, (i, o) => new { Id = i.Id, BoundingBox = i.BoundingBox, Centroid = i.Centroid, IndustryData = i.IndustryData, Demographics = o })
                    .Select(i => new Models.BestPlaces
                    {
                        AirportsNearby = i.Demographics.AirPortsWithinHalfMile,
                        BachelorsDegreeOrHigher = i.Demographics.BachelorsOrHigherPercentage,
                        BlueCollarWorkers = i.Demographics.BlueCollarWorkersPercentage,
                        CommuteTime = i.Demographics.CommuteTime,
                        HighSchoolOrHigher = i.Demographics.HighSchoolOrHigherPercentage,
                        HouseholdExpenditures = i.Demographics.AverageHouseholdExpenditure,
                        HouseholdIncome = i.Demographics.MedianHouseholdIncome,
                        MedianAge = i.Demographics.MedianAge,
                        Population = i.Demographics.TotalPopulation,
                        UniversitiesNearby = i.Demographics.UniversitiesWithinHalfMile,
                        WhiteCollarWorkers = i.Demographics.WhiteCollarWorkersPercentage,
                        YoungEducated = i.Demographics.YoungEducatedPercentage,
                        AverageEmployees = i.IndustryData.AverageEmployees,
                        AverageRevenue = i.IndustryData.AverageRevenue,
                        RevenuePerCapita = i.IndustryData.RevenuePerCapita,
                        TotalEmployees = i.IndustryData.TotalEmployees,
                        TotalRevenue = i.IndustryData.TotalRevenue,
                        EmployeesPerCapita = i.IndustryData.EmployeesPerCapita,

                        AverageEmployeesBand = i.IndustryData.IndustryDataByStateBandMappings.Where(b => b.Band.Attribute.Name == IndustryAttribute.AverageEmployees).Select(b => new Band<double> { Min = (double)b.Band.Min.Value, Max = (double)b.Band.Max.Value }).FirstOrDefault(),
                        TotalEmployeesBand = i.IndustryData.IndustryDataByStateBandMappings.Where(b => b.Band.Attribute.Name == IndustryAttribute.TotalEmployees).Select(b => new Band<double> { Min = (double)b.Band.Min.Value, Max = (double)b.Band.Max.Value }).FirstOrDefault(),
                        EmployeesPerCapitaBand = i.IndustryData.IndustryDataByStateBandMappings.Where(b => b.Band.Attribute.Name == IndustryAttribute.EmployeesPerCapita).Select(b => new Band<double> { Min = (double)b.Band.Min.Value, Max = (double)b.Band.Max.Value }).FirstOrDefault(),
                        AverageRevenueBand = i.IndustryData.IndustryDataByStateBandMappings.Where(b => b.Band.Attribute.Name == IndustryAttribute.AverageRevenue).Select(b => new Band<double> { Min = (double)b.Band.Min.Value, Max = (double)b.Band.Max.Value }).FirstOrDefault(),
                        TotalRevenueBand = i.IndustryData.IndustryDataByStateBandMappings.Where(b => b.Band.Attribute.Name == IndustryAttribute.TotalRevenue).Select(b => new Band<double> { Min = (double)b.Band.Min.Value, Max = (double)b.Band.Max.Value }).FirstOrDefault(),
                        RevenuePerCapitaBand = i.IndustryData.IndustryDataByStateBandMappings.Where(b => b.Band.Attribute.Name == IndustryAttribute.RevenuePerCapita).Select(b => new Band<double> { Min = (double)b.Band.Min.Value, Max = (double)b.Band.Max.Value }).FirstOrDefault(),



                        Centroid = i.Centroid,
                        BoundingBox = i.BoundingBox,
                        Place = place.FirstOrDefault(p => p.State.Id == i.Id)
                    });
            }

            data = FilterQuery(data, filters);

            IQueryable<Models.BestPlacesOutput> output = new List<Models.BestPlacesOutput>().AsQueryable();
            output = data.Select(i => new Models.BestPlacesOutput
            {
                Place = i.Place,
                Centroid = i.Centroid,
                BoundingBox = i.BoundingBox,
                TotalEmployees = i.TotalEmployeesBand,
                AverageEmployees = i.AverageEmployeesBand,
                EmployeesPerCapita = i.EmployeesPerCapitaBand,
                TotalRevenue = i.TotalRevenueBand,
                AverageRevenue = i.AverageRevenueBand,
                RevenuePerCapita = i.RevenuePerCapitaBand
            });
            return output;
        }

        public static List<Models.Band<double>> Bands(SizeUpContext context, long industryId, int itemCount, int bands, long? regionId, long? stateId, BestPlacesFilters filters, Granularity granularity)
        {
            var data = Get(context, industryId, regionId, stateId, filters, granularity)
                .Take(itemCount)
                .ToList();

            List<Models.Band<double>> output = new List<Models.Band<double>>();
            switch (filters.Attribute)
            {
                case "totalRevenue":
                    output = data.NTileDescending(i => i.TotalRevenue.Max, bands)
                       .Select(b => new Models.Band<double>() { Min = b.Min(i => i.TotalRevenue.Min), Max = b.Max(i => i.TotalRevenue.Max) })
                       .ToList();
                    break;
                case "averageRevenue":
                    output = data.NTileDescending(i => i.AverageRevenue.Max, bands)
                       .Select(b => new Models.Band<double>() { Min = b.Min(i => i.AverageRevenue.Min), Max = b.Max(i => i.AverageRevenue.Max) })
                       .ToList();
                    break;
                case "revenuePerCapita":
                    output = data.NTileDescending(i => i.RevenuePerCapita.Max, bands)
                       .Select(b => new Models.Band<double>() { Min = b.Min(i => i.RevenuePerCapita.Min), Max = b.Max(i => i.RevenuePerCapita.Max) })
                       .ToList();
                    break;
                case "underservedMarkets":
                    output = data.NTile(i => i.RevenuePerCapita.Max, bands)
                       .Select(b => new Models.Band<double>() { Min = b.Min(i => i.RevenuePerCapita.Min), Max = b.Max(i => i.RevenuePerCapita.Max) })
                       .ToList();
                    break;
                case "totalEmployees":
                    output = data.NTileDescending(i => i.TotalEmployees.Max, bands)
                       .Select(b => new Models.Band<double>() { Min = b.Min(i => i.TotalEmployees.Min), Max = b.Max(i => i.TotalEmployees.Max) })
                       .ToList();
                    break;
                case "averageEmployees":
                    output = data.NTileDescending(i => i.AverageEmployees.Max, bands)
                       .Select(b => new Models.Band<double>() { Min = b.Min(i => i.AverageEmployees.Min), Max = b.Max(i => i.AverageEmployees.Max) })
                       .ToList();
                    break;
                case "employeesPerCapita":
                    output = data.NTileDescending(i => i.EmployeesPerCapita.Max, bands)
                       .Select(b => new Models.Band<double>() { Min = b.Min(i => i.EmployeesPerCapita.Min), Max = b.Max(i => i.EmployeesPerCapita.Max) })
                       .ToList();
                    break;
            }


            if (filters.Attribute == "underservedMarkets")
            {
                output.Format();
            }
            else
            {
                output.FormatDescending();
            }
            return output;
        }
    }
}
