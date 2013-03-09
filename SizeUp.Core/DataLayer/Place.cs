using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SizeUp.Data;
using SizeUp.Core.DataLayer.Base;
using SizeUp.Core.DataLayer.Models;
namespace SizeUp.Core.DataLayer
{
    public class Place : Base.Base
    {

        public static Models.Place Get(SizeUpContext context, long? id)
        {
            var raw = Base.Place.Get(context);
            var data = raw
                .Where(i => i.Id == id)
                .Select(i => new Models.Place
                {
                    Id = i.Id,
                    DisplayName = raw.Count(s => s.City.Name == i.City.Name && s.County.State.Name == i.County.State.Name) > 1 ? (i.City.Name + ", " + i.County.State.Abbreviation + " (" + i.County.Name + " County - " + i.City.CityType.Name + ")") : (i.City.Name + ", " + i.County.State.Abbreviation),
                    City = new Models.City()
                    {
                        Id = i.City.Id,
                        Name = i.City.Name,
                        SEOKey = i.City.SEOKey,
                        TypeName = i.City.CityType.Name
                    },
                    County = new Models.County
                    {
                        Id = i.County.Id,
                        Name = i.County.Name,
                        SEOKey = i.County.SEOKey
                    },
                    Metro = new Models.Metro
                    {
                        Id = i.County.Metro.Id,
                        Name = i.County.Metro.Name,
                        SEOKey = i.County.Metro.SEOKey
                    },
                    State = new Models.State
                    {
                        Id = i.County.State.Id,
                        Abbreviation = i.County.State.Abbreviation,
                        Name = i.County.State.Name,
                        SEOKey = i.County.State.SEOKey
                    }
                })
                .FirstOrDefault();
            return data;
        }

        public static Models.Place GetByBusiness(SizeUpContext context, long businessId)
        {
            var raw = Base.Place.Get(context);
            var data = raw
                .Where(i => i.City.BusinessCityMappings.Any(bc=>bc.BusinessId == businessId))
                .Select(i => new Models.Place
                {
                    Id = i.Id,
                    DisplayName = raw.Count(s => s.City.Name == i.City.Name && s.County.State.Name == i.County.State.Name) > 1 ? (i.City.Name + ", " + i.County.State.Abbreviation + " (" + i.County.Name + " County - " + i.City.CityType.Name + ")") : (i.City.Name + ", " + i.County.State.Abbreviation),
                    City = new Models.City()
                    {
                        Id = i.City.Id,
                        Name = i.City.Name,
                        SEOKey = i.City.SEOKey,
                        TypeName = i.City.CityType.Name
                    },
                    County = new Models.County
                    {
                        Id = i.County.Id,
                        Name = i.County.Name,
                        SEOKey = i.County.SEOKey
                    },
                    Metro = new Models.Metro
                    {
                        Id = i.County.Metro.Id,
                        Name = i.County.Metro.Name,
                        SEOKey = i.County.Metro.SEOKey
                    },
                    State = new Models.State
                    {
                        Id = i.County.State.Id,
                        Abbreviation = i.County.State.Abbreviation,
                        Name = i.County.State.Name,
                        SEOKey = i.County.State.SEOKey
                    }
                })
                .FirstOrDefault();
            return data;
        }

        public static Models.Place GetLegacy(SizeUpContext context, string SEOKey)
        {
            var raw = Base.Place.Get(context);
            var data = raw
                .Where(i => i.City.LegacyCommunitySEOKeys.Any(l=>l.SEOKey == SEOKey))
                .Select(i => new Models.Place
                {
                    Id = i.Id,
                    DisplayName = raw.Count(s => s.City.Name == i.City.Name && s.County.State.Name == i.County.State.Name) > 1 ? (i.City.Name + ", " + i.County.State.Abbreviation + " (" + i.County.Name + " County - " + i.City.CityType.Name + ")") : (i.City.Name + ", " + i.County.State.Abbreviation),
                    City = new Models.City()
                    {
                        Id = i.City.Id,
                        Name = i.City.Name,
                        SEOKey = i.City.SEOKey,
                        TypeName = i.City.CityType.Name
                    },
                    County = new Models.County
                    {
                        Id = i.County.Id,
                        Name = i.County.Name,
                        SEOKey = i.County.SEOKey
                    },
                    Metro = new Models.Metro
                    {
                        Id = i.County.Metro.Id,
                        Name = i.County.Metro.Name,
                        SEOKey = i.County.Metro.SEOKey
                    },
                    State = new Models.State
                    {
                        Id = i.County.State.Id,
                        Abbreviation = i.County.State.Abbreviation,
                        Name = i.County.State.Name,
                        SEOKey = i.County.State.SEOKey
                    }
                })
                .FirstOrDefault();
            return data;
        }

