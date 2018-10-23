using System;
using System.Collections.Generic;
using Android.Util;
using SQLite;

namespace RiseAndWalk_Android.Models
{
    internal class SQLiteDatabase<T> where T : class, new()
    {
        private readonly string _databasePath;

        public SQLiteDatabase(string databaseName)
        {
            _databasePath = System.IO.Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.Personal), databaseName + ".db");

            CreateIfNotExist();
        }

        public bool CreateIfNotExist()
        {
            try
            {
                using (var connection = new SQLiteConnection(_databasePath))
                {
                    connection.CreateTable<T>();
                    return true;
                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLiteEx", ex.Message);
                return false;
            }
        }

        public bool Add(T item)
        {
            try
            {
                using (var connection = new SQLiteConnection(_databasePath))
                {
                    connection.Insert(item);
                    return true;
                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLiteEx", ex.Message);
                return false;
            }
        }

        public IEnumerable<T> Get()
        {
            try
            {
                using (var connection = new SQLiteConnection(_databasePath))
                {
                    return connection.Table<T>().ToList();
                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLiteEx", ex.Message);
                return null;
            }
        }

        public T Get(Guid guid)
        {
            try
            {
                using (var connection = new SQLiteConnection(_databasePath))
                {
                    return connection.Get<T>(guid);
                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLiteEx", ex.Message);
                return null;
            }
        }

        public bool Delete(T item)
        {
            try
            {
                using (var connection = new SQLiteConnection(_databasePath))
                {
                    connection.Delete(item);
                    return true;
                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLiteEx", ex.Message);
                return false;
            }
        }

        public bool Delete(Guid guid)
        {
            try
            {
                using (var connection = new SQLiteConnection(_databasePath))
                {
                    connection.Delete(connection.Get<T>(guid));
                    return true;
                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLiteEx", ex.Message);
                return false;
            }
        }

        public bool Update(T item)
        {
            try
            {
                using (var connection = new SQLiteConnection(_databasePath))
                {
                    connection.Update(item);
                    return true;
                }
            }
            catch (SQLiteException ex)
            {
                Log.Info("SQLiteEx", ex.Message);
                return false;
            }
        }
    }
}