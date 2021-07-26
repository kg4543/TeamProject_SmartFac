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

namespace ERPAPP.View.Factory.Worker
{
    /// <summary>
    /// WorkerView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class WorkerView : Page
    {
        public WorkerView()
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
                List<tblWorker> Worker = new List<tblWorker>();
                Worker = DataAcess.GetWorker();
                DataContext = Worker;
            }
            catch (Exception ex)
            {
                Common.logger.Error($"Category 화면 로드 Error : {ex}");
                throw ex;
            }
        }

        private void DataClear()
        {
            TxtCode.Text = TxtName.Text = TxtFactory.Text = TxtPhone.Text = TxtAddr.Text = string.Empty;
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
                //검색한 아이템이 있는지 확인
                DataContext = DataAcess.GetWorker().Where(i => i.WorkerCode.Trim().Contains(searchCode)
                                                        & i.WorkerName.Trim().Contains(searchName)).ToList();
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
                    Common.SELECT_Worker = GrdData.SelectedItem as tblWorker;
                    var selectedItem = Common.SELECT_Worker;

                    TxtCode.Text = selectedItem.WorkerCode.ToString();
                    TxtName.Text = selectedItem.WorkerName.ToString();
                    TxtFactory.Text = selectedItem.FactoryCode.ToString();
                    TxtPhone.Text = $"{selectedItem.WorkerPhone.Substring(0, 3)} - {selectedItem.WorkerPhone.Substring(3, 4)} - {selectedItem.WorkerPhone.Substring(7, 4)}";
                    TxtAddr.Text = selectedItem.WorkerAddr.ToString();
                }
            }
            catch (Exception ex)
            {
                Common.logger.Error($"직원 데이터 선택 Error : {ex}");
                throw ex;
            }
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            WorkerAdd WorkerAdd = new WorkerAdd();
            WorkerAdd.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            WorkerAdd.ShowDialog();
            DataClear();
            DataLoad();
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (GrdData.SelectedItem != null)
            {
                WorkerEdit WorkerEdit = new WorkerEdit();
                WorkerEdit.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                WorkerEdit.ShowDialog();
                DataClear();
                DataLoad();
            }
            else
            {
                Common.ShowMessageAsync("데이터 선택", "직원을 선택해주세요.");
            }
        }
    }
}
