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
        public string pass = "";
        public static string connStr = "server=chuc.caseum.ru;port=33333;user=st_2_20_8;database=is_2_20_st8_KURS;password=82411770;";


        //Переменная соединения
        MySqlConnection conn;
        //Логин и пароль к данной форме Вы сможете посмотреть в БД db_test таблице t_user

        //Вычисление хэша строки и возрат его из метода
        static string sha256(string randomString)
        {
            //Тут происходит криптографическая магия. Смысл данного метода заключается в том, что строка залетает в метод
            var crypt = new System.Security.Cryptography.SHA256Managed();
            var hash = new System.Text.StringBuilder();
            byte[] crypto = crypt.ComputeHash(Encoding.UTF8.GetBytes(randomString));
            foreach (byte theByte in crypto)
            {
                hash.Append(theByte.ToString("x2"));
            }
            return hash.ToString();
        }

        //Метод запроса данных пользователя по логину для запоминания их в полях класса
        public void GetUserInfo(string login_user)
        {
            //Объявлем переменную для запроса в БД
            string selected_id_stud = textBox1.Text;
            // устанавливаем соединение с БД
            conn.Open();
            // запрос
            string sql = $"SELECT * FROM t_user WHERE loginUser='{login_user}'";
            // объект для выполнения SQL-запроса
            MySqlCommand command = new MySqlCommand(sql, conn);
            // объект для чтения ответа сервера
            MySqlDataReader reader = command.ExecuteReader();
            // читаем результат
            while (reader.Read())
            {
                // элементы массива [] - это значения столбцов из запроса SELECT
                Auth.auth_id = reader[0].ToString();
                Auth.auth_fio = reader[1].ToString();
            }
            reader.Close(); // закрываем reader
            // закрываем соединение с БД
            conn.Close();
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
            button1.BackColor = Color.FromArgb(15, 51, 117);
            button2.BackColor = Color.FromArgb(15, 51, 117);
        }

        Main main = new Main();
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
            textChang();
            
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            textBox1.BackColor = Color.White;
            label1.BackColor = Color.White;
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e) => MessageBox.Show("Информация о программе", "Main");

        private void разработчикToolStripMenuItem_Click(object sender, EventArgs e) => MessageBox.Show("telegram: @grumm4ik\nVK: vk.com/grumm4ik", "О создателе");

        void Login()
        {
            if (textBox1.Text.Length == 0)
            {
                label1.BackColor = Color.FromArgb(155, 4, 0);
                textBox1.BackColor = Color.FromArgb(155, 4, 0);
            }
            if (textBox2.Text.Length == 0)
            {
                label2.BackColor = Color.FromArgb(155, 4, 0);
                textBox2.BackColor = Color.FromArgb(155, 4, 0);
            }
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
            //Если вернулась больше 0 строк, значит такой пользователь существует
            
            if (table.Rows.Count > 0)
            {
                //Присваеваем глобальный признак авторизации
                Auth.auth = true;
                //Достаем данные пользователя в случае успеха
                GetUserInfo(textBox1.Text);
                
                //Закрываем форму
                this.Hide();
                main.Show();
                new ToastContentBuilder()
                    .AddArgument("action", "viewConversation")
                    .AddArgument("conversationId", 9813)
                    .AddText("Выполнен вход")
                    .AddText($"Пользователь: {textBox1.Text}")
                    .AddAppLogoOverride(new Uri("C:\\Users\\Kirill\\Desktop\\1\\1\\materials\\bg.jpg"), ToastGenericAppLogoCrop.Circle)
                    .Show(); // Not seeing the Show() me
                table = null;
                adapter = null;
                pass = null;
                command = null;
            }
            else
            {
                

                if (textBox1.Text.Length == 0 || textBox2.Text.Length == 0) 
                { 
                    MessageBox.Show("Поля не должны оставаться пустыми!", "Ошибка входа:"); 
                    
                    
                }
                if (table.Rows.Count == 0)
                {
                    DeleteText();
                    MessageBox.Show("Неверные данные авторизации!", "Ошибка входа:");
                }
                table = null;
                adapter = null;
                pass = null;
                command = null;
            }
        }
        internal void DeleteText()
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action(async () => await Task.Run(() => textBox1.Text = "")));
                this.Invoke(new Action(async () => await Task.Run(() => textBox2.Text = "")));
            }
            else
            {
                textBox1.Text = "";
                textBox2.Text = "";
            }
            
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
                Login();
            if (e.KeyData == Keys.Back)
                textBox1.Text.Replace(Convert.ToString(textBox1.Text[textBox1.Text.Length - 1]), "");
        }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
                Login();
            if (e.KeyData == Keys.Back)
                pass.Replace(Convert.ToString(pass[pass.Length - 1]), "");
        }
    }
}
