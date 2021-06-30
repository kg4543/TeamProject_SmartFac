using ERPAPP.Helper;
using ERPAPP.Logic;
using ERPAPP.Model;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;

namespace ERPAPP.View.ITEM
{
    /// <summary>
    /// ItemEdit.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ItemEdit : MetroWindow
    {
        internal string imgSrc; ////로드한 이미지 주소

        public ItemEdit()
        {
            InitializeComponent();
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            DataLoad();
        }

        private async void BtnUpload_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openfile = new OpenFileDialog();
            if (openfile.ShowDialog() == true) // file open
            {
                if (File.Exists(openfile.FileName)) //file이 있을 경우
                {
                    //이미지 확장자 체크
                    if (System.IO.Path.GetExtension(openfile.FileName) == ".jpg" |
                        System.IO.Path.GetExtension(openfile.FileName) == ".png" |
                        System.IO.Path.GetExtension(openfile.FileName) == ".gif")
                    {
                        //파일 소스를 이미지 소스로 바꿔줌
                        BitmapImage bitmapImage = new BitmapImage(new Uri(openfile.FileName, UriKind.RelativeOrAbsolute));
                        ImgItem.Source = bitmapImage;
                        imgSrc = openfile.FileName;
                    }
                    else
                    {
                        await this.ShowMessageAsync("이미지 등록", "지원하는 확장자 파일이 아닙니다.");
                    }
                }
            }
        }

        private async void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (IsValid())
            {
                var selectedItem = Common.SELECT_ITEM;

                selectedItem.ItemName = TxtName.Text;
                selectedItem.BrandCode = CmbBrand.Text;
                selectedItem.ICateCode = CmbCate.Text;
                selectedItem.ItemImage = imgSrc;
                selectedItem.ItemDescription = TxtDesc.Text;
                selectedItem.ModDate = DateTime.Now.Date;
                selectedItem.ModID = Common.LOGINED_USER.UserId.ToString();

                DataAcess.SetItems(selectedItem);

                await this.ShowMessageAsync("데이터 수정", "아이템 정보가 수정되었습니다.");
                Close();
            }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close(); //창 닫기
        }
        
        private void DataLoad()
        {
            // 브랜드& 카테고리 콤보박스 리스트 로드
            var Brands = DataAcess.GetBrands();

            foreach (var item in Brands)
            {
                CmbBrand.Items.Add(item.BrandCode);
            }

            var iCates = DataAcess.GetICates();

            foreach (var item in iCates)
            {
                CmbCate.Items.Add(item.IcateCode);
            }

            //선택한 아이템 데이터값 로드
            var selectedItem = Common.SELECT_ITEM;

            TxtCode.Text = selectedItem.ItemCode.ToString();
            TxtName.Text = selectedItem.ItemName.ToString();
            CmbBrand.Text = selectedItem.BrandCode.ToString();
            CmbCate.Text = selectedItem.ICateCode.ToString();
            TxtDesc.Text = selectedItem.ItemDescription.ToString();

            if (selectedItem.ItemImage != null)
            {
                BitmapImage bitmapImage = new BitmapImage(new Uri(selectedItem.ItemImage, UriKind.RelativeOrAbsolute));
                ImgItem.Source = bitmapImage;
            }
        }
        private bool IsValid()
        {
            if (string.IsNullOrEmpty(TxtName.Text))
            {
                this.ShowMessageAsync("입력오류", "아이템 이름을 입력해주세요.");
                return false;
            }
            return true;
        }
    }
}
