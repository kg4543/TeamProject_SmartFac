using ERPAPP.Helper;
using ERPAPP.Logic;
using ERPAPP.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ERPAPP.View.ITEM
{
    /// <summary>
    /// ItemMain.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ItemMain : Page
    {

        public ItemMain()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            DataLoad();
        }

        private void TxtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            //검색창에서 엔터 시 검색됨
            if (e.Key == Key.Enter)
                BtnSearch_Click(sender, e);
        }

        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            //검색 창
            string searchCode = TxtSearchCode.Text;
            string searchName = TxtSearchName.Text;

            try
            {
                //검색한 아이템이 있는지 확인
                DataContext = DataAcess.GetItems().Where(i => i.ItemCode.Trim().Contains(searchCode)
                                                        & i.ItemName.Trim().Contains(searchName)).ToList();
            }
            catch (Exception ex)
            {
                Common.logger.Error($"화면 로드 Error : {ex}");
                throw ex;
            }
        }

        private void GrdData_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            if (GrdData.SelectedItem != null)
            {
                try
                {
                    Common.SELECT_ITEM = GrdData.SelectedItem as tblItem;
                    var selectedItem = Common.SELECT_ITEM;

                    TxtCode.Text = selectedItem.ItemCode.ToString();
                    TxtName.Text = selectedItem.ItemName.ToString();
                    TxtBrand.Text = selectedItem.BrandCode.ToString();
                    TxtCategory.Text = selectedItem.ICateCode.ToString();
                    TxtDesc.Text = selectedItem.ItemDescription.ToString();
                }

                catch (Exception ex)
                {
                    Common.logger.Error($"데이터 선택 Error : {ex}");
                    throw;
                }
            }
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            ItemAdd itemAdd = new ItemAdd();
            itemAdd.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            itemAdd.ShowDialog();
            DataLoad();
        }

        private async void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (GrdData.SelectedItem != null)
            {
                ItemEdit itemEdit = new ItemEdit();
                itemEdit.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                itemEdit.ShowDialog();
                DataLoad();
            }
            else
            {
                await Common.ShowMessageAsync("아이템 선택","아이템을 선택해주세요.");
            }
        }

        private void DataLoad()
        {
            try
            {
                //Item DB List 생성
                List<tblItem> Items = new List<tblItem>();
                //값 받아오기
                Items = DataAcess.GetItems();

                //데이터 그리드 바인딩에 Item DB 정보 로드
                DataContext = Items;
            }
            catch (Exception ex)
            {
                Common.logger.Error($"화면 로드 Error : {ex}");
                throw ex;
            }
        }
    }
}
