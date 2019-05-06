using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication2
{
    class Const
    {

        public static string insertGovData = @"
INSERT INTO[dbo].[GovData]
           ([product_model]
           ,[brand_name]
           ,[labeling_company]
           ,[from_date_of_expiration]
           ,[to_date_of_expiration]
           ,[efficiency_benchmark]
           ,[test_report_of_energy_efficiency]
           ,[annual_power_consumption_degrees_dive_year]
           ,[Id]
           ,[login_number]
           ,[detailUri]
           ,[efficiency_rating]
           ,[key_word])
     VALUES
           (@product_model
           ,@brand_name
           ,@labeling_company
           ,@from_date_of_expiration
           ,@to_date_of_expiration
           ,@efficiency_benchmark
           ,@test_report_of_energy_efficiency
           ,@annual_power_consumption_degrees_dive_year
           ,@Id
           ,@login_number
           ,@detailUri
           ,@efficiency_rating
           ,@key_word)";

        public static string insertPchomeData = @"
INSERT INTO [dbo].[PchomePrice]
           ([Id]
           ,[name]
           ,[originprice]
           ,[pics]
           ,[picb]
,[key_word]
,[data_from])
     VALUES
           (@Id
           ,@name
           ,@originprice
           ,@pics
           ,@picb
,@key_word
,@data_from)";

        public static string insertMixData = @"
INSERT INTO [dbo].[MixGovPcData]
           ([product_model]
           ,[brand_name]
           ,[labeling_company]
           ,[from_date_of_expiration]
           ,[to_date_of_expiration]
           ,[efficiency_benchmark]
           ,[test_report_of_energy_efficiency]
           ,[annual_power_consumption_degrees_dive_year]
           ,[Id]
           ,[name]
           ,[originprice]
           ,[pics]
           ,[picb]
           ,[login_number]
           ,[detailUri]
           ,[efficiency_rating]
        ,[key_word]
,[Pchome_Id]
,[data_from])
     VALUES
           (@product_model
           ,@brand_name
           ,@labeling_company
           ,@from_date_of_expiration
           ,@to_date_of_expiration
           ,@efficiency_benchmark
           ,@test_report_of_energy_efficiency
           ,@annual_power_consumption_degrees_dive_year
           ,@Id
           ,@name
           ,@originprice
           ,@pics
           ,@picb
           ,@login_number
           ,@detailUri
           ,@efficiency_rating
            ,@key_word
            ,@Pchome_Id
,@data_from)";
    }
}
