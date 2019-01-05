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
    /// Логика взаимодействия для CommentItem.xaml
    /// </summary>
    public partial class CommentItem : UserControl
    {
        public CommentItem(string username, string comment, int value)
        {
            InitializeComponent();
            Username.Text = username;
            UserFirstLetter.Text = (username).Substring(0, 1).ToUpper();
            ValueBox.Text = Convert.ToString(value);
            CommentTxt.Text = comment;
        }
        public CommentItem()
        {

        }
    }
}
