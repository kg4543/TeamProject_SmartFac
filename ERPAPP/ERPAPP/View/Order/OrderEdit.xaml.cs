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
    /// OrderEdit.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class OrderEdit : MetroWindow
    {
        public tblItem selectedItem;

        public OrderEdit()
        {
            InitializeComponent();
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            DataClear();
            DataLoad();
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (IsValid())
            {
                var selectedOrder = Common.SELECT_ORDER;

                selectedOrder.BrandCode = CmbBrand.Text;
                selectedOrder.ItemCode = CmbItem.Text;
                selectedOrder.ShipDate = DateTime.Parse(DtpShipdate.Text);
                selectedOrder.Destination = CmbDest.Text;
                selectedOrder.Quantity = (int)NumQuantity.Value;
                selectedOrder.UnitPrice = int.Parse(TxtPrice.Text);
                selectedItem.ModDate = DateTime.Now.Date;
                selectedItem.ModID = Common.LOGINED_USER.UserId.ToString();

                DataAcess.SetOrders(selectedOrder);

                this.ShowMessageAsync("데이터 수정", "아이템 정보가 수정되었습니다.");
                Close();
            }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
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

        private bool IsValid()
        {
            if (DataAcess.GetOrders().Where(i => i.OrderCode.Trim().Equals(TxtCode.Text.Trim())).Count() > 0) //기본키 중복
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

        private void DataClear()
        {
            TxtCode.Text = TxtPrice.Text = string.Empty;
            NumQuantity.Value = 0;
            CmbItem.Items.Clear();
            CmbBrand.SelectedItem = CmbItem.SelectedItem = CmbDest.SelectedItem = DtpShipdate.SelectedDate = null;
        }

        private void DataLoad()
        {
            var selectedOrder = Common.SELECT_ORDER;

            var Brands = DataAcess.GetBrands();

            foreach (var item in Brands)
            {
                CmbBrand.Items.Add(item.BrandCode);
            }

            var Items = DataAcess.GetItems().Where(i => i.BrandCode.Equals(selectedOrder.BrandCode)).ToList();

            foreach (var item in Items)
            {
                CmbItem.Items.Add(item.ItemCode);
            }

            CmbDest.Items.Add("KOR");
            CmbDest.Items.Add("CHA");
            CmbDest.Items.Add("USA");
            CmbDest.Items.Add("JPN");

            TxtCode.Text = selectedOrder.ItemCode.ToString();
            CmbBrand.Text = selectedOrder.BrandCode.ToString();
            CmbItem.Text = selectedOrder.ItemCode.ToString();
            DtpShipdate.SelectedDate = DateTime.Parse(selectedOrder.ShipDate.ToString());
            CmbDest.Text = selectedOrder.Destination.ToString();
            NumQuantity.Value = selectedOrder.Quantity;
            TxtPrice.Text = selectedOrder.UnitPrice.ToString();

            var Item = DataAcess.GetItems().Where(i => i.ItemCode.Trim().Equals(selectedOrder.ItemCode.Trim())).FirstOrDefault();

            if (Item.ItemImage != null)
            {
                BitmapImage bitmapImage = new BitmapImage(new Uri(Item.ItemImage, UriKind.RelativeOrAbsolute));
                ImgItem.Source = bitmapImage;
            }
        }
    }
}
