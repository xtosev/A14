using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace A14_1
{
    public partial class OAplikaciji : Form
    {
        public OAplikaciji()
        {
            InitializeComponent();
        }

        private void OAplikaciji_Load(object sender, EventArgs e)
        {
            richTextBox1.LoadFile("../../A14.rtf");
        }
    }
}
