using ERPAPP.Helper;
using ERPAPP.Logic;
using ERPAPP.Model;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Media.Imaging;

namespace ERPAPP.View.Production
{
    /// <summary>
    /// ProductionAdd.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ProductionAdd : MetroWindow
    {
        public ProductionAdd()
        {
            InitializeComponent();
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            DataClear();
            DataLoad();
        }

        private void BtnOrderSerch_Click(object sender, RoutedEventArgs e)
        {
            OrderSearch OrderSearch = new OrderSearch();
            OrderSearch.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            OrderSearch.ShowDialog();

            if (Common.SELECT_ORDER != null)
            {
                var selOrder = Common.SELECT_ORDER;
                TxtOrder.Text = selOrder.OrderCode;
                TxtItem.Text = DataAcess.GetOrders().Where(i => i.OrderCode.Equals(selOrder.OrderCode)).FirstOrDefault().ItemCode;
                Image();
            }
        }

        private async void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (IsValid())
            {
                tblProduction production = new tblProduction()
                {
                    ProductionCode = TxtCode.Text,
                    FactoryCode = CmbFactory.Text,
                    OrderCode = TxtOrder.Text,
                    ItemCode = TxtItem.Text,
                    StartDate = DateTime.Parse(DtpStartdate.Text),
                    EndDate = DateTime.Parse(DtpEnddate.Text),
                    PlanQuantity = (int)NumPlanQty.Value,
                    FQuantity = (int)NumFinishQty.Value,
                    RegDate = DateTime.Now.Date,
                    RegID = Common.LOGINED_USER.UserId.ToString()
                };
                DataAcess.SetProductions(production);

                //this.ShowMessageAsync("데이터 등록", "오더가 등록되었습니다.");
                var result = await this.ShowMessageAsync("데이터 등록", "오더가 등록되었습니다.\n 추가 등록하시겠습니까?",
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
            TxtCode.Text = TxtOrder.Text = TxtItem.Text = string.Empty;
            CmbFactory.SelectedItem = DtpStartdate.SelectedDate = DtpEnddate.SelectedDate = null;
            NumPlanQty.Value = NumFinishQty.Value = 0;
            Common.SELECT_ORDER = null;
            Image();
        }

        private void DataLoad()
        {
            var Brands = DataAcess.GetFactorys();
            foreach (var item in Brands)
                CmbFactory.Items.Add(item.FactoryCode);
        }

        private void Image()
        {
            if (TxtItem.Text != string.Empty)
            {
                var selectedItem = DataAcess.GetItems().Where(i => i.ItemCode.Equals(TxtItem.Text)).FirstOrDefault();

                if (selectedItem.ItemImage != null)
                {
                    BitmapImage bitmapImage = new BitmapImage(new Uri(selectedItem.ItemImage, UriKind.RelativeOrAbsolute));
                    ImgItem.Source = bitmapImage;
                }
                else
                {
                    BitmapImage bitmapImage = new BitmapImage(new Uri("/Resources/No_Picture.jpg", UriKind.RelativeOrAbsolute));
                    ImgItem.Source = bitmapImage;
                }
            }
            else
            {
                BitmapImage bitmapImage = new BitmapImage(new Uri("/Resources/No_Picture.jpg", UriKind.RelativeOrAbsolute));
                ImgItem.Source = bitmapImage;
            }
        }

        private bool IsValid()
        {
            /*if (string.IsNullOrEmpty(TxtCode.Text))
            {
                this.ShowMessageAsync("입력오류", "오더 코드를 입력해주세요.");
                return false;
            }
            else if (DataAcess.GetOrders().Where(i => i.OrderCode.Trim().Equals(TxtCode.Text.Trim())).Count() > 0) //기본키 중복
            {
                this.ShowMessageAsync("입력오류", "이미 등록된 오더 코드입니다.");
                return false;
            }
            if (string.IsNullOrEmpty(TxtPrice.Text))
            {
                this.ShowMessageAsync("입력오류", "가격을 입력해주세요.");
                return false;
            }
            if (DtpShipdate.SelectedDate == null)
            {
                this.ShowMessageAsync("입력오류", "납기일을 입력해주세요.");
                return false;
            }
            if (CmbBrand.SelectedItem == null)
            {
                this.ShowMessageAsync("입력오류", "브랜드 코드를 입력해주세요.");
                return false;
            }
            if (CmbItem.SelectedItem == null)
            {
                this.ShowMessageAsync("입력오류", "아이템 코드를 입력해주세요.");
                return false;
            }
            if (CmbDest.SelectedItem == null)
            {
                this.ShowMessageAsync("입력오류", "도착지를 입력해주세요.");
                return false;
            }
            if (NumQuantity.Value <= 0)
            {
                this.ShowMessageAsync("입력오류", "금액을 다시 설정해주세요.");
                return false;
            }*/
            return false; //임시 false
        }
    }
}
