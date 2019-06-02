﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Reflection;
/// <summary>
/// Summary description for DBHelp
/// </summary>
namespace QuanLyXuatNhapKho.DAL
{
    public class DataHelper
    {
        #region Property
        //=====================Property===============================
        private SqlCommand cm;
        private SqlConnection cn;
        public static string error = "";
        //===================End Property===============================
        #endregion

        //===============================Funtion Basic===========================================
        public DataHelper()
        {
            string strCn = @"Data Source=.;Initial Catalog=QuanLyXuatNhapKho;Integrated Security=True";//ConfigurationManager.ConnectionStrings["QLKSConnectionString"].ConnectionString;
            cn = new SqlConnection(strCn);
        }

        public void Connect()
        {
            if (cn.State == ConnectionState.Closed)
            {
                try
                {
                    cn.Open();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        public void DisConnect()
        {
            if (cn.State == ConnectionState.Open)
                cn.Close();
        }

        public bool ExcuteNonQuery(string sql, SqlParameter[] prs, CommandType cmType)
        {
            try
            {
                Connect();
                cm = new SqlCommand(sql, cn);
                if (prs != null)
                    cm.Parameters.AddRange(prs);
                cm.ExecuteNonQuery();
                DisConnect();

                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                throw ex;
            }
        }

        public object ExecuteScalar(string sql, SqlParameter[] prs, CommandType cmType)
        {
            try
            {
                Connect();
                cm = new SqlCommand(sql, cn);
                if (prs != null)
                    cm.Parameters.AddRange(prs);
                object result = cm.ExecuteScalar();
                DisConnect();

                return result;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return null;
            }
        }

        public SqlDataReader ExcuteDataReader(string sql, SqlParameter[] prs, CommandType cmType)
        {
            try
            {
                Connect();
                cm = new SqlCommand(sql, cn);
                if (prs != null)
                    cm.Parameters.AddRange(prs);
                SqlDataReader dr = cm.ExecuteReader();
                dr.Close();
                DisConnect();
                return dr;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                throw ex;
            }
        }
        public DataTable ExcuteDataTable(string sql, SqlParameter[] prs, CommandType cmType)
        {
            try
            {
                Connect();
                cm = new SqlCommand(sql, cn);
                if (prs != null)
                    cm.Parameters.AddRange(prs);
                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cm);
                da.Fill(dt);
                DisConnect();
                return dt;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                throw ex;
            }
        }

        public List<T> MapReaderToList<T>(SqlDataReader r) where T : new()
        {
            List<T> res = new List<T>();
            if (r != null)
            {
                while (r.Read())
                {
                    T t = new T();

                    for (int inc = 0; inc < r.FieldCount; inc++)
                    {
                        Type type = t.GetType();
                        PropertyInfo prop = type.GetProperty(r.GetName(inc));
                        var value = r.GetValue(inc);
                        if (prop != null && value != DBNull.Value)
                        {
                            prop.SetValue(t, value, null);
                        }
                    }

                    res.Add(t);
                }
                r.Close();
            }
            return res;

        }
        public void ExecuteStoredProcedure(string procedureName, object model)
        {
            try
            {
                Connect();
                var parameters = GenerateSQLParameters(model);
                cm = new SqlCommand(procedureName, cn);
                cm.CommandType = CommandType.StoredProcedure;
                foreach (var param in parameters)
                {
                    cm.Parameters.Add(param);
                }
                cm.ExecuteNonQuery();
                DisConnect();
            }
            catch (Exception ex)
            {
                error = ex.Message;
                throw ex;
            }
        }
        private List<SqlParameter> GenerateSQLParameters(object model)
        {
            var paramList = new List<SqlParameter>();
            Type modelType = model.GetType();
            var properties = modelType.GetProperties();
            foreach (var property in properties)
            {
                if (property.GetValue(model) == null)
                {
                    paramList.Add(new SqlParameter(property.Name, DBNull.Value));
                }
                else
                {
                    paramList.Add(new SqlParameter(property.Name, property.GetValue(model)));
                }
            }
            return paramList;

        }


    }
}