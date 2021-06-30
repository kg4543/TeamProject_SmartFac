using ERPAPP.Helper;
using ERPAPP.Logic;
using ERPAPP.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ERPAPP.View.ITEM.Category
{
    /// <summary>
    /// ItemCategoryView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ItemCategoryView : Page
    {
        public ItemCategoryView()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            DataClear();
            DataLoad(); //그리드 데이터 로드
        }
        

        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            //검색 내용
            string searchCode = TxtSearchCode.Text;
            string searchName = TxtSearchName.Text;

            try
            {
                //검색한 아이템이 있는지 확인
                DataContext = DataAcess.GetICates().Where(i => i.IcateCode.Trim().Contains(searchCode)
                                                        & i.IcategName.Trim().Contains(searchName)).ToList();
            }
            catch (Exception ex)
            {
                Common.logger.Error($"검색 로드 Error : {ex}");
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

        private void TxtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                BtnSearch_Click(sender, e);
        }

        private void GrdData_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            try
            {
                if (GrdData.SelectedItem != null)
                {
                    //선택한 아이템을 외부에서 사용하도록 지정
                    Common.SELECT_ICate = GrdData.SelectedItem as tblICate;
                    var selectedItem = Common.SELECT_ICate;

                    //선택한 아이템 정보를 로드
                    TxtCode.Text = selectedItem.IcateCode.ToString();
                    TxtName.Text = selectedItem.IcategName.ToString();
                }
            }
            catch (Exception ex)
            {
                Common.logger.Error($"카테고리 데이터 선택 Error : {ex}");
                throw ex;
            }
        }
        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            ICateAdd categoryAdd = new ICateAdd();
            categoryAdd.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            categoryAdd.ShowDialog();
            DataClear();
            DataLoad();
        }

        private async void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (GrdData.SelectedItem != null)
            {
                ICateEdit categoryEdit = new ICateEdit();
                categoryEdit.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                categoryEdit.ShowDialog();
                DataClear();
                DataLoad();
            }
            else
            {
                await Common.ShowMessageAsync("데이터 선택", "카테고리를 선택해주세요.");
            }
        }

        private void DataClear()
        {
            TxtCode.Text = TxtName.Text = string.Empty;
            GrdData.SelectedItem = null;
        }

        private void DataLoad()
        {
            try
            {
                List<tblICate> ICates = new List<tblICate>();
                ICates = DataAcess.GetICates();
                DataContext = ICates;
            }
            catch (Exception ex)
            {
                Common.logger.Error($"화면 로드 Error : {ex}");
                throw ex;
            }
        }
    }
}
