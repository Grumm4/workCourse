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
            ComboBox combo = this.comboBox1;
            NumericUpDown num = this.numericUpDown1;

            //Вызов метода обработки заказа
            if (Convert.ToString(comboBox1.SelectedItem) != "" && numericUpDown1.Value != 0)
            {
                try
                {
                    await m.GoSelling(num, combo, this);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else if (Convert.ToString(comboBox1.SelectedItem) == "")
            {
                MessageBox.Show("Вы должны выбрать товар, который хотите продать!");
                numericUpDown1.Value = 0;
            }
            else if (numericUpDown1.Value == 0)
            {
                MessageBox.Show("Вы должны указать количество товара, который хотите продать!");
                comboBox1.SelectedValue = "";
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
    }
}