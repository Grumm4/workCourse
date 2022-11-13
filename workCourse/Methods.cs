using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using ComboBox = System.Windows.Forms.ComboBox;
using TextBox = System.Windows.Forms.TextBox;
namespace workCourse
{
    internal class Methods
    {
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
    }
}
