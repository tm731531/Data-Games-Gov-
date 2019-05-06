using ConsoleApplication2.Common;
using Dapper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static ConsoleApplication2.PriceStruct;

namespace ConsoleApplication2.Method
{
    class CurlData
    {
        private static string PchomeUrl = "https://ecshweb.pchome.com.tw/search/v3.3/{0}/results?q={1}&page={2}&sort=sale/dc";
        private static string GovDataUrl = "https://ranking.energylabel.org.tw/product/Approval/upt.aspx?pageno=10&key2=&key=&con=0&pprovedateA=&pprovedateB=&approvedateA=&approvedateB=&Type=48&comp=0&RANK=0&refreA=0&refreB=0&condiA=0&condiB=0&HDA=0&HDB=0&SWHA=0&SWHB=0&p0={0}&id={1}";
        private static object o = new object();

        internal static void Pchome()
        {
            foreach (var key_word in FillJsonDic())
            {
                //CurlPchomeData(key_word).Wait();
                // CurlGovData(key_word).Wait();
                LoadData(key_word.Key, "Pchome");
            }
        }
        internal static void Momo()
        {
            foreach (var key_word in FillJsonDic())
            {
                LoadData(key_word.Key, "Momo");
            }
        }
        internal static void GovData(string v1, string v2, string v3, string v4)
        {

            //int par_id = 20591;
            //int par_p0 = 81442;
            List<TempData> lt = new List<TempData>();
            var htmlWeb = new HtmlAgilityPack.HtmlWeb();

            for (int par_p0 = Convert.ToInt32(v1); par_p0 < Convert.ToInt32(v2); ++par_p0)
            {
                for (int par_id = Convert.ToInt32(v3); par_id < Convert.ToInt32(v4); ++par_id)
                {
                    var doc = htmlWeb.Load(string.Format(GovDataUrl, par_p0, par_id));
                    //選擇的ul整個區塊
                    //div[@class='col-xs-6 col-md-6  text-right']
                    //col-xs-6 col-md-6 text-left
                    TempData td = new TempData();
                    var items = doc.DocumentNode.SelectNodes("//div[@class='col-xs-6 col-md-6 text-left']");
                    if (items != null)
                    {
                        td.labeling_company = items[0].InnerText;
                        td.product_category = items[1].InnerText;
                        td.product_model = items[2].InnerText;
                        int annual_power_consumption_degrees_dive_year = -1;
                        Int32.TryParse(Regex.Replace(items[items.Count - 1].InnerText, "[^0-9]", ""), out annual_power_consumption_degrees_dive_year);
                        td.annual_power_consumption_degrees_dive_year = annual_power_consumption_degrees_dive_year;
                        td.Id = items[items.Count - 2].InnerText;
                        td.benchmarks = items[items.Count - 3].InnerText;
                        td.Done = "Y";
                        td.Text = doc.DocumentNode.SelectNodes("//*[@id=\"form1\"]/div[6]/div/div/div[3]/div/div[2]").FirstOrDefault().InnerText;
                        td.par_id = par_id;
                        td.par_p0 = par_p0;
                        items.RemoveAt(items.Count - 1);
                        items.RemoveAt(items.Count - 1);
                        items.RemoveAt(items.Count - 1);
                        items.RemoveAt(0);
                        items.RemoveAt(0);
                        items.RemoveAt(0);
                        foreach (var item in items)
                        {
                            if (td.r0 == null) td.r0 = item.InnerText;
                            else if (td.r1 == null) td.r1 = item.InnerText;
                            else if (td.r2 == null) td.r2 = item.InnerText;
                            else if (td.r3 == null) td.r3 = item.InnerText;
                            else if (td.r4 == null) td.r4 = item.InnerText;
                            else if (td.r5 == null) td.r5 = item.InnerText;
                            else if (td.r6 == null) td.r6 = item.InnerText;
                            else if (td.r7 == null) td.r7 = item.InnerText;

                        }
                        if (!string.IsNullOrEmpty(td.product_category)) lt.Add(td);
                    }
                    if (lt.Count() > 300)
                    {
                        Console.WriteLine($"{par_p0} 300 write to DB");

                        DBMethod.BulkDapperInsert(lt);
                        lt.Clear();
                    }
                }
                Console.WriteLine($"{par_p0} done");
                DBMethod.BulkDapperInsert(lt);
                lt.Clear();

            }
        }
        private static void LoadData(string key_word, string data_from = null)
        {
            System.Net.Http.HttpClient ht = new System.Net.Http.HttpClient();
            var momoItems = new PriceStruct.PcHome();
            var tagItems = new PriceStruct.Tags();
            var joinItems = new List<PriceStruct.JoinProduct>();

            int count = 0;
            using (StreamReader r = new StreamReader(FillJsonDic(data_from)[key_word]))
            {
                var json = r.ReadToEnd();
                momoItems = JsonConvert.DeserializeObject<PcHome>(json);
                foreach (var a in momoItems.product)
                {
                    a.key_word = key_word;
                    a.data_from = data_from;
                }
                DBMethod.BulkDapperInsert(momoItems.product);
                Console.WriteLine($"Done {data_from}");
                Console.WriteLine($"{ momoItems.product.Count()}");
            }


            var pchomeData = DBMethod.BulkDapperSearch<PchomePrice>($" SELECT  * FROM PchomePrice where key_word =N'{key_word}'"
                 );
            Console.WriteLine($"{data_from}  / { pchomeData.Count()}");
            var ttB = new List<TagsToDBProduct>();

            using (StreamReader r = new StreamReader(FillJsonDic()[key_word]))
            {
                tagItems = JsonConvert.DeserializeObject<Tags>(r.ReadToEnd());
                foreach (var data in tagItems.product)
                {
                    ttB.Add(ConvertToDB(data, key_word));
                }
                Console.WriteLine($"Gov { tagItems.product.Count()}");
            }
            DBMethod.BulkDapperInsert(ttB);
            foreach (var item in tagItems.product)
            {
                var objectData = momoItems.product.Where(x => x.name.Contains(item.product_model)).Select(x => x).FirstOrDefault();
                if (objectData != null)
                {
                    CheckCount(ref count);
                    joinItems.Add(FillAllData(item, objectData, key_word, data_from));
                }
            }

            DBMethod.BulkDapperInsert(joinItems);
            Console.WriteLine($"{ joinItems.Count()}");


        }
        private static void CheckCount(ref int count)
        {
            lock (o)
            {
                count++;
                if (count % 10 == 0) { Console.WriteLine($"{(count / 10) * 10}筆資料進入"); };
            }
        }
        private static Dictionary<string, string> FillJsonDic(string mode = null)
        {
            Dictionary<string, string> lisVal = new Dictionary<string, string>();

            if (string.IsNullOrEmpty(mode))
            {
                string path = "D:\\Csharp\\ConsoleApplication2\\Gov\\Data\\Gov\\";

                lisVal["溫熱型開飲機"] = $"{path}溫熱型開飲機Tag.json";
                lisVal["電冰箱"] = $"{path}電冰箱Tag.json";
                lisVal["電熱水瓶"] = $"{path}電熱水瓶Tag.json";
                lisVal["除濕機"] = $"{path}除濕機Tag.json";
                lisVal["冷暖空調"] = $"{path}冷暖空調Tag.json";
                lisVal["溫熱型飲水機"] = $"{path}溫熱型飲水機Tag.json";
                lisVal["冰溫熱型開飲機"] = $"{path}冰溫熱型開飲機Tag.json";
                lisVal["冰溫熱型飲水機"] = $"{path}冰溫熱型飲水機Tag.json";
            }
            else if (mode.ToUpper() == "MOMO")
            {
                string path = "D:\\Csharp\\ConsoleApplication2\\Gov\\Data\\Price\\Momo\\";
                lisVal["溫熱型開飲機"] = $"{path}溫熱型開飲機.json";
                lisVal["電冰箱"] = $"{path}電冰箱.json";
                lisVal["電熱水瓶"] = $"{path}電熱水瓶.json";
                lisVal["除濕機"] = $"{path}除濕機.json";
                lisVal["冷暖空調"] = $"{path}冷暖空調.json";
                lisVal["溫熱型飲水機"] = $"{path}溫熱型飲水機.json";
                lisVal["冰溫熱型開飲機"] = $"{path}冰溫熱型開飲機.json";
                lisVal["冰溫熱型飲水機"] = $"{path}冰溫熱型飲水機.json";
            }
            else if (mode.ToUpper() == "PCHOME")
            {
                string path = "D:\\Csharp\\ConsoleApplication2\\Gov\\Data\\Price\\Pchome\\";
                lisVal["冷暖空調"] = $"{path}冷暖空調.json";
            }
            return lisVal;
        }

