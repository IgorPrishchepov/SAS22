using Newtonsoft.Json;
using RestSharp;
using System;
using System.Reflection.Metadata;
using Parameter = RestSharp.Parameter;

namespace SAS22
{
    internal static class ApiClient
    {       

        public static RestClient GetApiClient(string path, string query = null)
        {
            var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            var environment = config.GetValue<string>("Environment");

            var uriBuilder = new UriBuilder()
            {
                Scheme = environment switch
                {
                    "default" => "http",
                    "cloud" => "https",
                    _ => throw new Exception("Check Environment in appsettings")
                },

                Host = environment switch
                {
                    "default" => config.GetValue<string>("RestApiHost:DefaultHost"),
                    "cloud" => config.GetValue<string>("RestApiHost:CloudHost"),
                    _ => throw new Exception("Check Environment in appsettings")
                },

                Port = environment switch
                {
                    "default" => 8000,
                    "cloud" => -1,
                    _ => throw new Exception("Check Environment in appsettings")
                },

                Path = path,
                Query = query
            };

            return new RestClient(uriBuilder.Uri);
        }


        public static T ExecutelCall<T>(
           this RestClient client,
           Method restMethod,
           params Parameter[] requestParameters)
           where T : new()
        {
            var request = new RestRequest()
            {
                Method = restMethod
            };

            if (requestParameters != null)
            {
                requestParameters.ToList().ForEach(p => request.AddParameter(p));
            }

            var response = client.Execute<T>(request);
            if (!response.IsSuccessful)
            {
                throw new Exception($"{response.ResponseUri} responded with " +
                    $"'{response.StatusCode}' error and message '{response.ErrorMessage}'");
            }

            return JsonConvert.DeserializeObject<T>(response.Content);
        }

        public static RestResponse ExecutelCall(
           this RestClient client,
           Method restMethod,
           params Parameter[] requestParameters)
        {
            var request = new RestRequest()
            {
                Method = restMethod
            };

            if (requestParameters != null)
            {
                requestParameters.ToList().ForEach(p => request.AddParameter(p));
            }

            var response = client.Execute(request);
            if (!response.IsSuccessful)
            {
                throw new Exception($"{response.ResponseUri} responded with " +
                    $"'{response.StatusCode}' error and message '{response.ErrorMessage}'");
            }

            return response;
        }

        public static T ExecutelCallWithBody<T>(
          this RestClient client,
          Method restMethod,
          T body,
          params Parameter[] requestParameters)
          where T : new()
        {
            var request = new RestRequest()
            {
                Method = restMethod
            };

            if (requestParameters != null)
            {
                requestParameters.ToList().ForEach(p => request.AddParameter(p));
            }

            var json = JsonConvert.SerializeObject(body, Formatting.Indented);
            request.AddJsonBody(json);

            var response = client.Execute<T>(request);
            if (!response.IsSuccessful)
            {
                throw new Exception($"{response.ResponseUri} responded with " +
                    $"'{response.StatusCode}' error and message '{response.ErrorMessage}'");
            }

            return JsonConvert.DeserializeObject<T>(response.Content);
        }
    }
}
