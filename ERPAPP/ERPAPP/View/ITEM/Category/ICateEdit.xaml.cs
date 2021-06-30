using ERPAPP.Helper;
using ERPAPP.Logic;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Windows;

namespace ERPAPP.View.ITEM.Category
{
    /// <summary>
    /// ICateEdit.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ICateEdit : MetroWindow
    {
        public ICateEdit()
        {
            InitializeComponent();
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            DataLoad();
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private async void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (IsValid())
            {
                var selectedItem = Common.SELECT_ICate;

                selectedItem.IcategName = TxtName.Text;
                selectedItem.ModDate = DateTime.Now.Date;
                selectedItem.ModID = Common.LOGINED_USER.UserId.ToString();

                DataAcess.SetICates(selectedItem);

                await this.ShowMessageAsync("데이터 수정", "카테고리 정보가 수정되었습니다.");
                Close();
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

        private void DataLoad()
        {
            var selectedItem = Common.SELECT_ICate;

            TxtCode.Text = selectedItem.IcateCode.ToString();
            TxtName.Text = selectedItem.IcategName.ToString();
        }
    }
}
