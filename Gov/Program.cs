using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
namespace ConsoleApplication2
{
    class Program
    {

        public static string connectionString =
        ConfigurationManager.ConnectionStrings["DBConnection"].ConnectionString;


        public string PchomeTable = "PchomePrice";

        public string GovTable = "GovData";

        public string MixTable = "MixGovPcData";



        static void Main(string[] args)
        {
            LoadJson();
        }

        static string pchomeJson = "c:\\users\\tm731\\documents\\visual studio 2015\\Projects\\ConsoleApplication2\\ConsoleApplication2\\Pchome.json";
        static string tagJson = "c:\\users\\tm731\\documents\\visual studio 2015\\Projects\\ConsoleApplication2\\ConsoleApplication2\\Tag.json";
        public static void LoadJson()
        {
            var pchomeItems = new PriceStruct.PcHome();
            var tagItems = new PriceStruct.Tags();
            var joinItems = new List<PriceStruct.JoinProduct>();
            SqlConnection connect = new SqlConnection(connectionString);
            using (var connection = connect)
            {
                connection.Open();
                int count = 0;
                using (StreamReader r = new StreamReader(pchomeJson))
                {
                    var json = r.ReadToEnd();
                    pchomeItems = JsonConvert.DeserializeObject<PriceStruct.PcHome>(json);
                    using (var trans = connection.BeginTransaction())
                    {
                        foreach (var item in pchomeItems.product)
                        {
                            connection.Execute(Const.insertPchomeData, item, trans);
                            CheckCount(ref count);
                        }
                        trans.Commit();
                    }

                    Console.WriteLine("{0} {1}", "Done", "PCHOME");
                    Console.WriteLine($"{ pchomeItems.product.Count()}");
                }
                using (StreamReader r = new StreamReader(tagJson))
                {
                    var json = r.ReadToEnd();
                    tagItems = JsonConvert.DeserializeObject<PriceStruct.Tags>(json);
                    var ttB = new PriceStruct.TagsToDBProduct();
                    foreach (var item in tagItems.product)
                    {
                        ttB= ConvertToDB(item);
                        connection.Execute(Const.insertGovData, ttB);
                        CheckCount(ref count);
                    }

                    Console.WriteLine($"{ tagItems.product.Count()}");

                }

                foreach (var item in tagItems.product)
                {
                    var objectData = pchomeItems.product.Where(x => x.name.Contains(item.product_model)).Select(x => x).FirstOrDefault();
                    if (objectData != null)
                    {

                        connection.Execute(Const.insertMixData, FillAllData(item, objectData));
                        CheckCount(ref count);

                        joinItems.Add(FillAllData(item, objectData));
                    }

                }


                Console.WriteLine($"{ joinItems.Count()}");
            }
            Console.ReadLine();
        }

        private static PriceStruct.TagsToDBProduct ConvertToDB(PriceStruct.TagsProduct item)
        {
            var returnModel = new PriceStruct.TagsToDBProduct();

            returnModel.annual_power_consumption_degrees_dive_year = item.annual_power_consumption_degrees_dive_year;
            returnModel.detailUri = item.detailUri;
            returnModel.brand_name = item.brand_name;
            returnModel.efficiency_benchmark = item.efficiency_benchmark;
            returnModel.efficiency_rating = item.efficiency_rating;
            returnModel.from_date_of_expiration = item.from_date_of_expiration;
            returnModel.Id = item.Id;
            returnModel.labeling_company = item.labeling_company;
            returnModel.login_number = item.login_number;
            returnModel.product_model = item.product_model;
            returnModel.test_report_of_energy_efficiency =
             //(item.test_report_of_energy_efficiency).ToString();
            JsonConvert.SerializeObject(item.test_report_of_energy_efficiency);

            return returnModel;
        }

        private static void CheckCount(ref int count)
        {
            count++;
            if (count % 10 == 0) { Console.WriteLine($"{(count / 10) * 10}筆資料進入"); };
        }

        private static PriceStruct.JoinProduct FillAllData(PriceStruct.TagsProduct item, PriceStruct.PcHomeProduct objectData)
        {
            var joinProduct = new PriceStruct.JoinProduct();

            joinProduct.annual_power_consumption_degrees_dive_year = item.annual_power_consumption_degrees_dive_year;
            joinProduct.detailUri = item.detailUri;
            joinProduct.brand_name = item.brand_name;
            joinProduct.efficiency_benchmark = item.efficiency_benchmark;
            joinProduct.efficiency_rating = item.efficiency_rating;
            joinProduct.from_date_of_expiration = item.from_date_of_expiration;
            joinProduct.Id = item.Id;
            joinProduct.labeling_company = item.labeling_company;
            joinProduct.login_number = item.login_number;
            joinProduct.name = objectData.name;
            joinProduct.originprice = objectData.originprice;
            joinProduct.picb = objectData.picb;
            joinProduct.pics = objectData.pics;
            joinProduct.product_model = item.product_model;
            joinProduct.test_report_of_energy_efficiency =

             //(item.test_report_of_energy_efficiency).ToString();
            JsonConvert.SerializeObject(item.test_report_of_energy_efficiency);



            return joinProduct;
        }
    }
}
