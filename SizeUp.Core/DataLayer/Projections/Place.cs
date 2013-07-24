using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using SizeUp.Data;
using SizeUp.Core.DataLayer.Models;
namespace SizeUp.Core.DataLayer.Projections
{
    public static class Place
    {
        public class Default : Projection<Data.Place, Models.Place>
        {
            public override Expression<Func<Data.Place, Models.Place>> Expression
            {
                get
                {
                    return (i => new Models.Place
                    {
                        Id = i.Id,
                        DisplayName = i.GeographicLocation.LongName,
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
                        },
                        Region = new Models.Division
                        {
                            Id = i.County.State.Division.Id,
                            RegionName = i.County.State.Division.Region.Name,
                            Name = i.County.State.Division.Name
                        },
                        Nation = new Models.Nation
                        {
                            Id = i.County.State.Nation.Id,
                            Name = i.County.State.Nation.Name
                        }
                    });
                }
            }
        }

        public class Distance : Projection<DistanceEntity<Data.Place>, DistanceEntity<Models.Place>>
        {
            public override Expression<Func<DistanceEntity<Data.Place>, DistanceEntity<Models.Place>>> Expression
            {
                get
                {
                    return (i => new DistanceEntity<Models.Place>
                    {

                        Distance = i.Distance,
                        Entity = new Models.Place
                        {
                            Id = i.Entity.Id,
                            DisplayName = i.Entity.GeographicLocation.LongName,
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
                            },
                            Region = new Models.Division
                            {
                                Id = i.Entity.County.State.Division.Id,
                                RegionName = i.Entity.County.State.Division.Region.Name,
                                Name = i.Entity.County.State.Division.Name
                            },
                            Nation = new Models.Nation
                            {
                                Id = i.Entity.County.State.Nation.Id,
                                Name = i.Entity.County.State.Nation.Name
                            }
                        }
                    });
                }
            }
        }

        public class County : Projection<Data.Place, Models.Place>
        {
            public override Expression<Func<Data.Place, Models.Place>> Expression
            {
                get
                {
                    return (i => new Models.Place
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
                        },
                        Region = new Models.Division
                        {
                            Id = i.County.State.Division.Id,
                            RegionName = i.County.State.Division.Region.Name,
                            Name = i.County.State.Division.Name
                        },
                        Nation = new Models.Nation
                        {
                            Id = i.County.State.Nation.Id,
                            Name = i.County.State.Nation.Name
                        }
                    });
                }
            }
        }


        public class Metro : Projection<Data.Place, Models.Place>
        {
            public override Expression<Func<Data.Place, Models.Place>> Expression
            {
                get
                {
                    return (i => new Models.Place
                    {

                        City = new Models.City()
                        {
                        },
                        County = new Models.County
                        {
                        },
                        Metro = new Models.Metro
                        {
                            Id = i.County.Metro.Id,
                            Name = i.County.Metro.Name,
                            SEOKey = i.County.Metro.SEOKey
                        },
                        State = new Models.State
                        {
                        },
                        Region = new Models.Division
                        {
                        },
                        Nation = new Models.Nation
                        {
                        }
                    });
                }
            }
        }

        public class State : Projection<Data.Place, Models.Place>
        {
            public override Expression<Func<Data.Place, Models.Place>> Expression
            {
                get
                {
                    return (i => new Models.Place
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
                        },
                        Region = new Models.Division
                        {
                            Id = i.County.State.Division.Id,
                            RegionName = i.County.State.Division.Region.Name,
                            Name = i.County.State.Division.Name
                        },
                        Nation = new Models.Nation
                        {
                            Id = i.County.State.Nation.Id,
                            Name = i.County.State.Nation.Name
                        }
                    });
                }
            }
        }
    }
}
