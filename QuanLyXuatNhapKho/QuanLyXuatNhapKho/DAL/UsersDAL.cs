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
    public class UsersDAL
    {
        DataHelper _helper;
        public UsersDAL()
        {
            _helper = new DataHelper();
        }
        //Lay tat ca nhung nguoi dung (Bao gom dang ton tai va da xoa)
        public List<Users> GetAll()
        {
            List<Users> listct = new List<Users>();
            string sql = "SELECT u.*, e.FullName FROM Users u JOIN Employees e ON u.ID = e.EmpID ORDER BY u.CreatedOn DESC";
            SqlDataReader dr = _helper.ExcuteDataReader(sql,null,CommandType.Text);
            listct = _helper.MapReaderToList<Users>(dr);
            _helper.DisConnect();
            return listct;
        }

        //Lay tat ca nhung nguoi dung dang ton tai
        public List<Users> GetAllExisting()
        {
            List<Users> listct = new List<Users>();
            string sql = "SELECT u.*, e.FullName FROM Users u JOIN Employees e ON u.ID = e.EmpID where u.IsDeleted = 0 ORDER BY u.CreatedOn DESC ";
            SqlDataReader dr = _helper.ExcuteDataReader(sql, null, CommandType.Text);
            listct = _helper.MapReaderToList<Users>(dr);
            _helper.DisConnect();
            return listct;
        }

        public bool Create(Users u)
        {           
            u.Password = SecurityHelper.Encrypt(u.Password);
            string sql = "INSERT INTO Users(ID, UserName, Password, IsAdmin, CreatedOn, CreatedBy, IsDeleted) VALUES(@ID, @UserName, @Password, @IsAdmin,@CreatedOn, @CreatedBy, @IsDeleted)";
            SqlParameter[] pr ={
                              new SqlParameter("@ID",u.ID),
                              new SqlParameter("@UserName",u.UserName),
                              new SqlParameter("@Password", u.Password),
                              new SqlParameter("@IsAdmin",u.IsAdmin),
                              new SqlParameter("@CreatedOn",u.CreatedOn),
                              new SqlParameter("@CreatedBy",u.CreatedBy),
                              new SqlParameter("@IsDeleted",u.IsDeleted)
                              };
           return _helper.ExcuteNonQuery(sql, pr, CommandType.Text);//excutenonquery thực thi , trả về true or false( thêm xóa xửa)
        }

        public bool Delete(int ID)
        {
            string sql = "delete Users where ID=@ID";
            SqlParameter[] pr ={
                               new SqlParameter ("@ID", ID)
                               };
            return _helper.ExcuteNonQuery(sql, pr, CommandType.Text);
        }

        public Users GetbyID(int ID)
        {
            Users u = new Users();
            string sql = "SELECT u.*, e.FullName FROM Users u JOIN Employees e u.ID = e.EmpID where ID=@ID";
            SqlParameter[] pr ={
                              new SqlParameter("@ID",ID)
                              };
            SqlDataReader dr= _helper.ExcuteDataReader(sql, pr, CommandType.Text);
            u = _helper.MapReaderToList<Users>(dr).FirstOrDefault();
            _helper.DisConnect();
            return u;
        }

        public Users DangNhap(string userName, string password)
        {
            password = SecurityHelper.Encrypt(password);            
            string sql = "SELECT * FROM Users where UserName=@UserName AND Password=@Password";
            SqlParameter[] pr ={
                              new SqlParameter("@UserName", userName),
                              new SqlParameter("@Password", password)
                              };
            SqlDataReader dr = _helper.ExcuteDataReader(sql, pr, CommandType.Text);
            Users u = _helper.MapReaderToList<Users>(dr).FirstOrDefault();
            _helper.DisConnect();
            return u;
        }

        public bool Update(Users u)
        {
            string sql = string.Empty;
            SqlParameter[] pr = new SqlParameter[] { };
            if (u.Password.Length == 0) //nếu không sửa pass
            {
                sql = "UPDATE Users SET EmpID=@EmpID, UserName=@UserName, IsAdmin=@IsAdmin, ModifiedOn =  @ModifiedOn , ModifiedBy = @ModifiedBy WHERE ID=@ID";
                pr = new SqlParameter[] {
                              new SqlParameter("@UserName",u.UserName),
                              new SqlParameter("@EmpID",u.EmpID),
                              new SqlParameter("@ID",u.ID),
                              new SqlParameter("@ModifiedOn",u.ModifiedOn),
                              new SqlParameter("@ModifiedBy",u.ModifiedBy)
                              };
            }
            else
            {
                sql = "UPDATE Users SET EmpID=@EmpID, UserName=@UserName, Password=@Password, IsAdmin=@IsAdmin, ModifiedOn =  @ModifiedOn , ModifiedBy = @ModifiedBy WHERE ID=@ID";
                pr = new SqlParameter[] {
                              new SqlParameter("@UserName",u.UserName),
                              new SqlParameter("@Password", SecurityHelper.Encrypt(u.Password)),
                              new SqlParameter("@EmpID",u.EmpID),
                              new SqlParameter("@IsAdmin",u.IsAdmin),
                              new SqlParameter("@ID",u.ID),
                              new SqlParameter("@ModifiedOn",u.ModifiedOn),
                              new SqlParameter("@ModifiedBy",u.ModifiedBy)
                              };
            }
            return _helper.ExcuteNonQuery(sql, pr, CommandType.Text);
        }

        //Update IsDeleted = 1 khi thuc hien xoa nguoi dung
        public bool UpdateXoaForQLUsers(Users u)
        {
            SqlParameter[] pr = new SqlParameter[] { };
            string sql = "UPDATE Users SET IsDeleted = 1, ModifiedOn =  @ModifiedOn , ModifiedBy = @ModifiedBy Where ID = @ID";
            pr = new SqlParameter[] {
                    new SqlParameter("@ID",u.ID),
                    new SqlParameter("@ModifiedOn",u.ModifiedOn),
                    new SqlParameter("@ModifiedBy",u.ModifiedBy)
            };
            return _helper.ExcuteNonQuery(sql, pr, CommandType.Text);
        }

        //Update DaXoa = 0 khi thuc hien them nguoi dung da tung ton tai
        public bool UpdateForQLUsers(Users u)
        {
            u.Password = SecurityHelper.Encrypt(u.Password);
            SqlParameter[] pr = new SqlParameter[] { };
            string sql = "UPDATE Users SET IsDeleted = 0, UserName = @UserName, Password = @Password,  ModifiedOn =  @ModifiedOn , ModifiedBy = @ModifiedBy Where ID = @ID";
            pr = new SqlParameter[] {
                    new SqlParameter("@UserName",u.UserName),
                    new SqlParameter("@Password", SecurityHelper.Encrypt(u.Password)),
                    new SqlParameter("@EmpID",u.EmpID),
                    new SqlParameter("@IsAdmin",u.IsAdmin),
                    new SqlParameter("@ID",u.ID),
                    new SqlParameter("@ModifiedOn",u.ModifiedOn),
                    new SqlParameter("@ModifiedBy",u.ModifiedBy)
            };
            return _helper.ExcuteNonQuery(sql, pr, CommandType.Text);
        }
    }
}