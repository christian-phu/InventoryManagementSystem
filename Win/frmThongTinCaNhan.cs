using System;
using System.Windows.Forms;
using System.Linq;
using Win.Data;

namespace Win
{
    public partial class frmThongTinCaNhan : Form
    {
        WinQLVatTuEntities db = new WinQLVatTuEntities();
        public frmThongTinCaNhan()
        {
            InitializeComponent();
        }

        private void frmThongTinCaNhan_Load(object sender, EventArgs e)
        {
            frmDangNhap dn = new frmDangNhap();
            var ma = dn.MaNhanVien();
            var taikhoan = db.NhanViens.FirstOrDefault(x => x.MaNhanVien == ma);

            lblTaiKhoan.Text = taikhoan.TaiKhoan;
            lblHoTen.Text = taikhoan.HoTen;
            lblQuyen.Text = taikhoan.Quyen;
        }
    }
}
