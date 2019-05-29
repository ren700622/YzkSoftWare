using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YzkSoftWare
{
    public abstract class TypeSetAttribute : Attribute
    {

        protected TypeSetAttribute(Type typevalue)
        {
            TypeValue = typevalue;
        }

        public Type TypeValue { get; private set; }

    }
}
