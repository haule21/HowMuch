using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HowMuch
{
    public sealed class WebApiClient
    {
        private static string CSRF_TOKEN = null;
        private static CookieContainer CookieContainer;
        private static HttpClientHandler ClientHandler;
        private static HttpClient Client;
        const string URL = "http://52.141.25.58:8080/api/";
        static WebApiClient instance = null;
        static readonly object _lock = new object();

        WebApiClient()
        {
            CookieContainer = new CookieContainer();
            ClientHandler = new HttpClientHandler { 
                UseCookies = true, 
                CookieContainer = CookieContainer
            };
            Client = new HttpClient(ClientHandler);
            Client.Timeout = TimeSpan.FromSeconds(5);


        }

        public static WebApiClient Instance
        {
            get
            {
                lock (_lock)
                {
                    if (instance == null)
                    {
                        instance = new WebApiClient();
                    }
                    return instance;
                }
            }
        }
        private void GetCookieValueFromResponse(HttpResponseMessage response)
        {
            if (CSRF_TOKEN == null) {
                if (response.Headers.TryGetValues("X-XSRF-TOKEN", out var cookies))
                {
                    CSRF_TOKEN = cookies.First();
                    Client.DefaultRequestHeaders.Add("X-XSRF-TOKEN", CSRF_TOKEN);
                }
            }
        }
            

        public async Task<string> Get(string endPoint, Param parameter = null)
        {
            string content = string.Empty;

            try
            {
                string query = "";

                if (parameter != null)
                {
                    query = parameter.GetQuery();
                }

                HttpResponseMessage response = await Client.GetAsync(URL + endPoint + query);

                // 응답이 성공적인지 확인
                if (response.IsSuccessStatusCode)
                {
                    GetCookieValueFromResponse(response);
                    Console.WriteLine($"Status Code: {response.StatusCode}");
                    content = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(content);
                }
                else
                {
                    content = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Error: {response.StatusCode}");
                }
            }
            catch (HttpRequestException ex)
            {
                if (ex == null)
                {
                    content = "서버와 연결할 수 없습니다.";
                }
                else
                {
                    content = ex.Message;
                    Console.WriteLine($"Request error: {ex.Message}");
                }
            }
            catch (TaskCanceledException ex)
            {
                // Time out   
                content = "서버와 연결할 수 없습니다.";
                Console.WriteLine($"Request error: {ex.Message}");
            }
            catch (Exception ex)
            {
                // Time out   
                content = "관리자에게 문의해 주세요.";
                Console.WriteLine($"Request error: {ex.Message}");
            }

            return content;
        }

        public async Task<string> Post(string endPoint, Param parameter = null)
        {

            string jsonData = JsonConvert.SerializeObject(parameter.GetParameter());
            StringContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            PostResponse defaultResponse = new PostResponse()
            {
                message = "",
                state = false
            };
            string responseBody = string.Empty;
            try
            {
                HttpResponseMessage response = await Client.PostAsync(URL + endPoint, content);

                if (response.IsSuccessStatusCode)
                {
                    responseBody = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Response: {responseBody}");

                    return responseBody;
                }
                else
                {
                    Console.WriteLine($"Response: {response.StatusCode}");
                    defaultResponse.message = await response.Content.ReadAsStringAsync();
                    return JsonConvert.SerializeObject(defaultResponse);
                }
            }
            catch (HttpRequestException ex)
            {
                if (ex == null)
                {
                    defaultResponse.message = "서버와 연결할 수 없습니다.";
                }
                else
                {
                    Console.WriteLine($"Request error: {ex.Message}");
                    defaultResponse.message = ex.Message;
                }
            }
            catch (TaskCanceledException ex)
            {
                // Time out   
                defaultResponse.message = "서버와 연결할 수 없습니다.";
                Console.WriteLine($"Request error: {ex.Message}");
            }
            catch (Exception ex)
            {
                // Time out   
                defaultResponse.message = "관리자에게 문의해 주세요.";
                Console.WriteLine($"Request error: {ex.Message}");
            }
            return JsonConvert.SerializeObject(defaultResponse);
        }
    }
}
