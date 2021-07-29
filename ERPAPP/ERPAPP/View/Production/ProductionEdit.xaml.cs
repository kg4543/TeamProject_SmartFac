using ERPAPP.Helper;
using ERPAPP.Logic;
using ERPAPP.Model;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Media.Imaging;

namespace ERPAPP.View.Production
{
    /// <summary>
    /// ProductionEdit.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ProductionEdit : MetroWindow
    {
        public ProductionEdit()
        {
            InitializeComponent();
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            DataClear();
            DataLoad();
        }

        private void BtnOrderSerch_Click(object sender, RoutedEventArgs e)
        {
            OrderSearch OrderSearch = new OrderSearch();
            OrderSearch.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            OrderSearch.ShowDialog();

            if (Common.SELECT_ORDER != null)
            {
                var selOrder = Common.SELECT_ORDER;
                TxtOrder.Text = selOrder.OrderCode;
                TxtItem.Text = DataAcess.GetOrders().Where(i => i.OrderCode.Equals(selOrder.OrderCode)).FirstOrDefault().ItemCode;
                Image();
            }
        }

        private async void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (IsValid())
            {
                var production = Common.SELECT_Production;

                production.FactoryCode = CmbFactory.Text;
                production.OrderCode = TxtOrder.Text;
                production.ItemCode = TxtItem.Text;
                production.StartDate = DateTime.Parse(DtpStartdate.Text);
                production.EndDate = DateTime.Parse(DtpEnddate.Text);
                production.PlanQuantity = (int)NumPlanQty.Value;
                production.FQuantity = (int)NumFinishQty.Value;

                DataAcess.SetProductions(production);


                this.ShowMessageAsync("데이터 수정", "생산계획이 수정되었습니다.");
                Close();
            }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void DataClear()
        {
            TxtCode.Text = TxtOrder.Text = TxtItem.Text = string.Empty;
            CmbFactory.SelectedItem = DtpStartdate.SelectedDate = DtpEnddate.SelectedDate = null;
            NumPlanQty.Value = NumFinishQty.Value = 0;
            Common.SELECT_ORDER = null;
            Image();
        }

        private void DataLoad()
        {
            var factory = DataAcess.GetFactorys();
            foreach (var item in factory)
                CmbFactory.Items.Add(item.FactoryCode);

            var selectedProd = Common.SELECT_Production;

            TxtCode.Text = selectedProd.ProductionCode.ToString();
            CmbFactory.Text = selectedProd.FactoryCode.ToString();
            TxtOrder.Text = selectedProd.OrderCode.ToString();
            TxtItem.Text = selectedProd.ItemCode.ToString();
            DtpStartdate.SelectedDate = DateTime.Parse(selectedProd.StartDate.ToString());
            DtpEnddate.SelectedDate = DateTime.Parse(selectedProd.EndDate.ToString());
            NumPlanQty.Value = selectedProd.PlanQuantity;
            NumFinishQty.Value = selectedProd.FQuantity;
            Image();
        }

        private void Image()
        {
            if (TxtItem.Text != string.Empty)
            {
                var selectedItem = DataAcess.GetItems().Where(i => i.ItemCode.Equals(TxtItem.Text)).FirstOrDefault();

                if (selectedItem.ItemImage != null)
                {
                    BitmapImage bitmapImage = new BitmapImage(new Uri(selectedItem.ItemImage, UriKind.RelativeOrAbsolute));
                    ImgItem.Source = bitmapImage;
                }
                else
                {
                    BitmapImage bitmapImage = new BitmapImage(new Uri("/Resources/No_Picture.jpg", UriKind.RelativeOrAbsolute));
                    ImgItem.Source = bitmapImage;
                }
            }
            else
            {
                BitmapImage bitmapImage = new BitmapImage(new Uri("/Resources/No_Picture.jpg", UriKind.RelativeOrAbsolute));
                ImgItem.Source = bitmapImage;
            }
        }

        private bool IsValid()
        {
            if (string.IsNullOrEmpty(TxtOrder.Text))
            {
                this.ShowMessageAsync("입력오류", "가격을 입력해주세요.");
                return false;
            }
            if (DtpStartdate.SelectedDate == null)
            {
                this.ShowMessageAsync("입력오류", "생산 시작 예정일을 입력해주세요.");
                return false;
            }
            if (DtpEnddate.SelectedDate == null)
            {
                this.ShowMessageAsync("입력오류", "생산 종료 예정일을 입력해주세요.");
                return false;
            }
            if (DtpEnddate.SelectedDate < DtpStartdate.SelectedDate)
            {
                this.ShowMessageAsync("입력오류", "종료 예정일이 시작 예정일보다 빠릅니다.");
                return false;
            }
            if (CmbFactory.SelectedItem == null)
            {
                this.ShowMessageAsync("입력오류", "생산 공장을 입력해주세요.");
                return false;
            }
            if (NumPlanQty.Value <= 0)
            {
                this.ShowMessageAsync("입력오류", "수량이 0보다 작습니다.");
                return false;
            }

            // 오더 수량
            var order = DataAcess.GetOrders().Where(i => i.OrderCode.Equals(TxtOrder.Text)).FirstOrDefault();

            // 해당 오더로 세운 생산 계획 수량
            var prod = DataAcess.GetProductions().Where(i => i.OrderCode.Equals(TxtOrder.Text)).ToList();
            int sum = 0;
            foreach (var item in prod)
            {
                sum += item.PlanQuantity;
            }
            sum -= Common.SELECT_Production.PlanQuantity; //수정 전 수량은 빼준다.

            // 오더 수량이 생산 수량보다 넘을 경우
            if (order.Quantity < sum + (int)NumPlanQty.Value)
            {
                this.ShowMessageAsync("입력오류", "오더 수량보다 계획 수량이 더 많습니다.");
                return false;
            }
            if (order.ShipDate < DtpEnddate.SelectedDate)
            {
                this.ShowMessageAsync("입력오류", "선적일보다 생산 종료 예정일이 더 늦습니다.");
                return false;
            }

            return true;
        }
    }
}
