//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace amonicAirlines
{
    using System;
    using System.Collections.Generic;
    
    public partial class Shadowing
    {
        public int ID { get; set; }
        public System.DateTime Date { get; set; }
        public System.DateTime LogIn { get; set; }
        public Nullable<System.DateTime> LogOut { get; set; }
        public Nullable<System.DateTime> TimeSpent { get; set; }
        public Nullable<int> Crashes { get; set; }
        public string CrashReason { get; set; }
        public int UserID { get; set; }

        public String logInFormated { get => LogIn.TimeOfDay.ToString().Substring(0, 5); }
        public String logOutFormated { get => LogOut?.TimeOfDay.ToString().Substring(0, 5); }
        public String dateFormated { get => LogIn.Date.ToString("d"); }
    
        public virtual Users Users { get; set; }

        public String timeSpent
        {
            get
            {
                if (spentDuration.TotalSeconds == 0)
                {
                    return "";
                }
                else
                {
                    return spentDuration.ToString().Substring(3, 5);
                }
            }
        }

        public TimeSpan spentDuration
        {
            get
            {
                var duration = TimeSpan.FromTicks(LogOut?.Subtract(LogIn).Ticks ?? 0).Duration();

                return duration;

            }
        }
    }
}
