using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Threading.Tasks;
using CMS.Models;
using System.Net;
using System.Threading;

namespace CMS.Security
{
    public class ApiKeyHandler : DelegatingHandler
    {
        private static ApplicationDbContext db = new ApplicationDbContext();

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (request.Method.Equals(HttpMethod.Get))
                return await base.SendAsync(request, cancellationToken);

            bool isAPIKeyValid = false;
            IEnumerable<string> lsHeaders;

            var checkApiKeyExists = request.Headers.TryGetValues("ApiKey", out lsHeaders);

            if (checkApiKeyExists)
            {
                string apiKeyClient = lsHeaders.FirstOrDefault();
                isAPIKeyValid = db.ApiKeySet.Count(x => x.Status == true &&
                    x.ApiKeyValue.Equals(apiKeyClient)) > 0;
            }

            if (!isAPIKeyValid)
                return request.CreateResponse(HttpStatusCode.Forbidden, "Bad API Key");

            var response = await base.SendAsync(request, cancellationToken);

            return response;
        }
    }
}