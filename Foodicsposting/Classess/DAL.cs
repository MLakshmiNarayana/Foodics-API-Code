using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Globalization;
using Focus.Conn;
using System.Xml;
using Focus.Common.DataStructs;
using System.Windows;

//using Focus.Masters.DataStructs;
using Focus.Transactions.DataStructs;

using Focus.TranSettings.DataStructs;


namespace Foodicsposting
{
    class DAL
    {
        public DAL()
        {
        }

        public static DataSet GetData(string strSelQry, ref string error)
        {
            try
            {
                Output output = null;
                string str = "";
                object[] objArray1 = new object[] { ExternalCallMethods.ExecuteSql, strSelQry, str };
                output = Connection.CallServeRequest(ServiceType.ExternalCall, objArray1);
                DataSet returnData = (DataSet)output.ReturnData;
                error = output.Message;
                return returnData;
            }
            catch
            {
                return null;
            }
        }

        public static Date GetIntToDate(int iDate)
        {
            try
            {
                return (new Date(iDate, CalendarType.Gregorean));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static int GetExecute(string strSelQry, ref string error)
        {
            try
            {
                Output output = null;
                string str = "";
                object[] objArray1 = new object[] { ExternalCallMethods.ExecuteNonQuery, strSelQry, str };
                output = Connection.CallServeRequest(ServiceType.ExternalCall, objArray1);
                int num = Convert.ToInt32(output.ReturnData);
                error = output.Message;
                return num;
            }
            catch
            {
                return 0;
            }
        }

        public static void Delete_Voucher(Int32 iVType, string sDocNo)
        {
            var x = Connection.CallServeRequest(ServiceType.Transaction, TransMethods.DeleteVoucher, iVType, sDocNo, Convert.ToInt32(Application.Current.Properties["LogId"]));

        }

        public static void Delete_Suspend(Int32 iVType, string sDocNo)
        {
            var x = Connection.CallServeRequest(ServiceType.Transaction, TransMethods.SuspendVouchers, iVType, sDocNo, Convert.ToInt32(Application.Current.Properties["LogId"]));
            //var x1 = Connection.CallServeRequest(ServiceType.Transaction, TransMethods.PostVoucher, iVType, sDocNo, Convert.ToInt32(Application.Current.Properties["LogId"]));
            if (x.ReturnData != null)
            {
                MessageBox.Show(x.ReturnData.ToString());
            }
        }
        public static int GetDateToInt(DateTime dt)
        {
            int val;
            val = Convert.ToInt16(dt.Year) * 65536 + Convert.ToInt16(dt.Month) * 256 + Convert.ToInt16(dt.Day);
            return val;
        }


        public static string GetStringDateToint(string dDate)
        {
            try
            {
                IFormatProvider culture = new CultureInfo("fr-FR", true);
                var dt2 = DateTime.Parse(dDate, culture, DateTimeStyles.AssumeLocal);
                return Date.DateToInt(dt2).ToString();
            }
            catch (Exception)
            {
                return "0";
            }
            
        }

        public static string GetStringDateToint_2(string dDate)
        {
            try
            {
                CultureInfo MyCultureInfo = new CultureInfo("en-US");
                DateTime dt2 = DateTime.Parse(dDate, MyCultureInfo);
                return Date.DateToInt(dt2).ToString();
            }
            catch (Exception)
            {
                return "0";
            }

        }
        private int DateToInt(DateTime dt)
        {
            int iValue = 0;

            try
            {
                iValue = new Date(dt.Year, dt.Month, dt.Day, CalendarType.Gregorean).Value;
                return (iValue);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static string getServiceLink()
        {
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                string strFileName = "";
                strFileName = System.AppDomain.CurrentDomain.BaseDirectory + "ERPXML\\ServerSettings.xml";

                xmlDoc.Load(strFileName);
                XmlNodeList nodeList = xmlDoc.DocumentElement.SelectNodes("/ServSetting/ExternalModule/ServerName");
                string strValue;
                XmlNode node = nodeList[0];
                if (node != null)
                    strValue = node.InnerText;
                else
                    strValue = "";
                return strValue;
            }
            catch
            {
                return " ";
            }

        }
        public static string Get_Focus_Next_VNo(Int32 iVT)
        {
            string sDocNo = "";
            try
            {
             //   MessageBox.Show("Before get next voucher -1");
                SeriesValues objSeriesVals = new SeriesValues();
                objSeriesVals.VoucherDate = new Focus.Common.DataStructs.Date(Convert.ToDateTime(DateTime.Now).Year,
                    Convert.ToDateTime(DateTime.Now).Month, Convert.ToDateTime(DateTime.Now).Day, CalendarType.Gregorean);//  FConvert.DateToInt(DateTime.Now));

                //int m_iVoucherType = 3592;
               // MessageBox.Show("Before get next voucher -2");
                Output objReturn = Connection.CallServeRequest(ServiceType.Transaction, TransMethods.GetNewVoucherNo, iVT, objSeriesVals);
                if (objReturn.ReturnData != null)
                    sDocNo = objReturn.ReturnData.ToString(); //(string)objReturn.ReturnData;
                else
                    sDocNo = "1";
            }
            catch (Exception ex)
            {
                //MessageBox.Show("SM " + ex.Message);
                sDocNo = "0";
            }
            return sDocNo;

        }
        public static string GetVoucherNo(int ivouchertype, string prefix, int Noofdigits)
        {
            string sPrefix = default(string);
            string strerror = default(string);
            string svoucherno = default(string);
            sPrefix = prefix; //warehouse code
            string StrQry = "select isnull(MAX(CAST(DocNo as int)),0)+1 as DocNo from(select SUBSTRING(DocNo,len('" + sPrefix + "')+1,len(DocNo)) as DocNo from " +
                "(select distinct(svoucherno) as DocNo from tCore_Header_0   where iVoucherType=" + ivouchertype + " and " +
                "sVoucherNo  like '" + sPrefix + "%')t)t1 where DocNo NOT LIKE '%[a-z]%' AND DocNo NOT LIKE '%.%'";

            StrQry = "select isnull(max(cast(LEFT(sVoucherNo, LEN(sVoucherNo)-0) as int)), 0) + 1 as DocNo from ( " +
                     "select right(sVoucherNo, LEN(sVoucherNo)-(len('" + sPrefix + "')+1)) sVoucherNo from ( " +
                     " select sVoucherNo from tCore_Header_0 with (readuncommitted) where iVoucherType = " + ivouchertype + " and LOWER(sVoucherNo) like LOWER('" + sPrefix + "-[0-9]%') ) a) b";
            DataSet ds = DAL.GetData(StrQry, ref strerror);
            if (ds.Tables[0].Rows.Count > 0)
            {
                svoucherno = Convert.ToString(ds.Tables[0].Rows[0]["DocNo"]);

                for (int i = svoucherno.Length; i < Noofdigits; i++)
                {
                    svoucherno = "0" + svoucherno;
                }

                svoucherno = sPrefix + '-' + svoucherno;
            }
            return svoucherno;
        }


    }
}
