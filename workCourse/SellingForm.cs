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
    public partial class SellingForm : Form
    {
        public SellingForm() => InitializeComponent();

        private void button1_Click(object sender, EventArgs e)
        {
            Main mn = new Main();
            this.Hide();
            mn.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            MySqlConnection conn = new MySqlConnection(Form1.connStr);
            conn.Open();

            string query = $"UPDATE `Main` SET `count` = `count` - {numericUpDown1.Value}, WHERE `title` = '{comboBox1.Text}'";
            MySqlCommand command = new MySqlCommand(query, conn);
            command.ExecuteNonQuery();
            if (MessageBox.Show("Товар продан, вернуться на главную страницу?", "", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                Main mn = new Main();
                this.Hide();
                mn.Show();
            }
            conn.Close();
        }

        private void SellingForm_Load(object sender, EventArgs e)
        {
            MySqlConnection conn = new MySqlConnection(Form1.connStr);
            conn.Open();

            //заполнение combobox1 названиями товаров
            string query = "SELECT `title` FROM `Main`";
            MySqlCommand command = new MySqlCommand(query, conn);
            MySqlDataReader reader = command.ExecuteReader();
            while (reader.Read()) { comboBox1.Items.Add(reader.GetString(0)); }
            reader.Close();
        }
    }
}
