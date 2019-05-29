using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YzkSoftWare.DataService
{
    /// <summary>
    /// 应用服务
    /// </summary>
    public interface IAppService : IDisposable
    {
        /// <summary>
        /// 当前应用名称
        /// </summary>
        string AppName { get; set; }

    }

    /// <summary>
    /// 应用服务基础类
    /// </summary>
    public abstract class AppServiceBase : IAppService
    {

        public string AppName { get; set; }

        public virtual void Dispose()
        {
        }
    }
}
