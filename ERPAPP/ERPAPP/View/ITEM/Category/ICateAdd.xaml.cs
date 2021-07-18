using ERPAPP.Helper;
using ERPAPP.Model;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Linq;
using System.Windows;

namespace ERPAPP.View.ITEM.Category
{
    /// <summary>
    /// ICateAdd.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ICateAdd : MetroWindow
    {
        public ICateAdd()
        {
            InitializeComponent();
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            DataClear();
        }

        private async void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (IsValid())
            {
                tblICate iCate = new tblICate()
                {
                    IcateCode = TxtCode.Text,
                    IcategName = TxtName.Text,
                    RegDate = DateTime.Now.Date,
                    RegID = Common.LOGINED_USER.UserId
                };
                Logic.DataAcess.SetICates(iCate);
                var result = await this.ShowMessageAsync("데이터 등록", "카테고리가 등록되었습니다.\n 추가 등록하시겠습니까?",
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

        //데이터 무결성 검사
        private bool IsValid()
        {
            if (string.IsNullOrEmpty(TxtCode.Text))
            {
                this.ShowMessageAsync("입력오류", "카테고리 코드를 입력하세요.");
                return false;
            }
            else if (Logic.DataAcess.GetICates().Where(c => c.IcateCode.Equals(TxtCode)).Count() > 0)
            {
                this.ShowMessageAsync("입력오류", "동일한 코드값이 존재합니다.");
                return false;
            }
            if (string.IsNullOrEmpty(TxtName.Text))
            {
                this.ShowMessageAsync("입력오류", "카테고리 이름을 입력하세요.");
                return false;
            }
            return true;
        }

        private void DataClear()
        {
            TxtCode.Text = TxtName.Text = string.Empty;
            TxtCode.Focus();
        }
    }
}
