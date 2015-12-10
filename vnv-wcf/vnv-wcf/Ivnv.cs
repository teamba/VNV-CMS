using System;
using System.Collections;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace vnv_wcf
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码和配置文件中的接口名“Ivnv”。
    [ServiceContract]
    public interface Ivnv
    {
        [OperationContract]
        void DoWork();

        [OperationContract]
        string GetColumn(int columnID);

        [OperationContract]
        string GetColumns(int parentID);
    }
}
