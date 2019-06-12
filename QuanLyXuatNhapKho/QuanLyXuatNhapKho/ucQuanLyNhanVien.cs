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
using System.IO;

namespace QuanLyXuatNhapKho
{
    public partial class ucQuanLyNhanVien : XtraUserControl
    {
        //List<Employees> _emps;
        DataHelper _helper = new DataHelper();
        string appPath = Path.GetDirectoryName(Application.ExecutablePath) + @"\Images\"; 
        public ucQuanLyNhanVien()
        {
            InitializeComponent();
        }
        public void Process()
        {
            var ds = _helper.ExcuteStore("sp_Employees_GetAll", null);
            gDSNV.DataSource = ds.Tables[0];
            gvDSNV_Click(null, null);
        }

        private void gvDSNV_Click(object sender, EventArgs e)
        {
            var row = gvDSNV.GetDataRow(gvDSNV.FocusedRowHandle);
            if (row == null) { Clear();  return; }
            txtMaNV.Text = row["EmpID"].ToString();
            txtHoTen.Text = row["FullName"].ToString();
            txtCMND.Text = row["IdentityCard"].ToString();
            dateNgaySinh.EditValue = row["BirthDay"];
            dateNgayLamViec.EditValue = row["WorkDay"];
            txtDiaChi.Text = row["Address"].ToString();
            txtGhiChu.Text = row["Note"].ToString();
            txtEmail.Text = row["Email"].ToString();
            txtSDT.Text = row["Phone"].ToString();
            rdbNam.Checked = row["Sex"].ToString() == "Nam" ? true : false;
            rdbNu.Checked = row["Sex"].ToString() == "Nam" ? false : true;
            if (File.Exists(appPath + row["Image"].ToString()))
            {
                pictureEdit1.Image = Image.FromFile(appPath + row["Image"].ToString());
            }
            else
            {
                pictureEdit1.Image = null;
            }
            
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
            pictureEdit1.Image = null;
            
        }

        private bool CheckError()
        {
            if(txtHoTen.Text == "")
            {
                MessageBox.Show("Vui lòng nhập họ tên nhân viên!", "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                txtHoTen.Focus();
                return false;
            }
            if (txtCMND.Text == "" || txtCMND.Text == "___ ___ ___" )
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
            if (dateNgaySinh.EditValue != null)
            {
                if (DateTime.Now.Year - dateNgaySinh.DateTime.Year < 15)
                {
                    MessageBox.Show("Nhân viên này chưa đủ tuổi làm việc!", "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                    dateNgaySinh.Focus();
                    return false;
                }
            }
            return true;
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (CheckError() == false) return;
            var isSuccess = _helper.ExcuteStore("sp_Employees_Save", new
            {
                EmpID = txtMaNV.Text,
                IdentityCard = txtCMND.Text,
                FullName = txtHoTen.Text,
                BirthDay = dateNgaySinh.EditValue, //== null dateNgaySinh.DateTime,
                WorkDay = dateNgayLamViec.EditValue,
                Email = txtEmail.Text,
                Phone = txtSDT.Text.Replace("_",""),
                Address = txtDiaChi.Text,
                Note = txtGhiChu.Text,
                Sex = rdbNam.Checked,
                Image = pictureEdit1.Name,
                UserID = Program.CurrentUser.ID
            });
            if (isSuccess == null)
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

        private void ucQuanLyNhanVien_Load(object sender, EventArgs e)
        {
            //LoadData();
        }

        private void repositoryItemButtonEdit1_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn có muốn xóa nhân viên này không?", "Cảnh báo", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                var row = gvDSNV.GetDataRow(gvDSNV.FocusedRowHandle);
                if (row == null) return;
                var isSuccess = _helper.ExcuteStore("sp_Employees_Delete", new
                {
                    EmpID = row["EmpID"].ToString()
                });
                if (isSuccess == null)
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

        private void gvDSNV_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            //DataTable tb = new DataTable();
            //tb.Rows.Add("", "", "");
            //tb.AcceptChanges();
        }

        private void textEdit7_TextChanged(object sender, EventArgs e)
        {
           gvDSNV.ActiveFilterString = "[EmpID] like '%" + txtSearch.Text.Trim() + "%' OR [FullName] like '%" + txtSearch.Text.Trim() + "%' OR [Phone] like '%" + txtSearch.Text.Trim() + "%'";
            Task.Delay(500);
            gvDSNV.FocusedRowHandle = 0;
            gvDSNV_Click(null, null);
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

        private void btnChangePicture_Click(object sender, EventArgs e)
        {
            pictureEdit1_Click(null, null);
        }

    }
}
