using amonicAirlines.models;
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
            var offices = db.GetInstance().Offices.ToList();
            var allOfficesItem = new Offices();
            allOfficesItem.Title = "All offices";
            offices.Add(allOfficesItem);
            officeBox.ItemsSource = offices;
            officeBox.SelectedItem = 0;
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
            customUserModel user = data.SelectedItem as customUserModel;
            if (user == null)
            {
                MessageBox.Show("Select user.", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            };
            Users selectedUser = db.GetInstance().Users.First((element) => element.ID == user.id);
            NavigationService.Navigate(new changeUserRole(selectedUser));
        }

        private void enableBtn_Click(object sender, RoutedEventArgs e)
        {
            customUserModel user = data.SelectedItem as customUserModel;
            if (user == null)
            {
                MessageBox.Show("Select user.", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            };
            Users selectedUser = db.GetInstance().Users.First((element) => element.ID == user.id);

            selectedUser.Active = !selectedUser.Active;
            try
            {
                db.GetInstance().Users.AddOrUpdate(selectedUser);
                db.GetInstance().SaveChanges();
                changeData();
            } catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void officeBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            changeData();
        }

        private void data_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            db.GetInstance().ChangeTracker.Entries().ToList().ForEach(entry => { entry.Reload(); });
            List<customUserModel> users = new List<customUserModel>();
            foreach (var user in db.GetInstance().Users.ToList())
            {
                users.Add(new customUserModel(user.ID, user.getAge(), user.Email, user.FirstName, user.LastName, user.Offices, user.Active, user.Roles));
            }
            data.ItemsSource = users;
        }

        private void changeData()
        {
            Offices selectedValue = officeBox.SelectedItem as Offices;

            List<customUserModel> users = new List<customUserModel>();

            if (selectedValue.Title == "All offices")
            {
                foreach (var user in db.GetInstance().Users.ToList())
                {
                    users.Add(new customUserModel(user.ID, user.getAge(), user.Email, user.FirstName, user.LastName, user.Offices, user.Active, user.Roles));
                }
            }
            else
            {
                foreach (var user in db.GetInstance().Users.Where((element) => element.OfficeID == selectedValue.ID).ToList())
                {
                    users.Add(new customUserModel(user.ID, user.getAge(), user.Email, user.FirstName, user.LastName, user.Offices, user.Active, user.Roles));
                }
            }

            data.ItemsSource = users;
        }
    }
}
