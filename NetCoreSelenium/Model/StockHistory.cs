using System;
using MongoDB.Bson;

namespace NetCoreSelenium.Model
{
    public class Stock
    {
        public ObjectId Id { get; set; }
        public string StockName { get; set; }
        public string Price { get; set; }
        public string updated_time { get; set; }
    }
    public class StockHistory
    {
        public ObjectId Id { get; set; }
        public string StockName { get; set; }
        public string Price { get; set; }
        public string Amplitude { get; set; }
        public string updated_time { get; set; }
    }
}
