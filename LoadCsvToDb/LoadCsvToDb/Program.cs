using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OleDb;
using System.Data;
using System.Data.SqlClient;

namespace LoadCsvToDb
{
  class Program
  {
    static void Main(string[] args)
    {
      try
      {
        //PROVIDE LOCAL FOLDER AND NAME OF THE CSV
        string csvFile = Path.Combine(@"‪C:\myDir\", "myCSV.csv");
        //CONFIGURE CONNECTION STRING TO CSV FILE
        string extendedProperties = @"""text;HDR=YES;FMT=Delimited;Format=Delimited(,)"";";
        string csvConn = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=C:\myDir\;Extended Properties=" + extendedProperties + "";
        OleDbConnection oleDbConn = new OleDbConnection(csvConn);
        oleDbConn.Open();
        OleDbCommand oleDbCmd = new OleDbCommand("SELECT * FROM [" + Path.GetFileName(csvFile) + "]", oleDbConn);
        OleDbDataReader oleDbRdr = oleDbCmd.ExecuteReader();
        int colCount = oleDbRdr.FieldCount;
        oleDbRdr.Close();
        DataTable dt = new DataTable();
        OleDbDataAdapter da = new OleDbDataAdapter("SELECT * FROM [" + Path.GetFileName(csvFile) + "]", oleDbConn);
        da.Fill(dt);
        //PROVIDE CONNECTION STRING TO DATABASE
        SqlBulkCopy sqlBulkCopy = new SqlBulkCopy("Data Source=.;Initial Catalog=xCart; Integrated Security=true");
        //SPECIFY TABLE IN DATABASE TO LOAD THE DATA INTO
        sqlBulkCopy.DestinationTableName = "[orders]";
        sqlBulkCopy.BatchSize = 0;
        sqlBulkCopy.BulkCopyTimeout = 0;
        sqlBulkCopy.WriteToServer(dt);
        sqlBulkCopy.Close();
      }
      catch (Exception ex)
      {

        Console.WriteLine(ex.Message.ToString());
      }
      
    }
  }
}
