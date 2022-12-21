using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SQLite;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace tyaf
{
    public partial class Authrisation : Window
    {
        private static string db_link = System.IO.Directory.GetCurrentDirectory() + @"\tyafbaza).db";
        private static string connection_string = @"Data Source= " + db_link + ";Version=3;";
        public Authrisation()
        {

            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }

        private void Button_Click_Vhod(object sender, RoutedEventArgs e)
        {
            string login = textboxlogin.Text.Trim();
            string password = passbox.Password.Trim();
            int id_user = 0;
            if (login.Length < 5)
            {
                textboxlogin.ToolTip = "Слишком короткий логин, минимум 5 символов";
                textboxlogin.Background = Brushes.DarkRed;
            }
            else
            {
                textboxlogin.Background = Brushes.Transparent;
            }
            if (password.Length < 5)
            {
                passbox.ToolTip = "Слишком короткий пароль, минимум 5 символов";
                passbox.Background = Brushes.DarkRed;
            }
            else
            {
                passbox.Background = Brushes.Transparent;
            }
            if (passbox.Background == Brushes.Transparent && textboxlogin.Background == Brushes.Transparent)
            {
                User authUser = new User();
                using (SQLiteConnection Connect = new SQLiteConnection(connection_string))
                {
                    Connect.Open();
                    string CommandText = @"SELECT * FROM [Users] WHERE [login] = @log AND [pass] = @pass";
                    SQLiteCommand Command = new SQLiteCommand(CommandText, Connect);
                    Command.Parameters.AddWithValue("@log", login);
                    Command.Parameters.AddWithValue("@pass", password);
                    Command.ExecuteNonQuery();
                    SQLiteDataReader sqlReader = Command.ExecuteReader();
                    while (sqlReader.Read())
                    {
                        authUser.id_user = Convert.ToInt32(sqlReader["id"]);
                        authUser.Login = (string)sqlReader["login"];
                        authUser.Pass = (string)sqlReader["pass"];
                    }
                    Connect.Close();
                    id_user = authUser.id_user;
                }
                if (authUser != null && id_user != 0)
                {
                    spisok_chelikov spisok_Chelikov = new spisok_chelikov(id_user);
                    spisok_Chelikov.Show();
                    Hide();
                }
                else
                    MessageBox.Show("Авторизация невозможна!\nТакого пользователя не существует");
            }
            else
            {
                MessageBox.Show("Авторизация невозможна!\nНекорректный ввод данных");
            }

        }



        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            Hide();
        }
    }
}

