using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace workCourse
{
    //static class Auth
    //{
    //    //��������� ����, ������� ������ �������� ������� �����������
    //    public static bool auth = false;
    //    //��������� ����, ������� ������ �������� ID ������������
    //    public static string auth_id = null;
    //    //��������� ����, ������� ������ �������� ��� ������������
    //    public static string auth_fio = null;
    //    //��������� ����, ������� ������ ���������� ���������� ������������
    //    public static int auth_role = 0;
    //}
    static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
