using DeviceMotion.Plugin;
using DeviceMotion.Plugin.Abstractions;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Diagnostics;

namespace SensorLogs {
    public class TaskCounter {

        private BigInteger count = 0;
        private string filename = "";
        private string Time = "";
        Stopwatch stopWatch = new Stopwatch();

        public async Task RunCounter(CancellationToken token) {
            
            if (filename == "") filename = "Log - " + DateTime.Now.ToShortTimeString();
            

            await Task.Run(() => {

                token.ThrowIfCancellationRequested();

                string X, Y, Z;

                CrossDeviceMotion.Current.SensorValueChanged += (s, a) => {

                    if ((bool)App.Current.Properties["timefield"]) {
                        stopWatch.Start();
                        switch (a.SensorType) {
                            case MotionSensorType.Accelerometer:

                                X = ((MotionVector)a.Value).X.ToString("F");
                                Y = ((MotionVector)a.Value).Y.ToString("F");
                                Z = ((MotionVector)a.Value).Z.ToString("F");
                                count++;
                                
                                Save($"{X}|{Y}|{Z}|" + stopWatch.Elapsed.Milliseconds + Environment.NewLine);
                                stopWatch.Stop();
                                stopWatch.Reset();
                                var message = new TickedMessage {
                                    Message = $"x = {X}, y = {Y}, z = {Z}",
                                    Count = count
                                };

                                Device.BeginInvokeOnMainThread(() => {
                                    MessagingCenter.Send<TickedMessage>(message, "TickedMessage");
                                });

                                break;
                        }
                    } else {
                        switch (a.SensorType) {
                            case MotionSensorType.Accelerometer:

                                X = ((MotionVector)a.Value).X.ToString("F");
                                Y = ((MotionVector)a.Value).Y.ToString("F");
                                Z = ((MotionVector)a.Value).Z.ToString("F");
                                count++;
                                

                                Save($"{X}|{Y}|{Z}" + Environment.NewLine);
                               
                                var message = new TickedMessage {
                                    Message = $"x = {X}, y = {Y}, z = {Z}",
                                    Count = count
                                };

                                Device.BeginInvokeOnMainThread(() => {
                                    MessagingCenter.Send<TickedMessage>(message, "TickedMessage");
                                });

                                break;
                        }
                    }           
                };
            }, token);
        }

        async void Save(string text) {

            if (String.IsNullOrEmpty(filename)) return;
            // если файл не существует
            if (!await DependencyService.Get<IFileWorker>().ExistsAsync(filename)) {
                // запрашиваем разрешение на перезапись
                //bool isRewrited = await DisplayAlert("Подверждение", "Файл уже существует, перезаписать его?", "Да", "Нет");
                //if (isRewrited == false) return;
                count = 0;
            }
            // перезаписываем файл
            await DependencyService.Get<IFileWorker>().SaveTextAsync(filename, text);
            // обновляем список файлов
        }

    }
}
