using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Net;
using System.Net.Http.Headers;
using System.Text;

namespace CentralTask.Core.RepositoryBase
{
    public class HttpRepositoryBase
    {
        private string _defaultContentType = "application/json";
        private string _urlBase;
        private string _token;
        private HttpClient _httpClient;

        public HttpRepositoryBase()
        {
            _httpClient = new HttpClient();
            if (_urlBase != null)
            {
                SetBaseUrl(_urlBase);
            }
        }

        protected void SetBaseUrl(string url)
        {
            if (!string.IsNullOrEmpty(url))
            {
                _urlBase = url;
                _httpClient.BaseAddress = new Uri(url);
            }
        }

        protected void SetToken(string token)
        {
            _token = token;
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        private async Task<ApiResponse<T>> ParseResponse<T>(HttpResponseMessage response)
        {
            var listErros = new List<string>();
            var result = new ApiResponse<T>()
            {
                StatusCode = 0,
                Success = false
            };

            string responseText = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                result.StatusCode = response.StatusCode;

                try
                {
                    result.Result = JsonConvert.DeserializeObject<T>(responseText, new JsonSerializerSettings
                    {
                        MissingMemberHandling = MissingMemberHandling.Error
                    });
                }
                catch (Exception ex)
                {
                    result.Result = default;
                    result.StatusCode = HttpStatusCode.InternalServerError;
                    listErros.Add($"Exception:{ex.Message}\n\nReponse:{responseText}");
                    result.ErrorMessages = listErros;
                }
            }
            else
            {
                listErros.Add(responseText);
                result.StatusCode = response.StatusCode;
                result.ErrorMessages = listErros;
            }

            result.Success = !(result.ErrorMessages != null && result.ErrorMessages.Any());

            return result;
        }

        private HttpContent GenerateHeaderAndContent(object data, Dictionary<string, string> additionalHeaders = null)
        {
            var settings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

            var jsonContent = data == null ? string.Empty : JsonConvert.SerializeObject(data, settings);
            var contentString = new StringContent(jsonContent, Encoding.UTF8, _defaultContentType);
            contentString.Headers.ContentType = new MediaTypeHeaderValue(_defaultContentType);

            if (additionalHeaders?.Any() ?? false)
            {
                foreach (var item in additionalHeaders)
                {
                    _httpClient.DefaultRequestHeaders.TryAddWithoutValidation(item.Key, item.Value);
                }
            }

            return contentString;
        }

        public async Task<ApiResponse<T>> GetAsync<T>(string action, Dictionary<string, string> additionalHeaders = null)
        {
            try
            {
                var content = GenerateHeaderAndContent(null, additionalHeaders);
                var response = await _httpClient.GetAsync(action);
                return await ParseResponse<T>(response);
            }
            catch (HttpRequestException ex)
            {
                return new ApiResponse<T>()
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Success = false,
                    ErrorMessages = new List<string>() { ex.Message }
                };
            }
        }

        public async Task<ApiResponse<T>> PostAsync<T>(string action, object data,
            Dictionary<string, string> additionalHeaders = null)
        {
            try
            {
                var content = GenerateHeaderAndContent(data, additionalHeaders);
                var response = await _httpClient.PostAsync(action, content);
                return await ParseResponse<T>(response);
            }
            catch (HttpRequestException ex)
            {
                return new ApiResponse<T>()
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Success = false,
                    ErrorMessages = new List<string>() { ex.Message }
                };
            }
        }

        public async Task<ApiResponse<T>> PostFormDataAsync<T>(string action, object data, MultipartFormDataContent formData = null)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Post, $"{_urlBase}{action}");

                if (!string.IsNullOrEmpty(_token))
                {
                    request.Headers.Add("Authorization", $"Bearer {_token}");
                }

                request.Content = formData;
                var response = await _httpClient.SendAsync(request);
                return await ParseResponse<T>(response);
            }
            catch (HttpRequestException ex)
            {
                return new ApiResponse<T>()
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Success = false,
                    ErrorMessages = new List<string>() { ex.Message }
                };
            }
        }

        public async Task<ApiResponse<T>> PutAsync<T>(string action, object data,
            Dictionary<string, string> additionalHeaders = null)
        {
            try
            {
                var content = GenerateHeaderAndContent(data, additionalHeaders);
                var response = await _httpClient.PutAsync(action, content);
                return await ParseResponse<T>(response);
            }
            catch (HttpRequestException ex)
            {
                return new ApiResponse<T>()
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Success = false,
                    ErrorMessages = new List<string>() { ex.Message }
                };
            }
        }

        public async Task<ApiResponse<T>> DeleteAsync<T>(string action, Dictionary<string, string> additionalHeaders = null)
        {
            try
            {
                var content = GenerateHeaderAndContent(null, additionalHeaders);
                var response = await _httpClient.DeleteAsync(action);
                return await ParseResponse<T>(response);
            }
            catch (HttpRequestException ex)
            {
                return new ApiResponse<T>()
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Success = false,
                    ErrorMessages = new List<string>() { ex.Message }
                };
            }
        }
    }
}