using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Common.Logging;

namespace Cheer.JsonVisualizer.UpdateUtil
{
    internal class UpdateChecker
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(UpdateChecker));

        private static readonly Uri ReleaseInfoUrl = new Uri("http://www.facebook.com/json-visualizer/client/latest-release.xml");

         

        public async Task GetProductReleaseInfoAsync()
        {
            using(var client = new HttpClient())
            {
                client.Timeout = TimeSpan.FromSeconds(10);

                string responseText = await client.GetStringAsync(ReleaseInfoUrl);

                log.DebugFormat("Response:{0}{1}", Environment.NewLine, responseText);
            }
        }
    }
}