        public static Models.Place Get(SizeUpContext context, string stateSEOKey, string countySEOKey, string citySEOKey)
        {
            var raw = Base.Place.Get(context);
            Models.Place output = new Models.Place() { City = new Models.City(), County = new Models.County(), Metro = new Models.Metro(), State = new Models.State() };
            if (!string.IsNullOrEmpty(stateSEOKey) && !string.IsNullOrEmpty(countySEOKey) && !string.IsNullOrEmpty(citySEOKey))
            {
                var data = raw
                    .Where(i => i.County.SEOKey == countySEOKey && i.County.State.SEOKey == stateSEOKey && i.City.SEOKey == citySEOKey)
                    .Select(i => new Models.Place
                    {
                        Id = i.Id,
                        DisplayName = raw.Count(s => s.City.Name == i.City.Name && s.County.State.Name == i.County.State.Name) > 1 ? (i.City.Name + ", " + i.County.State.Abbreviation + " (" + i.County.Name + " County - " + i.City.CityType.Name + ")") : (i.City.Name + ", " + i.County.State.Abbreviation),
                        City = new Models.City()
                        {
                            Id = i.City.Id,
                            Name = i.City.Name,
                            SEOKey = i.City.SEOKey,
                            TypeName = i.City.CityType.Name
                        },
                        County = new Models.County
                        {
                            Id = i.County.Id,
                            Name = i.County.Name,
                            SEOKey = i.County.SEOKey
                        },
                        Metro = new Models.Metro
                        {
                            Id = i.County.Metro.Id,
                            Name = i.County.Metro.Name,
                            SEOKey = i.County.Metro.SEOKey
                        },
                        State = new Models.State
                        {
                            Id = i.County.State.Id,
                            Abbreviation = i.County.State.Abbreviation,
                            Name = i.County.State.Name,
                            SEOKey = i.County.State.SEOKey
                        }
                    })
                    .FirstOrDefault();
                if (data != null)
                {
                    output = data;
                }
            }
            else if (!string.IsNullOrEmpty(stateSEOKey) && !string.IsNullOrEmpty(countySEOKey))
            {
                var data = raw
                    .Where(i => i.County.SEOKey == countySEOKey && i.County.State.SEOKey == stateSEOKey)
                    .Select(i => new Models.Place
                    {
                        City = new Models.City()
                        {
                        },
                        County = new Models.County
                        {
                            Id = i.County.Id,
                            Name = i.County.Name,
                            SEOKey = i.County.SEOKey
                        },
                        Metro = new Models.Metro
                        {
                            Id = i.County.Metro.Id,
                            Name = i.County.Metro.Name,
                            SEOKey = i.County.Metro.SEOKey
                        },
                        State = new Models.State
                        {
                            Id = i.County.State.Id,
                            Abbreviation = i.County.State.Abbreviation,
                            Name = i.County.State.Name,
                            SEOKey = i.County.State.SEOKey
                        }
                    })
                    .FirstOrDefault();
                if (data != null)
                {
                    output = data;
                }
            }
            else if (!string.IsNullOrEmpty(stateSEOKey))
            {
                var data = raw
                    .Where(i => i.County.State.SEOKey == stateSEOKey)
                    .Select(i => new Models.Place
                    {
                        City = new Models.City()
                        {
                        },
                        County = new Models.County
                        {
                        },
                        Metro = new Models.Metro
                        {
                        },
                        State = new Models.State
                        {
                            Id = i.County.State.Id,
                            Abbreviation = i.County.State.Abbreviation,
                            Name = i.County.State.Name,
                            SEOKey = i.County.State.SEOKey
                        }
                    })
                    .FirstOrDefault();
                if (data != null)
                {
                    output = data;
                }
            }
            return output;
        }

