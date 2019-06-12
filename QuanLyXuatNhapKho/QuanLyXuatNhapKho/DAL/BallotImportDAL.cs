using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using QuanLyXuatNhapKho.DTO;
using System.Configuration;
namespace QuanLyXuatNhapKho.DAL
{
    class BallotImportDAL
    {DataHelper _helper;
    public BallotImportDAL()
        {
            _helper = new DataHelper();
        }
        public DataSet CreateStore(string procedureName, object model)
        {
            return _helper.ExecuteStoredProcedure(procedureName, model);
        }
    }
}
