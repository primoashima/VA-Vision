using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VAVision
{
    static class Global
    {
        private static string _globalVar = "";
        private static bool _sendnotification = false;
        private static int projectid = 0;
        private static int todoid = 0;
        private static int userid = 0;
        private static string breaktime = "";
        private static string breakseconds = "";
        private static DateTime starttime = DateTime.Now;

        public static string GlobalVar
        {
            get { return _globalVar; }
            set { _globalVar = value; }
        }
        public static int UserId
        {
            get { return userid; }
            set { userid = value; }
        }
        public static bool SendNotification
        {
            get { return _sendnotification; }
            set { _sendnotification = value; }
        }
        public static int ProjectId
        {
            get { return projectid; }
            set { projectid = value; }
        }

        public static string BreakTime  
        {
            get { return breaktime; }
            set { breaktime = value; }
        }
        public static string BreakSeconds
        {
            get { return breakseconds; }
            set { breakseconds = value; }
        }
        public static int TodoId
        {
            get { return todoid; }
            set { todoid = value; }
        }
        public static DateTime StartTime
        {
            get { return starttime; }
            set { starttime = value; }
        }

    }
}