        public static IQueryable<Models.Base.DistanceEntity<Models.Place>> ListNear(SizeUpContext context, Core.Geo.LatLng latLng)
        {
            var dist = Base.Place.Distance(context, latLng);
            var raw = Base.Place.Get(context);
            var data = dist.Select(i => new Models.Base.DistanceEntity<Models.Place>
            {
                Distance = i.Distance,
                Entity = new Models.Place
                {
                    Id = i.Entity.Id,
                    DisplayName = raw.Count(s => s.City.Name == i.Entity.City.Name && s.County.State.Name == i.Entity.County.State.Name) > 1 ? (i.Entity.City.Name + ", " + i.Entity.County.State.Abbreviation + " (" + i.Entity.County.Name + " County - " + i.Entity.City.CityType.Name + ")") : (i.Entity.City.Name + ", " + i.Entity.County.State.Abbreviation),
                    City = new Models.City()
                    {
                        Id = i.Entity.City.Id,
                        Name = i.Entity.City.Name,
                        SEOKey = i.Entity.City.SEOKey,
                        TypeName = i.Entity.City.CityType.Name
                    },
                    County = new Models.County
                    {
                        Id = i.Entity.County.Id,
                        Name = i.Entity.County.Name,
                        SEOKey = i.Entity.County.SEOKey
                    },
                    Metro = new Models.Metro
                    {
                        Id = i.Entity.County.Metro.Id,
                        Name = i.Entity.County.Metro.Name,
                        SEOKey = i.Entity.County.Metro.SEOKey
                    },
                    State = new Models.State
                    {
                        Id = i.Entity.County.State.Id,
                        Abbreviation = i.Entity.County.State.Abbreviation,
                        Name = i.Entity.County.State.Name,
                        SEOKey = i.Entity.County.State.SEOKey
                    }
                }
            });
            return data;
        }

        public static List<Models.Place> List(SizeUpContext context, List<long> placeIds)
        {
            var raw = Base.Place.Get(context);
            var data = raw
                .Where(i => placeIds.Contains(i.Id))
                .Select(i => new Models.Place
                {
                    Id = i.Id,
                    DisplayName = raw.Count(s => s.City.Name == i.City.Name && s.County.State.Name == i.County.State.Name) > 1 ? (i.City.Name + ", " + i.County.State.Abbreviation + " (" + i.County.Name + " County - " + i.City.CityType.Name + ")") : (i.City.Name + ", " + i.County.State.Abbreviation),
                    City = new Models.City()
                    {
                        Id = i.City.Id,
                        Name = i.City.Name,
                        SEOKey = i.City.SEOKey,
                        TypeName = i.City.CityType.Name
                    },
                    County = new Models.County
                    {
                        Id = i.County.Id,
                        Name = i.County.Name,
                        SEOKey = i.County.SEOKey
                    },
                    Metro = new Models.Metro
                    {
                        Id = i.County.Metro.Id,
                        Name = i.County.Metro.Name,
                        SEOKey = i.County.Metro.SEOKey
                    },
                    State = new Models.State
                    {
                        Id = i.County.State.Id,
                        Abbreviation = i.County.State.Abbreviation,
                        Name = i.County.State.Name,
                        SEOKey = i.County.State.SEOKey
                    }
                })
                .ToList();
            return data;
        }

        public static List<Models.Place> ListInState(SizeUpContext context, long stateId)
        {
            var raw = Base.Place.Get(context);
            var data = raw
                .Where(i => i.County.StateId == stateId)
                .Select(i => new Models.Place
                {
                    Id = i.Id,
                    DisplayName = raw.Count(s => s.City.Name == i.City.Name && s.County.State.Name == i.County.State.Name) > 1 ? (i.City.Name + ", " + i.County.State.Abbreviation + " (" + i.County.Name + " County - " + i.City.CityType.Name + ")") : (i.City.Name + ", " + i.County.State.Abbreviation),
                    City = new Models.City()
                    {
                        Id = i.City.Id,
                        Name = i.City.Name,
                        SEOKey = i.City.SEOKey,
                        TypeName = i.City.CityType.Name
                    },
                    County = new Models.County
                    {
                        Id = i.County.Id,
                        Name = i.County.Name,
                        SEOKey = i.County.SEOKey
                    },
                    Metro = new Models.Metro
                    {
                        Id = i.County.Metro.Id,
                        Name = i.County.Metro.Name,
                        SEOKey = i.County.Metro.SEOKey
                    },
                    State = new Models.State
                    {
                        Id = i.County.State.Id,
                        Abbreviation = i.County.State.Abbreviation,
                        Name = i.County.State.Name,
                        SEOKey = i.County.State.SEOKey
                    }
                })
                .ToList();
            return data;
        }

