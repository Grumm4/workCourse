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
using static System.Net.Mime.MediaTypeNames;

namespace workCourse
{
    public partial class SellingForm : Form
    {
        decimal totalPrice;
        //Dictionary<string, int> orderBasket = new Dictionary<string, int>();
        Methods m = new Methods();
        public Dictionary<string, int> orderBasket = new Dictionary<string, int>();
        public Dictionary<string, int> realBasket = new Dictionary<string, int>();
        public SellingForm() => InitializeComponent();

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            var date = DateTime.Now;
            string formatedDate = date.ToString("yyyy:MM:dd");
            var dateOnly = new DateOnly(date.Year, date.Month, date.Day);
            
            Regex reg = new Regex("^((\\+7)+([0-9]){10})$");
            MatchCollection mc = reg.Matches(textBox2.Text);
            //Random rnd = new Random();
            int rand = new Random().Next(100000, 999999);
            if (mc.Count > 0) 
            {
                MySqlConnection conn = new MySqlConnection(Form1.connStr);
                string query2 = $"INSERT INTO Orders (phoneNumber, orderId, dateOrd, totalPrice) VALUES ('{textBox2.Text}', {rand}, '{formatedDate}', {m.RemoveRub(textBox1.Text)})";
                MySqlCommand command2 = new MySqlCommand(query2, conn);
                conn.Open();
                command2.ExecuteNonQuery();
                conn.Close();
                if (orderBasket.Count > 0)
                {
                    foreach (var tov in realBasket)
                    {
                        try { await m.GoSelling(Convert.ToInt32(tov.Value), tov.Key, textBox2, formatedDate, rand); }
                        catch (Exception ex) { MessageBox.Show(ex.Message); }
                    }
                }
                else
                {
                    MessageBox.Show("Вы не добавили ни одного товара к заказу!!!", String.Empty, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                if (MessageBox.Show("Товар продан, вернуться на главную страницу?", String.Empty, MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                {
                    Close();
                }
            }
            else
            {
                MessageBox.Show("Вы не ввели номер телефона, либо ввели его в неправильном формате!", String.Empty, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }

        private void SellingForm_Load(object sender, EventArgs e)
        {
            /*MySqlConnection conn = new MySqlConnection(Form1.connStr);
            conn.Open();

            //заполнение combobox1 названиями товаров
            string query = "SELECT `title` FROM `Main`";
            MySqlCommand command = new MySqlCommand(query, conn);
            MySqlDataReader reader = command.ExecuteReader();
            while (reader.Read()) { comboBox1.Items.Add(reader.GetString(0)); }
            reader.Close();
            */
        }

        //private void numericUpDown1_ValueChanged(object sender, EventArgs e) => m.PriceEntry(numericUpDown1, textBox1, comboBox1);

        //private void comboBox1_SelectedIndexChanged(object sender, EventArgs e) => m.PriceEntry(numericUpDown1, textBox1, comboBox1);

        private void button3_Click(object sender, EventArgs e)
        {
            /*orderBasket.Add(comboBox1.Text, Convert.ToInt32(numericUpDown1.Value));
            listBox1.Items.Add($"Товар: {comboBox1.Text} | Количество: {Convert.ToInt32(numericUpDown1.Value)}");
            totalPrice += Convert.ToDecimal(textBox1.Text);

            textBox1.Text = "";
            comboBox1.SelectedItem = null;
            numericUpDown1.Value = 0;
            */
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            
            tovInStock inStock = new tovInStock();
            inStock.ShowDialog();
            textBox1.Text = string.Format("{0:C0}", Convert.ToInt32(inStock.label3.Text));
            inStock.FillList(out orderBasket);
            foreach (var item in orderBasket)
            {
                //textBox1.Text += 
                realBasket.Add(item.Key, item.Value);
                listBox1.Items.Add($"Товар: {item.Key} | Количество: {Convert.ToInt32(item.Value)}");
            }
        }
    }
}