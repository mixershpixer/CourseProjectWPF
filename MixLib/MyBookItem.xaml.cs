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

namespace MixLib
{
    /// <summary>
    /// Логика взаимодействия для MyBookItem.xaml
    /// </summary>
    public partial class MyBookItem : UserControl
    {
        public SolidColorBrush color1 = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#F5F5F5"));
        public SolidColorBrush color2 = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E0E0E0"));

        public string RedDiscription;
        public string RedBooktitle;

        private readonly int bookid;
        private string bookName;
        private string author;
        private int godIzdaniya;
        private string country;
        private string discription;
        private string picture;
        private string genre;

        public string Obrezka(string stroka, int index)
        {
            if (stroka.Length > index)
            {
                int lValue = 0;
                for (int i = index; i > 0 && !stroka[i].Equals(' '); i--)
                    lValue++;
                return stroka.Substring(0, index - lValue) + "...";
            }
            return stroka;
        }
        public MyBookItem()
        {
            InitializeComponent();
        }
        public MyBookItem(Book book)
        {
            InitializeComponent();
            RedBooktitle = book.BookName;
            RedDiscription = book.Discription;

            this.bookid = book.Id;
            this.bookName = book.BookName;
            this.author = book.Author;
            this.godIzdaniya = book.GodIzdaniya;
            this.country = book.Country;
            this.discription = book.Discription;
            this.picture = book.Picture;
            this.genre = book.Genre;

            this.BookName.Text = Obrezka(bookName, 24);
            this.Author.Text = author;
            this.GodIzdaniya.Text = Convert.ToString(godIzdaniya);
            this.Country.Text = country;
            this.Discription.Text = Obrezka(discription, 250);
            this.Genre.Text = genre;
            string z = "pack://application:,,,/Resources/" + picture;
            this.PictureBox.Source = new BitmapImage(new Uri(z));
        }
        public MyBookItem(int id, string BookName, string Author, int GodIzdaniya, string Country, string Discription, string Picture, string Genre)
        {
            InitializeComponent();

            this.BookName.Text = BookName;
            this.Author.Text = Author;
            this.GodIzdaniya.Text = Convert.ToString(GodIzdaniya);
            this.Country.Text = Country;
            this.Discription.Text = Discription;
            this.Genre.Text = Genre;
            string z = "pack://application:,,,/Resources/" + Picture;
            this.PictureBox.Source = new BitmapImage(new Uri(z));
        }

        private void UserControl_MouseUp(object sender, MouseButtonEventArgs e)
        {
            int index = 0;
            string picture = Convert.ToString(this.PictureBox.Source);
            for (int i = 0; i < picture.Length; i++)
            {
                if (picture[i] == '/')
                {
                    index = i;
                }
            }
            picture = picture.Remove(0, index + 1);

            int id = this.bookid;
            string booktitle = this.BookName.Text;
            string author = this.Author.Text;
            int godizd = Convert.ToInt32(this.GodIzdaniya.Text);
            string country = this.Country.Text;
            string disc = this.Discription.Text;
            string genre = this.Genre.Text;
            Book book = new Book(id, RedBooktitle, author, godizd, country, RedDiscription, picture, genre);
            MainWindow.THIS.ContentGrid.Children.Clear();
            MainWindow.THIS.ContentGrid.Children.Add(new OpenMyBook(book));
        }

        private void Grid_MouseEnter(object sender, MouseEventArgs e)
        {
            background.Background = color2;
        }

        private void Grid_MouseLeave(object sender, MouseEventArgs e)
        {
            background.Background = color1;
        }
    }
}
