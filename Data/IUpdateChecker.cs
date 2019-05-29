using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YzkSoftWare.Data.DataService;
using YzkSoftWare.Data.Sql;
using YzkSoftWare.DataModel;

namespace YzkSoftWare.Data
{
    /// <summary>
    /// 数据更新检查接口
    /// </summary>
    public interface IUpdateChecker
    {

        /// <summary>
        /// 检查错误
        /// </summary>
        /// <param name="context">当前要提交的更新数据</param>
        /// <param name="grouptocommit">一起提交的更新数据</param>
        /// <param name="dataSelector">数据选择器</param>
        /// <returns></returns>
        Exception CheckErrors(IUpdateContext context, IEnumerable<IUpdateContext> grouptocommit, IDataSelector dataSelector);

    }

    /// <summary>
    /// 数据更新检查接口的缺省实现类
    /// </summary>
    public class DefaultUpdateChceker : IUpdateChecker
    {
        /// <summary>
        /// 当前更新项
        /// </summary>
        protected IUpdateContext CurrentContext { get; private set; }
        /// <summary>
        /// 与当前项一起提交的更新项
        /// </summary>
        protected IEnumerable<IUpdateContext> CurrentGroups { get; private set; }
        /// <summary>
        /// 当前数据选择器
        /// </summary>
        protected IDataSelector CurrentSelector { get; private set; }
        /// <summary>
        /// 检查未知跟新上下文,缺省实现返回null
        /// </summary>
        /// <returns></returns>
        protected virtual Exception CheckUnkownUpdateContext()
        {
            return null;
        }
        /// <summary>
        /// 检查添加
        /// </summary>
        /// <returns></returns>
        protected virtual Exception CheckAddNew()
        {
            List<DataModalFieldException> errs = new List<DataModalFieldException>();
            DefaultCheckAddOrUpdate(errs);
            if(errs.Count>0)
                return new DataModalFieldCollectionException(errs);
            return null;
        }
        /// <summary>
        /// 检查修改
        /// </summary>
        /// <returns></returns>
        protected virtual Exception CheckUpdate()
        {
            List<DataModalFieldException> errs = new List<DataModalFieldException>();
            DefaultCheckAddOrUpdate(errs);
            if (errs.Count > 0)
                return new DataModalFieldCollectionException(errs);
            return null;
        }
        /// <summary>
        /// 检查删除
        /// </summary>
        /// <returns></returns>
        protected virtual Exception CheckDelete()
        {
            List<DataModalFieldException> errs = new List<DataModalFieldException>();
            DefaultCheckDelete(errs);
            if (errs.Count > 0)
                return new DataModalFieldCollectionException(errs);
            return null;
        }

        /// <summary>
        /// 从Type中搜索特性T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type"></param>
        /// <returns></returns>
        protected IEnumerable<T> SelectAttributesFromType<T>(Type type)
           where T : Attribute
        {
            if (Attribute.IsDefined(type, typeof(T), true))
            {
                return type.GetCustomAttributes(typeof(T), true).Select<object, T>(f => f as T).AsEnumerable();
            }
            return null;
        }

        /// <summary>
        /// 删除时检测被引用的数据
        /// </summary>
        /// <param name="dm"></param>
        /// <param name="modalcontext"></param>
        /// <param name="errors"></param>
        /// <param name="view"></param>
        private void CheckDeleteRefence(
            IDataModel dm,
            IDataUpdateContext modalcontext,
            IList<DataModalFieldException> errors)
        {
            if (modalcontext.ModalState != ModalState.Delete) return;
            IEnumerable<DeleteCheckRefenceAttribute> x = SelectAttributesFromType<DeleteCheckRefenceAttribute>(modalcontext.ModalType);
            if (x != null)
            {
                foreach (DeleteCheckRefenceAttribute attr in x)
                {
                    DeleteCheckRefenceAttribute y = attr as DeleteCheckRefenceAttribute;
                    if (y.IsChild) continue;
                    long fid;
                    if (CheckDeleteRefenceExists(dm, modalcontext, y,out fid))
                    {
                        var dm2 = y.CheckModalType != null ? y.CheckModalType.GetDataModel() : dm;
                        string note = string.Format(LocalResource.RecordIsRefencedError, dm.DisplayName, dm2.DisplayName, fid);
                        DataModalFieldIsOnRefencedException b = new DataModalFieldIsOnRefencedException("Id", note);
                        errors.Add(b);
                    }
                }
            }
        }

