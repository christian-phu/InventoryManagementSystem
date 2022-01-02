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
    public partial class frmTonKho : Form
    {
        WinQLVatTuEntities db = new WinQLVatTuEntities();
        frmDangNhap dn = new frmDangNhap();
        public frmTonKho()
        {
            InitializeComponent();
        }

        private void btnThongKe_Click(object sender, EventArgs e)
        {
            dgView.AutoGenerateColumns = false;
            dgView.DataSource = db.VatTus.Where(x => x.SoLuong > 0).Select(x => new { x.TenVatTu, SoLuong = x.SoLuong.Value }).ToList();
        }

        private void btnIn_Click(object sender, EventArgs e)
        {
            frmIn frm = new frmIn(0, 1, "", "");
            frm.ShowDialog();
        }

        private void frmTonKho_Load(object sender, EventArgs e)
        {

        }
    }
}
