using Android.Nfc;
using System;

namespace RiseAndWalk_Android.Models
{
    public class Alarm
    {
        public Guid Id;
        public DateTime Time;
        public string Description;
        public DayOfWeek[] DaysOfWeek;
        public Tag NfcTag;

        public string GetDaysOfWeekString()
        {
            return "пн, пт, вс";
        }
    }
}