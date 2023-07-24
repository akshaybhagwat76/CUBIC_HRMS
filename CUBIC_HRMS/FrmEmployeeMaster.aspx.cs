using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using static CUBIC_HRMS.GlobalVariable;
using static CUBIC_HRMS.GlobalProjectClass;

namespace CUBIC_HRMS
{
    public partial class FrmEmployeeMaster : System.Web.UI.Page
    {
        public static string G_Editable = "P";
        public static string G_Viewable = "P";
        public static string G_ViewConfidential = "N";
        public static string G_IsConfidential = "N";
        public string UploadAllPass = "False";

        public object DialogResult { get; private set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //MultiView1.ActiveViewIndex = 0;
                populateEmpCodeddl();
                populateDirectCodeddl();
                populateOnBehalf();

                populatePosition();
                populateDepartment();
                populateCurrency();
                populateCountry();
                populateBankCode();


                dtDOB.Text = DateTime.Now.ToLocalTime().ToString("yyyy-MM-dd");
                dtDateJoin.Text = DateTime.Now.ToLocalTime().ToString("yyyy-MM-dd");
                DTDatePayReview.Text = DateTime.Now.ToLocalTime().ToString("yyyy-MM-dd");
                dtDateResign.Text = DateTime.Now.ToLocalTime().ToString("yyyy-MM-dd");

                dtDateResign.Visible = false;

                GetAccessRightEditable(CUBIC_HRMS.GlobalVariable.G_UserLogin);

                //check is shawn login
                //if(CUBIC_HRMS.GlobalVariable.G_UserLogin=="1")
                //{
                //    ProtectforShawn();
                //}

                if (G_Editable == "F" && G_Viewable == "F")
                {
                    if (G_ViewConfidential == "N")
                    {
                        CheckEmpIsConf();
                        //Can edit and add salary and allowance, but cannot see confidential Y.Ex:Shawn
                        if (G_IsConfidential == "Y")
                        {
                            txtSalaryWedges.Enabled = false;
                            txtSalaryWedges.Visible = false;

                            lblSVSalaryWedges.Visible = false;
                            lblSVSalCurrency.Visible = false;


                            lblSVAllWages.Visible = false;
                            lblSVAllCurrency.Visible = false;

                            ddlSalaryCurrency.Enabled = false;
                            ddlSalaryCurrency.Enabled = false;


                            txtAllowanceWedges.Enabled = false;
                            txtAllowanceWedges.Visible = false;

                            ddlAllowanceCurrency.Enabled = false;
                            ddlAllowanceCurrency.Visible = false;
                        }
                        else
                        {
                            txtSalaryWedges.Enabled = true;
                            txtSalaryWedges.Visible = true;

                            ddlSalaryCurrency.Enabled = true;
                            ddlSalaryCurrency.Enabled = true;

                            txtAllowanceWedges.Enabled = true;
                            txtAllowanceWedges.Visible = true;

                            ddlAllowanceCurrency.Enabled = true;
                            ddlAllowanceCurrency.Visible = true;
                        }
                    }
                    else
                    {
                        //For super user & HR Manager
                        txtSalaryWedges.Enabled = true;
                        txtSalaryWedges.Visible = true;

                        ddlSalaryCurrency.Enabled = true;
                        ddlSalaryCurrency.Enabled = true;

                        txtAllowanceWedges.Enabled = true;
                        txtAllowanceWedges.Visible = true;

                        ddlAllowanceCurrency.Enabled = true;
                        ddlAllowanceCurrency.Visible = true;

                    }
                }

                if (G_Editable == "N" && G_Viewable == "N" && G_ViewConfidential == "N")
                {
                    ProtectAccess();
                }

                
            }


        }

        private void ProtectAccess()
        {
            txtSalaryWedges.Enabled = false;
            txtSalaryWedges.Visible = false;

            ddlSalaryCurrency.Enabled = false;
            ddlSalaryCurrency.Visible = false;

            txtAllowanceWedges.Enabled = false;
            txtAllowanceWedges.Visible = false;

            ddlAllowanceCurrency.Enabled = false;
            ddlAllowanceCurrency.Visible = false;

            lblSVSalaryWedges.Visible = false;
            lblSVSalCurrency.Visible = false;

            lblSVAllWages.Visible = false;
            lblSVAllCurrency.Visible = false;

            lblConEmpCode.Visible = false;
            ddlConEmpCode.Visible = false;
        }


        private void ProtectForLevel99()
        {
            MultiView1.ActiveViewIndex = 3;

            populateConEmpCodeddl();
            lblConEmpCode.Visible = true;
            ddlConEmpCode.Visible = true;

            btnBackPage3.Visible = false;
            btnSubmit.Visible = false;
        }


        private void populateCurrency()
        {
            SqlConnection Conn = new SqlConnection(CUBIC_HRMS.GlobalVariable.ConnectionString);
            GF_CheckConnectionStatus(Conn);
            Conn.Open();

            try
            {
                string SQLCommand = "SELECT DISTINCT [CURR_CODE] FROM [dbo].M_EXCHANGE_CURRENCY ORDER BY CURR_CODE ";
                SqlDataAdapter da = new SqlDataAdapter(SQLCommand, Conn);
                DataTable dt = new DataTable();

                ddlSalaryCurrency.Items.Clear();
                ddlSalaryCurrency.SelectedValue = null;
                ddlAllowanceCurrency.Items.Clear();
                ddlAllowanceCurrency.SelectedValue = null;

                da.Fill(dt);
                ddlSalaryCurrency.DataSource = dt;
                ddlSalaryCurrency.DataBind();
                ddlSalaryCurrency.DataTextField = "CURR_CODE";
                ddlSalaryCurrency.DataValueField = "CURR_CODE";
                ddlSalaryCurrency.DataBind();

                ddlAllowanceCurrency.DataSource = dt;
                ddlAllowanceCurrency.DataBind();
                ddlAllowanceCurrency.DataTextField = "CURR_CODE";
                ddlAllowanceCurrency.DataValueField = "CURR_CODE";
                ddlAllowanceCurrency.DataBind();

                // Then add your first item
                ddlSalaryCurrency.Items.Insert(0, "MYR");
                ddlAllowanceCurrency.Items.Insert(0, "MYR");

            }
            catch (Exception ex)
            {
                MessageBoxShow("There is no Data");

            }
            finally
            {
                Conn.Dispose();
                Conn.Close();
            }
        }
        public void GetAccessRightEditable(string _Login)
        {

            if (CUBIC_HRMS.GlobalVariable.G_UserLevel == 99)
            {
                G_Editable = "F";
                G_Viewable = "F";
            }
            else
            {
                G_Editable = "P";
                G_Viewable = "P";
                G_ViewConfidential = "N";

                SqlConnection Conn = new SqlConnection(ConnectionString);

                string Query = "SELECT [EMP_EDITTYPE],[EMP_VIEWTYPE],[EMP_VIEWCONFIDENTIAL] ";
                Query = Query + "FROM [dbo].[M_EMP_LOGIN] ";
                Query = Query + "WHERE EMP_CODE = '" + _Login + "' ";

                SqlCommand cmdDataBase = new SqlCommand(Query, Conn);

                SqlDataReader myReader;

                try
                {
                    GF_CheckConnectionStatus(Conn);
                    Conn.Open();
                    myReader = cmdDataBase.ExecuteReader();

                    if (myReader.HasRows)
                    {
                        while (myReader.Read())
                        {
                            G_Editable = (myReader["EMP_EDITTYPE"].ToString());
                            G_Viewable = (myReader["EMP_VIEWTYPE"].ToString());
                            G_ViewConfidential = (myReader["EMP_VIEWCONFIDENTIAL"].ToString());
                        }
                    }

                }
                catch (Exception ex)
                {
                    string TempAudit = ex.ToString().Replace("'", "");
                    GF_InsertAuditLog("-", "Data Check", "FrmEmployeeMaster", TempAudit, "GetAccessRightEditable");
                }
                finally
                {
                    Conn.Close();
                    Conn.Dispose();
                }
            }
        }


        // make sure can edit other but cannot edit shawn n adrian
        private void CheckEmpIsConf()
        {

            G_IsConfidential = "N";


            SqlConnection Conn = new SqlConnection(ConnectionString);

            string Query = "SELECT EMP_ISCONFIDENTIAL ";
            Query = Query + "FROM [dbo].[M_EMP_LOGIN] ";
            Query = Query + "WHERE EMP_CODE = '" + txtEmpCode.Text + "' ";

            SqlCommand cmdDataBase = new SqlCommand(Query, Conn);

            SqlDataReader myReader;

            try
            {
                GF_CheckConnectionStatus(Conn);
                Conn.Open();
                myReader = cmdDataBase.ExecuteReader();

                while (myReader.Read())
                {
                    G_IsConfidential = (myReader["EMP_ISCONFIDENTIAL"]).ToString();
                }
            }
            catch (Exception ex)
            {
                string message = ex.Message.ToString();
                MessageBoxShow(message);
            }
            finally
            {
                Conn.Close();
                Conn.Dispose();
            }
        }


        private void populateEmpCodeddl()
        {
            SqlConnection Conn = new SqlConnection(CUBIC_HRMS.GlobalVariable.ConnectionString);
            GF_CheckConnectionStatus(Conn);
            Conn.Open();
            string SQLCommand = "-";

            try
            {

                SQLCommand = "SELECT [EMP_CODE], [EMP_CODE]+'--'+[EMP_NICK_NAME] AS [COMBINE] ";
                SQLCommand = SQLCommand + "FROM[dbo].[M_EMP_MASTER] ";
                SQLCommand = SQLCommand + "WHERE EMP_CODE NOT IN ('Super') ";
                //SQLCommand = SQLCommand + "AND EMP_BU = '" + CUBIC_HRMS.GlobalVariable.G_User_BU + "' ";
                SQLCommand = SQLCommand + "ORDER BY EMP_CODE";


                SqlDataAdapter adpt = new SqlDataAdapter(SQLCommand, Conn);
                DataTable dt = new DataTable();

                ddlSelectEmpCode.Items.Clear();
                ddlSelectEmpCode.SelectedValue = null;
                adpt.Fill(dt);
                ddlSelectEmpCode.DataSource = dt;
                ddlSelectEmpCode.DataBind();
                ddlSelectEmpCode.DataTextField = "COMBINE";
                ddlSelectEmpCode.DataValueField = "EMP_CODE";
                ddlSelectEmpCode.DataBind();
                ddlSelectEmpCode.Items.Insert(0, "Create New");

            }
            catch (Exception ex)
            {
                string TempAudit = ex.ToString().Replace("'", "");
                GF_InsertAuditLog("-", "Data Check", "FrmEmployeeMaster", TempAudit, "populateEmpCodeddl");
            }
            finally
            {
                Conn.Dispose();
                Conn.Close();
            }
        }


        private void populateConEmpCodeddl()
        {
            SqlConnection Conn = new SqlConnection(CUBIC_HRMS.GlobalVariable.ConnectionString);
            GF_CheckConnectionStatus(Conn);
            Conn.Open();
            string SQLCommand = "-";

            try
            {
                SQLCommand = "SELECT [EMP_CODE], [EMP_CODE]+'--'+[EMP_NICK_NAME] AS [COMBINE] ";
                SQLCommand = SQLCommand + "FROM[dbo].[M_EMP_MASTER] ";
                SQLCommand = SQLCommand + "WHERE EMP_CODE  NOT IN ('Super') ";
                //SQLCommand = SQLCommand + "AND EMP_BU = '" + CUBIC_HRMS.GlobalVariable.G_User_BU + "' ";
                SQLCommand = SQLCommand + "ORDER BY EMP_CODE";

                SqlDataAdapter adpt = new SqlDataAdapter(SQLCommand, Conn);
                DataTable dt = new DataTable();


                this.ddlConEmpCode.Items.Clear();
                this.ddlConEmpCode.SelectedValue = null;
                adpt.Fill(dt);
                ddlConEmpCode.DataSource = dt;
                ddlConEmpCode.DataBind();
                ddlConEmpCode.DataTextField = "COMBINE";
                ddlConEmpCode.DataValueField = "EMP_CODE";
                ddlConEmpCode.DataBind();
                // Then add your first item
                ddlConEmpCode.Items.Insert(0, "- Select -");

            }
            catch (Exception ex)
            {
                string TempAudit = ex.ToString().Replace("'", "");
                GF_InsertAuditLog("-", "Data Check", "FrmEmployeeMaster", TempAudit, "populateConEmpCodeddl");
            }
            finally
            {
                Conn.Dispose();
                Conn.Close();
            }
        }

        private void populateDirectCodeddl()
        {
            SqlConnection Conn = new SqlConnection(CUBIC_HRMS.GlobalVariable.ConnectionString);
            GF_CheckConnectionStatus(Conn);
            Conn.Open();

            try
            {

                string SQLCommand = "-";
                SQLCommand = "SELECT  [EMP_CODE], [EMP_CODE]+'--'+[EMP_NICK_NAME] AS [COMBINE] ";
                SQLCommand = SQLCommand + "FROM [dbo].[M_EMP_MASTER] ";
                SQLCommand = SQLCommand + "WHERE EMP_CODE NOT IN ('Super') ";
                //SQLCommand = SQLCommand + "AND EMP_BU = '" + CUBIC_HRMS.GlobalVariable.G_User_BU + "' ";
                SQLCommand = SQLCommand + "ORDER BY EMP_CODE ";

                SqlDataAdapter da = new SqlDataAdapter(SQLCommand, Conn);
                DataTable dt = new DataTable();

                ddlSelectDSuperior.Items.Clear();
                ddlSelectDSuperior.SelectedValue = null;
                da.Fill(dt);
                ddlSelectDSuperior.DataSource = dt;
                ddlSelectDSuperior.DataBind();
                ddlSelectDSuperior.DataTextField = "COMBINE";
                ddlSelectDSuperior.DataValueField = "EMP_CODE";
                ddlSelectDSuperior.DataBind();
                // Then add your first item
                ddlSelectDSuperior.Items.Insert(0, "- Select -");

            }
            catch (Exception ex)
            {
                MessageBoxShow("There is no Data");

            }
            finally
            {
                Conn.Dispose();
                Conn.Close();
            }
        }


        private void populateOnBehalf()
        {
            SqlConnection Conn = new SqlConnection(CUBIC_HRMS.GlobalVariable.ConnectionString);
            GF_CheckConnectionStatus(Conn);
            Conn.Open();

            try
            {
                string SQLCommand = "-";
                SQLCommand = "SELECT  [EMP_CODE], [EMP_CODE]+'--'+[EMP_NICK_NAME] AS [COMBINE] ";
                SQLCommand = SQLCommand + "FROM [dbo].[M_EMP_MASTER] ";
                SQLCommand = SQLCommand + "WHERE EMP_CODE NOT IN ('Super') ";
                //SQLCommand = SQLCommand + "AND EMP_BU = '" + CUBIC_HRMS.GlobalVariable.G_User_BU + "' ";
                SQLCommand = SQLCommand + "ORDER BY EMP_CODE ";

                SqlDataAdapter da = new SqlDataAdapter(SQLCommand, Conn);
                DataTable dt = new DataTable();

                this.ddlSelectOnBehalf.Items.Clear();
                this.ddlSelectOnBehalf.SelectedValue = null;
                da.Fill(dt);
                ddlSelectOnBehalf.DataSource = dt;
                ddlSelectOnBehalf.DataBind();
                ddlSelectOnBehalf.DataTextField = "COMBINE";
                ddlSelectOnBehalf.DataValueField = "EMP_CODE";
                ddlSelectOnBehalf.DataBind();
                // Then add your first item
                ddlSelectOnBehalf.Items.Insert(0, "- Select -");

            }
            catch (Exception ex)
            {
                MessageBoxShow("There is no Data");

            }
            finally
            {
                Conn.Dispose();
                Conn.Close();
            }
        }

        //private void populateBU()
        //{

        //    SqlConnection Conn = new SqlConnection(CUBIC_HRMS.GlobalVariable.ConnectionString);
        //    GF_CheckConnectionStatus(Conn);
        //    Conn.Open();

        //    try
        //    {
        //        SqlDataAdapter da = new SqlDataAdapter("SELECT BU_CODE, BU_NAME FROM [dbo].[M_BU] WHERE BU_CODE = '" + CUBIC_HRMS.GlobalVariable.G_User_BU + "'", Conn);
        //        DataTable dt = new DataTable();

        //        this.ddlBU.Items.Clear();
        //        this.ddlBU.SelectedValue = null;
        //        da.Fill(dt);
        //        ddlBU.DataSource = dt;
        //        ddlBU.DataBind();
        //        ddlBU.DataTextField = "BU_NAME";
        //        ddlBU.DataValueField = "BU_CODE";
        //        ddlBU.DataBind();
        //        // Then add your first item
        //        ddlBU.Items.Insert(0, "- Select -");
        //    }
        //    catch (Exception ex)
        //    {
        //        string TempAudit = ex.ToString().Replace("'", "");
        //        GF_InsertAuditLog("-", "Data Check", "FrmEmployeeMaster", TempAudit, "populateBU");
        //    }
        //    finally
        //    {
        //        Conn.Dispose();
        //        Conn.Close();
        //    }
        //}



        private void populateCountry()
        {

            SqlConnection Conn = new SqlConnection(CUBIC_HRMS.GlobalVariable.ConnectionString);
            GF_CheckConnectionStatus(Conn);
            Conn.Open();

            try
            {
                SqlDataAdapter da = new SqlDataAdapter("SELECT COUNTRY_NAME FROM [dbo].[M_COUNTRY] ORDER BY COUNTRY_NAME ", Conn);
                DataTable dt = new DataTable();

                ddlCitizen.Items.Clear();
                ddlCitizen.SelectedValue = null;
                da.Fill(dt);
                ddlCitizen.DataSource = dt;
                ddlCitizen.DataBind();
                ddlCitizen.DataTextField = "COUNTRY_NAME";
                ddlCitizen.DataValueField = "COUNTRY_NAME";
                ddlCitizen.DataBind();

                ddlCitizen.Items.Insert(0, "MALAYSIA");
            }
            catch (Exception ex)
            {
                string TempAudit = ex.ToString().Replace("'", "");
                GF_InsertAuditLog("-", "Data Check", "FrmEmployeeMaster", TempAudit, "populateCountry");
            }
            finally
            {
                Conn.Dispose();
                Conn.Close();
            }
        }


        private void populateBankCode()
        {

            SqlConnection Conn = new SqlConnection(CUBIC_HRMS.GlobalVariable.ConnectionString);
            GF_CheckConnectionStatus(Conn);
            Conn.Open();

            try
            {
                SqlDataAdapter da = new SqlDataAdapter("SELECT BANK_CODE, [BANK_CODE]+'--'+[BANK_NAME] AS [COMBINE]  FROM [dbo].[M_BANK_CODE] ORDER BY BANK_CODE ", Conn);
                DataTable dt = new DataTable();

                ddlBankCode.Items.Clear();
                ddlBankCode.SelectedValue = null;
                da.Fill(dt);
                ddlBankCode.DataSource = dt;
                ddlBankCode.DataBind();
                ddlBankCode.DataTextField = "COMBINE";
                ddlBankCode.DataValueField = "BANK_CODE";
                ddlBankCode.DataBind();
            }
            catch (Exception ex)
            {
                string TempAudit = ex.ToString().Replace("'", "");
                GF_InsertAuditLog("-", "Data Check", "FrmEmployeeMaster", TempAudit, "populateBankCode");
            }
            finally
            {
                Conn.Dispose();
                Conn.Close();
            }
        }


