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

        ~clsColumn() { }
    }

    public class clsColumnEx
    {
        public int ID {get;set;}
        public int ParentID { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Brief { get; set; }

        public clsColumnEx()
        {
            ID = 0;
            ParentID = 0;
            Name = "";
            Code = "";
            Brief = "";
        }

        ~clsColumnEx() { }
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

        public clsGroupEx()
        {
            ID = 0;
            ParentID = 0;
            Name = "";
            Code = "";
            Brief = "";
        }

        ~clsGroupEx() { }
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
        }

        ~clsResourceEx()
        {
        }
    }
}