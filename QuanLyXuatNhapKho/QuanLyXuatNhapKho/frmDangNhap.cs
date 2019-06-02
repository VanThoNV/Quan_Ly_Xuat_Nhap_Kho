using System;
using System.Windows.Forms;
using QuanLyXuatNhapKho.DAL;
using QuanLyXuatNhapKho.DTO;
using DevExpress.XtraEditors;
namespace QuanLyXuatNhapKho
{
    public partial class frmDangNhap : XtraForm
    {
        UsersDAL _userDAL = new UsersDAL();       
        public Users CurrentUser { get; set; }
        public frmDangNhap()
        {
            InitializeComponent();
            
        }

        private void btnDangNhap_Click(object sender, EventArgs e)
        {
            DangNhap();
        }

        private void btnDong_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void txtPassword_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                DangNhap();
            }
        }

        private void DangNhap()
        {
            lblError.Visible = false;
            string userName = txtUserName.Text.Trim();
            string password = txtPassword.Text;
            Users nguoiDung = _userDAL.DangNhap(userName, password);
            if (nguoiDung != null)
            {
                CurrentUser = nguoiDung;
                this.DialogResult = DialogResult.OK;
            }
            else
            {
                lblError.Visible = true;
            }
        }


    }
}
