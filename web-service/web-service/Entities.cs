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
        public string Brief { get; set; }

        public clsColumn()
        {
            ID = 0;
            ParentID = 0;
            Name = "";
            Code = "";
            Brief = "";
        }

        ~clsColumn() { }
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
}