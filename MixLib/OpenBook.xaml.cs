using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
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
using MaterialDesignThemes.Wpf;

namespace MixLib
{
    /// <summary>
    /// Логика взаимодействия для OpenBook.xaml
    /// </summary>
    public partial class OpenBook : UserControl
    {
        public int currentset = 0;
        public string ConnectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        Book book;
        SearchBooks searchBooks;
        public string username;
        public static Books library = new Books();
        public static OpenBook THIS;

        public int UpdateComment()
        {

            return 0;
        }
        public OpenBook(Book book, SearchBooks searchBooks)
        {
            InitializeComponent();
            THIS = this;
            username = MainWindow.THIS.UsernameTop.Text.Trim();
            if (username == "admin")
            {
                RedStack.Visibility = Visibility.Visible;
            }
            this.searchBooks = searchBooks;
            string a = searchBooks.SearchTextBox.Text;
            this.book = book;
            BookTitle.Text = book.BookName;
            Author.Text = book.Author;
            Country.Text = book.Country;
            Year.Text = Convert.ToString(book.GodIzdaniya);
            Genre.Text = book.Genre;
            Disc.Text = book.Discription;
            string z = "pack://application:,,,/Resources/" + book.Picture;
            Picture.Source = new BitmapImage(new Uri(z));

            try
            {
                using (SqlConnection con = new SqlConnection(ConnectionString))
                {
                    con.Open();
                    string CmdString = "select AVG(value) from Rating where book_id =" + book.Id;
                    SqlCommand cmd = new SqlCommand(CmdString, con);
                    string value = cmd.ExecuteScalar().ToString();
                    if (value.Length > 3)
                        value = value.Substring(0, 3);
                    RateBlock.Text = value;
                }

                using (SqlConnection con = new SqlConnection(ConnectionString))
                {
                    con.Open();
                    string Cmd = "SELECT (select COUNT(*) from Rating where book_id = " + book.Id + " and value = 1)[1],"
                                 + "(select COUNT(*) from Rating where book_id = " + book.Id + " and value = 2)[2],"
                                 + "(select COUNT(*) from Rating where book_id = " + book.Id + " and value = 3)[3],"
                                 + "(select COUNT(*) from Rating where book_id = " + book.Id + " and value = 4)[4],"
                                 + "(select COUNT(*) from Rating where book_id = " + book.Id + " and value = 5)[5],"
                                 + "(select COUNT(*) from Rating where book_id = " + book.Id + ")[count]";
                    SqlCommand command = new SqlCommand(Cmd, con);
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows) // если есть данные
                    {
                        reader.Read();
                        Rating rating = new Rating(Convert.ToInt32(reader.GetValue(0)), Convert.ToInt32(reader.GetValue(1)), Convert.ToInt32(reader.GetValue(2)), Convert.ToInt32(reader.GetValue(3)), Convert.ToInt32(reader.GetValue(4)), Convert.ToInt32(reader.GetValue(5)));
                        if (rating.total == 0)
                        {
                            NoRatesPanel.Visibility = Visibility.Visible;
                            RatesPanel.Visibility = Visibility.Collapsed;
                        }
                        else
                        {
                            int[] array = new int[] { rating.five, rating.four, rating.three, rating.two, rating.one };
                            int[] sorted = (from element in array orderby element ascending select element).ToArray();
                            double part = 150 / (double)sorted[4];
                            Five.Width = (double)rating.five * part;
                            Four.Width = (double)rating.four * part;
                            Three.Width = (double)rating.three * part;
                            Two.Width = (double)rating.two * part;
                            One.Width = (double)rating.one * part;
                        }
                    }
                    reader.Close();
                }
            }
            catch (Exception err)
            {
                NoRatesPanel.Visibility = Visibility.Visible;
                RatesPanel.Visibility = Visibility.Collapsed;
            }

