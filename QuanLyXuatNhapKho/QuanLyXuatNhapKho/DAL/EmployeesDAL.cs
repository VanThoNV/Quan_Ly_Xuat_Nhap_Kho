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
    public class EmployeesDAL
    {
        DataHelper _helper;
        public EmployeesDAL()
        {
            _helper = new DataHelper();
        }

        //Lay tat ca nhung nhan vien (Bao gom dang ton tai va da xoa)
        public List<Employees> GetAll()
        {
            List<Employees> listct = new List<Employees>();
            string sql = "SELECT * FROM Employees ORDER BY CreatedOn DESC";
            SqlDataReader dr = _helper.ExcuteDataReader(sql,null,CommandType.Text);
            listct = _helper.MapReaderToList<Employees>(dr);
            _helper.DisConnect();
            return listct;
        }

        //Lay tat ca nhung nhan vien dang ton tai
        public List<Employees> GetAllExisting()
        {
            List<Employees> listct = new List<Employees>();
            string sql = "SELECT * FROM Employees WHERE IsDeleted = 0 ORDER BY CreatedOn DESC";
            SqlDataReader dr = _helper.ExcuteDataReader(sql, null, CommandType.Text);
            listct = _helper.MapReaderToList<Employees>(dr);
            _helper.DisConnect();
            return listct;
        }

        //Lay nhan vien theo EmpID
        public Employees GetKhachHangByEmpID(int EmpID)
        {
            Employees e = new Employees();
            string sql = "SELECT * FROM Employees where EmpID=@EmpID";
            SqlParameter[] pr = {
                                new SqlParameter("@EmpID",EmpID )
                                };
            SqlDataReader dr = _helper.ExcuteDataReader(sql, pr, CommandType.Text);
            e = _helper.MapReaderToList<Employees>(dr).FirstOrDefault();
            _helper.DisConnect();
            return e;
        }

        //Lay nhan vien theo ten nhan vien
        public List<Employees> GetKhachHangByName(string Name)
        {
            List<Employees> listkhs = new List<Employees>();
            string sql = "SELECT * FROM Employees where FullName like 'N%" + Name + "%'";
            SqlParameter[] pr = {
                                new SqlParameter("@Name", Name)
                                };
            SqlDataReader dr = _helper.ExcuteDataReader(sql, pr, CommandType.Text);
            listkhs = _helper.MapReaderToList<Employees>(dr);
            _helper.DisConnect();
            return listkhs;
        }

        public bool Create(Employees e)
        {
            string sql = "Insert into Employees(FullName, BirthDay, Phone, IdentityCard, Address, Sex, Email, NgayLamViec, Note, CreatedOn, CreatedBy, IsDeleted) values (@FullName, @BirthDay, @Phone, @IdentityCard, @Address, @Sex, @Email, @NgayLamViec, @Note, @CreatedOn, @CreatedBy, @IsDeleted)";
            SqlParameter[] pr = {
                                new SqlParameter("@FullName",e.FullName ),
                                new SqlParameter("@BirthDay",e.BirthDay ),
                                new SqlParameter("@Phone", e.Phone),
                                new SqlParameter("@IdentityCard", e.IdentityCard ),
                                new SqlParameter("@Address",e.Address ),
                                new SqlParameter("@Sex", e.Sex ),
                                new SqlParameter("@Email", e.Email ),
                                new SqlParameter("@NgayLamViec", e.WorkDay ),
                                new SqlParameter("@CreatedOn",e.CreatedOn),
                                new SqlParameter("@CreatedBy",e.CreatedBy),
                                new SqlParameter("@IsDeleted", e.IsDeleted)
                              };
            return _helper.ExcuteNonQuery(sql, pr, CommandType.Text);
        }

        public bool Update(Employees nv)
        {
            string sql = "Update Employees set FullName = @FullName, BirthDay = @BirthDay, Phone = @Phone, IdentityCard = @IdentityCard, Address = @Address, Sex = @Sex, Email = @Email, NgayLamViec = @NgayLamViec, CreatedOn = @CreatedOn, CreatedBy = @CreatedBy where  EmpID = @EmpID";
            SqlParameter[] pr = {
                                new SqlParameter("@EmpID",nv.EmpID ),
                                new SqlParameter("@FullName",nv.FullName ),
                                new SqlParameter("@BirthDay",nv.BirthDay ),
                                new SqlParameter("@Phone", nv.Phone ),
                                new SqlParameter("@IdentityCard", nv.IdentityCard ),
                                new SqlParameter("@Address",nv.Address ),
                                new SqlParameter("@Sex", nv.Sex ),
                                new SqlParameter("@Email", nv.Email ),
                                new SqlParameter("@NgayLamViec", nv.WorkDay ),
                                new SqlParameter("@CreatedOn", nv.CreatedOn ),
                                new SqlParameter("@CreatedBy", nv.CreatedBy)
                              };
            return _helper.ExcuteNonQuery(sql, pr, CommandType.Text);
        }

        //Update IsDeleted = 1 khi thuc hien xoa nhan vien
        public bool UpdateXoaForQLEmployees(Employees nv)
        {
            SqlParameter[] pr = new SqlParameter[] { };
            string sql = "UPDATE Employees SET IsDeleted = 1, CreatedOn=@CreatedOn, CreatedBy=@CreatedBy Where EmpID = @EmpID";
            pr = new SqlParameter[] {
                    new SqlParameter("@EmpID",nv.EmpID),
                    new SqlParameter("@CreatedOn",nv.CreatedOn),
                    new SqlParameter("@CreatedBy", nv.CreatedBy)
            };
            return _helper.ExcuteNonQuery(sql, pr, CommandType.Text);
        }

        //Update IsDeleted = 0 khi thuc hien them nhan vien co tenNV va IdentityCard da tung ton tai
        public bool UpdateForQLEmployees(Employees nv)
        {
            SqlParameter[] pr = new SqlParameter[] { };
            string sql = "UPDATE Employees SET IsDeleted = 0, BirthDay = @BirthDay, Phone = @Phone, Address = @Address, Sex = @Sex, Email = @Email, NgayLamViec = @NgayLamViec, CreatedOn = @CreatedOn, CreatedBy = @CreatedBy where  FullName = @FullName and IdentityCard = @IdentityCard";
            pr = new SqlParameter[] {
                                new SqlParameter("@EmpID",nv.EmpID ),
                                new SqlParameter("@FullName",nv.FullName ),
                                new SqlParameter("@BirthDay",nv.BirthDay ),
                                new SqlParameter("@Phone", nv.Phone ),
                                new SqlParameter("@IdentityCard", nv.IdentityCard ),
                                new SqlParameter("@Address",nv.Address ),
                                new SqlParameter("@Sex", nv.Sex ),
                                new SqlParameter("@Email", nv.Email ),
                                new SqlParameter("@NgayLamViec", nv.WorkDay ),
                                new SqlParameter("@CreatedOn", nv.CreatedOn ),
                                new SqlParameter("@CreatedBy", nv.CreatedBy)
            };
            return _helper.ExcuteNonQuery(sql, pr, CommandType.Text);
        }
        public bool Delete(int EmpID)
        {
            string sql = "Delete Employees where EmpID = @EmpID";
            SqlParameter[] pr ={
                               new SqlParameter ("@EmpID", EmpID)
                               };
            return _helper.ExcuteNonQuery(sql, pr, CommandType.Text);
        }

        public bool Save(string procedureName,Employees nv)
        {
            try
            {
                _helper.ExecuteStoredProcedure(procedureName, nv);
                return true;
            }
            catch
            {
                return false;
            }
            
        }
    }
}