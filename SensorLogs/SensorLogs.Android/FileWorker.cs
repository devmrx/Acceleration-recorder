using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

using System.IO;
using System.Threading.Tasks;
using Xamarin.Forms;


[assembly: Dependency(typeof(SensorLogs.Droid.FileWorker))]

namespace SensorLogs.Droid {
    class FileWorker : IFileWorker {

        private string foldername = "/SensorLogs";

        public Task DeleteAsync(string filename) {

            // удаляем файл
            File.Delete(GetFilePath(filename));
            return Task.FromResult(true);
        }

        public Task<bool> ExistsAsync(string filename) {
            // получаем путь к файлу
            string filepath = GetFilePath(filename);
            // существует ли файл
            bool exists = File.Exists(filepath);
            return Task<bool>.FromResult(exists);
        }

        public Task<IEnumerable<string>> GetFilesAsync() {
            // получаем все все файлы из папки
            IEnumerable<string> filenames = from filepath in Directory.EnumerateFiles(GetDocsPath())
                                            select Path.GetFileName(filepath);
            return Task<IEnumerable<string>>.FromResult(filenames);
        }

        public async Task<string> LoadTextAsync(string filename) {
            string filepath = GetFilePath(filename);
            using (StreamReader reader = File.OpenText(filepath)) {
                return await reader.ReadToEndAsync();
            }
        }

        public async Task SaveTextAsync(string filename, string text) {
            
            string filepath = GetFilePath(filename);
            //string filepath = Android.OS.Environment.ExternalStorageDirectory.Path + "/myfileLOGdata.txt";
            using (StreamWriter writer = File.AppendText(filepath)) {
                await writer.WriteAsync(text);
            }
        }

        // вспомогательный метод для построения пути к файлу
        public string GetFilePath(string filename) {
            string pathToNewFolder = Android.OS.Environment.ExternalStorageDirectory.Path + foldername;
            Directory.CreateDirectory(pathToNewFolder);
            //return Path.Combine(Android.OS.Environment.ExternalStorageDirectory.Path, filename);
            //return Path.Combine(GetDocsPath(), filename);
            return pathToNewFolder + "/" + filename;
        }


        //// получаем путь к папке MyDocuments
        string GetDocsPath() {
            string pathToNewFolder = Android.OS.Environment.ExternalStorageDirectory.Path + foldername;
            Directory.CreateDirectory(pathToNewFolder);
            //return System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);
            //return System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            //return System.Environment.GetFolderPath(Android.OS.Environment.ExternalStorageDirectory.Path);
            //return Android.OS.Environment.ExternalStorageState; //(System.Environment.SpecialFolder.Personal);
            return Android.OS.Environment.ExternalStorageDirectory.Path + foldername;
        }

        //string GetDocsPath() {
        //    //string pathToNewFolder = Android.OS.Environment.ExternalStorageDirectory.Path + foldername;
        //    //Directory.CreateDirectory(pathToNewFolder);
        //    //return System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);
        //    return System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
        //    //return System.Environment.GetFolderPath(Android.OS.Environment.ExternalStorageDirectory.Path);
        //    //return Android.OS.Environment.ExternalStorageState; //(System.Environment.SpecialFolder.Personal);
        //    //return Android.OS.Environment.ExternalStorageDirectory.Path + foldername;
        //}

        //public string GetFilePath(string filename) {
        //    //string pathToNewFolder = Android.OS.Environment.ExternalStorageDirectory.Path + foldername;
        //    //Directory.CreateDirectory(pathToNewFolder);
        //    //return Path.Combine(Android.OS.Environment.ExternalStorageDirectory.Path, filename);
        //    return Path.Combine(GetDocsPath(), filename);
        //    //return pathToNewFolder + "/" + filename;
        //}
    }
}
