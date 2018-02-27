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
            MRZParser.Mrz = dfsMRZ.Text;
            dfsIssuingCountry.Text = MRZParser.IssuingCountryIso;
            dfsFirstName.Text = MRZParser.FirstName;
            dfsLastName.Text = MRZParser.LastName;
            dfsDocumentNumber.Text = MRZParser.DocumentNumber;
            dfsNationality.Text = MRZParser.NationalityIso;
            dfdDOB.Text = MRZParser.DateOfBirth.ToString("dd/MM/yyyy");
            dfdExpireDate.Text = MRZParser.ExpireDate.ToString("dd/MM/yyyy");
            dfsGender.Text = MRZParser.Gender;



        }

        private void btnMake_Click(object sender, EventArgs e)
        {
            MRZParser.Mrz = string.Empty;
   MRZParser.IssuingCountryIso=          dfsIssuingCountry.Text ;
 MRZParser.FirstName=            dfsFirstName.Text ;
 MRZParser.LastName=            dfsLastName.Text ;
 MRZParser.DocumentNumber=            dfsDocumentNumber.Text ;
 MRZParser.NationalityIso=            dfsNationality.Text ;
            MRZParser.DateOfBirth = DateTime.Parse(dfdDOB.Text);
 MRZParser.ExpireDate = DateTime.Parse(dfdExpireDate.Text); 
 MRZParser.Gender=            dfsGender.Text ;
        }
    }
}
