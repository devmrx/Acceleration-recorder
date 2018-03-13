using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Net;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Forms;

[assembly: Dependency(typeof(SensorLogs.Droid.Share))]
namespace SensorLogs.Droid {
    public class Share : IShare {
        private readonly Context _context;
        public Share() {
            _context = Android.App.Application.Context;
        }

        public Task Show(string title, string message, string filePath) {
            var extension = filePath.Substring(filePath.LastIndexOf(".") + 1).ToLower();
            var contentType = string.Empty;

            // You can manually map more ContentTypes here if you want.
            switch (extension) {
                case "pdf":
                    contentType = "application/pdf";
                    break;
                case "png":
                    contentType = "image/png";
                    break;
                default:
                    contentType = "application/octetstream";
                    break;
            }

            var intent = new Intent(Intent.ActionSend);
            intent.SetType(contentType);
            intent.PutExtra(Intent.ExtraStream, Uri.Parse("file://" + filePath));
            intent.PutExtra(Intent.ExtraText, string.Empty);
            intent.PutExtra(Intent.ExtraSubject, message ?? string.Empty);

            var chooserIntent = Intent.CreateChooser(intent, title ?? string.Empty);
            chooserIntent.SetFlags(ActivityFlags.ClearTop);
            chooserIntent.SetFlags(ActivityFlags.NewTask);
            _context.StartActivity(chooserIntent);

            return Task.FromResult(true);
        }
    }
}