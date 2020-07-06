using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using PuppeteerSharp;

namespace CrawlerSamples.AutoRunner
{
   public class zhifubao: RunnerBase
    {
        public zhifubao()
        {
            this.URL = "https://auth.alipay.com/login/index.htm?goto=https%3A%2F%2Fconsumeprod.alipay.com%2Frecord%2Fadvanced.htm";

        }

        internal override async Task GetDatas(Page page)
        {
            //Request URL to get the page
            // await page.GoToAsync(runnerInfo.URL);
            await page.GoToAsync("https://consumeprod.alipay.com/record/advanced.htm");
            //  await page.WaitForTimeoutAsync(10000);

            // var frame1 = page.Frames.First(f => f.Name == "frame1"); 
            //    var frame2 = page.MainFrame;
            //   var frame1 = frame2.ChildFrames.First(); 
            //    var waitForXPathPromise = frame1.WaitForXPathAsync("//div");
            //var test=await    frame1.GetContentAsync();
            var ifrmElement = await page.WaitForSelectorAsync("#J-datetime-select > a:nth-child(3)");
            // ifrmElement.
            //var ifrmFrame = await ifrmElement.ContentFrameAsync();
            //var ifrmHtml = await ifrmFrame.GetContentAsync();
            // var test  = await page.SelectAsync("#J-datetime-select > a:nth-child(3)");
            await page.ClickAsync("#J-datetime-select > a:nth-child(3)");
            await page.ClickAsync("body > div:nth-child(19) > ul > li:nth-child(5)");
            //  await  ifrmElement.ClickAsync();
            var ifrmElement1 = await page.XPathAsync("/html/body/div[10]/ul/li[5]");
            await ifrmElement1[0].ClickAsync();
            await page.ClickAsync("body > div:nth-child(24) > ul > li:nth-child(5)");
            var beginDate = await page.SelectAsync("#beginDate");
            await page.ClickAsync("#beginDate");
            await page.SelectAsync(".record-search-option-date > #J-search-date-container > #J-datetime-select #beginDate");
            await page.ClickAsync(".record-search-option-date > #J-search-date-container > #J-datetime-select #beginDate");


            await page.SelectAsync(".ui-calendar:nth-child(39) > .ui-calendar > .ui-calendar-data-container > .ui-calendar-date-column > .focused-element");
            await page.ClickAsync(".ui-calendar:nth-child(39) > .ui-calendar > .ui-calendar-data-container > .ui-calendar-date-column > .focused-element");


            await page.SelectAsync(".record-search-option-date > #J-search-date-container > #J-datetime-select #endDate");
            await page.ClickAsync(".record-search-option-date > #J-search-date-container > #J-datetime-select #endDate");


            await page.SelectAsync(".ui-calendar:nth-child(40) > .ui-calendar > .ui-calendar-data-container > .ui-calendar-date-column:nth-child(5) > .ui-calendar-day-0");
            await page.ClickAsync(".ui-calendar:nth-child(40) > .ui-calendar > .ui-calendar-data-container > .ui-calendar-date-column:nth-child(5) > .ui-calendar-day-0");
            await page.SelectAsync("#main > div.amount-top > div > div.fn-clear.action-other.action-other-show > div.fn-left > div > a.J-download-tip.mr-10");
            await page.ClickAsync("#main > div.amount-top > div > div.fn-clear.action-other.action-other-show > div.fn-left > div > a.J-download-tip.mr-10");


            //Get and return the HTML content of the page
            var htmlString = await page.GetContentAsync();
        }
    }
}
