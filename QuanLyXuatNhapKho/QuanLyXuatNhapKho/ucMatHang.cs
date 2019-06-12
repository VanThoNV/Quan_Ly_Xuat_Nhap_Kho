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
    public partial class ucMatHang : DevExpress.XtraEditors.XtraUserControl
    {
        ItemsDAL _ItemsDAL = new ItemsDAL();
        public ucMatHang()
        {
            InitializeComponent();
        }
        public  void Process()
        {
            Dictionary<string, object> query = new Dictionary<string, object>();
            query.Add("sp_Items_GetAll", null);
            query.Add("sp_Unit_GetAll", null);
            query.Add("sp_ItemType_GetAll", null);
            query.Add("sp_Countries_GetAll", null);
            var ds = _ItemsDAL.ExcuteStore(query);

            gDSMatHang.DataSource = ds.Tables[0];
            lkDonViTinh.Properties.DataSource = ds.Tables[1];
            lkLoaiHang.Properties.DataSource = ds.Tables[2];
            searchlkQuocGia.Properties.DataSource = ds.Tables[3];
            gvDSMH.FocusedRowHandle = 0;
            gvDSMH_Click(null, null);
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            Clear();          
        }
        public void Clear()
        {
            txtMaMatHang.Text = "";
            txtTenMatHang.Text = "";
            txtGiaBan.Text = "";
            txtSoLuongTon.Text = "";
            lkLoaiHang.ItemIndex = 0;
            lkDonViTinh.ItemIndex = 0;
            searchlkQuocGia.EditValue = 0;
            dateEditNgaySanXuat.EditValue = null;
            dateEditNgayHetHan.EditValue = null;
        }

        public bool CheckError()
        {
            if (txtTenMatHang.Text == "")
            {
                MessageBox.Show("Vui lòng nhập tên mặt hàng!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtTenMatHang.Focus();
                return false;
            }
            if (lkLoaiHang.EditValue == "")
            {
                MessageBox.Show("Vui lòng chọn loại hàng!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                lkLoaiHang.Focus();
                return false;
            }
            if (txtGiaBan.Text == "")
            {
                MessageBox.Show("Vui lòng nhập giá bán!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtGiaBan.Focus();
                return false;
            }
            if (Convert.ToInt64(txtGiaBan.EditValue) < 0)
            {
                MessageBox.Show("Giá bán không hợp lệ, vui lòng xem lại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtGiaBan.Focus();
                return false;
            }
            if (lkDonViTinh.EditValue == null)
            {
                MessageBox.Show("Vui lòng chọn đơn vị tính!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                lkDonViTinh.Focus();
                return false;
            }
            if (dateEditNgaySanXuat.EditValue == null)
            {
                MessageBox.Show("Vui lòng nhập ngày sản xuất!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                dateEditNgaySanXuat.Focus();
                return false;
            }
            if (dateEditNgayHetHan.EditValue == null)
            {
                MessageBox.Show("Vui lòng nhập ngày hết hạn!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                dateEditNgaySanXuat.Focus();
                return false;
            }
            if (dateEditNgayHetHan.DateTime < dateEditNgaySanXuat.DateTime)
            {
                MessageBox.Show("Ngày hết hạn hoặc ngày sản xuất không đúng, vui lòng xem lại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                dateEditNgayHetHan.Focus();
                return false ;
            }
            return true;
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (CheckError() == false) return;
            var ds = _ItemsDAL.ExcuteStore("sp_Items_Save", new
            {
                ItemID = txtMaMatHang.Text,                      
                ItemName = txtTenMatHang.Text,
                ItemTypeID = lkLoaiHang.EditValue,
                UnitID = lkDonViTinh.EditValue,
                ManufactureDate = dateEditNgaySanXuat.EditValue,
                ExpirationDate = dateEditNgayHetHan.EditValue,
                ItemPrice = txtGiaBan.Text,
                QtyExists = Convert.ToInt64(txtSoLuongTon.Text),
                CountryID = searchlkQuocGia.EditValue,
                UserID = Program.CurrentUser.ID

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

        private void gvDSMH_Click(object sender, EventArgs e)
        {
            var row = gvDSMH.GetDataRow(gvDSMH.FocusedRowHandle);
            if (row == null) { Clear(); return; }
            txtMaMatHang.Text = row["ItemID"].ToString();
            txtTenMatHang.Text = row["ItemName"].ToString();
            txtGiaBan.Text = row["ItemPrice"].ToString();
            txtSoLuongTon.Text = row["QtyExists"].ToString();
            lkLoaiHang.EditValue = row["ItemTypeID"].ToString();
            lkDonViTinh.EditValue = row["UnitID"].ToString();
            searchlkQuocGia.EditValue = row["CountryID"].ToString();
            dateEditNgaySanXuat.DateTime = Convert.ToDateTime( row["ManufactureDate"]);
            dateEditNgayHetHan.DateTime = Convert.ToDateTime(row["ExpirationDate"]);
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            gvDSMH.ActiveFilterString = "[ItemID] like '%" + txtSearch.Text.Trim() + "%' OR [ItemName] like '%" + txtSearch.Text.Trim()  + "%'";
        }

        private void gvDSMH_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            //var row = gvDSMH.GetDataRow(gvDSMH.FocusedRowHandle);
            gvDSMH_Click(null, null);
        }

        private void dateEditNgayHetHang_EditValueChanged(object sender, EventArgs e)
        {
            //if(dateEditNgayHetHan.DateTime<dateEditNgaySanXuat.DateTime)
            //{
            //    MessageBox.Show("Ngày hết hạng phải lớn hơn ngày sản xuất", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                
            //    return;
            //}
        }

        private void dateEditNgaySanXuat_EditValueChanged(object sender, EventArgs e)
        {
            dateEditNgayHetHan.Enabled = true;
        }

        private void repositoryItemButtonEdit2_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
             DialogResult result = MessageBox.Show("Bạn có muốn xóa mặt hàng này không?", "Cảnh báo", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
             if (result == DialogResult.Yes)
             {
                 var ds = _ItemsDAL.ExcuteStore("sp_Items_Delete", new
                 {
                     ItemID = txtMaMatHang.Text,
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

            
        }

        private void gDSMatHang_Click(object sender, EventArgs e)
        {

        }
    }
}
