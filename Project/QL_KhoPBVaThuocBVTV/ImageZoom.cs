using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QL_KhoPBVaThuocBVTV
{
    public class ImageZoom
    {
        private readonly Form zoomForm;
        private readonly PictureBox zoomBox;

        public ImageZoom()
        {
            zoomForm = new Form
            {
                FormBorderStyle = FormBorderStyle.None,
                BackColor = Color.White,
                Size = new Size(100, 100),
                StartPosition = FormStartPosition.Manual,
                TopMost = true,
                ShowInTaskbar = false
            };

            zoomBox = new PictureBox
            {
                Dock = DockStyle.Fill,
                SizeMode = PictureBoxSizeMode.Zoom,
                BorderStyle = BorderStyle.FixedSingle
            };

            zoomForm.Controls.Add(zoomBox);
        }

        public void Attach(Control control)
        {
            control.MouseMove += (s, e) =>
            {
                Image img = GetImageFromControl(control, e.Location);
                if (img != null)
                {
                    zoomBox.Image = img;
                    Point screenPoint = control.PointToScreen(new Point(e.X + 20, e.Y + 20));
                    zoomForm.Location = screenPoint;

                    if (!zoomForm.Visible)
                        zoomForm.Show();
                }
                else
                {
                    zoomForm.Hide();
                }
            };

            control.MouseLeave += (s, e) =>
            {
                zoomForm.Hide();
            };
        }

        private Image GetImageFromControl(Control control, Point localPoint)
        {
            switch (control)
            {
                case PictureBox pb:
                    return pb.Image;

                case Button btn:
                    return btn.Image;

                case Label lbl:
                    return lbl.Image;

                case DataGridView dgv:
                    var hit = dgv.HitTest(localPoint.X, localPoint.Y);
                    if (hit.RowIndex >= 0 && hit.ColumnIndex >= 0)
                    {
                        var cell = dgv.Rows[hit.RowIndex].Cells[hit.ColumnIndex];
                        if (cell is DataGridViewImageCell imgCell && cell.Value is Image img)
                        {
                            return img;
                        }
                    }
                    break;
            }

            return null;
        }
    }
}
