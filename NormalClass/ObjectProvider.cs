using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YzkSoftWare
{
    /// <summary>
    /// 对象容器
    /// </summary>
    /// <typeparam name="K">容器键类型</typeparam>
    /// <typeparam name="V">容器值类型</typeparam>
    public class ObjectProvider<K, V>
    {

        public ObjectProvider()
        {
            p_Container = new Dictionary<K, V>();
        }

        private IDictionary<K, V> p_Container;

        /// <summary>
        /// 替换元素之前调用
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        protected virtual void OnReplace(K key, V value)
        {

        }

        /// <summary>
        /// 添加元素之前调用
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        protected virtual void OnAdd(K key, V value)
        {

        }

        /// <summary>
        /// 移除元素之前调用
        /// </summary>
        /// <param name="key"></param>
        protected virtual void OnRemove(K key)
        {

        }

        /// <summary>
        /// 清空所有元素之前调用
        /// </summary>
        protected virtual void OnClear()
        {

        }

        /// <summary>
        /// 添加指定键的值:如果已存在指定键的值则不添加返回
        /// </summary>
        /// <param name="key">要添加的键</param>
        /// <param name="value">要添加的键的值</param>
        /// <returns>返回true指示成功添加,否否则指示已存在该键的值</returns>
        public bool AddValue(K key, V value)
        {
            if (!p_Container.ContainsKey(key))
            {
                OnAdd(key, value);
                p_Container.Add(key, value);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 移除指定键的值
        /// </summary>
        /// <param name="key">要移除值的键</param>
        /// <returns>如果成功移除则返回true,否则返回false</returns>
        public bool RemoveValue(K key)
        {
            OnRemove(key);
            return p_Container.Remove(key);
        }

        /// <summary>
        /// 清空所有键和值
        /// </summary>
        public void Clear()
        {
            OnClear();
            p_Container.Clear();
        }

        /// <summary>
        /// 当前容器元素的个数
        /// </summary>
        public int Count
        {
            get
            {
                return p_Container.Count;
            }
        }

        /// <summary>
        /// 添加或替换指定的键和值:如果已存在key的值则替换值为value,否则添加key和value
        /// </summary>
        /// <param name="key">要添加或替换的键</param>
        /// <param name="value">要添加或替换的值</param>
        public void AddOrReplaceValue(K key, V value)
        {
            if (!p_Container.ContainsKey(key))
            {
                OnAdd(key, value);
                p_Container.Add(key, value);
            }
            else
            {
                OnReplace(key, value);
                p_Container[key] = value;
            }
        }

        /// <summary>
        /// 获取指定键的值
        /// </summary>
        /// <param name="key">要获取值的键</param>
        /// <returns>如果存在key的值则返回该值,否则返回V的缺省值</returns>
        public V GetValue(K key)
        {
            if (p_Container.ContainsKey(key))
                return p_Container[key];
            return default(V);
        }

        /// <summary>
        /// 获取当前容器所有的键和值
        /// </summary>
        /// <returns>当前容器所有的键和值</returns>
        public IEnumerable<KeyValuePair<K, V>> GetKeyAndValues()
        {
            return p_Container.ToArray();
        }

        /// <summary>
        /// 获取当前容器所有的键
        /// </summary>
        /// <returns>当前容器所有的键</returns>
        public IEnumerable<K> GetKeys()
        {
            return p_Container.Keys.ToArray();
        }

        /// <summary>
        /// 获取当前容器所有的值
        /// </summary>
        /// <returns>当前容器所有的值</returns>
        public IEnumerable<V> GetValues()
        {
            return p_Container.Values.ToArray();
        }
    }

    //不支持泛型所以舍弃
    ///// <summary>
    ///// 线程安全的对象容器
    ///// </summary>
    ///// <typeparam name="K">容器键类型</typeparam>
    ///// <typeparam name="V">容器值类型</typeparam>
    //[System.Runtime.Remoting.Contexts.Synchronization]
    //public class ObjectProvider<K, V> : ContextBoundObject
    //{

    //    public ObjectProvider()
    //    {
    //        p_Container = new Dictionary<K, V>();
    //    }

    //    private IDictionary<K, V> p_Container;

    //    /// <summary>
    //    /// 替换元素之前调用
    //    /// </summary>
    //    /// <param name="key"></param>
    //    /// <param name="value"></param>
    //    protected virtual void OnReplace(K key, V value)
    //    {

    //    }

    //    /// <summary>
    //    /// 添加元素之前调用
    //    /// </summary>
    //    /// <param name="key"></param>
    //    /// <param name="value"></param>
    //    protected virtual void OnAdd(K key, V value)
    //    {

    //    }

    //    /// <summary>
    //    /// 移除元素之前调用
    //    /// </summary>
    //    /// <param name="key"></param>
    //    protected virtual void OnRemove(K key)
    //    {

    //    }

    //    /// <summary>
    //    /// 清空所有元素之前调用
    //    /// </summary>
    //    protected virtual void OnClear()
    //    {

    //    }

    //    /// <summary>
    //    /// 添加指定键的值:如果已存在指定键的值则不添加返回
    //    /// </summary>
    //    /// <param name="key">要添加的键</param>
    //    /// <param name="value">要添加的键的值</param>
    //    /// <returns>返回true指示成功添加,否否则指示已存在该键的值</returns>
    //    public bool AddValue(K key, V value)
    //    {
    //        if (!p_Container.ContainsKey(key))
    //        {
    //            OnAdd(key, value);
    //            p_Container.Add(key, value);
    //            return true;
    //        }
    //        return false;
    //    }

    //    /// <summary>
    //    /// 移除指定键的值
    //    /// </summary>
    //    /// <param name="key">要移除值的键</param>
    //    /// <returns>如果成功移除则返回true,否则返回false</returns>
    //    public bool RemoveValue(K key)
    //    {
    //        OnRemove(key);
    //        return p_Container.Remove(key);
    //    }

    //    /// <summary>
    //    /// 清空所有键和值
    //    /// </summary>
    //    public void Clear()
    //    {
    //        OnClear();
    //        p_Container.Clear();
    //    }

    //    /// <summary>
    //    /// 当前容器元素的个数
    //    /// </summary>
    //    public int Count
    //    {
    //        get
    //        {
    //            return p_Container.Count;
    //        }
    //    }

    //    /// <summary>
    //    /// 添加或替换指定的键和值:如果已存在key的值则替换值为value,否则添加key和value
    //    /// </summary>
    //    /// <param name="key">要添加或替换的键</param>
    //    /// <param name="value">要添加或替换的值</param>
    //    public void AddOrReplaceValue(K key, V value)
    //    {
    //        if (!p_Container.ContainsKey(key))
    //        {
    //            OnAdd(key, value);
    //            p_Container.Add(key, value);
    //        }
    //        else
    //        {
    //            OnReplace(key, value);
    //            p_Container[key] = value;
    //        }
    //    }

    //    /// <summary>
    //    /// 获取指定键的值
    //    /// </summary>
    //    /// <param name="key">要获取值的键</param>
    //    /// <returns>如果存在key的值则返回该值,否则返回V的缺省值</returns>
    //    public V GetValue(K key)
    //    {
    //        if (p_Container.ContainsKey(key))
    //            return p_Container[key];
    //        return default(V);
    //    }

    //    /// <summary>
    //    /// 获取当前容器所有的键和值
    //    /// </summary>
    //    /// <returns>当前容器所有的键和值</returns>
    //    public IEnumerable<KeyValuePair<K, V>> GetKeyAndValues()
    //    {
    //        return p_Container.ToArray();
    //    }

    //    /// <summary>
    //    /// 获取当前容器所有的键
    //    /// </summary>
    //    /// <returns>当前容器所有的键</returns>
    //    public IEnumerable<K> GetKeys()
    //    {
    //        return p_Container.Keys.ToArray();
    //    }

    //    /// <summary>
    //    /// 获取当前容器所有的值
    //    /// </summary>
    //    /// <returns>当前容器所有的值</returns>
    //    public IEnumerable<V> GetValues()
    //    {
    //        return p_Container.Values.ToArray();
    //    }
    //}
}
