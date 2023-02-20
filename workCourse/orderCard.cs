using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace workCourse
{
    public partial class orderCard : Form
    {
        List<string[]> data = new List<string[]>();
        DateOnly don;
        private string id;
        public string Id
        {
            get { return id; }
            set { id = value; }
        }
        public orderCard(string id)
        {
            InitializeComponent();
            Id = id;
        }

        private void orderCard_Load(object sender, EventArgs e)
        {
            FillDataGrid(Id);   
        }
        void FillDataGrid(string idOrder)
        {
            MySqlConnection conn = new MySqlConnection(Form1.connStr);
            dataGridView1.Rows.Clear();
            data.Clear();
            string sql = $"SELECT userId, item, count, dateOrder FROM infoOrder WHERE orderNumber = '{idOrder}'";
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
        }
    }
}
