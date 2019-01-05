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

namespace MixLib
{
    /// <summary>
    /// Логика взаимодействия для OpenMyBook.xaml
    /// </summary>
    /// 
    public partial class OpenMyBook : UserControl
    {
        Book book;
        public static Books library = new Books();

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
        public OpenMyBook()
        {
            InitializeComponent();
        }

        public OpenMyBook(Book book)
        {
            InitializeComponent();
            this.book = book;
            BookTitle.Text = book.BookName;
            Author.Text = book.Author;
            Country.Text = book.Country;
            Year.Text = Convert.ToString(book.GodIzdaniya);
            Genre.Text = book.Genre;
            Disc.Text = book.Discription;
            string z = "pack://application:,,,/Resources/" + book.Picture;
            this.Picture.Source = new BitmapImage(new Uri(z));
        }

        private async void BackToMy_Click(object sender, RoutedEventArgs e)
        {
            string usname = MainWindow.THIS.UsernameTop.Text;
            MyBooks myBooks = new MyBooks();
            using (SqlConnection con2 = new SqlConnection(ConnectionString))
            {
                con2.Open();
                string sqlExpression = "select id_book from Book_" + usname;
                SqlCommand command = new SqlCommand(sqlExpression, con2);
                SqlDataReader reader = command.ExecuteReader();
                List<int> id_list = new List<int>();
                if (reader.HasRows) // если есть данные
                {
                    while (reader.Read())
                    {
                        id_list.Add(Convert.ToInt32(reader.GetValue(0)));
                    }
                }
                reader.Close();
                List<Book> Spisok = await Task.Run(() => Founder(library.MyBooks));
                for (int i = 0; i < id_list.Count; i++)
                {
                    var spc = from n in Spisok
                              where n.Id == id_list[i]
                              select n;
                    foreach (Book n in spc)
                    {
                        MyBookItem mybookItem = new MyBookItem(n);
                        myBooks.MyBookWrapPanel.Children.Add(mybookItem);
                    }
                }
            }
            MainWindow.THIS.ContentGrid.Children.Clear();
            MainWindow.THIS.ContentGrid.Children.Add(myBooks);
        }

        private void ReadNow_OnClick(object sender, RoutedEventArgs e)
        {
            Read.IsOpen = true;
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            Donate.IsOpen = true;
        }
    }
}
