using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using QuanLyXuatNhapKho.DTO;
using System.Configuration;

namespace QuanLyXuatNhapKho.DAL
{
    class BalloyImportDetailDAL
    { 
        DataHelper _helper;
        public BalloyImportDetailDAL()
        {
            _helper = new DataHelper();
        }
        public DataSet CreateStore(string procedureName, object model)
        {
            return _helper.ExecuteStoredProcedure(procedureName, model);
        }
    }
}
