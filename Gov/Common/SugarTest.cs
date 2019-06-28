
using SqlSugar;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ConsoleApplication2.Common
{
    class SugarTest
    {

        public void Method()
        {
            SqlSugarClient db = new SqlSugarClient(
    new ConnectionConfig()
    {
        ConnectionString = "server=tomting.database.windows.net;uid=tomting;pwd=!QAZ2wsx;database=BookStore",
        DbType = DbType.SqlServer,//设置数据库类型
        IsAutoCloseConnection = true,//自动释放数据务，如果存在事务，在事务结束后释放
        InitKeyType = InitKeyType.Attribute //从实体特性中读取主键自增列信息
    });


            db.Aop.OnLogExecuting = (sql, pars) =>
            {
                Console.WriteLine(sql + "\r\n" +
                db.Utilities.SerializeObject(pars.ToDictionary(it => it.ParameterName, it => it.Value)));
                Console.WriteLine();
            };


            /*查询*/
            var list = db.Queryable<StudentModel>().ToList();//查询所有
            var getById = db.Queryable<StudentModel>().InSingle(1);//根据主键查询
            var getByWhere = db.Queryable<StudentModel>().Where(it => it.Id == 1).ToList();//根据条件查询
            var total = 0;
            var getPage = db.Queryable<StudentModel>().Where(it => it.Id == 1).ToPageList(1, 2, ref total);//根据分页查询
                                                                                                           //多表查询用法 http://www.codeisbug.com/Doc/8/1124

            /*插入*/
            var data = new StudentModel() { Name = "jack" };
            db.Insertable(data).ExecuteCommand();
            //更多插入用法 http://www.codeisbug.com/Doc/8/1130

            /*更新*/
            var data2 = new StudentModel() { Id = 1, Name = "jack" };
            db.Updateable(data2).ExecuteCommand();
            //更多更新用法 http://www.codeisbug.com/Doc/8/1129

            /*删除*/
            db.Deleteable<StudentModel>(1).ExecuteCommand();
            //更多删除用法 http://www.codeisbug.com/Doc/8/1128

            var result = db.Ado.UseTran(() =>
            {
                //这里写你的逻辑
            });
            if (result.IsSuccess)
            {
                //成功
            }
            else
            {
                Console.WriteLine(result.ErrorMessage);
            }
        }

    }
   
    [SugarTable("Student")]
    public class StudentModel
    {
        //指定主键和自增列，当然数据库中也要设置主键和自增列才会有效
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public int Id { get; set; }
        public string Name { get; set; }
    }
}