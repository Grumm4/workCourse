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
//using ClassOrder;

namespace workCourse
{
    public partial class Main : Form
    {
        //Order a = new Order();

        public Main()
        {
            InitializeComponent();
        }
        static string connStr = "server=chuc.caseum.ru;port=33333;user=st_2_20_8;database=is_2_20_st8_KURS;password=82411770;";
        MySqlConnection conn = new MySqlConnection(connStr);
        private void Main_Load(object sender, EventArgs e)
        {
            
            conn.Open();
            string query = "SELECT * FROM Main ORDER BY id";
            MySqlCommand command = new MySqlCommand(query, conn);

            MySqlDataReader reader = command.ExecuteReader();
            List<string[]> data = new List<string[]>();
            //a.Zak();
            while (reader.Read())
            {
                data.Add(new string[5]);

                data[data.Count - 1][0] = reader[0].ToString();
                data[data.Count - 1][1] = reader[1].ToString();
                data[data.Count - 1][2] = reader[2].ToString();
                data[data.Count - 1][3] = reader[3].ToString();
                data[data.Count - 1][4] = reader[4].ToString();
            }
            reader.Close();
            conn.Close();
            foreach (string[] s in data)
                dataGridView1.Rows.Add(s);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();   
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form1 form = new Form1();
            this.Hide();
            form.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            conn.Open();
            //Добавление всех price в коллекцию
            string query = "SELECT SUM(`price`*`count`) FROM `Main`";
            MySqlCommand command = new MySqlCommand(query, conn);
            //MySqlDataReader reader = command.ExecuteReader();
            MessageBox.Show($"Общая цена всех товаров: {Math.Round(Convert.ToDecimal(command.ExecuteScalar()), 2)}");
            conn.Close();
        }
        private void button4_Click(object sender, EventArgs e)
        {
            OrderForm of = new OrderForm();
            this.Hide();
            of.Show();
        }
    }
}
