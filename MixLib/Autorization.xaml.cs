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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Threading;
using MaterialDesignThemes.Wpf;

namespace MixLib
{
    /// <summary>
    /// Логика взаимодействия для Autorization.xaml
    /// </summary>
    public partial class Autorization : Window
    {
        public string ConnectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        public SolidColorBrush color = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF9800"));
        User user;

        [STAThread]
        public bool AutorizationMethod(string username, string password)
        {
            Thread.Sleep(1000);
            bool res = false;
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                SqlCommand cmd = new SqlCommand("Autorization", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@username", username);
                cmd.Parameters.AddWithValue("@password", password);
                int result = Convert.ToInt32(cmd.ExecuteScalar());
                if (result == 1)
                {
                    string sqlExpression = "SELECT name, surname, username FROM Users where username='" + username + "'";
                    SqlCommand command = new SqlCommand(sqlExpression, con);
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.HasRows) // если есть данные
                    {
                        reader.Read();
                        user = new User(Convert.ToString(reader.GetValue(0)), Convert.ToString(reader.GetValue(1)), Convert.ToString(reader.GetValue(2)));
                    }
                    reader.Close();
                    res = true;
                }
                else
                {
                    res = false;
                }
            }
            return res;
        }


        public Autorization()
        {
            Thread.Sleep(2000);
            InitializeComponent();
            Uri iconUri = new Uri(@"ML2.png", UriKind.RelativeOrAbsolute);
            this.Icon = BitmapFrame.Create(iconUri);
        }
        private async void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            string[] t1 = { UserNameTxt.Text, PasswordTxt.Password };
            PackIcon[] t2 = { Icon1, Icon2 };
            ProgressBar1.Visibility = Visibility.Visible;
            try
            {
                int i = 0;
                for (; i < 2; i++)
                    if (t1[i].Equals(""))
                    {
                        t2[i].Foreground = Brushes.Red;
                        break;
                    }
                if (i == 2)
                    if (await Task.Run(() => AutorizationMethod(t1[0], t1[1])))
                    {
                        (new MainWindow(user)).Show();
                        Close();
                    }
                    else throw new Exception();
            }
            catch
            {
                AutFail.IsOpen = true;
                t2[0].Foreground = t2[1].Foreground = Brushes.Red;
            }
            ProgressBar1.Visibility = Visibility.Collapsed;
        }

        private void CreateAccButton_Click(object sender, RoutedEventArgs e)
        {
            Registration registration = new Registration();
            this.Close();
            registration.Show();
        }

        private void UserNameTxt_GotFocus(object sender, RoutedEventArgs e)
        {
            Icon1.Foreground = color;
        }

        private void PasswordTxt_GotFocus(object sender, RoutedEventArgs e)
        {
            Icon2.Foreground = color;
        }
    }
}
