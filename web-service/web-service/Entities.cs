using System;
using System.Collections;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using System.Text;

namespace web_service
{
    public class clsColumn
    {
        public int ID {get;set;}
        public int ParentID { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }

        public clsColumn()
        {
            ID = 0;
            ParentID = 0;
            Name = "";
            Code = "";
        }

        ~clsColumn() {}
    }

    public class clsColumnEx
    {
        public int ID {get;set;}
        public int ParentID { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Brief { get; set; }

        public int ContentType { get; set; }
        public string Template { get; set; }
        public string SEO_Keyword { get; set; }
        public string SEO_Title { get; set; }
        public string SEO_Description { get; set; }

        public clsPropertySet Properties { get; set; }

        public clsColumnEx()
        {
            ID = 0;
            ParentID = 0;
            Name = "";
            Code = "";
            Brief = "";

            ContentType = 1;
            Template = "";
            SEO_Description = "";
            SEO_Keyword = "";
            SEO_Title = "";

            Properties = new clsPropertySet();
        }

        ~clsColumnEx() 
        {
            Properties.Clear();
        }
    }

    public class clsColumnSet : CollectionBase
    {
        public clsColumnSet() { }

        ~clsColumnSet()
        {
            List.Clear();
        }

        public clsColumn this[int index]
        {
            get
            {
                if (index >= 0 && index < List.Count) return (clsColumn)List[index];
                else return null;
            }

            set
            {
                if (index >= 0 && index < List.Count) List[index] = value;
            }
        }

        public void Add(clsColumn column)
        {
            List.Add(column);
        }
    }

    public class clsResult
    {
        public int flag;
        public string error;

        public Object items;

        public clsResult()
        {
            flag = 0;
            error = "";

            items = null;
        }

        ~clsResult()
        {
        }
    }

    public class clsGroup
    {
        public int ID { get; set; }
        public int ParentID { get; set; }

        // 1--article, 2--photo
        public int Type { get; set; }
        public string Name { get; set; }

        public clsGroup()
        {
            ID = 0;
            ParentID = 0;
            Type = 0;
            Name = "";
        }

        ~clsGroup()
        {
        }
    }

    public class clsGroupSet : CollectionBase
    {
        ~clsGroupSet()
        {
            List.Clear();
        }

        public clsGroup this[int index]
        {
            get
            {
                if (index >= 0 && index < List.Count) return (clsGroup)List[index];
                else return null;
            }

            set
            {
                if (index >= 0 && index < List.Count) List[index] = value;
            }
        }

        public void Add(clsGroup group)
        {
            List.Add(group);
        }
    }

    public class clsGroupEx
    {
        public int ID { get; set; }
        public int ParentID { get; set; }
        public int Type { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Brief { get; set; }

        public clsPropertySet Properties { get; set; }

        public clsGroupEx()
        {
            ID = 0;
            ParentID = 0;
            Name = "";
            Code = "";
            Brief = "";

            Properties = new clsPropertySet();
        }

        ~clsGroupEx() 
        {
            Properties.Clear();
        }
    }

    public class clsResource
    {
        public int ID { get; set; }
        public int GroupID { get; set; }
        public int Type { get; set; }
        public string Title { get; set; }
        public string CreateDate { get; set; }

        public clsResource()
        {
            ID = 0;
            GroupID = 0;
            Type = 1;
            Title = "";
            CreateDate = "";
        }

        ~clsResource()
        {
        }
    }

    public class clsResourceSet : CollectionBase
    {
        ~clsResourceSet()
        {
            List.Clear();
        }

        public clsResource this[int index]
        {
            get
            {
                if (index >= 0 && index < List.Count) return (clsResource)List[index];
                else return null;
            }

            set
            {
                if (index >= 0 && index < List.Count) List[index] = value;
            }
        }

        public void Add(clsResource resource)
        {
            List.Add(resource);
        }
    }

    public class clsResourceEx
    {
        public int ID { get; set; }
        public int GroupID { get; set; }
        public int Type { get; set; }
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public string Author { get; set; }
        public string Source { get; set; }
        public string Brief { get; set; }
        public int Status { get; set; }
        public DateTime CreateDate { get; set; }
        public string Content { get; set; }
        public string UID { get; set; }
        public string ParentUID { get; set; }
        public string GroupUID { get; set; }

        public clsPropertySet Properties { get; set; }

