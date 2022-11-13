using MySqlConnector;
using System.Data;
using System.Data.Common;

namespace SAS22
{
    public class BacktestSessionApi
    {
        public AppDb Db { get; }

        public BacktestSessionApi(AppDb db)
        {
            Db = db;
        }

        public async Task<BacktestSession?> FindOneAsync(int sessionId)
        {
            using var cmd = Db.Connection.CreateCommand();
            cmd.CommandText = @"SELECT * FROM ipfx_backtesting.backtest_session WHERE SessionId = @sessionId;";
            cmd.Parameters.Add(new MySqlParameter
            {
                ParameterName = "@sessionId",
                DbType = DbType.Int64,
                Value = sessionId
            });
            var result = await ReadAllAsync(await cmd.ExecuteReaderAsync());
            return result.Count > 0 ? result[0] : null;
        }       

        private async Task<List<BacktestSession>> ReadAllAsync(DbDataReader reader)
        {
            var sessions = new List<BacktestSession>();
            using (reader)
            {
                while (await reader.ReadAsync())
                {
                    var type = reader["IsActive"].GetType();
                    int? isActive;
                    if (type.Name == "DBNull")
                    {
                        isActive = null;
                    }
                    else
                    {
                        isActive = reader.GetInt16(6);
                    }
                    var session = new BacktestSession(Db)
                    {
                        SessionId = reader.GetInt32(0),
                        TestDate = reader.GetString(1),
                        Pair = reader.GetString(2),
                        TestPeriod = reader.GetInt32(3),
                        TestType = reader.GetString(4),
                        SettingsId = reader.GetInt32(5),
                        IsActive = isActive
                    };
                    sessions.Add(session);
                }
            }
            return sessions;
        }
    }
}
