using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication2
{
    class PriceStruct
    {

        public class PchomePrice
        {
            public string Id { get; set; }
            public string name { get; set; }
            public int originprice { get; set; }
            public string pics { get; set; }
            public string picb { get; set; }
            public string key_word { get; set; }
            public string data_from { get; set; }
        }

        public class PcHome

        {
            public string keyword { get; set; }
            public List<PchomePrice> product { get; set; }
        }



        public class TestReportOfEnergyEfficiency
        {
            public string EER { get; set; }
            public string CSPF { get; set; }
            public string rated_dehumidification_capacity { get; set; }
            public string energy_factor_value { get; set; }
            public string est24 { get; set; }
            public string hot_water_system_storage_tank_capacity_indication_value { get; set; }
            public string warm_water_system_storage_tank_capacity_indication_value { get; set; }
            public string ice_water_system_storage_tank_capacity_indication_value { get; set; }
            
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
            public string key_word { get; set; }
        }
        public class JoinProduct: TagsToDBProduct
        {
            public string name { get; set; }
            public int originprice { get; set; }
            public string pics { get; set; }
            public string picb { get; set; }
            public string key_word { get; set; }
            public string Pchome_Id { get; set; }
            public string data_from { get; set; }
        }
        public class Tags
        {
            public List<TagsProduct> product { get; set; }
        }

        public class TempData {
            public string Id { get; set; }
            public string product_model { get; set; }
            public string product_category { get; set; }
            public string labeling_company { get; set; }
            public string benchmarks { get; set; }
            public int annual_power_consumption_degrees_dive_year { get; set; }
            public string Done { get; set; }
            public string Text { get; set; }
            public int par_p0 { get; set; }
            public int par_id { get; set; }
            public string r0 { get; set; }
            public string r1 { get; set; }
            public string r2 { get; set; }
            public string r3 { get; set; }
            public string r4 { get; set; }
            public string r5 { get; set; }
            public string r6 { get; set; }
            public string r7 { get; set; }

        }
    }
}
