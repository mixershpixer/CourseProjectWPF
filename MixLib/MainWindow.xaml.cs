using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;

namespace MixLib
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static MainWindow THIS;
        User user;
        public static Books library = new Books();
        public SolidColorBrush color = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#7E57C2"));
        public string ConnectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

        [STAThread]
        public static List<Book> Founder(IQueryable<Book> LinqName)
        {
            List<Book> list = new List<Book>();
            foreach (Book book in LinqName)
            {
                list.Add(
                    new Book(
                        book.Id,
                        book.BookName,
                        book.Author,
                        book.GodIzdaniya,
                        book.Country,
                        book.Discription,
                        book.Picture,
                        book.Genre
                    )
                );
            }
            return list;
        }
        //book.BookName, book.Author, book.GodIzdaniya, book.Country, book.Discription, book.Picture, book.Genre

        public MainWindow(User user)
        {
            InitializeComponent();
            THIS = SettingsWindow.Window = this;
            Uri iconUri = new Uri(@"ML2.png", UriKind.RelativeOrAbsolute);
            this.Icon = BitmapFrame.Create(iconUri);
            this.user = user;
            NameTop.Text = user.name;
            SurnameTop.Text = user.surname;
            UserFirstLetter.Text = (UsernameTop.Text = user.username).Substring(0, 1).ToUpper();
            if (user.username.Equals("admin"))
                admin.Visibility = Visibility.Visible;
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
        private void OpenMenuButton_Click(object sender, RoutedEventArgs e)
        {
            OpenMenuButton.Visibility = Visibility.Collapsed;
            CloseMenuButton.Visibility = UserInfo.Visibility = Visibility.Visible;
        }

        private void CloseMenuButton_Click(object sender, RoutedEventArgs e)
        {
            OpenMenuButton.Visibility = Visibility.Visible;
            CloseMenuButton.Visibility = UserInfo.Visibility = Visibility.Collapsed;
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private async void MenuListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = MenuListView.SelectedIndex;
            if (index == -1) return;
            if (GridMenu.Width != 60)
            {
                OpenMenuButton.Visibility = Visibility.Visible;
                UserInfo.Visibility = CloseMenuButton.Visibility = Visibility.Collapsed;
                DoubleAnimation buttonAnimation = new DoubleAnimation
                {
                    From = GridMenu.ActualWidth,
                    To = 60,
                    Duration = TimeSpan.FromSeconds(0.2),
                    FillBehavior = FillBehavior.Stop
                };
                GridMenu.BeginAnimation(Button.WidthProperty, buttonAnimation);
                Storyboard sb = new Storyboard();
                ThicknessAnimation commGridAnimation = new ThicknessAnimation
                {
                    From = new Thickness(200, 0, 0, 0),
                    To = new Thickness(0, 0, 0, 0),
                    FillBehavior = FillBehavior.Stop,
                    Duration = TimeSpan.FromSeconds(0.3)
                };
                Storyboard.SetTarget(commGridAnimation, ContentGrid);
                Storyboard.SetTargetProperty(commGridAnimation, new PropertyPath(MarginProperty));
                sb.Children.Add(commGridAnimation);
                sb.Begin();
            }
            switch (index)
            {
                case 0:
                    ContentGrid.Children.Clear();
                    ContentGrid.Children.Add(new MyAccount(user));
                    break;
                case 1:
                    MyBooks myBooks = new MyBooks();
                    ProgressBar1.Visibility = Visibility.Visible;
                    using (SqlConnection con2 = new SqlConnection(ConnectionString))
                    {
                        con2.Open();
                        string sqlExpression = "select id_book from Book_" + user.username;
                        SqlCommand command = new SqlCommand(sqlExpression, con2);
                        SqlDataReader reader = command.ExecuteReader();
                        List<int> id_list = new List<int>();
                        if (reader.HasRows)
                            while (reader.Read())
                                id_list.Add(Convert.ToInt32(reader.GetValue(0)));
                        reader.Close();
                        if (id_list.Count() != 0)
                        {
                            List<Book> Spisok = await Task.Run(() => Founder(library.MyBooks));
                            for (int i = 0; i < id_list.Count; i++)
                            {
                                var spc = from n in Spisok
                                          where n.Id == id_list[i]
                                          select n;
                                foreach (Book n in spc)
                                {
                                    myBooks.MyBookWrapPanel.Children.Add(new MyBookItem(n));
                                }
                            }
                            ContentGrid.Children.Clear();
                            ContentGrid.Children.Add(myBooks);
                        }
                        else
                            Empty.IsOpen = true;
                        ProgressBar1.Visibility = Visibility.Collapsed;
                    }
                    break;
                case 2:
                    SearchBooks searchBooks = new SearchBooks();
                    ProgressBar1.Visibility = Visibility.Visible;
                    List<Book> Res = await Task.Run(() => Founder(library.MyBooks));
                    ProgressBar1.Visibility = Visibility.Collapsed;
                    foreach (Book n in Res)
                    {
                        searchBooks.ListBookWrapPanel.Children.Add(new BookItem(n));
                    }
                    ContentGrid.Children.Clear();
                    ContentGrid.Children.Add(searchBooks);
                    break;
                case 3:
                    ContentGrid.Children.Clear();
                    ContentGrid.Children.Add(new AddNewBookAdmin());
                    break;
            }
        }

        private void LogOutBtn_Click(object sender, RoutedEventArgs e)
        {
            (new Autorization()).Show();
            Close();
        }

        private void AboutBtn_Click(object sender, RoutedEventArgs e)
        {
            MenuListView.SelectedIndex = -1;
            ContentGrid.Children.Clear();
            ContentGrid.Children.Add(new AboutPage());
        }

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            MenuListView.SelectedIndex = -1;
            ContentGrid.Children.Clear();
            ContentGrid.Children.Add(new SettingsWindow());
        }
    }
}
