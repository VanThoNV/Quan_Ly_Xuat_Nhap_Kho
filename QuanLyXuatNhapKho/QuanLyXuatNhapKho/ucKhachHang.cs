using System.Drawing;
using System.Data;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using QuanLyXuatNhapKho.DTO;
using QuanLyXuatNhapKho.DAL;
using System;
using System.IO;
using System.Collections.Generic;
namespace QuanLyXuatNhapKho
{
    public partial class ucKhachHang : DevExpress.XtraEditors.XtraUserControl
    {

        string appPath = Path.GetDirectoryName(Application.ExecutablePath) + @"\Images\";
        DataHelper _helper = new DataHelper();
        public ucKhachHang()
        {
            InitializeComponent();
        }
        public void Process()
        {
            Dictionary<string, object> query = new Dictionary<string, object>();
            query.Add("sp_Customer_GetAll", null);
            query.Add("sp_CustomerType_GetAll", null);
            var ds = _helper.ExcuteStore(query);
            if(ds == null)
            {
                MessageBox.Show("Có lỗi hệ thống, vui lòng liên hệ người quản trị.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            gDSKhachHang.DataSource = ds.Tables[0];
            lkLoaiKhachHang.Properties.DataSource = ds.Tables[1];
            if (gDSKhachHang.DataSource == null) lkLoaiKhachHang.ItemIndex = 0;
            gvDSKH_Click(null, null);
        }
        public void Clear()
        {
            txtMaKH.Text = "";
            txtHoTen.Text = "";
            txtCMND.Text = "";
            txtDiaChi.Text = "";
            txtSDT.Text = "";
            txtEmail.Text = "";
            pictureEdit1.Image = null;
            lkLoaiKhachHang.ItemIndex = 0;
           
        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            Clear();
        }


        private void gvDSKH_Click(object sender, EventArgs e)
        {
            var row = gvDSKH.GetDataRow(gvDSKH.FocusedRowHandle);
            if (row == null) { Clear(); return; }
            txtMaKH.Text = row["CustomerID"].ToString();       
            txtHoTen.Text = row["FullName"].ToString();
            txtCMND.Text = row["IdentityCard"].ToString();          
            txtDiaChi.Text = row["Address"].ToString();          
            txtEmail.Text = row["Email"].ToString();
            txtSDT.Text = row["Phone"].ToString();
            lkLoaiKhachHang.EditValue = row["CustomerTypeID"].ToString();
            if (File.Exists(appPath + row["Image"].ToString()))
            {
                pictureEdit1.Image = Image.FromFile(appPath + row["Image"].ToString());
            }
            else
            {
                pictureEdit1.Image = null;
            }
           
        }

        private void gvDSKH_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            gvDSKH_Click(null, null);
        }

        private bool CheckErorr()
        {
            if (txtHoTen.Text == "")
            {
                MessageBox.Show("Vui lòng nhập họ tên khách hàng!", "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                txtHoTen.Focus();
                return false;
            }
            if (txtCMND.Text == "" || txtCMND.Text == "___ ___ ___")
            {
                MessageBox.Show("Vui lòng nhập số chứng minh thư!", "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                txtCMND.SelectionStart = 0;
                txtCMND.Focus();
                return false;
            }
            if (txtSDT.Text == "" || txtSDT.Text == "____ ___ ___")
            {
                MessageBox.Show("Vui lòng nhập số điện thoại!", "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                txtSDT.SelectionStart = 0;
                txtSDT.Focus();
                return false;
            }
            return true;
        }
        private void btnSave_Click_1(object sender, EventArgs e)
        {
            if (CheckErorr() == false) return;
            var ds = _helper.ExcuteStore("sp_Customer_Save", new
            {
                CustomerID = txtMaKH.Text,
                IdentityCard = txtCMND.Text,
                FullName = txtHoTen.Text,
                Email = txtEmail.Text,
                Phone = txtSDT.Text,
                Address = txtDiaChi.Text,
                CustomerTypeID= lkLoaiKhachHang.EditValue,
                Image = pictureEdit1.Name,
              
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

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            gvDSKH.ActiveFilterString = "[CustomerID] like '%" + txtSearch.Text.Trim() + "%' OR [FullName] like '%" + txtSearch.Text.Trim() + "%' OR [Phone] like '%" + txtSearch.Text.Trim() + "%'";
        }

        private void pictureEdit1_Click(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            // image filters  
            open.Filter = "Image Files(*.jpg; *.jpeg; *.gif; *.bmp)|*.jpg; *.jpeg; *.gif; *.bmp";
            if (Directory.Exists(appPath) == false)                                              // <---
            {                                                                                    // <---
                Directory.CreateDirectory(appPath);                                              // <---
            }
            if (open.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    string iName = open.SafeFileName;   // <---
                    string filepath = open.FileName;
                    if (File.Exists(appPath + iName))
                    {
                        pictureEdit1.Name = iName;
                        pictureEdit1.Image = Image.FromFile(open.FileName);

                        return;
                    }
                    File.Copy(filepath, appPath + iName); // <---
                    pictureEdit1.Name = iName;
                    pictureEdit1.Image = Image.FromFile(open.FileName);

                }
                catch (Exception exp)
                {
                    MessageBox.Show("Unable to open file " + exp.Message);
                }
            }
            else
            {
                open.Dispose();
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            var ds = _helper.ExcuteStore("sp_Customer_Delete", new
            {
                CustomerID = txtMaKH.Text,       
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

        private void repositoryItemButtonEdit2_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn có muốn xóa khách hàng này không?", "Cảnh báo", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.No) return;
            var ds = _helper.ExcuteStore("sp_Customer_Delete", new
            {
                CustomerID = txtMaKH.Text,
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

        private void btnChangePicture_Click(object sender, EventArgs e)
        {
            pictureEdit1_Click(null, null);
        }

        private void txtCMND_Click(object sender, EventArgs e)
        {
            if (txtCMND.Text == "___ ___ ___")
            {
                txtCMND.SelectionStart = 0;
                txtCMND.Focus();
            }
        }
        private void txtSDT_Click(object sender, EventArgs e)
        {
            if (txtSDT.Text == "____ ___ ___")
            {
                txtSDT.SelectionStart = 0;
                txtSDT.Focus();
            }
        }

       

       
    }
}
