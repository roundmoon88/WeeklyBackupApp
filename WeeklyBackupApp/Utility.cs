using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeeklyBackupApp
{
    public class Utility
    {
        public static string GetDateTimeStamp()
        {
            //DateTime dt = DateTime.Now.Date;
            DateTime dt = DateTime.Now;
            string timeStemp;
            string year = dt.Year.ToString();
            string month = dt.Month.ToString();
            if (month.Length == 1)
                month = "0" + month;
            string day = dt.Day.ToString();
            if (day.Length == 1)
                day = "0" + day;
            string hour = dt.Hour.ToString();
            if (hour.Length == 1)
                hour = "0" + hour;
            string minute = dt.Minute.ToString();
            if (minute.Length == 1)
                minute = "0" + minute;
            string second = dt.Second.ToString();
            if (second.Length == 1)
                second = "0" + second;
            string millisecond = dt.Millisecond.ToString();

            string dateSpliter = "-";
            //string dateSpliter = "/";
            string timeSpliter = ":";
            timeStemp = year + dateSpliter + month + dateSpliter + day  + " " + hour + timeSpliter + minute + timeSpliter + second + "." + millisecond;
            //timeStemp = year + dateSpliter + month + dateSpliter + day + " " + hour + timeSpliter + minute + timeSpliter + second; // + "." + millisecond;

            return timeStemp;
        }
    }
}
