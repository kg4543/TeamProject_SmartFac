using ERPAPP.Helper;
using ERPAPP.Logic;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Windows;

namespace ERPAPP.View.Factory
{
    /// <summary>
    /// FactoryEdit.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class FactoryEdit : MetroWindow
    {
        public FactoryEdit()
        {
            InitializeComponent();
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            DataLoad();
        }

        private async void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (IsValid())
            {
                var selectedItem = Common.SELECT_Factory;

                selectedItem.FactoryName = TxtName.Text;
                selectedItem.ModDate = DateTime.Now.Date;
                selectedItem.ModID = Common.LOGINED_USER.UserId.ToString();

                DataAcess.SetFactory(selectedItem);

                await this.ShowMessageAsync("데이터 수정", "공장 정보가 수정되었습니다.");
                Close();
            }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private bool IsValid()
        {
            if (string.IsNullOrEmpty(TxtName.Text))
            {
                this.ShowMessageAsync("입력오류", "공장 이름을 입력해주세요.");
                return false;
            }
            return true;
        }

        private void DataLoad()
        {
            var selectedItem = Common.SELECT_Factory;

            TxtCode.Text = selectedItem.FactoryCode.ToString();
            TxtName.Text = selectedItem.FactoryName.ToString();
        }
    }
}
