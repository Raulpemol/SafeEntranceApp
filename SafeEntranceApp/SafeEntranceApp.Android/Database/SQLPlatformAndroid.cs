using System;
using Android.OS;
using SafeEntranceApp.Droid.Database;
using SafeEntranceApp.Common;
using SQLite;
using System.IO;

[assembly: Xamarin.Forms.Dependency(typeof(SQLPlatformAndroid))]
namespace SafeEntranceApp.Droid.Database
{
    class SQLPlatformAndroid : ISQLPlatform
    {
        private string GetPath()
        {
            var fileName = "safeentrancelocaldb.db3";
            var path = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), fileName);
            return path;
        }

        public SQLiteConnection GetConnection()
        {
            return new SQLiteConnection(GetPath());
        }

        public SQLiteAsyncConnection GetConnectionAsync()
        {
            return new SQLiteAsyncConnection(GetPath());
        }
    }
}