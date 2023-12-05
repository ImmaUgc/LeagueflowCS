using Flurl.Http;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeagueflowCS
{
    public class FlowHttp
    {
        public LockfileContent lockfile;
        public string Endpoint;
        public FlurlClient Client;
        public FlowHttp()
        {
            lockfile = new Lockfile().lockfile;
            Endpoint = $"https://127.0.0.1:{lockfile.Port}";
            HttpClientHandler httpClientHandler = new HttpClientHandler();
            httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain,
              errors) => true;
            HttpClient httpClient = new HttpClient(httpClientHandler);
            httpClient.BaseAddress = new Uri(Endpoint);
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Basic {lockfile.Authorization}");
            Client = new FlurlClient(httpClient);
        }

        public async Task<T> GETJsonRequest<T>(string Path, object? Headers = null)
        {
            T response = await Client.Request(Path).WithHeaders(Headers).GetJsonAsync<T>();
            return response;
        }
        public async Task<string> GETStringRequest(string Path, object? Headers = null)
        {
            string response = await Client.Request(Path).WithHeaders(Headers).GetStringAsync();
            return response;
        }
        public async Task<T> POSTJsonRequest<T>(string Path,  object body, object? Headers = null)
        {
            T response = await Client.Request(Path).WithHeaders(Headers).PostJsonAsync(body).ReceiveJson<T>();
            return response;
        }
        public async Task<string> POSTStringRequest(string Path, object body, object? Headers = null)
        {
            string response = await Client.Request(Path).WithHeaders(Headers).PostJsonAsync(body).ReceiveString();
            return response;
        }
    }
}
