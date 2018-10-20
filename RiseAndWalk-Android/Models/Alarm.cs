using Android.Content;
using System;
using System.Linq;

namespace RiseAndWalk_Android.Models
{
    public class Alarm
    {
        public Guid Id;
        public DateTime Time;
        public string Description;
        public DayOfWeek[] DaysOfWeek;
        public string NfcTagHash;
        public bool Enabled;

        public string GetDaysOfWeekString(Context context)
        {
            return DaysOfWeek == null || DaysOfWeek.Length == 0 ?
                "" :
                string.Join(", ", DaysOfWeek
                    .Select((x) => DayOfWeekToResourceString(x, context))
                    .ToArray());
        }

        private static string DayOfWeekToResourceString(DayOfWeek day, Context context)
        {
            switch (day)
            {
                case DayOfWeek.Monday: return context.GetString(Resource.String.mon_short);
                case DayOfWeek.Tuesday: return context.GetString(Resource.String.tue_short);
                case DayOfWeek.Wednesday: return context.GetString(Resource.String.wed_short);
                case DayOfWeek.Thursday: return context.GetString(Resource.String.thu_short);
                case DayOfWeek.Friday: return context.GetString(Resource.String.fri_short);
                case DayOfWeek.Saturday: return context.GetString(Resource.String.sat_short);
                case DayOfWeek.Sunday: return context.GetString(Resource.String.sun_short);
                default: return "";
            }
        }
    }
}