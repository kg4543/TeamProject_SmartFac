using ERPAPP.Model;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPAPP.Helper
{
    class Common
    {
        public static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public static tblUser LOGINED_USER;

        public static tblItem SELECT_ITEM;
    }
}
