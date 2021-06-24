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

namespace ERPAPP.View.ITEM
{
    /// <summary>
    /// ItemAdd.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ItemAdd : MetroWindow
    {
        public ItemAdd()
        {
            InitializeComponent();
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
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

            InitData();
        }
        private void BtnUpload_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            if (openFile.ShowDialog() != null)
            {
                //ImgItem.Source = new ImageConverter(openFile.FileName);
            }
        }

        private async void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(TxtCode.Text))
            {
               await this.ShowMessageAsync("아이템 코드", "아이템 코드를 입력해주세요.");
            }
            else if(DataAcess.GetItems().Where(i => i.ItemCode.Trim().Equals(TxtCode.Text.Trim())).Count() > 0)
            {
               await this.ShowMessageAsync("아이템 코드", "이미 등록된 아이템 코드입니다.");
            }
            else
            {
                tblItem item = new tblItem()
                {
                    ItemCode = TxtCode.Text.Trim(),
                    ItemName = TxtName.Text,
                    BrandCode = CmbBrand.Text,
                    ICateCode = CmbCate.Text,
                    ItemDescription = TxtDesc.Text,
                    RegDate = DateTime.Now.Date,
                    RegID = Common.LOGINED_USER.UserId.ToString()
                };
                DataAcess.SetItems(item);

                await this.ShowMessageAsync("아이템 등록", "아이템이 등록되었습니다.");

                InitData();
            }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close(); //창 닫기
        }

        private void InitData()
        {
            TxtCode.Text = "";
            TxtName.Text = "";
            CmbBrand.Text = "";
            CmbCate.Text = "";
            TxtDesc.Text = "";
            TxtCode.Focus();
        }
    }
}
