using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Foodicsposting.Classess;
using Focus.Common.DataStructs;
using Focus.Conn;
using Focus.Masters.DataStructs;
using Focus.Transactions.DataStructs;
//using Focus.TranSettings.BL;
using Focus.TranSettings.DataStructs;
namespace Foodicsposting
{
    class ClsMain
    {
        public bool Foodic_Inetgration()
        {
            using (var frm = new Foodicsposting.Form1())
            {
                frm.ShowDialog();
            }

            return true;
        }
    }
}
