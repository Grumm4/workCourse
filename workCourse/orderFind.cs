﻿using System;
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
using System.Text.RegularExpressions;

namespace workCourse
{
    public partial class orderFind : Form
    {
        
        byte sale;
        decimal curentPrice;
        decimal result;
        Main mn = new Main();
        DateOnly don;
        public string id;
        MySqlConnection conn = new MySqlConnection(Form1.connStr);
        List<string[]> data = new List<string[]>();
        List<string[]> data2 = new List<string[]>();
        public orderFind()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string sql = $"SELECT phoneNumber, orderId, dateOrd, totalPrice FROM Orders WHERE phoneNumber = '{textBox1.Text}'";
            FillDG(sql);
        }
        void FillDG(string sql)
        {
            label4.Text = String.Empty;
            label6.Text = String.Empty;
            dataGridView1.Rows.Clear();
            data.Clear();
            
            MySqlCommand cmd = new MySqlCommand(sql, conn);
            conn.Open();
            MySqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                data.Add(new string[4]);
                data[data.Count - 1][0] = reader[0].ToString();
                data[data.Count - 1][1] = reader[1].ToString();
                don = new DateOnly(Convert.ToDateTime(reader[2]).Year, Convert.ToDateTime(reader[2]).Month, Convert.ToDateTime(reader[2]).Day);
                data[data.Count - 1][2] = don.ToString();
                data[data.Count - 1][3] = reader[3].ToString();

            }
            reader.Close();
            conn.Close();
            foreach (string[] s in data)
            {
                dataGridView1.Rows.Add(s);

            }
        }

        void ReturnPrice(int numb)
        {
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if (dataGridView1[0, i].Value.ToString() == dataGridView1[0,numb].Value.ToString())
                {
                    result += Convert.ToDecimal(dataGridView1[3, i].Value);
                }
            }

            label4.Text = String.Format("{0:c}", result).ToString();

            if (result < 5000m)
                label6.Text = "Скидка недоступна";
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
            Close();
        }

        private void orderFind_Load(object sender, EventArgs e)
        {
            string sql = $"SELECT phoneNumber, orderId, dateOrd, totalPrice FROM Orders";
            FillDG(sql);
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                id = dataGridView1[1, e.RowIndex].Value.ToString();
                orderCard od = new orderCard(id);
                od.ShowDialog();
            }
            catch (System.ArgumentOutOfRangeException)
            {
                MessageBox.Show("Не кликать по заголовкам!");
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if(textBox1.Text.Length == 0)
            {
                string sql = "SELECT phoneNumber, orderId, dateOrd, totalPrice FROM Orders";
                dataGridView1.Rows.Clear();

                FillDG(sql);
            }
            //else
            //{
            //    Regex regex = new Regex(@"^((\+7)+([0-9]){10})$");
            //    MatchCollection match = regex.Matches(textBox1.Text);
            //    if (match.Count > 0)
            //    {
                    
            //    }
                
            //}
        }

        private void dataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            int numb = e.RowIndex;
            if (dataGridView1.Rows.Count != 0)
            {
                ReturnPrice(numb);
            }
        }
    }
}
