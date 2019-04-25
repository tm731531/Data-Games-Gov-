using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication2
{
    class PriceStruct
    {

        public class PcHomeProduct
        {
            public string Id { get; set; }
            public string name { get; set; }
            public int originprice { get; set; }
            public string pics { get; set; }
            public string picb { get; set; }
        }

        public class PcHome

        {
            public string keyword { get; set; }
            public List<PcHomeProduct> product { get; set; }
        }



        public class TestReportOfEnergyEfficiency
        {
            public string EER { get; set; }
            public string CSPF { get; set; }
        }

        public class TagsProduct
        {
            public string Id { get; set; }
            public string product_model { get; set; }
            public string brand_name { get; set; }
            public string login_number { get; set; }
            public string detailUri { get; set; }
            public string labeling_company { get; set; }
            public string efficiency_rating { get; set; }
            public string from_date_of_expiration { get; set; }
            public TestReportOfEnergyEfficiency test_report_of_energy_efficiency { get; set; }
            public string efficiency_benchmark { get; set; }
            public string annual_power_consumption_degrees_dive_year { get; set; }
        }


        public class TagsToDBProduct
        {
            public string Id { get; set; }
            public string product_model { get; set; }
            public string brand_name { get; set; }
            public string login_number { get; set; }
            public string detailUri { get; set; }
            public string labeling_company { get; set; }
            public string efficiency_rating { get; set; }
            public string from_date_of_expiration { get; set; }
            public string to_date_of_expiration { get; set; }
            public string test_report_of_energy_efficiency { get; set; }
            public string efficiency_benchmark { get; set; }
            public string annual_power_consumption_degrees_dive_year { get; set; }
        }
        public class JoinProduct: TagsToDBProduct
        {
            public string name { get; set; }
            public int originprice { get; set; }
            public string pics { get; set; }
            public string picb { get; set; }
        }
        public class Tags
        {
            public List<TagsProduct> product { get; set; }
        }
    }
}
