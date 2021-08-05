using ERPAPP.Model;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using NLog;
using System.Windows.Shapes;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace ERPAPP.Helper
{
    class Common
    {
        public static readonly Logger logger = LogManager.GetCurrentClassLogger();

        //현재 로그인한 유저 정보
        public static tblUser LOGINED_USER;

        //유저가 선택한 데이터
        public static tblItem SELECT_ITEM;

        public static tblICate SELECT_ICate;

        public static tblBrand SELECT_BRAND;

        public static tblOrder SELECT_ORDER;

        public static tblFactory SELECT_Factory;

        internal static tblWorker SELECT_Worker;
        internal static tblBrand SELECT_Brand;
        internal static tblMachine SELECT_Machine;
        internal static tblProduction SELECT_Production;

        public static async Task<MessageDialogResult> ShowMessageAsync(
            string title, string message, MessageDialogStyle style = MessageDialogStyle.Affirmative)
        {
            // this.
            return await ((MetroWindow)Application.Current.MainWindow)
                .ShowMessageAsync(title, message, style, null);
        }
    }
}
