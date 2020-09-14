using AngleSharp;
using AngleSharp.Html.Parser;
using CrawlerSamples.AutoRunner.carFamily;
using PuppeteerSharp;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CrawlerSamples.AutoRunner
{
    public class ToubiaowangCrawler : RunnerBase
    {
        public ToubiaowangCrawler()
        {
            IsReload = IsReload;
            IsConnect = true;
            BrowserWSEndpoint = BrowserWSEndpoint;
        }

        internal override async Task GetDatas(Page page)
        {
            //Request URL to get the page
            // await page.GoToAsync(runnerInfo.URL);
            await page.GoToAsync("http://jzsc.mohurd.gov.cn/data/company/detail?id=060607000601040406060301070F02050F07");
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
           
            //int n = 0;
            //while (n++ < 29)
            //{

            await page.ClickAsync("#tab-perInfo");

            await page.WaitForTimeoutAsync(5000);
            //await page.ClickAsync("tab-companyQuality");
            //await page.WaitForTimeoutAsync(5000);
            //await page.WaitForSelectorAsync("div:contains('项目名称')");
            await page.WaitForSelectorAsync("#tab-perInfo");

            var result =new List<ToubiaoDatas>();

              var config = Configuration.Default;
            var context = BrowsingContext.New(config);
            var parser = context.GetService<IHtmlParser>();
           
            while (await NeedValid(page, parser)) {
                await page.WaitForTimeoutAsync(5000);
            }
            CreateModelWithAngleSharp(await page.GetContentAsync(), result);

            //await page.WaitForSelectorAsync(html, ".el-dialog__wrapper.dialog");
            //var dialog = await page.GetContentAsync(html, ".el-dialog__wrapper.dialog");



            //#pane-perInfo > div > div.el-table.data-table.el-table--fit.el-table--striped.el-table--enable-row-hover.el-table--enable-row-transition > div.el-table__body-wrapper.is-scrolling-none > table > tbody > tr > td.el-table_2_column_10.is-center
            // }
            //http://jzsc.mohurd.gov.cn/api/webApi/dataservice/query/comp/compPerformanceListSys?qy_id=060607000601040406060301070F02050F07&pg=0&pgsz=15
            ToubiaoDatas.IntrusiveExport(result);
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

        private static async  Task<bool> NeedValid( Page page, IHtmlParser parser)
        {
            
               var html = await page.GetContentAsync();
            AngleSharp.Html.Dom.IHtmlDocument node = parser.ParseDocument(html);

            var list = node.QuerySelector(".el-dialog__wrapper");
            var dialog = list.ChildNodes.Where(x => x.TextContent.IndexOf("重新验证") > -1)
                  .FirstOrDefault();
            var dialoghtml = dialog as AngleSharp.Html.Dom.IHtmlDivElement;
            if (list.OuterHtml.IndexOf("display: none;") > -1)
            {              
                return false;
            }
            return true;
        }

        private static void CreateModelWithAngleSharp(string html, List<ToubiaoDatas> result)
        {
            var config = Configuration.Default;
            var context = BrowsingContext.New(config);
            var parser = context.GetService<IHtmlParser>();
            var source = html;
            var node = parser.ParseDocument(source);
            var list = node.QuerySelectorAll("tbody");
            StringBuilder resultBuilde = new StringBuilder();
            //"1",
            foreach (var onebody in list)
            {
                foreach (var m in onebody.ChildNodes)
                {
                    if (!string.IsNullOrEmpty(m.TextContent))
                    {
                        foreach (var n in m.ChildNodes)
                        {
                            if (!string.IsNullOrEmpty(n.TextContent))
                            {
                                var value = n.TextContent.Replace("\n", "");
                                resultBuilde.Append($"\"{value}\",");
                            }
                        }
                    }
                    //var content = list.TextContent;
                    //content.Replace("举报", "").Replace("查看对话", "").Replace(" +1顶", "");
                    //var lines = content.Split("回复");
                    //foreach (var line in lines)
                    //{
                    //    int count;
                    //    var countStr = Regex.Match(line, @"(?<=\()\d+(?=\))", RegexOptions.Multiline).Value;
                    //    int.TryParse(countStr, out count);
                    //    result.Add(new ToubiaoDatas()
                    //    {
                    //        Name = line,
                    //        count = count,

                    //    });
                    //}
                }
            }
           
            File.WriteAllText("text.csv", resultBuilde.ToString(), Encoding.UTF8);
        }
    }
}