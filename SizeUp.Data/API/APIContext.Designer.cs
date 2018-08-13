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
#region EDM Relationship Metadata

[assembly: EdmRelationshipAttribute("SizeUp.Data.API", "FK_APIKeyDomain_APIKey", "APIKey", System.Data.Metadata.Edm.RelationshipMultiplicity.One, typeof(SizeUp.Data.API.APIKey), "APIKeyDomain", System.Data.Metadata.Edm.RelationshipMultiplicity.Many, typeof(SizeUp.Data.API.APIKeyDomain), true)]
[assembly: EdmRelationshipAttribute("SizeUp.Data.API", "FK_APIKeyRoleMapping_APIKey", "APIKey", System.Data.Metadata.Edm.RelationshipMultiplicity.One, typeof(SizeUp.Data.API.APIKey), "APIKeyRoleMapping", System.Data.Metadata.Edm.RelationshipMultiplicity.Many, typeof(SizeUp.Data.API.APIKeyRoleMapping), true)]
[assembly: EdmRelationshipAttribute("SizeUp.Data.API", "FK_APIKeyRoleMapping_Role", "Role", System.Data.Metadata.Edm.RelationshipMultiplicity.One, typeof(SizeUp.Data.API.Role), "APIKeyRoleMapping", System.Data.Metadata.Edm.RelationshipMultiplicity.Many, typeof(SizeUp.Data.API.APIKeyRoleMapping), true)]
[assembly: EdmRelationshipAttribute("SizeUp.Data.API", "FK_IdentityProvider_APIKey", "APIKey", System.Data.Metadata.Edm.RelationshipMultiplicity.One, typeof(SizeUp.Data.API.APIKey), "IdentityProvider", System.Data.Metadata.Edm.RelationshipMultiplicity.Many, typeof(SizeUp.Data.API.IdentityProvider), true)]

#endregion

namespace SizeUp.Data.API
{
    #region Contexts
    
    /// <summary>
    /// No Metadata Documentation available.
    /// </summary>
    public partial class APIContext : ObjectContext
    {
        #region Constructors
    
        /// <summary>
        /// Initializes a new APIContext object using the connection string found in the 'APIContext' section of the application configuration file.
        /// </summary>
        public APIContext() : base("name=APIContext", "APIContext")
        {
            this.ContextOptions.LazyLoadingEnabled = true;
            OnContextCreated();
        }
    
        /// <summary>
        /// Initialize a new APIContext object.
        /// </summary>
        public APIContext(string connectionString) : base(connectionString, "APIContext")
        {
            this.ContextOptions.LazyLoadingEnabled = true;
            OnContextCreated();
        }
    
        /// <summary>
        /// Initialize a new APIContext object.
        /// </summary>
        public APIContext(EntityConnection connection) : base(connection, "APIContext")
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
        public ObjectSet<APIKey> APIKeys
        {
            get
            {
                if ((_APIKeys == null))
                {
                    _APIKeys = base.CreateObjectSet<APIKey>("APIKeys");
                }
                return _APIKeys;
            }
        }
        private ObjectSet<APIKey> _APIKeys;
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        public ObjectSet<APIKeyDomain> APIKeyDomains
        {
            get
            {
                if ((_APIKeyDomains == null))
                {
                    _APIKeyDomains = base.CreateObjectSet<APIKeyDomain>("APIKeyDomains");
                }
                return _APIKeyDomains;
            }
        }
        private ObjectSet<APIKeyDomain> _APIKeyDomains;
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        public ObjectSet<APIKeyRoleMapping> APIKeyRoleMappings
        {
            get
            {
                if ((_APIKeyRoleMappings == null))
                {
                    _APIKeyRoleMappings = base.CreateObjectSet<APIKeyRoleMapping>("APIKeyRoleMappings");
                }
                return _APIKeyRoleMappings;
            }
        }
        private ObjectSet<APIKeyRoleMapping> _APIKeyRoleMappings;
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        public ObjectSet<Role> Roles
        {
            get
            {
                if ((_Roles == null))
                {
                    _Roles = base.CreateObjectSet<Role>("Roles");
                }
                return _Roles;
            }
        }
        private ObjectSet<Role> _Roles;
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        public ObjectSet<IdentityProvider> IdentityProviders
        {
            get
            {
                if ((_IdentityProviders == null))
                {
                    _IdentityProviders = base.CreateObjectSet<IdentityProvider>("IdentityProviders");
                }
                return _IdentityProviders;
            }
        }
        private ObjectSet<IdentityProvider> _IdentityProviders;