        private static PriceStruct.JoinProduct FillAllData(PriceStruct.TagsProduct item, PriceStruct.PchomePrice objectData, string key_word, string data_from)
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
            joinProduct.key_word = key_word;
            joinProduct.Pchome_Id = objectData.Id;
            joinProduct.data_from = data_from;
            return joinProduct;
        }
        private static async Task CurlPchomeData(KeyValuePair<string, string> key)
        {
            string keyWord = key.Key;

            List<PriceStruct.PchomePrice> lps = new List<PchomePrice>();
            List<string> lsTag = new List<string>() { "24h", "vdr", "kdn" };
            try
            {
                WebClient hc = new WebClient();
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                foreach (var tag in lsTag)
                {
                    var url = string.Format(PchomeUrl, tag, keyWord, 1);
                    var urlData = hc.DownloadString(url);
                    var structData = JsonConvert.DeserializeObject<response.Pchome>(urlData);
                    for (int i = 1; i < structData.totalPage; ++i)
                    {

                        urlData = hc.DownloadString(string.Format(PchomeUrl, tag, keyWord, i));
                        structData = JsonConvert.DeserializeObject<response.Pchome>(urlData);
                        foreach (var data in structData.prods)
                        {
                            lps.Add(new PchomePrice() { Id = data.Id, key_word = keyWord, name = data.name, originprice = data.originPrice, picb = data.picB, pics = data.picS });
                        }
                        DBMethod.BulkDapperInsert(lps);
                        lps.Clear();
                        Console.WriteLine($"{keyWord} {i} page done {tag} tag done");
                        await Task.Delay(8000);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        private static TagsToDBProduct ConvertToDB(TagsProduct item, string key_word)
        {
            var returnModel = new TagsToDBProduct();

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
            returnModel.key_word = key_word;

            return returnModel;
        }


    }
}