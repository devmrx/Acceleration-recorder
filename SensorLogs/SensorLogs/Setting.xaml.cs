using DeviceMotion.Plugin.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SensorLogs {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Setting : ContentPage {

        public Setting() {
            InitializeComponent();

            picker.IsEnabled = false;
            picker.IsVisible = false;
        }

        public void SwAdd_Time(object sender, ToggledEventArgs e) {
            App.Current.Properties["timefield"] = e.Value;
        }

        public void SpeedR_Click(object sender, ToggledEventArgs e) {
            picker.Focus();
        }

        void picker_SelectedIndexChanged(object sender, EventArgs e) {

            if (picker.SelectedIndex == 0) {
                App.Current.Properties["SpeedRead"] = MotionSensorDelay.Fastest;
            } else if (picker.SelectedIndex == 1) {
                App.Current.Properties["SpeedRead"] = MotionSensorDelay.Game;
            } else if (picker.SelectedIndex == 2) {
                App.Current.Properties["SpeedRead"] = MotionSensorDelay.Ui;
            } else if (picker.SelectedIndex == 3) {
                App.Current.Properties["SpeedRead"] = MotionSensorDelay.Default;
            } else {
                App.Current.Properties["SpeedRead"] = MotionSensorDelay.Default;
            }
        }
    }
}