using ERPAPP.Helper;
using ERPAPP.Logic;
using ERPAPP.Model;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace ERPAPP.View.ITEM.BOM
{
    /// <summary>
    /// BOMView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class BOMView : Page
    {
        public BOMView()
        {
            InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            DataLoad();
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DataLoad()
        {
            try
            {
                List<tblBOM> Items = new List<tblBOM>();
                Items = DataAcess.GetBOM();
                TreeViewItem part = new TreeViewItem();
                
            }
            catch (Exception ex)
            {
                Common.logger.Error($"BOM 화면 로드 Error : {ex}");
                throw ex;
            }
        }
    }
}
