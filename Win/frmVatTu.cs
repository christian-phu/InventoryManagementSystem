using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Win.Data;

namespace Win
{
    public partial class frmVatTu : Form
    {
        WinQLVatTuEntities db = new WinQLVatTuEntities();
        frmDangNhap dn = new frmDangNhap();
        private byte[] imgAnh;
        public frmVatTu()
        {
            InitializeComponent();
        }

        private void Them()
        {
            txtMaVatTu.Enabled = false;
            btnThem.Enabled = true;
            btnLuu.Enabled = false;
            btnXoa.Enabled = false;
            var obj = db.VatTus.OrderByDescending(x => x.MaVatTu).FirstOrDefault();
            if (obj != null)
            {
                txtMaVatTu.Text = (obj.MaVatTu + 1).ToString();
            }
            else
            {
                txtMaVatTu.Text = "1";
            }
        }

        private void Sua()
        {
            txtMaVatTu.Enabled = false;
            btnThem.Enabled = false;
            btnLuu.Enabled = true;
            btnXoa.Enabled = true;
        }

        private void LamMoi()
        {
            Them();
            LoadDuLieu();
            txtTenVatTu.Text = "";
            txtGia.Text = "";
            txtGhiChu.Text = "";
            txtHangSanXuat.Text = "";
            pbAnh.Image = null;
            imgAnh = null;
            cbbDonVi.SelectedIndex = 0;

            txtTiemKiem.Text = "Nhập tên vật tư";
        }

        private void LoadDuLieu()
        {
            try
            {
                BindingSource bs = new BindingSource();
                bs.DataSource = db.VatTus.ToList();
                bdnDieuKhien.BindingSource = bs;
                dgvDuLieu.AutoGenerateColumns = false;
                dgvDuLieu.DataSource = bs;
            }
            catch
            {
                MessageBox.Show("Dữ liệu đầu vào không đúng!");
            }
        }

        private void frmTaiKhoan_Load(object sender, EventArgs e)
        {
            LamMoi();
            DataGridViewColumn columnImg = dgvDuLieu.Columns["HinhAnh"];
            columnImg.Width = 80;
        }

