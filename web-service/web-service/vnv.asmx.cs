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

        #region Column Service

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

            result.flag = tc.ID; 
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

        #endregion

        #region Group Service

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
                group.Brief = (obj.Brief==null)?"":obj.Brief.Trim();
                group.Code = (obj.Code==null)?"":obj.Code.Trim();

                result.items = group;
            }
            else
            {
                result.flag = -1;
                result.error = "cann't find the group";
            }

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

        [WebMethod]
        public string AddGroup(int parentID, string strGroupEx)
        {
            clsGroupEx group = JsonConvert.DeserializeObject<clsGroupEx>(strGroupEx);
            clsResult result = new clsResult();
            string output = JsonConvert.SerializeObject(result);

            if (parentID != 0)
            {
                var obj = data.t_Group.First(g => g.ID == parentID);
                if (obj == null)
                {
                    result.flag = -1;
                    result.error = "parent unexist";
                    output = JsonConvert.SerializeObject(result);
                    return output;
                }
            }

            t_Group tg = new t_Group();
            tg.ParentID = parentID;
            tg.Name = group.Name.Trim();
            tg.Code = group.Code.Trim();
            tg.Brief = group.Brief.Trim();
            tg.CreateDate = System.DateTime.Now;
            tg.Type = group.Type;
            data.t_Group.InsertOnSubmit(tg);
            data.SubmitChanges();

            result.flag = tg.ID; 
            output = JsonConvert.SerializeObject(result);
            return output;
        }

        [WebMethod]
        public string UpdateGroup(string strGroupEx)
        {
            clsGroupEx group = JsonConvert.DeserializeObject<clsGroupEx>(strGroupEx);

            var obj = data.t_Group.First(g => g.ID == group.ID);

            clsResult result = new clsResult();
            if (obj == null)
            {
                result.flag = -1;
                result.error = "cann't find the group";
            }
            else
            {
                obj.Name = group.Name.Trim();
                obj.Code = group.Code.Trim();
                obj.Brief = group.Brief.Trim();
                obj.Type = group.Type;

                data.SubmitChanges();
            }

            string output = JsonConvert.SerializeObject(result);

            return output;
        }

        [WebMethod]
        public string DeleteGroup(int groupID)
        {
            string strSQL = "Delete t_Group Where ID=" + groupID;
            data.ExecuteCommand(strSQL);

            clsResult result = new clsResult();
            string output = JsonConvert.SerializeObject(result);

            return output;
        }

        #endregion

        #region Resource Service

        [WebMethod]
        public string GetResources(int groupID)
        {
            clsResource resource;
            clsResourceSet resources = new clsResourceSet();

            var objs = data.t_Resource.Where(r => r.GroupID == groupID);
            foreach (var obj in objs)
            {
                resource = new clsResource();
                resource.ID = obj.ID;
                resource.Title = obj.Title;
                resource.CreateDate = ((DateTime)obj.CreateDate).ToShortDateString();
                resource.Type = (int)obj.ResourceType;
                resource.GroupID = groupID;

                resources.Add(resource);
            }

            clsResult result = new clsResult();

            result.flag = resources.Count;
            result.items = resources;
            string output = JsonConvert.SerializeObject(result);

            return output;
        }

        [WebMethod]
        public string GetResourceEx(int ID)
        {
            clsResourceEx resource = new clsResourceEx();

            var obj = data.t_Resource.First(r => r.ID == ID);
            if (obj != null)
            {
                resource.Author = (obj.Author==null)?"": obj.Author.Trim();
                resource.Brief = (obj.Brief == null) ? "" : obj.Brief.Trim();
                resource.Content = (obj.Content == null) ? "" : obj.Content.Trim();
                resource.CreateDate = (DateTime)obj.CreateDate;
                resource.GroupID = (int)obj.GroupID;
                resource.GroupUID = obj.GroupUID.Trim();
                resource.ID = obj.ID;
                resource.ParentUID = (obj.ParentUID == null) ? "" : obj.ParentUID.Trim();
                resource.Source = (obj.Source == null) ? "" : obj.Source.Trim();
                resource.Status = (int)obj.Status;
                resource.SubTitle = (obj.SubTitle == null) ? "" : obj.SubTitle.Trim();
                resource.Title = (obj.Title == null) ? "" : obj.Title.Trim();
                resource.Type = (int)obj.ResourceType;
                resource.UID = (obj.UID == null) ? "" : obj.UID.Trim();
            }

            clsResult result = new clsResult();
            result.items = resource;
            string output = JsonConvert.SerializeObject(result);

            return output;
        }

        [WebMethod]
        public string AddResource(int groupID, string strResourceEx, string content)
        {
            clsResourceEx resource = JsonConvert.DeserializeObject<clsResourceEx>(strResourceEx);
            resource.Content = content;
            t_Resource tc = new t_Resource();
            tc.Author = resource.Author;
            tc.Brief = resource.Brief;
            tc.Content = resource.Content;
            tc.CreateDate = DateTime.Now;
            tc.GroupID = groupID;
            tc.GroupUID = resource.GroupUID;
            tc.ParentUID = "";
            tc.ResourceType = resource.Type;
            tc.Source = resource.Source;
            tc.Status = resource.Status;
            tc.SubTitle = resource.SubTitle;
            tc.TextType = 0;
            tc.Title = resource.Title;
            tc.UID = Guid.NewGuid().ToString();

            data.t_Resource.InsertOnSubmit(tc);
            data.SubmitChanges();

            clsResult result = new clsResult();
            result.flag = tc.ID;
            string output = JsonConvert.SerializeObject(result);

            return output;
        }

        [WebMethod]
        public string UpdateResource(string strResourceEx, string content)
        {
            clsResourceEx resource = JsonConvert.DeserializeObject<clsResourceEx>(strResourceEx);
            resource.Content = content;
            clsResult result = new clsResult();

            t_Resource tc = data.t_Resource.First(r => r.ID == resource.ID);
            if (tc == null)
            {
                result.flag = -1;
                result.error = "cann't find the resource";
            }
            else
            {
                tc.Author = resource.Author;
                tc.Brief = resource.Brief;
                tc.Content = resource.Content;
                tc.CreateDate = DateTime.Now;
                tc.GroupID = resource.GroupID;
                tc.GroupUID = resource.GroupUID;
                tc.ParentUID = resource.ParentUID;
                tc.ResourceType = resource.Type;
                tc.Source = resource.Source;
                tc.Status = resource.Status;
                tc.SubTitle = resource.SubTitle;
                tc.Title = resource.Title;

                data.SubmitChanges();
            }

            string output = JsonConvert.SerializeObject(result);
            return output;
        }

        [WebMethod]
        public string DeleteResource(int ID)
        {
            string strSQL = "Delete t_Resource Where ID=" + ID;
            data.ExecuteCommand(strSQL);

            clsResult result = new clsResult();
            string output = JsonConvert.SerializeObject(result);
            return output;
        }

        #endregion
    }
}