        #endregion

        #region AddTo Methods
    
        /// <summary>
        /// Deprecated Method for adding a new object to the APIKeys EntitySet. Consider using the .Add method of the associated ObjectSet&lt;T&gt; property instead.
        /// </summary>
        public void AddToAPIKeys(APIKey aPIKey)
        {
            base.AddObject("APIKeys", aPIKey);
        }
    
        /// <summary>
        /// Deprecated Method for adding a new object to the APIKeyDomains EntitySet. Consider using the .Add method of the associated ObjectSet&lt;T&gt; property instead.
        /// </summary>
        public void AddToAPIKeyDomains(APIKeyDomain aPIKeyDomain)
        {
            base.AddObject("APIKeyDomains", aPIKeyDomain);
        }
    
        /// <summary>
        /// Deprecated Method for adding a new object to the APIKeyRoleMappings EntitySet. Consider using the .Add method of the associated ObjectSet&lt;T&gt; property instead.
        /// </summary>
        public void AddToAPIKeyRoleMappings(APIKeyRoleMapping aPIKeyRoleMapping)
        {
            base.AddObject("APIKeyRoleMappings", aPIKeyRoleMapping);
        }
    
        /// <summary>
        /// Deprecated Method for adding a new object to the Roles EntitySet. Consider using the .Add method of the associated ObjectSet&lt;T&gt; property instead.
        /// </summary>
        public void AddToRoles(Role role)
        {
            base.AddObject("Roles", role);
        }
    
        /// <summary>
        /// Deprecated Method for adding a new object to the IdentityProviders EntitySet. Consider using the .Add method of the associated ObjectSet&lt;T&gt; property instead.
        /// </summary>
        public void AddToIdentityProviders(IdentityProvider identityProvider)
        {
            base.AddObject("IdentityProviders", identityProvider);
        }

        #endregion

    }

    #endregion

    #region Entities
    
    /// <summary>
    /// No Metadata Documentation available.
    /// </summary>
    [EdmEntityTypeAttribute(NamespaceName="SizeUp.Data.API", Name="APIKey")]
    [Serializable()]
    [DataContractAttribute(IsReference=true)]
    public partial class APIKey : EntityObject
    {
        #region Factory Method
    
