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
            ClearData();
        }

        private void ClearData()
        {
            TxtCode.Text = TxtName.Text = string.Empty;
            TxtCode.Focus();
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
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
                this.ShowMessageAsync("카테고리 등록","카테고리를 추가하였습니다.");
                ClearData();
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
    }
}
