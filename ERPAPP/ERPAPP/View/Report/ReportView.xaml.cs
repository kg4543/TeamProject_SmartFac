using ERPAPP.Helper;
using ERPAPP.Logic;
using ERPAPP.Model;
using ERPAPP.View.MES;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ERPAPP.View.Report
{
    /// <summary>
    /// ReportView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ReportView : Page
    {
        public ReportView()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            SelectProduction();
        }

        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            SelectProduction();
        }

        private void SelectProduction()
        {
            ProductionSearch Search = new ProductionSearch();
            Search.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            Search.ShowDialog();

            if (Common.SELECT_Production != null)
            {
                lblProduction.Content = "생산 : " + Common.SELECT_Production.ProductionCode;
                LoadData();
            }
            else
            {
                NavigationService.Navigate(null);
            }
        }

        private void LoadData()
        {
            var production = Common.SELECT_Production.ProductionCode.ToString();
            var list = DataAcess.GetReport(production);
            DisplayChart(list);
        }

        private void DisplayChart(IEnumerable<tblReport> list)
        {
            string[] dates = list.Select(a => a.Date.ToString("yy-MM-dd")).ToArray();
            var quantity = list.Select(a => a.Quantity).ToArray();

            //범례 위치 설정
            chart.LegendLocation = LiveCharts.LegendLocation.Top;

            chart.AxisX.Clear();
            chart.AxisY.Clear();

            //세로 눈금 값 설정
            chart.AxisY.Add(new LiveCharts.Wpf.Axis { MinValue = 0, MaxValue = 50 });

            //가로 눈금 값 설정
            chart.AxisX.Add(new LiveCharts.Wpf.Axis { Labels = dates });

            //모든 항목 지우기
            chart.Series.Clear();

            chart.Series.Add(new LiveCharts.Wpf.LineSeries()
            {
                Title = "생산수량",
                Stroke = new SolidColorBrush(Colors.Green),
                Values = new LiveCharts.ChartValues<int>(quantity)
            });
        }    }
}
