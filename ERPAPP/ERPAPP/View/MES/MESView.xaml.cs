using ERPAPP.Helper;
using ERPAPP.Logic;
using ERPAPP.Model;
using LiveCharts;
using LiveCharts.Wpf;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using System.Windows.Threading;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

namespace ERPAPP.View.MES
{
    /// <summary>
    /// MESView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MESView : Page
    {
        internal IEnumerable<tblMES> prodProcess;
        internal tblItem prodItem;

        internal double totalTime;
        internal double cycleTime;
        internal double workTime;
        internal int planQty;
        internal int prodQty;
        internal int sucess;
        internal int fail;

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
            initDataLoad();
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
            UpdateData(currentData);
            ShowData();
        }

        private void UpdateData(Dictionary<string, string> currentData)
        {
            try
            {
                var Process = new Model.tblMES()
                {
                    ProductionCode = Common.SELECT_Production.ProductionCode,
                    ITEMCode = Common.SELECT_Production.ItemCode,
                    OpIdx = int.Parse(GetProcess(currentData["DEV_ID"], "OpIdx")),
                    MachineCode = GetProcess(currentData["DEV_ID"], "MachineCode"),
                    IoTConnect = currentData["DEV_ID"],
                    Date = DateTime.Now,
                    StartTime = DateTime.Parse((currentData["PRC_Start"])),
                    EndTime = DateTime.Parse(currentData["PRC_End"]),
                    PrepareTime = float.Parse(currentData["PRC_Prepare"]),
                    WorkTime = float.Parse(currentData["PRC_Work"]),
                    TotalTime = float.Parse(currentData["PRC_Total"]),
                    Defect = (currentData["PRC_Defect"] == "Sucess" ? true : false)
                };
                var re = DataAcess.SetMES(Process);

                //생산 수량
                prodQty += 1;
                
                //공정 시간
                workTime += (double)Process.WorkTime;

                // 양품률
                if ((bool)Process.Defect)
                {
                    sucess += 1;
                }
                else
                {
                    fail -= 1;
                }
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

        private void initDataLoad()
        {
            // 현재 생산
            prodProcess = DataAcess.GetMES().Where(i => i.ProductionCode.Equals(Common.SELECT_Production.ProductionCode));

            if (prodProcess.FirstOrDefault() != null)
            {
                // 생산 아이템
                prodItem = DataAcess.GetItems().Where(i => i.ItemCode.Equals(prodProcess.FirstOrDefault().ITEMCode)).FirstOrDefault();

                // 생산 수량
                planQty = DataAcess.GetProductions().Where(i => i.ProductionCode.Equals(prodProcess.FirstOrDefault().ProductionCode)).FirstOrDefault().PlanQuantity;

                // Total CycleTime
                cycleTime = DataAcess.GetOperations().Where(i => i.ItemCode.Equals(prodItem.ItemCode)).Sum(i => i.CycleTime);

                // 이전 전체 작업 시간
                totalTime = (double)prodProcess.Sum(i => i.TotalTime);

                // 이전 전체 작업 시간
                workTime = (double)prodProcess.Sum(i => i.WorkTime);

                // 생산 수량
                prodQty = prodProcess.Count();

                // 양품 수
                sucess = prodProcess.Where(i => i.Defect.Equals(true)).Count();

                // 불량 수
                fail = prodProcess.Where(i => i.Defect.Equals(false)).Count();

                ShowData();
            }
            else
            {
                planQty = DataAcess.GetProductions().Where(i => i.ProductionCode.Equals(Common.SELECT_Production.ProductionCode)).FirstOrDefault().PlanQuantity; 
                lblTarQty.Content = $"목표 수량 : {planQty} 개";
            }
        }

        private void ShowData()
        {
            Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate
            {
                // 타겟 달성율
                lblTarQty.Content = $"목표 수량 : {planQty} 개";
                lblRealQty.Content = $"생산 수량 : {prodQty} 개";
                lvcPerf.Value = Math.Round((double)workTime / ((double)cycleTime * prodQty) * 100);

                // 가용성
                lblTtlTime.Content = $"전체 시간 : {(totalTime / 60).ToString("#.##")} 분";
                lblAvlTime.Content = $"공정 시간 : {(workTime / 60).ToString("#.##")} 분";
                lvcAvail.Value = Math.Round((double)workTime / ((double)totalTime * prodQty) * 100);

                // 양품률
                lblSuc.Content = $"양품 수량 : {sucess} 개";
                lblDef.Content = $"불량 수량 : {fail} 개";
                lvcDef.Value = Math.Round((double)sucess / (double)prodQty * 100);

                lblOEE.Content = $"OEE = {(((lvcPerf.Value/100) * (lvcAvail.Value/100) * (lvcDef.Value/100)) * 100).ToString("#.##")} %";
            }));            
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

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            client.Disconnect();
        }
    }
}
