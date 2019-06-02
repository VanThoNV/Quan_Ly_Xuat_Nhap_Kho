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
        public void LoadData()
        {
            _emps = _empDAL.GetAllExisting();
            gDSNV.DataSource = _emps;

        }

        private void gvDSNV_Click(object sender, EventArgs e)
        {
            DataRow row = gvDSNV.GetFocusedDataRow();
            txtMaNV.Text = row["EmpID"].ToString();
            txtHoTen.Text = row["FullName"].ToString();
            txtCMND.Text = row["IdentityCard"].ToString();
            dateNgaySinh.DateTime =Convert.ToDateTime(row["WorkDay"]);
            dateNgayLamViec.DateTime = Convert.ToDateTime(row["BirthDay"]);
            txtDiaChi.Text = row["Address"].ToString();
            txtGhiChu.Text = row["Note"].ToString();
            rdbNam.Checked = Convert.ToBoolean(row["Sex"]) == true ? true : false;
            rdbNu.Checked = Convert.ToBoolean(row["Sex"]) == true ? false : true;
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
            Employees emps = new Employees()
            {
                EmpID = txtMaNV.Text,
                IdentityCard = txtCMND.Text,
                FullName = txtHoTen.Text,
                BirthDay = dateNgaySinh.DateTime,
                WorkDay = dateNgayLamViec.DateTime,
                Email = txtEmail.Text,
                Phone = txtSDT.Text,
                Address = txtDiaChi.Text,
                Note = txtGhiChu.Text,
                Sex = rdbNam.Checked
            };
            bool isSuccess = _empDAL.Save(" ", emps);
            if(isSuccess == true)
            {
                MessageBox.Show("Lưu thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Có lỗi hệ thống, không lưu được.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
    }
}
