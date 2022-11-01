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
    public partial class OrderForm : Form
    {
        public static string connStr = "server=chuc.caseum.ru;port=33333;user=st_2_20_8;database=is_2_20_st8_KURS;password=82411770;";
        public OrderForm()
        {
            InitializeComponent();
        }

        
        private void button1_Click(object sender, EventArgs e)
        {
            Main mn = new Main();
            this.Hide();
            mn.Show();
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            textBox1.Text+=numericUpDown1.Value;
        }

        private void OrderForm_Load(object sender, EventArgs e)
        {
            MySqlConnection conn = new MySqlConnection(connStr);
            conn.Open();

            string query = "SELECT title FROM Main";
            MySqlCommand command = new MySqlCommand(query, conn);

            MySqlDataReader reader = command.ExecuteReader();
            List<string[]> data = new List<string[]>();
            
            while (reader.Read())
            {
                

                data[data.Count - 1][0] = reader[0].ToString();
                
            }
            reader.Close();
            conn.Close();
            int i = 0;
            foreach (string[] s in data)
            {
                comboBox1.Items.Add(s[i]);
                i++;
            }
        }
    }
}
