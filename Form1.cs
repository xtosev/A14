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
    public partial class Form1 : Form
    {
        string connstr = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\A14.mdf;Integrated Security=True";
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            OsveziGrid();
        }
        private void OsveziGrid()
        {
            SqlConnection conn = new SqlConnection(connstr);
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = "SELECT " +
                "l.LekID AS 'Šifra leka', " +
                "p.ProizvodjacID AS 'Šifra proizvođača', " +
                "l.NazivLeka AS 'Naziv leka', " +
                "l.NezasticenoIme AS 'Nezaštićeno ime', " +
                "p.Naziv AS 'Proizvođač' " +
                "FROM Proizvodjac AS p, Lek AS l " +
                "WHERE l.ProizvodjacID=p.ProizvodjacID " +
                "ORDER BY l.NazivLeka";
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            try
            {
                da.Fill(dt);
                dataGridView1.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Greska: " + ex.Message);
            }
            finally
            {
                da.Dispose();
                cmd.Dispose();
            }
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if(dataGridView1.SelectedRows.Count > 0)
            {
                textBoxNazivLeka.Text =
                    dataGridView1.SelectedRows[0].
                    Cells[2].Value.ToString();
                textBoxProizvodjac.Text =
                    dataGridView1.SelectedRows[0].
                    Cells[4].Value.ToString();
            }
            else
            {
                textBoxNazivLeka.Text = "";
                textBoxProizvodjac.Text = "";
            }
        }

        private void toolStripButtonBrisi_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0) // nema selekcije
            {
                MessageBox.Show("Nema selekcije!");
                return;
            }
            if (MessageBox.Show("Da li ste sigurni",
                "Brisanje leka iz evidencije",
                MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                SqlConnection conn = new SqlConnection(connstr);
                SqlCommand komanda = new SqlCommand();
                komanda.CommandText =
                    "DELETE FROM Lek " +
                    "WHERE LekID=@paramID";
                int ID = int.Parse(dataGridView1.SelectedRows[0].Cells[0].Value.ToString());
                komanda.Parameters.AddWithValue("@paramID", ID);
                komanda.Connection = conn;
                try
                {
                    conn.Open();
                    komanda.ExecuteNonQuery();
                    komanda.Dispose();
                    OsveziGrid();
                    textBoxNazivLeka.Text = "";
                    textBoxProizvodjac.Text = "";

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    conn.Close();
                    MessageBox.Show("Uspesno ste izbrisali lek");
                }
            }
        }

        private void toolStripButtonAnaliza_Click(object sender, EventArgs e)
        {
            FormStatistika fs= new FormStatistika();
            fs.ShowDialog();
        }

        private void toolStripButtonOAplikaciji_Click(object sender, EventArgs e)
        {
            OAplikaciji oa = new OAplikaciji();
            oa.ShowDialog();
        }

        private void toolStripButtonIzlaz_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
