using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RedirectorService.Models;

namespace RedirectorService.Repositories
{
    public class RedirectRepository : IRedirectRepository
    {
        private readonly string _redirectAPIResults = @"[
                {
                    'redirectUrl': '/campaignA',
                    'targetUrl': '/campaigns/targetcampaign',
                    'redirectType': 302,
                    'useRelative': false
                },
                {
                    'redirectUrl': '/campaignB',
                    'targetUrl': '/campaigns/targetcampaign/channelB',
                    'redirectType': 302,
                    'useRelative': false
                },
                {
                    'redirectUrl': '/product-directory',
                    'targetUrl': '/products',
                    'redirectType': 301,
                    'useRelative': true
                }
             ]";
        private readonly ILogger _logger;

        public RedirectRepository(ILogger logger)
        {
            _logger = logger;
        }

        

        public async Task<IEnumerable<RedirectModel>> GetAllAsync()
        {
            IEnumerable<RedirectModel> redirectModels = new List<RedirectModel>();
            try
            {
                redirectModels = await Task.Run(() =>
                {
                    List<RedirectModel>? deserializedRedirectModels = JsonConvert.DeserializeObject<List<RedirectModel>>(_redirectAPIResults);
                    if (deserializedRedirectModels == null)
                        deserializedRedirectModels = new List<RedirectModel>();

                    return deserializedRedirectModels;
                });

                if (redirectModels.Count() == 0) throw new ArgumentNullException($"the list of redirect objects returned from the API is empty...");
                else
                    _logger.LogInformation($"Successful API Call to get Redirect URLs was made");
            } catch (Exception ex)
            {
                _logger.LogError($"an error occured While retrieving the Redirect urls from the Redirect API: {ex.Message}");
            }
            return redirectModels;
        }
    }
}
