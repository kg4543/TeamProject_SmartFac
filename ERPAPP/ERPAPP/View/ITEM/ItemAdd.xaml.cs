using ERPAPP.Logic;
using ERPAPP.Model;
using MahApps.Metro.Controls;
using System.Windows;
using System;
using ERPAPP.Helper;
using System.IO;
using Microsoft.Win32;
using System.Drawing;
using System.Linq;
using MahApps.Metro.Controls.Dialogs;
using System.Windows.Media.Imaging;

namespace ERPAPP.View.ITEM
{
    /// <summary>
    /// ItemAdd.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ItemAdd : MetroWindow
    {
        internal string imgSrc; //로드한 이미지 주소

        public ItemAdd()
        {
            InitializeComponent();
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // 브랜드 콤보박스 리스트 로드
            var Brands = DataAcess.GetBrands();
            foreach (var item in Brands)
                CmbBrand.Items.Add(item.BrandCode);
            // 카테고리 콤보박스 리스트 로드
            var iCates = DataAcess.GetICates();
            foreach (var item in iCates)
                CmbCate.Items.Add(item.IcateCode);
        }

        //사진 업로드
        private void BtnUpload_Click(object sender, RoutedEventArgs e)
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
                        
                        //지정 위치에 사진 저장
                        using (FileStream fs = new FileStream("/Resources/ITEM", FileMode.Create
                                                                , FileAccess.ReadWrite, FileShare.None))
                        {
                            JpegBitmapEncoder encoder = new JpegBitmapEncoder();
                            encoder.Frames.Add(BitmapFrame.Create(bitmapImage.UriSource));
                            encoder.Save(fs);
                        }

                        // 저장된 사진 주소를 받아오기
                        //ImgItem.Source = "/Resources/ITEM" + Path.GetFileName(openfile.FileName);
                        imgSrc = ImgItem.Source.ToString();
                    }
                    else
                    {
                        this.ShowMessageAsync("이미지 등록", "지원하는 확장자 파일이 아닙니다.");
                    }
                }
            }
        }

        //DB에 데이터 추가
        private async void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            if(IsValid())
            {
                //아이템 추가 생성
                tblItem item = new tblItem()
                {
                    ItemCode = TxtCode.Text.Trim(),
                    ItemName = TxtName.Text,
                    ItemImage = imgSrc,
                    BrandCode = CmbBrand.Text,
                    ICateCode = CmbCate.Text,
                    //ICateCode = DataAcess.GetICates().Where(c => c.IcateCode.Equals(CmbCate.Text)).FirstOrDefault().IcateCode,
                    ItemDescription = TxtDesc.Text,
                    RegDate = DateTime.Now.Date,
                    RegID = Common.LOGINED_USER.UserId.ToString()
                };
                DataAcess.SetItems(item);

                var result = await this.ShowMessageAsync("데이터 등록", "아이템이 등록되었습니다.\n 추가 등록하시겠습니까?",
                                                    MessageDialogStyle.AffirmativeAndNegative, null);
                if (result == MessageDialogResult.Affirmative)
                {
                    DataClear();
                }
                else
                {
                    Close();
                }
            }
        }
        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close(); //창 닫기
        }

        private void DataClear()
        {
            TxtCode.Text =
            TxtName.Text =
            CmbBrand.Text =
            CmbCate.Text =
            TxtDesc.Text = string.Empty;
            ImgItem.Source = null;
            TxtCode.Focus();
        }

        private bool IsValid()
        {
            if (string.IsNullOrEmpty(TxtCode.Text))
            {
                this.ShowMessageAsync("입력오류", "아이템 코드를 입력해주세요.");
                return false;
            }
            else if (DataAcess.GetItems().Where(i => i.ItemCode.Trim().Equals(TxtCode.Text.Trim())).Count() > 0) //기본키 중복
            {
                this.ShowMessageAsync("입력오류", "이미 등록된 아이템 코드입니다.");
                return false;
            }

            if (string.IsNullOrEmpty(TxtName.Text))
            {
                this.ShowMessageAsync("입력오류", "아이템 이름을 입력해주세요.");
                return false;
            }
            return true;
        }
    }
}
