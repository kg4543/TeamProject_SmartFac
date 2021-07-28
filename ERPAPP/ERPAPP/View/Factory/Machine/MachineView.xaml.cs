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

namespace ERPAPP.View.Factory.Machine
{
    /// <summary>
    /// MachineView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MachineView : Page
    {
        public MachineView()
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
                List<tblMachine> Machine = new List<tblMachine>();
                Machine = DataAcess.GetMachines();
                DataContext = Machine;
            }
            catch (Exception ex)
            {
                Common.logger.Error($"Category 화면 로드 Error : {ex}");
                throw ex;
            }
        }

        private void DataClear()
        {
            TxtCode.Text = TxtName.Text = TxtCategory.Text = TxtFactory.Text = string.Empty;
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
                DataContext = DataAcess.GetMachines().Where(i => i.MachineCode.Trim().Contains(searchCode)
                                                        & i.MachineName.Trim().Contains(searchName)).ToList();
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
                    Common.SELECT_Machine = GrdData.SelectedItem as tblMachine;
                    var selectedItem = Common.SELECT_Machine;

                    TxtCode.Text = selectedItem.MachineCode.ToString();
                    TxtName.Text = selectedItem.MachineName.ToString();
                    TxtCategory.Text = selectedItem.MCateCode.ToString();
                    TxtFactory.Text = selectedItem.FactoryCode.ToString();
                }
            }
            catch (Exception ex)
            {
                Common.logger.Error($"기계 데이터 선택 Error : {ex}");
                throw ex;
            }
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            MachineAdd MachineAdd = new MachineAdd();
            MachineAdd.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            MachineAdd.ShowDialog();
            DataClear();
            DataLoad();
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (GrdData.SelectedItem != null)
            {
                MachineEdit MachineEdit = new MachineEdit();
                MachineEdit.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                MachineEdit.ShowDialog();
                DataClear();
                DataLoad();
            }
            else
            {
                Common.ShowMessageAsync("데이터 선택", "기계를 선택해주세요.");
            }
        }
    }
}
