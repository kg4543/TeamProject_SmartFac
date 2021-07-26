using ERPAPP.Helper;
using ERPAPP.Model;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ERPAPP.View.Brand
{
    /// <summary>
    /// BrandAdd.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class BrandAdd : MetroWindow
    {
        public BrandAdd()
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
                tblBrand brand = new tblBrand()
                {
                    BrandCode = TxtCode.Text,
                    BrandName = TxtName.Text,
                    RegDate = DateTime.Now.Date,
                    RegID = Common.LOGINED_USER.UserId
                };
                Logic.DataAcess.SetBrands(brand);
                var result = await this.ShowMessageAsync("데이터 등록", "브랜드가 등록되었습니다.\n 추가 등록하시겠습니까?",
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

        private bool IsValid()
        {
            if (string.IsNullOrEmpty(TxtCode.Text))
            {
                this.ShowMessageAsync("입력오류", "브랜드 코드를 입력하세요.");
                return false;
            }
            else if (Logic.DataAcess.GetICates().Where(c => c.IcateCode.Equals(TxtCode)).Count() > 0)
            {
                this.ShowMessageAsync("입력오류", "동일한 코드값이 존재합니다.");
                return false;
            }
            if (string.IsNullOrEmpty(TxtName.Text))
            {
                this.ShowMessageAsync("입력오류", "브랜드 이름을 입력하세요.");
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