        private string F_GetBankName(string _TempBankCode)
        {
            string TempReturn = "-";
            SqlConnection Conn = new SqlConnection(CUBIC_HRMS.GlobalVariable.ConnectionString);
            GF_CheckConnectionStatus(Conn);
            Conn.Open();

            try
            {
                string SQLCommand = "SELECT BANK_NAME  ";
                SQLCommand = SQLCommand + "FROM [dbo].[M_BANK_CODE] ";
                SQLCommand = SQLCommand + "WHERE ";
                SQLCommand = SQLCommand + "BANK_CODE = '" + _TempBankCode + "' ";

                SqlCommand cmdDataBase = new SqlCommand(SQLCommand, Conn);
                SqlDataReader myReader;

                myReader = cmdDataBase.ExecuteReader();


                // ** if have then update
                // ** else insert
                if (myReader.HasRows)
                {
                    while (myReader.Read())
                    {
                        TempReturn = myReader["BANK_NAME"].ToString();
                    }
                }
                else
                {
                    TempReturn = "-";
                }
            }
            catch (Exception ex)
            {
                string TempAudit = ex.ToString().Replace("'", "");
                GF_InsertAuditLog("-", "Data Check", "FrmEmployeeMaster", TempAudit, "populateBankCode");
            }
            finally
            {
                Conn.Dispose();
                Conn.Close();
            }

            return TempReturn;
        }


        private void populateDepartment()
        {
            //String BU = ddlBU.Text;
            SqlConnection Conn = new SqlConnection(CUBIC_HRMS.GlobalVariable.ConnectionString);
            GF_CheckConnectionStatus(Conn);
            Conn.Open();
            string SQLCommand = "-";

            try
            {

                SQLCommand = "SELECT [DEPT_CODE], [DEPT_CODE]+'--'+[DEPT_NAME] AS [COMBINE] ";
                SQLCommand = SQLCommand + "FROM[dbo].[M_DEPARTMENT_TYPE] ";
                SQLCommand = SQLCommand + "WHERE  [DEPT_STATUS]='Y' ";
                //SQLCommand = SQLCommand + "AND DEPT_BU = '" + CUBIC_HRMS.GlobalVariable.G_User_BU + "' ";
                SQLCommand = SQLCommand + "ORDER BY [DEPT_CODE]";

                SqlDataAdapter adpt = new SqlDataAdapter(SQLCommand, Conn);
                DataTable dt = new DataTable();


                this.ddlDepartment.Items.Clear();
                this.ddlDepartment.SelectedValue = null;
                adpt.Fill(dt);
                ddlDepartment.DataSource = dt;
                ddlDepartment.DataBind();
                ddlDepartment.DataTextField = "COMBINE";
                ddlDepartment.DataValueField = "DEPT_CODE";
                ddlDepartment.DataBind();
                // Then add your first item
                ddlDepartment.Items.Insert(0, "- Select -");
            }
            catch (Exception ex)
            {
                string TempAudit = ex.ToString().Replace("'", "");
                GF_InsertAuditLog("-", "Data Check", "FrmEmployeeMaster", TempAudit, "populateDepartment");
            }
            finally
            {
                Conn.Dispose();
                Conn.Close();
            }
        }


        private void populatePosition()
        {
            SqlConnection Conn = new SqlConnection(CUBIC_HRMS.GlobalVariable.ConnectionString);
            GF_CheckConnectionStatus(Conn);
            Conn.Open();

            try
            {
                string SQLCommand = "-";
                SQLCommand = "SELECT DISTINCT [POS_TITLE] ";
                SQLCommand = SQLCommand + "FROM [dbo].[M_POSITION_TYPE] ";
                SQLCommand = SQLCommand + "WHERE  [POS_STATUS]='Y' ";
                //SQLCommand = SQLCommand + "AND POS_BU = '" + CUBIC_HRMS.GlobalVariable.G_User_BU + "' ";
                SQLCommand = SQLCommand + "ORDER BY [POS_TITLE]";

                SqlDataAdapter da = new SqlDataAdapter(SQLCommand, Conn);
                DataTable dt = new DataTable();

                this.ddlPositionTitle.Items.Clear();
                this.ddlPositionTitle.SelectedValue = null;
                da.Fill(dt);
                ddlPositionTitle.DataSource = dt;
                ddlPositionTitle.DataBind();
                ddlPositionTitle.DataTextField = "POS_TITLE";
                ddlPositionTitle.DataValueField = "POS_TITLE";
                ddlPositionTitle.DataBind();
                // Then add your first item
                ddlPositionTitle.Items.Insert(0, "- Select -");
            }
            catch (Exception ex)
            {
                string TempAudit = ex.ToString().Replace("'", "");
                GF_InsertAuditLog("-", "Data Check", "FrmEmployeeMaster", TempAudit, "populatePosition");
            }
            finally
            {
                Conn.Dispose();
                Conn.Close();
            }
        }





