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
using System.Data.SQLite;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace tyaf
{
    public partial class DeleteObjectWindow : Window
    {
        private int Id_objects;
        private static string db_link = System.IO.Directory.GetCurrentDirectory() + @"\tyafbaza).db";
        private static string connection_string = @"Data Source= " + db_link + ";Version=3;";
        public DeleteObjectWindow()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }
        private void Button_Click_Accept(object sender, RoutedEventArgs e)
        {
            try
            {
                Id_objects = Convert.ToInt32(textBoxNumber.Text);
                using (SQLiteConnection Connect = new
                SQLiteConnection(connection_string))
                {
                    string commandText = "DELETE FROM [Objects] WHERE id_object = @id";
                    SQLiteCommand Command = new SQLiteCommand(commandText, Connect);
                    Command.Parameters.AddWithValue("@id", Id_objects);
                    Connect.Open();
                    Command.ExecuteNonQuery();
                    Connect.Close();
                }
                MessageBox.Show("Успешно удалено!");
                Hide();
            }
            catch
            {
                MessageBox.Show("Номер введён некорректно");
            }
        }
        private void Button_Click_Cancel(object sender, RoutedEventArgs e)
        {
            Hide();
        }
    }
}
