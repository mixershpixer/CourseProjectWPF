using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Threading;
using MaterialDesignThemes.Wpf;

namespace MixLib
{
    /// <summary>
    /// Логика взаимодействия для Registration.xaml
    /// </summary>
    public partial class Registration : Window
    {
        public string ConnectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        public SolidColorBrush color = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF9800"));
        User user;
        public Registration()
        {
            InitializeComponent();
            Uri iconUri = new Uri(@"ML2.png", UriKind.RelativeOrAbsolute);
            this.Icon = BitmapFrame.Create(iconUri);
        }
        [STAThread]
        public bool RegMethod(string name, string surname, string username, string password)
        {
            bool res = false;
            Thread.Sleep(1500);
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                SqlCommand cmd = new SqlCommand("AddUser", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@name", name);
                cmd.Parameters.AddWithValue("@surname", surname);
                cmd.Parameters.AddWithValue("@username", username);
                cmd.Parameters.AddWithValue("@password", password);
                cmd.ExecuteNonQuery();
                user = new User(name, surname, username);
                using (SqlConnection con2 = new SqlConnection(ConnectionString))
                {
                    con2.Open();
                    string CmdString = "Create table Book_" + username + "(id_book int primary key)";
                    SqlCommand cmd2 = new SqlCommand(CmdString, con2);
                    if (cmd2.ExecuteNonQuery() == -1)
                    {
                        res = true;
                    };
                }
            }
            return res;
        }
        private async void RegistrationButton_Click(object sender, RoutedEventArgs e)
        {
            bool[] t1 = { RegNameTxt.Text.Equals(""), RegSurnameTxt.Text.Equals(""), RegUserNameTxt.Text.Equals(""), RegPasswordTxt.Password.Equals("") };
            PackIcon[] t2 = { RegName, RegSurname, RegUserName, RegPassword };
            bool AllOk = true;
            bool Res = false;
            ProgressBar1.Visibility = Visibility.Visible;
            int i = 0;
            try
            {
                for (; i < 4; i++)
                    if (t1[i])
                    {
                        t2[i].Foreground = Brushes.Red;
                        break;
                    }
                if (i == 4)
                {
                    string name = RegNameTxt.Text.Trim();
                    string surname = RegSurnameTxt.Text.Trim();
                    string username = RegUserNameTxt.Text.Trim();
                    string password = RegPasswordTxt.Password.Trim();
                    Res = await Task.Run(() =>
                    {
                        try { return RegMethod(name, surname, username, password); }
                        catch { throw new Exception(); }
                    });
                }
            }
            catch(Exception es)
            {
                MessageBox.Show(es.Message); RegUserName.Foreground = Brushes.Red; AllOk = false; }
            ProgressBar1.Visibility = Visibility.Collapsed;
            if (AllOk && (i == 4)) Complite.IsOpen = true;
            else if (!AllOk) RegFail.IsOpen = !Res;
        }
        private void ReadyBtn_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow(user);
            mainWindow.Show();
            this.Close();
        }

        private void RegSurnameTxt_GotFocus(object sender, RoutedEventArgs e)
        {
            RegSurname.Foreground = color;
        }

        private void RegNameTxt_GotFocus(object sender, RoutedEventArgs e)
        {
            RegName.Foreground = color;
        }

        private void RegUserNameTxt_GotFocus(object sender, RoutedEventArgs e)
        {
            RegUserName.Foreground = color;
        }

        private void RegPasswordTxt_GotFocus(object sender, RoutedEventArgs e)
        {
            RegPassword.Foreground = color;
        }

        private void BackToLoginButton_Click(object sender, RoutedEventArgs e)
        {
            Autorization autorization = new Autorization();
            this.Close();
            autorization.Show();
        }
    }
}
