using Dapper;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using Gruppe6_Kartverket.Mvc.Models.Database;

public class DapperRepository
{
    private readonly IDbConnection _dbConnection;

    // Constructor to inject the IDbConnection
    public DapperRepository(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }

    // Example method to get all UserInfos
    public IEnumerable<UserInfo> GetUserInfos()
    {
        string sql = "SELECT * FROM UserInfos";
        return _dbConnection.Query<UserInfo>(sql);
    }

    // Add more methods as needed
}