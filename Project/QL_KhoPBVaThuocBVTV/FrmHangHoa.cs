using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QL_KhoPBVaThuocBVTV
{
    public partial class FrmHangHoa : Form
    {
        private string maND;
        public FrmHangHoa(string maND)
        {
            InitializeComponent();
            this.maND = maND;
        }

        private void FrmHangHoa_Load(object sender, EventArgs e)
        {
            dtpNgaySanXuat.MaxDate = DateTime.Today;
        }
    }
}
