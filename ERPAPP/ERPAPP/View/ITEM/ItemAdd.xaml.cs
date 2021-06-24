using ERPAPP.Logic;
using MahApps.Metro.Controls;
using System.Windows;

namespace ERPAPP.View.ITEM
{
    /// <summary>
    /// ItemAdd.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ItemAdd : MetroWindow
    {
        public ItemAdd()
        {
            InitializeComponent();
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // 브랜드& 카테고리 콤보박스 리스트 로드
            var Brands = DataAcess.GetBrands();

            foreach (var item in Brands)
            {
                CmbBrand.Items.Add(item.BrandCode);
            }

            var iCates = DataAcess.GetICates();

            foreach (var item in iCates)
            {
                CmbCate.Items.Add(item.IcateCode);
            }
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close(); //창 닫기
        }
    }
}
