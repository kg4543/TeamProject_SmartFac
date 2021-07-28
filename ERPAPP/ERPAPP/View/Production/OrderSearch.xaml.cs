using ERPAPP.Helper;
using ERPAPP.Logic;
using ERPAPP.Model;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ERPAPP.View.Production
{
    /// <summary>
    /// OrderSearch.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class OrderSearch : MetroWindow
    {
        public OrderSearch()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
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
            if (e.Key == Key.Enter)
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
                    //var selectedItem = GrdData.SelectedItem as tblOrder; 

                    /*TxtCode.Text = selectedItem.OrderCode.ToString();
                    TxtBrandCode.Text = selectedItem.BrandCode.ToString();
                    TxtItemCode.Text = selectedItem.ItemCode.ToString();
                    TxtDestination.Text = selectedItem.Destination.ToString();
                    TxtShipDate.Text = selectedItem.ShipDate.ToString();
                    TxtQuantity.Text = selectedItem.Quantity.ToString();
                    TxtUnitPrice.Text = selectedItem.UnitPrice.ToString();*/
                }
            }

            catch (Exception ex)
            {
                Common.logger.Error($"오더 데이터 선택 Error : {ex}");
                throw ex;
            }
        }

        private void BtnOK_Click(object sender, RoutedEventArgs e)
        {
            if (GrdData.SelectedItem != null)
            {
                Common.SELECT_ORDER = GrdData.SelectedItem as tblOrder;
                Close();
            }
            else
            {
                this.ShowMessageAsync("선택오류", "오더를 선택하세요.");
            }

        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
