using ERPAPP.Helper;
using ERPAPP.Logic;
using ERPAPP.Model;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace ERPAPP.View.MES
{
    /// <summary>
    /// ProductionSearch.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ProductionSearch : MetroWindow
    {
        public ProductionSearch()
        {
            InitializeComponent();
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            DataLoad();
        }

        private void DataLoad()
        {
            try
            {
                List<tblProduction> Items = new List<tblProduction>();
                Items = DataAcess.GetNowProduction();
                DataContext = Items;
            }
            catch (Exception ex)
            {
                Common.logger.Error($"ORDER 화면 로드 Error : {ex}");
                throw ex;
            }
        }

        private void BtnOK_Click(object sender, RoutedEventArgs e)
        {
            if (GrdData.SelectedItem != null)
            {
                Common.SELECT_Production = GrdData.SelectedItem as tblProduction;
                Close();
            }
            else
            {
                this.ShowMessageAsync("선택오류", "생산계획을 선택하세요.");
            }
        }
    }
}
