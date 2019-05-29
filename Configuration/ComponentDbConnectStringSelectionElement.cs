using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YzkSoftWare.Configuration
{

    /// <summary>
    /// 组件数据库适配器定义节
    /// </summary>
    public sealed class ComponentDbConnectStringSelectionElement : ConfigurationElement
    {

        [ConfigurationProperty("componentId")]
        public string ComponentId
        {
            get { return (string)this["componentId"]; }
            set { this["componentId"] = value; }
        }

        [ConfigurationProperty("connectString")]
        public string ConnectString
        {
            get { return (string)this["connectString"]; }
            set { this["connectString"] = value; }
        }

        [ConfigurationProperty("dabaseType", IsRequired = true)]
        public string DabaseType
        {
            get { return (string)this["dabaseType"]; }
            set { this["dabaseType"] = value; }
        }

        [ConfigurationProperty("masterConnectString")]
        public string MasterConnectString
        {
            get { return (string)this["masterConnectString"]; }
            set { this["masterConnectString"] = value; }
        }
    }

    /// <summary>
    /// 组件数据库适配器集合定义节
    /// </summary>
    public class ComponentDbConnectStringCollection : ConfigurationElementCollection
    {

        protected override ConfigurationElement CreateNewElement()
        {
            return new ComponentDbConnectStringSelectionElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ComponentDbConnectStringSelectionElement)element).ComponentId;
        }

        public override ConfigurationElementCollectionType CollectionType
        {
            get
            {
                return ConfigurationElementCollectionType.AddRemoveClearMap;
            }
        }
    }

    /// <summary>
    /// 应用组件数据库适配器定义节
    /// </summary>
    public class AppComponentDatabaseAdptor : ConfigurationElement
    {

        [ConfigurationProperty("appName")]
        public string AppName
        {
            get { return (string)this["appName"]; }
            set { this["appName"] = value; }
        }

        [ConfigurationProperty("componentDatabaeAdptors", IsRequired = true)]
        public ComponentDbConnectStringCollection ComponentDatabaeAdptors
        {
            get { return (ComponentDbConnectStringCollection)this["componentDatabaeAdptors"]; }
            set { this["componentDatabaeAdptors"] = value; }
        }
    }

    /// <summary>
    /// 应用组件数据库适配器定义集合
    /// </summary>
    public class AppComponentDatabaseAdptorCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new AppComponentDatabaseAdptor();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((AppComponentDatabaseAdptor)element).AppName;
        }

        public override ConfigurationElementCollectionType CollectionType
        {
            get
            {
                return ConfigurationElementCollectionType.AddRemoveClearMap;
            }
        }
    }

    /// <summary>
    /// 应用组件数据库适配器定义
    /// </summary>
    public class AppComponentDatabaseAdptorConfiguration : ConfigurationSection
    {
        /// <summary>
        /// 缺省数据库适配器
        /// 所有没有特别指定的组件都使用该数据库适配器
        /// </summary>
        [ConfigurationProperty("defaultAppConnectString", IsRequired = true)]
        public ComponentDbConnectStringSelectionElement DefaultAppConnectString
        {
            get { return (ComponentDbConnectStringSelectionElement)this["defaultAppConnectString"]; }
            set { this["defaultAppConnectString"] = value; }
        }

        /// <summary>
        /// 应用的数据库适配器定义集合
        /// </summary>
        [ConfigurationProperty("dbs")]
        public AppComponentDatabaseAdptorCollection Dbs
        {
            get { return this["dbs"] as AppComponentDatabaseAdptorCollection; }
        }
    }
}
