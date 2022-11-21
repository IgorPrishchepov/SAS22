using RestSharp;
using Xunit;

namespace SAS22
{
    public class ApiTests : IDisposable
    {
        List<BacktestSession> _sessionsStorage;

        public ApiTests()
        {
            _sessionsStorage = new List<BacktestSession>();
        }

        [Fact, Trait("Category", "ApiTest")]
        public void CreateDeleteSession()
        {
            var session = new BacktestSession
            {
                SessionId = 123,
                TestDate = DateTime.Now.ToString(),
                Pair = "Test",
                TestPeriod = 12,
                TestType = "TestType",
                SettingsId = 255,
                IsActive = 0
            };

            var apiClient = ApiClient.GetApiClient(path: "api/backtestsession");

            var sessionFromPost = apiClient.ExecutelCallWithBody(Method.Post, session);
            _sessionsStorage.Add(sessionFromPost);

            apiClient = ApiClient.GetApiClient(path: $"api/backtestsession/{session.SessionId}");

            var sessionFromGet = apiClient.ExecutelCall<BacktestSession>(Method.Get);
            Assert.True(sessionFromPost.Equals(sessionFromGet), "Backtest session from POST is not equal to session from GET");
        }

        public void Dispose()
        {
            _sessionsStorage.ForEach(s =>
            {
                ApiClient.GetApiClient(
                path: $"api/backtestsession",
                query: $"sessionId={s.SessionId}")
               .ExecutelCall(Method.Delete);
            });
        }
    }
}
