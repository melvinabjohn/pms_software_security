using System;
using Microsoft.Data.SqlClient;
using System.Data;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Test1.Data
{
    /// <summary>
    /// Data provider interface.      
    /// </summary>
    //public interface IDataProvider : IDisposable
    //{
    //    //List<T> FetchResultsForQuery<T>(Query query, IRowMapper<T> mapper);
    //    //List<T> FetchResultsForQuery<T>(Query query, IResultsMapper<T> mapper);
    //    //DataTable FetchResultsForQuery(Query query);
    //    //DataSet FetchResultsForQueryAsDataSet(Query query);
    //    IDbConnection GetConnection();
    //    //IDbCommand ConstructSQLCommand(IDbConnection connection, Query query);
    //}


    //public class SqlDataReadProvider : IDataProvider
    //{
    //    #region Private Fields
    //    private readonly string _connectionString;
    //    private readonly SqlConnection _connection;

    //    #endregion



    //    public SqlDataReadProvider()
    //    {

    //    }
    //    /// <summary>
    //    /// SQL Data Provider
    //    /// </summary>
    //    /// <param name="connectionString"></param>
    //    public SqlDataReadProvider(string connectionString)
    //    {
    //        _connectionString = connectionString;
    //        _connection = new SqlConnection(connectionString);
    //    }

    //    public SqlDataReadProvider(SqlConnection Connection)
    //    {
    //        _connection = Connection;
    //    }

    //    /// <summary>
    //    /// Get the SQL connection
    //    /// </summary>
    //    /// <returns></returns>

    //    public SqlConnection GetConnection()
    //    {
    //        SqlConnection sqlConnection = new SqlConnection(this._connectionString);
    //        sqlConnection.Open();
    //        return sqlConnection;
    //    }

    //    IDbConnection IDataProvider.GetConnection()
    //    {
    //        throw new NotImplementedException();
    //    }
    //    void IDisposable.Dispose()
    //    {
    //        _connection.Dispose();
    //    }
    //}
}