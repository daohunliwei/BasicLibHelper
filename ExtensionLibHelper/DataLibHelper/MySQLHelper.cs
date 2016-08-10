using System;
using MySql.Data.MySqlClient;
using System.Data;

namespace ExtensionLibHelper.DataLibHelper
{
    /// <summary>
    /// MySQL的链接帮助类
    /// </summary>
    public class MySQLHelper
    {
        #region 参数
        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        private string _ConString="";
        /// <summary>
        /// 数据库超时时间
        /// </summary>
        private int _Timeout = 99;
        /// <summary>
        /// 设置连接字符串
        /// </summary>
        public string ConString
        {
            set { _ConString = value; }
            get { return _ConString; }
        }
        /// <summary>
        /// 数据库超时时间
        /// </summary>
        public int Timeout
        {
            set { _Timeout = value; }
            get { return _Timeout; }
        }
        #endregion
        //public string ConnStr = "";// "User Id=root;Host=112.90.60.61,3306;Database=lifequota;password=doyouloveme123!";
        public MySQLHelper()
        { 
        }
        public MySQLHelper(string Connstr)
        {
            _ConString = Connstr;
        }

        #region Int
        public int ExecuteInt(CommandType commandType, string commandText)
        {
            return ExecuteInt(null, commandType, commandText, (MySqlParameter[])null);
        }
        public int ExecuteInt(CommandType commandType, string commandText, params MySqlParameter[] commandParameters)
        {
            return ExecuteInt(null, commandType, commandText, commandParameters);
        }
        public int ExecuteInt(string strCon, CommandType commandType, string commandText, params MySqlParameter[] commandParameters)
        {
            object o = ExecuteObject(strCon, commandType, commandText, commandParameters);
            if (o == null)
                return 0;
            else
                return Convert.ToInt32(o);
        }
        #endregion

        #region String
        public string ExecuteString(CommandType commandType, string commandText)
        {
            return ExecuteString(null, commandType, commandText, (MySqlParameter[])null);
        }
        public string ExecuteString(CommandType commandType, string commandText, params MySqlParameter[] commandParameters)
        {
            return ExecuteString(null, commandType, commandText, commandParameters);
        }
        public string ExecuteString(string strCon, CommandType commandType, string commandText, params MySqlParameter[] commandParameters)
        {
            object o = ExecuteObject(strCon, commandType, commandText, commandParameters);
            return (string)o;
        }
        #endregion

        #region Object
        public object ExecuteObject(CommandType commandType, string commandText)
        {
            return ExecuteObject(null, commandType, commandText, (MySqlParameter[])null);
        }
        public object ExecuteObject(CommandType commandType, string commandText, params MySqlParameter[] commandParameters)
        {
            return ExecuteObject(null, commandType, commandText, commandParameters);
        }
        public object ExecuteObject(string strCon, CommandType commandType, string commandText, params MySqlParameter[] commandParameters)
        {
            DataRow dr = ExecuteDataRow(strCon, commandType, commandText, commandParameters);
            if (dr == null || dr.ItemArray.Length <= 0)
                return null;
            else
                return dr.ItemArray[0];
        }
        #endregion

        #region DataRow
        public DataRow ExecuteDataRow(CommandType commandType, string commandText)
        {
            return ExecuteDataRow(null, commandType, commandText, (MySqlParameter[])null);
        }
        public DataRow ExecuteDataRow(CommandType commandType, string commandText, params MySqlParameter[] commandParameters)
        {
            return ExecuteDataRow(null, commandType, commandText, commandParameters);
        }
        public DataRow ExecuteDataRow(string strCon, CommandType commandType, string commandText, params MySqlParameter[] commandParameters)
        {
            DataTable dr = ExecuteDataTable(strCon, commandType, commandText, commandParameters);
            if (dr == null || dr.Rows.Count <= 0)
                return null;
            else
                return dr.Rows[0];
        }
        #endregion

        #region DataTable
        public DataTable ExecuteDataTable(CommandType commandType, string commandText)
        {
            return ExecuteDataTable(null, commandType, commandText, (MySqlParameter[])null);
        }
        public DataTable ExecuteDataTable(CommandType commandType, string commandText, params MySqlParameter[] commandParameters)
        {
            return ExecuteDataTable(null, commandType, commandText, commandParameters);
        }
        public DataTable ExecuteDataTable(string strCon, CommandType commandType, string commandText, params MySqlParameter[] commandParameters)
        {
            DataSet ds = ExecuteDataSet(strCon, commandType, commandText, commandParameters);
            if (ds == null || ds.Tables.Count <= 0)
                return null;
            else
                return ds.Tables[0];
        }
        #endregion