        /// <summary>
        /// 检测被删除的数据是否存在引用它的数据
        /// </summary>
        /// <param name="dm"></param>
        /// <param name="modalcontext"></param>
        /// <param name="d"></param>
        /// <param name="view"></param>
        /// <returns></returns>
        private bool CheckDeleteRefenceExists(
            IDataModel dm,
            IDataUpdateContext modalcontext,
            DeleteCheckRefenceAttribute d,
            out long fid)
        {
            Type rd = d.CheckModalType;
            if (rd == null)
                rd = modalcontext.ModalType;
            fid = (long)modalcontext.Data.GetPropertyValue("Id");
            List<string> filternames = new List<string>();
            List<object> filtervalues = new List<object>();
            filternames.Add(d.RefenceFieldName);
            filtervalues.Add(fid);
            string softkey = rd.GetDataModel().GetSoftDeleteFieldName();
            if (!string.IsNullOrEmpty(softkey))
            {
                filternames.Add(softkey);
                filtervalues.Add(false);
            }
            return CurrentSelector.GetDataByFieldEqValue(rd, filternames, filtervalues, new string[] { "Id" }) != null;
        }

        /// <summary>
        /// 是否忽略检查字段
        /// </summary>
        /// <param name="field"></param>
        /// <param name="rangle"></param>
        /// <param name="ranglefieldnames"></param>
        /// <returns></returns>
        private bool IsIgnoreCheckField(
            IDataFieldModel field,
            FieldSelectRange rangle,
            IEnumerable<string> ranglefieldnames)
        {
            if (field == null) return true;
            if (field.IsRowVersion()) return true;
            if (field.IsSoftDeleteField()) return true;
            if (field.GetDbGeneratedDefine() != null) return true;
            switch (rangle)
            {
                case FieldSelectRange.IgnoreFields: //忽略指定的字段
                    {
                        //指定字段是否是忽略的字段
                        return ranglefieldnames != null && ranglefieldnames.FirstOrDefault(f => string.Equals(f, field.Name, StringComparison.OrdinalIgnoreCase)) != null;
                    }
                case FieldSelectRange.OnlyFields: //仅检测指定的字段
                    {
                        //指定的字段是否在指定字段中
                        //如果不在则返回true,否则返回false
                        if (ranglefieldnames == null)
                            return true;
                        return ranglefieldnames.FirstOrDefault(f => string.Equals(f, field.Name, StringComparison.OrdinalIgnoreCase)) == null;
                    }
            }
            return false;
        }

