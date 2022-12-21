using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Data.Entity.Core.Objects;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;


namespace tyaf
{
    /// <summary>
    /// Логика взаимодействия для spisok_chelikov.xaml
    /// </summary>
    public partial class spisok_chelikov : Window
    {
        private int Id_user;
        private static string db_link = System.IO.Directory.GetCurrentDirectory() + @"\tyafbaza).db";
        private static string connection_string = @"Data Source= " + db_link + ";Version=3;";

        public spisok_chelikov(int id_user)
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            this.Id_user = id_user;
            //показ именно тех объектов данных, которые относятся к конкретному пользователю
            List<Objects> objectParameters = new List<Objects>(); //лист объектов
            List<int> id_objects = new List<int>();
            List<string> mails = new List<string>();
            List<string> fr_names = new List<string>();
            List<string> sc_names = new List<string>();
            List<long> phones = new List<long>();
            List<string> faculties = new List<string>();
            List<string> stud_groups = new List<string>();
            List<int> id_owners = new List<int>();

            using (SQLiteConnection Connect = new SQLiteConnection(connection_string))
            {
                Connect.Open();
                string CommandText = @"SELECT * FROM [Objects] WHERE [id_owner] = @id";
                SQLiteCommand Command = new SQLiteCommand(CommandText, Connect);
                Command.Parameters.AddWithValue("@id", Id_user);
                Command.ExecuteNonQuery();
                SQLiteDataReader sqlReader = Command.ExecuteReader();

                while (sqlReader.Read())
                {
                    id_objects.Add(Convert.ToInt32(sqlReader["id_object"]));
                    mails.Add(Convert.ToString(sqlReader["email"]));
                    fr_names.Add(sqlReader["first_name"] == DBNull.Value ? "" :
                    Convert.ToString(sqlReader["first_name"]));
                    sc_names.Add(sqlReader["second_name"] == DBNull.Value ? "" :
                    Convert.ToString(sqlReader["second_name"]));
                    phones.Add(sqlReader["phone"] == DBNull.Value ? 0 :
                    Convert.ToInt64(sqlReader["phone"]));
                    faculties.Add(sqlReader["faculty"] == DBNull.Value ? "" :
                    Convert.ToString(sqlReader["faculty"]));
                    stud_groups.Add(sqlReader["stud_group"] == DBNull.Value ? "" :
                    Convert.ToString(sqlReader["stud_group"]));
                    id_owners.Add(sqlReader["id_owner"] == DBNull.Value ? 0 :
                    Convert.ToInt32(sqlReader["id_owner"]));
                }
                Connect.Close();
            }
            for (int i = 0; i < mails.Count; i++)
            {
                Objects objects = new Objects();
                objects.id_object = id_objects[i];
                objects.Email = mails[i];
                objects.First_name = fr_names[i];
                objects.Last_name = sc_names[i];
                objects.Phone = phones[i];
                objects.Faculty = faculties[i];
                objects.Stud_group = stud_groups[i];
                objects.id_owner = id_owners[i];
                objectParameters.Add(objects);
            }
            listOfParameters.ItemsSource = objectParameters;
        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            UpdateWindow updateWindow = new UpdateWindow(Id_user);
            updateWindow.ShowDialog();
            Hide();
        }

        private void Button_Click_Delete_Objects(object sender, RoutedEventArgs e)
        {
            DeleteObjectWindow deleteObjectWindow = new DeleteObjectWindow();
            deleteObjectWindow.ShowDialog();
            Hide();
            spisok_chelikov spisok_Chelikov = new spisok_chelikov(Id_user);
            spisok_Chelikov.Show();
        }

        private void Button_Click_Delete_Profile(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = System.Windows.MessageBox.Show("Удалить профиль? ", "Подтвердите действие", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {

                User user = new User();
                user.id_user = Id_user;
                Objects objects = new Objects();
                objects.id_owner = Id_user;
                DeleteAllObjects(objects);
                DeleteProfile(user);
                Authrisation authrisation = new Authrisation();
                authrisation.Show();
                Hide();
            }
            else
            {
                result.CompareTo(MessageBoxResult.No);
            }
        }


        private void Button_Click_AuthWindow (object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            Hide();
        }

        private int ExecuteWrite(string query, Dictionary<string, object> args)
        {
            int numberOfRowsAffected;
            using (var con = new SQLiteConnection(connection_string))
            {
                con.Open();
                using (var cmd = new SQLiteCommand(query, con))
                {
                    foreach (var pair in args)
                    {
                        cmd.Parameters.AddWithValue(pair.Key, pair.Value);
                    }
                    numberOfRowsAffected = cmd.ExecuteNonQuery();

                }
                return numberOfRowsAffected;
            }
        }
        private int DeleteAllObjects(Objects objects)
        {
            const string query = "DELETE FROM Objects WHERE id_owner = @id";
            var args = new Dictionary<string, object> {
{"@id", objects.id_owner}
};
            return ExecuteWrite(query, args);
        }
        private int DeleteProfile(User user)
        {
            const string query = "DELETE FROM Users WHERE id = @id";
            var args = new Dictionary<string, object>
{
{"@id", user.id_user }
};
            return ExecuteWrite(query, args);
        }


        private void Button_Click_New_Objects(object sender, RoutedEventArgs e)
        {
            NewObjects newObjects = new NewObjects(Id_user);
            newObjects.Show();
            Hide();
        }


        internal class Objects
        {
            public int id_object { get; internal set; }
            public string email { get; internal set; }
            public string Email { get; internal set; }
            public string First_name { get; internal set; }
            public string Last_name { get; internal set; }
            public long Phone { get; internal set; }
            public string Faculty { get; internal set; }
            public string Stud_group { get; internal set; }
            public int id_owner { get; internal set; }
        }
    }
}