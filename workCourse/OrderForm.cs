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
using ClassOrder;
using static System.ComponentModel.Design.ObjectSelectorEditor;
using System.Collections;
using Microsoft.VisualBasic;
using Org.BouncyCastle.Utilities.Collections;

namespace workCourse
{
    public partial class OrderForm : Form
    {
        public static string connStr = "server=chuc.caseum.ru;port=33333;user=st_2_20_8;database=is_2_20_st8_KURS;password=82411770;";
        public OrderForm()
        {
            InitializeComponent();
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            Main mn = new Main();
            this.Hide();
            mn.Show();
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            PriceEntry();
        }

        private void OrderForm_Load(object sender, EventArgs e)
        {
            MySqlConnection conn = new MySqlConnection(connStr);
            conn.Open();

            //заполнение combobox1 названиями товаров
            string query = "SELECT `title` FROM `Main`";
            MySqlCommand command = new MySqlCommand(query, conn);
            MySqlDataReader reader = command.ExecuteReader();
            while (reader.Read()) { comboBox1.Items.Add(reader.GetString(0)); }
            reader.Close();


            //заполнение combobox2 названиями поставщиков
            query = "SELECT `name_provider` FROM `t_provider`";
            command = new MySqlCommand(query, conn);
            reader = command.ExecuteReader();
            while (reader.Read()) { comboBox2.Items.Add(reader.GetString(0)); }
            reader.Close();

            conn.Close();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            PriceEntry();
        }

        void PriceEntry()
        {
            MySqlConnection conn = new MySqlConnection(connStr);
            conn.Open();

            if (numericUpDown1.Value > -1)
            {
                string query = $"SELECT `price` FROM `Main` WHERE `title` = '{comboBox1.SelectedItem}'";
                MySqlCommand command = new MySqlCommand(query, conn);
                MySqlDataReader reader = command.ExecuteReader();
                string price = "";
                double res = 0;
                while (reader.Read()) { price = (reader.GetString(0)); res = Convert.ToDouble(price) * Convert.ToDouble(numericUpDown1.Value); textBox1.Text = Convert.ToString(res); }
            }
            conn.Close();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Random rand = new Random();
            byte a = Convert.ToByte(rand.Next(1, 2));
            MessageBox.Show($"Заказ совершён, поступление через {a} час(ов)");
            MySqlConnection conn = new MySqlConnection(connStr);
            conn.Open();

            string query = $"UPDATE Main SET count = count + {numericUpDown1.Value} WHERE title = '{comboBox1.Text}'";
            MySqlCommand command = new MySqlCommand(query, conn);
            command.ExecuteNonQuery();

            conn.Close();
        }
    }
}
