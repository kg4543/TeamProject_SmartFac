using System;
using System.Windows;
using MahApps.Metro.Controls;
using ERPAPP.View;
using MahApps.Metro.Controls.Dialogs;
using ERPAPP.Helper;
using ERPAPP.View.ITEM;
using ERPAPP.View.Order;
using ERPAPP.View.Factory;
using ERPAPP.View.Production;
using ERPAPP.View.MES;
using ERPAPP.Logic;
using System.Linq;
using ERPAPP.View.OPS;
using ERPAPP.View.AGV;

namespace ERPAPP
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        bool islogin = false;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void MetroWindow_ContentRendered(object sender, EventArgs e)
        {
            ShowLoginview();
        }
        private void MetroWindow_Activated(object sender, EventArgs e)
        {
            //로그인 시 권한에 따른 버튼 활성화
            if(Common.LOGINED_USER != null)
            {
                BtnLogin.Content = "LogOut";
                BtnAGV.IsEnabled = true;

                if (Common.LOGINED_USER.RItem == true)
                    BtnItem.IsEnabled = true;
                if (Common.LOGINED_USER.ROrder == true)
                    BtnOrder.IsEnabled = true;
                if (Common.LOGINED_USER.RProduction == true)
                    BtnProduction.IsEnabled = true;
                if (Common.LOGINED_USER.RMaterial == true)
                    BtnOp.IsEnabled = true;
                if (Common.LOGINED_USER.RFactory == true)
                    BtnFactory.IsEnabled = true;
                if (Common.LOGINED_USER.RMES == true)
                    BtnMES.IsEnabled = true;
            }
        }

        private async void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            if (Common.LOGINED_USER == null)
            {
                ShowLoginview();
            }
            else
            {
                var result = await this.ShowMessageAsync("LogOut", "로그아웃 하시겠습니까?"
                                                    , MessageDialogStyle.AffirmativeAndNegative, null);

                if (result == MessageDialogResult.Affirmative)
                {
                    BtnLogin.Content = "Login";
                    Common.LOGINED_USER = null;
                    ActivePage.Content = null;

                    BtnAGV.IsEnabled = false;
                    BtnItem.IsEnabled = false;
                    BtnOrder.IsEnabled = false;
                    BtnProduction.IsEnabled = false;
                    BtnOp.IsEnabled = false;
                    BtnFactory.IsEnabled = false;
                    BtnMES.IsEnabled = false;
                }
            }
        }
        private void ShowLoginview()
        {
            LoginView login = new LoginView();
            login.Owner = this;
            login.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            login.ShowDialog();
        }

        private async void BtnExit_Click(object sender, RoutedEventArgs e)
        {
            var result = await this.ShowMessageAsync("종료","종료하시겠습니까?"
                                                    ,MessageDialogStyle.AffirmativeAndNegative,null);

            if (result == MessageDialogResult.Affirmative)
            {
                Application.Current.Shutdown();
            }
        }

        private async void BtnItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ActivePage.Content = new ItemMain();
            }
            catch (Exception ex)
            {
                Common.logger.Error($"예외발생 BtnAccount_Click : {ex}");
                await this.ShowMessageAsync("예외", $"예외발생 : {ex}");
            }
        }

        private async void BtnOrder_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ActivePage.Content = new OrderView();
            }
            catch (Exception ex)
            {
                Common.logger.Error($"예외발생 BtnAccount_Click : {ex}");
                await this.ShowMessageAsync("예외", $"예외발생 : {ex}");
            }
        }

        private async void BtnFactory_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ActivePage.Content = new FactoryView();
            }
            catch (Exception ex)
            {
                Common.logger.Error($"예외발생 BtnAccount_Click : {ex}");
                await this.ShowMessageAsync("예외", $"예외발생 : {ex}");
            }
        }

        private async void BtnProduction_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ActivePage.Content = new ProductionView();
            }
            catch (Exception ex)
            {
                Common.logger.Error($"예외발생 BtnAccount_Click : {ex}");
                await this.ShowMessageAsync("예외", $"예외발생 : {ex}");
            }
        }

        private async void BtnOp_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ActivePage.Content = new OperationView();
            }
            catch (Exception ex)
            {
                Common.logger.Error($"예외발생 BtnAccount_Click : {ex}");
                await this.ShowMessageAsync("예외", $"예외발생 : {ex}");
            }
        }

        private async void BtnMES_Click(object sender, RoutedEventArgs e)
        {
            if (DataAcess.GetNowProduction().Count() == 0)
            {
                this.ShowMessageAsync("생산계획", "오늘자 생산계획이 없습니다.");
            }
            else
            {
                try
                {
                    Common.SELECT_Production = null;
                    ActivePage.Content = new MESView();
                }
                catch (Exception ex)
                {
                    Common.logger.Error($"예외발생 BtnAccount_Click : {ex}");
                    await this.ShowMessageAsync("예외", $"예외발생 : {ex}");
                }
            }
        }

        private void BtnAGV_Click(object sender, RoutedEventArgs e)
        {
            AGVView agv = new AGVView();
            agv.Owner = this;
            this.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            agv.Show();
        }
    }
}
