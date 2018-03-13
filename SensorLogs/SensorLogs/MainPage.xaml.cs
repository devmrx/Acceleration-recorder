using DeviceMotion.Plugin;
using DeviceMotion.Plugin.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace SensorLogs {
    public class StartLongRunningTaskMessage { }

    public class StopLongRunningTaskMessage { }

    public class TickedMessage {
        public string Message { get; set; }
        public BigInteger Count { get; set; }
    }

    public class CancelledMessage { }

    public partial class MainPage : ContentPage {

        
        private BigInteger count = 0;

        public MainPage() {
            InitializeComponent();


            ToolbarItem tb1 = new ToolbarItem {
                Text = "Настройки",
                Order = ToolbarItemOrder.Secondary,
                Priority = 1
            };

            ToolbarItem tb2 = new ToolbarItem {
                Text = "Об авторе",
                Order = ToolbarItemOrder.Secondary,
                Priority = 2
            };

            ToolbarItem tb3 = new ToolbarItem {
                Text = "О программе",
                Order = ToolbarItemOrder.Secondary,
                Priority = 3
            };

            tb1.Clicked += (s, e) => {
                Navigation.PushAsync(new Setting());
            };

            ToolbarItems.Add(tb1);
            ToolbarItems.Add(tb2);
            ToolbarItems.Add(tb3);


            object name = "";
            if (!App.Current.Properties.TryGetValue("timefield", out name)) {

                App.Current.Properties["timefield"] = false;

            }

            if (!App.Current.Properties.TryGetValue("SpeedRead", out name)) {

                App.Current.Properties["SpeedRead"] = MotionSensorDelay.Ui;

            }


            startLongRunningTask.Clicked += async (s, e) => {

                //if ((MotionSensorDelay)App.Current.Properties["SpeedRead"] == MotionSensorDelay.Ui) DisplayAlert("Delay", 60.ToString(), "OK");

                CrossDeviceMotion.Current.Start(MotionSensorType.Accelerometer, ((MotionSensorDelay)App.Current.Properties["SpeedRead"]));

                var message = new StartLongRunningTaskMessage();
                MessagingCenter.Send(message, "StartLongRunningTaskMessage");

                await UpdateFileList();
            };

            stopLongRunningTask.Clicked += async (s, e) => {
                CrossDeviceMotion.Current.Stop(MotionSensorType.Accelerometer);
                var message = new StopLongRunningTaskMessage();
                MessagingCenter.Send(message, "StopLongRunningTaskMessage");
                await UpdateFileList();
            };

            HandleReceivedMessages();

            
        }

        async void HandleReceivedMessages() {
            MessagingCenter.Subscribe<TickedMessage>(this, "TickedMessage", message => {
                

                Device.BeginInvokeOnMainThread(() => {

                    Posit.Text = message.Message;
                    Lbl_Count.Text = "Кол-во записей - " + message.Count.ToString();
                });
            });

            MessagingCenter.Subscribe<CancelledMessage>(this, "CancelledMessage", message => {
                Device.BeginInvokeOnMainThread(() => {
                    
                    Posit.Text = "Cancelled";
                    
                });
            });

            await UpdateFileList();
        }

        

        async void FileSelect(object sender, SelectedItemChangedEventArgs args) {
            if (args.SelectedItem == null) return;
            // получаем выделенный элемент
            string filename = (string)args.SelectedItem;
            // загружем текст в текстовое поле    
            //textEditor.Text = await DependencyService.Get<IFileWorker>().LoadTextAsync((string)args.SelectedItem);
            // снимаем выделение
            filesList.SelectedItem = null;


            await Navigation.PushAsync(new FileOperation(filename));
        }

        // обновление списка файлов
        async Task UpdateFileList() {

            Lbl_Sp.Text = "Список файлов";
            // получаем все файлы
            filesList.ItemsSource = await DependencyService.Get<IFileWorker>().GetFilesAsync();
            // снимаем выделение
            filesList.SelectedItem = null;
        }

    }
}
