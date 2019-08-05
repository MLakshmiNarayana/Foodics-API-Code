using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foodicsposting.Classess
{
    class SaveResult
    {
        public class saveresult
        {
            public class RootObject
            {
                public SaveVoucher2Result SaveVoucher2Result { get; set; }
                public SaveMasterByNamesResult SaveMAsterByNamesResult { get; set; }
            }
            public class SaveMasterByNamesResult
            {
                public Result iresult { get; set; }
                public int lResult { get; set; }
            }
            public class Result
            {
                public int lResult { get; set; }
                public string sValue { get; set; }
            }
            public class UserLoginResult
            {

                public Result Result { get; set; }

            }
            public class SaveVoucher2Result
            {
                public List<string> arrTransIds { get; set; }
                public int lResult11 { get; set; }
                public string sValue { get; set; }
            }

        }
    }
}
