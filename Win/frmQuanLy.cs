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
using System.Diagnostics;

namespace Win
{
    public partial class frmQuanLy : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        private System.Windows.Forms.Timer tm;
        private bool isLock = false;
        public frmQuanLy()
        {
            InitializeComponent();
        }



        private void frmQuanLy_Load(object sender, EventArgs e)
        {
            MdiClient ctlMDI;
            foreach (Control ctl in this.Controls)
            {
                try
                {
                    ctlMDI = (MdiClient)ctl;
                    ctlMDI.BackColor = this.BackColor;
                }
                catch
                {

                }
            }
            tm = new System.Windows.Forms.Timer();
            tm.Interval = 1 * 100; // 0,1 seconds
            tm.Tick += new EventHandler(LoadDieuKhien);
            tm.Start();
        }

        public void DongCacForm()
        {
            foreach (Form frm in this.MdiChildren)
            {
                frm.Close();
            }
        }

        public void LoadDieuKhien(object sender, EventArgs e)
        {
            frmDangNhap DN = new frmDangNhap();
            if (DN.DangNhap() == true)
            {
                btnDangNhap.Enabled = false;
                btnDoiMatKhau.Enabled = true;
                btnDangXuat.Enabled = true;
                btnThongTinCaNhan.Enabled = true;
                btnThongTinCaNhan.Caption = "Xin chào: " + DN.TaiKhoan().Trim();
                btnNhanVien.Enabled = true;
                btnNhapKho.Enabled = true;
                btnXuatKho.Enabled = true;
                //btnThuHoi.Enabled = true;
                btnSanPham.Enabled = true;
                btnBCNhapKho.Enabled = true;
                btnBCTonKho.Enabled = true;
                btnBCXuaKho.Enabled = true;
                //btnBCThuHoi.Enabled = true;
                //btnBaoHanh.Enabled = true;
                btnThanhToan.Enabled = true;
                //btnCongNoTo.Enabled = true;


                if (DN.Quyen() == "Nhân viên")
                {
                    btnNhanVien.Enabled = false;
                    btnNhapKho.Enabled = false;
                    btnSanPham.Enabled = false;
                    btnBCTonKho.Enabled = false;
                    btnBCNhapKho.Enabled = false;
                    //btnBaoHanh.Enabled = false;
                    btnThanhToan.Enabled = false;
                    //btnYeuCau.Enabled = true;
                }
            }
            else
            {
                btnDangNhap.Enabled = true;
                btnDoiMatKhau.Enabled = false;
                btnDangXuat.Enabled = false;
                btnThongTinCaNhan.Enabled = false;
                btnThongTinCaNhan.Caption = "Xin chào";
                btnNhanVien.Enabled = false;
                btnNhapKho.Enabled = false;
                btnXuatKho.Enabled = false;
                //btnThuHoi.Enabled = false;
                btnSanPham.Enabled = false;
                //btnYeuCau.Enabled = false;
                btnBCNhapKho.Enabled = false;
                btnBCTonKho.Enabled = false;
                btnBCXuaKho.Enabled = false;
                //btnBCThuHoi.Enabled = false;
                //btnBaoHanh.Enabled = false;
                btnThanhToan.Enabled = false;
                //btnCongNoTo.Enabled = false;
            }

            if (isLock == false)
            {
                frmDangNhap form = new frmDangNhap();
                form.MdiParent = this;
                form.Show();
                isLock = true;
            }
        }

        private void btnThongTin_ItemClick(object sender, ItemClickEventArgs e)
        {
            frmThongTin form = new frmThongTin();
            form.MdiParent = this;
            form.Show();
        }

        private void btnHuongDan_ItemClick(object sender, ItemClickEventArgs e)
        {
            string file = @"\huongdansudung.doc";
            string fileLoad = System.IO.Directory.GetCurrentDirectory();
            file = fileLoad + file;
            Process.Start("Winword.exe", file);
        }

        private void btnDangNhap_ItemClick(object sender, ItemClickEventArgs e)
        {
            DongCacForm();
            frmDangNhap form = new frmDangNhap();
            form.MdiParent = this;
            form.Show();
        }

        private void btnDangXuat_ItemClick(object sender, ItemClickEventArgs e)
        {
            DongCacForm();
            frmDangNhap form = new frmDangNhap();
            form.DangXuat();
        }

        private void btnDoiMatKhau_ItemClick(object sender, ItemClickEventArgs e)
        {
            DongCacForm();
            frmDoiMatKhau form = new frmDoiMatKhau();
            form.MdiParent = this;
            form.Show();
        }

        private void btnTaiKhoan_ItemClick(object sender, ItemClickEventArgs e)
        {
            DongCacForm();
            frmTaiKhoan form = new frmTaiKhoan();
            form.MdiParent = this;
            form.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            form.Dock = DockStyle.Fill;
            form.Show();
        }

