using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace A14_1
{
    public partial class FormStatistika : Form
    {
        SqlConnection konekcija = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\A14.mdf;Integrated Security=True");
        public FormStatistika()
        {
            InitializeComponent();
        }

        private void FormStatistika_Load(object sender, EventArgs e)
        {
            PopuniListCheck();
        }
        private void PopuniListCheck()
        {
            SqlCommand komanda = new SqlCommand();
            komanda.Connection = konekcija;
            komanda.CommandText = "SELECT Naziv, ProizvodjacID FROM Proizvodjac";
            SqlDataAdapter adapter = new SqlDataAdapter(komanda);
            DataTable dt = new DataTable();
            try
            {
                adapter.Fill(dt);
                checkedListBox1.DataSource = dt;
                checkedListBox1.DisplayMember = "Naziv";
                checkedListBox1.ValueMember = "ProizvodjacID";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void buttonPrikazi_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            foreach (DataRowView item in checkedListBox1.CheckedItems)
            {
                SqlCommand komanda = new SqlCommand();
                komanda.Connection = konekcija;
                komanda.CommandText = "SELECT p.Naziv,COUNT(l.LekID) AS BrojLekova " +
                    "FROM Lek AS l,Proizvodjac AS p " +
                    "WHERE l.ProizvodjacID=p.ProizvodjacID " +
                    "AND p.ProizvodjacID=@param1 " +
                    "GROUP BY p.Naziv  ";
                komanda.Parameters.AddWithValue("@param1", item["ProizvodjacID"]);
                SqlDataAdapter adapter = new SqlDataAdapter(komanda);
                adapter.Fill(dt);
            }
            chart1.DataSource = dt;
            chart1.Series[0].XValueMember = "Naziv";
            chart1.Series[0].YValueMembers = "BrojLekova";
            chart1.Series[0].IsValueShownAsLabel = true;
        }

        private void buttonIzadji_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
