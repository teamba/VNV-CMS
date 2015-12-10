using System;
using System.Collections;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace vnv_wcf
{
    public class clsColumn
    {
        [DataMember]
        public int ID {get;set;}
        [DataMember]
        public int ParentID { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Code { get; set; }
        [DataMember]
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
        [DataMember]
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
}