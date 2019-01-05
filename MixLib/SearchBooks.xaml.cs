using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MixLib
{
    /// <summary>
    /// Логика взаимодействия для SearchBooks.xaml
    /// </summary>
    /// 
    public partial class SearchBooks : UserControl
    {
        public static SearchBooks This;
        public static int SearchSelIndBack;
        public static string SearchTextBack;
        public static Books library = new Books();
        public SolidColorBrush color = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF9800"));
        public List<BookItem> booklist = new List<BookItem>();

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
        public void FounderTwo(IEnumerable<Book> LinqName)
        {
            if (LinqName.Count() != 0)
            {
                ListBookWrapPanel.Children.Clear();
                foreach (Book n in LinqName)
                {
                    BookItem bookItem = new BookItem(n);
                    ListBookWrapPanel.Children.Add(bookItem);
                }
            }
            else
            {
                SearchFail.IsOpen = true;
            }
        }
        public SearchBooks()
        {
            InitializeComponent();
            This = this;
        }
        private void Up_Click(object sender, RoutedEventArgs e)
        {
            scroll.LineUp();
        }

        private void Down_Click(object sender, RoutedEventArgs e)
        {
            scroll.LineDown();
        }

        private async void Search_Click(object sender, RoutedEventArgs e)
        {
            if (SearchTextBox.Text != "")
            {
                SearchBooks searchBooks = new SearchBooks();

                Books library = new Books();
                ThicknessAnimation anim1 = new ThicknessAnimation();
                anim1.From = new Thickness(0, 10, 0, 0);
                anim1.To = new Thickness(0, 30, 0, 0);
                anim1.FillBehavior = FillBehavior.Stop;
                anim1.Duration = new Duration(TimeSpan.FromSeconds(0.25));
                scroll.BeginAnimation(Label.MarginProperty, anim1);

                SearchSelIndBack = SearchCombo.SelectedIndex;
                SearchTextBack = SearchTextBox.Text.ToLower();
                This = this;
                int selected_index = SearchCombo.SelectedIndex;
                switch (selected_index)
                {
                    case 0:
                        List<Book> Res1 = await Task.Run(() => Founder(library.MyBooks));
                        var BookName = from n in Res1
                                       where n.BookName.ToLower().StartsWith(SearchTextBox.Text.ToLower())
                                       select n;
                        FounderTwo(BookName);
                        break;
                    case 1:
                        List<Book> Res2 = await Task.Run(() => Founder(library.MyBooks));
                        var Author = from n in Res2
                                     where n.Author.ToLower().StartsWith(SearchTextBox.Text.ToLower())
                                     select n;
                        FounderTwo(Author);
                        break;
                    case 2:
                        List<Book> Res3 = await Task.Run(() => Founder(library.MyBooks));
                        var PubCountry = from n in Res3
                                         where n.Country.ToLower().StartsWith(SearchTextBox.Text.ToLower())
                                         select n;
                        FounderTwo(PubCountry);
                        break;
                    case 3:
                        List<Book> Res4 = await Task.Run(() => Founder(library.MyBooks));
                        var Genres = from n in Res4
                                     where n.Genre.ToLower().StartsWith(SearchTextBox.Text.ToLower())
                                     select n;
                        FounderTwo(Genres);
                        break;
                    case 4:
                        int outyear;
                        bool result = Int32.TryParse(SearchTextBox.Text, out outyear);
                        if (result)
                        {
                            List<Book> Res5 = await Task.Run(() => Founder(library.MyBooks));
                            var GodIzd = from n in Res5
                                         where n.GodIzdaniya == outyear
                                         select n;
                            FounderTwo(GodIzd);
                        }
                        else Icon1.Foreground = Brushes.Red;

                        break;
                }
            }
            else
            {
                Icon1.Foreground = Brushes.Red;
            }
        }
        private async void Sort_Click(object sender, RoutedEventArgs e)
        {
            Books library = new Books();
            int selected_index = SortCombo.SelectedIndex;
            ListBookWrapPanel.Children.Clear();
            switch (selected_index)
            {
                case 0:
                    List<Book> Res1 = await Task.Run(() => Founder(library.MyBooks));
                    var SortBookTitle = from n in Res1
                                        orderby n.BookName
                                        select n;
                    FounderTwo(SortBookTitle);
                    break;
                case 1:
                    List<Book> Res2 = await Task.Run(() => Founder(library.MyBooks));
                    var SortAuthor = from n in Res2
                                     orderby n.Author
                                     select n;
                    FounderTwo(SortAuthor);
                    break;
                case 2:
                    List<Book> Res3 = await Task.Run(() => Founder(library.MyBooks));
                    var SortPubCountry = from n in Res3
                                         orderby n.Country
                                         select n;
                    FounderTwo(SortPubCountry);
                    break;
                case 3:
                    List<Book> Res4 = await Task.Run(() => Founder(library.MyBooks));
                    var SortGenre = from n in Res4
                                    orderby n.Genre
                                    select n;
                    FounderTwo(SortGenre);
                    break;
                case 4:
                    List<Book> Res5 = await Task.Run(() => Founder(library.MyBooks));

                    var SortPubYear = from n in Res5
                                      orderby n.GodIzdaniya
                                      select n;
                    FounderTwo(SortPubYear);
                    break;
            }
        }

        private void SearchTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            Icon1.Foreground = color;
        }

        private async void ViewAll_Click(object sender, RoutedEventArgs e)
        {
            ListBookWrapPanel.Children.Clear();

            List<Book> Res = await Task.Run(() => Founder(library.MyBooks));
            var BookName = from n in Res
                           orderby n.BookName
                           select n;
            FounderTwo(BookName);
        }

    }
}
