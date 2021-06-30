using ERPAPP.Model;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ERPAPP.Helper
{
    class Common
    {
        public static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public static tblUser LOGINED_USER;

        public static tblItem SELECT_ITEM;

        public static tblICate SELECT_ICate;

        public static async Task<MessageDialogResult> ShowMessageAsync(
            string title, string message, MessageDialogStyle style = MessageDialogStyle.Affirmative)
        {
            // this.
            return await ((MetroWindow)Application.Current.MainWindow)
                .ShowMessageAsync(title, message, style, null);
        }
    }
}