            try
            {
                using (SqlConnection con = new SqlConnection(ConnectionString))
                {
                    con.Open();
                    string CmdString = "select username, comment, value  from Rating where book_id = " + book.Id;
                    SqlCommand cmd = new SqlCommand(CmdString, con);
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows) // если есть данные
                        while (reader.Read())
                        {
                            CommentItem item = new CommentItem(Convert.ToString(reader.GetValue(0)), Convert.ToString(reader.GetValue(1)), Convert.ToInt32(reader.GetValue(2)));
                            CommentPanel.Children.Add(item);
                        }
                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void AddToMyBookBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (SqlConnection con2 = new SqlConnection(ConnectionString))
                {
                    con2.Open();
                    string CmdString = "Insert into Book_" + username + "(id_book) Values (" + book.Id + ")";
                    SqlCommand cmd2 = new SqlCommand(CmdString, con2);
                    cmd2.ExecuteNonQuery();

                    AddOk.IsOpen = true;
                }

            }
            catch (Exception)
            {
                AddFail.IsOpen = true;
            }
        }

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.THIS.ContentGrid.Children.Clear();
            MainWindow.THIS.ContentGrid.Children.Add(searchBooks);
        }

        private void RedBtn_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.THIS.ContentGrid.Children.Clear();
            MainWindow.THIS.ContentGrid.Children.Add(new OpenRedBook(book, searchBooks));
        }

        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            Books library = new Books();
            Book booker = library.MyBooks.Find(book.Id);
            library.MyBooks.Remove(booker);
            library.SaveChanges();
            DelOk.IsOpen = true;
        }
        private void DeleteBtnOk_Click(object sender, RoutedEventArgs e)
        {
            SearchBooks searchBooks = new SearchBooks();
            foreach (Book n in library.MyBooks)
            {
                BookItem bookItem = new BookItem(n);
                searchBooks.ListBookWrapPanel.Children.Add(bookItem);
            }
            MainWindow.THIS.ContentGrid.Children.Clear();
            MainWindow.THIS.ContentGrid.Children.Add(searchBooks);
        }

        private void DeleteBtnFirst_Click(object sender, RoutedEventArgs e)
        {
            DelSure.IsOpen = true;
        }

        private void Button_MouseEnter(object sender, MouseEventArgs e)
        {
            var x = Convert.ToInt32((sender as Button).Uid);
            for (int i = 1; i <= x; i++)
            {
                PackIcon z = Dev.FindChild<PackIcon>(THIS, "Icon_" + Convert.ToString(i));
                if (x == 1)
                    z.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFF4500"));
                else if (x == 2)
                    z.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFA500"));
                else if (x == 3)
                    z.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF9ACD32"));
                else if (x == 4)
                    z.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF228B22"));
                else if (x == 5)
                    z.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF32CD32"));
            }

            for (int i = x + 1; i <= 5; i++)
            {
                PackIcon z = Dev.FindChild<PackIcon>(THIS, "Icon_" + Convert.ToString(i));
                z.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF44404D"));
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            currentset = Convert.ToInt32((sender as Button).Uid);
            for (int i = 1; i <= currentset; i++)
            {
                PackIcon z = Dev.FindChild<PackIcon>(THIS, "Icon_" + Convert.ToString(i));
                if (currentset == 1)
                    z.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFF4500"));
                else if (currentset == 2)
                    z.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFA500"));
                else if (currentset == 3)
                    z.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF9ACD32"));
                else if (currentset == 4)
                    z.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF228B22"));
                else if (currentset == 5)
                    z.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF32CD32"));
            }
        }

        private void Button_MouseLeave(object sender, MouseEventArgs e)
        {
            for (int i = 1; i <= currentset; i++)
            {
                PackIcon z = Dev.FindChild<PackIcon>(THIS, "Icon_" + Convert.ToString(i));
                if (currentset == 1)
                    z.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFF4500"));
                else if (currentset == 2)
                    z.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFA500"));
                else if (currentset == 3)
                    z.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF9ACD32"));
                else if (currentset == 4)
                    z.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF228B22"));
                else if (currentset == 5)
                    z.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF32CD32"));
            }

            for (int i = currentset + 1; i <= 5; i++)
            {
                PackIcon z = Dev.FindChild<PackIcon>(THIS, "Icon_" + Convert.ToString(i));
                z.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF44404D"));
            }
        }
        private void SendRateBtn_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                using (SqlConnection con2 = new SqlConnection(ConnectionString))
                {
                    con2.Open();
                    string CmdString = "insert into Rating(username, book_id, value, comment)values('" + username + "'," + book.Id + "," + currentset + ",'" + Comment.Text + "')";
                    SqlCommand cmd2 = new SqlCommand(CmdString, con2);
                    cmd2.ExecuteNonQuery();
                    AddComOk.IsOpen = true;
                    CommentPanel.Children.Add(new CommentItem(username, Comment.Text, currentset));
                    Comment.Text = "";
                    for (int i = 1; i <= 5; i++)
                    {
                        PackIcon z = Dev.FindChild<PackIcon>(THIS, "Icon_" + Convert.ToString(i));
                        z.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF44404D"));
                    }
                }
            }
            catch (Exception)
            {
                AddComFail.IsOpen = true;
            }
            try
            {
                using (SqlConnection con = new SqlConnection(ConnectionString))
                {
                    con.Open();
                    string CmdString = "select AVG(value) from Rating where book_id =" + book.Id;
                    SqlCommand cmd = new SqlCommand(CmdString, con);
                    string value = cmd.ExecuteScalar().ToString();
                    if (value.Length > 3)
                        value = value.Substring(0, 3);
                    RateBlock.Text = value;
                }

                using (SqlConnection con = new SqlConnection(ConnectionString))
                {
                    con.Open();
                    string Cmd = "SELECT (select COUNT(*) from Rating where book_id = " + book.Id + " and value = 1)[1],"
                                 + "(select COUNT(*) from Rating where book_id = " + book.Id + " and value = 2)[2],"
                                 + "(select COUNT(*) from Rating where book_id = " + book.Id + " and value = 3)[3],"
                                 + "(select COUNT(*) from Rating where book_id = " + book.Id + " and value = 4)[4],"
                                 + "(select COUNT(*) from Rating where book_id = " + book.Id + " and value = 5)[5],"
                                 + "(select COUNT(*) from Rating where book_id = " + book.Id + ")[count]";
                    SqlCommand command = new SqlCommand(Cmd, con);
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows) // если есть данные
                    {
                        reader.Read();
                        Rating rating = new Rating(Convert.ToInt32(reader.GetValue(0)), Convert.ToInt32(reader.GetValue(1)), Convert.ToInt32(reader.GetValue(2)), Convert.ToInt32(reader.GetValue(3)), Convert.ToInt32(reader.GetValue(4)), Convert.ToInt32(reader.GetValue(5)));
                        if (rating.total == 0)
                            Five.Width = Four.Width = Three.Width = Two.Width = One.Width = 0;
                        else
                        {
                            int[] array = new int[] { rating.five, rating.four, rating.three, rating.two, rating.one };
                            int[] sorted = (from element in array orderby element ascending select element).ToArray();
                            double part = 175 / (double)sorted[4];
                            Five.Width = (double)rating.five * part;
                            Four.Width = (double)rating.four * part;
                            Three.Width = (double)rating.three * part;
                            Two.Width = (double)rating.two * part;
                            One.Width = (double)rating.one * part;
                        }
                    }
                    reader.Close();
                    if (NoRatesPanel.Visibility == Visibility.Visible)
                    {
                        NoRatesPanel.Visibility = Visibility.Collapsed;
                        RatesPanel.Visibility = Visibility.Visible;
                    }
                }
            }
            catch (Exception err)
            {
                NoRatesPanel.Visibility = Visibility.Visible;
                RatesPanel.Visibility = Visibility.Collapsed;
            }
        }
    }
}
