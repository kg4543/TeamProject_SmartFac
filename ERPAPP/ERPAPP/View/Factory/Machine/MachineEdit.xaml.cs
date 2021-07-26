using ERPAPP.Helper;
using ERPAPP.Logic;
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
    /// MachineEdit.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MachineEdit : MetroWindow
    {
        public MachineEdit()
        {
            InitializeComponent();
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            DataLoad();
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

            var selectedItem = Common.SELECT_Machine;

            TxtCode.Text = selectedItem.MachineCode.ToString();
            TxtName.Text = selectedItem.MachineName.ToString();
            CmbCategory.Text = selectedItem.MCateCode.ToString();
            CmbFactory.Text = selectedItem.FactoryCode.ToString();
        }

        private async void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (IsValid())
            {
                var selectedItem = Common.SELECT_Machine;

                selectedItem.MachineName = TxtName.Text;
                selectedItem.MCateCode = CmbCategory.Text;
                selectedItem.FactoryCode = CmbFactory.Text;
                selectedItem.ModDate = DateTime.Now.Date;
                selectedItem.ModID = Common.LOGINED_USER.UserId.ToString();

                DataAcess.SetMachine(selectedItem);

                await this.ShowMessageAsync("데이터 수정", "기계 정보가 수정되었습니다.");
                Close();
            }
        }

        private bool IsValid()
        {
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

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
