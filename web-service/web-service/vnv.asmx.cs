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
            string strProperties = "[{\"ID\":0,\"ObjectType\":4,\"ObjectID\":\"3022\",\"Key\":\"width\",\"Value\":\"10\"},{\"ID\":0,\"ObjectType\":4,\"ObjectID\":\"3022\",\"Key\":\"height\",\"Value\":\"20\"},{\"ID\":0,\"ObjectType\":4,\"ObjectID\":\"3022\",\"Key\":\"radiao\",\"Value\":\"30\"}]";

            IList objs = JsonConvert.DeserializeObject<IList>(strProperties);
            clsPropertySet properties = new clsPropertySet();
            clsProperty property;
            for (int i = 0; i < objs.Count; i++)
            {
                property = JsonConvert.DeserializeObject<clsProperty>(Convert.ToString(objs[i]));
                properties.Add(property);
            }

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
    }
}
