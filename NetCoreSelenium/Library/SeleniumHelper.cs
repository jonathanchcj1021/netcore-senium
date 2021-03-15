using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Vision.v1;
using Google.Apis.Vision.v1.Data;
using MongoDB.Bson.IO;
using NetCoreSelenium.Model;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace NetCoreSelenium.Library
{
    public class SeleniumHelper
    {
        public static LogHelper log = new LogHelper(typeof(SeleniumHelper));
        public static StockHistory findStock(string stockName)
        {
            try
            {
                StockHistory sh = new StockHistory();
                string url = "https://finance.yahoo.com/quote/";
                url = $"{url}{stockName}?p={stockName}";
                log.Info($"url : {url} ");
                var chromeOptions = new ChromeOptions();
                chromeOptions.AddArguments("headless");
                IWebDriver driver = new ChromeDriver(AppDomain.CurrentDomain.BaseDirectory + "MacDriver/", chromeOptions);

                driver.Navigate().GoToUrl(url);

                IWebElement stockPrice = driver.FindElement(By.CssSelector("span[data-reactid='32']"));
                IWebElement amplitude = driver.FindElement(By.CssSelector("span[data-reactid='33']"));



                sh.StockName = stockName;
                sh.Price = stockPrice.Text.ToString();
                sh.Amplitude = amplitude.Text.ToString();
                sh.updated_time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                log.Info($"StockName : {stockName} Price: {sh.Price} amplitude: {sh.Amplitude}");

                driver.Close();
                driver.Quit();
                return sh;
            }
            catch (Exception)
            {

                throw;
            }


        }

        public static void GotoLeis()
        {
            try
            {

                string url = "http://w2.leisurelink.lcsd.gov.hk/index/index.jsp";

                log.Info($"url : {url} ");
                var chromeOptions = new ChromeOptions();
                //chromeOptions.AddArguments("headless");
                IWebDriver driver = new ChromeDriver(AppDomain.CurrentDomain.BaseDirectory + "MacDriver/", chromeOptions);

                driver.Navigate().GoToUrl(url);
                IJavaScriptExecutor js = driver as IJavaScriptExecutor;
                Thread.Sleep(500);
                js.ExecuteScript("openwin(\'/application/CheckChannelSuspension.do?applicationId=LCSD_22&language=zh&country=HK\');");

                Thread.Sleep(500);
                driver.SwitchTo().Window(driver.WindowHandles[1]);
                //IWebElement kbkey = driver.FindElement(By.CssSelector(".kbkey:eq(0)"));
                Thread.Sleep(500);
                //string script = "$(\".kbkey:first-child\").text()";
                //var ff = js.ExecuteScript(script);

                List<IWebElement> list = new List<IWebElement>();
                IWebElement btn0 = driver.FindElement(By.CssSelector(".kbkey:nth-child(1)"));
                IWebElement btn1 = driver.FindElement(By.CssSelector(".kbkey:nth-child(2)"));
                IWebElement btn2 = driver.FindElement(By.CssSelector(".kbkey:nth-child(3"));
                IWebElement btn3 = driver.FindElement(By.CssSelector(".kbkey:nth-child(4)"));
                IWebElement btn4 = driver.FindElement(By.CssSelector(".kbkey:nth-child(5)"));
                IWebElement btn5 = driver.FindElement(By.CssSelector(".kbkey:nth-child(6)"));
                IWebElement btn6 = driver.FindElement(By.CssSelector(".kbkey:nth-child(7)"));
                IWebElement btn7 = driver.FindElement(By.CssSelector(".kbkey:nth-child(8)"));
                IWebElement btn8 = driver.FindElement(By.CssSelector(".kbkey:nth-child(9)"));
                IWebElement retryBtn = driver.FindElement(By.CssSelector(".actionBtnSmall"));
                IWebElement image = driver.FindElement(By.CssSelector("#inputTextWrapper > div > img"));
                IWebElement submitBtn = driver.FindElement(By.CssSelector(".actionBtnContinue"));
                log.Info($" {GetElementText(btn0)}  {GetElementText(btn1)} {GetElementText(btn2)}");
                log.Info($" {GetElementText(btn3)}  {GetElementText(btn4)} {GetElementText(btn5)}");
                log.Info($" {GetElementText(btn6)}  {GetElementText(btn7)} {GetElementText(btn8)}");
                //btn0.Click();

                list.Add(btn0);
                list.Add(btn1);
                list.Add(btn2);
                list.Add(btn3);
                list.Add(btn4);
                list.Add(btn5);
                list.Add(btn6);
                list.Add(btn7);
                list.Add(btn8);


                var base64Str = image.GetAttribute("src").Split("data:image/jpg;base64,")[1];
                log.Info($" image {base64Str}");

                string fileName = "verify_code.jpeg";
                ImageHelper.ImageFromBase64(base64Str, fileName);
                string verifyCode = string.Empty;

                int successCount = DetectorHelper.DetectFromImage(fileName, out verifyCode);
                while (successCount < 4)
                {
                    retryBtn.Click();
                    successCount = DetectorHelper.DetectFromImage(fileName, out verifyCode);
                }

                successCount = CompareNClick(list, verifyCode);
                while (successCount < 4)
                {
                    retryBtn.Click();
                    successCount = DetectorHelper.DetectFromImage(fileName, out verifyCode);
                }

                submitBtn.Click();

                driver.Close();
                driver.Quit();


            }
            catch (Exception)
            {

                throw;
            }


        }
        public static void GoToPollyU()
        {
            try
            {

                string url = "https://www40.polyu.edu.hk/possns/secure/login/loginhome.do";

                log.Info($"url : {url} ");
                var chromeOptions = new ChromeOptions();
                //chromeOptions.AddArguments("headless");
                IWebDriver driver = new ChromeDriver(AppDomain.CurrentDomain.BaseDirectory + "MacDriver/", chromeOptions);

                driver.Navigate().GoToUrl(url);
                IJavaScriptExecutor js = driver as IJavaScriptExecutor;
                Thread.Sleep(500);
                js.ExecuteScript("$(\"input[name='j_username']\").val(\"a000081562\")");
                js.ExecuteScript("$(\"input[name='j_password']\").val(\"Meilin0g\")");
                IWebElement submitBtn = driver.FindElement(By.CssSelector("button[name=\"buttonAction\"]"));
                submitBtn.Click();

                Thread.Sleep(500);


                IWebElement makeBtn = driver.FindElement(By.CssSelector("a[data-fctncode=\"FBNS_MAKE_BOOK\"]"));
                makeBtn.Click();


                string bookingDate = "26 Mar 2021";
                IWebElement act = driver.FindElement(By.CssSelector("#actvId"));
                IWebElement ctr = driver.FindElement(By.CssSelector("#ctrId"));
                js.ExecuteScript("$(\"#actvId\").val(2);");
                js.ExecuteScript("$(\"#ctrId\").val(3);");


                js.ExecuteScript($"$(\"#searchDate\").val(\"{bookingDate}\");");
                IWebElement searchBtn = driver.FindElement(By.CssSelector("#searchButton"));
                searchBtn.Click();
                //js.ExecuteScript($"$(\'#searchButton\').click();");

                Thread.Sleep(1000);
                string starTime = "12:30";
                string endTime = "13:30";

                //< div class="timeslot light-text" data-slot-column="BMT09" data-slot-date="15-03-2021" data-slot-start-time="13:30" data-slot-end-time="14:30" data-slot-charge="20" data-slot-rfnd-rsv-fee="0" data-slot-facility="29" data-slot-has-vacancy="true" data-slot-timestamp="1615786200000" style="background-color: rgb(0, 102, 255); border-color: rgb(0, 102, 255);"></div>
                string court = "BMT10";

                string script = "";
                //[data-slot-column=\"{court}\"]
                script = $"$(\'.timeslot[data-slot-column=\"{court}\"][data-slot-has-vacancy=\"true\"][data-slot-date=\"{bookingDate}\"][data-slot-start-time=\"{starTime}\"][data-slot-end-time=\"{endTime}\"]\').click();";
                js.ExecuteScript(script);



                 court = "BMT09";
             
                 script = $"$(\'.timeslot[data-slot-column=\"{court}\"][data-slot-has-vacancy=\"true\"][data-slot-date=\"{bookingDate}\"][data-slot-start-time=\"{starTime}\"][data-slot-end-time=\"{endTime}\"]\').click();";
                js.ExecuteScript(script);

                //string fileName = "verify_code.jpeg";
                ////ImageHelper.ImageFromBase64(base64Str, fileName);
                //string verifyCode = string.Empty;

                //int successCount = DetectorHelper.DetectFromImage(fileName, out verifyCode);
                //while (successCount < 4)
                //{
                //    //retryBtn.Click();
                //    successCount = DetectorHelper.DetectFromImage(fileName, out verifyCode);
                //}

                //successCount = CompareNClick(list, verifyCode);
                //while (successCount < 4)
                //{
                //    //retryBtn.Click();
                //    successCount = DetectorHelper.DetectFromImage(fileName, out verifyCode);
                //}

                //submitBtn.Click();

                driver.Close();
                driver.Quit();


            }
            catch (Exception)
            {

                throw;
            }


        }
        public static string GetElementText(IWebElement w)
        {
            return w.Text;
        }

        public static int CompareNClick(List<IWebElement> list, string verifyCode)
        {
            int clickCount = 0;
            try
            {
                foreach (var ele in list)
                {
                    foreach (char code in verifyCode)
                    {
                        log.Info($"code is {code}  verifCode is {verifyCode} ");
                        if (code.ToString() == GetElementText(ele))
                        {
                            ele.Click();
                            clickCount++;

                        }
                    }
                }
                return clickCount;
            }
            catch (Exception) {
                throw;
            }

            
        }

    }
    }
