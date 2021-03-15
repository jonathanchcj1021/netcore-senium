using System;
using System.Collections;
using System.IO;
using System.Reflection;
using System.Timers;
using log4net;
using log4net.Config;
using MongoDB.Bson;
using MongoDB.Driver;
using NetCoreSelenium.Library;
using NetCoreSelenium.Model;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace NetCoreSelenium
{
    class Program
    {
        public static LogHelper log = new LogHelper(typeof(Program));
        protected static IMongoClient _client;
        protected static IMongoDatabase _database;
        static void Main(string[] args)
        {
            try
            {
                var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
                XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));



                //Timer timer = new Timer(10000);

                ////timer.Elapsed += new ElapsedEventHandler(TimerCallFindStock);
                //timer.Elapsed += new ElapsedEventHandler(TimerCallFindLeis);
                //timer.Interval = 1000 * 5;
                //timer.Enabled = true;
                SeleniumHelper.GoToPollyU();
                //TimerCallFindLeis(null, null);
                Console.Read();
            }
            catch (Exception ex)
            {

                log.Error("Exception", ex);

            }
        }
        public static void TimerCallFindLeis(object sender, EventArgs e)
        {
            try
            {

                SeleniumHelper.GotoLeis();



                GC.Collect();
                //TaskHelper.StartNew(() => {


                //});

            }
            catch (Exception)
            {
                throw;
            }

        }

        private static void FindLeis()
        {
            try
            {
                
                

            }
            catch (Exception)
            {
                throw;
            }
        }

        public static void TimerCallFindStock(object sender, EventArgs e)
        {
            try
            {
                TaskHelper.StartNew(()=>{

                    DateTime start = DateTime.Today + new TimeSpan(22, 30, 0);

                    DateTime end = DateTime.Today.AddDays(1) + new TimeSpan(08, 30, 0);
                    DateTime now = DateTime.Now;
                    bool match = (now > start) && (now < end);
                    bool matchCurrent = (now.Date == start.Date);

                    log.Info($"now {now.ToString("yyyy-MM-dd HH:mm:ss")} , start {start.ToString("yyyy-MM-dd HH:mm:ss")} , end {end.ToString("yyyy-MM-dd HH:mm:ss")} , matchCurrent {matchCurrent} match {match}");

                    if (matchCurrent)
                    {
                        end = DateTime.Today + new TimeSpan(08, 30, 0);
                        match = (now < end);
                        if (match)
                            FindStock();
                    }
                    else if (match)
                    {
                        FindStock();
                    }

                });

            }
            catch (Exception)
            {
                throw;
            }

        }

        public static void FindStock()
        {
            string stockName = "ARKK".ToUpper();
            log.Info($"Start to get {stockName}");
            StockHistory sh = SeleniumHelper.findStock(stockName);
            _client = new MongoClient();
            _database = _client.GetDatabase("Stock");
            var stockHistoryCol = _database.GetCollection<StockHistory>("StockHistory");



            var stockCol = _database.GetCollection<Stock>("Stock");
            var currentPrice = stockCol.Find(Builders<Stock>.Filter.Eq("StockName", sh.StockName)).FirstOrDefault().Price;

            if (currentPrice != sh.Price)
            {
                stockHistoryCol.InsertOne(sh);
                var result = stockCol.FindOneAndUpdateAsync<Stock>(
                     Builders<Stock>.Filter.Eq("StockName", sh.StockName),
                     Builders<Stock>.Update.Set("Price", sh.Price).Set("updated_time", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
                );
            }
            else
            {
                log.Info($"match current price currentPrice {currentPrice} sh.Price {sh.Price}");
            }


            /*  var exists = stockCol.AsQueryable().Where;
              if (!exists)
              {
                  Stock s = new Stock()
                  {
                      StockName = sh.StockName,
                      Price = sh.Price
                  };
                  stockCol.InsertOne(s);

              }*/
            //bool isExits = stockCol.FindSync({ "StockName":stockName}).hasNext();


            GC.Collect();
        }



    }
}
