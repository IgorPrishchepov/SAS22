using FluentAssertions;
using Xunit;

namespace SAS22
{
    public class WFATests
    {
        [Theory]
        [MemberData(nameof(BacktestSessionsTestData))]
        public void BacktestSessionNegativeTests(BacktestSession backtestSession, string message)
        {
           Assert.Throws<Xunit.Sdk.XunitException>(() => WorkflowAuditor.VerifyBacktestSessionToAdd(backtestSession))
                .Message.Contains(message).Should().BeTrue();          
        }

        [Fact]
        public void BacktestSessionHappyPathTest()
        {
            Action act = () => WorkflowAuditor.VerifyBacktestSessionToAdd(new BacktestSession
            {
                SessionId = 123,
                TestDate = DateTime.Now.ToString(),
                Pair = "Test",
                TestPeriod = 12,
                TestType = "TestType",
                SettingsId = 123
            });

            act.Should().NotThrow<Xunit.Sdk.XunitException>();
        }

        public static IEnumerable<object[]> BacktestSessionsTestData()
        {
            yield return new object[] { new BacktestSession { }, "Session Id must be set <> 0" };
            yield return new object[] { new BacktestSession { SessionId = 123 }, "TestDate parameter must be of DateTime format"};
            yield return new object[] { new BacktestSession { SessionId = 123, TestDate = DateTime.Now.ToString()},
                "Pair parameter must be set" };
            yield return new object[] { new BacktestSession { SessionId = 123, TestDate = DateTime.Now.ToString(),
            Pair = "Test"}, "TestPeriod must be set > 0" };
            yield return new object[] { new BacktestSession { SessionId = 123, TestDate = DateTime.Now.ToString(),
            Pair = "Test", TestPeriod = 12}, "TestType parameter must be set" };
            yield return new object[] { new BacktestSession { SessionId = 123, TestDate = DateTime.Now.ToString(),
            Pair = "Test", TestPeriod = 12, TestType = "TestType"}, "SettingsId must be set <> 0" };            
        }
    }
}
