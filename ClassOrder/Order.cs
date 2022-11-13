using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using MySql.Data.MySqlClient;
using System.Windows.Forms;





namespace ClassOrder
{
    public delegate void a();
    public class Order
    {
        internal void DeleteText(a)
        {
            if (this.Con)
            {
                a.Invoke(new Action(async () => await Task.Run(() => textBox1.Text = "")));
                a.Invoke(new Action(async () => await Task.Run(() => textBox2.Text = "")));
            }
            else
            {
                textBox1.Text = "";
                textBox2.Text = "";
            }

        }
        public Order()
        {
            
        }
        public void Zak()
        {

        }


    }
}
