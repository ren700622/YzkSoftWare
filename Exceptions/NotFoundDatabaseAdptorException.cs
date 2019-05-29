using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YzkSoftWare.Exceptions
{
    public class NotFoundDatabaseAdptorException : Exception
    {

        public NotFoundDatabaseAdptorException(string componentId) : base(string.Format(LocalResource.NotFoundDatabaseAdptorException, componentId)) { }

    }
}
