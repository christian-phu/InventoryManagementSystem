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
    public partial class frmSanPham : Form
    {
        WinQLSanPhamEntities db = new WinQLSanPhamEntities();
        frmDangNhap dn = new frmDangNhap();
        private byte[] imgAnh;
        public frmSanPham()
        {
            InitializeComponent();
        }

        private void Them()
        {
            txtMaSanPham.Enabled = false;
            btnThem.Enabled = true;
            btnLuu.Enabled = false;
            btnXoa.Enabled = false;
            var obj = db.SanPhams.OrderByDescending(x => x.MaSanPham).FirstOrDefault();
            if (obj != null)
            {
                txtMaSanPham.Text = (obj.MaSanPham + 1).ToString();
            }
            else
            {
                txtMaSanPham.Text = "1";
            }
        }

        private void Sua()
        {
            txtMaSanPham.Enabled = false;
            btnThem.Enabled = false;
            btnLuu.Enabled = true;
            btnXoa.Enabled = true;
        }

        private void LamMoi()
        {
            Them();
            LoadDuLieu();
            txtTenSanPham.Text = "";
            txtGia.Text = "";
            txtGhiChu.Text = "";
            txtHangSanXuat.Text = "";
            pbAnh.Image = null;
            imgAnh = null;
            cbbPhanLoai.SelectedIndex = 0;

            txtTiemKiem.Text = "Nhập tên vật tư";
        }

        private void LoadDuLieu()
        {
            try
            {
                BindingSource bs = new BindingSource();
                bs.DataSource = db.SanPhams.ToList();
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

                    var obj = db.SanPhams.FirstOrDefault(x => x.MaSanPham == ma);

                    txtMaSanPham.Text = ma.ToString();
                    imgAnh = obj.HinhAnh;
                    Image myImage = ByteArrayToImage(imgAnh);
                    pbAnh.Image = myImage;
                    txtTenSanPham.Text = obj.TenSanPham;
                    txtGia.Text = obj.DonGia.Value.ToString();
                    txtGhiChu.Text = obj.GhiChu;
                    cbbPhanLoai.Text = obj.PhanLoai;
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
                var ma = int.Parse(txtMaSanPham.Text);

                var obj = db.SanPhams.FirstOrDefault(x => x.MaSanPham == ma);
                db.SanPhams.Attach(obj);
                db.Entry(obj).State = System.Data.Entity.EntityState.Deleted;
                db.SanPhams.Remove(obj);
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
                    var ma = int.Parse(txtMaSanPham.Text);
                    var gia = int.Parse(txtGia.Text);
                    var obj = db.SanPhams.FirstOrDefault(x => x.MaSanPham == ma);

                    var kiemTra = db.SanPhams.FirstOrDefault(x => x.TenSanPham == txtTenSanPham.Text);
                    if (kiemTra != null)
                    {
                        if (kiemTra.MaSanPham != obj.MaSanPham)
                        {
                            MessageBox.Show("Tên vật tư đã tồn tại!");
                            return;
                        }
                    }

                    if (txtTenSanPham.Text == "" || imgAnh.Length == 0 || txtHangSanXuat.Text == "")
                    {
                        MessageBox.Show("Nhập đầy đủ thông tin!");
                        return;
                    }

                    obj.TenSanPham = txtTenSanPham.Text;
                    obj.PhanLoai = cbbPhanLoai.Text;
                    obj.HinhAnh = imgAnh;
                    obj.GhiChu = txtGhiChu.Text;
                    obj.DonGia = gia;
                    obj.HangSanXuat = txtHangSanXuat.Text;

                    db.SanPhams.Attach(obj);
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
                    var kiemTra = db.SanPhams.FirstOrDefault(x => x.TenSanPham == txtTenSanPham.Text);
                    if (kiemTra != null)
                    {
                        MessageBox.Show("Tên vật tư đã tồn tại!");
                        return;
                    }

                    if (txtTenSanPham.Text == "" || imgAnh.Length == 0 || txtHangSanXuat.Text == "")
                    {
                        MessageBox.Show("Nhập đầy đủ thông tin!");
                        return;
                    }

                    var obj = new SanPham
                    {
                        TenSanPham = txtTenSanPham.Text,
                        PhanLoai = cbbPhanLoai.Text,
                        HinhAnh = imgAnh,
                        GhiChu = txtGhiChu.Text,
                        DonGia = gia,
                        HangSanXuat = txtHangSanXuat.Text,
                        SoLuong = 0
                    };


                    db.SanPhams.Add(obj);
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

                bs.DataSource = db.SanPhams.Where(x => x.TenSanPham.Contains(key)).ToList();
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
