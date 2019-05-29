using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YzkSoftWare.DataModel
{
    /// <summary>
    /// 数据字段模型集合接口
    /// </summary>
    public interface IDataFieldModelCollection : IEnumerable<IDataFieldModel>
    {

        /// <summary>
        /// 获取指定索引处的字段模型
        /// index小于0或index大于集合数,引发异常OutIndexException
        /// </summary>
        /// <param name="index">索引</param>
        /// <returns>返回该索引处的字段模型</returns>
        IDataFieldModel this[int index] { get; }

        /// <summary>
        /// 获取指定名称的字段模型
        /// 集合中不存在该名称的字段,引发异常NotFoundItemException
        /// </summary>
        /// <param name="name">字段名称</param>
        /// <returns>返回该名称的字段模型</returns>
        IDataFieldModel this[string name] { get; }

        /// <summary>
        /// 判断指定名称的字段模型是否存在
        /// </summary>
        /// <param name="name">字段名称</param>
        /// <returns>如果存在名为name的字段模型则返回true,否则返回false</returns>
        bool HasField(string name);
    }

    public sealed class DataFieldModelCollection : List<IDataFieldModel>, IDataFieldModelCollection
    {

        IDataFieldModel IDataFieldModelCollection.this[int index]
        {
            get
            {
                if (index < 0 || index >= Count)
                {
                    throw new OutIndexException(index);
                }
                return this[index];
            }
        }

        IDataFieldModel IDataFieldModelCollection.this[string name]
        {
            get
            {
                var n = this.FirstOrDefault(f => string.Equals(name, f.Name));
                if (n == null)
                {
                    throw new NotFoundItemException(string.Format(LocalResource.DataFieldModelCollection_NotFound, name));
                }
                return n;
            }
        }

        bool IDataFieldModelCollection.HasField(string name)
        {
            return this.FirstOrDefault(f => string.Equals(name, f.Name)) != null;
        }

        IEnumerator<IDataFieldModel> IEnumerable<IDataFieldModel>.GetEnumerator()
        {
            return GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