        #region DataSet
        public DataSet ExecuteDataSet(CommandType commandType, string commandText)
        {
            return ExecuteDataSet(null, commandType, commandText, (MySqlParameter[])null);
        }
        public DataSet ExecuteDataSet(CommandType commandType, string commandText, params MySqlParameter[] commandParameters)
        {
            return ExecuteDataSet(null, commandType, commandText, commandParameters);
        }
        public DataSet ExecuteDataSet(string strCon, CommandType commandType, string commandText, params MySqlParameter[] commandParameters)
        {
            if (strCon == null || strCon == "")
                strCon = _ConString;
            using (MySqlConnection conn = new MySqlConnection(strCon))
            {
                return ExecuteDataset(conn, commandType, commandText, commandParameters);
            }
        }
        public DataSet ExecuteDataset(MySqlConnection connection, CommandType commandType, string commandText, params MySqlParameter[] commandParameters)
        {
            //DateTime timeStart = DateTime.Now;
            if (connection == null) throw new ArgumentNullException("connection");

            // Create a command and prepare it for execution
            MySqlCommand cmd = new MySqlCommand(commandText, connection);
            cmd.CommandTimeout = Timeout;
            cmd.CommandType = commandType;
            if (commandParameters != null && commandParameters.Length > 0)
                cmd.Parameters.AddRange(commandParameters);
            // Create the DataAdapter & DataSet
            using (MySqlDataAdapter da = new MySqlDataAdapter(cmd))
            {
                DataSet ds = new DataSet();
                // Fill the DataSet using default values for DataTable names, etc
                connection.Open();
                da.Fill(ds);
                connection.Close();
                // Detach the MySqlParameters from the command object, so they can be used again
                cmd.Parameters.Clear();
                return ds;
            }
        }
        #endregion

        #region NonQuery
        public int ExecuteNonQuery(CommandType commandType, string commandText)
        {
            return ExecuteNonQuery(null, commandType, commandText, null);
        }
        public int ExecuteNonQuery(CommandType commandType, string commandText, params MySqlParameter[] commandParameters)
        {
            return ExecuteNonQuery(null, commandType, commandText, commandParameters);
        }
        public int ExecuteNonQuery(string strCon, CommandType commandType, string commandText, params MySqlParameter[] commandParameters)
        {
            if (strCon == null || strCon == "")
                strCon = _ConString;
            using (MySqlConnection conn = new MySqlConnection(strCon))
            {
                MySqlCommand cmd = new MySqlCommand(commandText, conn);
                cmd.CommandTimeout = Timeout;
                cmd.CommandType = commandType;
                if (commandParameters != null && commandParameters.Length > 0)
                    cmd.Parameters.AddRange(commandParameters);
                conn.Open();
                int intReturn = cmd.ExecuteNonQuery();
                conn.Close();
                cmd.Parameters.Clear();
                return intReturn;
            }
        }
        #endregion

        #region byte[]
        public byte[] Executebytes(CommandType commandType, string commandText)
        {
            return ExecuteBytes(null, commandType, commandText, (MySqlParameter[])null);
        }
        public byte[] Executebytes(CommandType commandType, string commandText, params MySqlParameter[] commandParameters)
        {
            return ExecuteBytes(null, commandType, commandText, commandParameters);
        }
        public byte[] ExecuteBytes(string strCon, CommandType commandType, string commandText, params MySqlParameter[] commandParameters)
        {
            if (strCon == null || strCon == "")
                strCon = _ConString;
            using (MySqlConnection conn = new MySqlConnection(strCon))
            {
                return Executebytes(conn, commandType, commandText, commandParameters);
            }
        }
        public byte[] Executebytes(MySqlConnection connection, CommandType commandType, string commandText, params MySqlParameter[] commandParameters)
        {
            //DateTime timeStart = DateTime.Now;
            if (connection == null) throw new ArgumentNullException("connection");

            // Create a command and prepare it for execution
            MySqlCommand cmd = new MySqlCommand(commandText, connection);
            cmd.CommandTimeout = Timeout;
            cmd.CommandType = commandType;
            if (commandParameters != null && commandParameters.Length > 0)
                cmd.Parameters.AddRange(commandParameters);
            // Create the DataAdapter & DataSet
            connection.Open();
            MySqlDataReader dr = cmd.ExecuteReader();
            // Fill the DataSet using default values for DataTable names, etc
            dr.Read();
            long l = dr.GetBytes(0, 0, null, 0, int.MaxValue);
            long lk = l;
            byte[] dys = new Byte[(dr.GetBytes(0, 0, null, 0, int.MaxValue))];
            dr.GetBytes(0, 0, dys, 0, dys.Length);
            dr.Close();
            connection.Close();
            // Detach the MySqlParameters from the command object, so they can be used again
            cmd.Parameters.Clear();
            return dys;
        }
        #endregion
    }
}