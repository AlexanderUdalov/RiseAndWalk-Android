using Android.App;
using Android.Content;
using System;
using System.Collections.Generic;
using TimeSetEventArgs = Android.App.TimePickerDialog.TimeSetEventArgs;

namespace RiseAndWalk_Android.Controllers
{
    public class DialogController
    {
        #region Singletone

        private static readonly Lazy<DialogController> _instanceHolder =
            new Lazy<DialogController>(() => new DialogController());

        public static DialogController Instance => _instanceHolder.Value;

        #endregion Singletone

        private Dialog _timePickerDialog;
        private Dialog _dayOfWeekPickerDialog;

        private bool _dialogShowed = false;
        
        #region TimePickerDialog
        public void ShowTimePickerDialog(Context context, EventHandler<TimeSetEventArgs> handler)
        {
            if (_dialogShowed) return;

            _dialogShowed = true;
            _timePickerDialog = CreateTimePickerDialog(context, handler);

            _timePickerDialog.Show();
        }

        private TimePickerDialog CreateTimePickerDialog(Context context, EventHandler<TimeSetEventArgs> handler)
        {
            var dialog = new TimePickerDialog(context, (object sender, TimeSetEventArgs args) =>
            {
                handler(sender, args);
                _dialogShowed = false;
            }, DateTime.Now.Hour, DateTime.Now.Minute, true);

            dialog.CancelEvent += delegate
            {
                _dialogShowed = false;
            };
            return dialog;
        }

        #endregion

        #region DayOfWeekDialog
        public void ShowDayOfWeekDialog(Context context, Action<List<int>> callback)
        {
            if (_dialogShowed) return;

            _dialogShowed = true;
            _dayOfWeekPickerDialog = CreateDayOfWeekDialog(context, callback);

            _dayOfWeekPickerDialog.Show();
        }
        //TODO: Отрефакторить
        private Dialog CreateDayOfWeekDialog(Context context, Action<List<int>> callback)
        {
            var builder = new AlertDialog.Builder(context);
            builder.SetTitle(Resource.String.choose_day_of_week);

            var choosedItems = new List<int>(9);
            var items = new string[9];

            var checkedItems = new bool[9];
            for (var i = 0; i < 8; i++)
                checkedItems[i] = false;
            checkedItems[8] = true;

            items[0] = context.GetString(Resource.String.mon);
            items[1] = context.GetString(Resource.String.tue);
            items[2] = context.GetString(Resource.String.wed);
            items[3] = context.GetString(Resource.String.thu);
            items[4] = context.GetString(Resource.String.fri);
            items[5] = context.GetString(Resource.String.sat);
            items[6] = context.GetString(Resource.String.sun);
            items[7] = context.GetString(Resource.String.everyday);
            items[8] = context.GetString(Resource.String.onetime);

            builder.SetMultiChoiceItems(items, checkedItems,
                (object sender, DialogMultiChoiceClickEventArgs args) =>
                {
                    if (args.Which < 6)
                    {
                        checkedItems[7] = false;
                        checkedItems[8] = false;
                        if (args.IsChecked)
                            choosedItems.Add(args.Which);
                        else choosedItems.Remove(args.Which);
                    }
                    else if (args.Which == 7)
                    {
                        if (args.IsChecked)
                        {
                            for (int i = 0; i < 7; i++)
                            {
                                checkedItems[i] = false;
                                choosedItems.Add(i);
                            }
                        }
                    }
                    else if (args.Which == 8)
                    {
                        if (args.IsChecked)
                        {
                            for (int i = 0; i < 8; i++)
                            {
                                checkedItems[i] = false;
                                choosedItems.Remove(i);
                            }
                        }
                    }
                });

            //TODO: Разобраться с кнопками и их событиями
            builder.SetPositiveButton("OK", delegate
            {
                callback.Invoke(choosedItems);
                _dialogShowed = false;
            });
            builder.SetNegativeButton("Cancel", delegate
            {
                callback.Invoke(new List<int>());
                _dialogShowed = false;
            });

            var dayOfWeekPickerDialog = builder.Create();

            dayOfWeekPickerDialog.DismissEvent += delegate
            {
                _dialogShowed = false;
            };
            return dayOfWeekPickerDialog;
        }

        #endregion
    }
}