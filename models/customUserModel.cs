using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace amonicAirlines.models
{
    class customUserModel
    {
        public int id { get; set; }
        public int age { get; set; }
        public String email { get; set; }
        public String name { get; set; }
        public String lastName { get; set; }
        public Offices office { get; set; }
        public bool isActive { get; set; }
        public Roles role { get; set; }
        public String code { get => isActive ? role.ID == 1 ? "Lime" : "White" : "Red"; }
        public customUserModel(int id, int age, string email, string name, string lastName, Offices officeTitle, bool isActive, Roles role)
        {
            this.id = id;
            this.age = age;
            this.email = email;
            this.name = name;
            this.lastName = lastName;
            this.office = officeTitle;
            this.isActive = isActive;
            this.role = role;
        }
    }
}
