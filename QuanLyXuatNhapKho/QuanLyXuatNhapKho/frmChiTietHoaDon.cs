using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using QuanLyXuatNhapKho.DTO;
using QuanLyXuatNhapKho.DAL;

namespace QuanLyXuatNhapKho
{

    public partial class frmChiTietHoaDon : DevExpress.XtraEditors.XtraForm
    {
        BalloyImportDetailDAL _BalloyImportDetail = new BalloyImportDetailDAL();
        public string _ID { get; set; }
        public string _IDName { get; set; }
        public string _TotalValue { get; set; }
        private double DetailTotalValue = 0;
        public double _TotalValue1 = 0;
        public double _QTy = 0, _Price = 0;
        public DataTable dt = new DataTable();
        public frmChiTietHoaDon()
        {
            InitializeComponent();
        }
        public void Process()
        {
            _TotalValue1 = double.Parse(_TotalValue);
            var ds = _BalloyImportDetail.CreateStore("sp_BalloyImportDetail_GetList", new
            {
                ID = _ID
            });
            var ds1 = _BalloyImportDetail.CreateStore("sp_Items_GetAll", null);
            dt = ds.Tables[0];
            gCTPhieuNhap.DataSource = dt;

            searchLoaiHang.Properties.DataSource = ds1.Tables[0];
        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (searchLoaiHang.Text == "")
            {
                MessageBox.Show("Loại hàng không thể rỗng", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                searchLoaiHang.Focus();
                return;
            }
            if (txtSoLuong.Text == "")
            {
                MessageBox.Show("Chưa nhập số lượng", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtSoLuong.Focus();
                return;
            }
            if (txtDonGia.Text == "")
            {
                MessageBox.Show("Chưa nhập đơn giá cho mặt hàng này", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtDonGia.Focus();
                return;
            }
            if(dt.Rows.Count>0)
            {
                DataRow orow;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    orow = dt.Rows[i];
                    if (orow["ItemID"].ToString() == searchLoaiHang.EditValue)
                    {
                        MessageBox.Show("Mặt hàng này đã có ban chỉ được sửa thông tin không được thêm", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        searchLoaiHang.Focus();
                        return;
                    }
                }
            }

            _QTy = double.Parse(txtSoLuong.Text);
            _Price = double.Parse(txtDonGia.Text);
            DetailTotalValue += _QTy * _Price;

            if (DetailTotalValue > _TotalValue1)
            {
                
                    MessageBox.Show("Tổng trị giá lớn hơn phiếu nhập", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    DetailTotalValue -= _QTy * _Price;
                    return;
                
            }
            dt.Rows.Add(_ID, _IDName, searchLoaiHang.EditValue, searchLoaiHang.Text, txtSoLuong.Text, txtDonGia.Text);
            dt.AcceptChanges();
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            DataRow orow;
          
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                orow = dt.Rows[i];
                if (orow["ItemID"].ToString() == searchLoaiHang.EditValue)
                {
                    _QTy = double.Parse(orow["Qty"].ToString());
                    _Price = double.Parse(orow["UnitPrice"].ToString());
                    DetailTotalValue -= _QTy * _Price;
                    dt.Rows.Remove(orow);
                }
            }
            

         
            //dt.Rows.Add(_ID, _IDName, searchLoaiHang.EditValue, searchLoaiHang.Text, txtSoLuong.Text, txtDonGia.Text);
            dt.AcceptChanges();
            
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            int flag=0;
            DataRow orow;
             if (searchLoaiHang.Text == "")
            {
                MessageBox.Show("Loại hàng không thể rỗng", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                searchLoaiHang.Focus();
                return;
            }
            if (txtSoLuong.Text == "")
            {
                MessageBox.Show("Chưa nhập số lượng", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtSoLuong.Focus();
                return;
            }
            if (txtDonGia.Text == "")
            {
                MessageBox.Show("Chưa nhập đơn giá cho mặt hàng này", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtDonGia.Focus();
                return;
            }
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                orow = dt.Rows[i];
                if (orow["ItemID"].ToString() == searchLoaiHang.EditValue)
                {
                    _QTy = double.Parse(orow["Qty"].ToString());
                    _Price = double.Parse(orow["UnitPrice"].ToString());
                    DetailTotalValue -= _QTy * _Price;
                    dt.Rows.Remove(orow);
                    flag = 1;
                }
            }
            if( flag==0)
            {
                MessageBox.Show("Mặt hàng này ban chưa chọn mời ban thêm mới", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                
                return;
            }
            _QTy = double.Parse(txtSoLuong.Text);
            _Price = double.Parse(txtDonGia.Text);
            DetailTotalValue += _QTy * _Price;
            if (DetailTotalValue > _TotalValue1)
            {

                MessageBox.Show("Tổng trị giá lớn hơn phiếu nhập", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                DetailTotalValue -= _QTy * _Price;
                return;

            }
            
            dt.Rows.Add(_ID, _IDName, searchLoaiHang.EditValue, searchLoaiHang.Text, txtSoLuong.Text, txtDonGia.Text);
            dt.AcceptChanges();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {

            if (DetailTotalValue != _TotalValue1)
            {

                MessageBox.Show("Chi tiết phiếu có chênh lệch với tổng trị giá ", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                
                return;

            }

            string a, b,c;
            a = ""; b = ""; c = "";

            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    if (row["ItemID"].ToString() != "")
                    {
                        if (a == "")
                        {
                            a = row["ItemID"].ToString();
                            b = row["Qty"].ToString();
                            c = row["UnitPrice"].ToString();

                        }
                        else
                        {
                            a = a + ";" + row["ItemID"].ToString();
                            b = b + ";" + row["Qty"].ToString();
                            c = c + ";" + row["UnitPrice"].ToString();
                        }
                    }
                }
            }
            var ds = _BalloyImportDetail.CreateStore("sp_BalloyImportDetail_Save", new
            {

                BallotID = _ID,
                ItemID=a,
                Qty=b,
                UnitPrice=c
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
    }
}