﻿//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.ComponentModel;
using System.Data.EntityClient;
using System.Data.Objects;
using System.Data.Objects.DataClasses;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml.Serialization;

[assembly: EdmSchemaAttribute()]
namespace SizeUp.Data.UserData
{
    #region Contexts
    
    /// <summary>
    /// No Metadata Documentation available.
    /// </summary>
    public partial class UserDataContext : ObjectContext
    {
        #region Constructors
    
        /// <summary>
        /// Initializes a new UserDataContext object using the connection string found in the 'UserDataContext' section of the application configuration file.
        /// </summary>
        public UserDataContext() : base("name=UserDataContext", "UserDataContext")
        {
            this.ContextOptions.LazyLoadingEnabled = true;
            OnContextCreated();
        }
    
        /// <summary>
        /// Initialize a new UserDataContext object.
        /// </summary>
        public UserDataContext(string connectionString) : base(connectionString, "UserDataContext")
        {
            this.ContextOptions.LazyLoadingEnabled = true;
            OnContextCreated();
        }
    
        /// <summary>
        /// Initialize a new UserDataContext object.
        /// </summary>
        public UserDataContext(EntityConnection connection) : base(connection, "UserDataContext")
        {
            this.ContextOptions.LazyLoadingEnabled = true;
            OnContextCreated();
        }
    
        #endregion
    
        #region Partial Methods
    
        partial void OnContextCreated();
    
        #endregion
    
        #region ObjectSet Properties
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        public ObjectSet<BusinessAttribute> BusinessAttributes
        {
            get
            {
                if ((_BusinessAttributes == null))
                {
                    _BusinessAttributes = base.CreateObjectSet<BusinessAttribute>("BusinessAttributes");
                }
                return _BusinessAttributes;
            }
        }
        private ObjectSet<BusinessAttribute> _BusinessAttributes;
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        public ObjectSet<CompetitorAttribute> CompetitorAttributes
        {
            get
            {
                if ((_CompetitorAttributes == null))
                {
                    _CompetitorAttributes = base.CreateObjectSet<CompetitorAttribute>("CompetitorAttributes");
                }
                return _CompetitorAttributes;
            }
        }
        private ObjectSet<CompetitorAttribute> _CompetitorAttributes;

        #endregion

        #region AddTo Methods
    
        /// <summary>
        /// Deprecated Method for adding a new object to the BusinessAttributes EntitySet. Consider using the .Add method of the associated ObjectSet&lt;T&gt; property instead.
        /// </summary>
        public void AddToBusinessAttributes(BusinessAttribute businessAttribute)
        {
            base.AddObject("BusinessAttributes", businessAttribute);
        }
    
        /// <summary>
        /// Deprecated Method for adding a new object to the CompetitorAttributes EntitySet. Consider using the .Add method of the associated ObjectSet&lt;T&gt; property instead.
        /// </summary>
        public void AddToCompetitorAttributes(CompetitorAttribute competitorAttribute)
        {
            base.AddObject("CompetitorAttributes", competitorAttribute);
        }

        #endregion

    }

    #endregion

    #region Entities
    
    /// <summary>
    /// No Metadata Documentation available.
    /// </summary>
    [EdmEntityTypeAttribute(NamespaceName="SizeUp.Data.UserData", Name="BusinessAttribute")]
    [Serializable()]
    [DataContractAttribute(IsReference=true)]
    public partial class BusinessAttribute : EntityObject
    {
        #region Factory Method
    
        /// <summary>
        /// Create a new BusinessAttribute object.
        /// </summary>
        /// <param name="id">Initial value of the Id property.</param>
        /// <param name="userId">Initial value of the UserId property.</param>
        /// <param name="industryId">Initial value of the IndustryId property.</param>
        /// <param name="placeId">Initial value of the PlaceId property.</param>
        public static BusinessAttribute CreateBusinessAttribute(global::System.Int64 id, global::System.Guid userId, global::System.Int64 industryId, global::System.Int64 placeId)
        {
            BusinessAttribute businessAttribute = new BusinessAttribute();
            businessAttribute.Id = id;
            businessAttribute.UserId = userId;
            businessAttribute.IndustryId = industryId;
            businessAttribute.PlaceId = placeId;
            return businessAttribute;
        }