        /// <summary>
        /// Create a new APIKey object.
        /// </summary>
        /// <param name="id">Initial value of the Id property.</param>
        /// <param name="name">Initial value of the Name property.</param>
        /// <param name="keyValue">Initial value of the KeyValue property.</param>
        /// <param name="isActive">Initial value of the IsActive property.</param>
        public static APIKey CreateAPIKey(global::System.Int64 id, global::System.String name, global::System.Guid keyValue, global::System.Boolean isActive)
        {
            APIKey aPIKey = new APIKey();
            aPIKey.Id = id;
            aPIKey.Name = name;
            aPIKey.KeyValue = keyValue;
            aPIKey.IsActive = isActive;
            return aPIKey;
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
        public global::System.String Name
        {
            get
            {
                return _Name;
            }
            set
            {
                OnNameChanging(value);
                ReportPropertyChanging("Name");
                _Name = StructuralObject.SetValidValue(value, false, "Name");
                ReportPropertyChanged("Name");
                OnNameChanged();
            }
        }
        private global::System.String _Name;
        partial void OnNameChanging(global::System.String value);
        partial void OnNameChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.Guid KeyValue
        {
            get
            {
                return _KeyValue;
            }
            set
            {
                OnKeyValueChanging(value);
                ReportPropertyChanging("KeyValue");
                _KeyValue = StructuralObject.SetValidValue(value, "KeyValue");
                ReportPropertyChanged("KeyValue");
                OnKeyValueChanged();
            }
        }
        private global::System.Guid _KeyValue;
        partial void OnKeyValueChanging(global::System.Guid value);
        partial void OnKeyValueChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.Boolean IsActive
        {
            get
            {
                return _IsActive;
            }
            set
            {
                OnIsActiveChanging(value);
                ReportPropertyChanging("IsActive");
                _IsActive = StructuralObject.SetValidValue(value, "IsActive");
                ReportPropertyChanged("IsActive");
                OnIsActiveChanged();
            }
        }
        private global::System.Boolean _IsActive;
        partial void OnIsActiveChanging(global::System.Boolean value);
        partial void OnIsActiveChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public global::System.String UserName
        {
            get
            {
                return _UserName;
            }
            set
            {
                OnUserNameChanging(value);
                ReportPropertyChanging("UserName");
                _UserName = StructuralObject.SetValidValue(value, true, "UserName");
                ReportPropertyChanged("UserName");
                OnUserNameChanged();
            }
        }
        private global::System.String _UserName;
        partial void OnUserNameChanging(global::System.String value);
        partial void OnUserNameChanged();

        #endregion

        #region Navigation Properties
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [XmlIgnoreAttribute()]
        [SoapIgnoreAttribute()]
        [DataMemberAttribute()]
        [EdmRelationshipNavigationPropertyAttribute("SizeUp.Data.API", "FK_APIKeyDomain_APIKey", "APIKeyDomain")]
        public EntityCollection<APIKeyDomain> APIKeyDomains
        {
            get
            {
                return ((IEntityWithRelationships)this).RelationshipManager.GetRelatedCollection<APIKeyDomain>("SizeUp.Data.API.FK_APIKeyDomain_APIKey", "APIKeyDomain");
            }
            set
            {
                if ((value != null))
                {
                    ((IEntityWithRelationships)this).RelationshipManager.InitializeRelatedCollection<APIKeyDomain>("SizeUp.Data.API.FK_APIKeyDomain_APIKey", "APIKeyDomain", value);
                }
            }
        }
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [XmlIgnoreAttribute()]
        [SoapIgnoreAttribute()]
        [DataMemberAttribute()]
        [EdmRelationshipNavigationPropertyAttribute("SizeUp.Data.API", "FK_APIKeyRoleMapping_APIKey", "APIKeyRoleMapping")]
        public EntityCollection<APIKeyRoleMapping> APIKeyRoleMappings
        {
            get
            {
                return ((IEntityWithRelationships)this).RelationshipManager.GetRelatedCollection<APIKeyRoleMapping>("SizeUp.Data.API.FK_APIKeyRoleMapping_APIKey", "APIKeyRoleMapping");
            }
            set
            {
                if ((value != null))
                {
                    ((IEntityWithRelationships)this).RelationshipManager.InitializeRelatedCollection<APIKeyRoleMapping>("SizeUp.Data.API.FK_APIKeyRoleMapping_APIKey", "APIKeyRoleMapping", value);
                }
            }
        }
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [XmlIgnoreAttribute()]
        [SoapIgnoreAttribute()]
        [DataMemberAttribute()]
        [EdmRelationshipNavigationPropertyAttribute("SizeUp.Data.API", "FK_IdentityProvider_APIKey", "IdentityProvider")]
        public EntityCollection<IdentityProvider> IdentityProviders
        {
            get
            {
                return ((IEntityWithRelationships)this).RelationshipManager.GetRelatedCollection<IdentityProvider>("SizeUp.Data.API.FK_IdentityProvider_APIKey", "IdentityProvider");
            }
            set
            {
                if ((value != null))
                {
                    ((IEntityWithRelationships)this).RelationshipManager.InitializeRelatedCollection<IdentityProvider>("SizeUp.Data.API.FK_IdentityProvider_APIKey", "IdentityProvider", value);
                }
            }
        }

        #endregion

    }
    
    /// <summary>
    /// No Metadata Documentation available.
    /// </summary>
    [EdmEntityTypeAttribute(NamespaceName="SizeUp.Data.API", Name="APIKeyDomain")]
    [Serializable()]
    [DataContractAttribute(IsReference=true)]
    public partial class APIKeyDomain : EntityObject
    {
        #region Factory Method
    
