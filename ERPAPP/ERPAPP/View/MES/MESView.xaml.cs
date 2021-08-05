using ERPAPP.Helper;
using ERPAPP.Logic;
using LiveCharts;
using LiveCharts.Wpf;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace ERPAPP.View.MES
{
    /// <summary>
    /// MESView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MESView : Page
    {
        public MESView()
        {
            InitializeComponent();

            PointLabel = chartPoint =>
                string.Format("{0} ({1:P})", chartPoint.Y, chartPoint.Participation);

            DataContext = this;
        }

        public Func<ChartPoint, string> PointLabel { get; set; }

        private void Chart_OnDataClick(object sender, ChartPoint chartpoint)
        {
            var chart = (LiveCharts.Wpf.PieChart)chartpoint.ChartView;

            //clear selected slice.
            foreach (PieSeries series in chart.Series)
                series.PushOut = 0;

            var selectedSeries = (PieSeries)chartpoint.SeriesView;
            selectedSeries.PushOut = 8;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            SelectProduction();
            InitConnectMqttBroker();
        }

        MqttClient client;

        private void InitConnectMqttBroker()
        {
            var brokerAddress = IPAddress.Parse("210.119.12.92");
            client = new MqttClient(brokerAddress);
            client.MqttMsgPublishReceived += Client_MqttMsgPublishReceived;
            client.Connect("Monitor");
            client.Subscribe(new string[] { "factory1/machine1/data/" }, new byte[] { MqttMsgBase.QOS_LEVEL_AT_MOST_ONCE });
        }

        Dictionary<string, string> currentData = new Dictionary<string, string>();

        private void Client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            var message = Encoding.UTF8.GetString(e.Message);
            currentData = JsonConvert.DeserializeObject<Dictionary<string, string>>(message);
            InsertData(currentData);
        }

        private void InsertData(Dictionary<string, string> currentData)
        {
            try
            {
                var Process = new Model.tblMES()
                {
                    ProductionCode = Common.SELECT_Production.ProductionCode,
                    OpIdx = int.Parse(GetProcess(currentData["DEV_ID"], "OpIdx")),
                    MachineCode = GetProcess(currentData["DEV_ID"], "MachineCode"),
                    IoTConnect = currentData["DEV_ID"],
                    Date = DateTime.Now,
                    StartTime = DateTime.Parse((currentData["PRC_Start"])),
                    EndTime = DateTime.Parse(currentData["PRC_End"]),
                    WorkTime = float.Parse(currentData["PRC_Work"]),
                    Defect = (currentData["PRC_Defect"] == "W" ? true : false)
                };
                var re = DataAcess.SetMES(Process);
            }
            catch (Exception ex)
            {
                Common.logger.Error($"데이터 업데이트 에러 : {ex}");
                throw ex;
            }
        }

        private string GetProcess(string devId, string attr)
        {
            if (attr == "OpIdx")
            {
                switch (devId)
                {
                    case "MACHINE01":
                        return "1";
                    case "MACHINE02":
                        return "2";
                }
            }
            else
            {
                switch (devId)
                {
                    case "MACHINE01":
                        return "ML001";
                    case "MACHINE02":
                        return "DEF001";
                }
            }
            return null;
        }

        private void UpdateData()
        {
            var workProcess = Logic.DataAcess.GetMES();
        }

        private void SelectProduction()
        {
            ProductionSearch Search = new ProductionSearch();
            Search.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            Search.ShowDialog();

            if (Common.SELECT_Production != null)
            {
                LblSchedule.Content = Common.SELECT_Production.ProductionCode + " / " + Common.SELECT_Production.StartDate.ToString("yyyy-MM-dd");
            }
            else
            {
                NavigationService.Navigate(null);
            }
        }
    }
}
