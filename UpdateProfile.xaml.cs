using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using System.Data.SQLite;

namespace tyaf
{
    /// <summary>
    /// Логика взаимодействия для UpdateWindow.xaml
    /// </summary>
    public partial class UpdateProfile : Window
    {
        private int id_object, id_user;
        private static string db_link = System.IO.Directory.GetCurrentDirectory() + @"\tyafbaza).db";
        private static string connection_string = @"Data Source= " + db_link + ";Version=3;";

        public UpdateProfile(int id_obj, int id_user)
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            this.id_object = id_obj;
            this.id_user = id_user;
        }



        private void Button_Click_Save(object sender, RoutedEventArgs e)
        {
            string last_name = textBoxLastName.Text.Trim();
            string first_name = textBoxFirstName.Text.Trim();
            string phone = textBoxPhone.Text.Trim();
            string email = textBoxEmail.Text.Trim();
            string faculty = textBoxFaculty.Text.Trim();
            string group = textBoxGroup.Text.Trim();
            try
            {
                if (last_name != "")
                {
                    Objects objects = new Objects();
                    objects.id_object = Convert.ToInt32(id_object);
                    objects.Last_name = last_name;
                    SetLastName(objects);
                }
            }
            catch
            {
                MessageBox.Show("Фамилия введена некорректно");
            }
            try
            {
                if (first_name != "")
                {
                    Objects objects = new Objects();
                    objects.id_object = Convert.ToInt32(id_object);
                    objects.First_name = first_name;
                    SetFirstName(objects);
                }
            }
            catch
            {
                MessageBox.Show("Имя введено некорректно");
            }
            try
            {
                if (phone != "")
                {
                    Objects objects = new Objects();
                    objects.id_object = Convert.ToInt32(id_object);
                    objects.Phone = Convert.ToInt64(phone);
                    SetPhone(objects);
                }
                else
                {
                    textBoxPhone.ToolTip = "Телефон должен содержать 11 цифр";
                    textBoxPhone.Background = Brushes.LightCoral;
                }
            }
            catch
            {
                MessageBox.Show("Телефон введен некорректно");
            }
            try
            {
                if (faculty != "")
                {
                    Objects objects = new Objects();
                    objects.id_object = Convert.ToInt32(id_object);
                    objects.Faculty = faculty;
                    SetFaculty(objects);
                }
            }
            catch
            {
                MessageBox.Show("Факультет введен некорректно");
            }
            try
            {
                if (group != "")
                {
                    Objects objects = new Objects();
                    objects.id_object = Convert.ToInt32(id_object);
                    objects.Stud_group = group;
                    SetGroup(objects);
                }
            }
            catch
            {
                MessageBox.Show("Группа введена некорректно");
            }
            spisok_chelikov spisok_Chelikov = new spisok_chelikov(id_user);
            spisok_Chelikov.Show();
            Hide();

        }
        private void Button_Click_Back(object sender, RoutedEventArgs e)
        {
            spisok_chelikov spisok_Chelikov = new spisok_chelikov(id_user);
            spisok_Chelikov.Show();
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
        private int SetLastName(Objects objects)
        {
            const string query = "UPDATE Objects SET second_name = @lastName " + "WHERE id_object = @id";

            var args = new Dictionary<string, object>
            {
            {"@lastName", objects.Last_name },
            {"@id", objects.id_object }
            };
            return ExecuteWrite(query, args);
        }
        private int SetFirstName(Objects objects)
        {
            const string query = "UPDATE Objects SET first_name = @firstName " +
            "WHERE id_object = @id";
            var args = new Dictionary<string, object>
            {
            {"@firstName", objects.First_name },
            {"@id", objects.id_object }
            };
            return ExecuteWrite(query, args);
        }
        private int SetPhone(Objects objects)
        {
            const string query = "UPDATE Objects SET phone = @Phone " +
            "WHERE id_object = @id";
            var args = new Dictionary<string, object>
            {
            {"@Phone", objects.Phone },
            {"@id", objects.id_object }
            };
            return ExecuteWrite(query, args);
        }
        private int SetFaculty(Objects objects)
        {
            const string query = "UPDATE Objects SET faculty = @Faculty " + "WHERE id_object = @id";
            var args = new Dictionary<string, object>
            {
            {"@Faculty", objects.Faculty },
            {"@id", objects.id_object }
            };
            return ExecuteWrite(query, args);

        }
        private int SetGroup(Objects objects)
        {
            const string query = "UPDATE Objects SET stud_group = @Group " + "WHERE id_object = @id";
            var args =
            new Dictionary<string, object>
            {
            {"@Group", objects.Stud_group },
            {"@id", objects.id_object }
            };
            return ExecuteWrite(query, args);
        }
    }

    internal class Objects
    {
       
        public int id_object { get; internal set; }
        public string Last_name { get; internal set; }
        public string First_name { get; internal set; }
        public long Phone { get; internal set; }
        public string Faculty { get; internal set; }
        public string Stud_group { get; internal set; }
        public object Email { get; internal set; }
        public object id_owner { get; internal set; }
    }
}