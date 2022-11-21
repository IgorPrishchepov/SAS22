using MySqlConnector;
using System.Data;

namespace SAS22;

public class BacktestSession
{
    public int SessionId { get; set; }    
    public string TestDate { get; set; }
    public string? Pair { get; set; }
    public int TestPeriod { get; set; }
    public string? TestType { get; set; }
    public int SettingsId { get; set; }
    public int? IsActive { get; set; }

    internal AppDb Db { get; set; }

    public BacktestSession()
    {
    }

    public BacktestSession(AppDb db)
    {
        Db = db;
    }

    public async Task InsertAsync()
    {
        using var cmd = Db.Connection.CreateCommand();
        cmd.CommandText = "INSERT INTO ipfx_backtesting.backtest_session (SessionId, TestDate, Pair, TestPeriod, TestType, SettingsId, IsActive)" +
            "VALUES (@sessionId, @testDate, @pair, @testPeriod, @testType, @settingsId, @isActive);";
        BindParams(cmd);
        await cmd.ExecuteNonQueryAsync();        
    }

    public async Task UpdateAsync()
    {
        using var cmd = Db.Connection.CreateCommand();
        cmd.CommandText = @"UPDATE ipfx_backtesting.backtest_session SET IsActive=0 WHERE SessionId = @sessionId;";
        BindParams(cmd);        
        await cmd.ExecuteNonQueryAsync();
    }

    public async Task DeleteAsync()
    {
        using var cmd = Db.Connection.CreateCommand();       
        cmd.CommandText = @"DELETE FROM ipfx_backtesting.backtest_session WHERE SessionId = @sessionId";
        BindParams(cmd);
        await cmd.ExecuteNonQueryAsync();
    }

    private void BindParams(MySqlCommand cmd)
    {
        cmd.Parameters.Add(new MySqlParameter
        {
            ParameterName = "@sessionId",
            DbType = DbType.Int32,
            Value = SessionId,
        });

        cmd.Parameters.Add(new MySqlParameter
        {
            ParameterName = "@testDate",
            DbType = DbType.String,
            Value = TestDate,
        });

        cmd.Parameters.Add(new MySqlParameter
        {
            ParameterName = "@pair",
            DbType = DbType.String,
            Value = Pair,
        }); 

        cmd.Parameters.Add(new MySqlParameter
        {
            ParameterName = "@testPeriod",
            DbType = DbType.Int16,
            Value = TestPeriod,
        });

        cmd.Parameters.Add(new MySqlParameter
        {
            ParameterName = "@testType",
            DbType = DbType.String,
            Value = TestType,
        });

        cmd.Parameters.Add(new MySqlParameter
        {
            ParameterName = "@settingsId",
            DbType = DbType.Int16,
            Value = SettingsId,
        });

        cmd.Parameters.Add(new MySqlParameter
        {
            ParameterName = "@isActive",
            DbType = DbType.SByte,
            Value = IsActive,
        });
    }

    public override bool Equals(object? obj)
    {
        return obj is BacktestSession session &&
               SessionId == session.SessionId &&
               TestDate == session.TestDate &&
               Pair == session.Pair &&
               TestPeriod == session.TestPeriod &&
               TestType == session.TestType &&
               SettingsId == session.SettingsId &&
               IsActive == session.IsActive;
    }
}
