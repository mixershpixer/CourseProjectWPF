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
    /// Логика взаимодействия для MyAccount.xaml
    /// </summary>
    public partial class MyAccount : UserControl
    {
        User user;
        public MyAccount(User user)
        {
            InitializeComponent();
            this.user = user;
            UserName.Text = user.name;
            UserSurname.Text = user.surname;
            UserUserName.Text = user.username;
        }
    }
}
