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
    /// Логика взаимодействия для SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : UserControl
    {
        public static MainWindow Window;
        //public List<SolidColorBrush> colorlist = new List<SolidColorBrush>();
        public SolidColorBrush color1 = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#7E57C2"));
        public SolidColorBrush color2 = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#EF5350"));
        public SolidColorBrush color3 = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#37474F"));
        public SolidColorBrush color4 = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#00BFA5"));
        public SolidColorBrush color5 = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#EC407A"));
        public SolidColorBrush color6 = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#5D4037"));
        public SolidColorBrush color7 = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#2E7D32"));

        public SolidColorBrush color11 = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#EDE7F6"));
        public SolidColorBrush color22 = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFEBEE"));
        public SolidColorBrush color33 = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#ECEFF1"));
        public SolidColorBrush color44 = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E0F2F1"));
        public SolidColorBrush color55 = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FCE4EC"));
        public SolidColorBrush color66 = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#EFEBE9"));
        public SolidColorBrush color77 = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E8F5E9"));

        public SettingsWindow()
        {
            InitializeComponent();
            //colorlist.Add(new SolidColorBrush((Color)ColorConverter.ConvertFromString("#7E57C2")));
        }

        private void Button1_Click(object sender, RoutedEventArgs e)
        {
            Window.TopTopGrid.Background = color1;
            Window.GridMenu.Background = color1;
            Window.FirstLetterCircle.Background = color1;
            Window.ContentGrid.Background = color11;
            Window.TopInfoGrid.Background = color11;
        }

        private void Button2_Click(object sender, RoutedEventArgs e)
        {
            Window.TopTopGrid.Background = color2;
            Window.GridMenu.Background = color2;
            Window.FirstLetterCircle.Background = color2;
            Window.ContentGrid.Background = color22;
            Window.TopInfoGrid.Background = color22;
        }

        private void Button3_Click(object sender, RoutedEventArgs e)
        {
            Window.TopTopGrid.Background = color3;
            Window.GridMenu.Background = color3;
            Window.FirstLetterCircle.Background = color3;
            Window.ContentGrid.Background = color33;
            Window.TopInfoGrid.Background = color33;
        }

        private void Button4_Click(object sender, RoutedEventArgs e)
        {
            Window.TopTopGrid.Background = color4;
            Window.GridMenu.Background = color4;
            Window.FirstLetterCircle.Background = color4;
            Window.ContentGrid.Background = color44;
            Window.TopInfoGrid.Background = color44;
        }

        private void Button5_Click(object sender, RoutedEventArgs e)
        {
            Window.TopTopGrid.Background = color5;
            Window.GridMenu.Background = color5;
            Window.FirstLetterCircle.Background = color5;
            Window.ContentGrid.Background = color55;
            Window.TopInfoGrid.Background = color55;
        }

        private void Button6_Click(object sender, RoutedEventArgs e)
        {
            Window.TopTopGrid.Background = color6;
            Window.GridMenu.Background = color6;
            Window.FirstLetterCircle.Background = color6;
            Window.ContentGrid.Background = color66;
            Window.TopInfoGrid.Background = color66;
        }

        private void Button7_Click(object sender, RoutedEventArgs e)
        {
            Window.TopTopGrid.Background = color7;
            Window.GridMenu.Background = color7;
            Window.FirstLetterCircle.Background = color7;
            Window.ContentGrid.Background = color77;
            Window.TopInfoGrid.Background = color77;
        }
    }
}
