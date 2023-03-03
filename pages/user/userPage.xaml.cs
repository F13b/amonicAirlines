using System;
using System.Collections.Generic;
using System.Data.Entity;
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
using System.Windows.Threading;

namespace amonicAirlines.pages
{
    /// <summary>
    /// Логика взаимодействия для userPage.xaml
    /// </summary>
    public partial class userPage : Page
    {
        private Users _user;
        DispatcherTimer _timer;
        TimeSpan _time;
        Shadowing shadowing;

        public userPage(Users user)
        {
            _user = user;

            shadowing = new Shadowing();
            shadowing.Date = DateTime.Now;
            shadowing.LogIn = DateTime.Now;
            shadowing.UserID = _user.ID;

            InitializeComponent();

            var events = _user.Shadowing.ToList();

            userGreeting.Text = $"Hi {_user.FirstName}, Welcome to AMONIC Airlines.";

            foreach (var errorEvent in events.Where((element) => element.LogOut == null && element.CrashReason == null).ToList())
            {
                var crash = new crashReportWindow(errorEvent);
                crash.ShowDialog();
            }

            db.GetInstance().Shadowing.Add(shadowing);
            db.GetInstance().SaveChanges();

            double spendedSeconds = 0;
            foreach (var errorEvent in events.Where((element) => DateTime.Now.Subtract(element.LogIn.Date).Days <= 30))
            {
                spendedSeconds += errorEvent.spentDuration.TotalSeconds;
            }
            initTimer((int)spendedSeconds);

            data.ItemsSource = events;
        }

        private void exitBtn_Click(object sender, RoutedEventArgs e)
        {
            shadowing.LogOut = DateTime.Now;
            db.GetInstance().SaveChangesAsync();
            NavigationService.GoBack();
        }

        private void initTimer(int startTime)
        {
            _time = TimeSpan.FromSeconds(startTime);
            userAccountInfo.Text = "Time spent on system: " + _time.ToString("c");
            _timer = new DispatcherTimer(new TimeSpan(0, 0, 1), DispatcherPriority.Normal, delegate
            {
                userAccountInfo.Text = "Time spent on system: " + _time.ToString("c");
                _time = _time.Add(TimeSpan.FromSeconds(+1));
            }, Application.Current.Dispatcher);

            _timer.Start();
        }
    }
}
