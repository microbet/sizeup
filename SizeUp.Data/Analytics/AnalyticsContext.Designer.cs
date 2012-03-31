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
namespace SizeUp.Data.Analytics
{
    #region Contexts
    
    /// <summary>
    /// No Metadata Documentation available.
    /// </summary>
    public partial class AnalyticsContext : ObjectContext
    {
        #region Constructors
    
        /// <summary>
        /// Initializes a new AnalyticsContext object using the connection string found in the 'AnalyticsContext' section of the application configuration file.
        /// </summary>
        public AnalyticsContext() : base("name=AnalyticsContext", "AnalyticsContext")
        {
            this.ContextOptions.LazyLoadingEnabled = true;
            OnContextCreated();
        }
    
        /// <summary>
        /// Initialize a new AnalyticsContext object.
        /// </summary>
        public AnalyticsContext(string connectionString) : base(connectionString, "AnalyticsContext")
        {
            this.ContextOptions.LazyLoadingEnabled = true;
            OnContextCreated();
        }
    
        /// <summary>
        /// Initialize a new AnalyticsContext object.
        /// </summary>
        public AnalyticsContext(EntityConnection connection) : base(connection, "AnalyticsContext")
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
        public ObjectSet<UserRegistration> UserRegistrations
        {
            get
            {
                if ((_UserRegistrations == null))
                {
                    _UserRegistrations = base.CreateObjectSet<UserRegistration>("UserRegistrations");
                }
                return _UserRegistrations;
            }
        }
        private ObjectSet<UserRegistration> _UserRegistrations;

        #endregion

        #region AddTo Methods
    
        /// <summary>
        /// Deprecated Method for adding a new object to the UserRegistrations EntitySet. Consider using the .Add method of the associated ObjectSet&lt;T&gt; property instead.
        /// </summary>
        public void AddToUserRegistrations(UserRegistration userRegistration)
        {
            base.AddObject("UserRegistrations", userRegistration);
        }

        #endregion

    }

    #endregion

    #region Entities
    
    /// <summary>
    /// No Metadata Documentation available.
    /// </summary>
    [EdmEntityTypeAttribute(NamespaceName="SizeUp.Data.Analytics", Name="UserRegistration")]
    [Serializable()]
    [DataContractAttribute(IsReference=true)]
    public partial class UserRegistration : EntityObject
    {
        #region Factory Method
    