        public clsResourceEx()
        {
            ID = 0;
            GroupID = 0;
            Type = 1;
            Title = "";
            CreateDate = DateTime.Now;

            SubTitle = "";
            Author = "";
            Source = "";
            Brief = "";
            Status = 0;
            Content = "";
            UID = "";
            ParentUID = "";
            GroupUID = "";

            Properties = new clsPropertySet();
        }

        ~clsResourceEx()
        {
            Properties.Clear();
        }
    }

    public class clsColumnUpdate
    {
        public int ID { get; set; }
        public int ColumnID { get; set; }
        public int OwnerID { get; set; }
        public DateTime CreateDate { get; set; }

        /* 状态定义
         * 0 -- 处于编辑状态，每个栏目只能有一个0
         * 1 -- 处于已发布状态，每个栏目只能有一个1
         * 2 -- 历史发布
         * 
         * 状态转换
         * 0 --> 1 发布编辑状态的内容
         * 0 --> 2 新增编辑内容，或者其它内容转为编辑状态
         * 
         * 1 --> 2 别的内容发布，原内容转为历史内容
         * 1 --> 0 已经发布的内容重编辑，则新增正在编辑的内容，保留已经发布的内容，原0转为2
         * 
         * 2 --> 0 旧内容重新编辑，原0转为2
         * 2 --> 1 旧内容重新发布，原1转为2
         */
        public int Status { get; set; }

        public clsPropertySet Properties { get; set; }

        public clsColumnUpdate()
        {
            ID = 0;
            ColumnID = 0;
            OwnerID = 0;
            CreateDate = DateTime.Now;
            Status = 0;

            Properties = new clsPropertySet();
        }

        ~clsColumnUpdate()
        {
            Properties.Clear();
        }
    }

    public class clsColumnUpdateSet : CollectionBase
    {
        ~clsColumnUpdateSet()
        {
            List.Clear();
        }

        public clsColumnUpdate this[int index]
        {
            get
            {
                if (index >= 0 && index < List.Count) return (clsColumnUpdate)List[index];
                else return null;
            }

            set
            {
                if (index >= 0 && index < List.Count) List[index] = value;
            }
        }

        public void Add(clsColumnUpdate columnUpdate)
        {
            List.Add(columnUpdate);
        }
    }

    public class clsUpdateItem
    {
        public int ID { get; set; }
        public int UpdateID { get; set; }
        public int ResourceID { get; set; }
        public string Title { get; set; }
        public int ListPoint { get; set; }
        public string Brief { get; set; }

        public clsPropertySet Properties { get; set; }

        public clsUpdateItem()
        {
            ID = 0;
            UpdateID = 0;
            ResourceID = 0;
            Title = "";
            ListPoint = 0;
            Brief = "";

            Properties = new clsPropertySet();
        }

        ~clsUpdateItem()
        {
            Properties.Clear();
        }
    }

    public class clsUpdateItemSet : CollectionBase
    {
        ~clsUpdateItemSet()
        {
            List.Clear();
        }

        public clsUpdateItem this[int index]
        {
            get
            {
                if (index >= 0 && index < List.Count) return (clsUpdateItem)List[index];
                else return null;
            }

            set
            {
                if (index >= 0 && index < List.Count) List[index] = value;
            }
        }

        public void Add(clsUpdateItem updateItem)
        {
            List.Add(updateItem);
        }

        public bool Exist(int resourceID)
        {
            for (int i = 0; i < List.Count; i++) if (this[i].ResourceID == resourceID) return true;

            return false;
        }
    }

    public class clsProperty
    {
        public int ID { get; set; }

        /// <summary>
        /// 1-column, 2-group, 3-resource, 4-update, 5-update item
        /// </summary>
        public int ObjectType { get; set; }
        public int ObjectID { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }

        public clsProperty()
        {
            ID = 0;
            ObjectID = 0;
            ObjectType = 0;
            Key = "";
            Value = "";
        }

        ~clsProperty()
        {
        }
    }

    public class clsPropertySet : CollectionBase
    {
        public clsPropertySet()
        {
        }

        ~clsPropertySet()
        {
            List.Clear();
        }

        public clsProperty this[int index]
        {
            get
            {
                if (index >= 0 && index < List.Count) return (clsProperty)List[index];
                else return null;
            }

            set
            {
                if (index >= 0 && index < List.Count) List[index] = value;
            }
        }

        public void Add(clsProperty property)
        {
            List.Add(property);
        }
    }
}