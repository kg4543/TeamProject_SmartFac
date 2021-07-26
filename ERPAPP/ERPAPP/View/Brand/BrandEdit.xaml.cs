using ERPAPP.Helper;
using ERPAPP.Logic;
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
    /// BrandEdit.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class BrandEdit : MetroWindow
    {
        public BrandEdit()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DataLoad();
        }

        private async void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (IsValid())
            {
                var selectedItem = Common.SELECT_Brand;

                selectedItem.BrandName = TxtName.Text;
                selectedItem.ModDate = DateTime.Now.Date;
                selectedItem.ModID = Common.LOGINED_USER.UserId.ToString();

                DataAcess.SetBrands(selectedItem);

                await this.ShowMessageAsync("데이터 수정", "브랜드 정보가 수정되었습니다.");
                Close();
            }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void DataLoad()
        {
            var selectedItem = Common.SELECT_Brand;

            TxtCode.Text = selectedItem.BrandCode.ToString();
            TxtName.Text = selectedItem.BrandName.ToString();
        }

        private bool IsValid()
        {
            if (string.IsNullOrEmpty(TxtName.Text))
            {
                this.ShowMessageAsync("입력오류", "브랜드 이름을 입력해주세요.");
                return false;
            }
            return true;
        }
    }
}
