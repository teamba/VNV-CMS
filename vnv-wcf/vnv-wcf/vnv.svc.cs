using System;
using System.Collections;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Newtonsoft.Json;

namespace vnv_wcf
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码、svc 和配置文件中的类名“vnv”。
    public class vnv : Ivnv
    {
        DataWebDataContext data = new DataWebDataContext();

        public void DoWork()
        {
        }

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
                column.Brief = obj.Brief.Trim();
            }

            string output = JsonConvert.SerializeObject(column);

            return output;
        }

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
                column.Brief = obj.Brief.Trim();

                columns.Add(column);
            }

            string output = JsonConvert.SerializeObject(columns);

            return output;
        }
    }
}
