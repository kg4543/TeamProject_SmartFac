using ERPAPP.Helper;
using ERPAPP.Logic;
using ERPAPP.Model;
using ERPAPP.View.ITEM.Category;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

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
            DataLoad(); //그리드 데이터 로드
        }

        //데이터 검색
        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            //검색 내용
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
                Common.logger.Error($"검색 로드 Error : {ex}");
                throw ex;
            }
        }

        //데이터 선택
        private void GrdData_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            try
            {
                if (GrdData.SelectedItem != null)
                {
                    //선택한 아이템을 외부에서 사용하도록 지정
                    Common.SELECT_ITEM = GrdData.SelectedItem as tblItem;
                    var selectedItem = Common.SELECT_ITEM;

                    //선택한 아이템 정보를 로드
                    TxtCode.Text = selectedItem.ItemCode.ToString();
                    TxtName.Text = selectedItem.ItemName.ToString();
                    TxtBrand.Text = selectedItem.BrandCode.ToString();
                    TxtCategory.Text = selectedItem.ICateCode.ToString();
                    TxtDesc.Text = selectedItem.ItemDescription.ToString();

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
            }

            catch (Exception ex)
            {
                Common.logger.Error($"아이템 데이터 선택 Error : {ex}");
                throw ex;
            }
        }

        // 아이템 추가
        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            //아이템 추가 창 불러오기
            ItemAdd itemAdd = new ItemAdd();
            itemAdd.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            itemAdd.ShowDialog();
            DataClear(); //선택값 초기화
            DataLoad(); //데이터 로드
        }

        // 아이템 정보 수정
        private async void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (GrdData.SelectedItem != null)
            {
                //아이템 수정 창 불러오기
                ItemEdit itemEdit = new ItemEdit();
                itemEdit.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                itemEdit.ShowDialog();
                DataClear();
                DataLoad();
            }
            else //아이템을 선택하지 않았을 경우
            {
                await Common.ShowMessageAsync("데이터 선택", "아이템을 선택해주세요.");
            }
        }

        private void DataClear()
        {
            //선택 값 초기화
            TxtCode.Text = TxtName.Text = TxtBrand.Text = TxtCategory.Text = TxtDesc.Text = string.Empty;
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
                Common.logger.Error($"화면 로드 Error : {ex}");
                throw ex;
            }
        }

        private void BtnDownload_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog imageFile = new SaveFileDialog();
            imageFile.Filter = "JPeg Image|*.jpg|Bitmap Image|*.bmp|Gif Image|*.gif|Png Image|*.png";
            imageFile.FileName = Path.GetFileName(ImgItem.Source.ToString());
            imageFile.ShowDialog();
            BitmapImage bitmapImage = new BitmapImage(new Uri(ImgItem.Source.ToString(), UriKind.RelativeOrAbsolute));
            using (FileStream fs = new FileStream(imageFile.FileName, FileMode.Create
                ,FileAccess.ReadWrite, FileShare.None))
            {
                JpegBitmapEncoder encoder = new JpegBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(bitmapImage.UriSource));
                encoder.Save(fs);
            }
        }

        private async void BtnCategory_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                NavigationService.Navigate(new ItemCategoryView());
            }
            catch (Exception ex)
            {
                Common.logger.Error($"예외발생 BtnAccount_Click : {ex}");
                await Common.ShowMessageAsync("예외", $"예외발생 : {ex}");
            }
        }

        private void TxtSearchCode_KeyDown(object sender, KeyEventArgs e)
        {
            //검색창에서 엔터 시 이름 검색창 포커스됨
            if (e.Key == Key.Enter)
                TxtSearchName.Focus();
        }

        private void TxtSearchName_KeyDown(object sender, KeyEventArgs e)
        {
            //검색창에서 엔터 시 검색됨
            if (e.Key == Key.Enter)
                BtnSearch_Click(sender, e);
        }
    }
}
