using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using COSC295assign2Cao7992;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;
using Xamarin.Forms;

/**
 * Helper method to get a full path of storage where the database located
 */
[assembly: Dependency(typeof(COSC295assign2Cao7992.Droid.FileHelper))]
namespace COSC295assign2Cao7992.Droid
{
    public class FileHelper : IFileHelper
    {
        public string GetLocalFilePath(string filename)
        {
            string path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            return Path.Combine(path, filename);
        }
    }
}