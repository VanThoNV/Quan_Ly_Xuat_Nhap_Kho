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

namespace QuanLyXuatNhapKho
{
    public partial class ucKhachHang : DevExpress.XtraEditors.XtraUserControl
    {
        string appPath = Path.GetDirectoryName(Application.ExecutablePath) + @"\Images\"; 
        CustomerDAL _CustomerDAL = new CustomerDAL();
        public ucKhachHang()
        {
            InitializeComponent();
        }
        public void Process()
        {
            btnSua.Enabled = true;
            btnSave.Enabled = false;
            btnAdd.Enabled = true;
            btnXoa.Enabled = true;
            var ds = _CustomerDAL.CreateStore("sp_Customer_GetAll", null);
            gDSKhachHang.DataSource = ds.Tables[0];
            txtMaKH.Enabled = false;
            txtHoTen.Enabled = false;
            txtCMND.Enabled = false;
            txtDiaChi.Enabled = false;
            txtEmail.Enabled = false;
            txtSDT.Enabled = false;
            txtKhachHang.Enabled = false;


        }
        public void Clear()
        {
            txtMaKH.Text = "";
            txtHoTen.Text = "";
            txtCMND.Text = "";
            txtDiaChi.Text = "";
            txtSDT.Text = "";
            txtEmail.Text = "";
            txtKhachHang.Text = "";
           
        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            btnSua.Enabled = false;
            btnSave.Enabled = true;
            btnAdd.Enabled = false;
            btnXoa.Enabled = false;
            txtMaKH.Enabled = true;
            txtHoTen.Enabled = true;
            txtCMND.Enabled = true;
            txtDiaChi.Enabled = true;
            txtEmail.Enabled = true;
            txtSDT.Enabled = true;
            txtKhachHang.Enabled = true;
            Clear();


        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            btnSua.Enabled = false;
            btnSave.Enabled = true;
            btnAdd.Enabled = false;
            btnXoa.Enabled = false;
            txtMaKH.Enabled = false;
            txtHoTen.Enabled = true;
            txtCMND.Enabled = true;
            txtDiaChi.Enabled = true;
            txtEmail.Enabled = true;
            txtSDT.Enabled = true;
            txtKhachHang.Enabled = true;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {

        }

        private void gvDSKH_Click(object sender, EventArgs e)
        {
            var row = gvDSKH.GetDataRow(gvDSKH.FocusedRowHandle);
            if (row == null) return;
            txtMaKH.Text = row["CustomeriD"].ToString();
            txtMaKH.Enabled = false;
            txtHoTen.Enabled = false;
            txtCMND.Enabled = false;
            txtDiaChi.Enabled = false;
            txtEmail.Enabled = false;
            txtSDT.Enabled = false;
            txtKhachHang.Enabled = false;
            btnSua.Enabled = true;
            btnSave.Enabled = false;
            txtHoTen.Text = row["FullName"].ToString();
            txtCMND.Text = row["IdentityCard"].ToString();
            
            txtDiaChi.Text = row["Address"].ToString();
           
            txtEmail.Text = row["Email"].ToString();
            txtSDT.Text = row["Phone"].ToString();
            txtKhachHang.Text = row["CustomerTypeID"].ToString();
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

        private void btnSave_Click_1(object sender, EventArgs e)
        {
            var ds = _CustomerDAL.CreateStore("sp_Cusstomer_Save", new
            {
                CustomerID = txtMaKH.Text,
                IdentityCard = txtCMND.Text,
                FullName = txtHoTen.Text,
               
                Email = txtEmail.Text,
                Phone = txtSDT.Text,
                Address = txtDiaChi.Text,
               CustomerTypeID=txtKhachHang.Text,
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

        private void pictureEdit1_EditValueChanged(object sender, EventArgs e)
        {

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
              var ds = _CustomerDAL.CreateStore("sp_Customer_Delete", new
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

       

       
    }
}
