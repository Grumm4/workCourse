using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace workCourse
{
    public partial class tovFromProv : Form
    {
        public int offset = 0;
        public decimal limit = 100;
        public int numFirPage = 1;
        public decimal numLastPage = 0;

        internal List<string> listOfTitles = new List<string>();
        public Dictionary<string, int> orderBusket = new Dictionary<string, int>();

        MySqlConnection conn;

        public tovFromProv()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e) => addToOrder(); 
        void addToOrder()
        {
            if (numericUpDown1.Value == 0)
            {
                MessageBox.Show("Вы не вбрали количество заказываемого товара!!");
            }
            else
            {
                listOfTitles.Add(dataGridView1[1, dataGridView1.SelectedRows[0].Index].Value.ToString());
                orderBusket.Add(dataGridView1[1, dataGridView1.SelectedRows[0].Index].Value.ToString(), Convert.ToInt32(numericUpDown1.Value));
                listBox1.Items.Add($"Товар {dataGridView1[1, dataGridView1.SelectedRows[0].Index].Value} добавлен в корзину с количеством {Convert.ToInt32(numericUpDown1.Value)}");
            }

        }

        private void tovFromProv_Load(object sender, EventArgs e)
        {
            conn = new MySqlConnection(Form1.connStr);
            Page(offset, limit);
        }
        void Page(int offset, decimal limit)
        {
            dataGridView1.Rows.Clear();
            conn.Open();
            string load = $"SELECT * FROM newItems LIMIT {limit} OFFSET {offset}";

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

        private void button2_Click(object sender, EventArgs e) => Close();
        internal void FillList(out Dictionary<string, int> dict) => dict = orderBusket;

        private void button3_Click(object sender, EventArgs e)
        {
            listBox1.Items.Add($"Товар {listOfTitles[listOfTitles.Count - 1]} удалён из корзины");
            orderBusket.Remove(listOfTitles[listOfTitles.Count - 1]);
        }
    }
}
