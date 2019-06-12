using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraBars;
using DevExpress.Utils;
using DevExpress.UserSkins;
namespace QuanLyXuatNhapKho
{
    public partial class frmMain1 : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        ucQuanLyNhanVien ucQLNV = new ucQuanLyNhanVien();
        ucQuanLyNguoiDung ucQLND = new ucQuanLyNguoiDung();
        ucKhachHang ucQLKH = new ucKhachHang();
        ucMatHang ucQLMH = new ucMatHang();
        public frmMain1()
        {
            InitializeComponent();
            btnDangNhap.Visibility = BarItemVisibility.Never;
        }

        private void btnQLNV_ItemClick(object sender, ItemClickEventArgs e)
        {    
            ucQLNV.Process();
            ucQLNV.Dock = DockStyle.Fill;
            pnMain.Controls.Clear();
            pnMain.Controls.Add(ucQLNV);
        }

        private void frmMain1_Load(object sender, EventArgs e)
        {
            //ucQuanLyNhanVien1.Visible = false;
        }

        private void ribbon_TabIndexChanged(object sender, EventArgs e)
        {
            if (ribbon.SelectedPage == tabQuanLy)
            {
                btnQLNV_ItemClick(null, null);
            }
        }

        private void ribbon_SelectedPageChanged(object sender, EventArgs e)
        {
            if (ribbon.SelectedPage == tabQuanLy)
            {
                btnQLNV_ItemClick(null, null);
            }
        }

        private void btnPhanQuyen_ItemClick(object sender, ItemClickEventArgs e)
        {
            ucQLND.Process();
            ucQLND.Dock = DockStyle.Top;
            pnMain.Controls.Clear();
            pnMain.Controls.Add(ucQLND);
        }

        private void barButtonItem3_ItemClick(object sender, ItemClickEventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn có muốn đăng xuất không?.", "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
            if(result == DialogResult.OK)
            {
                this.Hide();
                frmDangNhap fLogin = new frmDangNhap();
                if (fLogin.ShowDialog() == DialogResult.OK)
                {
                    Program.CurrentUser = fLogin.CurrentUser;
                    this.Show();
                }
            }
        }

        private void btnDoiMatKhau_ItemClick(object sender, ItemClickEventArgs e)
        {
            using (frmDoiMatKhau frm = new frmDoiMatKhau())
            {
                //frm._userDAL = ucQLND._userDAL;
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    
                }
            }
            
        }

        private void barButtonItem1_ItemClick(object sender, ItemClickEventArgs e)
        {
            
            pnMain.Controls.Clear();
            ucQLKH.Process();
            ucQLKH.Dock = DockStyle.Fill;
            
            pnMain.Controls.Add(ucQLKH);
        }

        private void barButtonItem2_ItemClick(object sender, ItemClickEventArgs e)
        { 
            pnMain.Controls.Clear();
            ucQLMH.Process();
            ucQLMH.Dock = DockStyle.Fill;
           
            pnMain.Controls.Add(ucQLMH);
        }

        private void frmMain1_FormClosed(object sender, FormClosedEventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn có muốn thoát khỏi chương trình?", "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
            if (result == DialogResult.OK)
            {
                Application.Exit();
            }
        }

        private void btnSaoLuu_ItemClick(object sender, ItemClickEventArgs e)
        {
            using (frmBackupDatabase frm = new frmBackupDatabase())
            {
                frm.Process();
                frm.ShowDialog();
            }
        }

        private void btnPhucHoi_ItemClick(object sender, ItemClickEventArgs e)
        {
            using (frmRestoreDatabase frm = new frmRestoreDatabase())
            {
                frm.Process();
                frm.ShowDialog();
            }
        }

    }
}