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
using Win.Data;

namespace Win
{
    public partial class frmDangNhap : DevExpress.XtraEditors.XtraForm
    {
        WinQLSanPhamEntities db = new WinQLSanPhamEntities();
        public static bool trangthai = false;
        public static string quyen = "";
        public static string taikhoan = "";
        public static int manhanvien = 0;

        public frmDangNhap()
        {
            InitializeComponent();
        }

        private void frmDangNhap_Load(object sender, EventArgs e)
        {

        }

        private void btDangNhap_Click(object sender, EventArgs e)
        {
            if (txtTenDangNhap.Text == "" || txtMatKhau.Text == "")
            {
                MessageBox.Show("Tên đăng nhập và mật khẩu không được để trống!");
            }
            else
            {
                var obj = db.NhanViens.Where(x => x.TaiKhoan == txtTenDangNhap.Text && x.MatKhau == txtMatKhau.Text).FirstOrDefault();
                if (obj != null)
                {
                    trangthai = true;
                    manhanvien = obj.MaNhanVien;
                    taikhoan = obj.HoTen;
                    quyen = obj.Quyen;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Tên đăng nhập hoặc mật khẩu không đúng!!");
                }
            }
        }

        public bool DangNhap()
        {
            return trangthai;
        }
        public void DangXuat()
        {
            trangthai = false;
            manhanvien = 0;
            quyen = "";
            taikhoan = "";
        }
        public int MaNhanVien()
        {
            return manhanvien;
        }
        public string Quyen()
        {
            return quyen.Trim();
        }
        public string TaiKhoan()
        {
            return taikhoan;
        }

        private void txtMatKhau_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}