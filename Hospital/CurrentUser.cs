using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hospital
{
    public class CurrentUser
    {

        public static int RoleID { get; set; }
        public static int UserID { get; set; }

        Dictionary<string, string> currentUser = new Dictionary<string, string>()
        {
            { "user", "Администратор" },
            { "user2", "Врач" },
            { "user3", "Пациент" }
        };
    }
}