using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication;

namespace TestApp.Proxy
{
    public class TestServiceProxy
    {
        private readonly B2CAuthenticationOptions authOptions;
        private readonly TestServiceOptions serviceOptions;
        private readonly IHttpContextAccessor httpContextAccessor;

        public TestServiceProxy(IOptions<B2CAuthenticationOptions> authOptions, IOptions<TestServiceOptions> serviceOptions, IHttpContextAccessor httpContextAccessor)
        {
            this.authOptions = authOptions.Value;
            this.serviceOptions = serviceOptions.Value;
            this.httpContextAccessor = httpContextAccessor;
        }

        public async Task<string> GetValuesAsync()
        {
            var client = new HttpClient { BaseAddress = new Uri(serviceOptions.BaseUrl, UriKind.Absolute) };
            client.DefaultRequestHeaders.Authorization = 
                new AuthenticationHeaderValue("Bearer", await GetAccessTokenAsync());

            return await client.GetStringAsync("api/values");
        }

        private Task<string> GetAccessTokenAsync()
        {
            return httpContextAccessor.HttpContext.GetTokenAsync("access_token");
        }
    }
}