        /// <summary>
        /// 对新增和修改的检测
        /// 1、是否可为空
        /// 2、字符长度
        /// 3、数字范围
        /// </summary>
        /// <param name="datamoudle"></param>
        /// <param name="f"></param>
        /// <param name="v"></param>
        /// <param name="errs"></param>
        private void CheckAddOrUpdateFieldValue(
            IDataModel datamoudle,
            IDataFieldModel f,
            object v,
            IList<DataModalFieldException> errs)
        {
            if(!DataValueParase.PassNullValue(f,v))
            {
                errs.Add(ErrorCreateHelper.CreateFieldIsNullError(f.Name, f.Title));
                return;
            }
            if (f.FieldDbType == System.Data.DbType.String)
            {
                //字符是否超出最大长度
                int mx = f.GetFieldSize();
                //等于int.maxvalue则不限制字符大小
                if (mx != int.MaxValue)
                {
                    mx = mx * 2; //字符类型在数据库存储均使用nvarchar类型(Unicode:每个字符占两字节)
                    string vv = (string)v;
                    if (!string.IsNullOrEmpty(vv))
                    {
                        int sl = System.Text.Encoding.Unicode.GetByteCount(vv);
                        if (sl > mx)
                        {
                            errs.Add(ErrorCreateHelper.CreateFieldValueOutOfRangeError(f.Name, f.Title, mx));
                            return;
                        }
                    }
                }
            }
            else
            {
                //数字是否超出范围
                Type fieldType = f.GetClrType();
                //数字是否超出范围
                NumberType nt = DataValueParase.IsNumberType(fieldType);
                if ((nt != NumberType.Unkown) || (fieldType == typeof(DateTime) || fieldType == typeof(TimeSpan)))
                {
                    string exp = f.GetMinMaxExpress();
                    if (!string.IsNullOrWhiteSpace(exp) && !DataValueParase.IsDbNullValue(v))
                    {
                        if (!MinMaxValueHelper.ValueIsInRangle(exp, v))
                        {
                            errs.Add(ErrorCreateHelper.CreateFieldValueOutOfRangeError(f.Name, f.Title, exp));
                            return;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 判断指定字段值是否全部为null
        /// </summary>
        /// <param name="v"></param>
        /// <param name="names"></param>
        /// <returns></returns>
        private bool IsAllNullValue(object v, IEnumerable<string> names, IDataModel dm)
        {
            foreach (var str in names)
                if (dm.Fields[str].GetModelFieldValue(v) != null) return false;
            return true;
        }

        private void AddOnlyValueErrors(IEnumerable<string> xx,IDataModel dm, IList<DataModalFieldException> errs)
        {
            foreach (string k in xx)
            {
                if (errs.FirstOrDefault(f => string.Equals(f.FieldName, k, StringComparison.OrdinalIgnoreCase)) != null) continue;
                string display = k;
                var sss = dm.Fields[k];
                if (sss != null)
                    display = sss.Title;
                DataModalFieldIsRepeateException e = new DataModalFieldIsRepeateException(k, display);
                errs.Add(e);
            }
        }

        /// <summary>
        /// 检测唯一规则
        /// </summary>
        /// <param name="view"></param>
        /// <param name="dm"></param>
        /// <param name="onlyattr"></param>
        /// <param name="uponlyfields"></param>
        /// <param name="modalcontext"></param>
        /// <param name="errs"></param>
        private void CheckOnlyValues(
            IDataModel dm,
            OnlyValueKeysAttribute onlyattr,
            IEnumerable<string> uponlyfields,
            IDataUpdateContext modalcontext,
            IList<DataModalFieldException> errs)
        {
            if (((modalcontext.ModalState & ModalState.AddNew) == ModalState.AddNew
                    || (modalcontext.ModalState & ModalState.Update) == ModalState.Update)
                && onlyattr != null && onlyattr.OnlyKeys != null && onlyattr.OnlyKeys.Count() > 0
                && uponlyfields != null && uponlyfields.Count() > 0)
            {
                IDataSelector view = CurrentSelector;
                object data = modalcontext.Data;
                //要查询的参数名称
                List<string> parametnames = new List<string>(onlyattr.OnlyKeys);
                //构建查选参数
                object paramet = modalcontext.ModalType.CreateObject();
                switch (modalcontext.ModalState)
                {
                    case ModalState.AddNew:
                        {
                            //设置所有唯一值
                            foreach (var str in parametnames)
                            {
                                var f = dm.Fields[str];
                                f.SetModelFieldValue(paramet, f.GetModelFieldValue(data));
                            }
                            break;
                        }
                    case ModalState.Update:
                        {
                            paramet.SetPropertyValue("Id", data.GetPropertyValue("Id"));
                            object orgvalue = null;
                            foreach (var str in parametnames)
                            {
                                if (uponlyfields.FirstOrDefault(f => string.Equals(f, str, StringComparison.OrdinalIgnoreCase)) != null)
                                {
                                    //值已修改
                                    var f = dm.Fields[str];
                                    f.SetModelFieldValue(paramet, f.GetModelFieldValue(data));
                                }
                                else
                                {
                                    //值未修改
                                    //获取原值
                                    if (orgvalue == null)
                                        orgvalue = view.GetDataById(modalcontext.ModalType, null, (long)data.GetMemberValue("Id"));
                                    var f = dm.Fields[str];
                                    f.SetModelFieldValue(paramet, f.GetModelFieldValue(orgvalue));
                                }
                            }
                            break;
                        }
                }
                //全为null值时的策略
                if (!onlyattr.IsIncludeAllNull && IsAllNullValue(paramet, onlyattr.OnlyKeys,dm))
                    return;
                string softdelfieldname = dm.GetSoftDeleteFieldName();
                if (!string.IsNullOrEmpty(softdelfieldname))
                {
                    //该模型是软删除模型
                    //设置为未删除
                    if (!parametnames.Contains(softdelfieldname))
                        parametnames.Add(softdelfieldname);
                    dm.Fields[softdelfieldname].SetModelFieldValue(paramet, false);
                }

                List<string> filters = new List<string>();
                List<IDataParamet> filterpn = new List<IDataParamet>();
                foreach (var pn in parametnames)
                {
                    IDataParamet pv;
                    string f = QuerySqlFactory.CreateEqWhere(view.DbTypeName, pn, dm.Fields[pn].GetModelFieldValue(paramet), out pv);
                    if (!string.IsNullOrEmpty(f))
                        filters.Add(f);
                    if (pv != null)
                        filterpn.Add(pv);
                }

                switch (modalcontext.ModalState)
                {
                    case ModalState.Update:
                        {
                            //排除本身的条件
                            long id = (long)data.GetPropertyValue("Id");
                            IDataParamet pv;
                            string f = QuerySqlFactory.CreateNotEqWhere(view.DbTypeName, "Id", id, out pv);
                            if (!string.IsNullOrEmpty(f))
                                filters.Add(f);
                            if (pv != null)
                                filterpn.Add(pv);
                            break;
                        }
                }
                if (view.GetDataByAndFilterExpress(modalcontext.ModalType, filters, filterpn, new string[] { "Id" }) != null)
                    AddOnlyValueErrors(onlyattr.OnlyKeys, dm, errs);
            }
        }

        /// <summary>
        /// 检测数据中引用的Id是否存在
        /// </summary>
        /// <param name="dm"></param>
        /// <param name="modalcontext"></param>
        /// <param name="d"></param>
        /// <param name="view"></param>
        /// <returns></returns>
        private bool CheckDataIdRefenceIsExists(
            IDataModel dm,
            IDataUpdateContext modalcontext,
            DataIdRefenceAttribute d,
            out long rfid)
        {
            rfid = 0;
            IDataSelector view = CurrentSelector;
            //获取引用数据的Id值
            object refeceId = modalcontext.Data.GetMemberValue(d.SourceFieldName);
            if (refeceId != null && (refeceId is long || refeceId is long?))
            {
                long id = (long)refeceId;
                if (id > 0)
                {
                    rfid = id;
                    //创建引用数据类型的参数
                    Type refeeType = d.RefenceModalType != null ? d.RefenceModalType : modalcontext.ModalType;
                    List<string> eqfieldnames = new List<string>();
                    List<object> eqvalues = new List<object>();
                    eqfieldnames.Add("Id");
                    eqvalues.Add(id);
                    //是否是软删除模型
                    var refeeDm = refeeType.GetDataModel();
                    string softdeltekey = refeeDm.GetSoftDeleteFieldName();
                    if (!string.IsNullOrEmpty(softdeltekey))
                    {
                        eqfieldnames.Add(softdeltekey);
                        eqvalues.Add(false);
                    }

                    return view.GetDataByFieldEqValue(refeeType, eqfieldnames, eqvalues, new string[] { "Id" }) != null;

                }
            }
            //值为null,返回引用正常
            return true;
        }

        /// <summary>
        /// 判断自身父子引用的父级是否合法
        /// </summary>
        /// <param name="modalcontext"></param>
        /// <param name="d"></param>
        /// <param name="view"></param>
        /// <returns></returns>
        private bool IsValidOwnerParent(
            IDataUpdateContext modalcontext,
            DataIdRefenceAttribute d)
        {
            object pid = modalcontext.Data.GetPropertyValue(d.SourceFieldName);
            if (pid != null && pid is long && (long)pid > 0)
            {
                long id = (long)modalcontext.Data.GetPropertyValue("Id");
                if (id == (long)pid)
                    return false;
                if (NormalServiceHelper.IsChildId(modalcontext.ModalType, id, (long)pid, d.SourceFieldName, CurrentSelector))
                    return false;
            }
            return true;
        }

        /// <summary>
        /// 引用检测
        /// </summary>
        /// <param name="modalcontext"></param>
        /// <param name="errors"></param>
        /// <param name="view"></param>
        private void CheckDataIdRefrence(
            IDataModel dm,
            IDataUpdateContext modalcontext,
            IList<DataModalFieldException> errors)
        {
            //删除操作不检测
            if (modalcontext.ModalState == ModalState.Delete) return;
            IEnumerable<DataIdRefenceAttribute> attrs = SelectAttributesFromType<DataIdRefenceAttribute>(modalcontext.ModalType);
            if (attrs != null)
            {
                foreach (var attr in attrs)
                {
                    if (modalcontext.ModalState == ModalState.Update
                        && IsIgnoreCheckField(dm.Fields[attr.SourceFieldName], modalcontext.CheckRangle, modalcontext.CheckFieldNames))
                        continue; //忽略没有修改的字段
                    long rfid;
                    if (!CheckDataIdRefenceIsExists(dm, modalcontext, attr,out rfid))
                    {
                        DataModalFieldNotIsExistsException b = new DataModalFieldNotIsExistsException(attr.SourceFieldName, attr.RefenceModalType.GetDescription(), string.Format("Id={0}", rfid));
                        errors.Add(b);
                    }

                    //自身为父子引用的父级合法检测
                    if ((attr.RefenceModalType == null || attr.RefenceModalType == modalcontext.ModalType)
                        && attr.RefenceIsParent && modalcontext.ModalState == ModalState.Update)
                    {
                        if (!IsValidOwnerParent(modalcontext, attr))
                        {
                            DataModalFieldValidParentException b = new DataModalFieldValidParentException(attr.SourceFieldName);
                            errors.Add(b);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 缺省删除检查
        /// </summary>
        /// <param name="errs"></param>
        protected void DefaultCheckDelete(IList<DataModalFieldException> errs)
        {
             IDataUpdateContext modalcontext = CurrentContext as IDataUpdateContext;
            if (modalcontext.ModalType != null
                && modalcontext.Data != null
                && (modalcontext.ModalState & ModalState.Delete) == ModalState.Delete)
            {
                //检查引用
                CheckDataIdRefrence(modalcontext.ModalType.GetDataModel(), modalcontext, errs);
            }
        }

        /// <summary>
        /// 缺省添加或修改检查
        /// </summary>
        /// <param name="errs"></param>
        protected void DefaultCheckAddOrUpdate(IList<DataModalFieldException> errs)
        {
            IDataUpdateContext modalcontext = CurrentContext as IDataUpdateContext;
            if (modalcontext.ModalType != null
                && modalcontext.Data != null
                && ((modalcontext.ModalState & ModalState.AddNew) == ModalState.AddNew
                    || (modalcontext.ModalState & ModalState.Update) == ModalState.Update))
            {
                //检测范围
                FieldSelectRange rangle = modalcontext.CheckRangle;
                IEnumerable<string> ranglefieldnames = modalcontext.CheckFieldNames;
                //数据模型
                IDataModel dm = modalcontext.ModalType.GetDataModel();
                IEnumerable<IDataFieldModel> fields = dm.Fields;
                //组合值唯一规则
                IEnumerable<OnlyValueKeysAttribute> onlyattrs =
                    SelectAttributesFromType<OnlyValueKeysAttribute>(modalcontext.ModalType);
                List<OnlyValueKeysAttribute> effects = new List<OnlyValueKeysAttribute>();
                List<string> effectnames = new List<string>();

                #region 检查每个字段

                foreach (var field in fields)
                {
                    //忽略该忽略的字段
                    if (IsIgnoreCheckField(field, rangle, ranglefieldnames)) continue;
                    //检测:是否可为空、字符长度、数字范围
                    CheckAddOrUpdateFieldValue(dm, field, field.GetModelFieldValue(modalcontext.Data), errs);
                    if (onlyattrs != null)
                    {
                        //查找并添加该字段影响到的唯一规则 到检测集合中
                        //条件:OnlyValueKeysAttribute.OnlyKeys包含该字段
                        effects.AddRange(onlyattrs.Where(f => f.OnlyKeys != null
                            && f.OnlyKeys.FirstOrDefault(f2 => string.Equals(f2, field.Name, StringComparison.OrdinalIgnoreCase)) != null
                            ));
                        effectnames.Add(field.Name);
                    }
                }

                #endregion

                //检查唯一规则
                foreach (var ef in effects)
                {
                    CheckOnlyValues(dm, ef, effectnames, modalcontext, errs);
                }

                //引用检查
                CheckDataIdRefrence(dm, modalcontext, errs);
            }
        }

        Exception IUpdateChecker.CheckErrors(IUpdateContext context, IEnumerable<IUpdateContext> grouptocommit, IDataSelector dataSelector)
        {
            CurrentContext = context;
            CurrentGroups = grouptocommit;
            CurrentSelector = dataSelector;
            if (context is IDataUpdateContext)
            {
                IDataUpdateContext k = context as IDataUpdateContext;
                switch (k.ModalState)
                {
                    case ModalState.AddNew: return CheckAddNew();
                    case ModalState.Update: return CheckUpdate();
                    case ModalState.Delete: return CheckDelete();
                    default: return null;
                }
            }
            return CheckUnkownUpdateContext();
        }
    }
}
