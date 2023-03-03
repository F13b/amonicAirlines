using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
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

namespace amonicAirlines.pages
{
    /// <summary>
    /// Логика взаимодействия для changeUserRole.xaml
    /// </summary>
    public partial class changeUserRole : Page
    {
        private Users _user;
        public changeUserRole(Users user)
        {
            InitializeComponent();
            _user = user;

            email.Text = _user.Email;
            firstName.Text = _user.FirstName;
            lastName.Text = _user.LastName;
            office.ItemsSource = db.GetInstance().Offices.Where(o => o.ID == _user.OfficeID).ToList();
            office.SelectedItem = 0;

            if (user.RoleID == 1)
            {
                adminRole.IsChecked = true;
            } else
            {
                userRole.IsChecked = true;
            }
        }

        private void saveBtn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (adminRole.IsChecked == true)
                {
                    _user.RoleID = 1;
                } else if (userRole.IsChecked == true) 
                {
                    _user.RoleID = 2;
                }

                db.GetInstance().Users.AddOrUpdate(_user);
                db.GetInstance().SaveChanges();


                NavigationService.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error!");
            }
        }

        private void cancelBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
