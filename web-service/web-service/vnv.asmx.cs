using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Script.Services;

using Newtonsoft.Json;

namespace web_service
{
    /// <summary>
    /// vnv 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消对下行的注释。
    [System.Web.Script.Services.ScriptService]
    public class vnv : System.Web.Services.WebService
    {
        webdataDataContext data = new webdataDataContext();

        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }

        [WebMethod]
        public string GetColumn(int columnID)
        {
            t_Column obj = data.t_Column.First(c => c.ID == columnID);
            clsColumn column = new clsColumn();

            if (obj != null)
            {
                column.ID = obj.ID;
                column.ParentID = (int)obj.ParentID;
                column.Code = obj.Code.Trim();
                column.Name = obj.Name.Trim();
            }

            clsResult result = new clsResult();
            result.items = column;

            string output = JsonConvert.SerializeObject(result);

            return output;
        }

        [WebMethod]
        public string GetGroup(int groupID)
        {
            t_Group obj = data.t_Group.First(g => g.ID == groupID);
            clsGroup group = new clsGroup();

            clsResult result = new clsResult();
            if (obj != null)
            {
                group.ID = obj.ID;
                group.ParentID = (int)obj.ParentID;
                group.Name = obj.Name.Trim();
            }
            else
            {
                result.flag = -1;
                result.error = "cann't find the group";
            }

            string output = JsonConvert.SerializeObject(result);
            result.items = group;

            return output;
        }

        [WebMethod]
        public string GetColumnEx(int columnID)
        {
            t_Column obj = data.t_Column.First(c => c.ID == columnID);
            clsColumnEx column = new clsColumnEx();

            if (obj != null)
            {
                column.ID = obj.ID;
                column.ParentID = (int)obj.ParentID;
                column.Code = obj.Code.Trim();
                column.Name = obj.Name.Trim();
                column.Brief = obj.Brief.Trim();
            }

            clsResult result = new clsResult();
            result.items = column;

            string output = JsonConvert.SerializeObject(result);

            return output;
        }

        [WebMethod]
        public string GetGroupEx(int groupID)
        {
            t_Group obj = data.t_Group.First(g => g.ID == groupID);
            clsGroupEx group = new clsGroupEx();

            clsResult result = new clsResult();
            if (obj != null)
            {
                group.ID = obj.ID;
                group.ParentID = (int)obj.ParentID;
                group.Name = obj.Name.Trim();
                group.Type = (int)obj.Type;
                group.Brief = obj.Brief.Trim();
            }
            else
            {
                result.flag = -1;
                result.error = "cann't find the group";
            }

            string output = JsonConvert.SerializeObject(result);
            result.items = group;

            return output;
        }

        [WebMethod]
        public string UpdateColumn(string strColumnEx)
        {
            clsColumnEx column = JsonConvert.DeserializeObject<clsColumnEx>(strColumnEx);

            var obj = data.t_Column.First(c => c.ID == column.ID);

            clsResult result = new clsResult();
            if (obj == null)
            {
                result.flag = -1;
                result.error = "cann't find the column";
            }
            else
            {
                obj.Name = column.Name.Trim();
                obj.Code = column.Code.Trim();
                obj.Brief = column.Brief.Trim();

                data.SubmitChanges();
            }

            string output = JsonConvert.SerializeObject(result);

            return output;
        }

        [WebMethod]
        public string AddColumn(int parentID, string strColumnEx)
        {
            clsColumnEx column = JsonConvert.DeserializeObject<clsColumnEx>(strColumnEx);
            clsResult result = new clsResult();
            string output = JsonConvert.SerializeObject(result);

            if (parentID != 0)
            {
                var obj = data.t_Column.First(c => c.ID == parentID);
                if (obj == null)
                {
                    result.flag = -1;
                    result.error = "parent unexist";
                    output = JsonConvert.SerializeObject(result);
                    return output;
                }
            }

            t_Column tc = new t_Column();
            tc.ParentID = parentID;
            tc.Name = column.Name.Trim();
            tc.Code = column.Code;
            tc.Brief = column.Brief;
            data.t_Column.InsertOnSubmit(tc);
            data.SubmitChanges();

            result.flag = tc.ID; // to be sure!
            output = JsonConvert.SerializeObject(result);
            return output;
        }

        [WebMethod]
        public string DeleteColumn(int columnID)
        {
            var obj = data.t_Column.First(c => c.ID == columnID);
            if (obj!=null)
            {
                data.t_Column.DeleteOnSubmit(obj);
                data.SubmitChanges();
            }

            clsResult result = new clsResult();
            string output = JsonConvert.SerializeObject(result);

            return output;
        }

        [WebMethod] 
        public string GetColumns(int parentID)
        {
            var objs = data.t_Column.Where(c => c.ParentID == parentID);
            clsColumnSet columns = new clsColumnSet();
            clsColumn column;

            foreach (var obj in objs)
            {
                column = new clsColumn();

                column.ID = obj.ID;
                column.ParentID = (int)obj.ParentID;
                column.Code = obj.Code.Trim();
                column.Name = obj.Name.Trim();

                columns.Add(column);
            }

            clsResult result = new clsResult();
            result.items = columns;
            result.flag = columns.Count;
            string output = JsonConvert.SerializeObject(result);

            return output;
        }

        [WebMethod]
        public string GetColumnAll()
        {
            var objs = data.t_Column;
            clsColumnSet columns = new clsColumnSet();
            clsColumn column;

            foreach (var obj in objs)
            {
                column = new clsColumn();

                column.ID = obj.ID;
                column.ParentID = (int)obj.ParentID;
                column.Code = obj.Code.Trim();
                column.Name = obj.Name.Trim();

                columns.Add(column);
            }

            clsResult result = new clsResult();
            result.items = columns;
            result.flag = columns.Count;
            string output = JsonConvert.SerializeObject(result);

            return output;
        }

        [WebMethod]
        public string GetGroupAll(int Type)
        {
            var objs = data.t_Group.Where(g => g.Type == Type);
            clsGroupSet groups = new clsGroupSet();
            clsGroup group;

            foreach (var obj in objs)
            {
                group = new clsGroup();
                group.ID = obj.ID;
                group.ParentID = (int)obj.ParentID;
                group.Name = obj.Name.Trim();

                groups.Add(group);
            }

            clsResult result = new clsResult();
            result.items = groups;
            result.flag = groups.Count;

            string output = JsonConvert.SerializeObject(result);

            return output;
        }
    }
}
