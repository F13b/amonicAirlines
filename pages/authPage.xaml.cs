using amonicAirlines.pages;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Policy;
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
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace amonicAirlines
{
    /// <summary>
    /// Логика взаимодействия для authPage.xaml
    /// </summary>
    public partial class authPage : Page
    {

        public authPage()
        {
            InitializeComponent();
        }

        private void exitBtn_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void loginBtn_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder erros = new StringBuilder();

            if (string.IsNullOrWhiteSpace(login.Text))
                erros.AppendLine("Enter email.");
            if (!Regex.IsMatch(login.Text, @"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.IgnoreCase))
                erros.AppendLine("Enter correct email.");
            if (string.IsNullOrWhiteSpace(password.Password))
                erros.AppendLine("Enter password.");

            if (erros.Length > 0)
            {
                MessageBox.Show(erros.ToString(), "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }   

            try
            {
                var hashedPassword = hashPassword(password.Password);

                var user = db.GetInstance().Users.First((element) => element.Email == login.Text.Trim() && element.Password.Trim() == hashedPassword);

                if (user != null)
                {
                    if (!user.Active)
                    {
                        MessageBox.Show("Error", "You are banned and I will not unban you!", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else
                    {
                        if (user.Roles.Title == "Administrator")
                        {
                            NavigationService.Navigate(new adminPage());
                        }
                        else
                        {
                            NavigationService.Navigate(new userPage(user));
                        }
                    }
                } else
                {
                    MessageBox.Show("User not found.", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                }

            } catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private static string hashPassword(string password)
        {
            var md5 = MD5.Create();
            byte[] passwordBytes = Encoding.ASCII.GetBytes(password);
            byte[] hash = md5.ComputeHash(passwordBytes);
            StringBuilder stringBuilder = new StringBuilder();

            foreach (var stroke in hash)
            {
                stringBuilder.Append(stroke.ToString("X2"));
            }
            return Convert.ToString(stringBuilder);
        }
    }
}
