using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using ComboBox = System.Windows.Forms.ComboBox;
using TextBox = System.Windows.Forms.TextBox;
using Button = System.Windows.Forms.Button;
namespace workCourse
{
    internal class Methods
    {
        internal ComboBox cb1;
        public Methods()
        {
            
        }
        //для orderform, для заполнения текстбокса с ценой
        internal static void PriceEntry(NumericUpDown num, ComboBox com, TextBox tex)
        {
            MySqlConnection conn = new MySqlConnection(Form1.connStr);
            conn.Open();

            if (num.Value > -1)
            {
                string query = $"SELECT `price` FROM `Main` WHERE `title` = '{com.SelectedItem}'";
                MySqlCommand command = new MySqlCommand(query, conn);
                MySqlDataReader reader = command.ExecuteReader();
                string price = "";
                double res = 0;
                while (reader.Read()) { price = (reader.GetString(0)); res = Convert.ToDouble(price) * Convert.ToDouble(num.Value); tex.Text = Convert.ToString(res); }
            }
            conn.Close();
        }
        internal void Delay()
        {
            Thread.Sleep(10000);
        }
        internal async Task GoDelay(NumericUpDown n, ComboBox c)
        {
            Random rnd = new Random();
            int time = rnd.Next(1, 2);
            bool b = true;
            int a = DateTime.Now.Minute;
            MessageBox.Show("Заказ сделан, ожидайте поступления");
            //ComboBox.Invoke((MethodInvoker)delegate { c.Text; });
            
            while (b)
            {
                await Task.Run(() => Delay());
                if (a + time == DateTime.Now.Minute)
                {


                    //MessageBox.Show($"num: {n.Value}, com: {cc.Text}");
                    b = false;
                    MySqlConnection conn = new MySqlConnection(Form1.connStr);
                    conn.Open();
                    string query = $"UPDATE Main SET count = count + {n.Value} WHERE title = '{c.Text}'";
                    MySqlCommand command = new MySqlCommand(query, conn);
                    command.ExecuteNonQuery();
                    if (MessageBox.Show("Заказ поступил, вернуться на главную страницу?", "", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        Main mn = new Main();
                        OrderForm of = new OrderForm();
                        mn.Show();
                        of.Hide();
                    }
                    conn.Close();

                    //catch (InvalidOperationException ex)
                    //{
                    //    MessageBox.Show(ex.);
                    //}

                }
                
            }
        }

        //DateTime dt = new DateTime();

        //

    }
}