        public void MessageBoxShow(string AlarmMessage)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "My Alert", "alert('" + AlarmMessage + "');", true);
        }



        //***********Button for function*************//

        // ** Need check if already exist, then only can update
        // ** INSERT
        protected void btnNextPage1_Click(object sender, EventArgs e)
        {

            /* Check Records Exist in Wo*/
            SqlConnection Conn = new SqlConnection(CUBIC_HRMS.GlobalVariable.ConnectionString);
            GF_CheckConnectionStatus(Conn);
            Conn.Open();

            try
            {
                //Need check if got duplicate
                if (Page.IsValid)
                {
                    string SQLQuery = "SELECT EMP_CODE  ";
                    SQLQuery = SQLQuery + "FROM [dbo].[M_EMP_MASTER] ";
                    SQLQuery = SQLQuery + "WHERE ";
                    SQLQuery = SQLQuery + "EMP_CODE = '" + txtEmpCode.Text + "' ";

                    SqlCommand cmdDataBase = new SqlCommand(SQLQuery, Conn);
                    SqlDataReader myReader;

                    myReader = cmdDataBase.ExecuteReader();


                    // ** if have then update
                    // ** else insert
                    if (myReader.HasRows)
                    {
                        while (myReader.Read())
                        {
                            //Update into DB on first page
                            ValidationB4UpdateMasterInfo();
                        }
                        UploadImage();

                    }
                    else
                    {


                        //Start saving into database
                        F_NewSaveEmpInfoPage1();

                        UploadImage();
                    }


                    // ** upload got or not also go next page
                    //if (UploadAllPass == "False")
                    //{

                    //}
                    //else
                    //{
                    //    ////Open next view
                    MultiView1.ActiveViewIndex = 1;
                    LoadExisitingAddressInfo(txtEmpCode.Text);
                    //}
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Message", "alert('Error occured : " + ex.Message.ToString() + "');", true);
            }
            finally
            {
                Conn.Close();
                Conn.Dispose();
            }

        }


        protected void btnNextPage2_Click(object sender, EventArgs e)
        {

            /* Check Records Exist in Wo*/
            SqlConnection Conn = new SqlConnection(CUBIC_HRMS.GlobalVariable.ConnectionString);
            GF_CheckConnectionStatus(Conn);
            Conn.Open();

            try
            {
                if (Page.IsValid)
                {
                    string EmpCodetxt = txtEmpCode.Text.Trim();
                    string SelectEmpCodeddl = ddlSelectEmpCode.Text;

                    string SQLCommand = "select * from [dbo].[M_EMP_SUB] where [SUB_EMP_CODE]= '" + EmpCodetxt + "'";

                    SqlCommand cmdDataBase = new SqlCommand(SQLCommand, Conn);
                    SqlDataReader myReader;
                    myReader = cmdDataBase.ExecuteReader();
                    if (myReader.HasRows)
                    {
                        while (myReader.Read())
                        {
                            F_UpdateExistAddressInfo();
                        }
                    }
                    else
                    {
                        F_NewSaveEmpAddPage2();
                    }
                }
                MultiView1.ActiveViewIndex = 2;


                dtDateJoin.Text = DateTime.Now.ToLocalTime().ToString("yyyy-MM-dd");
                DTDatePayReview.Text = DateTime.Now.ToLocalTime().ToString("yyyy-MM-dd");
                dtDateResign.Text = DateTime.Now.ToLocalTime().ToString("yyyy-MM-dd");
                LoadExisitingPayrollInfo(txtEmpCode.Text);

            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Message", "alert('Error occured : " + ex.Message.ToString() + "');", true);
            }
            finally
            {
                Conn.Close();
                Conn.Dispose();
            }

        }



        protected void btnSummary_Click(object sender, EventArgs e)
        {

            /* Check Records Exist in Wo*/
            SqlConnection Conn = new SqlConnection(CUBIC_HRMS.GlobalVariable.ConnectionString);
            GF_CheckConnectionStatus(Conn);
            Conn.Open();

            try
            {
                if (Page.IsValid)
                {
                    //string SelectEmpCodeddl = ddlSelectEmpCode.Text;
                    string SelectEmpCodeddl = txtEmpCode.Text;
                    string SQLCommand = "select * from [dbo].[M_EMP_PAYROLL] where [PR_EMP_CODE]= '" + SelectEmpCodeddl + "'";

                    SqlCommand cmdDataBase = new SqlCommand(SQLCommand, Conn);
                    SqlDataReader myReader;
                    myReader = cmdDataBase.ExecuteReader();
                    if (myReader.HasRows)
                    {
                        while (myReader.Read())
                        {
                            F_UpdateExisPayrollInfo();
                        }
                    }
                    else
                    {
                        F_NewSaveEmpAddPage3();
                    }
                    MultiView1.ActiveViewIndex = 3;
                    LoadSummaryPage();
                }
            }

            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Message", "alert('Error occured : " + ex.Message.ToString() + "');", true);
            }
            finally
            {
                Conn.Close();
                Conn.Dispose();
            }
        }


        protected void btnBackPage1_Click(object sender, EventArgs e)
        {
            MultiView1.ActiveViewIndex = 0;

            LoadExisitingMasterInfo(txtEmpCode.Text);

            //if (ddlSelectEmpCode.Text == "Create New" &&  txtEmpCode.Text=="")
            if (txtEmpCode.Text == "")
            {
                EmpPImage.ImageUrl = "~/Image/DefaultProfile.png";
            }
            else
            {
                string _TempURL = "";
                string _ConvertReadableURL = "";

                trtxtEmpCode.Visible = true;

                //txtEmpCode.Text = ddlSelectEmpCode.Text;
                LoadExisitingMasterInfo(txtEmpCode.Text);
                _TempURL = LoadAnyImage(txtEmpCode.Text, "Employee Image");

                if (_TempURL == "")
                {
                    EmpPImage.ImageUrl = "~/Image/DefaultProfile.png";
                }
                else
                {
                    string[] DataToSplit = _TempURL.Split('\\');
                    _ConvertReadableURL = "~/" + DataToSplit[3] + "/" + DataToSplit[4];
                    EmpPImage.ImageUrl = _ConvertReadableURL;
                }

            }
        }


        protected void btnBackPage2_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                MultiView1.ActiveViewIndex = 1;

                LoadExisitingAddressInfo(txtEmpCode.Text);
            }

        }


        protected void btnBackPage3_Click(object sender, EventArgs e)
        {
            MultiView1.ActiveViewIndex = 2;

            LoadExisitingPayrollInfo(txtEmpCode.Text);
        }


        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            //Display success message and clear the form.
            string message = "Your details have been updated/added into database successfully. ";
            MessageBoxShow(message);

            MultiView1.ActiveViewIndex = 0;

            ClearAllText();
        }

        private void F_NewSaveEmpInfoPage1()
        {
            SqlConnection Conn = new SqlConnection(CUBIC_HRMS.GlobalVariable.ConnectionString);
            GF_CheckConnectionStatus(Conn);
            Conn.Open();

            //string BU = ddlBU.Text;
            string BU = CUBIC_HRMS.GlobalVariable.G_User_BU;
            string EmpCode = txtEmpCode.Text.Trim();
            string Department = ddlDepartment.Text;
            string FirstName = txtFirstName.Text.Trim();
            string LastName = txtLastName.Text.Trim();
            string NickName = txtNickName.Text.Trim();
            string Gender;

            if (rdGender.SelectedItem.ToString() == "Female")
            {
                Gender = "F";
            }
            else
            {
                Gender = "M";
            }

            string ICNo = txtICNo.Text.Trim();
            string DOB = dtDOB.Text;

            string Citiizen = ddlCitizen.Text.Trim();
            string EPFNo = txtEPFNo.Text.Trim();

            string IncomeTaxNo = txtIncomeTaxNo.Text.Trim();

            string SocsoNo = txtSocsoNo.Text.Trim();
            //string WPDueDate = dtWPermitEDate.Text;
            string EmpCategory = ddlEmpCategory.Text;
            //string EmpGrade = ddlEmpGrade.Text;
            string EmpEmail = txtEmpEmail.Text;
            string EmpContactNo = txtEmpContact.Text;

            //string IsLocal = ddlIsLocal.Text;
            string DirectSup = ddlSelectDSuperior.Text;
            string OnBehalf = ddlSelectOnBehalf.Text;
            string DirectSupName = txtDirectSuperiorName.Text;
            string OnBehalfName = txtOnBehalfName.Text;

            string AccessCard = txtAccessCard.Text;
            string EmContactPerson = txtEContPerson1.Text;
            string EMContactNo1 = txtECContactNo1.Text;
            //string EMContactNo2 = txtECContactNo2.Text;
            string PosTtitle = ddlPositionTitle.Text;


            //** All DDL if default - Select - , when save to database , all save - 
            if (EmpCategory == "- Select -")
            {
                EmpCategory = "-";
            }


            if (PosTtitle == "- Select -")
            {
                PosTtitle = "-";
            }

            if (DirectSup == "- Select -")
            {
                DirectSup = "-";
            }

            if (OnBehalf == "- Select -")
            {
                OnBehalf = "-";
            }



            string Status;

            //if (rbStatus.SelectedItem.ToString() == "Yes")
            //{
            //    Status = "Y";
            //}
            //else
            //{
            //    Status = "N";
            //}
            if (ddlStatus.Text == "Yes")
            {
                Status = "Y";
            }
            else
            {
                Status = "N";
            }


            try
            {
                string TempPrefix = "E";
                EmpCode = CUBIC_HRMS.GlobalVariable.GF_GetRunningNumber(CUBIC_HRMS.GlobalVariable.G_User_BU, TempPrefix);
                txtEmpCode.Text = EmpCode;
                string SQLInsertHeader = "INSERT INTO [M_EMP_MASTER]";
                SQLInsertHeader = SQLInsertHeader + "([EMP_BU]";
                SQLInsertHeader = SQLInsertHeader + ",[EMP_CODE]";
                SQLInsertHeader = SQLInsertHeader + ",[EMP_DEPT]";

                SQLInsertHeader = SQLInsertHeader + ",[EMP_FIRST_NAME] ";
                SQLInsertHeader = SQLInsertHeader + ",[EMP_LAST_NAME] ";
                SQLInsertHeader = SQLInsertHeader + ",[EMP_NICK_NAME] ";
                SQLInsertHeader = SQLInsertHeader + ",[EMP_IC_NO] ";
                SQLInsertHeader = SQLInsertHeader + ",[EMP_MAIL] ";
                SQLInsertHeader = SQLInsertHeader + ",[EMP_CONTACT_NO] ";
                SQLInsertHeader = SQLInsertHeader + ",[EMP_DOB] ";

                SQLInsertHeader = SQLInsertHeader + ",[EMP_GENDER] ";
                //SQLInsertHeader = SQLInsertHeader + ",[EMP_ISLOCAL] ";
                SQLInsertHeader = SQLInsertHeader + ",[EMP_CITIZEN] ";
                SQLInsertHeader = SQLInsertHeader + ",[EMP_EPF_NO] ";
                SQLInsertHeader = SQLInsertHeader + ",[EMP_INCOMETAX_NO]";
                SQLInsertHeader = SQLInsertHeader + ",[EMP_SOCSO_NO]";

                //SQLInsertHeader = SQLInsertHeader + ",[EMP_VISA_EXPIRED] ";
                //SQLInsertHeader = SQLInsertHeader + ",[EMP_WORK_PERMIT] ";
                //SQLInsertHeader = SQLInsertHeader + ",[EMP_WP_EXPIRED] ";
                SQLInsertHeader = SQLInsertHeader + ",[EMP_CATEGORY] ";
                //SQLInsertHeader = SQLInsertHeader + ",[EMP_GRADE] ";
                SQLInsertHeader = SQLInsertHeader + ",[EMP_POSITION_TITLE] ";

                SQLInsertHeader = SQLInsertHeader + ",[EMP_DIRECT_SUPERIOR] ";
                SQLInsertHeader = SQLInsertHeader + ",[EMP_APP_ONBEHALF] ";
                SQLInsertHeader = SQLInsertHeader + ",[EMP_DIRECT_SUPNAME] ";
                SQLInsertHeader = SQLInsertHeader + ",[EMP_APP_ONBEHALFNAME] ";

                SQLInsertHeader = SQLInsertHeader + ",[EMP_ACCESS_CARD]";
                SQLInsertHeader = SQLInsertHeader + ",[EMP_EMER_NAME1] ";
                SQLInsertHeader = SQLInsertHeader + ",[EMP_EMER_CONTACT1] ";
                //SQLInsertHeader = SQLInsertHeader + ",[EMP_EMER_NAME2]";
                //SQLInsertHeader = SQLInsertHeader + ",[EMP_EMER_CONTACT2]";
                SQLInsertHeader = SQLInsertHeader + ",[EMP_STATUS]";

                SQLInsertHeader = SQLInsertHeader + ",[EMP_CREATED_BY] ";
                SQLInsertHeader = SQLInsertHeader + ",[EMP_CREATED_DATE] ";
                SQLInsertHeader = SQLInsertHeader + ",[EMP_MODIFIED_BY]";
                SQLInsertHeader = SQLInsertHeader + ",[EMP_MODIFIED_DATE])";

                SQLInsertHeader = SQLInsertHeader + "VALUES ";
                SQLInsertHeader = SQLInsertHeader + "('" + BU + "' ";
                SQLInsertHeader = SQLInsertHeader + ",'" + EmpCode + "' ";

                SQLInsertHeader = SQLInsertHeader + ",'" + Department + "' ";

                SQLInsertHeader = SQLInsertHeader + ",'" + FirstName + "' ";
                SQLInsertHeader = SQLInsertHeader + ",'" + LastName + "' ";
                SQLInsertHeader = SQLInsertHeader + ",'" + NickName + "' ";
                SQLInsertHeader = SQLInsertHeader + ",'" + ICNo + "' ";
                SQLInsertHeader = SQLInsertHeader + ",'" + EmpEmail + "' ";
                SQLInsertHeader = SQLInsertHeader + ",'" + EmpContactNo + "' ";
                SQLInsertHeader = SQLInsertHeader + ",'" + DOB + "' ";

                SQLInsertHeader = SQLInsertHeader + ",'" + Gender + "' ";
                //SQLInsertHeader = SQLInsertHeader + ", '" + IsLocal + "' ";
                SQLInsertHeader = SQLInsertHeader + ",'" + Citiizen + "' ";
                SQLInsertHeader = SQLInsertHeader + ",'" + EPFNo + "' ";
                SQLInsertHeader = SQLInsertHeader + ",'" + IncomeTaxNo + "' ";
                SQLInsertHeader = SQLInsertHeader + ",'" + SocsoNo + "' ";

                //SQLInsertHeader = SQLInsertHeader + ",'" + VisaDueDate + "' ";
                //SQLInsertHeader = SQLInsertHeader + ",'" + WorkPermitType + "' ";
                //SQLInsertHeader = SQLInsertHeader + ",'" + WPDueDate + "' ";
                SQLInsertHeader = SQLInsertHeader + ",'" + EmpCategory + "' ";
                //SQLInsertHeader = SQLInsertHeader + ",'" + EmpGrade + "' ";

                SQLInsertHeader = SQLInsertHeader + ",'" + PosTtitle + "' ";

                SQLInsertHeader = SQLInsertHeader + ",'" + DirectSup + "' ";
                SQLInsertHeader = SQLInsertHeader + ",'" + OnBehalf + "' ";

                SQLInsertHeader = SQLInsertHeader + ",'" + DirectSupName + "' ";
                SQLInsertHeader = SQLInsertHeader + ",'" + OnBehalfName + "' ";

                SQLInsertHeader = SQLInsertHeader + ",'" + AccessCard + "' ";
                SQLInsertHeader = SQLInsertHeader + ",'" + EmContactPerson + "' ";
                SQLInsertHeader = SQLInsertHeader + ",'" + EMContactNo1 + "' ";
                //SQLInsertHeader = SQLInsertHeader + ",'" + EmContactPerson2 + "' ";
                //SQLInsertHeader = SQLInsertHeader + ",'" + EMContactNo2 + "' ";
                SQLInsertHeader = SQLInsertHeader + ",'" + Status + "' ";

                SQLInsertHeader = SQLInsertHeader + ",'" + CUBIC_HRMS.GlobalVariable.G_UserLogin + "' ";
                SQLInsertHeader = SQLInsertHeader + ",getdate() ";
                SQLInsertHeader = SQLInsertHeader + ",'" + CUBIC_HRMS.GlobalVariable.G_UserLogin + "' ";
                SQLInsertHeader = SQLInsertHeader + ",getdate() )";

                var cmd2 = new SqlCommand(SQLInsertHeader, Conn);
                cmd2.ExecuteNonQuery();


                F_InsertEmployeeLeaveDefault();



                CUBIC_HRMS.GlobalVariable.GF_UpdateRunningNumber(CUBIC_HRMS.GlobalVariable.G_User_BU, TempPrefix);


                // Display success message and clear the form.
                //string message = "Your details have been added into table below successfully.";
                //MessageBoxShow(message);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Message", "alert('Error occured : " + ex.Message.ToString() + "');", true);
            }
            finally
            {
                Conn.Close();
                Conn.Dispose();
            }
        }

        //Insert everytime when new employee submit all default diff leave

        private void F_InsertEmployeeLeaveDefault()
        {
            SqlConnection Conn = new SqlConnection(CUBIC_HRMS.GlobalVariable.ConnectionString);
            SqlConnection Conn1 = new SqlConnection(CUBIC_HRMS.GlobalVariable.ConnectionString);
            GF_CheckConnectionStatus(Conn);
            GF_CheckConnectionStatus(Conn1);
            Conn.Open();
            Conn1.Open();

            try
            {
                //string IsLocal = "Y";
                //if (ddlIsLocal.Text=="N")
                //{
                //    IsLocal = "Foreigner";
                //}
                //else
                //{
                //    IsLocal = "Local";
                //}

                string SQLCommand = "SELECT LEAVE_CODE, LEAVE_DAY  FROM [M_LEAVE_TYPE] ";
                //SQLCommand = SQLCommand + "WHERE LEAVE_BU = '" + CUBIC_HRMS.GlobalVariable.G_User_BU + "' ";
                //SQLCommand = SQLCommand + "WHERE (LEAVE_TYPE = '" + IsLocal + "' OR LEAVE_TYPE = 'All' )";

                SqlCommand cmdDataBase = new SqlCommand(SQLCommand, Conn);
                SqlDataReader myReader;

                myReader = cmdDataBase.ExecuteReader();
                if (myReader.HasRows)
                {
                    while (myReader.Read())
                    {

                        ////looping
                        string SQLCommand1 = "INSERT INTO [dbo].[T_EMP_LEAVE_BAL] ";
                        SQLCommand1 = SQLCommand1 + "( ";
                        SQLCommand1 = SQLCommand1 + "LEAVE_BU, ";
                        SQLCommand1 = SQLCommand1 + "EMP_CODE, ";
                        SQLCommand1 = SQLCommand1 + "[LEAVE_CODE] ,[LEAVE_DAY] ,[LEAVE_EXTRA],";
                        SQLCommand1 = SQLCommand1 + "[LEAVE_CREATE_BY],[LEAVE_CREATE_DATE]) ";
                        SQLCommand1 = SQLCommand1 + "VALUES ";
                        SQLCommand1 = SQLCommand1 + "(";
                        SQLCommand1 = SQLCommand1 + " '" + CUBIC_HRMS.GlobalVariable.G_User_BU + "','" + txtEmpCode.Text + "', '" + myReader["LEAVE_CODE"] + "', '" + myReader["LEAVE_DAY"] + "',0,";
                        //SQLCommand1 = SQLCommand1 + "'" + CUBIC_HRMS.GlobalVariable.G_User_BU + "', '" + txtEmpCode.Text + "', '" + myReader["LEAVE_CODE"] + "', '" + myReader["LEAVE_DAY"] + "',0,";
                        SQLCommand1 = SQLCommand1 + "'" + CUBIC_HRMS.GlobalVariable.G_UserLogin + "',GETDATE()) ";

                        var cmd2 = new SqlCommand(SQLCommand1, Conn1);
                        cmd2.ExecuteNonQuery();

                    }
                }
            }
            catch (Exception ex)
            {
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "Message", "alert('Error occured : " + ex.Message.ToString() + "');", true);
                string message = ex.Message.ToString();
                MessageBoxShow(message);
                GF_InsertAuditLog("-", "Catch Error", "FrmEmployeeMaster", "F_InsertEmployeeLeaveDefault", ex.ToString().Replace("'", ""));
            }
            finally
            {
                Conn.Close();
                Conn.Dispose();
                Conn1.Close();
                Conn1.Dispose();
            }
        }



        //Saving Address & Mailing Address 
        private void F_NewSaveEmpAddPage2()
        {
            SqlConnection Conn = new SqlConnection(CUBIC_HRMS.GlobalVariable.ConnectionString);
            GF_CheckConnectionStatus(Conn);
            Conn.Open();

            //**********Address Information**************//

            string Add1 = txtAddress1.Text.Trim();
            string Add2 = txtAddress2.Text.Trim();
            string Add3 = txtAddress3.Text.Trim();
            string Add4 = txtAddress4.Text.Trim();
            string PostCode = txtPostCode.Text.Trim();

            string State = txtState.Text.Trim();
            string AddContactNo1 = txtAddContactNo1.Text.Trim();
            //string AddContactNo2 = txtAddContactNo2.Text.Trim();
            string City = txtCity.Text.Trim();
            string Country1 = txtCountry.Text.Trim();
            //string AddEmailC1 = txtAddEmailC1.Text.Trim();

            //**********Mailing Address Information**************//

            string MAdd1 = txtMAddress1.Text.Trim();
            string MAdd2 = txtMAddress2.Text.Trim();
            string MAdd3 = txtMAddress3.Text.Trim();
            string MAdd4 = txtMAddress4.Text.Trim();
            string MPostCode = txtMPostCode.Text.Trim();

            string MState = txtMState.Text.Trim();
            string MAddContactNo1 = txtMContactNo1.Text.Trim();
            string MCity = txtMCity.Text.Trim();
            string MCountry = txtMCountry.Text.Trim();
            //string MEmail = txtMEmail.Text.Trim();

            DateTime Current = DateTime.Now;

            string chkSameAdd;
            if (chkSameAsAbove.Checked == true)
            {
                chkSameAdd = "Y";
            }
            else
            {
                chkSameAdd = "N";
            }


            try
            {
                string SQLInsertHeader = "INSERT INTO [M_EMP_SUB]";
                SQLInsertHeader = SQLInsertHeader + "( ";
                SQLInsertHeader = SQLInsertHeader + "[SUB_EMP_CODE]";
                SQLInsertHeader = SQLInsertHeader + ",[SUB_HM_ADD1]";
                SQLInsertHeader = SQLInsertHeader + ",[SUB_HM_ADD2]";
                SQLInsertHeader = SQLInsertHeader + ",[SUB_HM_ADD3] ";
                SQLInsertHeader = SQLInsertHeader + ",[SUB_HM_ADD4] ";
                SQLInsertHeader = SQLInsertHeader + ",[SUB_HM_POSTCODE] ";
                SQLInsertHeader = SQLInsertHeader + ",[SUB_HM_STATE] ";
                SQLInsertHeader = SQLInsertHeader + ",[SUB_HM_CONTACT_1] ";
                SQLInsertHeader = SQLInsertHeader + ",[SUB_HM_CITY] ";
                SQLInsertHeader = SQLInsertHeader + ",[SUB_HM_COUNTRY] ";
                SQLInsertHeader = SQLInsertHeader + ",[SUB_MAIL_ADD1] ";
                SQLInsertHeader = SQLInsertHeader + ",[SUB_MAIL_ADD2] ";
                SQLInsertHeader = SQLInsertHeader + ",[SUB_MAIL_ADD3] ";
                SQLInsertHeader = SQLInsertHeader + ",[SUB_MAIL_ADD4] ";
                SQLInsertHeader = SQLInsertHeader + ",[SUB_MAIL_POSTCODE]";
                SQLInsertHeader = SQLInsertHeader + ",[SUB_MAIL_STATE]";
                SQLInsertHeader = SQLInsertHeader + ",[SUB_MAIL_CITY] ";
                SQLInsertHeader = SQLInsertHeader + ",[SUB_MAIL_COUNTRY] ";
                SQLInsertHeader = SQLInsertHeader + ",[SUB_MAIL_CONTACT] ";
                SQLInsertHeader = SQLInsertHeader + ",[SUB_CHK_SAMEABVADD] ";
                SQLInsertHeader = SQLInsertHeader + ",[SUB_MODIFIED_DATE] ";
                SQLInsertHeader = SQLInsertHeader + ") ";
                SQLInsertHeader = SQLInsertHeader + "VALUES ";
                SQLInsertHeader = SQLInsertHeader + "( ";
                SQLInsertHeader = SQLInsertHeader + "'" + txtEmpCode.Text + "' ";
                SQLInsertHeader = SQLInsertHeader + ",'" + Add1 + "' ";
                SQLInsertHeader = SQLInsertHeader + ",'" + Add2 + "' ";
                SQLInsertHeader = SQLInsertHeader + ",'" + Add3 + "' ";
                SQLInsertHeader = SQLInsertHeader + ",'" + Add4 + "' ";
                SQLInsertHeader = SQLInsertHeader + ",'" + PostCode + "' ";
                SQLInsertHeader = SQLInsertHeader + ",'" + MState + "' ";
                SQLInsertHeader = SQLInsertHeader + ",'" + AddContactNo1 + "' ";
                SQLInsertHeader = SQLInsertHeader + ",'" + City + "' ";
                SQLInsertHeader = SQLInsertHeader + ",'" + Country1 + "' ";
                SQLInsertHeader = SQLInsertHeader + ",'" + MAdd1 + "' ";
                SQLInsertHeader = SQLInsertHeader + ",'" + MAdd2 + "' ";
                SQLInsertHeader = SQLInsertHeader + ",'" + MAdd3 + "' ";
                SQLInsertHeader = SQLInsertHeader + ",'" + MAdd4 + "' ";
                SQLInsertHeader = SQLInsertHeader + ",'" + MPostCode + "' ";
                SQLInsertHeader = SQLInsertHeader + ",'" + MState + "' ";
                SQLInsertHeader = SQLInsertHeader + ",'" + MCity + "' ";
                SQLInsertHeader = SQLInsertHeader + ",'" + MCountry + "' ";
                SQLInsertHeader = SQLInsertHeader + ",'" + MAddContactNo1 + "' ";
                SQLInsertHeader = SQLInsertHeader + ",'" + chkSameAdd + "' ";
                SQLInsertHeader = SQLInsertHeader + ",'" + Current + "' ";
                SQLInsertHeader = SQLInsertHeader + ") ";

                var cmd2 = new SqlCommand(SQLInsertHeader, Conn);
                cmd2.ExecuteNonQuery();

                // Display success message and clear the form.
                //string message = "Your details have been added into table below successfully.";
                //MessageBoxShow(message);
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Message", "alert('Error occured : " + ex.Message.ToString() + "');", true);
            }
            finally
            {
                Conn.Close();
                Conn.Dispose();
            }
        }


        //Saving Payroll
        private void F_NewSaveEmpAddPage3()
        {
            SqlConnection Conn = new SqlConnection(CUBIC_HRMS.GlobalVariable.ConnectionString);
            GF_CheckConnectionStatus(Conn);
            Conn.Open();

            //**********Payroll Information**************//

            string PaymentType = ddlPaymentType.Text;
            //string BankCode = ddlBankCode.Text.Trim();
            string BankName = txtBankName.Text.Trim();
            string AccountNo = txtAccountNo.Text.Trim();
            string EmpType = ddlEmpType.Text;
            string SalaryType = ddlSalaryType.Text.Trim();
            string SalaryWedges = txtSalaryWedges.Text.Trim();

            if (SalaryWedges == "" || SalaryWedges == null)
            {
                SalaryWedges = "0";
            }
            else
            {
                SalaryWedges = txtSalaryWedges.Text.Trim();
            }

            string SalaryCurrency = ddlSalaryCurrency.Text.Trim();


            string ShiftPreset = ddlShiftPreset.Text;

            string WorkingHour = txtWorkingHour.Text.Trim();
            string WorkingDay = txtWorkingDay.Text.Trim();
            //string IsResident = ddlResident.Text;

            string AllowanceWedges = txtAllowanceWedges.Text.Trim();

            if (AllowanceWedges == "" || AllowanceWedges == null)
            {
                AllowanceWedges = "0";
            }
            else
            {
                AllowanceWedges = txtAllowanceWedges.Text.Trim();
            }

            string AllowanceCurrency = ddlAllowanceCurrency.Text.Trim();
            string Date1stPayReview = DTDatePayReview.Text;
            string DateofJoin = dtDateJoin.Text;
            string DateofResign;

            string IsResign;

            if (rbIsResign.SelectedItem.ToString() == "Yes")
            {
                IsResign = "Y";
                DateofResign = dtDateResign.Text;

            }
            else
            {
                IsResign = "N";
                DateofResign = DateTime.Now.ToString();

            }

            string ResignType = ddlResignType.Text;
            string EntitleClaimOT;

            if (rbEClaimOT.SelectedItem.ToString() == "Yes")
            {
                EntitleClaimOT = "Y";
            }
            else
            {
                EntitleClaimOT = "N";
            }

            string AttTracking;

            //if (rbAttTracking.SelectedItem.ToString() == "Yes")
            //{
            //    AttTracking = "Y";
            //}
            //else
            //{
            //    AttTracking = "N";
            //}

            try
            {
                string SQLInsertHeader = "INSERT INTO [M_EMP_PAYROLL]";
                SQLInsertHeader = SQLInsertHeader + "([PR_EMP_CODE]";
                SQLInsertHeader = SQLInsertHeader + ",[PR_PAYMENT_TYPE]";
                //SQLInsertHeader = SQLInsertHeader + ",[PR_BANK_CODE]";
                SQLInsertHeader = SQLInsertHeader + ",[PR_BANK_NAME] ";
                SQLInsertHeader = SQLInsertHeader + ",[PR_ACC_NO] ";
                SQLInsertHeader = SQLInsertHeader + ",[PR_SALARY_TYPE] ";
                SQLInsertHeader = SQLInsertHeader + ",[PR_SALARY_WAGES] ";
                SQLInsertHeader = SQLInsertHeader + ",[PR_SALARY_CURRENCY] ";
                SQLInsertHeader = SQLInsertHeader + ",[PR_WORKINGHOUR] ";

                SQLInsertHeader = SQLInsertHeader + ",[PR_WORKINGDAY] ";
                //SQLInsertHeader = SQLInsertHeader + ",[PR_RESIDENT]";

                SQLInsertHeader = SQLInsertHeader + ",[PR_EMPLOYMENT_TYPE] ";
                SQLInsertHeader = SQLInsertHeader + ",[PR_ALLOWANCE_WAGES] ";
                SQLInsertHeader = SQLInsertHeader + ",[PR_ALLOWANCE_CURRENCY] ";
                SQLInsertHeader = SQLInsertHeader + ",[PR_PAYREVIEW_DATE] ";

                //Value from first page
                SQLInsertHeader = SQLInsertHeader + ",[PR_OT] ";
                //SQLInsertHeader = SQLInsertHeader + ",[PR_ATTENDANCE] ";

                SQLInsertHeader = SQLInsertHeader + ",[PR_JOIN_DATE] ";

                SQLInsertHeader = SQLInsertHeader + ",[PR_ISRESIGN] ";
                SQLInsertHeader = SQLInsertHeader + ",[PR_RESIGNTYPE] ";
                SQLInsertHeader = SQLInsertHeader + ",[PR_SHIFT_PRESET] ";

                SQLInsertHeader = SQLInsertHeader + ",[PR_RESIGN_DATE]) ";
                SQLInsertHeader = SQLInsertHeader + "VALUES ";
                SQLInsertHeader = SQLInsertHeader + "('" + txtEmpCode.Text + "' ";
                SQLInsertHeader = SQLInsertHeader + ",'" + PaymentType + "' ";
                //SQLInsertHeader = SQLInsertHeader + ",'" + BankCode + "' ";
                SQLInsertHeader = SQLInsertHeader + ",'" + BankName + "' ";
                SQLInsertHeader = SQLInsertHeader + ",'" + AccountNo + "' ";
                SQLInsertHeader = SQLInsertHeader + ",'" + SalaryType + "' ";
                SQLInsertHeader = SQLInsertHeader + ",'" + SalaryWedges + "' ";
                SQLInsertHeader = SQLInsertHeader + ",'" + SalaryCurrency + "' ";
                SQLInsertHeader = SQLInsertHeader + ",'" + WorkingHour + "' ";

                SQLInsertHeader = SQLInsertHeader + ",'" + WorkingDay + "' ";
                //SQLInsertHeader = SQLInsertHeader + ",'" + IsResident + "' ";

                SQLInsertHeader = SQLInsertHeader + ",'" + EmpType + "' ";
                SQLInsertHeader = SQLInsertHeader + ",'" + AllowanceWedges + "' ";
                SQLInsertHeader = SQLInsertHeader + ",'" + AllowanceCurrency + "' ";
                SQLInsertHeader = SQLInsertHeader + ",'" + Date1stPayReview + "' ";

                SQLInsertHeader = SQLInsertHeader + ",'" + EntitleClaimOT + "' ";
                //SQLInsertHeader = SQLInsertHeader + ",'" + AttTracking + "' ";

                SQLInsertHeader = SQLInsertHeader + ",'" + DateofJoin + "' ";
                SQLInsertHeader = SQLInsertHeader + ",'" + IsResign + "' ";
                SQLInsertHeader = SQLInsertHeader + ",'" + ResignType + "' ";
                SQLInsertHeader = SQLInsertHeader + ",'" + ShiftPreset + "' ";
                SQLInsertHeader = SQLInsertHeader + ",'" + DateofResign + "') ";

                var cmd2 = new SqlCommand(SQLInsertHeader, Conn);
                cmd2.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Message", "alert('Error occured : " + ex.Message.ToString() + "');", true);
            }
            finally
            {
                Conn.Close();
                Conn.Dispose();
            }
        }


        private void ValidationB4UpdateMasterInfo()
        {
            SqlConnection Conn = new SqlConnection(CUBIC_HRMS.GlobalVariable.ConnectionString);
            GF_CheckConnectionStatus(Conn);
            Conn.Open();

            try
            {
                if (Page.IsValid)
                {
                    F_UpdateExistEmpInfo();
                }
            }
            catch (Exception ex)
            {
                GF_InsertAuditLog("-", "Catch Error", "FrmEmployeeMaster", "ValidationB4UpdateMasterInfo", ex.ToString().Replace("'", ""));
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Message", "alert('Error occured : " + ex.Message.ToString() + "');", true);
            }
            finally
            {
                Conn.Close();
                Conn.Dispose();
            }
        }


        private void F_UpdateExistEmpInfo()
        {
            SqlConnection Conn = new SqlConnection(CUBIC_HRMS.GlobalVariable.ConnectionString);
            GF_CheckConnectionStatus(Conn);
            Conn.Open();

            //string BU = ddlBU.Text;
            string EmpCode = txtEmpCode.Text.Trim();
            string Department = ddlDepartment.Text;
            string FirstName = txtFirstName.Text.Trim();
            string LastName = txtLastName.Text.Trim();
            string NickName = txtNickName.Text.Trim();
            string Gender;

            if (rdGender.SelectedItem.ToString() == "Female")
            {
                Gender = "F";
            }
            else
            {
                Gender = "M";
            }

            string ICNo = txtICNo.Text.Trim();
            string EmpEmail = txtEmpEmail.Text;

            string EmpContactNo = txtEmpContact.Text;
            string DOB = dtDOB.Text;

            string Citiizen = ddlCitizen.Text.Trim();
            //string IsLocal = ddlIsLocal.Text;
            string IncomeTaxNo = txtIncomeTaxNo.Text.Trim();

            string EPFNo = txtEPFNo.Text.Trim();
            string SocsoNo = txtSocsoNo.Text.Trim();
            //string VisaDueDate = dtVisaEDate.Text;
            //string WorkPermitType = txtWorkPermitType.Text.Trim();
            //string WPDueDate = dtWPermitEDate.Text;
            string EmpCategory = ddlEmpCategory.Text;
            //string EmpGrade = ddlEmpGrade.Text;

            string PosTtitle = ddlPositionTitle.Text;

            string DirectSup = ddlSelectDSuperior.Text;
            string OnBehalf = ddlSelectOnBehalf.Text;
            string DirectSupName = txtDirectSuperiorName.Text;
            string OnBehalfName = txtOnBehalfName.Text;

            string AccessCard = txtAccessCard.Text;
            string EmContactPerson = txtEContPerson1.Text;
            string EMContactNo1 = txtECContactNo1.Text;
            //string EMContactNo2 = txtECContactNo2.Text;
            string Status;



            //** All DDL if default - Select - , when save to database , all save - 
            if (EmpCategory == "- Select -")
            {
                EmpCategory = "-";
            }

            if (PosTtitle == "- Select -")
            {
                PosTtitle = "-";
            }

            if (DirectSup == "- Select -")
            {
                DirectSup = "-";
            }

            if (OnBehalf == "- Select -")
            {
                OnBehalf = "-";
            }

            if (ddlStatus.Text == "Yes")
            {
                Status = "Y";
            }
            else
            {
                Status = "N";
            }

            string createdBy = CUBIC_HRMS.GlobalVariable.G_UserLogin;


            try
            {
                // If Wo not exist then just insert into the purchasing Header.
                string SQLUpdateHeader = "UPDATE [dbo].[M_EMP_MASTER] ";
                SQLUpdateHeader = SQLUpdateHeader + "SET";
                //SQLUpdateHeader = SQLUpdateHeader + "[EMP_BU] = '" + BU + "', ";
                SQLUpdateHeader = SQLUpdateHeader + "[EMP_DEPT] = '" + Department + "', ";
                SQLUpdateHeader = SQLUpdateHeader + "[EMP_FIRST_NAME] = '" + FirstName + "', ";
                SQLUpdateHeader = SQLUpdateHeader + "[EMP_LAST_NAME] = '" + LastName + "', ";
                SQLUpdateHeader = SQLUpdateHeader + "[EMP_NICK_NAME] = '" + NickName + "', ";

                SQLUpdateHeader = SQLUpdateHeader + "[EMP_GENDER] = '" + Gender + "', ";
                SQLUpdateHeader = SQLUpdateHeader + "[EMP_DOB] = '" + DOB + "', ";
                SQLUpdateHeader = SQLUpdateHeader + "[EMP_IC_NO] = '" + ICNo + "', ";
                SQLUpdateHeader = SQLUpdateHeader + "[EMP_MAIL] = '" + EmpEmail + "', ";
                SQLUpdateHeader = SQLUpdateHeader + "[EMP_CONTACT_NO] = '" + EmpContactNo + "', ";
                SQLUpdateHeader = SQLUpdateHeader + "[EMP_CITIZEN] = '" + Citiizen + "', ";
                //SQLUpdateHeader = SQLUpdateHeader + "[EMP_ISLOCAL] = '" + IsLocal + "', ";

                SQLUpdateHeader = SQLUpdateHeader + "[EMP_INCOMETAX_NO] = '" + IncomeTaxNo + "', ";

                SQLUpdateHeader = SQLUpdateHeader + "[EMP_EPF_NO] = '" + EPFNo + "', ";
                SQLUpdateHeader = SQLUpdateHeader + "[EMP_SOCSO_NO] = '" + SocsoNo + "', ";
                //SQLUpdateHeader = SQLUpdateHeader + "[EMP_VISA_EXPIRED] = '" + VisaDueDate + "', ";
                //SQLUpdateHeader = SQLUpdateHeader + "[EMP_WORK_PERMIT] = '" + WorkPermitType + "', ";
                //SQLUpdateHeader = SQLUpdateHeader + "[EMP_WP_EXPIRED] = '" + WPDueDate + "', ";

                SQLUpdateHeader = SQLUpdateHeader + "[EMP_ACCESS_CARD] = '" + AccessCard + "', ";
                SQLUpdateHeader = SQLUpdateHeader + "[EMP_CATEGORY] = '" + EmpCategory + "', ";
                //SQLUpdateHeader = SQLUpdateHeader + "[EMP_GRADE] = '" + EmpGrade + "', ";
                SQLUpdateHeader = SQLUpdateHeader + "[EMP_POSITION_TITLE] = '" + PosTtitle + "', ";

                SQLUpdateHeader = SQLUpdateHeader + "[EMP_DIRECT_SUPERIOR] = '" + DirectSup + "', ";
                SQLUpdateHeader = SQLUpdateHeader + "[EMP_APP_ONBEHALF] = '" + OnBehalf + "', ";

                SQLUpdateHeader = SQLUpdateHeader + "[EMP_DIRECT_SUPNAME]= '" + DirectSupName + "', ";
                SQLUpdateHeader = SQLUpdateHeader + "[EMP_APP_ONBEHALFNAME] = '" + OnBehalfName + "', ";

                SQLUpdateHeader = SQLUpdateHeader + "[EMP_EMER_NAME1] = '" + EmContactPerson + "', ";
                SQLUpdateHeader = SQLUpdateHeader + "[EMP_EMER_CONTACT1] = '" + EMContactNo1 + "', ";
                SQLUpdateHeader = SQLUpdateHeader + "[EMP_STATUS] = '" + Status + "', ";

                SQLUpdateHeader = SQLUpdateHeader + "[EMP_MODIFIED_BY] = '" + CUBIC_HRMS.GlobalVariable.G_UserLogin + "', ";
                SQLUpdateHeader = SQLUpdateHeader + "[EMP_MODIFIED_DATE] = getdate() ";
                SQLUpdateHeader = SQLUpdateHeader + "WHERE EMP_CODE = '" + txtEmpCode.Text + "' ";

                var cmd2 = new SqlCommand(SQLUpdateHeader, Conn);
                cmd2.ExecuteNonQuery();

                // NEED CHECK LEAVE TABLE ALSO, IF ALREADY HAVE 1 RECORD, DONT DO ANYTHING, IF TOTALLY DONT HAVE, THEN NEED INSERT.

                SqlConnection Conn1 = new SqlConnection(CUBIC_HRMS.GlobalVariable.ConnectionString);
                GF_CheckConnectionStatus(Conn1);
                Conn1.Open();

                string SQLCommand1 = " SELECT* FROM T_EMP_LEAVE_BAL ";
                SQLCommand1 = SQLCommand1 + "WHERE ";
                //SQLCommand1 = SQLCommand1 + "LEAVE_BU = '" + CUBIC_HRMS.GlobalVariable.G_User_BU + "' ";
                //SQLCommand1 = SQLCommand1 + "AND EMP_CODE = '"+ txtEmpCode.Text +"' ";
                SQLCommand1 = SQLCommand1 + " EMP_CODE = '" + txtEmpCode.Text + "' ";

                SqlCommand cmdDataBase1 = new SqlCommand(SQLCommand1, Conn);
                SqlDataReader myReader1;
                myReader1 = cmdDataBase1.ExecuteReader();

                if (myReader1.HasRows)
                {
                    // ** if already have data, then dont do anything
                }
                else
                {
                    // ** if nothing, then do insert
                    F_InsertEmployeeLeaveDefault();
                }
                Conn1.Close();
                Conn1.Dispose();
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Message", "alert('Error occured : " + ex.Message.ToString() + "');", true);
                GF_InsertAuditLog("-", "Catch Error", "FrmEmployeeMaster", "-", ex.ToString().Replace("'", ""));
            }
            finally
            {
                Conn.Close();
                Conn.Dispose();
            }
        }

        private void F_UpdateEmpStatus(string _EmpCode)
        {
            SqlConnection Conn = new SqlConnection(CUBIC_HRMS.GlobalVariable.ConnectionString);
            GF_CheckConnectionStatus(Conn);
            Conn.Open();


            string Status = "N";

            try
            {
                // If Wo not exist then just insert into the purchasing Header.
                string SQLUpdateHeader = "UPDATE [dbo].[M_EMP_MASTER] ";
                SQLUpdateHeader = SQLUpdateHeader + "SET";
                SQLUpdateHeader = SQLUpdateHeader + "[EMP_STATUS] = '" + Status + "' ";
                SQLUpdateHeader = SQLUpdateHeader + "WHERE EMP_CODE = '" + txtEmpCode.Text + "' ";

                var cmd2 = new SqlCommand(SQLUpdateHeader, Conn);
                cmd2.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Message", "alert('Error occured : " + ex.Message.ToString() + "');", true);
                GF_InsertAuditLog("-", "Catch Error", "FrmEmployeeMaster", "-", ex.ToString().Replace("'", ""));
            }
            finally
            {
                Conn.Close();
                Conn.Dispose();
            }
        }


        private void F_UpdateExistAddressInfo()
        {
            SqlConnection Conn = new SqlConnection(CUBIC_HRMS.GlobalVariable.ConnectionString);
            GF_CheckConnectionStatus(Conn);
            Conn.Open();

            //**********Address Information**************//

            string Add1 = txtAddress1.Text.Trim();
            string Add2 = txtAddress2.Text.Trim();
            string Add3 = txtAddress3.Text.Trim();
            string Add4 = txtAddress4.Text.Trim();
            string PostCode = txtPostCode.Text.Trim();

            string State = txtState.Text.Trim();
            string AddContactNo1 = txtAddContactNo1.Text.Trim();
            //string AddContactNo2 = txtAddContactNo2.Text.Trim();
            string City = txtCity.Text.Trim();
            string Country1 = txtCountry.Text.Trim();
            //string AddEmailC1 = txtAddEmailC1.Text.Trim();


            //**********Mailing Address Information**************//

            string MAdd1 = txtMAddress1.Text.Trim();
            string MAdd2 = txtMAddress2.Text.Trim();
            string MAdd3 = txtMAddress3.Text.Trim();
            string MAdd4 = txtMAddress4.Text.Trim();
            string MPostCode = txtMPostCode.Text.Trim();

            string MState = txtMState.Text.Trim();
            string MAddContactNo1 = txtMContactNo1.Text.Trim();
            string MCity = txtMCity.Text.Trim();
            string MCountry = txtMCountry.Text.Trim();
            //string MEmail = txtMEmail.Text.Trim();

            string chkSameAdd;
            if (chkSameAsAbove.Checked == true)
            {
                chkSameAdd = "Y";
            }
            else
            {
                chkSameAdd = "N";
            }

            DateTime CurrentTime = DateTime.Now;
            //string createdBy = CUBIC_HRMS.GlobalVariable.G_UserLogin;

            try
            {

                string SQLUpdateHeader = "UPDATE [dbo].[M_EMP_SUB] ";
                SQLUpdateHeader = SQLUpdateHeader + "SET";
                SQLUpdateHeader = SQLUpdateHeader + "[SUB_HM_ADD1] = '" + Add1 + "', ";
                SQLUpdateHeader = SQLUpdateHeader + "[SUB_HM_ADD2] = '" + Add2 + "', ";
                SQLUpdateHeader = SQLUpdateHeader + "[SUB_HM_ADD3] = '" + Add3 + "', ";
                SQLUpdateHeader = SQLUpdateHeader + "[SUB_HM_ADD4] = '" + Add4 + "', ";
                SQLUpdateHeader = SQLUpdateHeader + "[SUB_HM_POSTCODE] = '" + PostCode + "', ";

                SQLUpdateHeader = SQLUpdateHeader + "[SUB_HM_STATE] = '" + State + "', ";
                SQLUpdateHeader = SQLUpdateHeader + "[SUB_HM_CITY] = '" + City + "', ";
                SQLUpdateHeader = SQLUpdateHeader + "[SUB_HM_COUNTRY] = '" + Country1 + "', ";
                SQLUpdateHeader = SQLUpdateHeader + "[SUB_HM_CONTACT_1] = '" + AddContactNo1 + "', ";

                SQLUpdateHeader = SQLUpdateHeader + "[SUB_MAIL_ADD1] = '" + MAdd1 + "', ";
                SQLUpdateHeader = SQLUpdateHeader + "[SUB_MAIL_ADD2] = '" + MAdd2 + "', ";
                SQLUpdateHeader = SQLUpdateHeader + "[SUB_MAIL_ADD3] = '" + MAdd3 + "', ";
                SQLUpdateHeader = SQLUpdateHeader + "[SUB_MAIL_ADD4] = '" + MAdd4 + "', ";

                SQLUpdateHeader = SQLUpdateHeader + "[SUB_MAIL_POSTCODE] = '" + MPostCode + "', ";
                SQLUpdateHeader = SQLUpdateHeader + "[SUB_MAIL_STATE] = '" + MState + "', ";
                SQLUpdateHeader = SQLUpdateHeader + "[SUB_MAIL_CITY] = '" + MCity + "', ";
                SQLUpdateHeader = SQLUpdateHeader + "[SUB_MAIL_COUNTRY] = '" + MCountry + "', ";
                SQLUpdateHeader = SQLUpdateHeader + "[SUB_CHK_SAMEABVADD] = '" + chkSameAdd + "', ";
                SQLUpdateHeader = SQLUpdateHeader + "[SUB_MODIFIED_DATE] ='" + CurrentTime + "', ";

                SQLUpdateHeader = SQLUpdateHeader + "[SUB_MAIL_CONTACT] = '" + MAddContactNo1 + "'";
                SQLUpdateHeader = SQLUpdateHeader + "WHERE [SUB_EMP_CODE] = '" + txtEmpCode.Text + "' ";

                var cmd2 = new SqlCommand(SQLUpdateHeader, Conn);
                cmd2.ExecuteNonQuery();

                //// Display success message and clear the form.
                //string message = "Your details have been updated into table below successfully.";
                //MessageBoxShow(message);

            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Message", "alert('Error occured : " + ex.Message.ToString() + "');", true);
            }
            finally
            {
                Conn.Close();
                Conn.Dispose();
            }
        }


        private void F_UpdateExisPayrollInfo()
        {
            SqlConnection Conn = new SqlConnection(CUBIC_HRMS.GlobalVariable.ConnectionString);
            GF_CheckConnectionStatus(Conn);
            Conn.Open();

            //**********Payroll Information**************//

            string PaymentType = ddlPaymentType.Text;
            //string BankCode = ddlBankCode.Text.Trim();
            string BankName = txtBankName.Text.Trim();
            string AccountNo = txtAccountNo.Text.Trim();
            string EmpType = ddlEmpType.Text;
            string SalaryType = ddlSalaryType.Text.Trim();
            string SalaryWedges = txtSalaryWedges.Text.Trim();
            string SalaryCurrency = ddlSalaryCurrency.Text.Trim();
            string AllowanceWedges = txtAllowanceWedges.Text.Trim();
            string AllowanceCurrency = ddlAllowanceCurrency.Text.Trim();
            string WorkingHour = txtWorkingHour.Text.Trim();


            string ShiftPreset = ddlShiftPreset.Text;
            string WorkingDay = txtWorkingDay.Text.Trim();
            //string IsResident = ddlResident.Text;


            string Date1stPayReview = DTDatePayReview.Text;
            string DateofJoin = dtDateJoin.Text;
            string ResignType = ddlResignType.Text;
            string DateofResign;
            string IsResign;

            if (rbIsResign.SelectedItem.ToString() == "Yes")
            {
                IsResign = "Y";
                DateofResign = dtDateResign.Text;
            }
            else
            {
                IsResign = "N";
                DateofResign = Convert.ToString(DateTime.Now);
            }

            string EntitleClaimOT;

            if (rbEClaimOT.SelectedItem.ToString() == "Yes")
            {
                EntitleClaimOT = "Y";
            }
            else
            {
                EntitleClaimOT = "N";
            }

            string AttTracking;

            //if (rbAttTracking.SelectedItem.ToString() == "Yes")
            //{
            //    AttTracking = "Y";
            //}
            //else
            //{
            //    AttTracking = "N";
            //}
            DateTime CurrentTime = DateTime.Now;
            //string createdBy = CUBIC_HRMS.GlobalVariable.G_UserLogin;

            try
            {

                string SQLUpdateHeader = "UPDATE [dbo].[M_EMP_PAYROLL] ";
                SQLUpdateHeader = SQLUpdateHeader + "SET";
                SQLUpdateHeader = SQLUpdateHeader + "[PR_PAYMENT_TYPE] = '" + PaymentType + "', ";
                SQLUpdateHeader = SQLUpdateHeader + "[PR_ACC_NO] = '" + AccountNo + "', ";
                SQLUpdateHeader = SQLUpdateHeader + "[PR_BANK_NAME] = '" + BankName + "', ";
                SQLUpdateHeader = SQLUpdateHeader + "[PR_SALARY_WAGES] = '" + SalaryWedges + "', ";

                SQLUpdateHeader = SQLUpdateHeader + "[PR_SALARY_CURRENCY] = '" + SalaryCurrency + "', ";
                SQLUpdateHeader = SQLUpdateHeader + "[PR_WORKINGHOUR] = '" + WorkingHour + "', ";

                SQLUpdateHeader = SQLUpdateHeader + "[PR_EMPLOYMENT_TYPE] = '" + EmpType + "', ";
                SQLUpdateHeader = SQLUpdateHeader + "[PR_SALARY_TYPE] = '" + SalaryType + "', ";
                SQLUpdateHeader = SQLUpdateHeader + "[PR_PAYREVIEW_DATE] = '" + Date1stPayReview + "', ";
                SQLUpdateHeader = SQLUpdateHeader + "[PR_OT] = '" + EntitleClaimOT + "', ";
                SQLUpdateHeader = SQLUpdateHeader + "[PR_ISRESIGN] = '" + IsResign + "', ";

                SQLUpdateHeader = SQLUpdateHeader + "[PR_SHIFT_PRESET] = '" + ShiftPreset + "', ";


                //SQLUpdateHeader = SQLUpdateHeader + "[PR_RESIDENT] = '" + IsResident + "', ";
                SQLUpdateHeader = SQLUpdateHeader + "[PR_WORKINGDAY] = '" + WorkingDay + "', ";

                //SQLUpdateHeader = SQLUpdateHeader + "[PR_ATTENDANCE] = '" + AttTracking + "', ";
                SQLUpdateHeader = SQLUpdateHeader + "[PR_JOIN_DATE] = '" + DateofJoin + "', ";

                SQLUpdateHeader = SQLUpdateHeader + "[PR_RESIGNTYPE] = '" + ResignType + "', ";
                SQLUpdateHeader = SQLUpdateHeader + "[PR_RESIGN_DATE] = '" + DateofResign + "', ";
                SQLUpdateHeader = SQLUpdateHeader + "[PR_ALLOWANCE_WAGES] = '" + AllowanceWedges + "', ";
                SQLUpdateHeader = SQLUpdateHeader + "[PR_ALLOWANCE_CURRENCY] = '" + AllowanceCurrency + "', ";
                SQLUpdateHeader = SQLUpdateHeader + "[PR_MODIFIED_DATE] ='" + CurrentTime + "'";

                SQLUpdateHeader = SQLUpdateHeader + "WHERE [PR_EMP_CODE] = '" + txtEmpCode.Text + "' ";

                var cmd2 = new SqlCommand(SQLUpdateHeader, Conn);
                cmd2.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                GF_InsertAuditLog("-", "Catch Error", "FrmEmployeeMaster", "-", ex.ToString().Replace("'", ""));
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Message", "alert('Error occured : " + ex.Message.ToString() + "');", true);
            }
            finally
            {
                Conn.Close();
                Conn.Dispose();
            }
        }


        protected void chkSameAsAbove_CheckedChanged(object sender, EventArgs e)
        {
            if (chkSameAsAbove.Checked == true)
            {

                txtMAddress1.Text = txtAddress1.Text.Trim();
                txtMAddress2.Text = txtAddress2.Text.Trim();
                txtMAddress3.Text = txtAddress3.Text.Trim();
                txtMAddress4.Text = txtAddress4.Text.Trim();
                txtMPostCode.Text = txtPostCode.Text.Trim();

                txtMState.Text = txtState.Text.Trim();
                txtMContactNo1.Text = txtAddContactNo1.Text.Trim();
                txtMCity.Text = txtCity.Text.Trim();
                txtMCountry.Text = txtCountry.Text.Trim();
                //txtMEmail.Text = txtAddEmailC1.Text.Trim();

                txtMAddress1.ReadOnly = true;
                txtMAddress2.ReadOnly = true;
                txtMAddress3.ReadOnly = true;
                txtMAddress4.ReadOnly = true;
                txtMPostCode.ReadOnly = true;

                txtMState.ReadOnly = true;
                txtMContactNo1.ReadOnly = true;
                txtMCity.ReadOnly = true;
                txtMCountry.ReadOnly = true;
                //txtMEmail.ReadOnly = true;

            }
            else
            {
                txtMAddress1.Text = "";
                txtMAddress2.Text = "";
                txtMAddress3.Text = "";
                txtMAddress4.Text = "";
                txtMPostCode.Text = "";

                txtMState.Text = "";
                txtMContactNo1.Text = "";
                txtMCity.Text = "";
                txtMCountry.Text = "";
                //txtMEmail.Text = "";

                txtMAddress1.ReadOnly = false;
                txtMAddress2.ReadOnly = false;
                txtMAddress3.ReadOnly = false;
                txtMAddress4.ReadOnly = false;
                txtMPostCode.ReadOnly = false;

                txtMState.ReadOnly = false;
                txtMContactNo1.ReadOnly = false;
                txtMCity.ReadOnly = false;
                txtMCountry.ReadOnly = false;
                //txtMEmail.ReadOnly = false;
            }
        }


        protected void UploadImage()
        {

            string filetype1 = "Employee Image";
            string filetype2 = "Employee Offer Letter";
            string filetype3 = "Employee Resume";
            string filetype4 = "Employee Information";
            int UploadStatus = 0; ////if pass = 1, if fail first 1 = 91, if fail 2 = 92, if fail 3 = 93

            //upload cannot pun in panel
            //string FolderPath = Server.MapPath("~/Files/");
            string FolderPath = G_AttFilePath;
            //Check whether Directory (Folder) exists.
            if (!Directory.Exists(FolderPath))
            {
                //If Directory (Folder) does not exists. Create it.
                Directory.CreateDirectory(FolderPath);
            }

            ////delete existing file
            if (File.Exists(FolderPath + FileUploadAtt.FileName))
            {
                File.Delete(FolderPath + FileUploadAtt.FileName);
            }

            if (File.Exists(FolderPath + FileUploadOfferLetter.FileName))
            {
                File.Delete(FolderPath + FileUploadOfferLetter.FileName);
            }
            if (File.Exists(FolderPath + FileUploadResumeDoc.FileName))
            {
                File.Delete(FolderPath + FileUploadResumeDoc.FileName);
            }

            if (File.Exists(FolderPath + FileUploadEmpInfoDoc.FileName))
            {
                File.Delete(FolderPath + FileUploadEmpInfoDoc.FileName);
            }

            if (FileUploadAtt.HasFile)
            {
                try
                {
                    if (FileUploadAtt.PostedFile.ContentType.ToLower() == "image/jpeg")
                    {

                        //string filename = Path.GetFileName(FileUploadAtt.FileName);
                        string filename = "ProfilePicture";
                        FileUploadAtt.SaveAs(FolderPath + filename);

                        //upload to database
                        UpdateUploadAttData(1, txtEmpCode.Text, FolderPath + filename, filename, filetype1);
                        LoadAnyImage(txtEmpCode.Text, "Employee Image");

                        UploadAllPass = "True";
                        UploadStatus = 1;
                    }
                    else
                    {
                        UploadAllPass = "False";
                        UploadStatus = 91;
                    }
                }
                catch (Exception ex)
                {
                    MessageBoxShow("Upload status: The file could not be uploaded. The following error occured: ");
                }
            }


            //92 Offer Letter
            if (FileUploadOfferLetter.HasFile)
            {
                try
                {
                    //  if (FileUploadWrkPermit.PostedFile.ContentType.ToLower() == "application/pdf" || FileUploadWrkPermit.PostedFile.ContentType.ToLower() == "image/jpeg")
                    if (FileUploadOfferLetter.PostedFile.ContentType.ToLower() == "application/pdf")
                    {

                        string filename = Path.GetFileName(FileUploadOfferLetter.FileName);
                        FileUploadOfferLetter.SaveAs(FolderPath + filename);

                        //upload to database
                        UpdateUploadAttData(1, txtEmpCode.Text, FolderPath + filename, filename, filetype2);
                        LoadOfferLetterLink(txtEmpCode.Text, "Employee Offer Letter");

                        if (UploadStatus != 1)
                        {

                        }
                        else
                        {
                            UploadAllPass = "True";
                            UploadStatus = 1;
                        }


                    }
                    else
                    {
                        UploadAllPass = "False";
                        UploadStatus = 92;
                    }
                }
                catch (Exception ex)
                {
                    MessageBoxShow("Upload status: The file could not be uploaded. The following error occured: ");
                }
            }

            //93 Resume
            if (FileUploadResumeDoc.HasFile)
            {
                try
                {
                    if (FileUploadResumeDoc.PostedFile.ContentType.ToLower() == "application/pdf")
                    {

                        string filename = Path.GetFileName(FileUploadResumeDoc.FileName);
                        FileUploadResumeDoc.SaveAs(FolderPath + filename);

                        //upload to database
                        UpdateUploadAttData(1, txtEmpCode.Text, FolderPath + filename, filename, filetype3);
                        LoadResumeLink(txtEmpCode.Text, "Employee Resume");

                        if (UploadStatus != 1)
                        {

                        }
                        else
                        {
                            UploadAllPass = "True";
                            UploadStatus = 1;
                        }

                    }
                    else
                    {
                        UploadAllPass = "False";
                        UploadStatus = 93;
                    }
                }
                catch (Exception ex)
                {
                    MessageBoxShow("Upload status: The file could not be uploaded. The following error occured: ");
                }
            }


            //94 Employee Information
            if (FileUploadEmpInfoDoc.HasFile)
            {
                try
                {
                    if (FileUploadEmpInfoDoc.PostedFile.ContentType.ToLower() == "application/pdf")
                    {

                        string filename = Path.GetFileName(FileUploadEmpInfoDoc.FileName);
                        FileUploadEmpInfoDoc.SaveAs(FolderPath + filename);

                        //upload to database
                        UpdateUploadAttData(1, txtEmpCode.Text, FolderPath + filename, filename, filetype4);
                        LoadEmpInfLink(txtEmpCode.Text, "Employee Information");

                        if (UploadStatus != 1)
                        {

                        }
                        else
                        {
                            UploadAllPass = "True";
                            UploadStatus = 1;
                        }

                    }
                    else
                    {
                        UploadAllPass = "False";
                        UploadStatus = 94;
                    }
                }
                catch (Exception ex)
                {
                    MessageBoxShow("Upload status: The file could not be uploaded. The following error occured: ");
                }
            }



            if (UploadAllPass == "False")
            {
                if (UploadStatus == 91)
                {
                    MessageBoxShow("Upload status: Only Jpeg files are accepted! Please re-select your employee photo in JPEG File.");

                }
                else if (UploadStatus == 92)
                {
                    MessageBoxShow("Upload status: Only pdf  files are accepted! Please re-select your Offer Letter in pdf File.");
                }
                else if (UploadStatus == 93)
                {
                    MessageBoxShow("Upload status: Only pdf  files are accepted! Please re-select your Resume in pdf File.");
                }
                else if (UploadStatus == 94)
                {
                    MessageBoxShow("Upload status: Only pdf  files are accepted! Please re-select your Employee Information in pdf File.");
                }
                else if (UploadStatus == 0)
                {
                    UploadAllPass = "True";
                }
                else
                {
                    MessageBoxShow("Upload status: Only pdf  files are accepted!");
                }
            }
            else
            {
                UploadAllPass = "True";
                MessageBoxShow("Upload Successful: File uploaded!");
            }
        }

        //Resume
        private string LoadResumeLink(string _TempEmpCode, string _FileType)
        {
            ////Load Out Image base on FILE TYPE  pass in. in database name : ATT_TYPE
            string TempEmpCode = _TempEmpCode;
            string FileType = _FileType;
            //string _TempImageURL = "";
            string _TempImageRemark = "";

            SqlConnection Conn = new SqlConnection(CUBIC_HRMS.GlobalVariable.ConnectionString);
            GF_CheckConnectionStatus(Conn);
            Conn.Open();

            try
            {
                string SQLCommand = "SELECT [ATT_FILE_REMARK] FROM T_EMP_ATT WHERE ATT_EMP_CODE = '" + TempEmpCode + "' AND ATT_TYPE = '" + FileType + "' ";
                SqlCommand cmdDataBase = new SqlCommand(SQLCommand, Conn);
                SqlDataReader myReader;
                myReader = cmdDataBase.ExecuteReader();

                if (myReader.HasRows)
                {
                    while (myReader.Read())
                    {
                        //_TempImageURL = myReader["ATT_FILE_PATH"].ToString();
                        _TempImageRemark = myReader["ATT_FILE_REMARK"].ToString();
                        LBtnResumeDoc.Text = _TempImageRemark;
                        LBtnSVResume.Text = _TempImageRemark;
                    }
                }
                else
                {
                    //_TempImageURL = "";
                    _TempImageRemark = "";
                }
            }
            catch (Exception ex)
            {
                string message = ex.Message.ToString();
                MessageBoxShow(message);
            }
            finally
            {
                Conn.Close();
                Conn.Dispose();
            }

            return _TempImageRemark;
        }

        //Offer Letter
        private string LoadOfferLetterLink(string _TempEmpCode, string _FileType)
        {
            ////Load Out Image base on FILE TYPE  pass in. in database name : ATT_TYPE
            string TempEmpCode = _TempEmpCode;
            string FileType = _FileType;
            //string _TempImageURL = "";
            string _TempImageRemark = "";

            SqlConnection Conn = new SqlConnection(CUBIC_HRMS.GlobalVariable.ConnectionString);
            GF_CheckConnectionStatus(Conn);
            Conn.Open();

            try
            {
                string SQLCommand = "SELECT [ATT_FILE_REMARK] FROM T_EMP_ATT WHERE ATT_EMP_CODE = '" + TempEmpCode + "' AND ATT_TYPE = '" + FileType + "' ";
                SqlCommand cmdDataBase = new SqlCommand(SQLCommand, Conn);
                SqlDataReader myReader;
                myReader = cmdDataBase.ExecuteReader();

                if (myReader.HasRows)
                {
                    while (myReader.Read())
                    {
                        //_TempImageURL = myReader["ATT_FILE_PATH"].ToString();
                        _TempImageRemark = myReader["ATT_FILE_REMARK"].ToString();
                        LBtnOLP.Text = _TempImageRemark;
                        //Summary Area
                        LBtnSVOfferLetterDoc.Text = _TempImageRemark;
                    }
                }
                else
                {
                    //_TempImageURL = "";
                    _TempImageRemark = "";
                }
            }
            catch (Exception ex)
            {
                string message = ex.Message.ToString();
                MessageBoxShow(message);
            }
            finally
            {
                Conn.Close();
                Conn.Dispose();
            }

            return _TempImageRemark;
        }

        //Employee Information Letter
        private string LoadEmpInfLink(string _TempEmpCode, string _FileType)
        {
            ////Load Out Image base on FILE TYPE  pass in. in database name : ATT_TYPE
            string TempEmpCode = _TempEmpCode;
            string FileType = _FileType;
            //string _TempImageURL = "";
            string _TempImageRemark = "";

            SqlConnection Conn = new SqlConnection(CUBIC_HRMS.GlobalVariable.ConnectionString);
            GF_CheckConnectionStatus(Conn);
            Conn.Open();

            try
            {
                string SQLCommand = "SELECT [ATT_FILE_REMARK] FROM T_EMP_ATT WHERE ATT_EMP_CODE = '" + TempEmpCode + "' AND ATT_TYPE = '" + FileType + "' ";
                SqlCommand cmdDataBase = new SqlCommand(SQLCommand, Conn);
                SqlDataReader myReader;
                myReader = cmdDataBase.ExecuteReader();

                if (myReader.HasRows)
                {
                    while (myReader.Read())
                    {
                        //_TempImageURL = myReader["ATT_FILE_PATH"].ToString();
                        _TempImageRemark = myReader["ATT_FILE_REMARK"].ToString();
                        LBtnUploadEmpInfoDoc.Text = _TempImageRemark;
                        //Stella
                        LBtnSVEmpInfo.Text = _TempImageRemark;
                    }
                }
                else
                {
                    //_TempImageURL = "";
                    _TempImageRemark = "";
                }
            }
            catch (Exception ex)
            {
                string message = ex.Message.ToString();
                MessageBoxShow(message);
            }
            finally
            {
                Conn.Close();
                Conn.Dispose();
            }

            return _TempImageRemark;
        }


        private string LoadAnyImage(string _TempEmpCode, string _FileType)
        {
            ////Load Out Image base on FILE TYPE  pass in. in database name : ATT_TYPE
            ////if pass in Employee Image, then will load photo, if pass in Employee Visa, will load visa
            string TempEmpCode = _TempEmpCode;
            string FileType = _FileType;
            string _TempImageURL = "";
            string _TempImageRemark = "";

            SqlConnection Conn = new SqlConnection(CUBIC_HRMS.GlobalVariable.ConnectionString);
            GF_CheckConnectionStatus(Conn);
            Conn.Open();

            try
            {
                string SQLCommand = "SELECT ATT_FILE_PATH, [ATT_FILE_REMARK] FROM T_EMP_ATT WHERE ATT_EMP_CODE = '" + TempEmpCode + "' AND ATT_TYPE = '" + FileType + "' ";
                SqlCommand cmdDataBase = new SqlCommand(SQLCommand, Conn);
                SqlDataReader myReader;
                myReader = cmdDataBase.ExecuteReader();

                if (myReader.HasRows)
                {
                    while (myReader.Read())
                    {
                        _TempImageURL = myReader["ATT_FILE_PATH"].ToString();
                        _TempImageRemark = myReader["ATT_FILE_REMARK"].ToString();
                        LBtnSProImage.Text = _TempImageRemark;
                        LBtnEmpImage.Text = _TempImageRemark;
                    }
                }
                else
                {
                    _TempImageURL = "";
                }
            }
            catch (Exception ex)
            {
                string message = ex.Message.ToString();
                MessageBoxShow(message);
            }
            finally
            {
                Conn.Close();
                Conn.Dispose();
            }

            return _TempImageURL;
        }


        private void UpdateUploadAttData(int AttachmentNo, string _TempEmpCode, string _TempFilePath, string _TempFileRemark, string _FileType)
        {
            string TempEmpCode = _TempEmpCode;

            string TempFilePath1 = "-";
            string TempFilePath2 = "-";
            string TempFilePath3 = "-";
            string TempFileRemark1 = "-";
            string TempFileRemark2 = "-";
            string TempFileRemark3 = "-";
            string FileType = _FileType;

            if (AttachmentNo == 1)
            {
                TempFilePath1 = _TempFilePath;
                TempFileRemark1 = _TempFileRemark;
            }

            if (AttachmentNo == 2)
            {
                TempFilePath2 = _TempFilePath;
                TempFileRemark2 = _TempFileRemark;
            }

            if (AttachmentNo == 3)
            {
                TempFilePath3 = _TempFilePath;
                TempFileRemark3 = _TempFileRemark;
            }

            SqlConnection Conn = new SqlConnection(CUBIC_HRMS.GlobalVariable.ConnectionString);
            //SqlConnection Conn1 = new SqlConnection(CUBIC_HRMS.GlobalVariable.ConnectionString);
            GF_CheckConnectionStatus(Conn);
            Conn.Open();

            //if (Page.IsValid)
            //{

            try
            {
                // at here, do select statement first. 
                string SQLCommand = "SELECT ATT_TYPE FROM T_EMP_ATT WHERE ATT_EMP_CODE = '" + TempEmpCode + "' AND ATT_TYPE = '" + FileType + "' ";

                SqlCommand cmdDataBase = new SqlCommand(SQLCommand, Conn);
                SqlDataReader myReader;
                myReader = cmdDataBase.ExecuteReader();

                if (myReader.HasRows)
                {
                    UpdateEmpImage(TempEmpCode, TempFilePath1, TempFileRemark1, FileType);
                }
                else
                {
                    //DO INSERT STATMENT HERE
                    InsertEmpImage(TempEmpCode, TempFilePath1, TempFileRemark1, FileType);
                }
            }

            catch (Exception ex)
            {
                string message = ex.Message.ToString();
                MessageBoxShow(message);
            }
            finally
            {
                Conn.Close();
                Conn.Dispose();
            }

        }


        private void UpdateEmpImage(string _TempEmpCode, string _TempFilePath, string _TempFileRemark, string _FileType)
        {
            string TempEmpCode = _TempEmpCode;

            string TempFilePath1 = "-";
            string TempFileRemark1 = "-";
            string FileType = _FileType;

            TempFilePath1 = _TempFilePath;
            TempFileRemark1 = _TempFileRemark;

            SqlConnection Conn = new SqlConnection(CUBIC_HRMS.GlobalVariable.ConnectionString);
            GF_CheckConnectionStatus(Conn);
            Conn.Open();

            if (Page.IsValid)
            {
                try
                {
                    //DO UPDATE STATMENT HERE
                    string SQLInsertHeader = "UPDATE [dbo].[T_EMP_ATT] SET ";
                    //SQLInsertHeader = SQLInsertHeader + "[ATT_TYPE]= '" + FileType + "',";
                    SQLInsertHeader = SQLInsertHeader + "[ATT_FILE_PATH] = '" + TempFilePath1 + "',";
                    SQLInsertHeader = SQLInsertHeader + "[ATT_FILE_REMARK]= '" + TempFileRemark1 + "',";
                    SQLInsertHeader = SQLInsertHeader + "[ATT_MODIFY_BY]= '" + CUBIC_HRMS.GlobalVariable.G_UserLogin + "',";
                    SQLInsertHeader = SQLInsertHeader + "[ATT_MODIFY_DATE ]= getdate() ";
                    SQLInsertHeader = SQLInsertHeader + "WHERE ";
                    SQLInsertHeader = SQLInsertHeader + "ATT_EMP_CODE = '" + TempEmpCode + "' ";
                    SQLInsertHeader = SQLInsertHeader + " AND ATT_TYPE = '" + FileType + "' ";

                    var cmd2 = new SqlCommand(SQLInsertHeader, Conn);
                    cmd2.ExecuteNonQuery();

                }
                catch (Exception ex)
                {
                    string message = ex.Message.ToString();
                    MessageBoxShow(message);
                }
                finally
                {
                    Conn.Close();
                    Conn.Dispose();
                }
            }
        }


        private void InsertEmpImage(string _TempEmpCode, string _TempFilePath, string _TempFileRemark, string _FileType)
        {
            string TempEmpCode = _TempEmpCode;

            string TempFilePath1 = "-";
            string TempFileRemark1 = "-";
            string FileType = _FileType;

            TempFilePath1 = _TempFilePath;
            TempFileRemark1 = _TempFileRemark;

            SqlConnection Conn = new SqlConnection(CUBIC_HRMS.GlobalVariable.ConnectionString);
            GF_CheckConnectionStatus(Conn);
            Conn.Open();

            if (Page.IsValid)
            {
                try
                {
                    //DO INSERT STATMENT HERE
                    string SQLInsertHeader2 = "INSERT INTO [dbo].[T_EMP_ATT] ";
                    SQLInsertHeader2 = SQLInsertHeader2 + "([ATT_EMP_CODE]";
                    SQLInsertHeader2 = SQLInsertHeader2 + ",[ATT_TYPE]";
                    SQLInsertHeader2 = SQLInsertHeader2 + ",[ATT_FILE_PATH]";
                    SQLInsertHeader2 = SQLInsertHeader2 + ",[ATT_FILE_REMARK] ";
                    SQLInsertHeader2 = SQLInsertHeader2 + ",[ATT_CREATE_BY] ";
                    SQLInsertHeader2 = SQLInsertHeader2 + ",[ATT_CREATE_DATE] ";
                    SQLInsertHeader2 = SQLInsertHeader2 + ",[ATT_MODIFY_BY] ";
                    SQLInsertHeader2 = SQLInsertHeader2 + ",[ATT_MODIFY_DATE]) ";
                    SQLInsertHeader2 = SQLInsertHeader2 + "VALUES ";
                    SQLInsertHeader2 = SQLInsertHeader2 + "('" + TempEmpCode + "' ";
                    SQLInsertHeader2 = SQLInsertHeader2 + ",'" + FileType + "' ";
                    SQLInsertHeader2 = SQLInsertHeader2 + ",'" + TempFilePath1 + "' ";
                    SQLInsertHeader2 = SQLInsertHeader2 + ",'" + TempFileRemark1 + "' ";
                    SQLInsertHeader2 = SQLInsertHeader2 + ",'" + CUBIC_HRMS.GlobalVariable.G_UserLogin + "' ";
                    SQLInsertHeader2 = SQLInsertHeader2 + ",getdate() ";
                    SQLInsertHeader2 = SQLInsertHeader2 + ",'" + CUBIC_HRMS.GlobalVariable.G_UserLogin + "' ";
                    SQLInsertHeader2 = SQLInsertHeader2 + ",getdate() )";

                    var cmd3 = new SqlCommand(SQLInsertHeader2, Conn);
                    cmd3.ExecuteNonQuery();

                }
                catch (Exception ex)
                {
                    string message = ex.Message.ToString();
                    MessageBoxShow(message);
                }
                finally
                {
                    Conn.Close();
                    Conn.Dispose();
                }
            }
        }


        //Link after upload emp image
        protected void LBtnEmpImage_Click(object sender, EventArgs e)
        {
            string TempFileName = LBtnEmpImage.Text;
            //// at here, when we use syntax Server.MapPath 
            /// it will use our project directory as the main path and url
            /// this is why, when our project, run on D, 
            /// at database we will see it keep like D:\HR Attendance System\CUBIC_HRMS v1.0.35 21042020\Files
            ///  if this project, we run on E
            ///  at database we will see it keep like E:\HR Attendance System\CUBIC_HRMS v1.0.35 21042020\Files
            ///  but if this project, after we publish, we will run this project at C drive. 
            ///  thus, 
            ///  server.MapPath, will use this C:\www\TRIMULIA_HRMS\Files
            ///  at database we will see it keep data like C:\www\TRIMULIA_HRMS\Files
            ///  all is because of this syntax Server.MapPath
            ///  if, we wanna do flexible.. actually we do do like this 
            ///  instead of user Server.MapPath, we direct point it to certain path
            ///  like : //public static string G_AttFilePath = "C:\\www\\TRIMULIA_HRMS\\Files\\";
            ///  then we change
            /// string FilePath  = G_AttFilePath or
            ///  string FilePath = "C:\\www\\TRIMULIA_HRMS\\Files\\"
            ///  then everytime when we see at database. it will keep as the path we fixed
            string FilePath = G_AttFilePath;
            WebClient User = new WebClient();
            Byte[] FileBuffer = User.DownloadData(FilePath + TempFileName);

            if (FileBuffer != null)
            {
                Response.ContentType = "image/jpeg";
                Response.AddHeader("content-length", FileBuffer.Length.ToString());
                Response.TransmitFile(FilePath + TempFileName);
                Response.BinaryWrite(FileBuffer);
            }
        }


        protected void LBtnOLP_Click(object sender, EventArgs e)
        {

            //txtTempMessage.Text = "You clicked the link button";

            string TempFileName = LBtnOLP.Text;
            string FilePath = G_AttFilePath;
            //string FilePath = Server.MapPath("~/Files/");
            WebClient User = new WebClient();
            Byte[] FileBuffer = User.DownloadData(FilePath + TempFileName);

            if (FileBuffer != null)
            {
                Response.ContentType = "application/pdf";
                Response.AddHeader("content-length", FileBuffer.Length.ToString());
                Response.TransmitFile(FilePath + TempFileName);
                Response.BinaryWrite(FileBuffer);
            }
        }


        protected void LBtnResumeDoc_Click(object sender, EventArgs e)
        {

            //txtTempMessage.Text = "You clicked the link button";

            string TempFileName = LBtnResumeDoc.Text;
            string FilePath = G_AttFilePath;
            //string FilePath = Server.MapPath("~/Files/");
            WebClient User = new WebClient();
            Byte[] FileBuffer = User.DownloadData(FilePath + TempFileName);

            if (FileBuffer != null)
            {

                Response.ContentType = "application/pdf";
                Response.AddHeader("content-length", FileBuffer.Length.ToString());
                Response.TransmitFile(FilePath + TempFileName);
                Response.BinaryWrite(FileBuffer);

            }
        }



        protected void LBtnUploadEmpInfoDoc_Click(object sender, EventArgs e)
        {

            //txtTempMessage.Text = "You clicked the link button";

            string TempFileName = LBtnUploadEmpInfoDoc.Text;
            string FilePath = G_AttFilePath;
            //string FilePath = Server.MapPath("~/Files/");
            WebClient User = new WebClient();
            Byte[] FileBuffer = User.DownloadData(FilePath + TempFileName);

            if (FileBuffer != null)
            {

                Response.ContentType = "application/pdf";
                Response.AddHeader("content-length", FileBuffer.Length.ToString());
                Response.TransmitFile(FilePath + TempFileName);
                Response.BinaryWrite(FileBuffer);

            }
        }

        //***********Loading function*************//


        private void LoadSummaryPage()
        {
            //Loading Summary Information Page
            LoadMasterInfo(txtEmpCode.Text.Trim());

            LoadResumeLink(txtEmpCode.Text, "Employee Resume");
            LoadOfferLetterLink(txtEmpCode.Text, "Employee Offer Letter");
            LoadEmpInfLink(txtEmpCode.Text, "Employee Information");

            LoadAnyImage(txtEmpCode.Text, "Employee Image");

            LoadAddressInfo(txtEmpCode.Text.Trim());

            LoadPayrollInfo(txtEmpCode.Text.Trim());
        }

        //Load to Summary Page only
        private void LoadMasterInfo(string _EmpID)
        {
            SqlConnection Conn = new SqlConnection(ConnectionString);

            //TAKE FROM VIEW >> V_EMP_MASTER, so that can direct get BU name, DEPT name
            //View will more fast, if not understand, ask New ^-^
            string Query = "SELECT *  ";
            Query = Query + "FROM ";
            Query = Query + "[dbo].[V_EMP_MASTER] ";
            Query = Query + "WHERE ";
            //Query = Query + "EMP_BU = '" + CUBIC_HRMS.GlobalVariable.G_User_BU + "' AND ";
            Query = Query + "EMP_CODE = '" + _EmpID + "' ";

            SqlCommand cmdDataBase = new SqlCommand(Query, Conn);
            SqlDataReader myReader;

            try
            {
                GF_CheckConnectionStatus(Conn);

                Conn.Open();

                myReader = cmdDataBase.ExecuteReader();

                while (myReader.Read())
                {
                    //lblSVBusinessUnit.Text = myReader["EMP_BU"].ToString().Trim() + "-"+ myReader["BU_NAME"].ToString().Trim();
                    lblSVDepartment.Text = myReader["EMP_DEPT"].ToString().Trim() + "-" + myReader["DEPT_NAME"].ToString().Trim();
                    lblSVEmpCode.Text = myReader["EMP_CODE"].ToString().Trim();
                    lblSVFirstName.Text = myReader["EMP_FIRST_NAME"].ToString().Trim();
                    lblSVLastName.Text = myReader["EMP_LAST_NAME"].ToString().Trim();
                    lblSVNickName.Text = myReader["EMP_NICK_NAME"].ToString().Trim();
                    lblSVGender.Text = myReader["EMP_GENDER"].ToString().Trim();
                    //lblSVIsLocal.Text = myReader["EMP_ISLOCAL"].ToString().Trim();
                    DateTime DOB = Convert.ToDateTime(myReader["EMP_DOB"]);
                    lblSVDOB.Text = DOB.ToString("dd/MMM/yyyy");

                    lblSVICNo.Text = myReader["EMP_IC_NO"].ToString().Trim();
                    lblSVEmpEmail.Text = myReader["EMP_MAIL"].ToString().Trim();
                    lblSVEmpContactNo.Text = myReader["EMP_CONTACT_NO"].ToString().Trim();
                    lblSVCitizen.Text = myReader["EMP_CITIZEN"].ToString().Trim();
                    lblSVEPFNO.Text = myReader["EMP_EPF_NO"].ToString().Trim();
                    lblSVIncomeTaxNo.Text = myReader["EMP_INCOMETAX_NO"].ToString().Trim();
                    lblSValueSocso.Text = myReader["EMP_SOCSO_NO"].ToString().Trim();

                    //DateTime PassExpired = Convert.ToDateTime(myReader["EMP_PASSPORT_EXPIRED"]);
                    //lblSVPassExpDate.Text = PassExpired.ToString("dd/MMM/yyyy");

                    //lblSValueVisa.Text = myReader["EMP_VISA"].ToString().Trim();

                    //DateTime VisaExpired = Convert.ToDateTime(myReader["EMP_VISA_EXPIRED"]);
                    //lblSValueVisaExpDate.Text = VisaExpired.ToString("dd/MMM/yyyy");


                    //lblSVWorkPType.Text = myReader["EMP_WORK_PERMIT"].ToString().Trim();

                    //DateTime WPExpired = Convert.ToDateTime(myReader["EMP_WP_EXPIRED"]);
                    //lblSVWPExpDate.Text = WPExpired.ToString("dd/MMM/yyyy");

                    //lblSVAccessCard.Text = myReader["EMP_ACCESS_CARD"].ToString().Trim();
                    lblSVEmpCategory.Text = myReader["EMP_CATEGORY"].ToString().Trim();
                    lblSVEmpGrade.Text = myReader["EMP_GRADE"].ToString().Trim();
                    lblSVPositionTitle.Text = myReader["EMP_POSITION_TITLE"].ToString().Trim();
                    lblSVDirectSup.Text = myReader["EMP_DIRECT_SUPERIOR"].ToString().Trim() + "-" + myReader["EMP_DIRECT_SUPNAME"].ToString().Trim();
                    lblSVOnBehalf.Text = myReader["EMP_APP_ONBEHALF"].ToString().Trim() + "-" + myReader["EMP_APP_ONBEHALFNAME"].ToString().Trim();
                    lblSVEmeContPer1.Text = myReader["EMP_EMER_NAME1"].ToString().Trim();
                    lblSVEmeContNo1.Text = myReader["EMP_EMER_CONTACT1"].ToString().Trim();
                    //lblSVEmeContPer2.Text = myReader["EMP_EMER_NAME2"].ToString().Trim();
                    //lblSVEmeContNo2.Text = myReader["EMP_EMER_CONTACT2"].ToString().Trim();


                    string Status = (myReader["EMP_STATUS"].ToString().Trim());

                    if (Status == "Y")
                    {
                        lblSVStatus.Text = "Active";
                    }
                    else
                    {
                        lblSVStatus.Text = "Inactive";
                    }
                }
            }

            catch (Exception ex)
            {
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "Message", "alert('Error occured : " + ex.Message.ToString() + "');", true);
                string message = ex.Message.ToString();
                MessageBoxShow(message);
            }
            finally
            {
                Conn.Close();
                Conn.Dispose();
            }

        }

        //Load to Summary Page only
        private void LoadAddressInfo(string _EmpID)
        {
            SqlConnection Conn = new SqlConnection(ConnectionString);

            string Query = "SELECT *  ";
            Query = Query + "FROM ";
            Query = Query + ".[dbo].[M_EMP_SUB] ";
            Query = Query + "WHERE [SUB_EMP_CODE] = '" + _EmpID + "' ";

            SqlCommand cmdDataBase = new SqlCommand(Query, Conn);
            SqlDataReader myReader;

            try
            {
                GF_CheckConnectionStatus(Conn);

                Conn.Open();

                myReader = cmdDataBase.ExecuteReader();

                while (myReader.Read())
                {
                    lblSVAddress1.Text = myReader["SUB_HM_ADD1"].ToString().Trim();
                    lblSVAddress2.Text = myReader["SUB_HM_ADD2"].ToString().Trim();
                    lblSVAddress3.Text = myReader["SUB_HM_ADD3"].ToString().Trim();
                    lblSVAddress4.Text = myReader["SUB_HM_ADD4"].ToString().Trim();
                    lblSVPostCode.Text = myReader["SUB_HM_POSTCODE"].ToString().Trim();
                    lblSVState.Text = myReader["SUB_HM_STATE"].ToString().Trim();
                    lblSVCity.Text = myReader["SUB_HM_CITY"].ToString().Trim();
                    lblSVCountry.Text = myReader["SUB_HM_COUNTRY"].ToString().Trim();
                    //lblSVEmail.Text = myReader["SUB_HM_EMAIL"].ToString().Trim();

                    lblSVContactNo1.Text = myReader["SUB_HM_CONTACT_1"].ToString().Trim();
                    //lblSVContactNo2.Text = myReader["SUB_HM_CONTACT_2"].ToString().Trim();

                    lblSVMAddress1.Text = myReader["SUB_MAIL_ADD1"].ToString().Trim();
                    lblSVMAddress2.Text = myReader["SUB_MAIL_ADD2"].ToString().Trim();
                    lblSVMAddress3.Text = myReader["SUB_MAIL_ADD3"].ToString().Trim();
                    lblSVMAddress4.Text = myReader["SUB_MAIL_ADD4"].ToString().Trim();
                    lblSVMPostCode.Text = myReader["SUB_MAIL_POSTCODE"].ToString().Trim();

                    lblSVMState.Text = myReader["SUB_MAIL_STATE"].ToString().Trim();
                    lblSVMCity.Text = myReader["SUB_MAIL_CITY"].ToString().Trim();
                    lblSVMCountry.Text = myReader["SUB_MAIL_COUNTRY"].ToString().Trim();
                    //lblSVMEmail.Text = myReader["SUB_MAIL_EMAIL"].ToString().Trim();
                    lblSVMContact1.Text = myReader["SUB_MAIL_CONTACT"].ToString().Trim();

                }
            }

            catch (Exception ex)
            {
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "Message", "alert('Error occured : " + ex.Message.ToString() + "');", true);
                string message = ex.Message.ToString();
                MessageBoxShow(message);
            }
            finally
            {
                Conn.Close();
                Conn.Dispose();
            }

        }

        //Load to Summary Page only
        private void LoadPayrollInfo(string _EmpID)
        {
            SqlConnection Conn = new SqlConnection(ConnectionString);

            string Query = "SELECT *  ";
            Query = Query + "FROM ";
            Query = Query + "[dbo].[M_EMP_PAYROLL] ";
            Query = Query + "WHERE [PR_EMP_CODE] = '" + _EmpID + "' ";

            SqlCommand cmdDataBase = new SqlCommand(Query, Conn);
            SqlDataReader myReader;

            try
            {
                GF_CheckConnectionStatus(Conn);

                Conn.Open();

                myReader = cmdDataBase.ExecuteReader();

                while (myReader.Read())
                {
                    lblSVPaymentType.Text = myReader["PR_PAYMENT_TYPE"].ToString().Trim();



                    /*  lblSVSHiftPreset.Text= myReader["PR_SHIFT_PRESET"].ToString().Trim()*/
                    ;
                    //lblSVBankCode.Text = myReader["PR_BANK_CODE"].ToString().Trim();
                    lblSVAccountNo.Text = myReader["PR_ACC_NO"].ToString().Trim();
                    lblSVBankName.Text = myReader["PR_BANK_NAME"].ToString().Trim();
                    lblSVSalaryWedges.Text = myReader["PR_SALARY_WAGES"].ToString().Trim();
                    lblSVSalCurrency.Text = myReader["PR_SALARY_CURRENCY"].ToString().Trim();


                    lblSVEmpType.Text = myReader["PR_EMPLOYMENT_TYPE"].ToString().Trim();
                    lblSVSalType.Text = myReader["PR_SALARY_TYPE"].ToString().Trim();

                    lblSVIsDesign.Text = myReader["PR_ISRESIGN"].ToString().Trim();
                    lblSVWorkingHour.Text = myReader["PR_WORKINGHOUR"].ToString().Trim();

                    lblSVWorkingDay.Text = myReader["PR_WORKINGDAY"].ToString().Trim();
                    //lblSVIsResident.Text = myReader["PR_RESIDENT"].ToString().Trim();

                    DateTime firstPayDate = Convert.ToDateTime(myReader["PR_PAYREVIEW_DATE"]);
                    lblSVDate1stPayRev.Text = firstPayDate.ToString("dd/MMM/yyyy");

                    //lblSVLastNa.Text = myReader["PR_LASTREVIEW_DATE"].ToString().Trim();

                    //lblSVEClaimOT.Text = myReader["PR_OT"].ToString().Trim();
                    //lblSVAttTracking.Text = myReader["PR_ATTENDANCE"].ToString().Trim();
                    lblSVResignType.Text = myReader["PR_RESIGNTYPE"].ToString().Trim();
                    DateTime JoinDate = Convert.ToDateTime(myReader["PR_JOIN_DATE"]);
                    lblSVDateFirstJoin.Text = JoinDate.ToString("dd/MMM/yyyy");

                    //Stella: avoid confuse, only if is resign = yes, only show date
                    if (lblSVIsDesign.Text == "N")
                    {
                        lblSVDateResign.Text = "";
                    }
                    else
                    {
                        DateTime ResignDate = Convert.ToDateTime(myReader["PR_RESIGN_DATE"]);
                        lblSVDateResign.Text = ResignDate.ToString("dd/MMM/yyyy");
                    }


                    lblSVAllWages.Text = myReader["PR_ALLOWANCE_WAGES"].ToString().Trim();
                    lblSVAllCurrency.Text = myReader["PR_ALLOWANCE_CURRENCY"].ToString().Trim();

                }
            }

            catch (Exception ex)
            {
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "Message", "alert('Error occured : " + ex.Message.ToString() + "');", true);
                string message = ex.Message.ToString();
                MessageBoxShow(message);
            }
            finally
            {
                Conn.Close();
                Conn.Dispose();
            }

        }


        private void LoadExisitingMasterInfo(string _empCode)
        {
            SqlConnection Conn = new SqlConnection(ConnectionString);

            string Query = "SELECT *  ";
            Query = Query + "FROM ";
            Query = Query + "[dbo].[M_EMP_MASTER] ";
            Query = Query + "WHERE [EMP_CODE] = '" + _empCode + "' ";
            //Query = Query + "ORDER BY [EMP_CODE] ASC ";

            SqlCommand cmdDataBase = new SqlCommand(Query, Conn);
            SqlDataReader myReader;

            try
            {
                GF_CheckConnectionStatus(Conn);

                Conn.Open();

                myReader = cmdDataBase.ExecuteReader();
                if (myReader.HasRows)
                {

                    while (myReader.Read())
                    {
                        //ddlBU.Text = (myReader["EMP_BU"].ToString());

                        populatePosition();
                        populateDepartment();



                        if (myReader["EMP_DEPT"].ToString() == "-")
                        {
                            ddlDepartment.SelectedIndex = 0;
                        }
                        else
                        {
                            ddlDepartment.Text = (myReader["EMP_DEPT"].ToString());
                        }


                        txtEmpCode.Text = (myReader["EMP_CODE"].ToString());
                        txtFirstName.Text = (myReader["EMP_FIRST_NAME"].ToString());
                        txtLastName.Text = (myReader["EMP_LAST_NAME"].ToString());
                        txtNickName.Text = (myReader["EMP_NICK_NAME"].ToString());
                        string Gender = (myReader["EMP_GENDER"].ToString());

                        if (Gender == "F")
                        {
                            rdGender.Text = "Female";
                        }
                        else
                        {
                            rdGender.Text = "Male";
                        }

                        //dtDOB.CalendarDate = Convert.ToDateTime(myReader["EMP_DOB"]);
                        DateTime DOB = Convert.ToDateTime(myReader["EMP_DOB"].ToString());
                        dtDOB.Text = DOB.ToLocalTime().ToString("yyyy-MM-dd");


                        txtICNo.Text = (myReader["EMP_IC_NO"].ToString());

                        txtEmpEmail.Text = (myReader["EMP_MAIL"].ToString());
                        txtEmpContact.Text = (myReader["EMP_CONTACT_NO"].ToString());
                        ddlCitizen.Text = (myReader["EMP_CITIZEN"].ToString());
                        //ddlIsLocal.Text= (myReader["EMP_ISLOCAL"].ToString());
                        txtEPFNo.Text = (myReader["EMP_EPF_NO"].ToString());

                        //dtPassportEDate.CalendarDate = Convert.ToDateTime(myReader["EMP_PASSPORT_EXPIRED"]);
                        txtIncomeTaxNo.Text = (myReader["EMP_INCOMETAX_NO"].ToString());
                        txtSocsoNo.Text = (myReader["EMP_SOCSO_NO"].ToString());

                        //DateTime PassportEDate = Convert.ToDateTime(myReader["EMP_PASSPORT_EXPIRED"].ToString());
                        //dtPassportEDate.Text = PassportEDate.ToLocalTime().ToString("yyyy-MM-dd");


                        //dtPassportEDate.CalendarDate = Convert.ToDateTime(myReader["EMP_PASSPORT_EXPIRED"]);

                        //txtVisa.Text = (myReader["EMP_VISA"].ToString());

                        //dtVisaEDate.CalendarDate = Convert.ToDateTime(myReader["EMP_VISA_EXPIRED"]);
                        //dtVisaEDate.Text = (myReader["EMP_VISA_EXPIRED"].ToString());

                        //DateTime EmpVisaExpired = Convert.ToDateTime(myReader["EMP_VISA_EXPIRED"].ToString());
                        //dtVisaEDate.Text = EmpVisaExpired.ToLocalTime().ToString("yyyy-MM-dd");

                        //txtWorkPermitType.Text = (myReader["EMP_WORK_PERMIT"].ToString());

                        //dtWPermitEDate.CalendarDate = Convert.ToDateTime(myReader["EMP_WP_EXPIRED"]);
                        //dtWPermitEDate.Text = (myReader["EMP_WP_EXPIRED"].ToString());

                        //DateTime WPExpired = Convert.ToDateTime(myReader["EMP_WP_EXPIRED"].ToString());
                        //dtWPermitEDate.Text = WPExpired.ToLocalTime().ToString("yyyy-MM-dd");


                        txtAccessCard.Text = (myReader["EMP_ACCESS_CARD"].ToString());
                        ddlEmpCategory.Text = (myReader["EMP_CATEGORY"].ToString());
                        //ddlEmpGrade.Text = (myReader["EMP_GRADE"].ToString());


                        if (myReader["EMP_POSITION_TITLE"].ToString() == "-")
                        {
                            ddlPositionTitle.SelectedIndex = 0;
                        }
                        else
                        {
                            ddlPositionTitle.Text = (myReader["EMP_POSITION_TITLE"].ToString());
                        }


                        txtDirectSuperiorName.Text = (myReader["EMP_DIRECT_SUPNAME"].ToString());
                        txtOnBehalfName.Text = (myReader["EMP_APP_ONBEHALFNAME"].ToString());


                        if (myReader["EMP_DIRECT_SUPERIOR"].ToString() == "-")
                        {
                            ddlSelectDSuperior.SelectedIndex = 0;
                        }
                        else
                        {
                            ddlSelectDSuperior.Text = (myReader["EMP_DIRECT_SUPERIOR"].ToString());
                        }


                        if (myReader["EMP_APP_ONBEHALF"].ToString() == "-")
                        {
                            ddlSelectOnBehalf.SelectedIndex = 0;
                        }
                        else
                        {
                            ddlSelectOnBehalf.Text = (myReader["EMP_APP_ONBEHALF"].ToString());
                        }



                        txtEContPerson1.Text = (myReader["EMP_EMER_NAME1"].ToString());
                        txtECContactNo1.Text = (myReader["EMP_EMER_CONTACT1"].ToString());

                        string Status = (myReader["EMP_STATUS"].ToString());

                        if (Status == "Y")
                        {
                            ddlStatus.Text = "Yes";
                        }
                        else
                        {
                            ddlStatus.Text = "No";
                        }


                    }
                }

            }
            catch (Exception ex)
            {
                string TempAudit = ex.ToString().Replace("'", "");
                GF_InsertAuditLog("-", "Data Check", "FrmEmployeeMaster", TempAudit, "LoadExisitingMasterInfo");
            }
            finally
            {
                Conn.Close();
                Conn.Dispose();
            }

        }


        private void LoadExisitingAddressInfo(string _empCode)
        {
            SqlConnection Conn = new SqlConnection(ConnectionString);

            string Query = "SELECT *  ";
            Query = Query + "FROM ";
            Query = Query + "[dbo].[M_EMP_SUB]";
            Query = Query + "WHERE [SUB_EMP_CODE] = '" + _empCode + "' ";

            SqlCommand cmdDataBase = new SqlCommand(Query, Conn);
            SqlDataReader myReader;

            try
            {
                GF_CheckConnectionStatus(Conn);

                Conn.Open();

                myReader = cmdDataBase.ExecuteReader();

                while (myReader.Read())
                {

                    txtAddress1.Text = (myReader["SUB_HM_ADD1"].ToString());
                    txtAddress2.Text = (myReader["SUB_HM_ADD2"].ToString());
                    txtAddress3.Text = (myReader["SUB_HM_ADD3"].ToString());
                    txtAddress4.Text = (myReader["SUB_HM_ADD4"].ToString());
                    txtPostCode.Text = (myReader["SUB_HM_POSTCODE"].ToString());
                    txtState.Text = (myReader["SUB_HM_STATE"].ToString());
                    txtCity.Text = (myReader["SUB_HM_CITY"].ToString());
                    txtCountry.Text = (myReader["SUB_HM_COUNTRY"].ToString());
                    //txtAddEmailC1.Text = (myReader["SUB_HM_EMAIL"].ToString());
                    txtAddContactNo1.Text = (myReader["SUB_HM_CONTACT_1"].ToString());
                    //txtAddContactNo2.Text = (myReader["SUB_HM_CONTACT_2"].ToString());

                    string chkAddress = (myReader["SUB_CHK_SAMEABVADD"].ToString());

                    if (chkAddress == "true")
                    {
                        chkSameAsAbove.Checked = true;
                    }
                    else
                    {
                        chkSameAsAbove.Checked = false;
                    }

                    txtMAddress1.Text = (myReader["SUB_MAIL_ADD1"].ToString());
                    txtMAddress2.Text = (myReader["SUB_MAIL_ADD2"].ToString());
                    txtMAddress3.Text = (myReader["SUB_MAIL_ADD3"].ToString());
                    txtMAddress4.Text = (myReader["SUB_MAIL_ADD4"].ToString());
                    txtMPostCode.Text = (myReader["SUB_MAIL_POSTCODE"].ToString());
                    txtMState.Text = (myReader["SUB_MAIL_STATE"].ToString());
                    txtMCity.Text = (myReader["SUB_MAIL_CITY"].ToString());
                    txtMCountry.Text = (myReader["SUB_MAIL_COUNTRY"].ToString());
                    //txtMEmail.Text = (myReader["SUB_MAIL_EMAIL"].ToString());
                    txtMContactNo1.Text = (myReader["SUB_MAIL_CONTACT"].ToString());
                }

            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Message", "alert('Error occured : " + ex.Message.ToString() + "');", true);
            }
            finally
            {
                Conn.Close();
                Conn.Dispose();
            }

        }


        private void LoadExisitingPayrollInfo(string _empCode)
        {
            SqlConnection Conn = new SqlConnection(ConnectionString);

            string Query = "SELECT *  ";
            Query = Query + "FROM ";
            Query = Query + "[dbo].[M_EMP_PAYROLL]";
            Query = Query + "WHERE [PR_EMP_CODE] = '" + _empCode + "' ";

            SqlCommand cmdDataBase = new SqlCommand(Query, Conn);
            SqlDataReader myReader;

            try
            {
                GF_CheckConnectionStatus(Conn);

                Conn.Open();

                myReader = cmdDataBase.ExecuteReader();

                if (myReader.HasRows)
                {
                    while (myReader.Read())
                    {

                        ddlPaymentType.Text = (myReader["PR_PAYMENT_TYPE"].ToString());
                        //ddlBankCode.Text = (myReader["PR_BANK_CODE"].ToString());
                        txtAccountNo.Text = (myReader["PR_ACC_NO"].ToString());
                        txtBankName.Text = (myReader["PR_BANK_NAME"].ToString());

                        txtSalaryWedges.Text = (myReader["PR_SALARY_WAGES"].ToString());
                        ddlSalaryCurrency.Text = (myReader["PR_SALARY_CURRENCY"].ToString());

                        txtAllowanceWedges.Text = (myReader["PR_ALLOWANCE_WAGES"].ToString());
                        ddlAllowanceCurrency.Text = (myReader["PR_ALLOWANCE_CURRENCY"].ToString());

                        ddlEmpType.Text = (myReader["PR_EMPLOYMENT_TYPE"].ToString());
                        ddlSalaryType.Text = (myReader["PR_SALARY_TYPE"].ToString());
                        txtWorkingHour.Text = (myReader["PR_WORKINGHOUR"].ToString());

                        txtWorkingDay.Text = (myReader["PR_WORKINGDAY"].ToString());
                        //ddlResident.Text = (myReader["PR_RESIDENT"].ToString());
                        ddlShiftPreset.Text = (myReader["PR_SHIFT_PRESET"].ToString());


                        //DTDatePayReview.CalendarDate = Convert.ToDateTime(myReader["PR_PAYREVIEW_DATE"]);

                        DateTime PayReview = Convert.ToDateTime(myReader["PR_PAYREVIEW_DATE"].ToString());
                        DTDatePayReview.Text = PayReview.ToString("yyyy-MM-dd");


                        //DTDatePayReview.CalendarDate = Convert.ToDateTime(myReader["PR_LASTREVIEW_DATE"]);
                        //dtDateJoin.CalendarDate = Convert.ToDateTime(myReader["PR_JOIN_DATE"]);
                        DateTime JoinDate = Convert.ToDateTime(myReader["PR_JOIN_DATE"].ToString());
                        dtDateJoin.Text = JoinDate.ToString("yyyy-MM-dd");


                        //dtDateResign.CalendarDate = Convert.ToDateTime(myReader["PR_RESIGN_DATE"]);
                        //dtDateResign.Text = myReader["PR_RESIGN_DATE"].ToString();

                        DateTime ResignDate = Convert.ToDateTime(myReader["PR_RESIGN_DATE"].ToString());
                        dtDateResign.Text = JoinDate.ToString("yyyy-MM-dd");

                        ddlResignType.Text = (myReader["PR_RESIGNTYPE"].ToString());
                        string IsResign = (myReader["PR_ISRESIGN"].ToString());

                        if (IsResign == "Y")
                        {
                            rbIsResign.SelectedValue = "Yes";
                            dtDateResign.Visible = true;
                        }
                        else
                        {
                            rbIsResign.SelectedValue = "No";
                            dtDateResign.Visible = false;
                        }

                        string EclaimOT = (myReader["PR_OT"].ToString());

                        if (EclaimOT == "Y")
                        {
                            rbEClaimOT.SelectedValue = "Yes";
                        }
                        else
                        {
                            rbEClaimOT.SelectedValue = "No";
                        }

                        string AttTracking = (myReader["PR_ATTENDANCE"].ToString());

                        //if (AttTracking == "Y")
                        //{
                        //    rbAttTracking.SelectedValue = "Yes";
                        //}
                        //else
                        //{
                        //    rbAttTracking.SelectedValue = "No";
                        //}

                    }
                }
                else
                {

                }

            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Message", "alert('Error occured : " + ex.Message.ToString() + "');", true);
            }
            finally
            {
                Conn.Close();
                Conn.Dispose();
            }

        }



        private void ClearAllTextWOEmpCode()
        {
            //Clear first page
            //populateBU();
            populateDepartment();

            txtEmpCode.Text = "";
            txtFirstName.Text = "";
            txtLastName.Text = "";
            txtICNo.Text = "";
            txtEmpEmail.Text = "";
            txtNickName.Text = "";
            ddlCitizen.SelectedIndex = 0;
            txtEPFNo.Text = "";
            txtIncomeTaxNo.Text = "";
            txtSocsoNo.Text = "";
            ddlSelectDSuperior.SelectedIndex = 0;
            ddlSelectOnBehalf.SelectedIndex = 0;
            txtOnBehalfName.Text = "";
            txtDirectSuperiorName.Text = "";
            txtAccessCard.Text = "";
            txtEContPerson1.Text = "";
            txtECContactNo1.Text = "";
            //txtECContactNo2.Text = "";

            rbIsResign.Text = "No";
            //rbStatus.SelectedIndex = 0;

            ddlStatus.SelectedIndex = 0;
            rbEClaimOT.SelectedIndex = 0;
            //rbAttTracking.SelectedIndex = 0;

            //dtPassportEDate.CalendarDate = DateTime.Now;
            //dtVisaEDate.CalendarDate = DateTime.Now;
            //dtWPermitEDate.CalendarDate = DateTime.Now;
            //dtDOB.CalendarDate = DateTime.Now;

            //dtPassportEDate.Text = DateTime.Now.ToLocalTime().ToString("yyyy-MM-dd");
            //dtVisaEDate.Text = DateTime.Now.ToLocalTime().ToString("yyyy-MM-dd");
            //dtWPermitEDate.Text = DateTime.Now.ToLocalTime().ToString("yyyy-MM-dd");
            dtDOB.Text = DateTime.Now.ToLocalTime().ToString("yyyy-MM-dd");


            //Clear Second Page
            txtAddress1.Text = "";
            txtAddress2.Text = "";
            txtAddress3.Text = "";
            txtAddress4.Text = "";
            txtPostCode.Text = "";
            txtState.Text = "";
            txtAddContactNo1.Text = "";
            //txtAddContactNo2.Text = "";
            txtCity.Text = "";
            txtCountry.Text = "";
            //txtAddEmailC1.Text = "";

            chkSameAsAbove.Checked = false;

            txtMAddress1.Text = "";
            txtMAddress2.Text = "";
            txtMAddress3.Text = "";
            txtMPostCode.Text = "";
            txtMState.Text = "";
            txtMContactNo1.Text = "";
            txtMCity.Text = "";
            txtMCountry.Text = "";
            //txtMEmail.Text = "";

            //payroll
            txtBankName.Text = "";
            txtAccountNo.Text = "";
            txtSalaryWedges.Text = "";
            ddlSalaryCurrency.Text = "MYR";
            ddlAllowanceCurrency.Text = "MYR";
            txtWorkingHour.Text = "9";
            txtWorkingDay.Text = "26";
            ddlShiftPreset.SelectedIndex = 0;
            //ddlResident.SelectedIndex = 0;


            //dtDateJoin.CalendarDate = DateTime.Now;
            //DTDatePayReview.CalendarDate = DateTime.Now;
            //dtDateResign.CalendarDate = DateTime.Now;
            dtDateJoin.Text = DateTime.Now.ToLocalTime().ToString("yyyy-MM-dd");
            DTDatePayReview.Text = DateTime.Now.ToLocalTime().ToString("yyyy-MM-dd");
            dtDateResign.Text = DateTime.Now.ToLocalTime().ToString("yyyy-MM-dd");

            //Summary page
            //lblSVBusinessUnit.Text = "";
            lblSVDepartment.Text = "";
            lblSVEmpCode.Text = "";
            lblSVFirstName.Text = "";
            lblSVNickName.Text = "";
            lblSVICNo.Text = "";
            lblSVCitizen.Text = "";
            //lblSVPassNo.Text = "";
            //lblSValueVisa.Text = "";
            //lblSVWorkPType.Text = "";
            lblSVEmpCategory.Text = "";
            //lblSVEClaimOT.Text = "";
            lblSVPositionTitle.Text = "";
            lblSVOnBehalf.Text = "";

            lblSVLastName.Text = "";
            lblSVGender.Text = "";
            lblSVDOB.Text = "";
            lblSVStatus.Text = "";
            //lblSVPassExpDate.Text = "";
            //lblSValueVisaExpDate.Text = "";
            //lblSVWPExpDate.Text = "";
            lblSVEmpGrade.Text = "";
            //lblSVAttTracking.Text = "";
            lblSVDirectSup.Text = "";
            //lblSVAccessCard.Text = "";

            lblSVEmeContPer1.Text = "";
            //lblSVEmeContPer2.Text = "";
            lblSVEmeContNo1.Text = "";
            //lblSVEmeContNo2.Text = "'";

            //address information
            lblSVAddress1.Text = "";
            lblSVAddress2.Text = "";
            lblSVAddress3.Text = "";
            lblSVAddress4.Text = "'";
            lblSVPostCode.Text = "";
            lblSVState.Text = "";
            lblSVContactNo1.Text = "";
            //lblSVContactNo2.Text = "";
            lblSVCity.Text = "";
            lblSVCountry.Text = "";
            //lblSVEmail.Text = "";

            //Mailing Address info
            lblSVMAddress1.Text = "";
            lblSVMAddress2.Text = "";
            lblSVMAddress3.Text = "";
            lblSVMAddress4.Text = "";
            lblSVMPostCode.Text = "";
            lblSVMState.Text = "";
            lblSVMContact1.Text = "'";
            lblSVMCity.Text = "'";
            lblSVMCountry.Text = "";
            //lblSVMEmail.Text = "";

            //Payroll info
            lblSVPaymentType.Text = "";
            //lblSVSHiftPreset.Text = "";
            lblSVBankName.Text = "";
            lblSVEmpType.Text = "";
            lblSVSalaryWedges.Text = "";
            lblSVSalCurrency.Text = "";

            lblSVAllWages.Text = "";
            lblSVAllCurrency.Text = "";
            lblSVDateFirstJoin.Text = "";
            //lblSVBankCode.Text = "";
            lblSVAccountNo.Text = "";
            lblSVSalType.Text = "";
            lblSVDate1stPayRev.Text = "";
            lblSVDateResign.Text = "";
            lblSVIsDesign.Text = "";

        }


        private void ClearAllText()
        {
            //Clear first page
            //populateBU();
            populateDepartment();
            populateEmpCodeddl();

            EmpPImage.ImageUrl = "~/Image/DefaultProfile.png";

            txtEmpCode.Text = "";
            txtFirstName.Text = "";
            txtLastName.Text = "";
            txtICNo.Text = "";
            txtEmpEmail.Text = "";
            txtNickName.Text = "";
            ddlCitizen.SelectedIndex = 0;
            txtEPFNo.Text = "";
            txtIncomeTaxNo.Text = "";
            txtSocsoNo.Text = "";
            ddlSelectDSuperior.SelectedIndex = 0;
            ddlSelectOnBehalf.SelectedIndex = 0;
            txtOnBehalfName.Text = "";
            txtDirectSuperiorName.Text = "";
            txtAccessCard.Text = "";
            txtEContPerson1.Text = "";
            txtECContactNo1.Text = "";
            //txtECContactNo2.Text = "";

            //rbStatus.SelectedIndex = 0;
            rbEClaimOT.SelectedIndex = 0;
            //rbAttTracking.SelectedIndex = 0;
            rbIsResign.Text = "No";

            //dtPassportEDate.CalendarDate = DateTime.Now;
            //dtVisaEDate.CalendarDate = DateTime.Now;
            //dtWPermitEDate.CalendarDate = DateTime.Now;
            //dtDOB.CalendarDate = DateTime.Now;

            //dtPassportEDate.Text = DateTime.Now.ToLocalTime().ToString("yyyy-MM-dd");
            //dtVisaEDate.Text = DateTime.Now.ToLocalTime().ToString("yyyy-MM-dd");
            //dtWPermitEDate.Text = DateTime.Now.ToLocalTime().ToString("yyyy-MM-dd");
            dtDOB.Text = DateTime.Now.ToLocalTime().ToString("yyyy-MM-dd");

            //Clear Second Page
            txtAddress1.Text = "";
            txtAddress2.Text = "";
            txtAddress3.Text = "";
            txtAddress4.Text = "";
            txtPostCode.Text = "";
            txtState.Text = "";
            txtAddContactNo1.Text = "";
            //txtAddContactNo2.Text = "";
            txtCity.Text = "";
            txtCountry.Text = "";
            //txtAddEmailC1.Text = "";

            chkSameAsAbove.Checked = false;

            txtMAddress1.Text = "";
            txtMAddress2.Text = "";
            txtMAddress3.Text = "";
            txtMPostCode.Text = "";
            txtMState.Text = "";
            txtMContactNo1.Text = "";
            txtMCity.Text = "";
            txtMCountry.Text = "";
            //txtMEmail.Text = "";

            //payroll
            txtBankName.Text = "";
            txtAccountNo.Text = "";
            txtSalaryWedges.Text = "";
            ddlShiftPreset.SelectedIndex = 0;
            ddlSalaryCurrency.Text = "RIEL";
            ddlAllowanceCurrency.Text = "RIEL";
            txtWorkingHour.Text = "9";

            txtWorkingDay.Text = "26";
            //ddlResident.SelectedIndex = 0;

            //dtDateJoin.CalendarDate = DateTime.Now;
            //DTDatePayReview.CalendarDate = DateTime.Now;
            //dtDateResign.CalendarDate = DateTime.Now;
            dtDateJoin.Text = DateTime.Now.ToLocalTime().ToString("yyyy-MM-dd");
            DTDatePayReview.Text = DateTime.Now.ToLocalTime().ToString("yyyy-MM-dd");
            dtDateResign.Text = DateTime.Now.ToLocalTime().ToString("yyyy-MM-dd");

            //Summary page
            //lblSVBusinessUnit.Text = "";
            lblSVDepartment.Text = "";
            lblSVEmpCode.Text = "";
            lblSVFirstName.Text = "";
            lblSVNickName.Text = "";
            lblSVICNo.Text = "";
            lblSVCitizen.Text = "";
            //lblSVPassNo.Text = "";
            //lblSValueVisa.Text = "";
            //lblSVWorkPType.Text = "";
            lblSVEmpCategory.Text = "";
            //lblSVEClaimOT.Text = "";
            lblSVPositionTitle.Text = "";
            lblSVOnBehalf.Text = "";

            lblSVLastName.Text = "";
            lblSVGender.Text = "";
            lblSVDOB.Text = "";
            lblSVStatus.Text = "";
            //lblSVPassExpDate.Text = "";
            //lblSValueVisaExpDate.Text = "";
            //lblSVWPExpDate.Text = "";
            lblSVEmpGrade.Text = "";
            //lblSVAttTracking.Text = "";
            lblSVDirectSup.Text = "";
            //lblSVAccessCard.Text = "";

            lblSVEmeContPer1.Text = "";
            //lblSVEmeContPer2.Text = "";
            lblSVEmeContNo1.Text = "";
            //lblSVEmeContNo2.Text = "'";

            //address information
            lblSVAddress1.Text = "";
            lblSVAddress2.Text = "";
            lblSVAddress3.Text = "";
            lblSVAddress4.Text = "'";
            lblSVPostCode.Text = "";
            lblSVState.Text = "";
            lblSVContactNo1.Text = "";
            //lblSVContactNo2.Text = "";
            lblSVCity.Text = "";
            lblSVCountry.Text = "";
            //lblSVEmail.Text = "";

            //Mailing Address info
            lblSVMAddress1.Text = "";
            lblSVMAddress2.Text = "";
            lblSVMAddress3.Text = "";
            lblSVMAddress4.Text = "";
            lblSVMPostCode.Text = "";
            lblSVMState.Text = "";
            lblSVMContact1.Text = "'";
            lblSVMCity.Text = "'";
            lblSVMCountry.Text = "";
            //lblSVMEmail.Text = "";

            //Payroll info
            lblSVPaymentType.Text = "";
            //lblSVSHiftPreset.Text = "";
            lblSVBankName.Text = "";
            lblSVEmpType.Text = "";
            lblSVSalaryWedges.Text = "";
            lblSVSalCurrency.Text = "";

            lblSVAllWages.Text = "";
            lblSVAllCurrency.Text = "";
            lblSVDateFirstJoin.Text = "";
            //lblSVBankCode.Text = "";
            lblSVAccountNo.Text = "";
            lblSVSalType.Text = "";
            lblSVDate1stPayRev.Text = "";
            lblSVDateResign.Text = "";
            lblSVIsDesign.Text = "";

            LBtnResumeDoc.Text = "";
            LBtnOLP.Text = "";
            LBtnUploadEmpInfoDoc.Text = "";
            LBtnEmpImage.Text = "";
        }


        protected void btnClearAll1_Click(object sender, EventArgs e)
        {
            //Clear first page
            //populateBU();
            populateDepartment();
            populateEmpCodeddl();
            txtEmpCode.Text = "";

            txtFirstName.Text = "";
            txtLastName.Text = "";
            txtICNo.Text = "";
            txtEmpEmail.Text = "";
            txtNickName.Text = "";
            ddlCitizen.SelectedIndex = 0;
            txtEPFNo.Text = "";
            txtIncomeTaxNo.Text = "";
            txtSocsoNo.Text = "";

            ddlSelectDSuperior.SelectedIndex = 0;
            ddlSelectOnBehalf.SelectedIndex = 0;
            txtOnBehalfName.Text = "";
            txtDirectSuperiorName.Text = "";
            txtAccessCard.Text = "";
            txtEContPerson1.Text = "";
            txtECContactNo1.Text = "";
            //txtECContactNo2.Text = "";

            //rbStatus.SelectedIndex = 0;
            ddlStatus.SelectedIndex = 0;


            //dtPassportEDate.CalendarDate = DateTime.Now;
            //dtVisaEDate.CalendarDate = DateTime.Now;
            //dtWPermitEDate.CalendarDate = DateTime.Now;
            //dtDOB.CalendarDate = DateTime.Now;
            //dtPassportEDate.Text = DateTime.Now.ToLocalTime().ToString("yyyy-MM-dd");
            //dtVisaEDate.Text = DateTime.Now.ToLocalTime().ToString("yyyy-MM-dd");
            //dtWPermitEDate.Text = DateTime.Now.ToLocalTime().ToString("yyyy-MM-dd");
            dtDOB.Text = DateTime.Now.ToLocalTime().ToString("yyyy-MM-dd");
        }


        protected void btnClearAll2_Click(object sender, EventArgs e)
        {
            //Clear Second Page
            txtAddress1.Text = "";
            txtAddress2.Text = "";
            txtAddress3.Text = "";
            txtAddress4.Text = "";
            txtPostCode.Text = "";
            txtState.Text = "";
            txtAddContactNo1.Text = "";
            //txtAddContactNo2.Text = "";
            txtCity.Text = "";
            txtCountry.Text = "";
            //txtAddEmailC1.Text = "";

            chkSameAsAbove.Checked = false;

            txtMAddress1.Text = "";
            txtMAddress2.Text = "";
            txtMAddress3.Text = "";
            txtMPostCode.Text = "";
            txtMState.Text = "";
            txtMContactNo1.Text = "";
            txtMCity.Text = "";
            txtMCountry.Text = "";
            //txtMEmail.Text = "";

        }


        protected void btnClearAll3_Click(object sender, EventArgs e)
        {
            //payroll
            txtBankName.Text = "";
            txtAccountNo.Text = "";
            txtSalaryWedges.Text = "";
            ddlSalaryCurrency.Text = "RIEL";
            ddlAllowanceCurrency.Text = "RIEL";
            txtWorkingHour.Text = "9";
            txtWorkingDay.Text = "26";
            //ddlResident.SelectedIndex = 0;

            ddlShiftPreset.SelectedIndex = 0;

            //dtDateJoin.CalendarDate = DateTime.Now;
            //DTDatePayReview.CalendarDate = DateTime.Now;
            //dtDateResign.CalendarDate = DateTime.Now;
            dtDateJoin.Text = DateTime.Now.ToLocalTime().ToString("yyyy-MM-dd");
            DTDatePayReview.Text = DateTime.Now.ToLocalTime().ToString("yyyy-MM-dd");
            dtDateResign.Text = DateTime.Now.ToLocalTime().ToString("yyyy-MM-dd");

            rbEClaimOT.SelectedIndex = 0;
            //rbAttTracking.SelectedIndex = 0;
        }


        //Offer Letter
        protected void LBtnSVOfferLetterDoc_Click(object sender, EventArgs e)
        {
            string TempFileName = LBtnSVOfferLetterDoc.Text;
            string FilePath = G_AttFilePath;
            //string FilePath = Server.MapPath("~/Files/");
            WebClient User = new WebClient();
            Byte[] FileBuffer = User.DownloadData(FilePath + TempFileName);

            if (FileBuffer != null)
            {
                Response.ContentType = "application/pdf";
                Response.AddHeader("content-length", FileBuffer.Length.ToString());
                Response.TransmitFile(FilePath + TempFileName);
                Response.BinaryWrite(FileBuffer);
            }
        }

        //Resume
        protected void LBtnSVResume_Click(object sender, EventArgs e)
        {

            string TempFileName = LBtnSVResume.Text;
            string FilePath = G_AttFilePath;
            //string FilePath = Server.MapPath("~/Files/");
            WebClient User = new WebClient();
            Byte[] FileBuffer = User.DownloadData(FilePath + TempFileName);

            if (FileBuffer != null)
            {
                Response.ContentType = "application/pdf";
                Response.AddHeader("content-length", FileBuffer.Length.ToString());
                Response.TransmitFile(FilePath + TempFileName);
                Response.BinaryWrite(FileBuffer);
            }
        }


        //Resume
        protected void LBtnSVEmpInfo_Click(object sender, EventArgs e)
        {

            string TempFileName = LBtnSVEmpInfo.Text;
            string FilePath = G_AttFilePath;
            //string FilePath = Server.MapPath("~/Files/");
            WebClient User = new WebClient();
            Byte[] FileBuffer = User.DownloadData(FilePath + TempFileName);

            if (FileBuffer != null)
            {
                Response.ContentType = "application/pdf";
                Response.AddHeader("content-length", FileBuffer.Length.ToString());
                Response.TransmitFile(FilePath + TempFileName);
                Response.BinaryWrite(FileBuffer);
            }
        }



        protected void LBtnSProImage_Click(object sender, EventArgs e)
        {

            string _TempURL = "";
            string _ConvertReadableURL = "";
            string TempFileName = LBtnSProImage.Text;
            string FilePath = G_AttFilePath;
            //string FilePath = Server.MapPath("~/Files/"); //// last time, we always load image from this path G_AttFilePath = "C:\\www\\TRIMULIA_HRMS\\Files\\";
            WebClient User = new WebClient();
            Byte[] FileBuffer = User.DownloadData(FilePath + TempFileName);

            if (FileBuffer != null)
            {
                Response.ContentType = "image/jpeg";
                Response.AddHeader("content-length", FileBuffer.Length.ToString());
                Response.TransmitFile(FilePath + TempFileName);
                Response.BinaryWrite(FileBuffer);


                _TempURL = LoadAnyImage(txtEmpCode.Text, "Employee Image");

                if (_TempURL == "")
                {
                    EmpPImage.ImageUrl = "~/Image/DefaultProfile.png";
                }
                else
                {
                    string[] DataToSplit = _TempURL.Split('\\');
                    _ConvertReadableURL = "~/" + DataToSplit[3] + "/" + DataToSplit[4];
                    EmpPImage.ImageUrl = _ConvertReadableURL;

                }
            }
        }


        protected void txtEmpCode_TextChanged(object sender, EventArgs e)
        {

        }


        protected void ddlConEmpCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlConEmpCode.SelectedIndex == 0)
            {

            }
            else
            {
                //Loading Summary Information Page
                LoadMasterInfo(ddlConEmpCode.Text);

                LoadAddressInfo(ddlConEmpCode.Text);

                LoadPayrollInfo(ddlConEmpCode.Text);
            }
        }


        protected void ddlBU_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (ddlBU.SelectedIndex == 0)
            //{

            //}
            //else
            //{
            //    populateDepartment();
            //    populatePosition();
            //    populateEmpCodeddl();
            //    populateConEmpCodeddl();
            //    populateDirectCodeddl();
            //    populateOnBehalf();

            //}
        }


        protected void rbIsResign_SelectedIndexChanged(object sender, EventArgs e)
        {
            //If User click yes, only display the calendar
            if (rbIsResign.SelectedItem.ToString() == "Yes")
            {
                dtDateResign.Visible = true;
                //dtDateResign.CalendarDate = DateTime.Now;

                dtDateResign.Text = DateTime.Now.ToLocalTime().ToString("yyyy-MM-dd");
            }
            else
            {
                dtDateResign.Visible = false;
                //dtDateResign.CalendarDate = DateTime.Now;
                dtDateResign.Text = DateTime.Now.ToLocalTime().ToString("yyyy-MM-dd");
            }
        }


        protected void ddlSelectEmpCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtEmpCode.Text = "";
            chkSameAsAbove.Checked = false;

            if (ddlSelectEmpCode.Text == "Create New")
            {

                ClearAllTextWOEmpCode();


                LBtnUploadEmpInfoDoc.Text = "";
                LBtnResumeDoc.Text = "";
                LBtnEmpImage.Text = "";
                LBtnSProImage.Text = "";

                LBtnSVResume.Text = "";
                LBtnSVOfferLetterDoc.Text = "";
                LBtnSVEmpInfo.Text = "";
                LBtnOLP.Text = "";

                EmpPImage.ImageUrl = "~/Image/DefaultProfile.png";


            }
            else
            {
                LBtnUploadEmpInfoDoc.Text = "";
                LBtnResumeDoc.Text = "";
                LBtnEmpImage.Text = "";
                LBtnSProImage.Text = "";
                LBtnSVResume.Text = "";
                LBtnSVOfferLetterDoc.Text = "";
                LBtnSVEmpInfo.Text = "";
                LBtnOLP.Text = "";
                string _TempURL = "";
                string _ConvertReadableURL = "";

                trtxtEmpCode.Visible = true;

                txtEmpCode.Text = ddlSelectEmpCode.Text;
                LoadExisitingMasterInfo(txtEmpCode.Text);
                LoadOfferLetterLink(txtEmpCode.Text, "Employee Offer Letter");
                LoadEmpInfLink(txtEmpCode.Text, "Employee Information");
                LoadResumeLink(txtEmpCode.Text, "Employee Resume");


                _TempURL = LoadAnyImage(txtEmpCode.Text, "Employee Image");

                if (_TempURL == "")
                {
                    EmpPImage.ImageUrl = "~/Image/DefaultProfile.png";
                }
                else
                {
                    //C:\\www\\TRIMULIA_HRMS\\Files\\ ColourPOP2.JPG--->Only Emp Image upload use G_Attfilepath
                    //D:\\HR Attendance System\\CUBIC_HRMS v1.0.35 21042020\\CUBIC_HRMS\\Files\\ColourPOP2.JPG
                    ////this project run at D.
                    ///now using server path ~/ symbol
                    string[] DataToSplit = _TempURL.Split('\\');

                    ////this to get full path, for testing purpose only
                    ////_ConvertReadableURL = DataToSplit[0] + "/" + DataToSplit[1] + "/" + DataToSplit[2] + "/" + DataToSplit[3] + "/" + DataToSplit[4];


                    _ConvertReadableURL = "~/" + DataToSplit[3] + "/" + DataToSplit[4];
                    EmpPImage.ImageUrl = _ConvertReadableURL;
                }

            }
        }

        protected void ddlSelectDSuperior_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlSelectDSuperior.SelectedIndex == 0)
            {
                txtDirectSuperiorName.Text = "";
            }
            else
            {
                GetEmpNameSuperior(ddlSelectDSuperior.Text);
            }
        }

        public void GetEmpNameSuperior(string _ddlSelectDSuperior)
        {

            SqlConnection Conn = new SqlConnection(ConnectionString);

            string Query = "SELECT [EMP_NICK_NAME]";
            Query = Query + "FROM [dbo].[M_EMP_MASTER] ";
            Query = Query + "WHERE EMP_CODE = '" + _ddlSelectDSuperior + "' ";

            SqlCommand cmdDataBase = new SqlCommand(Query, Conn);

            SqlDataReader myReader;

            try
            {
                GF_CheckConnectionStatus(Conn);
                Conn.Open();
                myReader = cmdDataBase.ExecuteReader();

                while (myReader.Read())
                {
                    txtDirectSuperiorName.Text = (myReader["EMP_NICK_NAME"].ToString());
                }
            }
            catch (Exception ex)
            {
                string message = ex.Message.ToString();
                MessageBoxShow(message);
            }
            finally
            {
                Conn.Close();
                Conn.Dispose();
            }
        }


        protected void ddlSelectOnBehalf_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlSelectOnBehalf.SelectedIndex == 0)
            {
                txtOnBehalfName.Text = "'";
            }
            else
            {
                GetEmpNameOnBehalf(ddlSelectOnBehalf.Text);
            }
        }

        public void GetEmpNameOnBehalf(string _ddlSelectOnBehalf)
        {

            SqlConnection Conn = new SqlConnection(ConnectionString);

            string Query = "SELECT [EMP_NICK_NAME]";
            Query = Query + "FROM [dbo].[M_EMP_MASTER] ";
            Query = Query + "WHERE EMP_CODE = '" + _ddlSelectOnBehalf + "' ";

            SqlCommand cmdDataBase = new SqlCommand(Query, Conn);

            SqlDataReader myReader;

            try
            {
                GF_CheckConnectionStatus(Conn);
                Conn.Open();
                myReader = cmdDataBase.ExecuteReader();

                while (myReader.Read())
                {
                    txtOnBehalfName.Text = (myReader["EMP_NICK_NAME"].ToString());
                }
            }
            catch (Exception ex)
            {
                string message = ex.Message.ToString();
                MessageBoxShow(message);
            }
            finally
            {
                Conn.Close();
                Conn.Dispose();
            }
        }


        protected void ddlBankCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtBankName.Text = F_GetBankName(ddlBankCode.Text);
        }
    }
}