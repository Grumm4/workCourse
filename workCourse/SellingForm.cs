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
using System.Text.RegularExpressions;

namespace workCourse
{
    public partial class SellingForm : Form
    {
        Dictionary<string, int> orderBasket = new Dictionary<string, int>();
        Methods m = new Methods();
        public SellingForm() => InitializeComponent();

        private void button1_Click(object sender, EventArgs e)
        {
            Main mn = new Main();
            this.Hide();
            mn.Show();
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            var date = DateTime.Now;
            string formatedDate = date.ToString("yyyy:MM:dd");
            var dateOnly = new DateOnly(date.Year, date.Month, date.Day);
            
            Regex reg = new Regex("^((\\+7)+([0-9]){10})$");
            MatchCollection mc = reg.Matches(textBox2.Text);
            if (mc.Count > 0) 
            {
                foreach (var tov in orderBasket)
                {
                    try
                    {
                        await m.GoSelling(Convert.ToInt32(tov.Value), tov.Key, this, textBox2, formatedDate);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }

                if (MessageBox.Show("Товар продан, вернуться на главную страницу?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                {
                    Main mn = new Main();
                    this.Close();
                    mn.Show();
                }
            }
            
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

        private void numericUpDown1_ValueChanged(object sender, EventArgs e) => Methods.PriceEntry(numericUpDown1, comboBox1, textBox1);

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Methods.PriceEntry(numericUpDown1, comboBox1, textBox1);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            orderBasket.Add(comboBox1.Text, Convert.ToInt32(numericUpDown1.Value));
            listBox1.Items.Add($"Товар: {comboBox1.Text} | Количество: {Convert.ToInt32(numericUpDown1.Value)}");

            textBox1.Text = "";
            comboBox1.SelectedItem = null;
            numericUpDown1.Value = 0;
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }
    }
}