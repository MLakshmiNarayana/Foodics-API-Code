using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Foodicsposting.Classess;
using System.Windows.Forms;
//using Classess;
//using Focus.Common.DataStructs;
//using Focus.Conn;
using System.Data.SqlClient;
using Foodicsposting.TransactionService;
using Newtonsoft.Json.Linq;
using System.Net;
using Application = System.Windows.Application;
using Focus.Conn;
using MessageBox = System.Windows.Forms.MessageBox;

namespace Foodicsposting
{
    public partial class Form1 : Form
    {
        private string sMysql = "";
        private string mysql = "";
        private string qry = "";
        string ServerIp = "localhost";
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Load_orders();
        }
        private void get_orders()
        {

            dgv.DataSource = null;
            dgv.Columns.Clear();
            string date = dtp.Value.ToString("yyyy-MM-dd");
            sMysql = "select ori.Order_RefNo, ori.Business_date, br.Name as BranchName, ori.Branch_hid, ori.Order_Final_price, ori.Tax_amount, pm.Name as Payment_method_name, op.Amount from Orders ori join Branches br on br.hid = ori.Branch_hid join Order_PaymentMethods op on op.Order_refNo = ori.Order_RefNo join PaymentMethods pm on pm.hid=op.payment_method_hid where ori.Business_date ='" + date + "'";

            DataSet dSM = ClsSql.GetDs(sMysql);
            try
            {

                DataTable dt = dSM.Tables[0];
                cmbref.DataSource = dt;
                cmbref.DisplayMember = "Order_RefNo";
                //cmbref.ValueMember = "Order_RefNo";
                //cmbdiv.DataSource = dt;
                cmbdiv.Text = "FUNKEY MONKEY";
                //cmbdiv.ValueMember = "FUNKEY MONKEY";
                //cmbcc.DataSource = dt;
                cmbcc.Text = "";
                //cmbcc.ValueMember = "";
                cmbbr.DataSource = dt;
                cmbbr.DisplayMember = "BranchName";
                cmbbr.ValueMember = "Branch_hid";

                //cmbjd.DataSource = dt;

                cmbjd.Text = "Saudi Arabia";
                //cmbjd.ValueMember = "Saudi Arabia";
                // cmbps.DataSource = dt;
                cmbps.Text = "Saudi Arabia";

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void Load_orders()
        {

            dgv.DataSource = null;
            dgv.Columns.Clear();
            string date = dtp.Value.ToString("yyyy-MM-dd");
            sMysql = "select ori.Order_RefNo,ori.Business_date,br.Name as BranchName,ori.Branch_hid,ori.Order_Final_price,ori.Tax_amount," +
                     "(select[FocusAccount] from vsm_FocusAcc_PaymentMthd where sno = 0)[SalesAccount],pm.Name as Payment_method_name," +
                     "fpm.[FocusAccount], op.Amount from Orders ori join Branches br on br.hid = ori.Branch_hid join Order_PaymentMethods op on op.Order_refNo = ori.Order_RefNo join PaymentMethods pm on pm.hid = op.payment_method_hid join vsm_FocusAcc_PaymentMthd fpm on fpm.[PaymentMethod] collate Arabic_CI_AS = pm.Name  collate Arabic_CI_AS where br.Name = '" + cmbbr.Text + "' and ori.Business_date = '" + date + "' order by ori.Order_RefNo asc";

            DataSet dSM = ClsSql.GetDs(sMysql);
            try
            {

                DataTable dt = dSM.Tables[0];
                cmbref.DataSource = dt;
                cmbref.DisplayMember = "Order_RefNo";
                //cmbref.ValueMember = "Order_RefNo";
                //cmbdiv.DataSource = dt;
                cmbdiv.Text = "FUNKEY MONKEY";
                //cmbdiv.ValueMember = "FUNKEY MONKEY";
                //cmbcc.DataSource = dt;
                cmbcc.Text = "";
                //cmbcc.ValueMember = "";
                cmbbr.DataSource = dt;
                cmbbr.DisplayMember = "BranchName";
                cmbbr.ValueMember = "Branch_hid";

                //cmbjd.DataSource = dt;

                cmbjd.Text = "Saudi Arabia";
                //cmbjd.ValueMember = "Saudi Arabia";
                // cmbps.DataSource = dt;
                cmbps.Text = "Saudi Arabia";
                //cmbps.ValueMember = "Saudi Arabia";
                //dgv.DataSource = dt;
                dgv.AutoGenerateColumns = false;
                dgv.ColumnCount = 5;
                dgv.Columns[0].Name = "Order_RefNo";
                dgv.Columns[0].HeaderText = "Order_refNo";
                dgv.Columns[0].DataPropertyName = "Order_RefNo";
                dgv.Columns[1].Name = "Account";
                dgv.Columns[1].HeaderText = "Account";
                dgv.Columns[1].DataPropertyName = "FocusAccount";

                dgv.Columns[2].HeaderText = "Credit";
                dgv.Columns[2].Name = "Credit";
                dgv.Columns[2].DataPropertyName = "Order_Final_price";
                dgv.Columns[3].Name = "Sales Account";
                dgv.Columns[3].HeaderText = "Sales Account";
                dgv.Columns[3].DataPropertyName = "SalesAccount";

                dgv.Columns[4].Name = "Debit";
                dgv.Columns[4].HeaderText = "Debit";
                dgv.Columns[4].DataPropertyName = "Amount";
                //dgv.Columns[3].Name = "Tax_amount";
                // dgv.Columns[3].HeaderText = "Tax_amount";
                //dgv.Columns[3].DataPropertyName = "Tax_amount";
                dgv.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public void saveorders()
        {
            try
            {

                string sSession = "";
                string uid = "";
                string strServer = "";

                string date = dtp.Value.ToString("yyyy-MM-dd");
               // string sSession = null;
                _MTrans objMTrans = new _MTrans();

                //double Com = Convert.ToInt32(System.Windows.Application.Current.Properties["CompId"].ToString());
                //int UserId = Convert.ToInt32(System.Windows.Application.Current.Properties["UserId"]);
                //int Logid = Convert.ToInt32(System.Windows.Application.Current.Properties["LogId"]);
                //int LanguageId = Convert.ToInt32(System.Windows.Application.Current.Properties["LanguageId"]);
                //string uid = null;
                //  uid = Application.Current.Properties["UserId"].ToString();
                uid = System.Windows.Application.Current.Properties["UserId"].ToString();

                System.Collections.ICollection ht = System.Windows.Application.Current.Properties.Keys;
                sSession = System.Windows.Application.Current.Properties["SessionId"].ToString();
                //sSession =System.Windows.Application.Current.Properties["SessionId"].ToString();


                int iRow = 0;
                string CurrId = "1";
                decimal dDrAmount = 0, dCrAmount = 0;
                //_MTrans objMTrans = new _MTrans();
                Boolean blnpost = false;
                //string refno = "FNMKB01C017075";
                //Boolean bSelect = false;
                //select DISTINCT Order_RefNo from [v_salesorder] WHERE Order_RefNo = 'FNMKB01C017075'
                qry = "select distinct ori.Order_RefNo,ori.Business_date,br.Name as BranchName,ori.Branch_hid,ori.Order_Final_price,ori.Tax_amount,(select[FocusAccount] from vsm_FocusAcc_PaymentMthd where sno = 0)[SalesAccount],pm.Name as Payment_method_name,fpm.[FocusAccount], op.Amount from Orders ori join Branches br on br.hid = ori.Branch_hid join Order_PaymentMethods op on op.Order_refNo = ori.Order_RefNo join PaymentMethods pm on pm.hid = op.payment_method_hid join vsm_FocusAcc_PaymentMthd fpm on fpm.[PaymentMethod] collate Arabic_CI_AS = pm.Name  collate Arabic_CI_AS where ori.Business_date = '" + date + "' order by ori.Order_RefNo asc";
                DataSet sd = ClsSql.GetDs(qry);

                for (int j = 0; j < sd.Tables[0].Rows.Count; j++)
                {
                    string sr = sd.Tables[0].Rows[0]["Order_RefNo"].ToString();
                    sMysql = "select DISTINCT Order_RefNo from [v_orderssalesdetails] WHERE Order_RefNo = '" + sr + "'";
                
                DataSet ds = ClsSql.GetDs(sMysql);
                //dgv.AllowUserToAddRows = false;
                //dgv.DataSource = ds.Tables[0];
                //select * from [v_salesorder] WHERE Order_RefNo = DS.Table[0].rows[0][0].value
                string sRefNo = "";
                for (int d = 0; d < ds.Tables[0].Rows.Count; d++)
                {
                    sRefNo = ds.Tables[0].Rows[0][0].ToString();
                  mysql = "select *, case FocusAccount when '10201020003' then 1864 when '10201010005' then 1862 when '20302040001' then 1620  end AccId from [v_orderssalesdetails] WHERE Order_RefNo = '" + sRefNo + "'";
                    DataSet dst = ClsSql.GetDs(mysql);
                    // dgv.AllowUserToAddRows = false;
                    //dgv.DataSource = dst.Tables[0];
                    int icnt = 0;
                    for (int i = 0; i < dst.Tables[0].Rows.Count; i++)
                    {
                        //var cell = dgv.Rows[k].Cells["Order_RefNo"].Value; // Nul value handling
                        // bSelect = (cell.Value != null && cell.Value != "")? Convert.ToBoolean(dgv.Rows[k].Cells["Order_RefNo"].Value.ToString()): false;
                        if (icnt == 0)
                        {
                            List<Foodicsposting.TransactionService.IdNameTag> lstHeaderData = new List<Foodicsposting.TransactionService.IdNameTag>();
                            lstHeaderData.Add(new Foodicsposting.TransactionService.IdNameTag { sName = "DocNo", Value = sRefNo });
                            lstHeaderData.Add(new Foodicsposting.TransactionService.IdNameTag { sName = "Date", Value = dst.Tables[0].Rows[0]["BusinessDate"].ToString() });
                            lstHeaderData.Add(new Foodicsposting.TransactionService.IdNameTag { sName = "Branch", Value = "10" });//14,17,10 // "Jeddah Branch B01" });
                            lstHeaderData.Add(new Foodicsposting.TransactionService.IdNameTag { sName = "Division", Value = "5" });// "Funky Monky" });
                            lstHeaderData.Add(new Foodicsposting.TransactionService.IdNameTag { sName = "Placeof supply", Value = "8" }); //UAE
                            lstHeaderData.Add(new Foodicsposting.TransactionService.IdNameTag { sName = "Jurisdiction", Value = "8" }); //UAE

                            objMTrans.arrHeader = lstHeaderData.ToArray();
                            string[] arrBodyNames = new string[6];
                            arrBodyNames[0] = "Account";
                            arrBodyNames[1] = "TaxCode";
                            arrBodyNames[2] = "Debit";
                            arrBodyNames[3] = "Credit";
                            arrBodyNames[4] = "VAT";
                            arrBodyNames[5] = "sRemarks";
                            objMTrans.arrBodyNames = arrBodyNames;

                            objMTrans.arrBody = new object[dst.Tables[0].Rows.Count + 1][];

                            object[] arrBodyVals = new object[6];
                            arrBodyVals[0] = 1769;// dst.Tables[0].Rows[0]["Account"].ToString();// dgv.Rows[0].Cells["Account"].Value;
                            arrBodyVals[1] = "2";// dgv.Rows[k].Cells["Account"].Value;
                            arrBodyVals[2] = 0; // sales account
                            arrBodyVals[3] = dst.Tables[0].Rows[0]["Order_Final_price"].ToString();// dgv.Rows[0].Cells["Credit"].Value; // total creait
                            arrBodyVals[4] = "0";
                            arrBodyVals[5] = "Integratoin data";
                            objMTrans.arrBody[0] = arrBodyVals;
                            icnt += 1;
                        }

                        object[] arrBodyVals1 = new object[6];
                        arrBodyVals1[0] = dst.Tables[0].Rows[i]["AccId"].ToString(); // 2nd account
                        arrBodyVals1[1] = "2";// tax code
                        arrBodyVals1[2] = dst.Tables[0].Rows[i]["Amount"].ToString(); // Payment Method Amount
                        arrBodyVals1[3] = 0; // 
                        arrBodyVals1[4] = "0";
                        arrBodyVals1[5] = "Integratoin data";
                        objMTrans.arrBody[i + 1] = arrBodyVals1; // intcrementin array
                        icnt += 1;
                    }
                    JObject jsonobj = new JObject();
                    jsonobj = new JObject();
                    jsonobj.Add("objMTrans", JToken.FromObject(objMTrans));
                    jsonobj.Add("iVoucherType", JToken.FromObject(3591));

                    jsonobj.Add("sDocNo", "");

                    //Add parameter
                    jsonobj.Add("bByIds", false);//Add parameter if By field Ids then true, if by Field Names then false
                    jsonobj.Add("bIsNew", true);
                    using (WebClient client = new WebClient())
                    {
                        client.Headers.Add("fsessionid", sSession);
                        client.Headers.Add("Content-Type", "application/json");
                        string pathStr = "http://" + ServerIp + "/Focus8Library/TransactionService.svc/SaveVoucher2";
                        string arrResponse = client.UploadString(pathStr, jsonobj.ToString());

                        SaveResult.saveresult.RootObject MObjRoot = new SaveResult.saveresult.RootObject();
                        MObjRoot = Newtonsoft.Json.JsonConvert.DeserializeObject<SaveResult.saveresult.RootObject>(arrResponse);
                        if (MObjRoot.SaveVoucher2Result.arrTransIds != null)
                        {
                            //System.Windows.MessageBox.Show("Records Posted Successfully voucherno : " + MObjRoot.SaveVoucher2Result.sValue, "Posting Status", MessageBoxButton.OK, MessageBoxImage.Information);
                            MessageBox.Show("Data posted successfully" + MObjRoot.SaveVoucher2Result.sValue);
                        }
                        else
                        {

                            MessageBox.Show(MObjRoot.SaveVoucher2Result.sValue);

                        }

                    }
                    // for 1distincr vourncer 
                }
            }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            get_orders();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            saveorders();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            Load_orders();
            get_orders();
            saveorders();
        }

        private void dtp_ValueChanged(object sender, EventArgs e)
        {

        }
    }
}
