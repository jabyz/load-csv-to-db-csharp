using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace LoadFilesToDb
{
  class sqlService
  {
    public SqlConnection getSqlConn()
    {
      string conn = System.Configuration.ConfigurationManager.ConnectionStrings["NOVA-E10-TSQL"].ConnectionString;
      SqlConnection sqlConn = new SqlConnection(conn);

      return sqlConn;
    }

    public DataTable cmdToDataTable(SqlCommand cmd)
    {
      using (SqlConnection sqlConn = getSqlConn())
      {
        if (sqlConn.State == ConnectionState.Open)
          sqlConn.Close();
        try
        {
          cmd.Connection = sqlConn;
          cmd.CommandTimeout = 0;
          sqlConn.Open();
          SqlDataAdapter da = new SqlDataAdapter(cmd);
          DataTable dt = new DataTable();
          da.Fill(dt);
          cmd.Dispose();
          sqlConn.Close();
          sqlConn.Dispose();
          return dt;
        }
        catch (Exception ex)
        {

          cmd.Dispose();
          sqlConn.Close();
          sqlConn.Dispose();
          return null;
        }
      }
    }

    public int xQuery(SqlCommand cmd)
    {
      int ra = 0;
      using (SqlConnection sqlConn = getSqlConn())
      {
        if (sqlConn.State == ConnectionState.Open)
          sqlConn.Close();
        try
        {
          cmd.Connection = sqlConn;
          cmd.CommandTimeout = 0;
          sqlConn.Open();
          ra = cmd.ExecuteNonQuery();

          cmd.Dispose();
          sqlConn.Close();
          sqlConn.Dispose();

          return ra;
        }
        catch (Exception ex)
        {

          cmd.Dispose();
          sqlConn.Close();
          sqlConn.Dispose();
          return 0;
        }
      }
    }
  }
}
