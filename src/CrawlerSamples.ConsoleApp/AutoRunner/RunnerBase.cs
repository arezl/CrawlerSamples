using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using PuppeteerSharp;

namespace CrawlerSamples.AutoRunner
{
    public abstract class RunnerBase
    {
        public bool IsReload { get; set; }
        public bool IsConnect { get; set; }
        public string BrowserWSEndpoint { get; set; }
        public string URL { get; set; }

        public RunnerBase()
        {
            BrowserWSEndpoint = "ws://127.0.0.1:64426/devtools/browser/fff7bbe4-51cd-4912-893a-be12abab58d8";
        }

        internal abstract Task GetDatas(Page page);
    }
}
