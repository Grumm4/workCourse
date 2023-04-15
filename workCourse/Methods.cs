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
using static System.Net.Mime.MediaTypeNames;

namespace workCourse
{
    public class Methods
    {
        int curPrice;
        static MySqlConnection conn = new MySqlConnection(Form1.connStr);
        //Main mn = new Main();
        public Methods()
        {
            
        }
        //для orderForm и sellinForm, для заполнения текстбокса с ценой
        //internal void newPriceEntry(int num, string title)
        //{
        //    conn.Open();
        //    string query = $"SELECT `price` FROM `Main` WHERE `title` = '{title}'";
        //    MySqlCommand command = new MySqlCommand(query, conn);
        //    MySqlDataReader reader = command.ExecuteReader();
        //    string price;
        //    double res;
        //    while (reader.Read()) { price = (reader.GetString(0)); res = Convert.ToDouble(price) * Convert.ToDouble(num.Value); tex.Tex = Convert.ToString(res); }
        //    conn.Close();
        //}
        internal virtual void PriceEntry(NumericUpDown num, TextBox tex, params ComboBox[] comboxes)
        {
            //MySqlConnection conn = new MySqlConnection(Form1.connStr);
            conn.Open();

            if (num.Value > -1)
            {
                if (comboxes.Length > 1)
                {
                    if (comboxes[0].SelectedItem == null)
                    {
                        string query = $"SELECT `price` FROM `newItems` WHERE `title` = '{comboxes[1].SelectedItem}'";
                        MySqlCommand command = new MySqlCommand(query, conn);
                        MySqlDataReader reader = command.ExecuteReader();
                        string price;
                        double res;
                        while (reader.Read()) { price = (reader.GetString(0)); res = Convert.ToDouble(price) * Convert.ToDouble(num.Value); tex.Text = Convert.ToString(res); }
                    }
                    else if (comboxes[1].SelectedItem == null)
                    {
                        string query = $"SELECT `price` FROM `Main` WHERE `title` = '{comboxes[0].SelectedItem}'";
                        MySqlCommand command = new MySqlCommand(query, conn);
                        MySqlDataReader reader = command.ExecuteReader();
                        string price;
                        double res;
                        while (reader.Read()) { price = (reader.GetString(0)); res = Convert.ToDouble(price) * Convert.ToDouble(num.Value); tex.Text = Convert.ToString(res); }
                    }
                }
                else if (comboxes.Length == 1)
                {
                    string query = $"SELECT `price` FROM `Main` WHERE `title` = '{comboxes[0].SelectedItem}'";
                    MySqlCommand command = new MySqlCommand(query, conn);
                    MySqlDataReader reader = command.ExecuteReader();
                    string price;
                    double res;
                    while (reader.Read()) { price = (reader.GetString(0)); res = Convert.ToDouble(price) * Convert.ToDouble(num.Value); tex.Text = Convert.ToString(res); }
                }
                
                
                
            }
            conn.Close();
        }
        //

        //метод вызова задержки в 10 секунд
        internal void OrderDelay() => Thread.Sleep(10000);
        //

        //асинхронный метод заказа
        internal Task GoOrder(int n, string c, Form form)
        {
            //MySqlConnection conn = new MySqlConnection(Form1.connStr);
            try
            {
                int pPrice;
                int pId;
                int numb;
                conn.Open();
                string query = $"UPDATE Main SET count = count + {n} WHERE title = '{c}'";
                MySqlCommand command = new MySqlCommand(query, conn);
                numb = command.ExecuteNonQuery();
                if (numb == 0) 
                {
                    string getPId = $"SELECT provider_Id FROM newItems WHERE title = '{c}'";
                    command = new MySqlCommand(getPId, conn);
                    pId = Convert.ToInt32(command.ExecuteScalar());

                    string getPrice = $"SELECT price FROM newItems WHERE title = '{c}'";
                    command = new MySqlCommand(getPrice, conn);
                    pPrice = Convert.ToInt32(command.ExecuteScalar());

                    string query2 = $"INSERT INTO Main (title, count, provider_id, price) VALUES ('{c}', count+{n}, {pId}, {pPrice})";
                    command = new MySqlCommand(query2, conn);
                    command.ExecuteNonQuery();
                }
                return Task.CompletedTask;
            }
            catch (InvalidOperationException ex) { MessageBox.Show(ex.Message); return Task.CompletedTask; }
            finally { conn.Close(); }
        }
        //

        //асинхронный метод продажи
        internal Task GoSelling(int n, string c, TextBox text, string dt, int r)
        {
            try
            {
                InMainDB(n, c);
                InInfoOrder(n, c, text, dt, r);
                return Task.CompletedTask;
            }
            catch (InvalidOperationException ex) { MessageBox.Show($"123!!!!!!!!!!!!! IN METHODS{ex.Message}"); return Task.CompletedTask; }
            finally { conn.Close(); }
        }
        //
        void InMainDB(int num, string title)
        {
            try
            {
                conn.Open();
                string query = $"UPDATE Main SET count = count - {num} WHERE title = '{title}'";
                MySqlCommand command = new MySqlCommand(query, conn);
                command.ExecuteNonQuery();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            finally { conn.Close(); }
        }
        
        void GetPrice(int num, string title, out int curP)
        {
            curP = 0;
            try
            {
                conn.Open();
                string query2 = $"SELECT price * {num} FROM Main WHERE title = '{title}' ";
                MySqlCommand command2 = new MySqlCommand(query2, conn);
                curP = Convert.ToInt32(command2.ExecuteScalar());
                //return curPrice;
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            finally { conn.Close(); }
        }

        void InInfoOrder(int num, string com, TextBox text, string dt, int r)
        {
            try
            {
                GetPrice(num, com, out curPrice);
                conn.Open();
                string query3 = $"INSERT INTO infoOrder (userId, item, count, orderNumber, priceCurPos, dateOrder) VALUES ('{text.Text}', '{com}', {num}, {r}, {curPrice}, '{dt}')";
                MySqlCommand command3 = new MySqlCommand(query3, conn);
                command3.ExecuteNonQuery();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            finally { conn.Close(); }
        }
        internal string RemoveRub(string str)
        {
            string res = "";
            foreach (var item in str)
            {
                if (item == (char)160 || item == '₽' || item == (char)32)
                    continue;
                res += item;
            }
            return res;
        }
    }
}
