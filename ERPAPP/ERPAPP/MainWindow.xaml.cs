﻿using System;
using System.Windows;
using MahApps.Metro.Controls;
using ERPAPP.View;
using MahApps.Metro.Controls.Dialogs;
using ERPAPP.Helper;
using ERPAPP.View.ITEM;

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
                BtnReport.IsEnabled = true;

                if (Common.LOGINED_USER.RItem == true)
                    BtnItem.IsEnabled = true;
                if (Common.LOGINED_USER.ROrder == true)
                    BtnOrder.IsEnabled = true;
                if (Common.LOGINED_USER.RProduction == true)
                    BtnProduction.IsEnabled = true;
                if (Common.LOGINED_USER.RMaterial == true)
                    BtnMRP.IsEnabled = true;
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

                    BtnReport.IsEnabled = false;
                    BtnItem.IsEnabled = false;
                    BtnOrder.IsEnabled = false;
                    BtnProduction.IsEnabled = false;
                    BtnMRP.IsEnabled = false;
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
    }
}
