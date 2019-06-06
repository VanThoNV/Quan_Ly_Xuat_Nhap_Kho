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

            var   ds = _ItemsDAL.CreateStore("sp_Items_GetAll", null );
            var ds1 = _ItemsDAL.CreateStore("sp_Unit_GetAll", null);
            var ds2 = _ItemsDAL.CreateStore("sp_ItemType_GetAll", null);

            gDSMatHang.DataSource = ds.Tables[0];
            searchDonViTinh.Properties.DataSource = ds1.Tables[0];
            searchLoaiHang.Properties.DataSource = ds2.Tables[0];
            btnSua.Enabled = true;
            btnSave.Enabled = false;
            btnAdd.Enabled = true;
            btnXoa.Enabled = true;

            txtMaMatHang.Enabled = false;
            txtTenMatHang.Enabled = false;
            txtGiaBan.Enabled = false;
            searchLoaiHang .Enabled = false;
            searchDonViTinh.Enabled = false;
            dateEditNgaySanXuat.Enabled = false;
            dateEditNgayHetHang.Enabled = false;
            gvDSMH.FocusedRowHandle = 0;
            gvDSMH_Click(null, null);
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            Clear();
            btnSua.Enabled = false;
            btnSave.Enabled = true;
            btnAdd.Enabled = false;
            btnXoa.Enabled = false;

            txtMaMatHang     .Enabled =           false;
            txtTenMatHang    .Enabled =          true;
            txtGiaBan       .Enabled =             true;
            searchLoaiHang     .Enabled =            true;
            searchDonViTinh  .Enabled =       true;
            dateEditNgaySanXuat.Enabled =   true;
            dateEditNgayHetHang.Enabled = false;
            
        }
        public void Clear()
        {
            txtMaMatHang.Text = "";
            txtTenMatHang.Text = "";
            txtGiaBan.Text = "";
            searchLoaiHang.Text = "";
            searchDonViTinh.Text = "";
            dateEditNgaySanXuat.Text = "";
            dateEditNgayHetHang.Text = "";
        }
        private void btnSua_Click(object sender, EventArgs e)
        {
            btnSua.Enabled = false;
            btnSave.Enabled = true;
            btnAdd.Enabled = false;
            btnXoa.Enabled = false;
            txtMaMatHang.Enabled = false;
            txtTenMatHang.Enabled = true;
            txtGiaBan.Enabled = true;
            searchDonViTinh.Enabled = true;
            searchLoaiHang.Enabled = true;
            dateEditNgaySanXuat.Enabled = true;
            dateEditNgayHetHang.Enabled = false;
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            var ds = _ItemsDAL.CreateStore("sp_Customer_Delete", new
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

        private void btnSave_Click(object sender, EventArgs e)
        {
            if(txtTenMatHang.Text=="")
            {
                MessageBox.Show("Tên mặt hàng không thể rỗng", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtTenMatHang.Focus();
                return;
            }
            
            if(searchLoaiHang.EditValue=="")
            {
                MessageBox.Show("Chưa nhập loại hàng", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                searchLoaiHang.Focus();
                return;
            }
            if (searchDonViTinh.EditValue == "")
            {
                MessageBox.Show("Chưa nhập đơn vị tính", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                searchDonViTinh.Focus();
                return;
            }
            if(dateEditNgaySanXuat.EditValue=="")
            {
                MessageBox.Show("Chưa nhập ngày sản xuất", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                dateEditNgaySanXuat.Focus();
                return;
            }
            if (dateEditNgayHetHang.DateTime < dateEditNgaySanXuat.DateTime)
            {
                MessageBox.Show("Ngày hết hạng phải lớn hơn ngày sản xuất", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                dateEditNgayHetHang.Focus();
                return;
            }
            var ds = _ItemsDAL.CreateStore("sp_Items_Save", new
            {
                ItemID        =txtMaMatHang.Text,                      
                ItemName=txtTenMatHang.Text,
                ItemTypeID=searchLoaiHang.EditValue,
                UnitID=searchDonViTinh.EditValue,
                ManufactureDate =dateEditNgaySanXuat.DateTime,
                ExpirationDate=dateEditNgayHetHang.DateTime,
                QtyExists=txtGiaBan.Text

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
            if (row == null) return;

            btnAdd.Enabled = true;
            btnXoa.Enabled = true;
            btnSua.Enabled = true;
            btnSave.Enabled = false;
            txtMaMatHang.Text = row["ItemID"].ToString();
            txtTenMatHang.Text = row["ItemName"].ToString();
            txtGiaBan.Text = row["QtyExists"].ToString();
            searchLoaiHang.EditValue = row["ItemTypeID"].ToString();
            searchDonViTinh.EditValue = row["UnitID"].ToString();
            dateEditNgaySanXuat.DateTime = Convert.ToDateTime( row["ManufactureDate"]);
            dateEditNgayHetHang.DateTime = Convert.ToDateTime(row["ExpirationDate"]);
            txtMaMatHang.Enabled = false;
            txtTenMatHang.Enabled = false;
            txtGiaBan.Enabled = false;
            searchLoaiHang.Enabled = false;
            searchDonViTinh.Enabled = false;
            dateEditNgaySanXuat.Enabled = false;
            dateEditNgayHetHang.Enabled = false;
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
            if(dateEditNgayHetHang.DateTime<dateEditNgaySanXuat.DateTime)
            {
                MessageBox.Show("Ngày hết hạng phải lớn hơn ngày sản xuất", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                
                return;
            }
        }

        private void dateEditNgaySanXuat_EditValueChanged(object sender, EventArgs e)
        {
            dateEditNgayHetHang.Enabled = true;
        }
    }
}
