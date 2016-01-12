using System;
using System.Collections;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;

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

    public class clsUser
    {
        public int ID { get; set; }
        public int GroupID { get; set; }
        public int CompanyID { get; set; }
        public string Name { get; set; }
        public string Account { get; set; }

        public clsUser()
        {
            ID = 0;
            GroupID = 0;
            CompanyID = 0;
            Name = "";
            Account = "";
        }

        ~clsUser()
        {
        }
    }

    public class clsUserSet : CollectionBase
    {
        public clsUserSet()
        {
        }

        ~clsUserSet()
        {
            List.Clear();
        }

        public clsUser this[int index]
        {
            get
            {
                if (index >= 0 && index < List.Count) return (clsUser)List[index];
                else return null;
            }

            set
            {
                if (index >= 0 && index < List.Count) List[index] = value;
            }
        }

        public void Add(clsUser user)
        {
            List.Add(user);
        }
    }

    public class clsUserEx
    {
        public int ID { get; set; }
        public int GroupID { get; set; }
        public int CompanyID { get; set; }
        public string Name { get; set; }
        public string Account { get; set; }

        public string UID { get; set; }
        public string Password { get; set; }
        public DateTime CreateDate { get; set; }
        public int Type { get; set; }
        public int Status { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Sex { get; set; }
        public int IntroducerID { get; set; }
        public string CellPhone { get; set; }
        public string eMail { get; set; }
        public string Address { get; set; }

        public clsUserEx()
        {
            ID = 0;
            GroupID = 0;
            CompanyID = 0;
            Name = "";
            Account = "";

            UID = "";
            Password = "";
            CreateDate = DateTime.Now;
            Type = 0;
            Status = 0;
            FirstName = "";
            LastName = "";
            Sex = 0;
            IntroducerID = 0;
            CellPhone = "";
            eMail = "";
            Address = "";
        }

        ~clsUserEx()
        {
        }
    }

    public class clsCompany
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int GroupID { get; set; }
        public int ParentID { get; set; }
        public int Type { get; set; }
        public int IntroducerID { get; set; }
        public int ListPoint { get; set; }

        public clsCompany()
        {
            ID = 0;
            Name = "";
            GroupID = 0;
            ParentID = 0;
            Type = 0;
            IntroducerID = 0;
            ListPoint = 0;
        }

        ~clsCompany()
        {
        }
    }

    public class clsCompanySet : CollectionBase
    {
        public clsCompanySet()
        {
        }

        ~clsCompanySet()
        {
            List.Clear();
        }

        public clsCompany this[int index]
        {
            get
            {
                if (index >= 0 && index < List.Count) return (clsCompany)List[index];
                else return null;
            }

            set
            {
                if (index >= 0 && index < List.Count) List[index] = value;
            }
        }

        public void Add(clsCompany company)
        {
            List.Add(company);
        }
    }

    public class clsCompanyEx
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int GroupID { get; set; }
        public int ParentID { get; set; }
        public int Type { get; set; }
        public int IntroducerID { get; set; }
        public int ListPoint { get; set; }

        public string UID { get; set; }
        public string Address { get; set; }
        public string ParentUID { get; set; }

        public clsCompanyEx()
        {
            ID = 0;
            Name = "";
            GroupID = 0;
            ParentID = 0;
            Type = 0;
            IntroducerID = 0;
            ListPoint = 0;

            UID = "";
            Address = "";
            ParentUID = "";
        }

        ~clsCompanyEx()
        {
        }
    }

    public class clsPriority
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public int ObjectType { get; set; }
        public int ObjectID { get; set; }
        public int Type { get; set; }

        public clsPriority()
        {
            ID = 0;
            UserID = 0;
            ObjectType = 0;
            ObjectID = 0;
            Type = 0;
        }

        ~clsPriority()
        {
        }
    }

    public class clsPrioritySet : CollectionBase
    {
        public clsPrioritySet()
        {
        }

        ~clsPrioritySet()
        {
            List.Clear();
        }

        public clsPriority this[int index]
        {
            get
            {
                if (index >= 0 && index < List.Count) return (clsPriority)List[index];
                else return null;
            }

            set
            {
                if (index >= 0 && index < List.Count) List[index] = value;
            }
        }

        public void Add(clsPriority priority)
        {
            List.Add(priority);
        }
    }

    public class clsProductSeries
    {
        public int ID { get; set; }
        public int ParentID { get; set; }
        public string Name { get; set; }
        public string EnglishName { get; set; }
        public int CompanyID { get; set; }

        public clsProductSeries()
        {
            ID = 0;
            ParentID = 0;
            Name = "";
            EnglishName = "";
            CompanyID = 0;
        }

        ~clsProductSeries()
        {
        }
    }

    public class clsProductSeriesSet : CollectionBase
    {
        public clsProductSeriesSet()
        {
        }

        ~clsProductSeriesSet()
        {
            List.Clear();
        }

        public clsProductSeries this[int index]
        {
            get
            {
                if (index >= 0 && index < List.Count) return (clsProductSeries)List[index];
                else return null;
            }

            set
            {
                if (index >= 0 && index < List.Count) List[index] = value;
            }
        }

        public void Add(clsProductSeries productSeries)
        {
            List.Add(productSeries);
        }
    }

    public class clsProductMode
    {
        public int ID { get; set; }
        public int SeriesID { get; set; }
        public string Name { get; set; }
        public string EnglishName { get; set; }

        public clsProductMode()
        {
            ID = 0;
            SeriesID = 0;
            Name = "";
            EnglishName = "";
        }

        ~clsProductMode()
        {
        }
    }

    public class clsProductModeSet : CollectionBase
    {
        public clsProductModeSet()
        {
        }

        ~clsProductModeSet()
        {
            List.Clear();
        }

        public clsProductMode this[int index]
        {
            get
            {
                if (index >= 0 && index < List.Count) return (clsProductMode)List[index];
                else return null;
            }

            set
            {
                if (index >= 0 && index < List.Count) List[index] = value;
            }
        }

        public void Add(clsProductMode productMode)
        {
            List.Add(productMode);
        }
    }

    public class clsProduct
    {
        public int ID { get; set; }
        public int ModeID { get; set; }
        public string SN { get; set; }

        public clsProduct()
        {
            ID = 0;
            ModeID = 0;
            SN = "";
        }

        ~clsProduct()
        {
        }
    }

    public class clsProductSet : CollectionBase
    {
        public clsProductSet()
        {
        }

        ~clsProductSet()
        {
            List.Clear();
        }

        public clsProduct this[int index]
        {
            get
            {
                if (index >= 0 && index < List.Count) return (clsProduct)List[index];
                else return null;
            }

            set
            {
                if (index >= 0 && index < List.Count) List[index] = value;
            }
        }

        public void Add(clsProduct product)
        {
            List.Add(product);
        }
    }

    public class clsLog
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public int ObjectType { get; set; }
        public int ObjectID { get; set; }
        public int Action { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public string IP { get; set; }

        public clsLog()
        {
            ID = 0;
            UserID = 0;
            ObjectType = 0;
            ObjectID = 0;
            Action = 0;
            Description = "";
            IP = "";
        }

        ~clsLog()
        {
        }
    }

    public class clsLogSet : CollectionBase
    {
        public clsLogSet()
        {
        }

        ~clsLogSet()
        {
            List.Clear();
        }

        public clsLog this[int index]
        {
            get
            {
                if (index >= 0 && index < List.Count) return (clsLog)List[index];
                else return null;
            }

            set
            {
                if (index >= 0 && index < List.Count) List[index] = value;
            }
        }

        public void Add(clsLog log)
        {
            List.Add(log);
        }
    }

    public class clsCountry
    {
        public string Code { get; set; }
        public string Name { get; set; }

        public clsProvinceSet Provinces { get; set; }

        public clsCountry()
        {
            Code = "";
            Name = "";

            Provinces = new clsProvinceSet();
        }

        ~clsCountry()
        {
            Provinces.Clear();
        }

        public void Initialize(string pathFile)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(pathFile);

            XmlNode country = xmlDoc.SelectSingleNode("country");
            Code = country.Attributes["code"].Value;
            Name = country.Attributes["name"].Value;

            XmlNodeList list = country.ChildNodes;
            if (list!=null) Provinces.Initiaze(list);
        }
    }

    public class clsProvince
    {
        public string Code { get; set; }
        public string Name { get; set; }

        public clsCitySet Cities { get; set; }

        public clsProvince()
        {
            Code = "";
            Name = "";

            Cities = new clsCitySet();
        }

        ~clsProvince()
        {
            Cities.Clear();
        }
    }

    public class clsProvinceSet : CollectionBase
    {
        public clsProvinceSet()
        {
        }

        ~clsProvinceSet()
        {
            List.Clear();
        }

        public clsProvince this[int index]
        {
            get
            {
                if (index >= 0 && index < List.Count) return (clsProvince)List[index];
                else return null;
            }

            set
            {
                if (index >= 0 && index < List.Count) List[index] = value;
            }
        }

        public void Initiaze(XmlNodeList xmlList)
        {
            List.Clear();

            clsProvince province;
            foreach (XmlNode node in xmlList)
            {
                if (node.Name != "Province") continue;

                XmlNodeList list = node.ChildNodes;
                province = new clsProvince();
                province.Code = Convert.ToString(node.Attributes["code"].Value);
                province.Name = Convert.ToString(node.Attributes["pname"].Value);
                List.Add(province);

                if (list != null) province.Cities.Initiaze(list);
            }
        }

        public void Add(clsProvince province)
        {
            List.Add(province);
        }
    }

    public class clsCity
    {
        public string Code { get; set; }
        public string Name { get; set; }

        public clsCountySet Counnties { get; set; }

        public clsCity()
        {
            Code = "";
            Name = "";

            Counnties = new clsCountySet();
        }

        ~clsCity()
        {
            Counnties.Clear();
        }
    }

    public class clsCitySet : CollectionBase
    {
        public clsCitySet()
        {
        }

        ~clsCitySet()
        {
            List.Clear();
        }

        public clsCity this[int index]
        {
            get
            {
                if (index >= 0 && index < List.Count) return (clsCity)List[index];
                else return null;
            }

            set
            {
                if (index >= 0 && index < List.Count) List[index] = value;
            }
        }

        public void Initiaze(XmlNodeList xmlList)
        {
            List.Clear();

            clsCity city;
            foreach (XmlNode xn in xmlList)
            {
                if (xn.Name != "City") continue;

                city = new clsCity();
                city.Code = xn.Attributes["code"].Value;
                city.Name = xn.Attributes["cname"].Value;

                Add(city);

                XmlNodeList list = xn.ChildNodes;
                if (list != null) city.Counnties.Initiaze(list);
            }
        }

        public void Add(clsCity city)
        {
            List.Add(city);
        }
    }

    public class clsCounty
    {
        public string Code { get; set; }
        public string Name { get; set; }

        public clsCounty()
        {
            Code = "";
            Name = "";
        }
    }

    public class clsCountySet : CollectionBase
    {
        public clsCountySet()
        {
        }

        ~clsCountySet()
        {
            List.Clear();
        }

        public clsCounty this[int index]
        {
            get
            {
                if (index >= 0 && index < List.Count) return (clsCounty)List[index];
                else return null;
            }

            set
            {
                if (index >= 0 && index < List.Count) List[index] = value;
            }
        }

        public void Initiaze(XmlNodeList xmlList)
        {
            List.Clear();

            clsCounty county;
            foreach (XmlNode xn in xmlList)
            {
                if (xn.Name != "County") continue;

                county = new clsCounty();
                county.Code = xn.Attributes["code"].Value;
                county.Name = xn.Attributes["name"].Value;

                Add(county);
            }
        }

        public void Add(clsCounty county)
        {
            List.Add(county);
        }
    }
}