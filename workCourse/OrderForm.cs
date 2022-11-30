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

using static System.ComponentModel.Design.ObjectSelectorEditor;
using System.Collections;
using Microsoft.VisualBasic;
using Org.BouncyCastle.Utilities.Collections;
using System.Threading;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using ComboBox = System.Windows.Forms.ComboBox;

namespace workCourse
{
    public partial class OrderForm : Form
    {
        Methods m = new Methods();
        public OrderForm() 
        {
            InitializeComponent(); 
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Main mn = new Main();
            this.Close();
            mn.Show();
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e) => Methods.PriceEntry(numericUpDown1, comboBox1, textBox1);

        private void OrderForm_Load(object sender, EventArgs e)
        {
            MySqlConnection conn = new MySqlConnection(Form1.connStr);
            conn.Open();

            //заполнение combobox1 названиями товаров
            string query = "SELECT `title` FROM `Main`";
            MySqlCommand command = new MySqlCommand(query, conn);
            MySqlDataReader reader = command.ExecuteReader();
            while (reader.Read()) { comboBox1.Items.Add(reader.GetString(0)); }
            reader.Close();
            conn.Close();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e) => Methods.PriceEntry(numericUpDown1, comboBox1, textBox1);
        
        private async void button2_Click(object sender, EventArgs e)
        {
            ComboBox combo = this.comboBox1;
            NumericUpDown num = this.numericUpDown1;

            //Вызов метода обработки заказа
            try
            {
                await m.GoDelay(num, combo, this);
            }
            catch (MySqlException)
            {
                MessageBox.Show("Для этого товара места на складе больше нет");
            }
        }
        
        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
    
}
