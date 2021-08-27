using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Connecting;
using MQTTnet.Client.Options;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AGVControl
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void BtnGo_Clicked(object sender, EventArgs e)
        {
            LblStart.Text = "Go";
            SendData(TxtServer.Text, "G");
        }

        private void BtnStop_Clicked(object sender, EventArgs e)
        {
            LblStart.Text = "Stop";
            SendData(TxtServer.Text, "S");
        }

        private void BtnLefg_Clicked(object sender, EventArgs e)
        {
            LblStart.Text = "Left";
            SendData(TxtServer.Text,"L");
        }

        private void BtnRight_Clicked(object sender, EventArgs e)
        {
            LblStart.Text = "Right";
            SendData(TxtServer.Text,"R");
        }

        private void BtnBack_Clicked(object sender, EventArgs e)
        {
            LblStart.Text = "Back";
            SendData(TxtServer.Text,"B");
        }

        private void Btnone_Clicked(object sender, EventArgs e)
        {
            LblStart.Text = "1";
            SendData(TxtServer.Text,"1");
        }

        private void Btntwo_Clicked(object sender, EventArgs e)
        {
            LblStart.Text = "2";
            SendData(TxtServer.Text,"2");
        }

        private void Btnthree_Clicked(object sender, EventArgs e)
        {
            LblStart.Text = "3";
            SendData(TxtServer.Text,"3");
        }

        private void Btnfore_Clicked(object sender, EventArgs e)
        {
            LblStart.Text = "4";
            SendData(TxtServer.Text,"4");
        }

        private static async Task SendData(string server, string command)
        {
            var mqttClient = new MqttFactory().CreateMqttClient();

            var options = new MqttClientOptionsBuilder().WithTcpServer(server, 1883).Build();

            var message = new MqttApplicationMessageBuilder().WithTopic("MOTOR/TEST/").WithPayload(command).WithExactlyOnceQoS().Build();

            var result = await mqttClient.ConnectAsync(options, CancellationToken.None);

            if (result.ResultCode == MqttClientConnectResultCode.Success)
            {
                await mqttClient.PublishAsync(message);
            }
        }
    }
}
