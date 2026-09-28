using System.Windows.Controls;

namespace ГалиевЧудоОбувь
{
    public static class Manager
    {
        public static Frame MainFrame { get; set; }

        public static Users CurrentUser { get; set; }

        public static bool IsGuest => CurrentUser == null;
        public static bool IsUser => CurrentUser != null && CurrentUser.RoleID == 1;
        public static bool IsManager => CurrentUser != null && CurrentUser.RoleID == 2;
        public static bool IsAdmin => CurrentUser != null && CurrentUser.RoleID == 3;
    }
}