        /// <summary>
        /// Create a new APIKeyDomain object.
        /// </summary>
        /// <param name="id">Initial value of the Id property.</param>
        /// <param name="domain">Initial value of the Domain property.</param>
        /// <param name="aPIKeyId">Initial value of the APIKeyId property.</param>
        public static APIKeyDomain CreateAPIKeyDomain(global::System.Int64 id, global::System.String domain, global::System.Int64 aPIKeyId)
        {
            APIKeyDomain aPIKeyDomain = new APIKeyDomain();
            aPIKeyDomain.Id = id;
            aPIKeyDomain.Domain = domain;
            aPIKeyDomain.APIKeyId = aPIKeyId;
            return aPIKeyDomain;
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
        public global::System.String Domain
        {
            get
            {
                return _Domain;
            }
            set
            {
                OnDomainChanging(value);
                ReportPropertyChanging("Domain");
                _Domain = StructuralObject.SetValidValue(value, false, "Domain");
                ReportPropertyChanged("Domain");
                OnDomainChanged();
            }
        }
        private global::System.String _Domain;
        partial void OnDomainChanging(global::System.String value);
        partial void OnDomainChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.Int64 APIKeyId
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
        private global::System.Int64 _APIKeyId;
        partial void OnAPIKeyIdChanging(global::System.Int64 value);
        partial void OnAPIKeyIdChanged();

        #endregion

        #region Navigation Properties
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [XmlIgnoreAttribute()]
        [SoapIgnoreAttribute()]
        [DataMemberAttribute()]
        [EdmRelationshipNavigationPropertyAttribute("SizeUp.Data.API", "FK_APIKeyDomain_APIKey", "APIKey")]
        public APIKey APIKey
        {
            get
            {
                return ((IEntityWithRelationships)this).RelationshipManager.GetRelatedReference<APIKey>("SizeUp.Data.API.FK_APIKeyDomain_APIKey", "APIKey").Value;
            }
            set
            {
                ((IEntityWithRelationships)this).RelationshipManager.GetRelatedReference<APIKey>("SizeUp.Data.API.FK_APIKeyDomain_APIKey", "APIKey").Value = value;
            }
        }
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [BrowsableAttribute(false)]
        [DataMemberAttribute()]
        public EntityReference<APIKey> APIKeyReference
        {
            get
            {
                return ((IEntityWithRelationships)this).RelationshipManager.GetRelatedReference<APIKey>("SizeUp.Data.API.FK_APIKeyDomain_APIKey", "APIKey");
            }
            set
            {
                if ((value != null))
                {
                    ((IEntityWithRelationships)this).RelationshipManager.InitializeRelatedReference<APIKey>("SizeUp.Data.API.FK_APIKeyDomain_APIKey", "APIKey", value);
                }
            }
        }

        #endregion

    }
    
    /// <summary>
    /// No Metadata Documentation available.
    /// </summary>
    [EdmEntityTypeAttribute(NamespaceName="SizeUp.Data.API", Name="APIKeyRoleMapping")]
    [Serializable()]
    [DataContractAttribute(IsReference=true)]
    public partial class APIKeyRoleMapping : EntityObject
    {
        #region Factory Method
    
