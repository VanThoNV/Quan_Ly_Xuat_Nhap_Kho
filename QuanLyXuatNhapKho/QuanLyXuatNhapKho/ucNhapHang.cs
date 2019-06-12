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
    public partial class ucNhapHang : DevExpress.XtraEditors.XtraUserControl
    {
        BallotImportDAL _BallotImportDAL = new BallotImportDAL();
        public string _Name;
        public string _EmployID;
        public string _EmployID1;
        public ucNhapHang()
        {
            InitializeComponent();
        }
        public void Process ()
        {
           
            var ds = _BallotImportDAL.CreateStore("sp_BallotImport_GetAll", null);
            var ds1 = _BallotImportDAL.CreateStore("sp_Supplier_GetAll", null);
           
            gDSPhieuNhap.DataSource = ds.Tables[0];
            searchNhaCungCap.Properties.DataSource = ds1.Tables[0];
            
            btnSua.Enabled = true;
            btnSave.Enabled = false;
            btnAdd.Enabled = true;
            btnXoa.Enabled = true;
            txtMaDonHang.Enabled = false;
            txtTenDonHang.Enabled = false;
            txtTongTriGia.Enabled = false;
            searchNhaCungCap.Enabled = false;
            txtNguoiLapPhieu.Enabled = false;
            dateEditNgayLapPhieu.Enabled = false;
        }
        void Clear()
        {
            txtMaDonHang.Text = "";
            txtTenDonHang.Text = "";
            txtTongTriGia.Text = "";
            searchNhaCungCap.Text = "";
            txtNguoiLapPhieu.Text = "";
            dateEditNgayLapPhieu.Text = "";
            
        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            Clear();
            _EmployID = Program.CurrentUser.ID;
            var ds2 = _BallotImportDAL.CreateStore("sp_User_GetList", new
            {
               
                EmployID = _EmployID
            }
               );
            DataRow rows = ds2.Tables[0].Rows[0];
            btnChiTiet.Enabled = false;
            btnChiTiet.Enabled = false;
            btnSua.Enabled = false;
            btnSave.Enabled = true;
            btnAdd.Enabled = false;
            btnXoa.Enabled = false;
            txtMaDonHang.Enabled = false;
            txtTenDonHang.Enabled = true;
            txtTongTriGia.Enabled = true;
            searchNhaCungCap.Enabled = true;
            txtNguoiLapPhieu.Enabled = false;
            txtNguoiLapPhieu.Text = rows["Fullname"].ToString();
            _EmployID1 = rows["EmpID"].ToString();
                dateEditNgayLapPhieu.Enabled = true;
         
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            _EmployID = Program.CurrentUser.ID;
            var ds2 = _BallotImportDAL.CreateStore("sp_Supplier_GetAll", new
            {
                Acton = 1,
                EmployID = _EmployID
            }
               );
            DataRow rows = ds2.Tables[0].Rows[0];
            btnChiTiet.Enabled = false;
            btnSua.Enabled = false;
            btnSave.Enabled = true;
            btnAdd.Enabled = false;
            btnXoa.Enabled = false;
            txtMaDonHang.Enabled = false;
            txtTenDonHang.Enabled = true;
            txtTongTriGia.Enabled = true;
            searchNhaCungCap.Enabled = true;
            txtNguoiLapPhieu.Enabled = false;
            txtNguoiLapPhieu.Text = rows["Fullname"].ToString();
            dateEditNgayLapPhieu.Enabled = true;
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            var ds = _BallotImportDAL.CreateStore("sp_BallotImport_Delete", new
            {
                ID = txtMaDonHang.Text,
         
            });
            if (ds == null)
            {
                MessageBox.Show("Có lỗi hệ thống, không xóa được.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
            {
                MessageBox.Show("Xóa thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Process();
            }
        }
         
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (txtTenDonHang.Text == "")
            {
                MessageBox.Show("Tên đơn hàng không thể rỗng", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtTenDonHang.Focus();
                return;
            }
              if (txtTongTriGia.Text == "")
            {
                MessageBox.Show("chưa nhập tổng trị giá", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtTongTriGia.Focus();
                return;
            }

            if (searchNhaCungCap.EditValue == "")
            {
                MessageBox.Show("Chưa có nha cung cấp", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                searchNhaCungCap.Focus();
                return;
            }
            if (txtNguoiLapPhieu.Text == "")
            {
                MessageBox.Show("Chưa có ngươi lâp phiếu", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtNguoiLapPhieu.Focus();
                return;
            }
            if (dateEditNgayLapPhieu.EditValue == "")
            {
                MessageBox.Show("Chưa nhập ngày sản xuất", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                dateEditNgayLapPhieu.Focus();
                return;
            }
            //if (dateEditNgayHetHang.DateTime < dateEditNgaySanXuat.DateTime)
            //{
            //    MessageBox.Show("Ngày hết hạng phải lớn hơn ngày sản xuất", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    dateEditNgayHetHang.Focus();
            //    return;
            //}
            var ds = _BallotImportDAL.CreateStore("sp_BallotImport_Save", new
            {
                ID=txtMaDonHang.Text,
                ImportDate=dateEditNgayLapPhieu.DateTime,
                TotalValue=txtTongTriGia.Text,
                EmpID=_EmployID1,

                SupplierID=searchNhaCungCap.EditValue,
                Name=txtTenDonHang.Text




            });
            if (ds == null)
            {
                MessageBox.Show("Có lỗi hệ thống, không lưu được.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return;
            }
            else
            {
                MessageBox.Show("Lưu thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Process();
            }
        }

        private void gvDSPN_Click(object sender, EventArgs e)
        {
            var row = gvDSPN.GetDataRow(gvDSPN.FocusedRowHandle);
            if (row == null) return;
            btnChiTiet.Enabled = true;
            btnAdd.Enabled = true;
            btnXoa.Enabled = true;
            btnSua.Enabled = true;
            btnSave.Enabled = false;
            txtMaDonHang.Text = row["ID"].ToString();
            txtTenDonHang.Text = row["Name"].ToString();
            txtTongTriGia.Text = row["TotalValue"].ToString();
            searchNhaCungCap.EditValue = row["SupplierID"].ToString();
            txtNguoiLapPhieu.EditValue = row["FullName"].ToString();
            dateEditNgayLapPhieu.DateTime = Convert.ToDateTime(row["ImportDate"]);

            txtMaDonHang.Enabled = false;
            txtTenDonHang.Enabled = false;
            txtTongTriGia.Enabled = false;
            searchNhaCungCap.Enabled = false;
            txtNguoiLapPhieu.Enabled = false;
            dateEditNgayLapPhieu.Enabled = false;
            
        }

        private void searchNhaCungCap_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void labelControl8_Click(object sender, EventArgs e)
        {

        }

        private void txtMaDonHang_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void txtTenDonHang_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void labelControl6_Click(object sender, EventArgs e)
        {

        }

        private void dateEditNgayLapPhieu_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void labelControl1_Click(object sender, EventArgs e)
        {

        }

        private void txtTongTriGia_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void labelControl2_Click(object sender, EventArgs e)
        {

        }

        private void labelControl3_Click(object sender, EventArgs e)
        {

        }

        private void labelControl4_Click(object sender, EventArgs e)
        {

        }

        private void labelControl7_Click(object sender, EventArgs e)
        {

        }

        private void txtSearch_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void txtNguoiLapPhieu_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void panelControl1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void groupControl3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void gDSPhieuNhap_Click(object sender, EventArgs e)
        {

            
        }

        private void panelControl3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            gvDSPN.ActiveFilterString = "[ID] like '%" + txtSearch.Text.Trim() + "%' OR [Name] like '%" + txtSearch.Text.Trim() + "%'  OR [FullName] like '%"+ txtSearch.Text.Trim() + "%'";
        }
         private void btnChiTiet_Click(object sender, EventArgs e)
        {
            frmChiTietHoaDon a = new frmChiTietHoaDon();
            a._ID = txtMaDonHang.Text;
            a._TotalValue = txtTongTriGia.Text;
            a._IDName = txtTenDonHang.Text;
            a.Process();
            a.ShowDialog();
        }
    }
}