        public static IQueryable<Models.Place> List(SizeUpContext context)
        {
            var raw = Base.Place.Get(context);
            var data = raw
                .Select(i => new Models.Place
                {
                    Id = i.Id,
                    DisplayName = raw.Count(s => s.City.Name == i.City.Name && s.County.State.Name == i.County.State.Name) > 1 ? (i.City.Name + ", " + i.County.State.Abbreviation + " (" + i.County.Name + " County - " + i.City.CityType.Name + ")") : (i.City.Name + ", " + i.County.State.Abbreviation),
                    City = new Models.City()
                    {
                        Id = i.City.Id,
                        Name = i.City.Name,
                        SEOKey = i.City.SEOKey,
                        TypeName = i.City.CityType.Name
                    },
                    County = new Models.County
                    {
                        Id = i.County.Id,
                        Name = i.County.Name,
                        SEOKey = i.County.SEOKey
                    },
                    Metro = new Models.Metro
                    {
                        Id = i.County.Metro.Id,
                        Name = i.County.Metro.Name,
                        SEOKey = i.County.Metro.SEOKey
                    },
                    State = new Models.State
                    {
                        Id = i.County.State.Id,
                        Abbreviation = i.County.State.Abbreviation,
                        Name = i.County.State.Name,
                        SEOKey = i.County.State.SEOKey
                    }
                });
            return data;
        }


        public static IQueryable<Models.Place> Search(SizeUpContext context, string term)
        {
            var raw = Base.Place.Get(context);
            var keywords = Base.Place.Keywords(context);

            var searchSpace = raw.Select(i => new
            {
                Place = i,
                Search = i.City.Name
            })
            .Union(keywords.Select(i => new
            {
                Place = i.CityCountyMapping,
                Search = i.Name
            }));


            var search = term.Split(',');
            string city = search[0].Trim();
            string state = string.Empty;


            var data = searchSpace.Where(i => i.Search.StartsWith(city));
            if (search.Length > 1)
            {
                state = search[1].Trim();
                data = data.Where(i => i.Place.County.State.Abbreviation.StartsWith(state));
            }

            var output = data
                .OrderBy(i => i.Place.City.Name)
                .ThenBy(i => i.Place.City.State.Abbreviation)
                .ThenByDescending(i => i.Place.City.DemographicsByCities.Where(d=>d.Year == Year && d.Quarter == Quarter).FirstOrDefault().TotalPopulation)
                .Select(i => i.Place)
                .Select(i => new Models.Place
                {
                    Id = i.Id,
                    DisplayName = raw.Count(s => s.City.Name == i.City.Name && s.County.State.Name == i.County.State.Name) > 1 ? (i.City.Name + ", " + i.County.State.Abbreviation + " (" + i.County.Name + " County - " + i.City.CityType.Name + ")") : (i.City.Name + ", " + i.County.State.Abbreviation),
                    City = new Models.City()
                    {
                        Id = i.City.Id,
                        Name = i.City.Name,
                        SEOKey = i.City.SEOKey,
                        TypeName = i.City.CityType.Name

                    },
                    County = new Models.County
                    {
                        Id = i.County.Id,
                        Name = i.County.Name,
                        SEOKey = i.County.SEOKey
                    },
                    Metro = new Models.Metro
                    {
                        Id = i.County.Metro.Id,
                        Name = i.County.Metro.Name,
                        SEOKey = i.County.Metro.SEOKey
                    },
                    State = new Models.State
                    {
                        Id = i.County.State.Id,
                        Abbreviation = i.County.State.Abbreviation,
                        Name = i.County.State.Name,
                        SEOKey = i.County.State.SEOKey
                    }
                });

                
            return output;
        }
    }
}
