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
        public frmMain1()
        {
            InitializeComponent();
        }

        private void btnQLNV_ItemClick(object sender, ItemClickEventArgs e)
        {
            ucQuanLyNhanVien1.Visible = true;
            ucQuanLyNhanVien1.Process();
        }

        private void frmMain1_Load(object sender, EventArgs e)
        {
            ucQuanLyNhanVien1.Visible = false;
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

    }
}