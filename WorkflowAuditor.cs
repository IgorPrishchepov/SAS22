using FluentAssertions;
namespace SAS22
{
    public class WorkflowAuditor
    {
        public static void VerifyBacktestSessionToAdd(BacktestSession backtestSession)
        {
            backtestSession.SessionId.Should().NotBe(0, because: "Session Id must be set <> 0");
            DateTime.TryParse(backtestSession.TestDate, out _).Should().BeTrue(because: "TestDate parametr must be of DateTime format");
            backtestSession.Pair.Should().NotBeNullOrEmpty(because: "Pair parameter must be set");
            backtestSession.TestPeriod.Should().NotBe(0, because: "TestPeriod must be set > 0");
            backtestSession.TestType.Should().NotBeNullOrEmpty(because: "TestType parameter must be set");
            backtestSession.SettingsId.Should().NotBe(0, because: "SettingsId must be set <> 0");

        }
    }
}
