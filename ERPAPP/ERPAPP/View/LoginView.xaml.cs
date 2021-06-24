using MahApps.Metro.Controls;
using System.Windows;
using System.Windows.Input;
using System;
using MahApps.Metro.Controls.Dialogs;
using ERPAPP.Helper;
using ERPAPP.Logic;
using System.Linq;

namespace ERPAPP.View
{
    /// <summary>
    /// Login.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class LoginView : MetroWindow
    {
        public LoginView()
        {
            InitializeComponent();
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            TxtUserId.Focus(); //처음 아이디 입력창에 포커스 맞춤
            LblResult.Visibility = Visibility.Hidden; //로그인 결과값 숨김
        }

        private void TxtUserId_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                TxtUserPassword.Focus(); //아이디 입력창 다음 패스워드 입력창으로 포커스 이동
        }

        private void TxtUserPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter) //패스워드 입력 후 엔터 입력 시 로그인 버튼 클릭
                BtnLogin_Click(sender, e);
        }

        private async void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            LblResult.Visibility = Visibility.Hidden;
            //아이디 및 패스워드에 값이 있는지 체크
            if (string.IsNullOrEmpty(TxtUserId.Text) || string.IsNullOrEmpty(TxtUserPassword.Password))
            {
                LblResult.Visibility = Visibility.Visible;
                LblResult.Content = "아이디 / 패스워드를 입력하세요!!!";
                Common.logger.Warn("아이디/패스워드 미입력 접속 시도");
                return;
            }
            try
            {
                string userId = TxtUserId.Text; //아이디 입력 값
                string userPassword = TxtUserPassword.Password; //패스워드 입력 값

                int userNum = DataAcess.Getusers() // 조건(아이디 & 패스워드 일치)에 맞는 아이디가 있는지 체크
                            .Where(u => u.UserId.Equals(userId) && u.UserPassword.Equals(userPassword)).Count();

                if (userNum == 0) //일치하는 아이디가 존재하지 않을 경우
                {
                    LblResult.Visibility = Visibility.Visible;
                    LblResult.Content = "사용자가 존재하지 않습니다.!!!";
                    Common.logger.Warn("아이디/패스워드 불일치");
                    return;
                }
                else //로그인한 아이디 정보를 받아옴
                {
                    Common.LOGINED_USER = DataAcess.Getusers().Where(u => u.UserId.Equals(userId)).FirstOrDefault();
                    LblResult.Visibility = Visibility.Hidden;
                    Common.logger.Info($"{userId} 접속성공");
                    Close();
                }
            }
            catch (Exception ex) //로그인 예외처리
            {
                await this.ShowMessageAsync("예외", $"예외발생 {ex}");
            }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close(); //창 닫기
        }
    }
}