        private void btnThongTinCaNhan_ItemClick(object sender, ItemClickEventArgs e)
        {
            frmThongTinCaNhan form = new frmThongTinCaNhan();
            form.MdiParent = this;
            form.Show();
        }

        private void btnSanPham_ItemClick(object sender, ItemClickEventArgs e)
        {
            DongCacForm();
            frmSanPham form = new frmSanPham();
            form.MdiParent = this;
            form.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            form.Dock = DockStyle.Fill;
            form.Show();
        }

        private void btnKho_ItemClick(object sender, ItemClickEventArgs e)
        {

        }

        private void btnNhapKho_ItemClick(object sender, ItemClickEventArgs e)
        {
            DongCacForm();
            frmNhapKho form = new frmNhapKho();
            form.MdiParent = this;
            form.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            form.Dock = DockStyle.Fill;
            form.Show();
        }

        private void btnXuatKho_ItemClick(object sender, ItemClickEventArgs e)
        {
            DongCacForm();
            frmXuatKho form = new frmXuatKho();
            form.MdiParent = this;
            form.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            form.Dock = DockStyle.Fill;
            form.Show();
        }

        private void btnThuHoi_ItemClick(object sender, ItemClickEventArgs e)
        {
            //DongCacForm();
            //frmThuHoi form = new frmThuHoi();
            //form.MdiParent = this;
            //form.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            //form.Dock = DockStyle.Fill;
            //form.Show();
        }

        private void frmQuanLy_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("Bạn có muốn thoát khỏi chương trình?", "Thoát", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            {
                Dispose(true);
                Application.Exit();
            }
            else
            {
                e.Cancel = true;
            }
        }

        private void btnBCTonKho_ItemClick(object sender, ItemClickEventArgs e)
        {
            DongCacForm();
            frmTonKho form = new frmTonKho();
            form.MdiParent = this;
            form.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            form.Dock = DockStyle.Fill;
            form.Show();
        }

        private void btnBCNhapKho_ItemClick(object sender, ItemClickEventArgs e)
        {
            DongCacForm();
            frmBCNhapKho form = new frmBCNhapKho();
            form.MdiParent = this;
            form.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            form.Dock = DockStyle.Fill;
            form.Show();
        }

        private void btnBCXuaKho_ItemClick(object sender, ItemClickEventArgs e)
        {
            DongCacForm();
            frmBCXuatKho form = new frmBCXuatKho();
            form.MdiParent = this;
            form.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            form.Dock = DockStyle.Fill;
            form.Show();
        }

        private void btnBCThuHoi_ItemClick(object sender, ItemClickEventArgs e)
        {
            //DongCacForm();
            //frmBCThuHoi form = new frmBCThuHoi();
            //form.MdiParent = this;
            //form.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            //form.Dock = DockStyle.Fill;
            //form.Show();
        }

        private void btnYeuCau_ItemClick(object sender, ItemClickEventArgs e)
        {
            //DongCacForm();
            //frnYeuCau form = new frnYeuCau();
            //form.MdiParent = this;
            //form.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            //form.Dock = DockStyle.Fill;
            //form.Show();
        }

        private void btnBaoHanh_ItemClick(object sender, ItemClickEventArgs e)
        {
            //DongCacForm();
            //frmGuiBaoHanh form = new frmGuiBaoHanh();
            //form.MdiParent = this;
            //form.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            //form.Dock = DockStyle.Fill;
            //form.Show();
        }

        private void btnThanhToan_ItemClick(object sender, ItemClickEventArgs e)
        {
            DongCacForm();
            frmThanhToan form = new frmThanhToan();
            form.MdiParent = this;
            form.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            form.Dock = DockStyle.Fill;
            form.Show();
        }

        private void btnCongNoTo_ItemClick(object sender, ItemClickEventArgs e)
        {
            //DongCacForm();
            //frmCongNo form = new frmCongNo();
            //form.MdiParent = this;
            //form.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            //form.Dock = DockStyle.Fill;
            //form.Show();
        }

        private void ribbon_Click(object sender, EventArgs e)
        {

        }

        
        private void barButtonItem3_ItemClick(object sender, ItemClickEventArgs e)
        {
            System.Diagnostics.Process.Start("https://docs.google.com/spreadsheets/d/17nc1NGTVbhrt7ZGm4HcZsCwJGVnn0aHyH0dRyf6RGCk/edit#gid=294394684");
        }

        private void barButtonItem1_ItemClick(object sender, ItemClickEventArgs e)
        {
            System.Diagnostics.Process.Start("https://docs.google.com/spreadsheets/d/17nc1NGTVbhrt7ZGm4HcZsCwJGVnn0aHyH0dRyf6RGCk/edit#gid=294394684");
        }

        private void barButtonItem4_ItemClick(object sender, ItemClickEventArgs e)
        {
            System.Diagnostics.Process.Start("https://form.jotform.com/jrymatix/gomchambautruc");
        }
    }
}