using Android.Nfc;
using RiseAndWalk_Android.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RiseAndWalk_Android.Services
{
    public class MockDataStore : IDataStore<Alarm>
    {
        private List<Alarm> _alarms;

        public MockDataStore()
        {
            _alarms = new List<Alarm>();
            var mockAlarms = new List<Alarm>
            {
                new Alarm { Id = Guid.NewGuid(), Description="This is an 1 Alarm description.", Time = new DateTime(2000,1,2,1,20,0) },
                new Alarm { Id = Guid.NewGuid(), Description="This is an 2 Alarm description.", Time = new DateTime(2000,1,2,18,23,0)  },
                new Alarm { Id = Guid.NewGuid(), Description="This is an 3 Alarm description.", Time = new DateTime(2000,1,2,10,0,0)  },
                new Alarm { Id = Guid.NewGuid(), Description="This is an 4 Alarm description.", Time = new DateTime(2000,1,2,3,53,0)  },
                new Alarm { Id = Guid.NewGuid(), Description="This is an 5 Alarm description.", Time = new DateTime(2000,1,2,7,47,0)  },
                new Alarm { Id = Guid.NewGuid(), Description="This is an 6 Alarm description.", Time = new DateTime(2000,1,2,6,00,0)  },
                new Alarm { Id = Guid.NewGuid(), Description="This is an 1 Alarm description.", Time = new DateTime(2000,1,2,1,20,0) },
                new Alarm { Id = Guid.NewGuid(), Description="This is an 2 Alarm description.", Time = new DateTime(2000,1,2,18,23,0)  },
                new Alarm { Id = Guid.NewGuid(), Description="This is an 3 Alarm description.", Time = new DateTime(2000,1,2,10,0,0)  },
                new Alarm { Id = Guid.NewGuid(), Description="This is an 4 Alarm description.", Time = new DateTime(2000,1,2,3,53,0)  },
                new Alarm { Id = Guid.NewGuid(), Description="This is an 5 Alarm description.", Time = new DateTime(2000,1,2,7,47,0)  },
                new Alarm { Id = Guid.NewGuid(), Description="This is an 1 Alarm description.", Time = new DateTime(2000,1,2,1,20,0) },
                new Alarm { Id = Guid.NewGuid(), Description="This is an 2 Alarm description.", Time = new DateTime(2000,1,2,18,23,0)  },
                new Alarm { Id = Guid.NewGuid(), Description="This is an 3 Alarm description.", Time = new DateTime(2000,1,2,10,0,0)  },
                new Alarm { Id = Guid.NewGuid(), Description="This is an 4 Alarm description.", Time = new DateTime(2000,1,2,3,53,0)  },
                new Alarm { Id = Guid.NewGuid(), Description="This is an 5 Alarm description.", Time = new DateTime(2000,1,2,7,47,0)  },

            };

            foreach (var Alarm in mockAlarms)
            {
                _alarms.Add(Alarm);
            }
        }

        public async Task<bool> AddItemAsync(Alarm Alarm)
        {
            _alarms.Add(Alarm);

            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateItemAsync(Alarm Alarm)
        {
            var oldAlarm = _alarms.Where((Alarm arg) => arg.Id == Alarm.Id).FirstOrDefault();
            _alarms.Remove(oldAlarm);
            _alarms.Add(Alarm);

            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteItemAsync(Guid id)
        {
            var oldAlarm = _alarms.Where((Alarm arg) => arg.Id == id).FirstOrDefault();
            _alarms.Remove(oldAlarm);

            return await Task.FromResult(true);
        }

        public async Task<Alarm> GetItemAsync(Guid id)
        {
            return await Task.FromResult(_alarms.FirstOrDefault(s => s.Id == id));
        }

        public async Task<IEnumerable<Alarm>> GetItemsAsync(bool forceRefresh = false)
        {
            return await Task.FromResult(_alarms);
        }
    }
}