        /// <summary>
        /// Create a new UserRegistration object.
        /// </summary>
        /// <param name="id">Initial value of the Id property.</param>
        /// <param name="timestamp">Initial value of the Timestamp property.</param>
        /// <param name="minute">Initial value of the Minute property.</param>
        /// <param name="hour">Initial value of the Hour property.</param>
        /// <param name="day">Initial value of the Day property.</param>
        /// <param name="week">Initial value of the Week property.</param>
        /// <param name="month">Initial value of the Month property.</param>
        /// <param name="quarter">Initial value of the Quarter property.</param>
        /// <param name="year">Initial value of the Year property.</param>
        /// <param name="userId">Initial value of the UserId property.</param>
        /// <param name="returnUrl">Initial value of the ReturnUrl property.</param>
        /// <param name="email">Initial value of the Email property.</param>
        public static UserRegistration CreateUserRegistration(global::System.Int64 id, global::System.DateTime timestamp, global::System.Int32 minute, global::System.Int32 hour, global::System.Int32 day, global::System.Int32 week, global::System.Int32 month, global::System.Int32 quarter, global::System.Int32 year, global::System.Guid userId, global::System.String returnUrl, global::System.String email)
        {
            UserRegistration userRegistration = new UserRegistration();
            userRegistration.Id = id;
            userRegistration.Timestamp = timestamp;
            userRegistration.Minute = minute;
            userRegistration.Hour = hour;
            userRegistration.Day = day;
            userRegistration.Week = week;
            userRegistration.Month = month;
            userRegistration.Quarter = quarter;
            userRegistration.Year = year;
            userRegistration.UserId = userId;
            userRegistration.ReturnUrl = returnUrl;
            userRegistration.Email = email;
            return userRegistration;
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
        public global::System.DateTime Timestamp
        {
            get
            {
                return _Timestamp;
            }
            set
            {
                OnTimestampChanging(value);
                ReportPropertyChanging("Timestamp");
                _Timestamp = StructuralObject.SetValidValue(value, "Timestamp");
                ReportPropertyChanged("Timestamp");
                OnTimestampChanged();
            }
        }
        private global::System.DateTime _Timestamp;
        partial void OnTimestampChanging(global::System.DateTime value);
        partial void OnTimestampChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.Int32 Minute
        {
            get
            {
                return _Minute;
            }
            set
            {
                OnMinuteChanging(value);
                ReportPropertyChanging("Minute");
                _Minute = StructuralObject.SetValidValue(value, "Minute");
                ReportPropertyChanged("Minute");
                OnMinuteChanged();
            }
        }
        private global::System.Int32 _Minute;
        partial void OnMinuteChanging(global::System.Int32 value);
        partial void OnMinuteChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.Int32 Hour
        {
            get
            {
                return _Hour;
            }
            set
            {
                OnHourChanging(value);
                ReportPropertyChanging("Hour");
                _Hour = StructuralObject.SetValidValue(value, "Hour");
                ReportPropertyChanged("Hour");
                OnHourChanged();
            }
        }
        private global::System.Int32 _Hour;
        partial void OnHourChanging(global::System.Int32 value);
        partial void OnHourChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.Int32 Day
        {
            get
            {
                return _Day;
            }
            set
            {
                OnDayChanging(value);
                ReportPropertyChanging("Day");
                _Day = StructuralObject.SetValidValue(value, "Day");
                ReportPropertyChanged("Day");
                OnDayChanged();
            }
        }
        private global::System.Int32 _Day;
        partial void OnDayChanging(global::System.Int32 value);
        partial void OnDayChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.Int32 Week
        {
            get
            {
                return _Week;
            }
            set
            {
                OnWeekChanging(value);
                ReportPropertyChanging("Week");
                _Week = StructuralObject.SetValidValue(value, "Week");
                ReportPropertyChanged("Week");
                OnWeekChanged();
            }
        }
        private global::System.Int32 _Week;
        partial void OnWeekChanging(global::System.Int32 value);
        partial void OnWeekChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.Int32 Month
        {
            get
            {
                return _Month;
            }
            set
            {
                OnMonthChanging(value);
                ReportPropertyChanging("Month");
                _Month = StructuralObject.SetValidValue(value, "Month");
                ReportPropertyChanged("Month");
                OnMonthChanged();
            }
        }
        private global::System.Int32 _Month;
        partial void OnMonthChanging(global::System.Int32 value);
        partial void OnMonthChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.Int32 Quarter
        {
            get
            {
                return _Quarter;
            }
            set
            {
                OnQuarterChanging(value);
                ReportPropertyChanging("Quarter");
                _Quarter = StructuralObject.SetValidValue(value, "Quarter");
                ReportPropertyChanged("Quarter");
                OnQuarterChanged();
            }
        }
        private global::System.Int32 _Quarter;
        partial void OnQuarterChanging(global::System.Int32 value);
        partial void OnQuarterChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.Int32 Year
        {
            get
            {
                return _Year;
            }
            set
            {
                OnYearChanging(value);
                ReportPropertyChanging("Year");
                _Year = StructuralObject.SetValidValue(value, "Year");
                ReportPropertyChanged("Year");
                OnYearChanged();
            }
        }
        private global::System.Int32 _Year;
        partial void OnYearChanging(global::System.Int32 value);
        partial void OnYearChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public Nullable<global::System.Int64> APIKeyId
        {
            get
            {
                return _APIKeyId;
            }
            set
            {
                OnAPIKeyIdChanging(value);
                ReportPropertyChanging("APIKeyId");
                _APIKeyId = StructuralObject.SetValidValue(value, "APIKeyId");
                ReportPropertyChanged("APIKeyId");
                OnAPIKeyIdChanged();
            }
        }
        private Nullable<global::System.Int64> _APIKeyId;
        partial void OnAPIKeyIdChanging(Nullable<global::System.Int64> value);
        partial void OnAPIKeyIdChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public Nullable<global::System.Int64> CityId
        {
            get
            {
                return _CityId;
            }
            set
            {
                OnCityIdChanging(value);
                ReportPropertyChanging("CityId");
                _CityId = StructuralObject.SetValidValue(value, "CityId");
                ReportPropertyChanged("CityId");
                OnCityIdChanged();
            }
        }
        private Nullable<global::System.Int64> _CityId;
        partial void OnCityIdChanging(Nullable<global::System.Int64> value);
        partial void OnCityIdChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public Nullable<global::System.Int64> IndustryId
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
        private Nullable<global::System.Int64> _IndustryId;
        partial void OnIndustryIdChanging(Nullable<global::System.Int64> value);
        partial void OnIndustryIdChanged();
    
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
        public global::System.String ReturnUrl
        {
            get
            {
                return _ReturnUrl;
            }
            set
            {
                OnReturnUrlChanging(value);
                ReportPropertyChanging("ReturnUrl");
                _ReturnUrl = StructuralObject.SetValidValue(value, false, "ReturnUrl");
                ReportPropertyChanged("ReturnUrl");
                OnReturnUrlChanged();
            }
        }
        private global::System.String _ReturnUrl;
        partial void OnReturnUrlChanging(global::System.String value);
        partial void OnReturnUrlChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.String Email
        {
            get
            {
                return _Email;
            }
            set
            {
                OnEmailChanging(value);
                ReportPropertyChanging("Email");
                _Email = StructuralObject.SetValidValue(value, false, "Email");
                ReportPropertyChanged("Email");
                OnEmailChanged();
            }
        }
        private global::System.String _Email;
        partial void OnEmailChanging(global::System.String value);
        partial void OnEmailChanged();

        #endregion

    }

    #endregion

}
