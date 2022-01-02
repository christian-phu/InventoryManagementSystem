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
    public partial class frmBCNhapKho : Form
    {
        WinQLVatTuEntities db = new WinQLVatTuEntities();
        frmDangNhap dn = new frmDangNhap();
        public frmBCNhapKho()
        {
            InitializeComponent();
        }

        private void btnThongKe_Click(object sender, EventArgs e)
        {
            var ma = dn.MaNhanVien();
            dgView.AutoGenerateColumns = false;

            var tuNgay = DateTime.Parse(dtpTuNgay.Text + " 00:00:00");
            var denNgay = DateTime.Parse(dtpDenNgay.Text + " 23:59:59");

            var tu = tuNgay.ToShortDateString();
            var den = denNgay.ToShortDateString();

            dgView.DataSource = db.ChiTietNhapKhoes.Where(x => x.NhapKho.MaNhanVien == ma && x.NhapKho.NgayThang >= tuNgay && x.NhapKho.NgayThang < denNgay).Select(x => new { x.NhapKho.MaNhanVien, x.NhapKho.NhanVien.HoTen, x.VatTu.TenVatTu, SoLuong = x.SoLuong.Value, NgayThayDoi = x.NhapKho.NgayThang.Value, TuNgay = tu, DenNgay = den, x.NhapKho.GhiChu }).ToList();
        }

        private void btnIn_Click(object sender, EventArgs e)
        {
            var ma = dn.MaNhanVien();
            frmIn frm = new frmIn(ma, 2, dtpTuNgay.Text, dtpDenNgay.Text);
            frm.ShowDialog();
        }

        private void frmTonKho_Load(object sender, EventArgs e)
        {
           
        }
    }
}
