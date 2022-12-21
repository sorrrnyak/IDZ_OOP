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
    public partial class UpdateWindow : Window
    {
        private static string db_link = System.IO.Directory.GetCurrentDirectory() + @"\tyafbaza).db";
        private static string connection_string = @"Data Source= " + db_link + ";Version=3;";
        private int Id_objects, Id_user;
        public UpdateWindow(int id_user)
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            this.Id_user = id_user;
        }

        public UpdateWindow(int id_user, int id_user1) : this(id_user)
        {
        }

        private void Button_Click_Accept(object sender, RoutedEventArgs e)
        {
            try
            {
                this.Id_objects = Convert.ToInt32(textBoxNumber.Text);
                List<int> my_object = new List<int>();
                my_object.Add(0);
                using (SQLiteConnection Connect = new SQLiteConnection(connection_string))
                {
                    Connect.Open();
                    string CommandText = "SELECT * FROM [Objects] WHERE id_object = @id_obj AND id_owner = @id_own";
                    SQLiteCommand Command = new SQLiteCommand(CommandText, Connect);
                    Command.Parameters.AddWithValue("@id_obj", Id_objects);
                    Command.Parameters.AddWithValue("@id_own", Id_user);
                    SQLiteDataReader sqlReader = Command.ExecuteReader();
                    while (sqlReader.Read()) // считываем и вносим в лист все параметры
                    {
                        my_object[0] = Convert.ToInt32(sqlReader["id_object"]);
                    }
                    Connect.Close();
                }
                if (my_object[0] == Id_objects)
                {
                    UpdateProfile updateProfile = new UpdateProfile(Id_objects, Id_user);
                    updateProfile.Show();
                    Hide();
                }
                else
                {
                    MessageBox.Show("Такого пользователя не существует");
                }
            }
            catch
            {
                MessageBox.Show("Некорректный ввод номера студента. Попробуйте еще раз.");
            }
        }
        private void Button_Click_Cancel(object sender, RoutedEventArgs e)
        {
            spisok_chelikov spisok_Chelikov = new spisok_chelikov(Id_user);
            spisok_Chelikov.Show();
            Hide();
        }
    }
}
