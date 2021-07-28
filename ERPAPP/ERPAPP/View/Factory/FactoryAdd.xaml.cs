using ERPAPP.Helper;
using ERPAPP.Model;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Linq;
using System.Windows;

namespace ERPAPP.View.Factory
{
    /// <summary>
    /// FactoryAdd.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class FactoryAdd : MetroWindow
    {
        public FactoryAdd()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DataClear();
        }

        private async void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (IsValid())
            {
                tblFactory factory = new tblFactory()
                {
                    FactoryCode = TxtCode.Text,
                    FactoryName = TxtName.Text,
                    RegDate = DateTime.Now.Date,
                    RegID = Common.LOGINED_USER.UserId
                };
                Logic.DataAcess.SetFactorys(factory);
                var result = await this.ShowMessageAsync("데이터 등록", "공장이 등록되었습니다.\n 추가 등록하시겠습니까?",
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
            Close();
        }

        private void DataClear()
        {
            TxtCode.Text = TxtName.Text = string.Empty;
            TxtCode.Focus();
        }

        private bool IsValid()
        {
            if (string.IsNullOrEmpty(TxtCode.Text))
            {
                this.ShowMessageAsync("입력오류", "공장 코드를 입력하세요.");
                return false;
            }
            else if (Logic.DataAcess.GetICates().Where(c => c.IcateCode.Equals(TxtCode)).Count() > 0)
            {
                this.ShowMessageAsync("입력오류", "동일한 코드값이 존재합니다.");
                return false;
            }
            if (string.IsNullOrEmpty(TxtName.Text))
            {
                this.ShowMessageAsync("입력오류", "공장 이름을 입력하세요.");
                return false;
            }
            return true;
        }
    }
}
