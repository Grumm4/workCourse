using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using workCourse;
using MySql.Data.MySqlClient;

namespace workCourse
{
    public partial class orderFind : Form
    {
        byte sale;
        decimal curentPrice;
        decimal result;
        Main mn = new Main();
        DateOnly don;
        MySqlConnection conn = new MySqlConnection(Form1.connStr);
        List<string[]> data = new List<string[]>();
        List<string[]> data2 = new List<string[]>();
        public orderFind()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            label4.Text = ""; 
            label6.Text = "";
            dataGridView1.Rows.Clear();
            data.Clear();
            string sql = $"SELECT userId, item, count, data FROM infoOrder WHERE userId = '{textBox1.Text}'";
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            conn.Open();
            MySqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                data.Add(new string[4]);
                data[data.Count - 1][0] = reader[0].ToString();
                data[data.Count - 1][1] = reader[1].ToString();
                data[data.Count - 1][2] = reader[2].ToString();
                don = new DateOnly(Convert.ToDateTime(reader[3]).Year, Convert.ToDateTime(reader[3]).Month, Convert.ToDateTime(reader[3]).Day);
                data[data.Count - 1][3] = don.ToString();
                
            }
            reader.Close();
            conn.Close();
            foreach (string[] s in data)
            {
                dataGridView1.Rows.Add(s);
                
            }
            if (dataGridView1.Rows.Count != 0)
            {
                ReturnPrice();
            }
            
        }
        void ReturnPrice()
        {
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {

                string sql = $"SELECT price FROM Main WHERE title = '{dataGridView1[1, i].Value}'";
                MySqlCommand cmd = new MySqlCommand(sql, conn);
                conn.Open();
                curentPrice = Convert.ToDecimal(cmd.ExecuteScalar());
                conn.Close();
                result += curentPrice * Convert.ToDecimal(dataGridView1[2, i].Value);
            }
            label4.Text = String.Format("{0:c}", result).ToString();
            
            if (result > 5000m)
                label6.Text = "2%";
            if (result > 10000m)
                label6.Text = "5%";
            if (result > 15000m)
                label6.Text = "7%";
            if (result > 30000m)
                label6.Text = "11%";
            if (result > 50000m) 
                label6.Text = "15%";

            result = 0;
            curentPrice = 0;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            mn.Show();
            this.Close();
        }
    }
}
