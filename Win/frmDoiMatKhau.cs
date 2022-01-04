using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Win.Data;

namespace Win
{
    public partial class frmDoiMatKhau : Form
    {
        WinQLSanPhamEntities db = new WinQLSanPhamEntities();
        public frmDoiMatKhau()
        {
            InitializeComponent();
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
        }

        private void frmDoiMatKhau_Load(object sender, EventArgs e)
        {

        }

        private void btThayDoi_Click(object sender, EventArgs e)
        {
            frmDangNhap DN = new frmDangNhap();
            if (txtMKCu.Text == "" || txtMKMoi.Text == "" || txtMKXacNhan.Text == "")
            {
                MessageBox.Show("Mật khẩu không được để trống!");
            }
            else
            {
                int manhanvien = DN.MaNhanVien();
                var takhoan = db.NhanViens.Where(x => x.MaNhanVien == manhanvien && x.MatKhau == txtMKCu.Text).FirstOrDefault();
                if (takhoan != null)
                {
                    if (txtMKMoi.Text == txtMKXacNhan.Text)
                    {
                        try
                        {
                            takhoan.MatKhau = txtMKMoi.Text;
                            db.NhanViens.Attach(takhoan);
                            db.Entry(takhoan).State = System.Data.Entity.EntityState.Modified;
                            db.SaveChanges();
                            Close();
                            MessageBox.Show("Thay đổi mật khẩu thành công!");
                        }
                        catch
                        {
                            MessageBox.Show("Thao tác không thành công!");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Xác nhận mật khẩu không chính xác!");
                    }
                }
                else
                {
                    MessageBox.Show("Mật khẩu không chính xác!");
                }
            }
        }
    }
}
