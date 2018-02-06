using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using onMRZ;

namespace onTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MRZParser.Mrz = "P<GBRMALIK<<MUSSARAT<ZARIN<<<<<<<<<<<<<<<<<<5119237240GBR4612078F2212119<<<<<<<<<<<<<<04";
            MessageBox.Show(MRZParser.IssuingCountryName);
        }
    }
}
