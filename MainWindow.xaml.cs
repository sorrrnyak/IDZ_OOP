using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace tyaf
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static string db_link = System.IO.Directory.GetCurrentDirectory() + @"\tyafbaza).db";
        private static string connection_string = @"Data Source= " + db_link + ";Version=3;";
        public MainWindow()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }

        private void Button_Reg_Click(object sender, RoutedEventArgs e)
        {
            textboxlogin.ToolTip = "";
            textboxlogin.Background = Brushes.Transparent;
            passbox.ToolTip = "";
            passbox.Background = Brushes.Transparent;
            passbox_2.ToolTip = "";
            passbox_2.Background = Brushes.Transparent;
            textboxemail.ToolTip = "";
            textboxemail.Background = Brushes.Transparent;
            string login = textboxlogin.Text.Trim();
            string pass = passbox.Password.Trim();
            string pass_2 = passbox_2.Password.Trim();
            string email = textboxemail.Text.Trim().ToLower();

            if (login.Length < 5)
            {
                textboxlogin.ToolTip = "Логин должен содержать от 6 символов!";
                textboxlogin.Background = Brushes.Red;
            }
            else if (pass.Length < 6)
            {
                passbox.ToolTip = "Пароль должен содержать от 6 символов!";
                passbox.Background = Brushes.Red;
            }
            else if (pass != pass_2)
            {
                passbox_2.ToolTip = "Пароли не совпадают";
                passbox_2.Background = Brushes.Red;
            }
            else if (email.Length < 5 || !email.Contains("@") || !email.Contains("."))
            {
                textboxemail.ToolTip = "Это не почта!";
                textboxemail.Background = Brushes.Red;
            }
            else
            {
                textboxlogin.Background = Brushes.Transparent;
                passbox.Background = Brushes.Transparent;
                passbox_2.Background = Brushes.Transparent;
                textboxemail.Background = Brushes.Transparent;
            }
            if (textboxlogin.Background == Brushes.Transparent && passbox.Background == Brushes.Transparent && passbox_2.Background == Brushes.Transparent && textboxemail.Background == Brushes.Transparent)
            {
                User user = new User();
                user.Login = login;
                user.Pass = pass;
                using (SQLiteConnection connect = new SQLiteConnection(connection_string))
                {
                    connect.Open();
                    string command_text = "INSERT INTO [Users] ([login], [pass]) VALUES(@LOG, @PASS)";

                    SQLiteCommand command = new SQLiteCommand(command_text, connect);
                    command.Parameters.AddWithValue("@LOG", user.Login);
                    command.Parameters.AddWithValue("@PASS", user.Pass);
                    command.ExecuteNonQuery();
                    connect.Close();

                }
                int id_user = 0;
                using (SQLiteConnection connect = new SQLiteConnection(connection_string))
                {
                    connect.Open();
                    string command_text = "SELECT * FROM [Users] WHERE login = @LOG AND pass = @PASS";
                    SQLiteCommand command = new SQLiteCommand(command_text, connect);
                    command.Parameters.AddWithValue("@LOG", user.Login);
                    command.Parameters.AddWithValue("@PASS", user.Pass);
                    SQLiteDataReader sqlreader = command.ExecuteReader();
                    List<string> me_user = new List<string>();
                    while (sqlreader.Read())
                    {
                        me_user.Add(Convert.ToString(sqlreader["id"]));
                        me_user.Add(Convert.ToString(sqlreader["login"]));
                        me_user.Add(Convert.ToString(sqlreader["pass"]));
                    }
                    id_user = Convert.ToInt32(me_user[0]);
                    connect.Close();
                }
                using (SQLiteConnection connect = new SQLiteConnection(connection_string))
                {
                    connect.Open();
                    string command_text = "INSERT INTO [Objects] ([email], [id_owner]) VALUES (@MAIL, @ID)";
                    SQLiteCommand command = new SQLiteCommand(command_text, connect);
                    command.Parameters.AddWithValue("@MAIL", email);
                    command.Parameters.AddWithValue("@ID", id_user);
                    command.ExecuteNonQuery();
                    connect.Close();
                    MessageBox.Show("Регистрация пройдена успешно");
                }
                
            }
            else
            {
                MessageBox.Show("Ошибки в заполнении формы регистрации");
            }
        }

        private void Button_Enter_Click(object sender, RoutedEventArgs e)
        {
            Authrisation authrisation = new Authrisation();
            authrisation.Show();
            Hide();
        }
    }
}     
    

