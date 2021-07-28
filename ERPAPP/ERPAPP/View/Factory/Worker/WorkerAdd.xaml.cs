using ERPAPP.Helper;
using ERPAPP.Logic;
using ERPAPP.Model;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ERPAPP.View.Factory.Worker
{
    /// <summary>
    /// WorkerAdd.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class WorkerAdd : MetroWindow
    {
        public WorkerAdd()
        {
            InitializeComponent();
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            DataClear();
            DataLoad();
        }

        private async void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (IsValid())
            {
                tblWorker worker = new tblWorker()
                {
                    WorkerCode = TxtCode.Text,
                    WorkerName = TxtName.Text,
                    FactoryCode = CmbFactory.Text,
                    WorkerPhone = TxtPhone.Text,
                    WorkerAddr = TxtAddr.Text,
                    RegDate = DateTime.Now.Date,
                    RegID = Common.LOGINED_USER.UserId.ToString()
                };
                DataAcess.SetWorkers(worker);

                var result = await this.ShowMessageAsync("데이터 등록", "기계정보가 등록되었습니다.\n 추가 등록하시겠습니까?",
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

        private bool IsValid()
        {
            if (string.IsNullOrEmpty(TxtCode.Text))
            {
                this.ShowMessageAsync("입력오류", "직원 코드를 입력해주세요.");
                return false;
            }
            else if (DataAcess.GetOrders().Where(i => i.OrderCode.Trim().Equals(TxtCode.Text.Trim())).Count() > 0) //기본키 중복
            {
                this.ShowMessageAsync("입력오류", "이미 등록된 직원 코드입니다.");
                return false;
            }
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

        private void DataClear()
        {
            TxtCode.Text = TxtName.Text = TxtPhone.Text = TxtAddr.Text = string.Empty;
            CmbFactory.SelectedItem = null;
        }
        private void DataLoad()
        {
            // 콤보박스 리스트 로드
            var Factorys = DataAcess.GetFactorys();
            foreach (var item in Factorys)
                CmbFactory.Items.Add(item.FactoryCode);
        }
    }
}
