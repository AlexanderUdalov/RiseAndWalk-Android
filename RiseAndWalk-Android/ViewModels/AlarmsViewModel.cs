using RiseAndWalk_Android.Models;
using RiseAndWalk_Android.Services;

namespace RiseAndWalk_Android.ViewModels
{
    public class AlarmsViewModel
    {
        public IDataStore<Alarm> Alarms { get; }

        public AlarmsViewModel()
        {
            //Поменять при подключении сервера
            Alarms = new MockDataStore();
        }
    }
}