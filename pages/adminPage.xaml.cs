using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
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

namespace amonicAirlines.pages
{
    /// <summary>
    /// Логика взаимодействия для adminPage.xaml
    /// </summary>
    public partial class adminPage : Page
    {
        private List<Users> users;
        public adminPage()
        {
            InitializeComponent();
            officeBox.ItemsSource = db.GetInstance().Offices.ToList();
        }

        private void exit_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new authPage());
        }

        private void addUserBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new addUser());
        }

        private void changeRoleBtn_Click(object sender, RoutedEventArgs e)
        {
            Users user = data.SelectedItem as Users;
            if (user == null)
            {
                MessageBox.Show("Select user.", "Error!");
                return;
            };
            NavigationService.Navigate(new changeUserRole(user));
        }

        private void enableBtn_Click(object sender, RoutedEventArgs e)
        {
            Users user = data.SelectedItem as Users;
            if (user == null)
            {
                MessageBox.Show("Select user.", "Error!");
                return;
            };

            user.Active = !user.Active;
            try
            {
                db.GetInstance().Users.AddOrUpdate(user);
                db.GetInstance().SaveChanges();
            } catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error!");
            }
        }

        private void officeBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Offices selectedValue = officeBox.SelectedItem as Offices;

            if (selectedValue != null)
            {
                List<Users> filtered = users.ToList().Where(el => el.OfficeID == selectedValue.ID).ToList();
                data.ItemsSource = filtered;
            }
        }

        private void data_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            db.GetInstance().ChangeTracker.Entries().ToList().ForEach(entry => { entry.Reload(); });
            users = db.GetInstance().Users.ToList();
            data.ItemsSource = users;
        }
    }
}
