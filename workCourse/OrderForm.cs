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
            textBox1.Text+=numericUpDown1.Value;
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


            //заполнение combobox1 названиями поставщиков
            query = "SELECT `name_provider` FROM `t_provider`";
            command = new MySqlCommand(query, conn);
            reader = command.ExecuteReader();
            while (reader.Read()) { comboBox2.Items.Add(reader.GetString(0)); }
            reader.Close();

            conn.Close();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            MySqlConnection conn = new MySqlConnection(connStr);
            conn.Open();

            //заполнение текстбокса с ценой
            string query = $"SELECT `price` FROM `Main` WHERE `title` = '{comboBox1.SelectedItem}'";
            MySqlCommand command = new MySqlCommand(query, conn);
            MySqlDataReader reader = command.ExecuteReader();
            while (reader.Read()) { textBox1.Text = (reader.GetString(0)); }
        }
    }
}