        #endregion

        #region Simple Properties
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=true, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.Int64 Id
        {
            get
            {
                return _Id;
            }
            set
            {
                if (_Id != value)
                {
                    OnIdChanging(value);
                    ReportPropertyChanging("Id");
                    _Id = StructuralObject.SetValidValue(value, "Id");
                    ReportPropertyChanged("Id");
                    OnIdChanged();
                }
            }
        }
        private global::System.Int64 _Id;
        partial void OnIdChanging(global::System.Int64 value);
        partial void OnIdChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.Guid UserId
        {
            get
            {
                return _UserId;
            }
            set
            {
                OnUserIdChanging(value);
                ReportPropertyChanging("UserId");
                _UserId = StructuralObject.SetValidValue(value, "UserId");
                ReportPropertyChanged("UserId");
                OnUserIdChanged();
            }
        }
        private global::System.Guid _UserId;
        partial void OnUserIdChanging(global::System.Guid value);
        partial void OnUserIdChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.Int64 IndustryId
        {
            get
            {
                return _IndustryId;
            }
            set
            {
                OnIndustryIdChanging(value);
                ReportPropertyChanging("IndustryId");
                _IndustryId = StructuralObject.SetValidValue(value, "IndustryId");
                ReportPropertyChanged("IndustryId");
                OnIndustryIdChanged();
            }
        }
        private global::System.Int64 _IndustryId;
        partial void OnIndustryIdChanging(global::System.Int64 value);
        partial void OnIndustryIdChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.Int64 PlaceId
        {
            get
            {
                return _PlaceId;
            }
            set
            {
                OnPlaceIdChanging(value);
                ReportPropertyChanging("PlaceId");
                _PlaceId = StructuralObject.SetValidValue(value, "PlaceId");
                ReportPropertyChanged("PlaceId");
                OnPlaceIdChanged();
            }
        }
        private global::System.Int64 _PlaceId;
        partial void OnPlaceIdChanging(global::System.Int64 value);
        partial void OnPlaceIdChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public Nullable<global::System.Int64> Revenue
        {
            get
            {
                return _Revenue;
            }
            set
            {
                OnRevenueChanging(value);
                ReportPropertyChanging("Revenue");
                _Revenue = StructuralObject.SetValidValue(value, "Revenue");
                ReportPropertyChanged("Revenue");
                OnRevenueChanged();
            }
        }
        private Nullable<global::System.Int64> _Revenue;
        partial void OnRevenueChanging(Nullable<global::System.Int64> value);
        partial void OnRevenueChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public Nullable<global::System.Int32> YearStarted
        {
            get
            {
                return _YearStarted;
            }
            set
            {
                OnYearStartedChanging(value);
                ReportPropertyChanging("YearStarted");
                _YearStarted = StructuralObject.SetValidValue(value, "YearStarted");
                ReportPropertyChanged("YearStarted");
                OnYearStartedChanged();
            }
        }
        private Nullable<global::System.Int32> _YearStarted;
        partial void OnYearStartedChanging(Nullable<global::System.Int32> value);
        partial void OnYearStartedChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public Nullable<global::System.Int64> AverageSalary
        {
            get
            {
                return _AverageSalary;
            }
            set
            {
                OnAverageSalaryChanging(value);
                ReportPropertyChanging("AverageSalary");
                _AverageSalary = StructuralObject.SetValidValue(value, "AverageSalary");
                ReportPropertyChanged("AverageSalary");
                OnAverageSalaryChanged();
            }
        }
        private Nullable<global::System.Int64> _AverageSalary;
        partial void OnAverageSalaryChanging(Nullable<global::System.Int64> value);
        partial void OnAverageSalaryChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public Nullable<global::System.Int64> Employees
        {
            get
            {
                return _Employees;
            }
            set
            {
                OnEmployeesChanging(value);
                ReportPropertyChanging("Employees");
                _Employees = StructuralObject.SetValidValue(value, "Employees");
                ReportPropertyChanged("Employees");
                OnEmployeesChanged();
            }
        }
        private Nullable<global::System.Int64> _Employees;
        partial void OnEmployeesChanging(Nullable<global::System.Int64> value);
        partial void OnEmployeesChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public Nullable<global::System.Int64> HealthcareCost
        {
            get
            {
                return _HealthcareCost;
            }
            set
            {
                OnHealthcareCostChanging(value);
                ReportPropertyChanging("HealthcareCost");
                _HealthcareCost = StructuralObject.SetValidValue(value, "HealthcareCost");
                ReportPropertyChanged("HealthcareCost");
                OnHealthcareCostChanged();
            }
        }
        private Nullable<global::System.Int64> _HealthcareCost;
        partial void OnHealthcareCostChanging(Nullable<global::System.Int64> value);
        partial void OnHealthcareCostChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public Nullable<global::System.Decimal> WorkersComp
        {
            get
            {
                return _WorkersComp;
            }
            set
            {
                OnWorkersCompChanging(value);
                ReportPropertyChanging("WorkersComp");
                _WorkersComp = StructuralObject.SetValidValue(value, "WorkersComp");
                ReportPropertyChanged("WorkersComp");
                OnWorkersCompChanged();
            }
        }
        private Nullable<global::System.Decimal> _WorkersComp;
        partial void OnWorkersCompChanging(Nullable<global::System.Decimal> value);
        partial void OnWorkersCompChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public global::System.String BusinessType
        {
            get
            {
                return _BusinessType;
            }
            set
            {
                OnBusinessTypeChanging(value);
                ReportPropertyChanging("BusinessType");
                _BusinessType = StructuralObject.SetValidValue(value, true, "BusinessType");
                ReportPropertyChanged("BusinessType");
                OnBusinessTypeChanged();
            }
        }
        private global::System.String _BusinessType;
        partial void OnBusinessTypeChanging(global::System.String value);
        partial void OnBusinessTypeChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public global::System.String BusinessSize
        {
            get
            {
                return _BusinessSize;
            }
            set
            {
                OnBusinessSizeChanging(value);
                ReportPropertyChanging("BusinessSize");
                _BusinessSize = StructuralObject.SetValidValue(value, true, "BusinessSize");
                ReportPropertyChanged("BusinessSize");
                OnBusinessSizeChanged();
            }
        }
        private global::System.String _BusinessSize;
        partial void OnBusinessSizeChanging(global::System.String value);
        partial void OnBusinessSizeChanged();

