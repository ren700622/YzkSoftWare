using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YzkSoftWare.Data.DataService
{
    public static class NormalServiceHelper
    {

        /// <summary>
        /// 判断自身父子类型中指定的child是否是sourceid的子级
        /// </summary>
        /// <param name="modalType"></param>
        /// <param name="sourceid"></param>
        /// <param name="childid"></param>
        /// <returns></returns>
        public static bool IsChildId(Type modalType, long sourceid, long childid, string parentfieldid, IDataSelector view)
        {
            if (sourceid == childid) return false;
            object filter = modalType.CreateObject();
            List<string> parametnames = new List<string>();
            parametnames.Add(parentfieldid);
            List<object> parvalues = new List<object>();
            parvalues.Add(sourceid);
            var softdelkey = modalType.GetDataModel().GetSoftDeleteFieldName();
            if (!string.IsNullOrEmpty(softdelkey))
            {
                parametnames.Add(softdelkey);
                parvalues.Add(false);
            }
            var l = view.GetDataListByFieldEqValue(modalType, parametnames, parvalues, new string[] { "Id", parentfieldid });
            if (l != null)
            {
                foreach (object o in l)
                {
                    long id = (long)o.GetMemberValue("Id");
                    if (id == childid) return true;
                    if (IsChildId(modalType, id, childid, parentfieldid, view)) return true;
                }
            }
            return false;
        }

    }
}
