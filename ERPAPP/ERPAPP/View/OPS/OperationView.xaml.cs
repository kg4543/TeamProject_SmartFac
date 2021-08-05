using ERPAPP.Helper;
using ERPAPP.Logic;
using ERPAPP.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace ERPAPP.View.OPS
{
    /// <summary>
    /// OperationView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class OperationView : Page
    {
        public OperationView()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            DataClear();
            DataLoad();
        }

        private void DataClear()
        {
            GrdData.SelectedItem = null;
        }

        private void DataLoad()
        {
            try
            {
                //Item DB List 생성
                List<tblItem> Items = new List<tblItem>();
                Items = DataAcess.GetItems();
                //데이터 그리드 바인딩에 Item DB 정보 로드
                DataContext = Items;
            }
            catch (Exception ex)
            {
                Common.logger.Error($"ITEM 화면 로드 Error : {ex}");
                throw ex;
            }
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
            string searchCode = TxtSearchCode.Text;
            string searchName = TxtSearchName.Text;

            try
            {
                DataContext = DataAcess.GetItems().Where(i => i.ItemCode.Trim().Contains(searchCode)
                                                        & i.ItemName.Trim().Contains(searchName)).ToList();
            }
            catch (Exception ex)
            {
                Common.logger.Error($"검색 로드 Error : {ex}");
                throw ex;
            }
        }

        private void GrdData_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            OpCanvas.Children.Clear();

            if (GrdData.SelectedItem != null)
            {
                var selectedItem = GrdData.SelectedItem as tblItem;
                var operations = DataAcess.GetOperations().Where(i => i.ItemCode.Trim().Equals(selectedItem.ItemCode.Trim()));
                foreach (var item in operations)
                {
                    TextBlock opname = new TextBlock();
                    opname.Text = operations.FirstOrDefault().OpIdx + ". " + operations.FirstOrDefault().OpName;
                    
                    Rectangle rec = new Rectangle();//사각형 생성
                    rec.Width = 100;
                    rec.Height = 100;
                    rec.Stroke = Brushes.SkyBlue;

                    this.OpCanvas.Children.Add(rec);
                    this.OpCanvas.Children.Add(opname);
                }
            }
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            OperationAdd operationAdd = new OperationAdd();
            operationAdd.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            operationAdd.ShowDialog();
            DataClear();
            DataLoad();
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (GrdData.SelectedItem != null)
            {
                OperationEdit operationEdit = new OperationEdit();
                operationEdit.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                operationEdit.ShowDialog();
                DataClear();
                DataLoad();
            }
            else
            {
                Common.ShowMessageAsync("데이터 선택", "아이템을 선택해주세요.");
            }
        }
    }
}
