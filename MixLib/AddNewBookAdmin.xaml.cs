using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Mapping;
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
    /// Логика взаимодействия для AddNewBookAdmin.xaml
    /// </summary>
    public partial class AddNewBookAdmin : UserControl
    {
        public SolidColorBrush color = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF9800"));

        public int index = 0;
        bool check = false;
        public string filename;
        public AddNewBookAdmin()
        {
            InitializeComponent();
        }

        private void ChooseImage_Click(object sender, RoutedEventArgs e)
        {
            check = true;
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
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
                Borderchik.BorderBrush = null;
            }
        }

        private void AddNewBookBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int outyear;
                bool result = Int32.TryParse(PubYear.Text, out outyear);
                Books library = new Books();
                string[] t1 = { BookTitle.Text, Author.Text, Genre.Text, PubCountry.Text, PubYear.Text, Disc.Text };
                TextBlock[] t2 = { BookTitleBlock, AuthorBlock, GenreBlock, PubCountryBlock, PubYearBlock, DiscBlock };
                int i = 0;
                for (; i < 6; i++)
                    if (t1[i].Equals(""))
                    {
                        t2[i].Foreground = Brushes.Red;
                        break;
                    }
                if (i == 6 && result && outyear <= 2018 && outyear >= 0)
                {
                    if (!Disc.Text.Equals("") && Disc.Text.Length <= 300)
                    {
                        if (check)
                        {
                            Book book = new Book(
                                0,
                                BookTitle.Text,
                                Author.Text,
                                Convert.ToInt32(PubYear.Text),
                                PubCountry.Text,
                                Disc.Text,
                                filename,
                                Genre.Text
                            );
                            library.MyBooks.Add(book);
                            library.SaveChanges();

                            Book getbook = (from c in library.MyBooks
                                            where c.BookName == BookTitle.Text
                                            select c).SingleOrDefault();
                            if (getbook == null)
                                AddFail.IsOpen = true;
                            else AddOk.IsOpen = true;
                        }
                        else Borderchik.BorderBrush = Brushes.Red;
                    }
                    else DiscBlock.Foreground = Brushes.Red;
                }
                else PubYearBlock.Foreground = Brushes.Red;

            }
            catch
            {
                AddFail.IsOpen = true;
            }
        }

        private void Disc_TextChanged(object sender, TextChangedEventArgs e)
        {
            int len = Disc.Text.Length;
            DiscBlock.Text = "Annotation: (" + (300 - len) + ")";
        }

        private void BookTitle_GotFocus(object sender, RoutedEventArgs e)
        {
            BookTitleBlock.Foreground = Brushes.Black;
        }

        private void Author_GotFocus(object sender, RoutedEventArgs e)
        {
            AuthorBlock.Foreground = Brushes.Black;
        }

        private void Genre_GotFocus(object sender, RoutedEventArgs e)
        {
            GenreBlock.Foreground = Brushes.Black;
        }

        private void PubCountry_GotFocus(object sender, RoutedEventArgs e)
        {
            PubCountryBlock.Foreground = Brushes.Black;
        }

        private void PubYear_GotFocus(object sender, RoutedEventArgs e)
        {
            PubYearBlock.Foreground = Brushes.Black;
        }

        private void Disc_GotFocus(object sender, RoutedEventArgs e)
        {
            DiscBlock.Foreground = Brushes.Black;
        }

        private void BookTitle_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !("0123456789 ,".IndexOf(e.Text) < 0);

        }
    }
}
