using IdentityModel.Client;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleClient
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            MainAsync().GetAwaiter().GetResult();
        }

        private static async Task MainAsync()
        {
            var tokenResponse =await ClientCredentials();
            //var tokenResponse = await ResourceOwner();
            await CallClient(tokenResponse);

            Console.Read();

        }

        private static async Task CallClient(TokenResponse tokenResponse)
        {
            //Consume our Customer API
            HttpClient client = new HttpClient();
            client.SetBearerToken(tokenResponse.AccessToken);

            StringContent customerInfo = new StringContent(
                JsonConvert.SerializeObject(
                        new { Id = 10, FirstName = "Manish", LastName = "Narayan" }),
                        Encoding.UTF8, "application/json");

            HttpResponseMessage createCustomerResponse = await client.PostAsync("http://localhost:64789/api/customers"
                                                            , customerInfo);

            if (!createCustomerResponse.IsSuccessStatusCode)
            {
                Console.WriteLine(createCustomerResponse.StatusCode);
            }

            HttpResponseMessage getCustomerResponse = await client.GetAsync("http://localhost:64789/api/customers");
            if (!getCustomerResponse.IsSuccessStatusCode)
            {
                Console.WriteLine(getCustomerResponse.StatusCode);
            }
            else
            {
                string content = await getCustomerResponse.Content.ReadAsStringAsync();
                Console.WriteLine(JArray.Parse(content));
            }
        }

        public static async Task<TokenResponse> ClientCredentials()
        {
            //discover all the endpoints using metadata of identity server
            DiscoveryResponse disco = await DiscoveryClient.GetAsync("http://localhost:5000");
            if (disco.IsError)
            {
                Console.WriteLine(disco.Error);
                return null;
            }

            //Grab a bearer token
            TokenClient tokenClient = new TokenClient(disco.TokenEndpoint, "client", "secret");
            TokenResponse tokenResponse = await tokenClient.RequestClientCredentialsAsync("bankOfDotNetApi");

            if (tokenResponse.IsError)
            {
                Console.WriteLine(tokenResponse.Error);
                return null;
            }

            Console.WriteLine(tokenResponse.Json);
            Console.WriteLine("\n\n");
            return tokenResponse;
        }

        public static async Task<TokenResponse> ResourceOwner()
        {
            //discover all the endpoints using metadata of identity server
            DiscoveryResponse disco = await DiscoveryClient.GetAsync("http://localhost:5000");
            if (disco.IsError)
            {
                Console.WriteLine(disco.Error);
                return null;
            }

            //Grab a bearer token
            TokenClient tokenClientRO = new TokenClient(disco.TokenEndpoint, "ro.client", "secret");
            TokenResponse tokenResponseRO = await tokenClientRO.RequestResourceOwnerPasswordAsync("Kaushik1", "password", "bankOfDotNetApi");

            if (tokenResponseRO.IsError)
            {
                Console.WriteLine(tokenResponseRO.Error);
                return null;
            }


            Console.WriteLine(tokenResponseRO.Json);
            Console.WriteLine("\n\n");
            return tokenResponseRO;
        }
    }
}
