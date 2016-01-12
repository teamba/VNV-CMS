using System;
using System.Collections;
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

        [WebMethod(EnableSession=true)]
        public string HelloWorld()
        {
            return "Hello World";
        }

        #region Column Service

        [WebMethod(EnableSession=true)]
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

        [WebMethod(EnableSession=true)]
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

                column.ContentType = (int)obj.Type;
                if (obj.SEO_Description != null) column.SEO_Description = (Convert.ToString(obj.SEO_Description)).Trim();
                if (obj.SEO_Keyword != null) column.SEO_Keyword = (Convert.ToString(obj.SEO_Keyword)).Trim();
                if (obj.SEO_Title != null) column.SEO_Title = (Convert.ToString(obj.SEO_Title)).Trim();
                if (obj.Template != null) column.Template = (Convert.ToString(obj.Template)).Trim();
            }

            GetProperties(1, columnID, column.Properties);

            clsResult result = new clsResult();
            result.items = column;

            string output = JsonConvert.SerializeObject(result);

            return output;
        }

        [WebMethod(EnableSession=true)]
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

                obj.Type = column.ContentType;
                obj.SEO_Description = column.SEO_Description.Trim();
                obj.SEO_Keyword = column.SEO_Keyword.Trim();
                obj.SEO_Title = column.SEO_Title.Trim();
                //obj.Template = column.Template.Trim();

                data.SubmitChanges();

                if (column.Properties != null) SaveProperties(1, column.ID, column.Properties);
            }

            string output = JsonConvert.SerializeObject(result);

            return output;
        }

        [WebMethod(EnableSession=true)]
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
            tc.Code = column.Code.Trim();
            tc.Brief = column.Brief.Trim();
            tc.CreateDate = DateTime.Now;

            tc.Type = column.ContentType;
            tc.SEO_Description = column.SEO_Description.Trim();
            tc.SEO_Keyword = column.SEO_Keyword.Trim();
            tc.SEO_Title = column.SEO_Title.Trim();
            //tc.Template = column.Template.Trim();

            data.t_Column.InsertOnSubmit(tc);
            data.SubmitChanges();

            column.ID = tc.ID;
            if (column.Properties != null) SaveProperties(1, column.ID, column.Properties);

            result.flag = tc.ID; 
            output = JsonConvert.SerializeObject(result);
            return output;
        }

        [WebMethod(EnableSession=true)]
        public string DeleteColumn(int columnID)
        {
            var obj = data.t_Column.First(c => c.ID == columnID);
            if (obj!=null)
            {
                data.t_Column.DeleteOnSubmit(obj);
                data.SubmitChanges();

                DeleteProperties(1, columnID);
            }

            clsResult result = new clsResult();
            string output = JsonConvert.SerializeObject(result);

            return output;
        }

        [WebMethod(EnableSession=true)] 
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

        [WebMethod(EnableSession=true)]
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

        [WebMethod(EnableSession=true)]
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

        [WebMethod(EnableSession=true)]
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

                GetProperties(2, groupID, group.Properties);

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

        [WebMethod(EnableSession=true)]
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

        [WebMethod(EnableSession = true)]
        public string GetGroups(int Type, int parentID=0)
        {
            var objs = data.t_Group.Where(g => g.Type == Type && g.ParentID==parentID);
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

        [WebMethod(EnableSession=true)]
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

            group.ID = tg.ID;
            if (group.Properties != null) SaveProperties(2, group.ID, group.Properties);

            result.flag = tg.ID; 
            output = JsonConvert.SerializeObject(result);
            return output;
        }

        [WebMethod(EnableSession=true)]
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

                if (group.Properties != null) SaveProperties(2, group.ID, group.Properties);
            }

            string output = JsonConvert.SerializeObject(result);

            return output;
        }

        [WebMethod(EnableSession=true)]
        public string DeleteGroup(int groupID)
        {
            string strSQL = "Delete t_Group Where ID=" + groupID;
            data.ExecuteCommand(strSQL);

            DeleteProperties(2, groupID);

            clsResult result = new clsResult();
            string output = JsonConvert.SerializeObject(result);

            return output;
        }

        #endregion

        #region Resource Service

        [WebMethod(EnableSession=true)]
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

        [WebMethod(EnableSession=true)]
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

                GetProperties(3, resource.ID, resource.Properties);
            }

            clsResult result = new clsResult();
            result.items = resource;
            string output = JsonConvert.SerializeObject(result);

            return output;
        }

        [WebMethod(EnableSession=true)]
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

            resource.ID = tc.ID;
            if (resource.Properties != null) SaveProperties(3, resource.ID, resource.Properties);

            clsResult result = new clsResult();
            result.flag = tc.ID;
            string output = JsonConvert.SerializeObject(result);

            return output;
        }

        [WebMethod(EnableSession=true)]
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

                if (resource.Properties != null) SaveProperties(3, resource.ID, resource.Properties);
            }

            string output = JsonConvert.SerializeObject(result);
            return output;
        }

        [WebMethod(EnableSession=true)]
        public string DeleteResource(int ID)
        {
            string strSQL = "Delete t_Resource Where ID=" + ID;
            data.ExecuteCommand(strSQL);

            DeleteProperties(3, ID);

            clsResult result = new clsResult();
            string output = JsonConvert.SerializeObject(result);
            return output;
        }

        #endregion

        #region Publish Service

        [WebMethod(EnableSession=true)]
        public string GetEditUpdate(int columnID)
        {
            /* 取指定栏目正在编辑的内容
             * 如果没有正在编辑的内容，则返回空
             */
            int userID = 0;
            if (Session["userID"] != null) userID = Convert.ToInt32(Session["userID"]);
            var objs = from i in data.t_UpdateItem
                       join u in data.t_ColumnUpdate on i.UpdateID equals u.ID
                       join r in data.t_Resource on i.ResourceID equals r.ID
                       where u.OwnerID == userID && u.Status == 0 && u.ColumnID == columnID
                       select new clsUpdateItem
                       {
                           ID = i.ID,
                           UpdateID = u.ID,
                           ResourceID = r.ID,
                           Title = (i.Title.Trim()=="")?r.Title.Trim():i.Title.Trim(),
                           Brief = i.Brief,
                           ListPoint = (int)i.ListPoint
                       };

            clsResult result = new clsResult();

            result.flag = objs.Count();
            result.items = objs;

            string output = JsonConvert.SerializeObject(result);

            return output;
        }

        [WebMethod(EnableSession=true)]
        public string SaveEditUpdate(int updateID, int columnID, string strUpdateItem)
        {
            /* 将指定的内容保存成正在被编辑
             * 如果updateID>0，则有效，
             *      如果其状态也为0，则替换原内容
             *      如果其状态为1或者2，则保留原内容，保存成新内容，原状态0内容转为2
             *      
             * 如果updateID<0，则以columnID(必须>0)为准
             *      如果不存在状态0的内容，则新增内容，状态为0
             *      如果存在状态为0的内容，则将新内容添加进去，避免内容重复
             *      
             * strUpdateItem
             * resourceID|resourceID|...|resourceID
             * 按次序保存
             */
            int userID = 0;
            if (Session["userID"] != null) userID = Convert.ToInt32(Session["userID"]);

            string[] items = strUpdateItem.Split('|');

            clsResult result = new clsResult();

            t_ColumnUpdate tu = null;

            if (updateID > 0)
            {
                var objs = data.t_ColumnUpdate.Where(u => u.ID == updateID);
                if (objs.Count() > 0) foreach (var obj in objs) { tu = obj; break; }
            }
            else
            {
                var objs = data.t_ColumnUpdate.Where(u => u.OwnerID == userID && u.Status == 0);
                if (objs.Count() > 0) foreach (var obj in objs) { tu = obj; break; }
            }

            // change the other editing update to used update
            string strSQL = "Update t_ColumnUpdate Set Status=2 Where Status=0 And OwnerID=" + userID + " And ColumnID=" + columnID + " And ID!=" + updateID;
            data.ExecuteCommand(strSQL);

            int updateID_backup = 0;
            if (tu == null)
            {
                // create a new update
                tu = new t_ColumnUpdate();
                tu.OwnerID = userID;
                tu.ColumnID = columnID;
                tu.CreateDate = DateTime.Now;
                tu.Status = 0;

                data.t_ColumnUpdate.InsertOnSubmit(tu);
                data.SubmitChanges();
            }
            else
            {
                if (tu.Status != 0)
                {
                    //tu.Status = 2;
                    //data.SubmitChanges();

                    // not the editing update, keep the exist update and create the new one
                    updateID_backup = tu.ID;
                    tu.Status = 0;
                    tu.CreateDate = DateTime.Now;
                    data.t_ColumnUpdate.InsertOnSubmit(tu);
                    data.SubmitChanges();
                }
            }

            //tu.Status = 0;
            //data.SubmitChanges();

            // get the items exist in the editing update
            var objss = data.t_UpdateItem.Where(u => u.UpdateID == tu.ID); // updateID_backup);
            clsUpdateItemSet uitems = new clsUpdateItemSet();
            clsUpdateItem uitem;
            if (objss.Count() > 0) foreach (var obj in objss)
                {
                    uitem = new clsUpdateItem();
                    uitem.ResourceID = (int)obj.ResourceID;
                    uitems.Add(uitem);
                }

            t_UpdateItem ui;
            for (int i = 0; i < items.Count(); i++)
            {
                // the resource exist in the update items, change the list point only
                if (uitems.Exist(Convert.ToInt32(items[i])))
                {
                    var obj = data.t_UpdateItem.First(u => u.UpdateID == tu.ID && u.ResourceID == Convert.ToInt32(items[i]));
                    obj.ListPoint = i + 1;
                    data.SubmitChanges();

                    continue;
                }

                // add a new item into the update
                ui = new t_UpdateItem();
                ui.ResourceID = Convert.ToInt32(items[i]);
                ui.ListPoint = i + 1;
                ui.UpdateID = tu.ID;
                ui.Title = "";
                ui.Brief = "";
                data.t_UpdateItem.InsertOnSubmit(ui);
                data.SubmitChanges();

                uitem = new clsUpdateItem();
                uitem.ResourceID = Convert.ToInt32(items[i]);
                uitems.Add(uitem);
            }

            result.flag = uitems.Count;
            result.items = tu;
 
            string output = JsonConvert.SerializeObject(result);

            return output;
        }

        [WebMethod(EnableSession=true)]
        public string SaveAllUpdate(string strUpdates)
        {
            /* 给所有指定的栏目保存内容
             * 内容只能追加保存，避免重复
             * 
             * strUpdate
             * columnid:resouseid|resourceid|..resourceid-columnid:resourceid|...resourceid
             */
            int userID = 0;
            if (Session["userID"] != null) userID = Convert.ToInt32(Session["userID"]);

            string[] items1 = strUpdates.Split('-');
            string[] items2;
            for (int i = 0; i < items1.Count(); i++)
            {
                items2 = items1[i].Split(':');
                if (items2.Count() != 2) continue;

                SaveEditUpdate(0, Convert.ToInt32(items2[0]), items2[1]);
            }

            clsResult result = new clsResult();
            result.flag = items1.Count();

            string output = JsonConvert.SerializeObject(result);

            return output;
        }

        [WebMethod(EnableSession=true)]
        public string SaveUpdateItem(string strUpdateItem)
        {
            /* 保存一单项内容，包括brief, title, listpoint
             */
            int userID = 0;
            if (Session["userID"] != null) userID = Convert.ToInt32(Session["userID"]);

            clsUpdateItem item = JsonConvert.DeserializeObject<clsUpdateItem>(strUpdateItem);

            var objs = data.t_UpdateItem.Where(u => u.ID == item.ID);
            foreach (var obj in objs)
            {
                obj.Title = item.Title.Trim();
                obj.ListPoint = item.ListPoint;
                obj.Brief = item.Brief.Trim();

                data.SubmitChanges();
            }

            clsResult result = new clsResult();

            string output = JsonConvert.SerializeObject(result);

            return output;
        }

        [WebMethod(EnableSession=true)]
        public string GetUpdates(int columnID)
        {
            /* 取指定栏目的所有update
             */
            int userID = 0;
            if (Session["userID"] != null) userID = Convert.ToInt32(Session["userID"]);

            var objs = from u in data.t_ColumnUpdate
                       where u.ColumnID == columnID && u.OwnerID == userID 
                       orderby u.CreateDate descending
                       select new clsColumnUpdate
                       {
                           ID = u.ID,
                           OwnerID = (int)u.OwnerID,
                           ColumnID = columnID,
                           CreateDate = (DateTime)u.CreateDate,
                           Status = (int)u.Status
                       };

            clsResult result = new clsResult();
            result.flag = objs.Count();
            result.items = objs;

            string output = JsonConvert.SerializeObject(result);

            return output;
        }

        [WebMethod(EnableSession=true)]
        public string GetUpdateItems(int updateID)
        {
            /* 取指定update的所有内容
             */
            int userID = 0;
            if (Session["userID"] != null) userID = Convert.ToInt32(Session["userID"]);

            var objs = from i in data.t_UpdateItem
                       join r in data.t_Resource on i.ResourceID equals r.ID
                       where i.UpdateID == updateID
                       orderby i.ListPoint
                       select new clsUpdateItem
                       {
                           ID = i.ID,
                           ResourceID = r.ID,
                           Brief = (Convert.ToString(i.Brief)).Trim(),
                           Title = ((Convert.ToString(i.Title)).Trim() == "") ? (Convert.ToString(r.Title)).Trim() : (Convert.ToString(i.Title)).Trim(),
                           UpdateID = updateID,
                           ListPoint = (int)i.ListPoint
                       };
            /*
            clsUpdateItem item;
            clsUpdateItemSet items = new clsUpdateItemSet();
            foreach (var obj in objs) //if (objs != null) 
                {
                    item = new clsUpdateItem();
                    item.ID = obj.ID;
                    item.ResourceID = obj.ResourceID;
                    item.Title = obj.Title;
                    item.Brief = obj.Brief;
                    item.UpdateID = obj.UpdateID;
                    item.ListPoint = obj.ListPoint;
                    items.Add(item);
                }
            */
            clsResult result = new clsResult();
            result.flag = objs.Count();// items.Count;
            result.items = objs;// items;

            string output = JsonConvert.SerializeObject(result);

            return output;
        }

        [WebMethod(EnableSession=true)]
        public string DeleteUpdateItem(int ID)
        {
            /* 删除指定update中的某项内容
             */
            int userID = 0;
            if (Session["userID"] != null) userID = Convert.ToInt32(Session["userID"]);

            string strSQL = "Delete from t_UpdateItem Where ID=" + ID;
            data.ExecuteCommand(strSQL);

            DeleteProperties(5, ID);

            clsResult result = new clsResult();

            string output = JsonConvert.SerializeObject(result);

            return output;
        }

        [WebMethod(EnableSession = true)]
        public string DeleteUpdateItems(int updateID)
        {
            /* 删除指定update中的所有内容
             */
            int userID = 0;
            if (Session["userID"] != null) userID = Convert.ToInt32(Session["userID"]);

            string strSQL = "Delete from t_UpdateItem Where UpdateID=" + updateID;
            data.ExecuteCommand(strSQL);

            // Delete From t_Property Where ObjectType=5 And ObjectID in (Select ResourceID From t_UpdateItem Where UpdateID=1)
            strSQL = "Delete From t_Property Where ObjectType=5 And ObjectID in (Select ResourceID From t_UpdateItem Where UpdateID=" + updateID + ")";
            data.ExecuteCommand(strSQL);

            clsResult result = new clsResult();

            string output = JsonConvert.SerializeObject(result);

            return output;
        }

        [WebMethod(EnableSession = true)]
        public string PublishColumn(int columnID)
        {
            int userID = 0;
            if (Session["userID"] != null) userID = Convert.ToInt32(Session["userID"]);

            string strSQL = "Update t_ColumnUpdate Set Status=2 Where Status=1 And ColumnID=" + columnID + " And OwnerID=" + userID;
            data.ExecuteCommand(strSQL);

            strSQL = "Update t_ColumnUpdate Set Status=1 Where Status=0 And ColumnID=" + columnID + " And OwnerID=" + userID;
            data.ExecuteCommand(strSQL);

            clsResult result = new clsResult();

            string output = JsonConvert.SerializeObject(result);

            return output;
        }

        #endregion

        #region Property Service

        public int GetProperties(int objectType, int objectID, clsPropertySet properties)
        {
            int ret = 0;

            properties.Clear();
            clsProperty property;

            var objs = data.t_Property.Where(p => p.ObjectID == objectID && p.ObjectType == objectType);
            foreach (var obj in objs)
            {
                property = new clsProperty();
                property.ID = obj.ID;
                property.Key = obj.Key.Trim();
                property.ObjectID = obj.ID;
                property.ObjectType = (int)obj.ObjectType;
                property.Value = obj.Value.Trim();

                properties.Add(property);
            }

            ret = properties.Count;
            return ret;
        }

        public int SaveProperties(int objectType, int objectID, clsPropertySet properties)
        {
            int ret = 0;

            t_Property tp;
            for (int i = 0; i < properties.Count; i++)
            {
                tp = new t_Property();
                tp.ObjectType = properties[i].ObjectType;
                tp.ObjectID = properties[i].ObjectID;
                tp.Key = properties[i].Key.Trim();
                tp.Value = properties[i].Value.Trim();

                data.t_Property.InsertOnSubmit(tp);
                data.SubmitChanges();
            }

            return ret;
        }

        public void DeleteProperties(int objectType, int objectID)
        {
            string strSQL = "Delete t_Property Where ObjectType=" + objectType + " And ObjectID=" + objectID;
            data.ExecuteCommand(strSQL);
        }

        [WebMethod(EnableSession = true)]
        public string GetProperties(int objectType, int objectID)
        {
            clsResult result = new clsResult();

            clsPropertySet properties = new clsPropertySet();
            GetProperties(objectType, objectID, properties);

            result.items = properties;

            string output = JsonConvert.SerializeObject(result);

            return output;
        }

        [WebMethod(EnableSession = true)]
        public string SaveProperties(int objectType, int objectID, string strProperties)
        {
            IList objs = JsonConvert.DeserializeObject<IList>(strProperties);
            clsPropertySet properties = new clsPropertySet();
            clsProperty property;
            for (int i = 0; i < objs.Count; i++)
            {
                property = JsonConvert.DeserializeObject<clsProperty>(Convert.ToString(objs[i]));
                properties.Add(property);
            }

            SaveProperties(objectType, objectID, properties);

            clsResult result = new clsResult();

            string output = JsonConvert.SerializeObject(result);

            return output;
        }

        #endregion

        #region User Service

        [WebMethod(EnableSession = true)]
        public string GetUserAll()
        {
            var objs = data.t_User;
            clsUserSet users = new clsUserSet();
            clsUser user;

            foreach (var obj in objs)
            {
                user = new clsUser();
                user.Account = obj.Account.Trim();
                user.CompanyID = (int)obj.CompanyID;
                user.GroupID = (int)obj.GroupID;
                user.ID = obj.ID;
                user.Name = obj.Name.Trim();

                users.Add(user);
            }

            clsResult result = new clsResult();

            result.flag = users.Count;
            result.items = users;
            string output = JsonConvert.SerializeObject(result);

            return output;
        }

        [WebMethod(EnableSession = true)]
        public string GetUsersByGroup(int groupID)
        {
            var objs = data.t_User.Where(u=>u.GroupID == groupID);
            clsUserSet users = new clsUserSet();
            clsUser user;

            foreach (var obj in objs)
            {
                user = new clsUser();
                user.Account = obj.Account.Trim();
                user.CompanyID = (int)obj.CompanyID;
                user.GroupID = (int)obj.GroupID;
                user.ID = obj.ID;
                user.Name = obj.Name.Trim();

                users.Add(user);
            }

            clsResult result = new clsResult();

            result.flag = users.Count;
            result.items = users;
            string output = JsonConvert.SerializeObject(result);

            return output;
        }

        [WebMethod(EnableSession = true)]
        public string GetUsersByCompany(int companyID)
        {
            var objs = data.t_User.Where(u=>u.CompanyID == companyID);
            clsUserSet users = new clsUserSet();
            clsUser user;

            foreach (var obj in objs)
            {
                user = new clsUser();
                user.Account = obj.Account.Trim();
                user.CompanyID = (int)obj.CompanyID;
                user.GroupID = (int)obj.GroupID;
                user.ID = obj.ID;
                user.Name = obj.Name.Trim();

                users.Add(user);
            }

            clsResult result = new clsResult();

            result.flag = users.Count;
            result.items = users;
            string output = JsonConvert.SerializeObject(result);

            return output;
        }

        private clsUserEx GetUserEx(string account)
        {
            account = account.Trim();
            if (account == "") return null;

            clsUserEx user = null;

            var objs = data.t_User.Where(u => u.Account == account);
            foreach (var obj in objs)
            {
                user.Account = obj.Account.Trim();
                user.Address = obj.Address.Trim();
                user.CellPhone = obj.CellPhone.Trim();
                user.CompanyID = (int)obj.CompanyID;
                user.CreateDate = (DateTime)obj.CreateDate;
                user.eMail = obj.eMail.Trim();
                user.FirstName = obj.FirstName.Trim();
                user.GroupID = (int)obj.GroupID;
                user.ID = obj.ID;
                user.IntroducerID = (int)obj.IntroducerID;
                user.LastName = obj.LastName.Trim();
                user.Name = obj.Name;
                user.Password = obj.Password.Trim();
                user.Sex = (int)obj.Sex;
                user.Status = (int)obj.Status;
                user.Type = (int)obj.Type;
                user.UID = obj.UID.Trim();

                break;
            }

            return user;
        }

        [WebMethod(EnableSession = true)]
        public string GetUserByAccount(string account)
        {
            clsUserEx user = GetUserEx(account);

            clsResult result = new clsResult();
            if (user == null)
            {
                result.flag = -1;
                result.error = "cann't find the user";
            }
            else
            {
                result.flag = 1;
                user.Password = ""; // cann't return the password
                result.items = user;
            }

            string output = JsonConvert.SerializeObject(result);
            
            return output;
        }

        [WebMethod(EnableSession = true)]
        public string Logoff()
        {
            Session["userID"] = null;
            clsResult result = new clsResult();

            string output = JsonConvert.SerializeObject(result);

            return output;
        }

        [WebMethod(EnableSession = true)]
        public string Login(string account, string password)
        {
            Session["userID"] = null;
            clsResult result = new clsResult();

            clsUserEx user = GetUserEx(account);
            if (user == null)
            {
                result.flag = -1;
                result.error = "cann't find the user";
            }
            else if (user.Password != password)
            {
                result.flag = -2;
                result.error = "invalid password";
            }
            else
            {
                result.flag = 1;
                user.Password = ""; // cann't return the password
                result.items = user;

                Session["userID"] = user.ID;
            }

            string output = JsonConvert.SerializeObject(result);

            return output;
        }

        [WebMethod(EnableSession = true)]
        public string GetUser(int ID)
        {
            var objs = data.t_User.Where(u => u.ID == ID);

            clsResult result = new clsResult();
            result.flag = -1;
            result.error = "cann't find the user";

            clsUserEx user = new clsUserEx();
            foreach (var obj in objs)
            {
                user.Account = obj.Account.Trim();
                user.Address = obj.Address.Trim();
                user.CellPhone = obj.CellPhone.Trim();
                user.CompanyID = (int)obj.CompanyID;
                user.CreateDate = (DateTime)obj.CreateDate;
                user.eMail = obj.eMail.Trim();
                user.FirstName = obj.FirstName.Trim();
                user.GroupID = (int)obj.GroupID;
                user.ID = obj.ID;
                user.IntroducerID = (int)obj.IntroducerID;
                user.LastName = obj.LastName.Trim();
                user.Name = obj.Name;
                user.Password = obj.Password.Trim();
                user.Sex = (int)obj.Sex;
                user.Status = (int)obj.Status;
                user.Type = (int)obj.Type;
                user.UID = obj.UID.Trim();

                result.flag = 1;
                result.error = "";
                result.items = user;
                break;
            }

            string output = JsonConvert.SerializeObject(result);

            return output;
        }

        [WebMethod(EnableSession = true)]
        public string AddUser(string strUserEx)
        {
            clsUserEx user = JsonConvert.DeserializeObject<clsUserEx>(strUserEx);
            clsResult result = new clsResult();
            string output = JsonConvert.SerializeObject(result);

            clsUserEx check = GetUserEx(user.Account);
            if (check != null)
            {
                result.flag = -1;
                result.error = "the account had exist!";
                output = JsonConvert.SerializeObject(result);

                return output;
            }

            t_User tu = new t_User();
            tu.Account = user.Account.Trim();
            tu.Address = user.Address.Trim();
            tu.CellPhone = user.CellPhone.Trim();
            tu.CompanyID = user.CompanyID;
            tu.CreateDate = DateTime.Now;
            tu.eMail = user.eMail.Trim();
            tu.FirstName = user.FirstName.Trim();
            tu.GroupID = user.GroupID;
            tu.IntroducerID = user.IntroducerID;
            tu.LastName = user.LastName.Trim();
            tu.Name = user.Name.Trim();
            tu.Password = user.Password.Trim();
            tu.Sex = user.Sex;
            tu.Status = user.Status;
            tu.Type = user.Type;
            tu.UID = user.UID;

            data.t_User.InsertOnSubmit(tu);
            data.SubmitChanges();

            user.ID = tu.ID;

            result.flag = tu.ID;
            output = JsonConvert.SerializeObject(result);
            return output;
        }

        [WebMethod(EnableSession = true)]
        public string UpdateUser(string strUserEx)
        {
            clsUserEx user = JsonConvert.DeserializeObject<clsUserEx>(strUserEx);

            var obj = data.t_User.First(u => u.ID == user.ID);

            clsResult result = new clsResult();
            if (obj == null)
            {
                result.flag = -1;
                result.error = "cann't find the user";
            }
            else
            {
                obj.Account = user.Account.Trim();
                obj.Address = user.Address.Trim();
                obj.CellPhone = user.CellPhone.Trim();
                obj.CompanyID = user.CompanyID;
                obj.eMail = user.eMail.Trim();
                obj.FirstName = user.FirstName.Trim();
                obj.GroupID = user.GroupID;
                obj.IntroducerID = user.IntroducerID;
                obj.LastName = user.LastName.Trim();
                obj.Name = user.Name.Trim();
                obj.Password = user.Password.Trim();
                obj.Sex = user.Sex;
                obj.Status = user.Status;
                obj.Type = user.Type;
                obj.UID = user.UID.Trim();

                data.SubmitChanges();
            }

            string output = JsonConvert.SerializeObject(result);

            return output;
        }

        [WebMethod(EnableSession = true)]
        public string DeleteUser(int ID)
        {
            string strSQL = "Delete t_User Where ID=" + ID;
            data.ExecuteCommand(strSQL);

            clsResult result = new clsResult();
            string output = JsonConvert.SerializeObject(result);

            return output;
        }

        #endregion

        #region Company Service

        [WebMethod(EnableSession = true)]
        public string GetCompanyAll()
        {
            var objs = data.t_Company;
            clsCompanySet companies = new clsCompanySet();
            clsCompany company;

            foreach (var obj in objs)
            {
                company = new clsCompany();

                company.GroupID = (int)obj.GroupID;
                company.ID = obj.ID;
                company.IntroducerID = (int)obj.IntroducerID;
                company.ListPoint = (int)obj.ListPoint;
                company.Name = obj.Name.Trim();
                company.ParentID = (int)company.ParentID;
                company.Type = (int)company.Type;

                companies.Add(company);
            }

            clsResult result = new clsResult();

            result.flag = companies.Count;
            result.items = companies;
            string output = JsonConvert.SerializeObject(result);

            return output;
        }

        [WebMethod(EnableSession = true)]
        public string GetCompanies(int parentID)
        {
            var objs = data.t_Company.Where(c=>c.ParentID == parentID);
            clsCompanySet companies = new clsCompanySet();
            clsCompany company;

            foreach (var obj in objs)
            {
                company = new clsCompany();

                company.GroupID = (int)obj.GroupID;
                company.ID = obj.ID;
                company.IntroducerID = (int)obj.IntroducerID;
                company.ListPoint = (int)obj.ListPoint;
                company.Name = obj.Name.Trim();
                company.ParentID = (int)company.ParentID;
                company.Type = (int)company.Type;

                companies.Add(company);
            }

            clsResult result = new clsResult();

            result.flag = companies.Count;
            result.items = companies;
            string output = JsonConvert.SerializeObject(result);

            return output;
        }

        [WebMethod(EnableSession = true)]
        public string GetCompaniesByGroup(int groupID)
        {
            var objs = data.t_Company.Where(c=>c.GroupID == groupID);
            clsCompanySet companies = new clsCompanySet();
            clsCompany company;

            foreach (var obj in objs)
            {
                company = new clsCompany();

                company.GroupID = (int)obj.GroupID;
                company.ID = obj.ID;
                company.IntroducerID = (int)obj.IntroducerID;
                company.ListPoint = (int)obj.ListPoint;
                company.Name = obj.Name.Trim();
                company.ParentID = (int)company.ParentID;
                company.Type = (int)company.Type;

                companies.Add(company);
            }

            clsResult result = new clsResult();

            result.flag = companies.Count;
            result.items = companies;
            string output = JsonConvert.SerializeObject(result);

            return output;
        }

        [WebMethod(EnableSession = true)]
        public string GetCompany(int ID)
        {
            var objs = data.t_Company.Where(c => c.ID == ID);

            clsResult result = new clsResult();
            result.flag = -1;
            result.error = "cann't find the company";

            clsCompanyEx company = new clsCompanyEx();
            foreach (var obj in objs)
            {
                company.Address = obj.Address.Trim();
                company.GroupID = (int)obj.GroupID;
                company.ID = obj.ID;
                company.IntroducerID = (int)obj.IntroducerID;
                company.ListPoint = (int)obj.ListPoint;
                company.Name = obj.Name.Trim();
                company.ParentID = (int)obj.ParentID;
                company.ParentUID = obj.ParentUID;
                company.Type = (int)obj.Type;
                company.UID = obj.UID.Trim();

                result.flag = 1;
                result.error = "";
                result.items = company;
                break;
            }

            string output = JsonConvert.SerializeObject(result);

            return output;
        }

        [WebMethod(EnableSession = true)]
        public string DeleteCompany(int ID)
        {
            string strSQL = "Delete t_Company Where ID=" + ID;
            data.ExecuteCommand(strSQL);

            clsResult result = new clsResult();
            string output = JsonConvert.SerializeObject(result);

            return output;
        }

        [WebMethod(EnableSession = true)]
        public string AddCompany(string strCompanyEx)
        {
            clsCompanyEx company = JsonConvert.DeserializeObject<clsCompanyEx>(strCompanyEx);
            clsResult result = new clsResult();
            string output = JsonConvert.SerializeObject(result);

            t_Company tc = new t_Company();
            tc.Address = company.Address.Trim();
            tc.GroupID = company.GroupID;
            tc.IntroducerID = company.IntroducerID;
            tc.ListPoint = company.ListPoint;
            tc.Name = company.Name.Trim();
            tc.ParentID = company.ParentID;
            tc.ParentUID = company.ParentUID.Trim();
            tc.Type = company.Type;
            tc.UID = company.UID.Trim();

            data.t_Company.InsertOnSubmit(tc);
            data.SubmitChanges();

            company.ID = tc.ID;

            result.flag = tc.ID;
            output = JsonConvert.SerializeObject(result);
            return output;
        }

        [WebMethod(EnableSession = true)]
        public string UpdateCompany(string strCompanyEx)
        {
            clsCompanyEx company = JsonConvert.DeserializeObject<clsCompanyEx>(strCompanyEx);

            var obj = data.t_Company.First(c => c.ID == company.ID);

            clsResult result = new clsResult();
            if (obj == null)
            {
                result.flag = -1;
                result.error = "cann't find the company";
            }
            else
            {
                obj.Address = company.Address.Trim();
                obj.GroupID = company.GroupID;
                obj.IntroducerID = company.IntroducerID;
                obj.ListPoint = company.ListPoint;
                obj.Name = company.Name.Trim();
                obj.ParentID = company.ParentID;
                obj.ParentUID = company.UID.Trim();
                obj.Type = company.Type;
                obj.UID = company.UID.Trim();

                data.SubmitChanges();
            }

            string output = JsonConvert.SerializeObject(result);

            return output;
        }

        #endregion

        #region Priority Service

        [WebMethod(EnableSession = true)]
        public string GetPriority(int userID, int objectType, int objectID)
        {
            var objs = data.t_Priority.Where(p => p.ObjectID == objectID && p.ObjectType==objectType && p.UserID==userID);
            clsPriority priority = new clsPriority();

            clsResult result = new clsResult();
            result.flag = -1;
            result.error = "cann't find the priority";

            foreach (var obj in objs)
            {
                priority.ID = obj.ID;
                priority.ObjectID = (int)obj.ObjectID;
                priority.ObjectType = (int)obj.ObjectType;
                priority.Type = (int)obj.Type;
                priority.UserID = (int)obj.UserID;

                result.flag = 1;
                result.error = "";
                break;
            }

            result.flag = 1;
            result.error = "";
            result.items = priority;
            string output = JsonConvert.SerializeObject(result);

            return output;
        }

        [WebMethod(EnableSession = true)]
        public string GetPriorities(int objectType, int objectID)
        {
            var objs = data.t_Priority.Where(p => p.ObjectID == objectID && p.ObjectType == objectType);
            clsPrioritySet priorities = new clsPrioritySet();
            clsPriority priority;

            foreach (var obj in objs)
            {
                priority = new clsPriority();
                priority.ID = obj.ID;
                priority.ObjectID = (int)obj.ObjectID;
                priority.ObjectType = (int)obj.ObjectType;
                priority.Type = (int)obj.Type;
                priority.UserID = (int)obj.UserID;

                priorities.Add(priority);
            }

            clsResult result = new clsResult();

            result.flag = priorities.Count;
            result.items = priorities;
            string output = JsonConvert.SerializeObject(result);

            return output;
        }

        [WebMethod(EnableSession = true)]
        public string AddPriority(string strPriority)
        {
            clsPriority priority = JsonConvert.DeserializeObject<clsPriority>(strPriority);
            clsResult result = new clsResult();
            string output = JsonConvert.SerializeObject(result);

            t_Priority tp = new t_Priority();
            tp.ObjectID = priority.ObjectID;
            tp.ObjectType = priority.ObjectType;
            tp.Type = priority.Type;
            tp.UserID = priority.UserID;

            data.t_Priority.InsertOnSubmit(tp);
            data.SubmitChanges();

            priority.ID = tp.ID;

            result.flag = tp.ID;
            output = JsonConvert.SerializeObject(result);
            return output;
        }

        [WebMethod(EnableSession = true)]
        public string UpdatePriority(string strPriority)
        {
            clsPriority priority = JsonConvert.DeserializeObject<clsPriority>(strPriority);

            var obj = data.t_Priority.First(c => c.ID == priority.ID);

            clsResult result = new clsResult();
            if (obj == null)
            {
                result.flag = -1;
                result.error = "cann't find the priority";
            }
            else
            {
                obj.ObjectID = priority.ObjectID;
                obj.ObjectType = priority.ObjectType;
                obj.Type = priority.Type;
                obj.UserID = priority.UserID;

                data.SubmitChanges();
            }

            string output = JsonConvert.SerializeObject(result);

            return output;
        }

        [WebMethod(EnableSession = true)]
        public string DeletePriority(int ID)
        {
            string strSQL = "Delete t_Priority Where ID=" + ID;
            data.ExecuteCommand(strSQL);

            clsResult result = new clsResult();
            string output = JsonConvert.SerializeObject(result);

            return output;
        }

        #endregion

        #region Product Service

        [WebMethod(EnableSession = true)]
        public string GetProductSerieses(int companyID)
        {
            var objs = data.t_ProductSeries.Where(p=>p.CompanyID == companyID);
            clsProductSeriesSet productSerieses = new clsProductSeriesSet();
            clsProductSeries productSeries;

            foreach (var obj in objs)
            {
                productSeries = new clsProductSeries();
                productSeries.CompanyID = (int)obj.CompanyID;
                productSeries.EnglishName = obj.EnglishName.Trim();
                productSeries.ID = obj.ID;
                productSeries.Name = obj.Name.Trim();
                productSeries.ParentID = (int)obj.ParentID;

                productSerieses.Add(productSeries);
            }

            clsResult result = new clsResult();

            result.flag = productSerieses.Count;
            result.items = productSerieses;
            string output = JsonConvert.SerializeObject(result);

            return output;
        }

        [WebMethod(EnableSession = true)]
        public string GetProductSeriesesByParent(int parentID)
        {
            var objs = data.t_ProductSeries.Where(p => p.CompanyID == parentID);
            clsProductSeriesSet productSerieses = new clsProductSeriesSet();
            clsProductSeries productSeries;

            foreach (var obj in objs)
            {
                productSeries = new clsProductSeries();
                productSeries.CompanyID = (int)obj.CompanyID;
                productSeries.EnglishName = obj.EnglishName.Trim();
                productSeries.ID = obj.ID;
                productSeries.Name = obj.Name.Trim();
                productSeries.ParentID = (int)obj.ParentID;

                productSerieses.Add(productSeries);
            }

            clsResult result = new clsResult();

            result.flag = productSerieses.Count;
            result.items = productSerieses;
            string output = JsonConvert.SerializeObject(result);

            return output;
        }

        private clsProductSeries GetProductSeries(int companyID, string name, string englishName)
        {
            clsProductSeries productSeries = null;

            name = name.Trim();
            if (name != "")
            {
                var objs = data.t_ProductSeries.Where(s => s.CompanyID == companyID && s.Name == name);
                foreach (var obj in objs)
                {
                    productSeries = new clsProductSeries();
                    productSeries.CompanyID = (int)obj.CompanyID;
                    productSeries.EnglishName = obj.EnglishName.Trim();
                    productSeries.ID = obj.ID;
                    productSeries.Name = obj.Name.Trim();
                    productSeries.ParentID = (int)obj.ParentID;

                    return productSeries;
                }
            }

            englishName = englishName.Trim();
            if (englishName != "")
            {
                var objs = data.t_ProductSeries.Where(s => s.CompanyID == companyID && s.EnglishName == englishName);
                foreach (var obj in objs)
                {
                    productSeries = new clsProductSeries();
                    productSeries.CompanyID = (int)obj.CompanyID;
                    productSeries.EnglishName = obj.EnglishName.Trim();
                    productSeries.ID = obj.ID;
                    productSeries.Name = obj.Name.Trim();
                    productSeries.ParentID = (int)obj.ParentID;

                    return productSeries;
                }
            }

            return productSeries;
        }

        [WebMethod(EnableSession = true)]
        public string AddProductSeries(string strProductSeries)
        {
            clsProductSeries series = JsonConvert.DeserializeObject<clsProductSeries>(strProductSeries);
            clsResult result = new clsResult();
            string output = JsonConvert.SerializeObject(result);

            clsProductSeries check = GetProductSeries(series.CompanyID, series.Name, series.EnglishName);
            if (check != null)
            {
                result.flag = -1;
                result.error = "the series name or english name had been used.";
                output = JsonConvert.SerializeObject(result);
                return output;
            }

            t_ProductSeries tp = new t_ProductSeries();
            tp.CompanyID = series.CompanyID;
            tp.EnglishName = series.EnglishName.Trim();
            tp.Name = series.Name.Trim();
            tp.ParentID = series.ParentID;

            data.t_ProductSeries.InsertOnSubmit(tp);
            data.SubmitChanges();

            series.ID = tp.ID;

            result.flag = tp.ID;
            output = JsonConvert.SerializeObject(result);
            return output;
        }

        [WebMethod(EnableSession = true)]
        public string UpdateProductSeries(string strProductSeries)
        {
            clsProductSeries series = JsonConvert.DeserializeObject<clsProductSeries>(strProductSeries);

            var obj = data.t_ProductSeries.First(c => c.ID == series.ID);

            clsResult result = new clsResult();
            if (obj == null)
            {
                result.flag = -1;
                result.error = "cann't find the product series";
            }
            else
            {
                obj.CompanyID = series.CompanyID;
                obj.EnglishName = series.EnglishName.Trim();
                obj.Name = series.Name.Trim();
                obj.ParentID = series.ParentID;
                
                data.SubmitChanges();
            }

            string output = JsonConvert.SerializeObject(result);

            return output;
        }

        [WebMethod(EnableSession = true)]
        public string DeleteProductSeries(int ID)
        {
            string strSQL = "Delete t_ProductSeries Where ID=" + ID;
            data.ExecuteCommand(strSQL);

            clsResult result = new clsResult();
            string output = JsonConvert.SerializeObject(result);

            return output;
        }

        [WebMethod(EnableSession = true)]
        public string GetProductModes(int productSeriesID)
        {
            var objs = data.t_ProductMode.Where(p=>p.SeriesID == productSeriesID);
            clsProductModeSet productModes = new clsProductModeSet();
            clsProductMode productMode;

            foreach (var obj in objs)
            {
                productMode = new clsProductMode();

                productMode.EnglishName = obj.EnglishName.Trim();
                productMode.ID = obj.ID;
                productMode.Name = obj.Name.Trim();
                productMode.SeriesID = (int)obj.SeriesID;

                productModes.Add(productMode);
            }

            clsResult result = new clsResult();

            result.flag = productModes.Count;
            result.items = productModes;
            string output = JsonConvert.SerializeObject(result);
          
            return output;
        }

        private clsProductMode GetProductMode(int seriesID, string name, string englishName)
        {
            clsProductMode productMode = null;

            name = name.Trim();
            if (name != "")
            {
                var objs = data.t_ProductMode.Where(m => m.SeriesID == seriesID && m.Name == name);
                foreach (var obj in objs)
                {
                    productMode.EnglishName = obj.EnglishName.Trim();
                    productMode.ID = obj.ID;
                    productMode.Name = obj.Name.Trim();
                    productMode.SeriesID = (int)obj.SeriesID;

                    return productMode;
                }
            }

            englishName = englishName.Trim();
            if (englishName != "")
            {
                var objs = data.t_ProductMode.Where(m => m.SeriesID == seriesID && m.EnglishName == englishName);
                foreach (var obj in objs)
                {
                    productMode.EnglishName = obj.EnglishName.Trim();
                    productMode.ID = obj.ID;
                    productMode.Name = obj.Name.Trim();
                    productMode.SeriesID = (int)obj.SeriesID;

                    return productMode;
                }
            }

            return productMode;
        }

        [WebMethod(EnableSession = true)]
        public string AddProductMode(string strProductMode)
        {
            clsProductMode mode = JsonConvert.DeserializeObject<clsProductMode>(strProductMode);
            clsResult result = new clsResult();
            string output = JsonConvert.SerializeObject(result);

            clsProductMode check = GetProductMode(mode.SeriesID, mode.Name, mode.EnglishName);
            if (check != null)
            {
                result.flag = -1;
                result.error = "the product mode name or english name had been used.";
                output = JsonConvert.SerializeObject(result);
                return output;
            }

            t_ProductMode tp = new t_ProductMode();
            tp.EnglishName = mode.EnglishName.Trim();
            tp.Name = mode.Name.Trim();
            tp.SeriesID = mode.SeriesID;

            data.t_ProductMode.InsertOnSubmit(tp);
            data.SubmitChanges();

            mode.ID = tp.ID;

            result.flag = tp.ID;
            output = JsonConvert.SerializeObject(result);
            return output;
        }

        [WebMethod(EnableSession = true)]
        public string UpdateProductMode(string strProductMode)
        {
            clsProductMode mode = JsonConvert.DeserializeObject<clsProductMode>(strProductMode);

            var obj = data.t_ProductMode.First(c => c.ID == mode.ID);

            clsResult result = new clsResult();
            if (obj == null)
            {
                result.flag = -1;
                result.error = "cann't find the product mode";
            }
            else
            {
                obj.EnglishName = mode.EnglishName.Trim();
                obj.Name = mode.Name.Trim();
                obj.SeriesID = mode.SeriesID;

                data.SubmitChanges();
            }

            string output = JsonConvert.SerializeObject(result);

            return output;
        }

        [WebMethod(EnableSession = true)]
        public string DeleteProductMode(int ID)
        {
            string strSQL = "Delete t_ProductMode Where ID=" + ID;
            data.ExecuteCommand(strSQL);

            clsResult result = new clsResult();
            string output = JsonConvert.SerializeObject(result);

            return output;
        }

        [WebMethod(EnableSession = true)]
        public string GetProductNum(int productModeID)
        {
            clsResult result = new clsResult();
            result.flag = data.t_Product.Count(p => p.ModeID == productModeID);

            string output = JsonConvert.SerializeObject(result);

            return output;
        }

        [WebMethod(EnableSession = true)]
        public string GetProducts(int productModeID, int start, int num)
        {
            var objs = data.t_Product.Where(p=>p.ModeID == productModeID).Skip(start).Take(num); 
            clsProductSet products = new clsProductSet();
            clsProduct product;
            foreach (var obj in objs)
            {
                product = new clsProduct();

                product.ID = obj.ID;
                product.ModeID = (int)obj.ModeID;
                product.SN = obj.SN.Trim();

                products.Add(product);
            }

            clsResult result = new clsResult();

            result.flag = products.Count;
            result.items = products;
            string output = JsonConvert.SerializeObject(result);

            return output;
        }

        private clsProduct GetProduct(int modeID, string SN)
        {
            clsProduct product = null;
            SN = SN.Trim();

            var objs = data.t_Product.Where(p => p.ModeID == modeID && p.SN == SN);
            foreach (var obj in objs)
            {
                product = new clsProduct();
                product.ID = obj.ID;
                product.ModeID = (int)obj.ModeID;
                product.SN = obj.SN.Trim();

                break;
            }

            return product;
        }

        [WebMethod(EnableSession = true)]
        public string AddProduct(string strProduct)
        {
            clsProduct product = JsonConvert.DeserializeObject<clsProduct>(strProduct);
            clsResult result = new clsResult();
            string output = JsonConvert.SerializeObject(result);

            clsProduct check = GetProduct(product.ModeID, product.SN);
            if (check != null)
            {
                result.flag = -1;
                result.error = "the product SN had been used.";
                output = JsonConvert.SerializeObject(result);
                return output;
            }

            t_Product tp = new t_Product();
            tp.ModeID = product.ModeID;
            tp.SN = product.SN.Trim();

            data.t_Product.InsertOnSubmit(tp);
            data.SubmitChanges();

            product.ID = tp.ID;

            result.flag = tp.ID;
            output = JsonConvert.SerializeObject(result);
            return output;
        }

        [WebMethod(EnableSession = true)]
        public string UpdateProduct(string strProduct)
        {
            clsProduct product = JsonConvert.DeserializeObject<clsProduct>(strProduct);

            var obj = data.t_Product.First(c => c.ID == product.ID);

            clsResult result = new clsResult();
            if (obj == null)
            {
                result.flag = -1;
                result.error = "cann't find the product";
            }
            else
            {
                obj.ModeID = product.ModeID;
                obj.SN = product.SN.Trim();

                data.SubmitChanges();
            }

            string output = JsonConvert.SerializeObject(result);

            return output;
        }

        [WebMethod(EnableSession = true)]
        public string DeleteProduct(int ID)
        {
            string strSQL = "Delete t_Product Where ID=" + ID;
            data.ExecuteCommand(strSQL);

            clsResult result = new clsResult();
            string output = JsonConvert.SerializeObject(result);

            return output;
        }

        #endregion

        #region Log Service

        [WebMethod(EnableSession = true)]
        public string GetLogs(int objectType, int objectID)
        {
            var objs = data.t_Log.Where(l=>l.ObjectID == objectID && l.ObjectType==objectType);
            clsLogSet logs = new clsLogSet();
            clsLog log;

            foreach (var obj in objs)
            {
                log = new clsLog();

                log.ID = obj.ID;
                log.Action = (int)obj.Action;
                log.Date = (DateTime)obj.Date;
                log.Description = obj.Descrition.Trim();
                log.IP = obj.IP.Trim();
                log.ObjectID = (int)obj.ObjectID;
                log.ObjectType = (int)obj.ObjectType;
                log.UserID = (int)obj.UserID;

                logs.Add(log);
            }

            clsResult result = new clsResult();

            result.flag = logs.Count;
            result.items = logs;
            string output = JsonConvert.SerializeObject(result);

            return output;
        }

        [WebMethod(EnableSession = true)]
        public string AddLog(string strLog)
        {
            clsLog log = JsonConvert.DeserializeObject<clsLog>(strLog);
            clsResult result = new clsResult();
            string output = JsonConvert.SerializeObject(result);

            t_Log tl = new t_Log();
            tl.Action = log.Action;
            tl.Date = DateTime.Now;
            tl.Descrition = log.Description.Trim();
            tl.IP = log.IP.Trim();
            tl.ObjectID = log.ObjectID;
            tl.ObjectType = log.ObjectType;
            tl.UserID = log.UserID;

            data.t_Log.InsertOnSubmit(tl);
            data.SubmitChanges();

            log.ID = tl.ID;

            result.flag = tl.ID;
            output = JsonConvert.SerializeObject(result);
            return output;
        }

        [WebMethod(EnableSession = true)]
        public string DeleteLogALL()
        {
            string strSQL = "Delete t_Log";
            data.ExecuteCommand(strSQL);

            clsResult result = new clsResult();
            string output = JsonConvert.SerializeObject(result);

            return output;
        }

        [WebMethod(EnableSession = true)]
        public string DeleteObjectLogs(int objectType, int objectID)
        {
            string strSQL = "Delete t_Log Where ObjectID=" + objectID + " And ObjectType=" + objectType;
            data.ExecuteCommand(strSQL);

            clsResult result = new clsResult();
            string output = JsonConvert.SerializeObject(result);

            return output;
        }

        [WebMethod(EnableSession = true)]
        public string DeleteUserLogs(int userID)
        {
            string strSQL = "Delete t_Log Where UserID=" + userID;
            data.ExecuteCommand(strSQL);

            clsResult result = new clsResult();
            string output = JsonConvert.SerializeObject(result);

            return output;
        }

        #endregion

        #region District Service

        [WebMethod(EnableSession = true)]
        public string GetCountryInfo()
        {
            string fileName = Server.MapPath("data\\XML\\Regionalism.xml");
            clsCountry country = new clsCountry();
            country.Initialize(fileName);

            clsResult result = new clsResult();
            result.items = country.Provinces;
            result.flag = country.Provinces.Count;

            string output = JsonConvert.SerializeObject(result);

            return output;
        }

        #endregion
    }
}
