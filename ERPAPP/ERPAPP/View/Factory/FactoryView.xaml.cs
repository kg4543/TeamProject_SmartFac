using ERPAPP.Helper;
using ERPAPP.Logic;
using ERPAPP.Model;
using ERPAPP.View.Factory.Machine;
using ERPAPP.View.Factory.Worker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;

namespace ERPAPP.View.Factory
{
    /// <summary>
    /// FactoryView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class FactoryView : Page
    {
        public FactoryView()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            DataClear();
            DataLoad(); //그리드 데이터 로드
        }

        private void DataLoad()
        {
            try
            {
                List<tblFactory> Factory = new List<tblFactory>();
                Factory = DataAcess.GetFactorys();
                DataContext = Factory;
            }
            catch (Exception ex)
            {
                Common.logger.Error($"Category 화면 로드 Error : {ex}");
                throw ex;
            }
        }

        private void DataClear()
        {
            TxtCode.Text = TxtName.Text = TxtWorkerQty.Text = TxtMachineQty.Text = string.Empty;
            GrdData.SelectedItem = null;
        }


        private void TxtSearchCode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                TxtSearchName.Focus();
        }

        private void TxtSearchName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                BtnSearch_Click(sender, e);
        }

        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            //검색 내용
            string searchCode = TxtSearchCode.Text;
            string searchName = TxtSearchName.Text;

            try
            {
                DataContext = DataAcess.GetFactorys().Where(i => i.FactoryCode.Trim().Contains(searchCode)
                                                        & i.FactoryName.Trim().Contains(searchName)).ToList();
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
                    Common.SELECT_Factory = GrdData.SelectedItem as tblFactory;
                    var selectedItem = Common.SELECT_Factory;
                    var WorkerQty = DataAcess.GetWorkers().Where(i => i.FactoryCode.Trim().Equals(selectedItem.FactoryCode.Trim().ToString())).Count();
                    var MachineQty = DataAcess.GetMachines().Where(i => i.FactoryCode.Trim().Equals(selectedItem.FactoryCode.Trim().ToString())).Count();

                    TxtCode.Text = selectedItem.FactoryCode.ToString();
                    TxtName.Text = selectedItem.FactoryName.ToString();
                    TxtWorkerQty.Text = $"{WorkerQty} 명";
                    TxtMachineQty.Text = $"{MachineQty} 대";
                }
            }
            catch (Exception ex)
            {
                Common.logger.Error($"공장 데이터 선택 Error : {ex}");
                throw ex;
            }
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            FactoryAdd factoryAdd = new FactoryAdd();
            factoryAdd.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            factoryAdd.ShowDialog();
            DataClear();
            DataLoad();
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (GrdData.SelectedItem != null)
            {
                FactoryEdit factoryEdit = new FactoryEdit();
                factoryEdit.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                factoryEdit.ShowDialog();
                DataClear();
                DataLoad();
            }
            else
            {
                Common.ShowMessageAsync("데이터 선택", "공장을 선택해주세요.");
            }
        }
        
        private void BtnWorker_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                NavigationService.Navigate(new WorkerView());
            }
            catch (Exception ex)
            {
                Common.logger.Error($"예외발생 BtnAccount_Click : {ex}");
                Common.ShowMessageAsync("예외", $"예외발생 : {ex}");
            }
        }

        private void BtnMachine_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                NavigationService.Navigate(new MachineView());
            }
            catch (Exception ex)
            {
                Common.logger.Error($"예외발생 BtnAccount_Click : {ex}");
                Common.ShowMessageAsync("예외", $"예외발생 : {ex}");
            }
        }
    }
}
