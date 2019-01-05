using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Логика взаимодействия для OpenRedBook.xaml
    /// </summary>
    public partial class OpenRedBook : UserControl
    {
        public string ConnectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        public static Books library = new Books();
        SearchBooks searchBooks;
        Book book;
        public string filename;
        public int index;
        public OpenRedBook(Book book, SearchBooks searchBooks)
        {
            InitializeComponent();
            this.book = book;
            this.searchBooks = searchBooks;
            BookTitle.Text = book.BookName;
            Author.Text = book.Author;
            Country.Text = book.Country;
            Year.Text = Convert.ToString(book.GodIzdaniya);
            Genre.Text = book.Genre;
            Disc.Text = book.Discription;
            string z = "pack://application:,,,/Resources/" + book.Picture;
            this.PictureBox.Source = new BitmapImage(new Uri(z));
        }

        private void ChangeImage_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.DefaultExt = ".jpg";
            dlg.Filter = "JPG Files (*.jpg)|*.jpg|JPEG Files (*.jpeg)|*.jpeg|PNG Files (*.png)|*.png|GIF Files (*.gif)|*.gif";
            bool? result = dlg.ShowDialog();
            if (result == true)
            {
                filename = dlg.FileName;

                for (int i = 0; i < filename.Length; i++)
                {
                    if (filename[i] == '\\')
                    {
                        index = i;
                    }
                }
                filename = filename.Remove(0, index + 1);
                string z = "pack://application:,,,/Resources/" + filename;
                this.PictureBox.Source = new BitmapImage(new Uri(z));
            }
        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            int index = 0;
            int outyear;
            string picture = Convert.ToString(this.PictureBox.Source);
            for (int i = 0; i < picture.Length; i++)
            {
                if (picture[i] == '/')
                {
                    index = i;
                }
            }
            picture = picture.Remove(0, index + 1);

            Books library = new Books();

            var booker = library.MyBooks.FirstOrDefault(c => c.Id == book.Id);
            bool result = Int32.TryParse(Year.Text, out outyear);
            if (result && booker != null)
            {

                booker.BookName = BookTitle.Text;
                booker.Author = Author.Text;
                booker.GodIzdaniya = outyear;
                booker.GodIzdaniya = Convert.ToInt32(Year.Text);
                booker.Discription = Disc.Text;
                booker.Picture = picture;
                booker.Country = Country.Text;
                booker.Genre = Genre.Text;

                library.SaveChanges();
                UpdateOk.IsOpen = true;
            }
            else
            {
                Fail.IsOpen = true;
                Year.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF53434"));
            }
        }

        private void ComeBack_Click(object sender, RoutedEventArgs e)
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

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            //SearchBooks searchBooks = new SearchBooks();
            //foreach (Book n in library.MyBooks)
            //{
            //    BookItem bookItem = new BookItem(n);
            //    searchBooks.ListBookWrapPanel.Children.Add(bookItem);
            //}
            //MainWindow.THIS.ContentGrid.Children.Clear();
            //MainWindow.THIS.ContentGrid.Children.Add(searchBooks);
            MainWindow.THIS.ContentGrid.Children.Clear();
            MainWindow.THIS.ContentGrid.Children.Add(new OpenBook(book, searchBooks));
        }

        private void Year_GotFocus(object sender, RoutedEventArgs e)
        {
            Year.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF706C65"));
        }

        private void Disc_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !("0123456789 ,".IndexOf(e.Text) < 0);
        }
    }
}
