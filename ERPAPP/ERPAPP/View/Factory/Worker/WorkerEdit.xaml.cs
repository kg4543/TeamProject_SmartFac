using ERPAPP.Helper;
using ERPAPP.Logic;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Windows;

namespace ERPAPP.View.Factory.Worker
{
    /// <summary>
    /// WorkerEdit.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class WorkerEdit : MetroWindow
    {
        public WorkerEdit()
        {
            InitializeComponent();
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            DataLoad();
        }

        private async void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (IsValid())
            {
                var selectedItem = Common.SELECT_Worker;

                selectedItem.WorkerName = TxtName.Text;
                selectedItem.FactoryCode = CmbFactory.Text;
                selectedItem.WorkerPhone = TxtPhone.Text;
                selectedItem.WorkerAddr = TxtAddr.Text;
                selectedItem.ModDate = DateTime.Now.Date;
                selectedItem.ModID = Common.LOGINED_USER.UserId.ToString();

                DataAcess.SetWorkers(selectedItem);

                await this.ShowMessageAsync("데이터 수정", "직원 정보가 수정되었습니다.");
                Close();
            }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private bool IsValid()
        {
            if (string.IsNullOrEmpty(TxtName.Text))
            {
                this.ShowMessageAsync("입력오류", "이름을 입력해주세요.");
                return false;
            }
            if (string.IsNullOrEmpty(TxtPhone.Text))
            {
                this.ShowMessageAsync("입력오류", "휴대폰 번호를 입력해주세요.");
                return false;
            }
            if (TxtPhone.Text.Length > 12)
            {
                this.ShowMessageAsync("입력오류", "'-'를 제외한 11자리를 입력해주세요.");
                return false;
            }
            if (CmbFactory.SelectedItem == null)
            {
                this.ShowMessageAsync("입력오류", "공장 코드를 입력해주세요.");
                return false;
            }

            return true;
        }

        private void DataLoad()
        {
            // 콤보박스 리스트 로드
            var Factorys = DataAcess.GetFactorys();
            foreach (var item in Factorys)
                CmbFactory.Items.Add(item.FactoryCode);

            var selectedItem = Common.SELECT_Worker;

            TxtCode.Text = selectedItem.WorkerCode.ToString();
            TxtName.Text = selectedItem.WorkerName.ToString();
            CmbFactory.Text = selectedItem.FactoryCode.ToString();
            TxtPhone.Text = selectedItem.WorkerPhone.ToString();
            TxtAddr.Text = selectedItem.WorkerAddr.ToString();
        }
    }
}
