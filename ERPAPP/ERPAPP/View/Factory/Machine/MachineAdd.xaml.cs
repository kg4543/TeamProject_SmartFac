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

namespace ERPAPP.View.Factory.Machine
{
    /// <summary>
    /// MachineAdd.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MachineAdd : MetroWindow
    {
        public MachineAdd()
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
                tblMachine machine = new tblMachine()
                {
                    MachineCode = TxtCode.Text,
                    MachineName = TxtName.Text,
                    MCateCode = CmbCategory.Text,
                    FactoryCode = CmbFactory.Text,
                    RegDate = DateTime.Now.Date,
                    RegID = Common.LOGINED_USER.UserId.ToString()
                };
                DataAcess.SetMachine(machine);

                var result = await this.ShowMessageAsync("데이터 등록", "직원정보가 등록되었습니다.\n 추가 등록하시겠습니까?",
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
                this.ShowMessageAsync("입력오류", "기계 코드를 입력해주세요.");
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
            if (CmbCategory.SelectedItem == null)
            {
                this.ShowMessageAsync("입력오류", "카테고리 코드를 입력해주세요.");
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
            TxtCode.Text = TxtName.Text = string.Empty;
            CmbCategory.SelectedItem = CmbFactory.SelectedItem = null;
        }

        private void DataLoad()
        {
            // 콤보박스 리스트 로드
            var Categorys = DataAcess.GetMcCate();
            foreach (var item in Categorys)
                CmbCategory.Items.Add(item.McateCode);

            var Factorys = DataAcess.GetFactory();
            foreach (var item in Factorys)
                CmbFactory.Items.Add(item.FactoryCode);
        }
    }
}
