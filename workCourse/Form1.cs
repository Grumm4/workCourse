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
using Microsoft.Toolkit.Uwp.Notifications;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace workCourse
{
    public partial class Form1 : Form
    {

        object locker = new();
        public string pass = "";
        public static string connStr = "server=chuc.sdlik.ru;port=33333;user=st_2_20_8;database=is_2_20_st8_KURS;password=82411770;";


        //Переменная соединения
        MySqlConnection conn;
        //Логин и пароль к данной форме Вы сможете посмотреть в БД db_test таблице t_user

        //Вычисление хэша строки и возрат его из метода
        static string sha256(string randomString)
        {
            StringBuilder hash = new StringBuilder();
            try
            {
                //Шифрование пароля пользователя
                var crypt = new System.Security.Cryptography.SHA256Managed();
                hash = new System.Text.StringBuilder();
                byte[] crypto = crypt.ComputeHash(Encoding.UTF8.GetBytes(randomString));
                foreach (byte theByte in crypto)
                {
                    hash.Append(theByte.ToString("x2"));
                }
                return hash.ToString();
            }
            catch (Exception)
            {
                return hash.ToString();
            }
           
        }

        public Form1()
        {
            InitializeComponent();
        }
        void aboutItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("О программе");
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            conn = new MySqlConnection(connStr);
            //button1.BackColor = Color.FromArgb(15, 51, 117);
            //button2.BackColor = Color.FromArgb(15, 51, 117);
        }
        private void button2_Click(object sender, EventArgs e) => Application.Exit();

        private void button1_Click(object sender, EventArgs e) => Login();

        public void textChang()
        {
            pass += textBox2.Text;
            textBox2.Text = System.Text.RegularExpressions.Regex.Replace($"{textBox2.Text}", @"\w", "*");
            textBox2.SelectionStart = textBox2.Text.Length;
            textBox2.BackColor = Color.White;
            label2.BackColor = Color.White;
            pass = pass.Replace($"*", "");
        }
        
        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            label3.Visible = false;
            textChang();
            
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            textBox1.BackColor = Color.White;
            label1.BackColor = Color.White;
            label3.Visible = false;
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e) => MessageBox.Show("Информация о программе", "Main");

        private void разработчикToolStripMenuItem_Click(object sender, EventArgs e) => MessageBox.Show("telegram: @grumm4ik\nVK: vk.com/grumm4ik", "О создателе");

        public int Login()
        {
            //if (textBox1.Text.Length == 0)
            //{
            //    label1.BackColor = Color.FromArgb(155, 4, 0);
            //    textBox1.BackColor = Color.FromArgb(155, 4, 0);
            //}
            //if (textBox2.Text.Length == 0)
            //{
            //    label2.BackColor = Color.FromArgb(155, 4, 0);
            //    textBox2.BackColor = Color.FromArgb(155, 4, 0);
            //}
            //Запрос в БД на предмет того, если ли строка с подходящим логином и паролем
            string sql = "SELECT * FROM t_user WHERE loginUser = @un and passUser = @up";
            //Открытие соединения
            conn.Open();
            //Объявляем таблицу
            DataTable table = new DataTable();
            //Объявляем адаптер
            MySqlDataAdapter adapter = new MySqlDataAdapter();
            //Объявляем команду
            MySqlCommand command = new MySqlCommand(sql, conn);
            //Определяем параметры
            command.Parameters.Add("@un", MySqlDbType.VarChar, 25);
            command.Parameters.Add("@up", MySqlDbType.VarChar, 25);
            //Присваиваем параметрам значение
            command.Parameters["@un"].Value = textBox1.Text;
            command.Parameters["@up"].Value = sha256(pass);
            //Заносим команду в адаптер
            adapter.SelectCommand = command;
            //Заполняем таблицу
            adapter.Fill(table);
            //Закрываем соединение
            conn.Close();
            //Если вернулось больше 0 строк, значит такой пользователь существует
            
            if (table.Rows.Count > 0)
            {
                Main main = new Main();
                //Закрытие формы
                this.Hide();
                main.Show();
                //main.Invoke(new Action(() => {  }));
                
                //new ToastContentBuilder()
                //    .AddArgument("action", "viewConversation")
                //    .AddArgument("conversationId", 9813)
                //    .AddText("Выполнен вход")
                //    .AddText($"Пользователь: {textBox1.Text}")
                //    .Show(); // Not seeing the Show() me
                table = null;
                adapter = null;
                pass = null;
                command = null;
                return 1;
            }
            else
            {
                if (textBox1.Text.Length == 0 || textBox2.Text.Length == 0) 
                {

                    label3.Visible = true;
                    label3.Text="Поля не должны оставаться пустыми!";
                    System.Threading.Thread thread2 = new System.Threading.Thread(OnOff);
                    thread2.Start();
                    
                    
                }
                else if (table.Rows.Count == 0)
                {
                    DeleteText();
                    label3.Visible = true;
                    label3.Text = "Неверные данные авторизации!";
                    System.Threading.Thread thread = new System.Threading.Thread(OnOff);
                    thread.Start();
                }
                table = null;
                adapter = null;
                pass = null;
                command = null;
                return 0;
            }
        }
        void OnOff()
        {
            lock (locker)
            {
                for (int i = 0; i < 5; i++)
                {
                    if (label3.Visible == true)
                    {
                        label3.Invoke(new Action(() => { label3.Visible = false; }));
                    }
                    else if (label3.Visible == false)
                    {
                        label3.Invoke(new Action(() => { label3.Visible = true; }));
                    }
                    System.Threading.Thread.Sleep(500);

                }
                label3.Invoke(new Action(() => { label3.Visible = true; }));
            }
        }
        internal void DeleteText()
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action(async () => await Task.Run(() => textBox1.Text = String.Empty)));
                this.Invoke(new Action(async () => await Task.Run(() => textBox2.Text = String.Empty)));
            }
            else
            {
                textBox1.Text = String.Empty;
                textBox2.Text = String.Empty;
            }
            
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
                Login();
            if (textBox2.Text.Length != 0)
            {
                if (e.KeyData == Keys.Back)
                {
                    textBox1.Text.Replace(Convert.ToString(textBox1.Text[textBox1.Text.Length - 1]), "");
                    pass.Replace(pass[pass.Length - 1], Convert.ToChar(""));
                }
            }
            
        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
                Login();
            if(textBox2.Text.Length != 0)
            {
                if (e.KeyData == Keys.Back)
                { 
                    pass = pass.Remove(pass.Length - 1, 1);
                    //pass = pass.Replace(pass[pass.Length - 1].ToString(), "");
                    //pass.Replace(pass[pass.Length - 1], Convert.ToChar(""));
                }
            }
            
        }

        //private void metroButton1_Click(object sender, EventArgs e)
        //{
        //    Login();
        //}

        //private void metroButton2_Click(object sender, EventArgs e)
        //{
        //    Application.Exit();
        //}
    }
}
