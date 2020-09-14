using AngleSharp;
using AngleSharp.Html.Parser;
using CrawlerSamples.AutoRunner;
using CrawlerSamples.AutoRunner.carFamily;
using PuppeteerSharp;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

 
    public class RunChoutiCraper : RunnerBase
    {
        public RunChoutiCraper()
        {
            IsReload = IsReload;
            IsConnect = false;
            BrowserWSEndpoint = BrowserWSEndpoint;
        }

        internal override async Task GetDatas(Page page)
        {
            //Request URL to get the page
            // await page.GoToAsync(runnerInfo.URL);
            await page.GoToAsync("https://www.autohome.com.cn/comment/Articlecomment.aspx?articleid=912864#pvareaid=3311690");
            //  await page.WaitForTimeoutAsync(10000);

            // var frame1 = page.Frames.First(f => f.Name == "frame1");
            //    var frame2 = page.MainFrame;
            //   var frame1 = frame2.ChildFrames.First();
            //    var waitForXPathPromise = frame1.WaitForXPathAsync("//div");
            //var test=await    frame1.GetContentAsync();
            //  var ifrmElement = await page.WaitForSelectorAsync("#reply-list");
            // ifrmElement.
            //var ifrmFrame = await ifrmElement.ContentFrameAsync();
            //var ifrmHtml = await ifrmFrame.GetContentAsync();
            // var test  = await page.SelectAsync("#J-datetime-select > a:nth-child(3)");
            var result = new List<CarFamilyDatas>();
            int n = 0;
            while (n++ < 29)
            {
                CreateModelWithAngleSharp(await page.GetContentAsync(), result);
                await page.ClickAsync("#topPager > div > a.page-item-next");
            }

            CarFamilyDatas.IntrusiveExport(result);
            //var replaylist = await page.SelectAsync("#reply-list");


            ////  await  ifrmElement.ClickAsync();
            //var ifrmElement1 = await page.XPathAsync("//*[@id=\"reply-item-72217813\"]");
            //await ifrmElement1[0].ClickAsync();
            //await page.ClickAsync("body > div:nth-child(24) > ul > li:nth-child(5)");
            //var beginDate = await page.SelectAsync("#beginDate");
            //await page.ClickAsync("#beginDate");
            //await page.SelectAsync(".record-search-option-date > #J-search-date-container > #J-datetime-select #beginDate");
            //await page.ClickAsync(".record-search-option-date > #J-search-date-container > #J-datetime-select #beginDate");

            //await page.SelectAsync(".ui-calendar:nth-child(39) > .ui-calendar > .ui-calendar-data-container > .ui-calendar-date-column > .focused-element");
            //await page.ClickAsync(".ui-calendar:nth-child(39) > .ui-calendar > .ui-calendar-data-container > .ui-calendar-date-column > .focused-element");

            //await page.SelectAsync(".record-search-option-date > #J-search-date-container > #J-datetime-select #endDate");
            //await page.ClickAsync(".record-search-option-date > #J-search-date-container > #J-datetime-select #endDate");

            //await page.SelectAsync(".ui-calendar:nth-child(40) > .ui-calendar > .ui-calendar-data-container > .ui-calendar-date-column:nth-child(5) > .ui-calendar-day-0");
            //await page.ClickAsync(".ui-calendar:nth-child(40) > .ui-calendar > .ui-calendar-data-container > .ui-calendar-date-column:nth-child(5) > .ui-calendar-day-0");
            //await page.SelectAsync("#main > div.amount-top > div > div.fn-clear.action-other.action-other-show > div.fn-left > div > a.J-download-tip.mr-10");
            //await page.ClickAsync("#main > div.amount-top > div > div.fn-clear.action-other.action-other-show > div.fn-left > div > a.J-download-tip.mr-10");

            ////Get and return the HTML content of the page
            //var htmlString = await page.GetContentAsync();
        }

        private static void CreateModelWithAngleSharp(string html, List<CarFamilyDatas> result)
        {
            var config = Configuration.Default;
            var context = BrowsingContext.New(config);
            var parser = context.GetService<IHtmlParser>();
            var source = html;
            var node = parser.ParseDocument(source);
            var list = node.QuerySelector("#reply-list");
            var content = list.TextContent.Replace("举报", "").Replace("查看对话", "").Replace(" +1顶", "");
            var lines = content.Split("回复");
            foreach (var line in lines)
            {
                int count;
                var countStr = Regex.Match(line, @"(?<=\()\d+(?=\))", RegexOptions.Multiline).Value;
                int.TryParse(countStr, out count);
                result.Add(new CarFamilyDatas()
                {
                    Name = line,
                    count = count,

                });
            }
        }
    }
