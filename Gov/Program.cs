using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using static ConsoleApplication2.PriceStruct;
using ECPay.Payment.Integration;
using System.Net.Http;
using ConsoleApplication2.Common;
using System.Net;
using System.Text.RegularExpressions;
using ConsoleApplication2.Method;

namespace ConsoleApplication2
{
    class Program
    {


        public string PchomeTable = "PchomePrice";
        public string GovTable = "GovData";
        public string MixTable = "MixGovPcData";
        static void Main(string[] args)

        {
            CurlData.LoadData("冰溫熱型開飲機", "PCHOME");
            CurlData.LoadData("冰溫熱型飲水機", "PCHOME");
            CurlData.LoadData("冷暖空調", "PCHOME");
            CurlData.LoadData("除濕機", "PCHOME");
            CurlData.LoadData("溫熱型開飲機", "PCHOME");
            CurlData.LoadData("溫熱型飲水機", "PCHOME");
            CurlData.LoadData("電冰箱", "PCHOME");
            CurlData.LoadData("電熱水瓶", "PCHOME");
            CurlData.LoadData("冰溫熱型開飲機", "MOMO");
            CurlData.LoadData("冰溫熱型飲水機", "MOMO");
            CurlData.LoadData("冷暖空調", "MOMO");
            CurlData.LoadData("除濕機", "MOMO");
            CurlData.LoadData("溫熱型開飲機", "MOMO");
            CurlData.LoadData("溫熱型飲水機", "MOMO");
            CurlData.LoadData("電冰箱", "MOMO");
            CurlData.LoadData("電熱水瓶", "MOMO");
            //if (args.Length < 2) { Console.WriteLine("參數不完整"); return; }

            //if (args[0].ToUpper() == "LOAD")
            //{
            //    switch (args[1].ToUpper())
            //    {
            //        case "PCHOME":
            //            CurlData.Pchome();
            //            break;
            //        case "MOMO":
            //            CurlData.Momo();
            //            break;
            //        default:
            //            break;
            //    }
            //}
            //else
            //{
            //    CurlData.GovData(args[0], args[1], args[2], args[3]);
            //}
            Console.WriteLine("Done");
            Console.ReadLine();
        }

      
     
       

    
        #region 沒有用到的
        private static Dictionary<string, string> FillDic()
        {

            Dictionary<string, string> lisVal = new Dictionary<string, string>();

            //  lisVal["電冰箱"] = "http://61.219.118.186/energylbapply/_outweb/gen/opendata/%E7%B6%93%E6%BF%9F%E9%83%A8%E8%83%BD%E6%BA%90%E5%B1%80_%E9%9B%BB%E5%86%B0%E7%AE%B1%E7%AF%80%E8%83%BD%E6%A8%99%E7%AB%A0%E6%9C%89%E6%95%88%E7%8D%B2%E8%AD%89%E8%B3%87%E8%A8%8A.csv";
            // lisVal["除濕機"] = "http://61.219.118.186/energylbapply/_outweb/gen/opendata/%E7%B6%93%E6%BF%9F%E9%83%A8%E8%83%BD%E6%BA%90%E5%B1%80_%E9%99%A4%E6%BF%95%E6%A9%9F%E7%AF%80%E8%83%BD%E6%A8%99%E7%AB%A0%E6%9C%89%E6%95%88%E7%8D%B2%E8%AD%89%E8%B3%87%E8%A8%8A.csv";
            // lisVal["溫熱型開飲機"] = "http://61.219.118.186/energylbapply/_outweb/gen/opendata/%E7%B6%93%E6%BF%9F%E9%83%A8%E8%83%BD%E6%BA%90%E5%B1%80_%E6%BA%AB%E7%86%B1%E5%9E%8B%E9%96%8B%E9%A3%B2%E6%A9%9F%E7%AF%80%E8%83%BD%E6%A8%99%E7%AB%A0%E6%9C%89%E6%95%88%E7%8D%B2%E8%AD%89%E8%B3%87%E8%A8%8A.csv";
            // lisVal["冰溫熱型開飲機"] = "http://61.219.118.186/energylbapply/_outweb/gen/opendata/%E7%B6%93%E6%BF%9F%E9%83%A8%E8%83%BD%E6%BA%90%E5%B1%80_%E5%86%B0%E6%BA%AB%E7%86%B1%E5%9E%8B%E9%96%8B%E9%A3%B2%E6%A9%9F%E7%AF%80%E8%83%BD%E6%A8%99%E7%AB%A0%E6%9C%89%E6%95%88%E7%8D%B2%E8%AD%89%E8%B3%87%E8%A8%8A.csv";
            // lisVal["溫熱型飲水機"] = "http://61.219.118.186/energylbapply/_outweb/gen/opendata/%E7%B6%93%E6%BF%9F%E9%83%A8%E8%83%BD%E6%BA%90%E5%B1%80_%E6%BA%AB%E7%86%B1%E5%9E%8B%E9%96%8B%E9%A3%B2%E6%A9%9F%E7%AF%80%E8%83%BD%E6%A8%99%E7%AB%A0%E6%9C%89%E6%95%88%E7%8D%B2%E8%AD%89%E8%B3%87%E8%A8%8A.csv";
            // lisVal["冰溫熱型飲水機"] = "http://61.219.118.186/energylbapply/_outweb/gen/opendata/%E7%B6%93%E6%BF%9F%E9%83%A8%E8%83%BD%E6%BA%90%E5%B1%80_%E5%86%B0%E6%BA%AB%E7%86%B1%E5%9E%8B%E9%96%8B%E9%A3%B2%E6%A9%9F%E7%AF%80%E8%83%BD%E6%A8%99%E7%AB%A0%E6%9C%89%E6%95%88%E7%8D%B2%E8%AD%89%E8%B3%87%E8%A8%8A.csv";
            lisVal["電熱水瓶"] = "http://61.219.118.186/energylbapply/_outweb/gen/opendata/%E7%B6%93%E6%BF%9F%E9%83%A8%E8%83%BD%E6%BA%90%E5%B1%80_%E9%9B%BB%E7%86%B1%E6%B0%B4%E7%93%B6%E7%AF%80%E8%83%BD%E6%A8%99%E7%AB%A0%E6%9C%89%E6%95%88%E7%8D%B2%E8%AD%89%E8%B3%87%E8%A8%8A.csv";
            //lisVal["冷暖空調"] = "http://61.219.118.186/energylbapply/_outweb/gen/opendata/%E7%B6%93%E6%BF%9F%E9%83%A8%E8%83%BD%E6%BA%90%E5%B1%80_%E5%86%B7%E6%B0%A3%E6%A9%9F%E7%AF%80%E8%83%BD%E6%A8%99%E7%AB%A0%E6%9C%89%E6%95%88%E7%8D%B2%E8%AD%89%E8%B3%87%E8%A8%8A.csv";

            //lisVal["洗衣機"] = "http://61.219.118.186/energylbapply/_outweb/gen/opendata/%E7%B6%93%E6%BF%9F%E9%83%A8%E8%83%BD%E6%BA%90%E5%B1%80_%E6%B4%97%E8%A1%A3%E6%A9%9F%E7%AF%80%E8%83%BD%E6%A8%99%E7%AB%A0%E6%9C%89%E6%95%88%E7%8D%B2%E8%AD%89%E8%B3%87%E8%A8%8A.csv";
            //lisVal["乾衣機"] = "http://61.219.118.186/energylbapply/_outweb/gen/opendata/%E7%B6%93%E6%BF%9F%E9%83%A8%E8%83%BD%E6%BA%90%E5%B1%80_%E4%B9%BE%E8%A1%A3%E6%A9%9F%E7%AF%80%E8%83%BD%E6%A8%99%E7%AB%A0%E6%9C%89%E6%95%88%E7%8D%B2%E8%AD%89%E8%B3%87%E8%A8%8A.csv";
            //lisVal["電視機"] = "http://61.219.118.186/energylbapply/_outweb/gen/opendata/%E7%B6%93%E6%BF%9F%E9%83%A8%E8%83%BD%E6%BA%90%E5%B1%80_%E9%9B%BB%E8%A6%96%E6%A9%9F%E7%AF%80%E8%83%BD%E6%A8%99%E7%AB%A0%E6%9C%89%E6%95%88%E7%8D%B2%E8%AD%89%E8%B3%87%E8%A8%8A.csv";
            //lisVal["空氣清淨機"] = "http://61.219.118.186/energylbapply/_outweb/gen/opendata/%E7%B6%93%E6%BF%9F%E9%83%A8%E8%83%BD%E6%BA%90%E5%B1%80_%E7%A9%BA%E6%B0%A3%E6%B8%85%E6%B7%A8%E6%A9%9F%E7%AF%80%E8%83%BD%E6%A8%99%E7%AB%A0%E6%9C%89%E6%95%88%E7%8D%B2%E8%AD%89%E8%B3%87%E8%A8%8A.csv";
            //lisVal["電扇"] = "http://61.219.118.186/energylbapply/_outweb/gen/opendata/%E7%B6%93%E6%BF%9F%E9%83%A8%E8%83%BD%E6%BA%90%E5%B1%80_%E9%9B%BB%E6%89%87%E7%AF%80%E8%83%BD%E6%A8%99%E7%AB%A0%E6%9C%89%E6%95%88%E7%8D%B2%E8%AD%89%E8%B3%87%E8%A8%8A.csv";
            //lisVal["電鍋電子鍋"] = "http://61.219.118.186/energylbapply/_outweb/gen/opendata/%E7%B6%93%E6%BF%9F%E9%83%A8%E8%83%BD%E6%BA%90%E5%B1%80_%E9%9B%BB%E9%8D%8B%E9%9B%BB%E5%AD%90%E9%8D%8B%E7%AF%80%E8%83%BD%E6%A8%99%E7%AB%A0%E6%9C%89%E6%95%88%E7%8D%B2%E8%AD%89%E8%B3%87%E8%A8%8A.csv";
            //lisVal["汽車"] = "http://61.219.118.186/energylbapply/_outweb/gen/opendata/%E7%B6%93%E6%BF%9F%E9%83%A8%E8%83%BD%E6%BA%90%E5%B1%80_%E6%B1%BD%E8%BB%8A%E7%AF%80%E8%83%BD%E6%A8%99%E7%AB%A0%E6%9C%89%E6%95%88%E7%8D%B2%E8%AD%89%E8%B3%87%E8%A8%8A.csv";
            return lisVal;
        }

