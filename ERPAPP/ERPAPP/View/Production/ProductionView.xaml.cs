using ERPAPP.Helper;
using ERPAPP.Logic;
using ERPAPP.Model;
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

namespace ERPAPP.View.Production
{
    /// <summary>
    /// ProductionView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ProductionView : Page
    {
        public ProductionView()
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
                List<tblProduction> Items = new List<tblProduction>();
                Items = DataAcess.GetProductions();
                DataContext = Items;
            }
            catch (Exception ex)
            {
                Common.logger.Error($"Production 화면 로드 Error : {ex}");
                throw ex;
            }
        }

        private void DataClear()
        {
            TxtCode.Text = TxtFactory.Text = TxtOrder.Text = TxtItem.Text = TxtStartDate.Text =
                TxtEndDate.Text = TxtPlanQty.Text = TxtFinishQty.Text = string.Empty;

            GrdData.SelectedItem = null;
        }

        private void TxtSearchCode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                DtpStart.Focus();
        }

        private void DtpStart_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) 
                BtnSearch_Click(sender, e);
        }

        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            //검색 내용
            string searchCode = TxtSearchCode.Text;
            string searchDate = DtpStart.Text;

            try
            {
                if (string.IsNullOrEmpty(searchDate))
                {
                    DataContext = DataAcess.GetProductions().Where(i => i.ProductionCode.Trim().Contains(searchCode)).ToList();
                }
                else
                {
                    DataContext = DataAcess.GetProductions().Where(i => i.ProductionCode.Trim().Contains(searchCode)
                                                        & i.StartDate.Equals(DateTime.Parse(searchDate))).ToList();
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
                    Common.SELECT_Production = GrdData.SelectedItem as tblProduction;
                    var selectedItem = Common.SELECT_Production;

                    TxtCode.Text = selectedItem.ProductionCode.ToString();
                    TxtFactory.Text = selectedItem.FactoryCode.ToString();
                    TxtOrder.Text = selectedItem.OrderCode.ToString();
                    TxtItem.Text = selectedItem.ItemCode.ToString();
                    TxtStartDate.Text = selectedItem.StartDate.ToString();
                    TxtEndDate.Text = selectedItem.EndDate.ToString();
                    TxtPlanQty.Text = selectedItem.PlanQuantity.ToString();
                    TxtFinishQty.Text = selectedItem.FQuantity.ToString();
                }
            }

            catch (Exception ex)
            {
                Common.logger.Error($"오더 데이터 선택 Error : {ex}");
                throw ex;
            }
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            ProductionAdd ProductionAdd = new ProductionAdd();
            ProductionAdd.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            ProductionAdd.ShowDialog();
            DataClear(); //선택값 초기화
            DataLoad(); //데이터 로드
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (GrdData.SelectedItem != null)
            {
                ProductionEdit ProductionEdit = new ProductionEdit();
                ProductionEdit.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                ProductionEdit.ShowDialog();
                DataClear(); //선택값 초기화
                DataLoad(); //데이터 로드
            }
            else
            {
                Common.ShowMessageAsync("데이터 선택", "생산일정을 선택해주세요.");
            }
        }
    }
}
