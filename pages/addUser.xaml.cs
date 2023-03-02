using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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
    /// Логика взаимодействия для addUser.xaml
    /// </summary>
    public partial class addUser : Page
    {
        private List<Offices> officesList;
        public addUser()
        {
            InitializeComponent();
            officesList = db.GetInstance().Offices.ToList();
            office.ItemsSource = officesList;
        }

        private void saveBtn_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder erros = new StringBuilder();

            if (string.IsNullOrWhiteSpace(email.Text))
                erros.AppendLine("Enter email.");
            if (!Regex.IsMatch(email.Text, @"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.IgnoreCase))
                erros.AppendLine("Enter correct email.");
            if (string.IsNullOrWhiteSpace(firstName.Text))
                erros.AppendLine("Enter firstName.");
            if (string.IsNullOrWhiteSpace(lastName.Text))
                erros.AppendLine("Enter lastName.");
            if (office.SelectedItem == null)
                erros.AppendLine("Enter office.");
            if (birthday.Text == null)
                erros.AppendLine("Enter birthday.");
            if (string.IsNullOrWhiteSpace(password.Password))
                erros.AppendLine("Enter password.");

            if (erros.Length > 0)
            {
                MessageBox.Show(erros.ToString(), "Error!");
                return;
            }

            try
            {
                Users user = new Users();

                user.Email = email.Text;
                user.FirstName = firstName.Text;
                user.LastName = lastName.Text;
                user.Birthdate = birthday.SelectedDate;

                Offices selectedOffice = office.SelectedItem as Offices;

                user.OfficeID = selectedOffice.ID;

                string hashedPassword = hashPassword(password.Password);

                user.Password = hashedPassword;
                user.RoleID = 2;
                user.Active = true;

                db.GetInstance().Users.Add(user);
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

        private static string hashPassword(string password)
        {
            MD5 md5 = MD5.Create();
            byte[] b = Encoding.ASCII.GetBytes(password);
            byte[] hash = md5.ComputeHash(b);

            StringBuilder sb = new StringBuilder();
            foreach (var a in hash)
                sb.Append(a.ToString("X2"));

            return Convert.ToString(sb);
        }
    }
}