        /// <summary>
        /// Create a new APIKeyRoleMapping object.
        /// </summary>
        /// <param name="id">Initial value of the Id property.</param>
        /// <param name="aPIKeyId">Initial value of the APIKeyId property.</param>
        /// <param name="roleId">Initial value of the RoleId property.</param>
        public static APIKeyRoleMapping CreateAPIKeyRoleMapping(global::System.Int64 id, global::System.Int64 aPIKeyId, global::System.Int64 roleId)
        {
            APIKeyRoleMapping aPIKeyRoleMapping = new APIKeyRoleMapping();
            aPIKeyRoleMapping.Id = id;
            aPIKeyRoleMapping.APIKeyId = aPIKeyId;
            aPIKeyRoleMapping.RoleId = roleId;
            return aPIKeyRoleMapping;
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
        public global::System.Int64 APIKeyId
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
        private global::System.Int64 _APIKeyId;
        partial void OnAPIKeyIdChanging(global::System.Int64 value);
        partial void OnAPIKeyIdChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.Int64 RoleId
        {
            get
            {
                return _RoleId;
            }
            set
            {
                OnRoleIdChanging(value);
                ReportPropertyChanging("RoleId");
                _RoleId = StructuralObject.SetValidValue(value, "RoleId");
                ReportPropertyChanged("RoleId");
                OnRoleIdChanged();
            }
        }
        private global::System.Int64 _RoleId;
        partial void OnRoleIdChanging(global::System.Int64 value);
        partial void OnRoleIdChanged();

        #endregion

        #region Navigation Properties
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [XmlIgnoreAttribute()]
        [SoapIgnoreAttribute()]
        [DataMemberAttribute()]
        [EdmRelationshipNavigationPropertyAttribute("SizeUp.Data.API", "FK_APIKeyRoleMapping_APIKey", "APIKey")]
        public APIKey APIKey
        {
            get
            {
                return ((IEntityWithRelationships)this).RelationshipManager.GetRelatedReference<APIKey>("SizeUp.Data.API.FK_APIKeyRoleMapping_APIKey", "APIKey").Value;
            }
            set
            {
                ((IEntityWithRelationships)this).RelationshipManager.GetRelatedReference<APIKey>("SizeUp.Data.API.FK_APIKeyRoleMapping_APIKey", "APIKey").Value = value;
            }
        }
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [BrowsableAttribute(false)]
        [DataMemberAttribute()]
        public EntityReference<APIKey> APIKeyReference
        {
            get
            {
                return ((IEntityWithRelationships)this).RelationshipManager.GetRelatedReference<APIKey>("SizeUp.Data.API.FK_APIKeyRoleMapping_APIKey", "APIKey");
            }
            set
            {
                if ((value != null))
                {
                    ((IEntityWithRelationships)this).RelationshipManager.InitializeRelatedReference<APIKey>("SizeUp.Data.API.FK_APIKeyRoleMapping_APIKey", "APIKey", value);
                }
            }
        }
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [XmlIgnoreAttribute()]
        [SoapIgnoreAttribute()]
        [DataMemberAttribute()]
        [EdmRelationshipNavigationPropertyAttribute("SizeUp.Data.API", "FK_APIKeyRoleMapping_Role", "Role")]
        public Role Role
        {
            get
            {
                return ((IEntityWithRelationships)this).RelationshipManager.GetRelatedReference<Role>("SizeUp.Data.API.FK_APIKeyRoleMapping_Role", "Role").Value;
            }
            set
            {
                ((IEntityWithRelationships)this).RelationshipManager.GetRelatedReference<Role>("SizeUp.Data.API.FK_APIKeyRoleMapping_Role", "Role").Value = value;
            }
        }
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [BrowsableAttribute(false)]
        [DataMemberAttribute()]
        public EntityReference<Role> RoleReference
        {
            get
            {
                return ((IEntityWithRelationships)this).RelationshipManager.GetRelatedReference<Role>("SizeUp.Data.API.FK_APIKeyRoleMapping_Role", "Role");
            }
            set
            {
                if ((value != null))
                {
                    ((IEntityWithRelationships)this).RelationshipManager.InitializeRelatedReference<Role>("SizeUp.Data.API.FK_APIKeyRoleMapping_Role", "Role", value);
                }
            }
        }

        #endregion

    }
    
    /// <summary>
    /// No Metadata Documentation available.
    /// </summary>
    [EdmEntityTypeAttribute(NamespaceName="SizeUp.Data.API", Name="IdentityProvider")]
    [Serializable()]
    [DataContractAttribute(IsReference=true)]
    public partial class IdentityProvider : EntityObject
    {
        #region Factory Method
    
