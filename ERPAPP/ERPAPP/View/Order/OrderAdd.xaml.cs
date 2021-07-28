using ERPAPP.Helper;
using ERPAPP.Logic;
using ERPAPP.Model;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace ERPAPP.View.Order
{
    /// <summary>
    /// OrderAdd.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class OrderAdd : MetroWindow
    {
        public tblItem selectedItem;

        public OrderAdd()
        {
            InitializeComponent();
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            DataClear();
            DataLoad();
        }

        private void CmbBrand_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CmbBrand.SelectedItem != null)
            {
                CmbItem.Items.Clear();
                CmbItem.SelectedItem = null;

                // 브랜드 선택시 해당 브랜드에 속한 아이템 리스트 로드
                var items = DataAcess.GetItems().Where(i => i.BrandCode.Equals(CmbBrand.SelectedItem.ToString())).ToList();
                foreach (var item in items)
                    CmbItem.Items.Add(item.ItemCode);
            }
        }
        private void CmbItem_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CmbItem.SelectedItem != null)
            {
                selectedItem = DataAcess.GetItems().Where(i => i.ItemCode.Equals(CmbItem.SelectedItem.ToString())).FirstOrDefault();

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

        private async void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (IsValid())
            {
                //오더 추가 생성
                tblOrder order = new tblOrder()
                {
                    OrderCode = TxtCode.Text,
                    BrandCode = CmbBrand.Text,
                    ItemCode = CmbItem.Text,
                    Destination = CmbDest.Text,
                    ShipDate = DateTime.Parse(DtpShipdate.Text),
                    Quantity = (int)NumQuantity.Value,
                    UnitPrice = int.Parse(TxtPrice.Text),
                    RegDate = DateTime.Now.Date,
                    RegID = Common.LOGINED_USER.UserId.ToString()
                };
                DataAcess.SetOrders(order);

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

        private bool IsValid()
        {
            if (string.IsNullOrEmpty(TxtCode.Text))
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
            }
            return true;
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void DataLoad()
        {
            // 브랜드 콤보박스 리스트 로드
            var Brands = DataAcess.GetBrands();
            foreach (var item in Brands)
                CmbBrand.Items.Add(item.BrandCode);

            CmbDest.Items.Add("KOR");
            CmbDest.Items.Add("CHA");
            CmbDest.Items.Add("USA");
            CmbDest.Items.Add("JPN");
        }

        private void DataClear()
        {
            TxtCode.Text = TxtPrice.Text = string.Empty;
            NumQuantity.Value = 0;
            CmbItem.Items.Clear();
            CmbBrand.SelectedItem = CmbItem.SelectedItem = CmbDest.SelectedItem = DtpShipdate.SelectedDate = null;
        }
    }
}
