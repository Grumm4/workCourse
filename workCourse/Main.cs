using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace workCourse
{
    public partial class Main : Form
    {
        public int offset = 0;
        public decimal limit = 20;
        public int numFirPage = 1;
        public decimal numLastPage = 0;
        
        MySqlConnection conn = new MySqlConnection(Form1.connStr);
        
        public Main()
        {
            InitializeComponent();
        }
        
        //delegate void Function();
        void Page(int offset, decimal limit)
        {
            dataGridView1.Rows.Clear();
            conn.Open();
            string load = $"SELECT * FROM Main LIMIT {limit} OFFSET {offset}";
            
            MySqlCommand cmd = new MySqlCommand(load, conn);
            MySqlDataReader read = cmd.ExecuteReader();
            List<string[]> data = new List<string[]>();
            while (read.Read())
            {
                data.Add(new string[5]);

                data[data.Count - 1][0] = read[0].ToString();
                data[data.Count - 1][1] = read[1].ToString();
                data[data.Count - 1][2] = read[2].ToString();
                data[data.Count - 1][3] = read[3].ToString();
                data[data.Count - 1][4] = read[4].ToString();
            }
            foreach (string[] s in data)
                dataGridView1.Rows.Add(s);

            string count = "SELECT COUNT(*) FROM Main";
            MySqlCommand cmd2 = new MySqlCommand(count, conn);
            read.Close();

            decimal read2 = Convert.ToInt32(cmd2.ExecuteScalar());
            decimal pag = Math.Round((read2 / limit), MidpointRounding.ToPositiveInfinity);

            numLastPage = pag;
            conn.Close();

            label1.Text = $"Страница {numFirPage}/{pag}";
        }
        public void Main_Load(object sender, EventArgs e)
        {
            Page(offset, limit);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();   
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form1 form = new Form1();
            form.Show();
            Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //conn.Open();
            ////Добавление всех price в коллекцию
            //string query = "SELECT SUM(`price`*`count`) FROM `Main`";
            //MySqlCommand command = new MySqlCommand(query, conn);
            ////MySqlDataReader reader = command.ExecuteReader();
            //MessageBox.Show($"Общая цена всех товаров: {Math.Round(Convert.ToDecimal(command.ExecuteScalar()), 2)}");
            //conn.Close();
        }
        
        private void button4_Click(object sender, EventArgs e)
        {
            OrderForm of = new OrderForm();
            var res = of.ShowDialog();
            if (res == DialogResult.Cancel)
            {
                dataGridView1.Rows.Clear();
                Page(offset, limit);
            }
            //Hide();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            SellingForm sf = new SellingForm();
            var res = sf.ShowDialog();
            if (res == DialogResult.Cancel)
            {
                dataGridView1.Rows.Clear();
                Page(offset, limit);
            }

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button7_Click(object sender, EventArgs e)
        {
            numFirPage++;
            offset += Convert.ToInt32(limit);
            Page(offset, limit);
            if (numFirPage > numLastPage)
            {
                numFirPage--;
                offset -= Convert.ToInt32(limit);
                Page(offset, limit);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            numFirPage--;
            offset -= Convert.ToInt32(limit);
            if (offset == -10)
            {
                numFirPage = 1; 
                offset += Convert.ToInt32(limit);
                Page(offset, limit);
            }
            else
                Page(offset, limit);
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button8_Click(object sender, EventArgs e)
        {
            orderFind of = new orderFind();
            var res = of.ShowDialog();
            if (res == DialogResult.OK)
            {
                dataGridView1.Rows.Clear();
                Page(offset, limit);
            }
        }
    }
}