        private void dgvDuLieu_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                int HangChon = e.RowIndex;
                if (HangChon != -1)
                {
                    var ma = int.Parse(dgvDuLieu[0, HangChon].Value.ToString());

                    var obj = db.VatTus.FirstOrDefault(x => x.MaVatTu == ma);

                    txtMaVatTu.Text = ma.ToString();
                    imgAnh = obj.HinhAnh;
                    Image myImage = ByteArrayToImage(imgAnh);
                    pbAnh.Image = myImage;
                    txtTenVatTu.Text = obj.TenVatTu;
                    txtGia.Text = obj.DonGia.Value.ToString();
                    txtGhiChu.Text = obj.GhiChu;
                    cbbDonVi.Text = obj.DonVi;
                    txtHangSanXuat.Text = obj.HangSanXuat;

                    Sua();
                }
            }
            catch
            {
                MessageBox.Show("Dữ liệu đầu vào không đúng!");
            }
        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            LamMoi();
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            try
            {
                var ma = int.Parse(txtMaVatTu.Text);

                var obj = db.VatTus.FirstOrDefault(x => x.MaVatTu == ma);
                db.VatTus.Attach(obj);
                db.Entry(obj).State = System.Data.Entity.EntityState.Deleted;
                db.VatTus.Remove(obj);
                db.SaveChanges();

                LamMoi();
                MessageBox.Show("Xóa thành công!");
            }
            catch
            {
                MessageBox.Show("Xóa không thành công!");
            }
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtGia.Text == "")
                {
                    MessageBox.Show("Chưa nhập giá!");
                }
                else
                {
                    var ma = int.Parse(txtMaVatTu.Text);
                    var gia = int.Parse(txtGia.Text);
                    var obj = db.VatTus.FirstOrDefault(x => x.MaVatTu == ma);

                    var kiemTra = db.VatTus.FirstOrDefault(x => x.TenVatTu == txtTenVatTu.Text);
                    if (kiemTra != null)
                    {
                        if (kiemTra.MaVatTu != obj.MaVatTu)
                        {
                            MessageBox.Show("Tên vật tư đã tồn tại!");
                            return;
                        }
                    }

                    if (txtTenVatTu.Text == "" || imgAnh.Length == 0 || txtHangSanXuat.Text == "")
                    {
                        MessageBox.Show("Nhập đầy đủ thông tin!");
                        return;
                    }

                    obj.TenVatTu = txtTenVatTu.Text;
                    obj.DonVi = cbbDonVi.Text;
                    obj.HinhAnh = imgAnh;
                    obj.GhiChu = txtGhiChu.Text;
                    obj.DonGia = gia;
                    obj.HangSanXuat = txtHangSanXuat.Text;

                    db.VatTus.Attach(obj);
                    db.Entry(obj).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();

                    LamMoi();
                    MessageBox.Show("Sửa thành công!");
                }
            }
            catch
            {
                MessageBox.Show("Sửa không thành công!");
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtGia.Text == "")
                {
                    MessageBox.Show("Chưa nhập giá!");
                }
                else
                {
                    var gia = int.Parse(txtGia.Text);
                    var kiemTra = db.VatTus.FirstOrDefault(x => x.TenVatTu == txtTenVatTu.Text);
                    if (kiemTra != null)
                    {
                        MessageBox.Show("Tên vật tư đã tồn tại!");
                        return;
                    }

                    if (txtTenVatTu.Text == "" || imgAnh.Length == 0 || txtHangSanXuat.Text == "")
                    {
                        MessageBox.Show("Nhập đầy đủ thông tin!");
                        return;
                    }

                    var obj = new VatTu
                    {
                        TenVatTu = txtTenVatTu.Text,
                        DonVi = cbbDonVi.Text,
                        HinhAnh = imgAnh,
                        GhiChu = txtGhiChu.Text,
                        DonGia = gia,
                        HangSanXuat = txtHangSanXuat.Text,
                        SoLuong = 0
                    };


                    db.VatTus.Add(obj);
                    db.SaveChanges();

                    LamMoi();
                    MessageBox.Show("Thêm thành công!");
                }
            }
            catch
            {
                MessageBox.Show("Thêm không thành công!");
            }
        }

        private void btnTaiAnh_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.ShowDialog();
                openFileDialog.Filter = "JPG files (*.jpg)|*jpg|All files (*.*)|*.*";
                openFileDialog.FilterIndex = 1;
                string file = openFileDialog.FileName;
                if (string.IsNullOrEmpty(file))
                {
                    return;
                }
                Image myImage = Image.FromFile(file).GetThumbnailImage(100, 100, null, IntPtr.Zero);
                pbAnh.Image = myImage;

                imgAnh = ImageToByteArray(myImage);

                openFileDialog.Dispose();
            }
            catch
            {
                MessageBox.Show("File bạn tải lên không phải là file hình ảnh");
            }
        }

        public Image ByteArrayToImage(byte[] byteArrayIn)
        {
            MemoryStream ms = new MemoryStream(byteArrayIn);
            Image returnImage = Image.FromStream(ms);
            return returnImage;
        }

        public byte[] ImageToByteArray(Image imageIn)
        {
            using (var ms = new MemoryStream())
            {
                imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                return ms.ToArray();
            }
        }

        private void txtCMTND_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            try
            {
                BindingSource bs = new BindingSource();

                var key = txtTiemKiem.Text;
                if (key == "Nhập tên vật tư")
                {
                    key = "";
                }

                bs.DataSource = db.VatTus.Where(x => x.TenVatTu.Contains(key)).ToList();
                bdnDieuKhien.BindingSource = bs;
                dgvDuLieu.AutoGenerateColumns = false;
                dgvDuLieu.DataSource = bs;

                DataGridViewColumn columnImg = dgvDuLieu.Columns["HinhAnh"];
                columnImg.Width = 80;
            }
            catch
            {
                MessageBox.Show("Tìm kiếm không đúng");
            }
        }

        private void txtTiemKiem_Enter(object sender, EventArgs e)
        {
            if (txtTiemKiem.Text == "Nhập tên vật tư")
            {
                txtTiemKiem.Text = "";
            }
        }

        private void txtTiemKiem_Leave(object sender, EventArgs e)
        {
            if (txtTiemKiem.Text == "")
            {
                txtTiemKiem.Text = "Nhập tên vật tư";
            }
        }
    }
}