        /// <summary>
        /// Create a new IdentityProvider object.
        /// </summary>
        /// <param name="id">Initial value of the Id property.</param>
        /// <param name="aPIKeyId">Initial value of the APIKeyId property.</param>
        /// <param name="entryPoint">Initial value of the EntryPoint property.</param>
        public static IdentityProvider CreateIdentityProvider(global::System.Int64 id, global::System.Int64 aPIKeyId, global::System.String entryPoint)
        {
            IdentityProvider identityProvider = new IdentityProvider();
            identityProvider.Id = id;
            identityProvider.APIKeyId = aPIKeyId;
            identityProvider.EntryPoint = entryPoint;
            return identityProvider;
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
        public global::System.Int64 APIKeyId
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
        private global::System.Int64 _APIKeyId;
        partial void OnAPIKeyIdChanging(global::System.Int64 value);
        partial void OnAPIKeyIdChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.String EntryPoint
        {
            get
            {
                return _EntryPoint;
            }
            set
            {
                OnEntryPointChanging(value);
                ReportPropertyChanging("EntryPoint");
                _EntryPoint = StructuralObject.SetValidValue(value, false, "EntryPoint");
                ReportPropertyChanged("EntryPoint");
                OnEntryPointChanged();
            }
        }
        private global::System.String _EntryPoint;
        partial void OnEntryPointChanging(global::System.String value);
        partial void OnEntryPointChanged();

        #endregion

        #region Navigation Properties
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [XmlIgnoreAttribute()]
        [SoapIgnoreAttribute()]
        [DataMemberAttribute()]
        [EdmRelationshipNavigationPropertyAttribute("SizeUp.Data.API", "FK_IdentityProvider_APIKey", "APIKey")]
        public APIKey APIKey
        {
            get
            {
                return ((IEntityWithRelationships)this).RelationshipManager.GetRelatedReference<APIKey>("SizeUp.Data.API.FK_IdentityProvider_APIKey", "APIKey").Value;
            }
            set
            {
                ((IEntityWithRelationships)this).RelationshipManager.GetRelatedReference<APIKey>("SizeUp.Data.API.FK_IdentityProvider_APIKey", "APIKey").Value = value;
            }
        }
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [BrowsableAttribute(false)]
        [DataMemberAttribute()]
        public EntityReference<APIKey> APIKeyReference
        {
            get
            {
                return ((IEntityWithRelationships)this).RelationshipManager.GetRelatedReference<APIKey>("SizeUp.Data.API.FK_IdentityProvider_APIKey", "APIKey");
            }
            set
            {
                if ((value != null))
                {
                    ((IEntityWithRelationships)this).RelationshipManager.InitializeRelatedReference<APIKey>("SizeUp.Data.API.FK_IdentityProvider_APIKey", "APIKey", value);
                }
            }
        }

        #endregion

    }
    
    /// <summary>
    /// No Metadata Documentation available.
    /// </summary>
    [EdmEntityTypeAttribute(NamespaceName="SizeUp.Data.API", Name="Role")]
    [Serializable()]
    [DataContractAttribute(IsReference=true)]
    public partial class Role : EntityObject
    {
        #region Factory Method
    
        /// <summary>
        /// Create a new Role object.
        /// </summary>
        /// <param name="id">Initial value of the Id property.</param>
        /// <param name="name">Initial value of the Name property.</param>
        public static Role CreateRole(global::System.Int64 id, global::System.String name)
        {
            Role role = new Role();
            role.Id = id;
            role.Name = name;
            return role;
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
        public global::System.String Name
        {
            get
            {
                return _Name;
            }
            set
            {
                OnNameChanging(value);
                ReportPropertyChanging("Name");
                _Name = StructuralObject.SetValidValue(value, false, "Name");
                ReportPropertyChanged("Name");
                OnNameChanged();
            }
        }
        private global::System.String _Name;
        partial void OnNameChanging(global::System.String value);
        partial void OnNameChanged();

        #endregion

        #region Navigation Properties
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [XmlIgnoreAttribute()]
        [SoapIgnoreAttribute()]
        [DataMemberAttribute()]
        [EdmRelationshipNavigationPropertyAttribute("SizeUp.Data.API", "FK_APIKeyRoleMapping_Role", "APIKeyRoleMapping")]
        public EntityCollection<APIKeyRoleMapping> APIKeyRoleMappings
        {
            get
            {
                return ((IEntityWithRelationships)this).RelationshipManager.GetRelatedCollection<APIKeyRoleMapping>("SizeUp.Data.API.FK_APIKeyRoleMapping_Role", "APIKeyRoleMapping");
            }
            set
            {
                if ((value != null))
                {
                    ((IEntityWithRelationships)this).RelationshipManager.InitializeRelatedCollection<APIKeyRoleMapping>("SizeUp.Data.API.FK_APIKeyRoleMapping_Role", "APIKeyRoleMapping", value);
                }
            }
        }

        #endregion

    }

    #endregion

}
