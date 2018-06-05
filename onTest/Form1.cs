using System;
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
            var parser = new MRZParser();
            var customer = parser.Parse(dfsMRZ.Text);
            dfsIssuingCountry.Text = customer.IssuingCountryIso;
            dfsFirstName.Text = customer.FirstName;
            dfsLastName.Text = customer.LastName;
            dfsDocumentNumber.Text = customer.DocumentNumber;
            dfsNationality.Text = customer.NationalityIso;
            dfdDOB.Text = customer.DateOfBirth.ToString("dd/MM/yyyy");
            dfdExpireDate.Text = customer.ExpireDate.ToString("dd/MM/yyyy");
            dfsGender.Text = customer.Gender;
        }

        private void btnMake_Click(object sender, EventArgs e)
        {
            var parser = new MRZParser();
            var customer = new Customer
            {
                IssuingCountryIso = dfsIssuingCountry.Text,
                FirstName = dfsFirstName.Text,
                LastName = dfsLastName.Text,
                DocumentNumber = dfsDocumentNumber.Text,
                NationalityIso = dfsNationality.Text,
                DateOfBirth = DateTime.Parse(dfdDOB.Text),
                ExpireDate = DateTime.Parse(dfdExpireDate.Text),
                Gender = dfsGender.Text
            };


            dfsMRZ.Text = parser.CreatMrz(customer, false);
        }
    }
}