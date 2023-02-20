﻿using MySql.Data.MySqlClient;
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

        Dictionary<string, int> orderBasket = new Dictionary<string, int>();
        Methods m = new Methods();
        public OrderForm()
        {
            InitializeComponent();
        }


        private void button1_Click(object sender, EventArgs e)
        {
            Main mn = new Main();
            mn.Show();
            this.Close();
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e) => m.PriceEntry(numericUpDown1, textBox1, comboBox1, comboBox2);

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

            //Заполняем combobox2
            string query2 = "SELECT `title` FROM `newItems`";
            MySqlCommand cmd = new MySqlCommand(query2, conn);
            MySqlDataReader reader2 = cmd.ExecuteReader();
            while (reader2.Read()) { comboBox2.Items.Add(reader2.GetString(0)); }
            reader2.Close();
            conn.Close();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e) => m.PriceEntry(numericUpDown1, textBox1, comboBox1, comboBox2);

        private async void button2_Click(object sender, EventArgs e)
        {
            foreach (var tov in orderBasket)
            {
                try
                {
                    await m.GoOrder(Convert.ToInt32(tov.Value), tov.Key, this);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            
            if (MessageBox.Show("Заказ поступил, вернуться на главную страницу?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            {
                Main mn = new Main();
                this.Close();
                mn.Hide();
                mn.Show();
                
            }
            else
            {
                listBox1.Items.Clear();
                orderBasket.Clear();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem == null)
            {
                orderBasket.Add(comboBox2.Text, Convert.ToInt32(numericUpDown1.Value));
                listBox1.Items.Add($"Товар: {comboBox2.Text} | Количество: {Convert.ToInt32(numericUpDown1.Value)}");
            }
            if (comboBox2.SelectedItem == null)
            {
                orderBasket.Add(comboBox1.Text, Convert.ToInt32(numericUpDown1.Value));
                listBox1.Items.Add($"Товар: {comboBox1.Text} | Количество: {Convert.ToInt32(numericUpDown1.Value)}");
            }
            
            comboBox1.SelectedItem = null;
            comboBox2.SelectedItem = null;
            numericUpDown1.Value = 0;
        }

        private void comboBox1_DropDown(object sender, EventArgs e) => comboBox2.SelectedItem = null;

        private void comboBox2_DropDown(object sender, EventArgs e) => comboBox1.SelectedItem = null;

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
    
}
