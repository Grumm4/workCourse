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
    public class Methods
    {
        
        public Methods()
        {
            
        }
        //для orderForm и sellinForm, для заполнения текстбокса с ценой
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
        //

        //метод вызова задержки в 10 секунд
        internal void OrderDelay()
        {
            Thread.Sleep(10000);
        }
        //

        //асинхронный метод заказа
        internal async Task GoOrder(int n, string c, Form form)
        {
            MySqlConnection conn = new MySqlConnection(Form1.connStr);
            try
            {
                conn.Open();
                string query = $"UPDATE Main SET count = count + {n} WHERE title = '{c}'";
                MySqlCommand command = new MySqlCommand(query, conn);
                command.ExecuteNonQuery();
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
        //

        //асинхронный метод продажи
        internal async Task GoSelling(int n, string c, Form form, TextBox text, string dt)
        {
            MySqlConnection conn = new MySqlConnection(Form1.connStr);
            try
            {
                conn.Open();
                string query = $"UPDATE Main SET count = count - {n} WHERE title = '{c}'";
                string query2 = $"INSERT INTO Orders (orderNumber) VALUES ('{text.Text}')";
                string query3 = $"INSERT INTO infoOrder (userId, item, count, data) VALUES ('{text.Text}', '{c}', '{Convert.ToInt32(n)}', '{dt}')";
                MySqlCommand command = new MySqlCommand(query, conn);
                MySqlCommand command2 = new MySqlCommand(query2, conn);
                MySqlCommand command3 = new MySqlCommand(query3, conn);
                command.ExecuteNonQuery();
                command2.ExecuteNonQuery();
                command3.ExecuteNonQuery();
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
        //
    }
}
