using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YzkSoftWare.DataModel
{
    internal sealed class DataModelBuffer
    {

        static DataModelBuffer()
        {
            Current = new DataModelBuffer();
        }

        private System.Collections.Concurrent.ConcurrentDictionary<Type, IDataModel> p_Bag = new System.Collections.Concurrent.ConcurrentDictionary<Type, IDataModel>();

        private void CheckType(Type type)
        {
            if (type == null)
                throw new ObjectIsNullException("type");
            if (!type.IsClass)
                throw new NotSupportException(ErrorCodes.UnkownError, string.Format(LocalResource.ModelType_NotIsClass, type.FullName));
            if (type.IsAbstract)
                throw new NotSupportException(ErrorCodes.UnkownError, string.Format(LocalResource.ModelType_Error_IsAbstract, type.FullName));
            if(type.IsPointer)
                throw new NotSupportException(ErrorCodes.UnkownError, string.Format(LocalResource.ModelType_Error_IsPointer, type.FullName));
            if (type.IsGenericType && !type.IsGenericTypeDefinition)
                throw new NotSupportException(ErrorCodes.UnkownError, string.Format(LocalResource.ModelType_Error_IsValidGenericType, type.FullName));
            if (type.IsArray)
                throw new NotSupportException(ErrorCodes.UnkownError, string.Format(LocalResource.ModelType_Error_IsArray, type.FullName));
            if (type.IsInterface)
                throw new NotSupportException(ErrorCodes.UnkownError, string.Format(LocalResource.ModelType_IsInterface, type.FullName));
        }

        public IDataModel CreateModel(Type type)
        {
            CheckType(type);
            return new TypeDataModel(type);
        }

        public IDataModel GetModel(Type type)
        {
            return p_Bag.GetOrAdd(type, f => CreateModel(f));
        }

        public IDataModel GetModel<T>()
        {
            return GetModel(typeof(T));
        }

        internal static DataModelBuffer Current { get; private set; }
    }
}
