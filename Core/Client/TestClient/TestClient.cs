using $safeprojectname$.Core.Services.CacheService;
using $safeprojectname$.Models.Config;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace $safeprojectname$.Core.Client.TestClient
{
    public class TestClient : APIClient, ITestClient
    {
        private readonly TestConfig _testConfig;
        private readonly ICacheService _cacheService;

        public TestClient(IOptions<TestConfig> testConfig, ICacheService cacheService, IBaseClient baseClient) : base(baseClient)
        {
            _testConfig = testConfig.Value;
            _cacheService = cacheService;
        }

        public async Task<List<int>> GetTest(string data)
        {
            return await _cacheService.GetAsync("GetTestListings", async () =>
            {
                NameValueCollection queryString = HttpUtility.ParseQueryString(string.Empty);
                queryString.Add("api_key", _testConfig.APIKey);

                var result = await GetAsync<List<int>>(_testConfig.BaseUrl, $"/v1.4/test/{data}?{queryString}");

                return result.Data;
            }, 1440).Unwrap();
        }
    }
}
