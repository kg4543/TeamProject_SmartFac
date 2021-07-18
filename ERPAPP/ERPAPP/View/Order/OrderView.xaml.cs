using ERPAPP.Helper;
using ERPAPP.Logic;
using ERPAPP.Model;
using ERPAPP.View.Brand;
using ERPAPP.View.ITEM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ERPAPP.View.Order
{
    /// <summary>
    /// OrderView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class OrderView : Page
    {
        public OrderView()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            DataClear();
            DataLoad();
        }

        private void DataLoad()
        {
            try
            {
                List<tblOrder> Items = new List<tblOrder>();
                Items = DataAcess.GetOrders();
                DataContext = Items;
            }
            catch (Exception ex)
            {
                Common.logger.Error($"ORDER 화면 로드 Error : {ex}");
                throw ex;
            }
        }

        private void TxtSearchCode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                DtpShipment.Focus();
        }

        private void DtpShipment_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) //need click checking
                BtnSearch_Click(sender, e);
        }

        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            //검색 내용
            string searchCode = TxtSearchCode.Text;
            string searchDate = DtpShipment.Text;

            try
            {
                if (string.IsNullOrEmpty(searchDate))
                {
                    DataContext = DataAcess.GetOrders().Where(i => i.OrderCode.Trim().Contains(searchCode)).ToList();
                }
                else
                {
                    DataContext = DataAcess.GetOrders().Where(i => i.OrderCode.Trim().Contains(searchCode)
                                                        & i.ShipDate.Equals(DateTime.Parse(searchDate))).ToList();
                }
            }
            catch (Exception ex)
            {
                Common.logger.Error($"검색 로드 Error : {ex}");
                throw ex;
            }
        }

        private void GrdData_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            try
            {
                if (GrdData.SelectedItem != null)
                {
                    //선택한 아이템을 외부에서 사용하도록 지정
                    Common.SELECT_ORDER = GrdData.SelectedItem as tblOrder;
                    var selectedItem = Common.SELECT_ORDER;

                    //선택한 아이템 정보를 로드
                    TxtCode.Text = selectedItem.OrderCode.ToString();
                    TxtBrandCode.Text = selectedItem.BrandCode.ToString();
                    TxtItemCode.Text = selectedItem.ItemCode.ToString();
                    TxtDestination.Text = selectedItem.Destination.ToString();
                    TxtShipDate.Text = selectedItem.ShipDate.ToString();
                    TxtQuantity.Text = selectedItem.Quantity.ToString();
                    TxtUnitPrice.Text = selectedItem.UnitPrice.ToString();
                }
            }

            catch (Exception ex)
            {
                Common.logger.Error($"오더 데이터 선택 Error : {ex}");
                throw ex;
            }
        }

        private async void BtnBrand_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                NavigationService.Navigate(new BrandView());
            }
            catch (Exception ex)
            {
                Common.logger.Error($"예외발생 BtnAccount_Click : {ex}");
                await Common.ShowMessageAsync("예외", $"예외발생 : {ex}");
            }
        }

        private async void BtnItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                NavigationService.Navigate(new ItemMain());
            }
            catch (Exception ex)
            {
                Common.logger.Error($"예외발생 BtnAccount_Click : {ex}");
                await Common.ShowMessageAsync("예외", $"예외발생 : {ex}");
            }
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            OrderAdd OrderAdd = new OrderAdd();
            OrderAdd.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            OrderAdd.ShowDialog();
            DataClear(); //선택값 초기화
            DataLoad(); //데이터 로드
        }

        private void DataClear()
        {
            TxtCode.Text = TxtBrandCode.Text = TxtItemCode.Text = TxtShipDate.Text = TxtDestination.Text =
                TxtQuantity.Text = TxtUnitPrice.Text = string.Empty;

            GrdData.SelectedItem = null;
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (GrdData.SelectedItem != null)
            {
                OrderEdit orderEdit = new OrderEdit();
                orderEdit.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                orderEdit.ShowDialog();
                DataClear(); //선택값 초기화
                DataLoad(); //데이터 로드
            }
            else
            {
                Common.ShowMessageAsync("데이터 선택", "아이템을 선택해주세요.");
            }
        }

        
    }
}
