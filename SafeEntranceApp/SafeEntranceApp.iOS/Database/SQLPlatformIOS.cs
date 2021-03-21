using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Foundation;
using SafeEntranceApp.iOS.Database;
using SQLite;
using UIKit;

[assembly: Xamarin.Forms.Dependency(typeof(SQLPlatformIOS))]
namespace SafeEntranceApp.iOS.Database
{
    class SQLPlatformIOS
    {
        private string GetPath()
        {
            var fileName = "safeentrancelocaldb.db3";
            string personalFolder = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            string libraryFolder = Path.Combine(personalFolder, "..", "Library");
            var path = Path.Combine(libraryFolder, fileName);
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