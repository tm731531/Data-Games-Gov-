using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication2.Common
{
    class response
    {
        public class Range
        {
            public string min { get; set; }
            public string max { get; set; }
        }

        public class Prod
        {
            public string Id { get; set; }
            public string cateId { get; set; }
            public string picS { get; set; }
            public string picB { get; set; }
            public string name { get; set; }
            public string describe { get; set; }
            public int price { get; set; }
            public int originPrice { get; set; }
            public string author { get; set; }
            public string brand { get; set; }
            public string publishDate { get; set; }
            public string sellerId { get; set; }
            public int isPChome { get; set; }
            public int isNC17 { get; set; }
            public List<object> couponActid { get; set; }
            public string BU { get; set; }
        }

        public class Pchome
        {
            public int QTime { get; set; }
            public int totalRows { get; set; }
            public int totalPage { get; set; }
            public Range range { get; set; }
            public string cateName { get; set; }
            public string q { get; set; }
            public string subq { get; set; }
            public List<string> token { get; set; }
            public List<Prod> prods { get; set; }
        }
    }
}
