using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DapperPlus;
using Dapper;
using static ConsoleApplication2.PriceStruct;

namespace ConsoleApplication2.Common
{
    static class DBMethod
    {
        public static string connectionString =
               ConfigurationManager.ConnectionStrings["DBConnection"].ConnectionString;

        public static SqlConnection connect = new SqlConnection(connectionString);

        public static void BulkDapperInsert(List<PchomePrice> data)
        {
            connect.Open();
            connect.InsertBulk<PchomePrice>(data);
            connect.Close();
        }

        public static void BulkDapperInsert(List<GovData> data)
        {
            connect.Open();
            connect.InsertBulk<GovData>(data);
            connect.Close();
        }
        public static void BulkDapperInsert(List<MixGovPcData> data)
        {
            connect.Open();
            connect.InsertBulk<MixGovPcData>(data);
            connect.Close();
        }
        public static void BulkDapperInsert(List<TempData> data)
        {
            connect.Open();
            connect.InsertBulk<TempData>(data);
            connect.Close();
        }
        public static List<T> BulkDapperSearch<T>(string query)
        {
            connect.Open();
            var returnObject = connect.Query<T>(query).ToList();
            connect.Close();
            return returnObject;
        }


    }
}