        #endregion

    }
    
    /// <summary>
    /// No Metadata Documentation available.
    /// </summary>
    [EdmEntityTypeAttribute(NamespaceName="SizeUp.Data.UserData", Name="CompetitorAttribute")]
    [Serializable()]
    [DataContractAttribute(IsReference=true)]
    public partial class CompetitorAttribute : EntityObject
    {
        #region Factory Method
    
        /// <summary>
        /// Create a new CompetitorAttribute object.
        /// </summary>
        /// <param name="id">Initial value of the Id property.</param>
        /// <param name="userId">Initial value of the UserId property.</param>
        /// <param name="industryId">Initial value of the IndustryId property.</param>
        /// <param name="placeId">Initial value of the PlaceId property.</param>
        public static CompetitorAttribute CreateCompetitorAttribute(global::System.Int64 id, global::System.Guid userId, global::System.Int64 industryId, global::System.Int64 placeId)
        {
            CompetitorAttribute competitorAttribute = new CompetitorAttribute();
            competitorAttribute.Id = id;
            competitorAttribute.UserId = userId;
            competitorAttribute.IndustryId = industryId;
            competitorAttribute.PlaceId = placeId;
            return competitorAttribute;
        }

        #endregion

        #region Simple Properties
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=true, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.Int64 Id
        {
            get
            {
                return _Id;
            }
            set
            {
                if (_Id != value)
                {
                    OnIdChanging(value);
                    ReportPropertyChanging("Id");
                    _Id = StructuralObject.SetValidValue(value, "Id");
                    ReportPropertyChanged("Id");
                    OnIdChanged();
                }
            }
        }
        private global::System.Int64 _Id;
        partial void OnIdChanging(global::System.Int64 value);
        partial void OnIdChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.Guid UserId
        {
            get
            {
                return _UserId;
            }
            set
            {
                OnUserIdChanging(value);
                ReportPropertyChanging("UserId");
                _UserId = StructuralObject.SetValidValue(value, "UserId");
                ReportPropertyChanged("UserId");
                OnUserIdChanged();
            }
        }
        private global::System.Guid _UserId;
        partial void OnUserIdChanging(global::System.Guid value);
        partial void OnUserIdChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.Int64 IndustryId
        {
            get
            {
                return _IndustryId;
            }
            set
            {
                OnIndustryIdChanging(value);
                ReportPropertyChanging("IndustryId");
                _IndustryId = StructuralObject.SetValidValue(value, "IndustryId");
                ReportPropertyChanged("IndustryId");
                OnIndustryIdChanged();
            }
        }
        private global::System.Int64 _IndustryId;
        partial void OnIndustryIdChanging(global::System.Int64 value);
        partial void OnIndustryIdChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.Int64 PlaceId
        {
            get
            {
                return _PlaceId;
            }
            set
            {
                OnPlaceIdChanging(value);
                ReportPropertyChanging("PlaceId");
                _PlaceId = StructuralObject.SetValidValue(value, "PlaceId");
                ReportPropertyChanged("PlaceId");
                OnPlaceIdChanged();
            }
        }
        private global::System.Int64 _PlaceId;
        partial void OnPlaceIdChanging(global::System.Int64 value);
        partial void OnPlaceIdChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public Nullable<global::System.Int64> ComsumerExpenditureId
        {
            get
            {
                return _ComsumerExpenditureId;
            }
            set
            {
                OnComsumerExpenditureIdChanging(value);
                ReportPropertyChanging("ComsumerExpenditureId");
                _ComsumerExpenditureId = StructuralObject.SetValidValue(value, "ComsumerExpenditureId");
                ReportPropertyChanged("ComsumerExpenditureId");
                OnComsumerExpenditureIdChanged();
            }
        }
        private Nullable<global::System.Int64> _ComsumerExpenditureId;
        partial void OnComsumerExpenditureIdChanging(Nullable<global::System.Int64> value);
        partial void OnComsumerExpenditureIdChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public Nullable<global::System.Int64> RootId
        {
            get
            {
                return _RootId;
            }
            set
            {
                OnRootIdChanging(value);
                ReportPropertyChanging("RootId");
                _RootId = StructuralObject.SetValidValue(value, "RootId");
                ReportPropertyChanged("RootId");
                OnRootIdChanged();
            }
        }
        private Nullable<global::System.Int64> _RootId;
        partial void OnRootIdChanging(Nullable<global::System.Int64> value);
        partial void OnRootIdChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public global::System.String Competitors
        {
            get
            {
                return _Competitors;
            }
            set
            {
                OnCompetitorsChanging(value);
                ReportPropertyChanging("Competitors");
                _Competitors = StructuralObject.SetValidValue(value, true, "Competitors");
                ReportPropertyChanged("Competitors");
                OnCompetitorsChanged();
            }
        }
        private global::System.String _Competitors;
        partial void OnCompetitorsChanging(global::System.String value);
        partial void OnCompetitorsChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public global::System.String Suppliers
        {
            get
            {
                return _Suppliers;
            }
            set
            {
                OnSuppliersChanging(value);
                ReportPropertyChanging("Suppliers");
                _Suppliers = StructuralObject.SetValidValue(value, true, "Suppliers");
                ReportPropertyChanged("Suppliers");
                OnSuppliersChanged();
            }
        }
        private global::System.String _Suppliers;
        partial void OnSuppliersChanging(global::System.String value);
        partial void OnSuppliersChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public global::System.String Buyers
        {
            get
            {
                return _Buyers;
            }
            set
            {
                OnBuyersChanging(value);
                ReportPropertyChanging("Buyers");
                _Buyers = StructuralObject.SetValidValue(value, true, "Buyers");
                ReportPropertyChanged("Buyers");
                OnBuyersChanged();
            }
        }
        private global::System.String _Buyers;
        partial void OnBuyersChanging(global::System.String value);
        partial void OnBuyersChanged();

        #endregion

    }

    #endregion

}