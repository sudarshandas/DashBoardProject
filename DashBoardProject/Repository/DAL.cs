using DashBoardProject.Extensions;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace DashBoardProject.Repository
{
    public class DAL<T> //where T : new()
    {
        private readonly string connectionString;
        public DAL()
        {
            connectionString = ConfigurationManager.ConnectionStrings["DbCon"].ConnectionString;
        }

        public async Task<IEnumerable<T>> GetRecords(string cmdText, CommandType cmdType, SqlParameter[] arrSqlParameter)
        {
            List<T> lstItems = new List<T>();

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(cmdText, con);
                cmd.CommandType = cmdType;

                if (arrSqlParameter != null)
                {
                    cmd.Parameters.AddRange(arrSqlParameter);
                }
                //cmd.CommandTimeout = 0;
                con.Open();

                using (SqlDataReader rdr = cmd.ExecuteReader())
                {
                    while (await rdr.ReadAsync())
                    {
                        lstItems.Add(Map<T>(rdr));
                    }
                }
                con.Close();
            }

            // return hasValue == true ? lstItems : default(IEnumerable<T>);
            return lstItems;
        }

        public async Task<T> GetRecord(string cmdText, CommandType cmdType, SqlParameter[] arrSqlParameter)
        {
            bool hasValue = false;
            T item = Activator.CreateInstance<T>();

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(cmdText, con);
                cmd.CommandType = cmdType;

                if (arrSqlParameter != null)
                {
                    cmd.Parameters.AddRange(arrSqlParameter);
                }

                con.Open();
                using (SqlDataReader rdr = cmd.ExecuteReader())
                {
                    while (await rdr.ReadAsync())
                    {
                        hasValue = true;
                        item = Map<T>(rdr);
                        break;
                    }
                }
                con.Close();
            }

            return hasValue == true ? item : default(T);
        }

        public async Task<int> Execute(string cmdText, CommandType cmdType, SqlParameter[] arrSqlParameter)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(cmdText, con);
                cmd.CommandType = cmdType;

                if (arrSqlParameter != null)
                {
                    cmd.Parameters.AddRange(arrSqlParameter);
                }

                con.Open();
                return await cmd.ExecuteNonQueryAsync();
            }
        }

        public async Task<string> GetSingleValue(string cmdText, CommandType cmdType, SqlParameter[] arrSqlParameter)
        {
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(cmdText, con);
                cmd.CommandType = cmdType;

                if (arrSqlParameter != null)
                {
                    cmd.Parameters.AddRange(arrSqlParameter);
                }

                con.Open();
                var x = cmd.ExecuteScalar();
                return Convert.ToString(await cmd.ExecuteScalarAsync());
            }
        }

        public IEnumerable<T> ToList(IDbCommand command)
        {
            using (var record = command.ExecuteReader())
            {
                List<T> items = new List<T>();
                while (record.Read())
                {
                    items.Add(Map<T>(record));
                }
                return items;
            }
        }

        public T Map<T>(IDataRecord record)
        {
            var objT = Activator.CreateInstance<T>();
            try
            {
                foreach (var property in typeof(T).GetProperties())
                {
                    if (record.HasColumn(property.Name) && !record.IsDBNull(record.GetOrdinal(property.Name)))
                    {
                        if (property.PropertyType == typeof(bool) && record[property.Name].GetType() == typeof(int))
                        {
                            property.SetValue(objT, Convert.ToBoolean(record[property.Name]));
                        }
                        else
                        {
                            property.SetValue(objT, record[property.Name]);
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            
            return objT;
        }
    }
}