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
using System.Windows.Shapes;

namespace amonicAirlines
{
    /// <summary>
    /// Логика взаимодействия для crashReportWindow.xaml
    /// </summary>
    public partial class crashReportWindow : Window
    {
        Shadowing _shadowing;
        public crashReportWindow(Shadowing shadowing)
        {
            InitializeComponent();
            _shadowing = shadowing;

            this._shadowing = shadowing;

            titleTextBlock.Text = "No logout detected for your last login on " + shadowing.dateFormated + " at " + shadowing.logInFormated;
            _shadowing = shadowing;
        }

        private void confrimBtn_Click(object sender, RoutedEventArgs e)
        {
            var shadowing = db.GetInstance().Shadowing.Find(_shadowing.ID);
            shadowing.CrashReason = new TextRange(errorDescriptionTextBox.Document.ContentStart, errorDescriptionTextBox.Document.ContentEnd).Text;

            db.GetInstance().SaveChanges();
            this.Close();
        }
    }
}
