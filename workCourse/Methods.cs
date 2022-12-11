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
        //internal ComboBox cb1;
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
                string price;
                double res;
                while (reader.Read()) { price = (reader.GetString(0)); res = Convert.ToDouble(price) * Convert.ToDouble(num.Value); tex.Text = Convert.ToString(res); }
            }
            conn.Close();
        }
        internal void OrderDelay()
        {
            Thread.Sleep(10000);
        }
        internal async Task GoOrder(NumericUpDown n, ComboBox c, Form form)
        {
            Random rnd = new Random();
            int time = rnd.Next(1, 3);
            bool b = true;
            int a = DateTime.Now.Minute;
            MessageBox.Show("Заказ сделан, ожидайте поступления");
            
            while (b)
            {
                await Task.Run(() => OrderDelay());
                if (a + time == a + time)
                {
                    MySqlConnection conn = new MySqlConnection(Form1.connStr);
                    try
                    {
                        b = false;
                        conn.Open();
                        string query = $"UPDATE Main SET count = count + {n.Value} WHERE title = '{c.Text}'";
                        MySqlCommand command = new MySqlCommand(query, conn);
                        command.ExecuteNonQuery();
                        if (MessageBox.Show("Заказ поступил, вернуться на главную страницу?", "", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            Main mn = new Main();
                            form.Close();
                            mn.Show();
                            b = false;
                        }
                        
                    }
                    catch (InvalidOperationException ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    finally
                    {
                        conn.Close();
                    }

                }
                
            }
        }
        internal async Task GoSelling(NumericUpDown n, ComboBox c, Form form)
        {
            Random rnd = new Random();
            int time = rnd.Next(1, 3);
            bool b = true;
            int a = DateTime.Now.Minute;
            MessageBox.Show("Операция выполнена, ожидайте отправки");

            while (b)
            {
                await Task.Run(() => OrderDelay());
                if (a + time == a + time)
                {
                    MySqlConnection conn = new MySqlConnection(Form1.connStr);
                    try
                    {
                        b = false;
                        conn.Open();
                        string query = $"UPDATE Main SET count = count - {n.Value} WHERE title = '{c.Text}'";
                        MySqlCommand command = new MySqlCommand(query, conn);
                        command.ExecuteNonQuery();
                        if (MessageBox.Show("Товар продан, вернуться на главную страницу?", "", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            Main mn = new Main();
                            form.Close();
                            mn.Show();
                            b = false;
                        }

                    }
                    catch (InvalidOperationException ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    finally
                    {
                        conn.Close();
                    }

                }

            }
        }
    }
}
