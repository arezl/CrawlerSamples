/*
 * This is a Puppeteer+AngleSharp crawler console app samples
 */

using AngleSharp.Dom;
using AngleSharp.Html.Parser;
using CrawlerSamples.AutoRunner;
using Newtonsoft.Json;
using PuppeteerSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace CrawlerSamples
{
    internal class Program
    {//https://www.cnblogs.com/TTonly/p/10920294.html
     //https://gist.github.com/hlaueriksson/4a4199f0802681b06f0f508a2916164d
     //https://github.com/kblok/puppeteer-sharp/blob/master/lib/PuppeteerSharp.Tests/FrameTests/WaitForXPathTests.cs
     //https://www.cnblogs.com/TTonly/p/10920294.html
     //  private const string Url = "https://store.mall.autohome.com.cn/83106681.html";
        private const string Url = "https://www.icloud.com";

        private const int ChromiumRevision = BrowserFetcher.DefaultRevision;
        //   private const int ChromiumRevision = 735830;

        private static async Task Main(string[] args)

        {
            //    await new BrowserFetcher().DownloadAsync(ChromiumRevision);
            //Download chromium browser revision package

            //Test AngleSharp
            await TestAngleSharp();

            Console.ReadKey();
        }

        private static async Task TestAngleSharp()
        {
            /*
             * Used AngleSharp loading of HTML document
             * TODO: Used WithJavaScript function need install AngleSharp.Scripting.Javascript nuget package
             * Note: that JavaScripts support is an experimental and does not support complex JavaScripts code.
             */
            //IConfiguration config = Configuration.Default.WithDefaultLoader().WithCss().WithCookies().WithJavaScript();
            //IBrowsingContext context = BrowsingContext.New(config);
            //IDocument document = await context.OpenAsync(url);

            //Used PuppeteerSharp loading of HTML document
            var htmlString = await TestPuppeteerSharp();

            /*
             * Parsing of HTML document string
             */
            var parser = new HtmlParser();
            var document = parser.ParseDocument(htmlString);

            //Selector carbox element list
            var carboxList = document.QuerySelectorAll("div.customGoodsList div.content div.list li.carbox");

            var carModelList = new List<CarModel>();
            foreach (var carbox in carboxList)
            {
                //Parsing and converting to the car model object.
                var model = CreateModelWithAngleSharp(carbox);
                carModelList.Add(model);

                //Printing to console windows
                var jsonString = JsonConvert.SerializeObject(model);
                Console.WriteLine(jsonString);
                Console.WriteLine();
            }

            Console.WriteLine("Total count:" + carModelList.Count);
        }

        private static async Task<string> TestPuppeteerSharp()
        {
            RunnerBase runnerInfo = new ToubiaowangCrawler();
            var chrom = @".local-chromium\Win64-735830\chrome.exe";
            if (File.Exists(chrom))
            {
                Console.WriteLine("test");
            }

            var launchOptions = new LaunchOptions
            {
                Headless = false,
                IgnoreHTTPSErrors = true,
                ExecutablePath = chrom,
                //        Args = new[] {
                //"--proxy-server=127.0.0.1:50376",
                //"--no-sandbox",
                //"--disable-infobars",不行啊，
                //"--disable-setuid-sandbox",
                //"--ignore-certificate-errors",
                //},
            };
            Browser browser = null;
            Page page = null;
            if (!runnerInfo.IsConnect)
            {
                browser = await Puppeteer.LaunchAsync(launchOptions);
                //New tab page
                var browserWSEndpoint = browser.WebSocketEndpoint;
                File.WriteAllText("WebSocketEndpoint.txt", browserWSEndpoint);
                page = await browser.NewPageAsync();
            }
            else
            {
            var browserWSEndpoint=   File.ReadAllText("WebSocketEndpoint.txt");
                browser = await Puppeteer.ConnectAsync(new ConnectOptions { BrowserWSEndpoint = browserWSEndpoint });
                page = await browser.NewPageAsync();
            }

            await runnerInfo.GetDatas(page);

            #region Dispose resources

            //Close tab page
          //  await page.CloseAsync();

            //Close headless browser, all pages will be closed here.
          //  await browser.CloseAsync();

            #endregion Dispose resources

            return string.Empty;
        }

        private static CarModel CreateModelWithAngleSharp(IParentNode node)
        {
            var model = new CarModel
            {
                Title = node.QuerySelector("a div.carbox-title").TextContent,
                ImageUrl = node.QuerySelector("a div.carbox-carimg img").GetAttribute("src"),
                ProductUrl = node.QuerySelector("a").GetAttribute("href"),
                Tip = node.QuerySelector("a div.carbox-tip").TextContent,
                OrdersNumber = node.QuerySelector("a div.carbox-number span").TextContent
            };

            return model;
        }
    }
}