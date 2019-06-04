using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using QuanLyXuatNhapKho.DTO;
using QuanLyXuatNhapKho.DAL;

namespace QuanLyXuatNhapKho
{
    public partial class ucQuanLyNhanVien : XtraUserControl
    {
        List<Employees> _emps;
        EmployeesDAL _empDAL = new EmployeesDAL();
        public ucQuanLyNhanVien()
        {
            InitializeComponent();
        }
        public void Process()
        {
            //var ds = _empDAL.CreateStore("sp_Employees_GetAll", null);
            //gDSNV.DataSource = ds.Tables[0];
            //gvDSNV_Click(null, null);
        }

        private void gvDSNV_Click(object sender, EventArgs e)
        {
            var row = gvDSNV.GetDataRow(gvDSNV.FocusedRowHandle);
            if (row == null) return;
            txtMaNV.Text = row["EmpID"].ToString();
            txtHoTen.Text = row["FullName"].ToString();
            txtCMND.Text = row["IdentityCard"].ToString();
            dateNgaySinh.DateTime = Convert.ToDateTime(row["BirthDay"]);
            dateNgayLamViec.DateTime = Convert.ToDateTime(row["WorkDay"]);
            txtDiaChi.Text = row["Address"].ToString();
            txtGhiChu.Text = row["Note"].ToString();
            txtEmail.Text = row["Email"].ToString();
            txtSDT.Text = row["Phone"].ToString();
            rdbNam.Checked = row["Sex"].ToString() == "Nam" ? true : false;
            rdbNu.Checked = row["Sex"].ToString() == "Nam" ? false : true;
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            Clear();
        }

        public void Clear()
        {
            txtMaNV.Text = String.Empty;
            txtHoTen.Text = String.Empty;
            txtCMND.Text = String.Empty;
            txtDiaChi.Text = String.Empty;
            txtSDT.Text = String.Empty;
            txtGhiChu.Text = String.Empty;
            dateNgaySinh.EditValue = null;
            dateNgayLamViec.EditValue = null;
            rdbNam.Checked = true;
            rdbNu.Checked = false;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            //var isSuccess = _empDAL.CreateStore("sp_Employees_Save", new
            //{
            //    EmpID = txtMaNV.Text,
            //    IdentityCard = txtCMND.Text,
            //    FullName = txtHoTen.Text,
            //    BirthDay = dateNgaySinh.DateTime,
            //    WorkDay = dateNgayLamViec.DateTime,
            //    Email = txtEmail.Text,
            //    Phone = txtSDT.Text,
            //    Address = txtDiaChi.Text,
            //    Note = txtGhiChu.Text,
            //    Sex = rdbNam.Checked,
            //    UserID = Program.CurrentUser.ID
            //});
            //if (isSuccess == null)
            //{
            //    MessageBox.Show("Có lỗi hệ thống, không lưu được.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    return;
            //}
            //else
            //{
            //    MessageBox.Show("Lưu thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //    Process();
            //}

        }

        private void ucQuanLyNhanVien_Load(object sender, EventArgs e)
        {
            //LoadData();
        }

        private void repositoryItemButtonEdit1_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            //DialogResult result = MessageBox.Show("Bạn có muốn xóa nhân viên này không?", "Cảnh báo", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            //if (result == DialogResult.Yes)
            //{
            //    var row = gvDSNV.GetDataRow(gvDSNV.FocusedRowHandle);
            //    if (row == null) return;
            //    var isSuccess = _empDAL.CreateStore("sp_Employees_Delete", new
            //    {
            //        EmpID =  row["EmpID"].ToString()
            //    });
            //    if (isSuccess == null)
            //    {
            //        MessageBox.Show("Có lỗi hệ thống, không xóa được.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //        return;
            //    }
            //    else
            //    {
            //        MessageBox.Show("Xóa thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //        Process();
            //    }
            //}
        }

        private void gvDSNV_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            gvDSNV_Click(null, null);
        }

        private void textEdit7_TextChanged(object sender, EventArgs e)
        {
            gvDSNV.ActiveFilterString = "[EmpID] like '%" + txtSearch.Text.Trim() + "%' OR [FullName] like '%" + txtSearch.Text.Trim() + "%' OR [Phone] like '%" + txtSearch.Text.Trim() + "%'";
        }
    }
}