        private static void EPay()
        {
            List<string> enErrors = new List<string>();
            try
            {
                using (AllInOne oPayment = new AllInOne())
                {
                    /* 服務參數 */
                    oPayment.ServiceMethod = ECPay.Payment.Integration.HttpMethod.HttpPOST;//介接服務時，呼叫 API 的方法
                    oPayment.ServiceURL = "https://payment-stage.ecpay.com.tw/Cashier/AioCheckOut/V5";//要呼叫介接服務的網址
                    oPayment.HashKey = "5294y06JbISpM5x9";//ECPay提供的Hash Key
                    oPayment.HashIV = "v77hoKGq4kWxNNIS";//ECPay提供的Hash IV
                    oPayment.MerchantID = "2000132";//ECPay提供的特店編號

                    /* 基本參數 */
                    oPayment.Send.ReturnURL = "http://example.com";//付款完成通知回傳的網址
                    oPayment.Send.ClientBackURL = "http://www.ecpay.com.tw/";//瀏覽器端返回的廠商網址
                                                                             //  oPayment.Send.OrderResultURL = "http://localhost:52413/CheckOutFeedback.aspx";//瀏覽器端回傳付款結果網址
                    oPayment.Send.MerchantTradeNo = "ECPay" + new Random().Next(0, 99999).ToString();//廠商的交易編號
                    oPayment.Send.MerchantTradeDate = DateTime.Now;//廠商的交易時間
                    oPayment.Send.TotalAmount = Decimal.Parse("3280");//交易總金額
                    oPayment.Send.TradeDesc = "交易描述";//交易描述
                    oPayment.Send.ChoosePayment = PaymentMethod.ALL;//使用的付款方式
                    oPayment.Send.Remark = "";//備註欄位
                    oPayment.Send.ChooseSubPayment = PaymentMethodItem.None;//使用的付款子項目
                    oPayment.Send.NeedExtraPaidInfo = ExtraPaymentInfo.Yes;//是否需要額外的付款資訊
                    oPayment.Send.DeviceSource = DeviceType.PC;//來源裝置
                    oPayment.Send.IgnorePayment = ""; //不顯示的付款方式
                    oPayment.Send.PlatformID = "";//特約合作平台商代號
                    oPayment.Send.CustomField1 = "";
                    oPayment.Send.CustomField2 = "";
                    oPayment.Send.CustomField3 = "";
                    oPayment.Send.CustomField4 = "";
                    oPayment.Send.EncryptType = 1;

                    //訂單的商品資料
                    oPayment.Send.Items.Add(new Item()
                    {
                        Name = "蘋果",//商品名稱
                        Price = Decimal.Parse("3280"),//商品單價
                        Currency = "新台幣",//幣別單位
                        Quantity = Int32.Parse("1"),//購買數量
                        URL = "http://google.com",//商品的說明網址

                    });

                    /*************************非即時性付款:ATM、CVS 額外功能參數**************/

                    #region ATM 額外功能參數

                    //oPayment.SendExtend.ExpireDate = 3;//允許繳費的有效天數
                    //oPayment.SendExtend.PaymentInfoURL = "";//伺服器端回傳付款相關資訊
                    //oPayment.SendExtend.ClientRedirectURL = "";//Client 端回傳付款相關資訊

                    #endregion


                    #region CVS 額外功能參數

                    //oPayment.SendExtend.StoreExpireDate = 3; //超商繳費截止時間 CVS:以分鐘為單位 BARCODE:以天為單位
                    //oPayment.SendExtend.Desc_1 = "test1";//交易描述 1
                    //oPayment.SendExtend.Desc_2 = "test2";//交易描述 2
                    //oPayment.SendExtend.Desc_3 = "test3";//交易描述 3
                    //oPayment.SendExtend.Desc_4 = "";//交易描述 4
                    //oPayment.SendExtend.PaymentInfoURL = "";//伺服器端回傳付款相關資訊
                    //oPayment.SendExtend.ClientRedirectURL = "";///Client 端回傳付款相關資訊

                    #endregion

                    /***************************信用卡額外功能參數***************************/

                    #region Credit 功能參數

                    //oPayment.SendExtend.BindingCard = BindingCardType.No; //記憶卡號
                    //oPayment.SendExtend.MerchantMemberID = ""; //記憶卡號識別碼
                    //oPayment.SendExtend.Language = ""; //語系設定

                    #endregion Credit 功能參數

                    #region 一次付清

                    //oPayment.SendExtend.Redeem = false;   //是否使用紅利折抵
                    //oPayment.SendExtend.UnionPay = true; //是否為銀聯卡交易

                    #endregion

                    #region 分期付款

                    //oPayment.SendExtend.CreditInstallment = "3,6";//刷卡分期期數

                    #endregion 分期付款

                    #region 定期定額

                    //oPayment.SendExtend.PeriodAmount = 1000;//每次授權金額
                    //oPayment.SendExtend.PeriodType = PeriodType.Day;//週期種類
                    //oPayment.SendExtend.Frequency = 1;//執行頻率
                    //oPayment.SendExtend.ExecTimes = 2;//執行次數
                    //oPayment.SendExtend.PeriodReturnURL = "";//伺服器端回傳定期定額的執行結果網址。

                    #endregion

                    /* 產生訂單 */
                    enErrors.AddRange(oPayment.CheckOut());
                    Console.WriteLine("OK");
                }
            }
            catch (Exception ex)
            {
                // 例外錯誤處理。
                enErrors.Add(ex.Message);
            }
            finally
            {
                // 顯示錯誤訊息。
                if (enErrors.Count() > 0)
                {
                    Console.WriteLine(String.Join("\\r\\n", enErrors));
                    // string szErrorMessage = String.Join("\\r\\n", enErrors);
                }
            }


        }
        private static async Task CurlGovData(KeyValuePair<string, string> key_word)
        {

            HttpClient hc = new HttpClient();
            // hc.Encoding = Encoding.UTF8;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            var urlData = hc.GetStringAsync(key_word.Value).Result;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(key_word.Value);
            var response = (HttpWebResponse)request.GetResponse();
            StreamReader sr;
            sr = new StreamReader(response.GetResponseStream());
            string responseString = sr.ReadToEnd();
            sr.Close();
            response.Close();
            //            return responseString;
            string[] stringSeparators = new string[] { "\r\n" };

            var datas = responseString.Split(stringSeparators, StringSplitOptions.None);
            foreach (var data in datas)
            {
                data.Split(',');

            }
            //var structData = JsonConvert.DeserializeObject<response.Pchome>(urlData);
            //for (int i = 1; i < structData.totalPage; ++i)
            //{

            //    urlData = hc.DownloadString(key_word.Value);
            //}
        }


        #endregion

    }
}
