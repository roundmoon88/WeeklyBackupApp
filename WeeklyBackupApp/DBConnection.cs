using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Configuration;
using System.Data;
using System.Data.OleDb;
namespace WeeklyBackupApp
{
    public class DBConnection
    {
        private readonly string connectionString;
        public DBConnection(string computerLocation)
        {
            if (computerLocation == "Office")
                connectionString = ConfigurationManager.ConnectionStrings["OfficeDb"].ConnectionString;
            else
                connectionString = ConfigurationManager.ConnectionStrings["HomeDb"].ConnectionString;
        }

        public DataSet GetDataSet(string sql)
        {
            DataSet ds = new DataSet();
            using (OleDbConnection dbConnection = new OleDbConnection(this.connectionString))
            {
                using (OleDbCommand dbCommand = new OleDbCommand())
                {
                    if (sql == null)
                        sql = "select * from [log$]";
                    //string sql = "select * from [log$]";
                    dbCommand.Connection = dbConnection;
                    
                    dbCommand.CommandType = CommandType.Text;
                    dbCommand.CommandText = sql;
                    dbConnection.Open();

                    OleDbDataAdapter adapter = new OleDbDataAdapter(dbCommand);
                    adapter.Fill(ds);

                }
            }
            //throw new NotImplementedException();
            return ds;
        }
        public int InsertLogFile(FileLogDomain file)
        {
            int retVal = 0;
            int logId = GetLogID();
            file.LogID = logId;

            using (OleDbConnection dbConnection = new OleDbConnection(this.connectionString))
            {
                using (OleDbCommand dbCommand = new OleDbCommand())
                {
                    dbCommand.Connection = dbConnection;
                    dbCommand.CommandType = CommandType.Text;
                    //string sql = " insert into [log$] (LogId, LogDate, FileName, FilePath, NewFileInd ) values (" + file.LogID.ToString() + ", \"" + @file.LogDate.ToString() + "\" , \""  + file.FileName + "\", \"" + file.FilePath + "\", " + file.NewFileInd.ToString() + ")" ;
                    //string sql = " insert into [log$] (LogId, LogDate, FileName, FilePath, NewFileInd, TimeStamp ) values (" + file.LogID.ToString() + ", \"" + @file.LogDate.ToString() + "\" , \"" + file.FileName + "\", \"" + file.FilePath + "\", " + file.NewFileInd.ToString() +",\"" + file.TimeStamp  + "\")";
                    //string sql = " insert into [log$] (LogId, LogDate, FileName, FilePath, NewFileInd, TimeStamp ) values (" + file.LogID.ToString() + ", \"" + @file.LogDate.ToString() + "\" , \"" + file.FileName + "\", \"" + file.FilePath + "\", " + file.NewFileInd.ToString() + ",\"" + @file.LogDate.ToString() + "\")";
                    //string sql = " insert into [log$] (LogId, LogDate, FileName, FilePath, NewFileInd, LogDate2 ) values (" + file.LogID.ToString() + ", \"" + file.LogDate.ToString() + "\" , \"" + file.FileName + "\", \"" + file.FilePath + "\", " + file.NewFileInd.ToString() + ", \"timestamp\"" + ")";  //good
                    //string sql = " insert into [log$] (LogId, LogDate, FileName, FilePath, NewFileInd, LogDate2 ) values (" + file.LogID.ToString() + ", \"" + file.LogDate.ToString() + "\" , \"" + file.FileName + "\", \"" + file.FilePath + "\", " + file.NewFileInd.ToString() + ", \"" + "timestamp" + "\"" + ")";  //good
                    //string sql = " insert into [log$] (LogId, LogDate, FileName, FilePath, NewFileInd, LogDate2 ) values (" + file.LogID.ToString() + ", \"" + file.LogDate.ToString() + "\" , \"" + file.FileName + "\", \"" + file.FilePath + "\", " + file.NewFileInd.ToString() + ", \"" + file.TimeStamp + "\"" + ")";  //good
                    string sql = " insert into [log$] (LogId, LogDate, FileName, FilePath, NewFileInd, Timestamp2 ) values (" + file.LogID.ToString() + ", \"" + file.LogDate.ToString() + "\" , \"" + file.FileName + "\", \"" + file.FilePath + "\", " + file.NewFileInd.ToString() + ", \"" + file.TimeStamp + "\"" + ")";  //good
                    //
                    dbCommand.CommandText = sql;
                    dbCommand.Connection.Open();
                    retVal = dbCommand.ExecuteNonQuery();
                }
            }
            return retVal;
               // throw new NotImplementedException();

        }

        /// <summary>
        /// Method to get a distinct LogId
        /// </summary>
        /// <returns></returns>
        public int GetLogID()
        {
            int curLogId = 0;
            string sql = "select max(LogID) as maxCurLogId from [log$]";
            DataSet ds = GetDataSet(sql);
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0][0] != DBNull.Value)
                    curLogId = Convert.ToInt32(ds.Tables[0].Rows[0][0]);
                else
                    curLogId = 0;
            }
                

            return curLogId + 1;
        }
    }
}
