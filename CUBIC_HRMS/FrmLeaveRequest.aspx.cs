using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using static CUBIC_HRMS.GlobalVariable;
using CUBIC_HRMS;
using System.IO;
using System.Net;
using System.Collections;
using System.Text;
using System.Drawing;


namespace CUBIC_HRMS
{
    public partial class FrmLeaveRequest : System.Web.UI.Page
    {
        public double G_LeaveAvailable = 0;
        public double G_TotalApplyDay = 0;
        public double G_LeaveDay = 0;
        public double G_LeavePending = 0;
        public double G_LeaveApproved = 0;
        public string G_LeaveCode = "";

        // ** use for get data from URL
        public static string ParamStatus = "";
        public static string ParamTO = "";

        public static string G_Gender = "M";
        public static string G_IsLocal = "N";
        public static string G_DirectSupName = "";

        public static string G_CheckTransNoStatus = "";
        public static bool G_LeaveDuplicate = false;
        private ArrayList G_arrDateTable = new ArrayList();// Recommended


        // ** STATUS REMARK
        // ** R = Reject
        // ** P = Pending
        // ** A = Approve
        // ** X = Cancel

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {
                //dtFrom.CalendarDate = DateTime.Now;
                //dtTo.CalendarDate = DateTime.Now;

                dtFrom.Text = DateTime.Now.ToLocalTime().ToString("yyyy-MM-dd");
                dtTo.Text = DateTime.Now.ToLocalTime().ToString("yyyy-MM-dd");
                FillTransactionNo(CUBIC_HRMS.GlobalVariable.G_UserLogin);
                populateLeaveType();

                GetFromURL();
                BindGrid();
            }
            else
            {
                DateCount();
            }
        }


        private void populateLeaveType()
        {
            //Verify the employee to show the correct BU Code
            GetGenderFromMaster();

            SqlConnection Conn = new SqlConnection(CUBIC_HRMS.GlobalVariable.ConnectionString);
            SqlDataAdapter da = new SqlDataAdapter();
            GF_CheckConnectionStatus(Conn);
            Conn.Open();

            try
            {
                if (G_Gender == "M")
                {
                    if (G_IsLocal == "Y")
                    {
                        da = new SqlDataAdapter("SELECT [LEAVE_NAME] FROM [dbo].[M_LEAVE_TYPE] WHERE LEAVE_BU = '" + CUBIC_HRMS.GlobalVariable.G_User_BU + "' AND [LEAVE_STATUS]='Y' AND LEAVE_CODE NOT IN ('MAL','ALF')", Conn);
                    }
                    else
                    {
                        da = new SqlDataAdapter("SELECT [LEAVE_NAME] FROM [dbo].[M_LEAVE_TYPE] WHERE LEAVE_BU = '" + CUBIC_HRMS.GlobalVariable.G_User_BU + "' AND [LEAVE_STATUS]='Y' AND LEAVE_CODE NOT IN ('MAL','ALL')", Conn);
                    }
                }
                else
                {
                    da = new SqlDataAdapter("SELECT [LEAVE_NAME] FROM [dbo].[M_LEAVE_TYPE] WHERE LEAVE_BU = '" + CUBIC_HRMS.GlobalVariable.G_User_BU + "' AND [LEAVE_STATUS]='Y' ", Conn);
                    if (G_IsLocal == "Y")
                    {
                        da = new SqlDataAdapter("SELECT [LEAVE_NAME] FROM [dbo].[M_LEAVE_TYPE] WHERE LEAVE_BU = '" + CUBIC_HRMS.GlobalVariable.G_User_BU + "' AND [LEAVE_STATUS]='Y' AND LEAVE_CODE NOT IN ('ALF')", Conn);
                    }
                    else
                    {
                        da = new SqlDataAdapter("SELECT [LEAVE_NAME] FROM [dbo].[M_LEAVE_TYPE] WHERE LEAVE_BU = '" + CUBIC_HRMS.GlobalVariable.G_User_BU + "' AND [LEAVE_STATUS]='Y' AND LEAVE_CODE NOT IN ('ALL')", Conn);
                    }
                }
                DataTable dt = new DataTable();

                this.ddlLeaveType.Items.Clear();
                this.ddlLeaveType.SelectedValue = null;
                da.Fill(dt);
                ddlLeaveType.DataSource = dt;
                ddlLeaveType.DataBind();
                ddlLeaveType.DataTextField = "LEAVE_NAME";
                ddlLeaveType.DataValueField = "LEAVE_NAME";
                ddlLeaveType.DataBind();
                // Then add your first item
                ddlLeaveType.Items.Insert(0, "- Select -");

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


        private void PopulateLeaveDetails(string _TransNo)
        {
            SqlConnection Conn = new SqlConnection(ConnectionString);

            string Query = "SELECT A.[LEAVEH_NO],[LEAVED_DATE_FROM], [LEAVED_DATE_TO],[LEAVED_FULLDAYORNOT],[LEAVED_TOTAL_APPLY_DAY], B.[LEAVED_LEAVE_DAY], [LEAVED_REMARK], A.[LEAVED_STATUS],LEAVE_NAME";
            Query = Query + " FROM [dbo].[T_EMP_LEAVE_HDR] A, [dbo].[T_EMP_LEAVE_DET] B, [M_LEAVE_TYPE] C ";
            Query = Query + " WHERE A.[LEAVEH_NO] = B.[LEAVED_NO] AND B.[LEAVED_NO] = '" + _TransNo + "' ";
            Query = Query + " AND A.[LEAVEH_CODE]=c.[LEAVE_CODE]";

            SqlCommand cmdDataBase = new SqlCommand(Query, Conn);

            SqlDataReader myReader;


            try
            {
                GF_CheckConnectionStatus(Conn);

                Conn.Open();

                myReader = cmdDataBase.ExecuteReader();

                while (myReader.Read())
                {
                    txtLeaveCodeKeep.Text = (myReader["LEAVE_CODE"].ToString());
                    ddlLeaveType.Text = (myReader["LEAVE_NAME"].ToString());

                    ddlHalfDay.Text = (myReader["LEAVED_FULLDAYORNOT"].ToString());


                    DateTime LeaveDateFrom = Convert.ToDateTime(myReader["LEAVED_DATE_FROM"]);
                    dtFrom.Text = LeaveDateFrom.ToString("dd/MMM/yyyy");

                    //dtFrom.CalendarDate = Convert.ToDateTime(myReader["LEAVE_DATE_FROM"]);

                    //string DateFrom = (myReader["LEAVE_DATE_FROM"].ToString());
                    //dtFrom.CalendarDate = Convert.ToDateTime(DateFrom);

                    //dtTo.CalendarDate = Convert.ToDateTime(myReader["LEAVE_DATE_TO"]);

                    DateTime LeaveDateTo = Convert.ToDateTime(myReader["LEAVED_DATE_TO"]);
                    dtTo.Text = LeaveDateTo.ToString("dd/MMM/yyyy");

                    txtDayApplied.Text = (myReader["LEAVED_TOTAL_APPLY_DAY"].ToString());
                    txtLeaveBalance.Text = (myReader["LEAVED_LEAVE_DAY"].ToString());
                    txtOtherDet.Text = myReader["LEAVED_REMARK"].ToString();
                    txtStatus.Text = myReader["LEAVED_STATUS"].ToString();
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

        public void GetFromURL()
        {
            if (Request.QueryString["Action"] != null)
            {
                ParamStatus = Request.QueryString["Action"];
            }
            else
            {
                btnSendRequest.Enabled = true;
                ddlLeaveType.Enabled = true;

                populateLeaveType();

                ddlHalfDay.Enabled = true;
                txtOtherDet.Enabled = true;
                ParamStatus = "";
                dtFrom.Enabled = true;
                dtTo.Enabled = true;
            }

            if (ParamStatus == "View")
            {
                btnSendRequest.Visible = false;
                ddlLeaveType.Enabled = false;
                ddlHalfDay.Enabled = false;
                txtOtherDet.Enabled = false;
                btnCancel.Visible = false;
                //dtFrom.Enabled = false;
                //dtTo.Enabled = false;
                tr_Transactionddl.Visible = false;
                ddlTransactionNo.Text = Request.QueryString["TRANSACTION_NO"];
            }
            if (ParamStatus == "Cancel")
            {
                ParamTO = Request.QueryString["TRANSACTION_NO"];
                txtLoadTransNo.Text = ParamTO;
                tr_Transactionddl.Visible = false;
                ddlTransactionNo.Text = Request.QueryString["TRANSACTION_NO"];

                PopulateLeaveDetails(txtLoadTransNo.Text);

                btnSendRequest.Visible = false;
                ddlLeaveType.Enabled = false;
                ddlHalfDay.Enabled = false;
                txtOtherDet.Enabled = false;
                btnCancel.Visible = true;


                if (txtStatus.Text == "Cancel")
                {
                    btnCancel.Visible = false;
                }
                else

                {
                    btnCancel.Visible = true;
                }
            }
            else if (ParamStatus == "" || ParamStatus == null)
            {
                btnSendRequest.Enabled = true;
                ddlLeaveType.Enabled = true;

                populateLeaveType();

                ddlHalfDay.Enabled = true;

                txtOtherDet.Enabled = true;
                txtOtherDet.Enabled = true;

                dtFrom.Enabled = true;
                dtTo.Enabled = true;
            }

            if (Request.QueryString["TRANSACTION_NO"] != null)
            {
                //ClearTextBox();
                ParamTO = Request.QueryString["TRANSACTION_NO"];
                txtLoadTransNo.Text = ParamTO;

                PopulateLeaveDetails(txtLoadTransNo.Text);
                LoadAttachment(ParamTO, "Leave Request");

                //BindGrid(txtLoadTransNo.Text);
            }
        }


        public void FillTransactionNo(String _EmpCode)
        {

            SqlConnection Conn = new SqlConnection(CUBIC_HRMS.GlobalVariable.ConnectionString);
            GF_CheckConnectionStatus(Conn);
            Conn.Open();

            try
            {
                string SQLCommand = "SELECT [LEAVEH_NO] FROM [dbo].[T_EMP_LEAVE_HDR] ";
                SQLCommand = SQLCommand + "WHERE [LEAVEH_EMP_CODE]='" + CUBIC_HRMS.GlobalVariable.G_UserLogin + "' ";
                SQLCommand = SQLCommand + "AND [LEAVEH_STATUS] = 'R' ";
                SQLCommand = SQLCommand + "ORDER BY [LEAVEH_MODIFY_DATE] DESC";

                SqlDataAdapter adpt = new SqlDataAdapter(SQLCommand, Conn);
                DataTable dt = new DataTable();
                adpt.Fill(dt);
                ddlTransactionNo.DataSource = dt;
                ddlTransactionNo.DataBind();
                ddlTransactionNo.DataTextField = "LEAVEH_NO";
                ddlTransactionNo.DataValueField = "LEAVEH_NO";
                ddlTransactionNo.DataBind();
                ddlTransactionNo.Items.Insert(0, "-- Create New --");
            }
            catch (Exception ex)
            {
                string TempAudit = ex.ToString().Replace("'", "");
                GF_InsertAuditLog("-", "Catch Error", "FrmLeaveRequest", TempAudit, "FillTransactionNo");
            }
            finally
            {
                Conn.Dispose();
                Conn.Close();
            }
        }


        private string LoadAttachment(string _TransNo, string _FileType)
        {
            ////Load Out Image base on FILE TYPE  pass in. in database name : ATT_TYPE
            string TempTransNO = _TransNo;
            string FileType = _FileType;
            string _TempImageRemark = "";

            SqlConnection Conn = new SqlConnection(CUBIC_HRMS.GlobalVariable.ConnectionString);
            GF_CheckConnectionStatus(Conn);
            Conn.Open();

            try
            {
                string SQLCommand = "SELECT [ATT_FILE_REMARK] FROM T_EMP_ATT WHERE TRANSACTION_NO = '" + _TransNo + "' AND ATT_TYPE = '" + FileType + "' ";
                SqlCommand cmdDataBase = new SqlCommand(SQLCommand, Conn);
                SqlDataReader myReader;
                myReader = cmdDataBase.ExecuteReader();

                if (myReader.HasRows)
                {
                    while (myReader.Read())
                    {
                        _TempImageRemark = myReader["ATT_FILE_REMARK"].ToString();
                        LBtnFileAtt.Text = _TempImageRemark;
                    }
                }
                else
                {
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



        private void VerifyIsTransNoPending(string _TransNo)
        {
            SqlConnection Conn = new SqlConnection(CUBIC_HRMS.GlobalVariable.ConnectionString);
            GF_CheckConnectionStatus(Conn);
            Conn.Open();

            try
            {
                //string LeaveType = ddlLeaveType.Text;

                string SQLCommand = "SELECT [LEAVEH_STATUS] FROM [dbo].[T_EMP_LEAVE_HDR] WHERE [LEAVEH_NO]= '" + _TransNo + "'";

                SqlCommand cmdDataBase = new SqlCommand(SQLCommand, Conn);
                SqlDataReader myReader;
                myReader = cmdDataBase.ExecuteReader();
                if (myReader.HasRows)
                {
                    while (myReader.Read())
                    {
                        G_CheckTransNoStatus = (myReader["LEAVEH_STATUS"].ToString());
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


        private void GetGenderFromMaster()
        {
            //string _TempReturnResult = "";
            SqlConnection Conn = new SqlConnection(CUBIC_HRMS.GlobalVariable.ConnectionString);
            GF_CheckConnectionStatus(Conn);
            Conn.Open();

            //try
            //{
                //string LeaveType = ddlLeaveType.Text;

                string SQLCommand = "SELECT [EMP_GENDER],[EMP_ISLOCAL],[EMP_DIRECT_SUPNAME] FROM [dbo].[M_EMP_MASTER] WHERE [EMP_CODE]= '" + CUBIC_HRMS.GlobalVariable.G_UserLogin + "'";

                SqlCommand cmdDataBase = new SqlCommand(SQLCommand, Conn);
                SqlDataReader myReader;
                myReader = cmdDataBase.ExecuteReader();
                if (myReader.HasRows)
                {
                    while (myReader.Read())
                    {
                        G_Gender = (myReader["EMP_GENDER"].ToString());
                        G_IsLocal = (myReader["EMP_ISLOCAL"].ToString());
                        G_DirectSupName = (myReader["EMP_DIRECT_SUPNAME"].ToString());
                    }
                }
            //}
            //catch (Exception ex)
            //{
            //    //ScriptManager.RegisterStartupScript(this, this.GetType(), "Message", "alert('Error occured : " + ex.Message.ToString() + "');", true);
            //    string message = ex.Message.ToString();
            //    MessageBoxShow(message);
            //}
            //finally
            //{
            //    Conn.Close();
            //    Conn.Dispose();
            //}

            //return _TempReturnResult;
        }

        private void GetLeaveCodeFrmDB(string _LeaveType)
        {
            SqlConnection Conn = new SqlConnection(CUBIC_HRMS.GlobalVariable.ConnectionString);
            GF_CheckConnectionStatus(Conn);
            Conn.Open();

            try
            {
                string LeaveType = ddlLeaveType.Text;

                string SQLCommand = "select LEAVE_CODE from [dbo].[M_LEAVE_TYPE] where [LEAVE_NAME]= '" + LeaveType + "'";

                SqlCommand cmdDataBase = new SqlCommand(SQLCommand, Conn);
                SqlDataReader myReader;
                myReader = cmdDataBase.ExecuteReader();
                if (myReader.HasRows)
                {
                    while (myReader.Read())
                    {
                        G_LeaveCode = (myReader["LEAVE_CODE"].ToString());
                        txtLeaveCodeKeep.Text = (myReader["LEAVE_CODE"].ToString());
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



        //Based on what type of leave
        private void GetLeaveDayFrmDB(string _LeaveCode, string _EmpCode)
        {
            //String EmpCode = CUBIC_HRMS.GlobalVariable.G_UserLogin;

            SqlConnection Conn = new SqlConnection(CUBIC_HRMS.GlobalVariable.ConnectionString);
            GF_CheckConnectionStatus(Conn);
            Conn.Open();

            try
            {
                //string LeaveType = ddlLeaveType.Text;

                string SQLCommand = "SELECT SUM([LEAVE_DAY]+[LEAVE_EXTRA]) AS TOTAL_DAY FROM [dbo].[T_EMP_LEAVE_BAL] WHERE [EMP_CODE]= '" + _EmpCode + "' AND  [LEAVE_CODE]= '" + _LeaveCode + "'";

                SqlCommand cmdDataBase = new SqlCommand(SQLCommand, Conn);
                SqlDataReader myReader;
                myReader = cmdDataBase.ExecuteReader();
                if (myReader.HasRows)
                {
                    while (myReader.Read())
                    {
                        //G_LeaveDay = Convert.ToDouble(myReader["LEAVE_DAY"]);
                        if (myReader["TOTAL_DAY"] is DBNull)
                        {
                            G_LeaveDay = 0;
                        }
                        else
                        {
                            G_LeaveDay = Convert.ToDouble(myReader["TOTAL_DAY"]);
                        }
                    }
                }
                else
                {
                    G_LeaveDay = 0;
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


        //Get Pending Status Under same Emp Code
        private void GetPendingLeaveFrmDb(string _LeaveCode, string _EmpCode)
        {
            String EmpCode = CUBIC_HRMS.GlobalVariable.G_UserLogin;

            SqlConnection Conn = new SqlConnection(CUBIC_HRMS.GlobalVariable.ConnectionString);
            GF_CheckConnectionStatus(Conn);
            Conn.Open();

            try
            {
                //string LeaveType = ddlLeaveType.Text;

                //string SQLCommand = "SELECT COUNT(*) as TOTALPENDING from [dbo].[T_EMP_LEAVE_HDR] where [EMP_CODE]= '" + _EmpCode + "' AND STATUS = 'Pending' AND LEAVE_CODE = '" + _LeaveCode + "'";
                String SQLCommand = " SELECT sum(B.[LEAVED_TOTAL_APPLY_DAY]) AS TOTAL_PENDING ";
                SQLCommand = SQLCommand + " FROM [dbo].[T_EMP_LEAVE_HDR] A, [dbo].[T_EMP_LEAVE_DET] B ";
                SQLCommand = SQLCommand + " WHERE A.[LEAVEH_NO]=B.[LEAVED_NO]";
                SQLCommand = SQLCommand + "AND A.[LEAVEH_STATUS] =  'P' AND [LEAVEH_CODE] = '" + _LeaveCode + "' ";
                SQLCommand = SQLCommand + "AND [LEAVEH_EMP_CODE] =  '" + _EmpCode + "' ";


                SqlCommand cmdDataBase = new SqlCommand(SQLCommand, Conn);
                SqlDataReader myReader;

                myReader = cmdDataBase.ExecuteReader();
                if (myReader.HasRows)
                {
                    while (myReader.Read())
                    {
                        if (myReader["TOTAL_PENDING"] is DBNull)
                        {
                            G_LeavePending = 0;
                        }
                        else
                        {
                            G_LeavePending = Convert.ToDouble(myReader["TOTAL_PENDING"]);
                        }
                    }
                }
                else
                {
                    G_LeavePending = 0;
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


        //Get Pending Status Under same Emp Code
        private void GetApprovedLeaveFrmDb(string _LeaveCode, string _EmpCode)
        {
            //String EmpCode = CUBIC_HRMS.GlobalVariable.G_UserLogin;

            SqlConnection Conn = new SqlConnection(CUBIC_HRMS.GlobalVariable.ConnectionString);
            GF_CheckConnectionStatus(Conn);
            Conn.Open();

            //try
            //{

            //string SQLCommand = "select count(*) as TOTALAPPROVED from [dbo].[T_EMP_LEAVE_HDR] where [EMP_CODE]= '" + _EmpCode + "' AND STATUS = 'Approved' AND LEAVE_CODE = '" + _LeaveCode + "'";
            String SQLCommand = " SELECT SUM(B.[LEAVED_TOTAL_APPLY_DAY]) AS TOTAL_APPROVED ";
            SQLCommand = SQLCommand + " FROM [dbo].[T_EMP_LEAVE_HDR] A, [dbo].[T_EMP_LEAVE_DET] B ";
            SQLCommand = SQLCommand + " WHERE A.[LEAVEH_NO]=B.[LEAVED_NO] ";
            SQLCommand = SQLCommand + "AND A.[LEAVEH_STATUS] =  'A'  AND [LEAVEH_CODE] = '" + _LeaveCode + "' ";
            SQLCommand = SQLCommand + "AND [LEAVEH_EMP_CODE] =  '" + _EmpCode + "' ";

            SqlCommand cmdDataBase = new SqlCommand(SQLCommand, Conn);
            SqlDataReader myReader;
            myReader = cmdDataBase.ExecuteReader();
            if (myReader.HasRows)
            {
                while (myReader.Read())
                {
                    if (myReader["TOTAL_APPROVED"] is DBNull)
                    {
                        G_LeaveApproved = 0;
                    }
                    else
                    {
                        G_LeaveApproved = Convert.ToDouble(myReader["TOTAL_APPROVED"]);
                    }
                }
            }
            else
            {
                G_LeaveApproved = 0;
            }
            //}
            //catch (Exception ex)
            //{
            //    string message = ex.Message.ToString();
            //    MessageBoxShow(message);
            //}
            //finally
            //{
            //    Conn.Close();
            //    Conn.Dispose();
            //}


        }


        protected void UploadFile(string _TempFileType, string _TempTransNo)
        {
            string filetype1 = _TempFileType;

            //upload cannot pun in panel
            string FolderPath = Server.MapPath("~/Files/"); ; ////Server.MapPath("~/Files/");
                                                              //Check whether Directory (Folder) exists.
            if (!Directory.Exists(FolderPath))
            {
                //If Directory (Folder) does not exists. Create it.
                Directory.CreateDirectory(FolderPath);
            }

            bool HasFile = FileUpload.HasFile;

            if (File.Exists(FolderPath + FileUpload.FileName))
            {
                File.Delete(FolderPath + FileUpload.FileName);
            }

            if (FileUpload.HasFile)
            {
                //try
                //{

                if (FileUpload.PostedFile.ContentType.ToLower() == "application/pdf")
                {

                    string filename = Path.GetFileName(FileUpload.FileName);
                    FileUpload.SaveAs(FolderPath + filename);

                    //upload to database
                    //UpdateUploadAttData(1, CUBIC_HRMS.GlobalVariable.G_UserLogin, FolderPath + filename, filename, filetype1);
                    UpdateUploadAttData(1, _TempTransNo, FolderPath + filename, filename, filetype1);

                    //LoadAttLink(CUBIC_HRMS.GlobalVariable.G_UserLogin, "Leave Request");
                    LoadAttLink(_TempTransNo, "Leave Request");
                }
                else
                {
                    //this not really can use, i m use the validation summary instead
                    MessageBoxUpdatePanel("System only accept pdf format. ");
                }
                //}
                //catch (Exception ex)
                //{
                //    MessageBoxShow("Upload status: The file could not be uploaded. The following error occured: ");
                //}
            }
            else
            {
                MessageBoxUpdatePanel("Upload Successful: File uploaded!");
            }
        }


        private string LoadAttLink(string _TempEmpCode, string _FileType)
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
                string SQLCommand = "SELECT [ATT_FILE_REMARK] FROM T_EMP_ATT WHERE EMP_CODE = '" + TempEmpCode + "' AND ATT_TYPE = '" + FileType + "' ";
                SqlCommand cmdDataBase = new SqlCommand(SQLCommand, Conn);
                SqlDataReader myReader;
                myReader = cmdDataBase.ExecuteReader();

                if (myReader.HasRows)
                {
                    while (myReader.Read())
                    {
                        //_TempImageURL = myReader["ATT_FILE_PATH"].ToString();
                        _TempImageRemark = myReader["ATT_FILE_REMARK"].ToString();
                        LBtnFileAtt.Text = _TempImageRemark;
                        //LBtnSVPassDoc.Text = _TempImageRemark;
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


        private void UpdateUploadAttData(int AttachmentNo, string _TempTransNo, string _TempFilePath, string _TempFileRemark, string _FileType)
        //private void UpdateUploadAttData(int AttachmentNo, string _TempEmpCode, string _TempFilePath, string _TempFileRemark, string _FileType)
        {
            //string TempEmpCode = _TempEmpCode;
            string TempTransNo = _TempTransNo;
            string TempFilePath1 = "-";
            string TempFileRemark1 = "-";
            string FileType = _FileType;

            if (AttachmentNo == 1)
            {
                TempFilePath1 = _TempFilePath;
                TempFileRemark1 = _TempFileRemark;
            }

            SqlConnection Conn = new SqlConnection(CUBIC_HRMS.GlobalVariable.ConnectionString);

            GF_CheckConnectionStatus(Conn);
            Conn.Open();

            try
            {
                //    DO INSERT STATMENT HERE
                InsertFile(TempTransNo, TempFilePath1, TempFileRemark1, FileType);
                //}
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


        //private void UpdateFile(string _TempEmpCode, string _TempFilePath, string _TempFileRemark, string _FileType)
        //{
        //    string TempEmpCode = _TempEmpCode;

        //    string TempFilePath1 = "-";
        //    string TempFileRemark1 = "-";
        //    string FileType = _FileType;

        //    TempFilePath1 = _TempFilePath;
        //    TempFileRemark1 = _TempFileRemark;

        //    SqlConnection Conn = new SqlConnection(CUBIC_HRMS.GlobalVariable.ConnectionString);
        //    GF_CheckConnectionStatus(Conn);
        //    Conn.Open();

        //    if (Page.IsValid)
        //    {
        //        try
        //        {
        //            //DO UPDATE STATMENT HERE
        //            string SQLInsertHeader = "UPDATE [dbo].[T_EMP_ATT] SET ";
        //            SQLInsertHeader = SQLInsertHeader + "[ATT_TYPE]= '" + FileType + "',";
        //            SQLInsertHeader = SQLInsertHeader + "[ATT_FILE_PATH] = '" + TempFilePath1 + "',";
        //            SQLInsertHeader = SQLInsertHeader + "[ATT_FILE_REMARK]= '" + TempFileRemark1 + "',";
        //            SQLInsertHeader = SQLInsertHeader + "[ATT_MODIFY_BY]= '" + CUBIC_HRMS.GlobalVariable.G_UserLogin + "',";
        //            SQLInsertHeader = SQLInsertHeader + "[ATT_MODIFY_DATE ]= getdate() ";
        //            SQLInsertHeader = SQLInsertHeader + "WHERE ";
        //            SQLInsertHeader = SQLInsertHeader + "EMP_CODE = '" + TempEmpCode + "' ";
        //            SQLInsertHeader = SQLInsertHeader + " AND ATT_TYPE = '" + FileType + "' ";

        //            var cmd2 = new SqlCommand(SQLInsertHeader, Conn);
        //            cmd2.ExecuteNonQuery();

        //        }
        //        catch (Exception ex)
        //        {
        //            string message = ex.Message.ToString();
        //            MessageBoxShow(message);
        //        }
        //        finally
        //        {
        //            Conn.Close();
        //            Conn.Dispose();
        //        }
        //    }
        //}

        private void InsertFile(string _TempTransNo, string _TempFilePath, string _TempFileRemark, string _FileType)
        //private void InsertFile(string _TempEmpCode, string _TempFilePath, string _TempFileRemark, string _FileType)
        {
            //string TempEmpCode = _TempEmpCode;
            string TempTransNo = _TempTransNo;

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
                    SQLInsertHeader2 = SQLInsertHeader2 + ",[ATT_NO]";
                    SQLInsertHeader2 = SQLInsertHeader2 + ",[ATT_TYPE]";
                    SQLInsertHeader2 = SQLInsertHeader2 + ",[ATT_FILE_PATH]";
                    SQLInsertHeader2 = SQLInsertHeader2 + ",[ATT_FILE_REMARK] ";
                    SQLInsertHeader2 = SQLInsertHeader2 + ",[ATT_CREATE_BY] ";
                    SQLInsertHeader2 = SQLInsertHeader2 + ",[ATT_CREATE_DATE] ";
                    SQLInsertHeader2 = SQLInsertHeader2 + ",[ATT_MODIFY_BY] ";
                    SQLInsertHeader2 = SQLInsertHeader2 + ",[ATT_MODIFY_DATE]) ";
                    SQLInsertHeader2 = SQLInsertHeader2 + "VALUES ";
                    SQLInsertHeader2 = SQLInsertHeader2 + "('" + CUBIC_HRMS.GlobalVariable.G_UserLogin + "' ";
                    SQLInsertHeader2 = SQLInsertHeader2 + ",'" + _TempTransNo + "' ";
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


        protected void btnSendRequest_Click(object sender, EventArgs e)
        {
            /* Check Records Exist in Wo*/
            SqlConnection Conn = new SqlConnection(CUBIC_HRMS.GlobalVariable.ConnectionString);
            GF_CheckConnectionStatus(Conn);
            Conn.Open();

            //try
            //{
            if (Page.IsValid)
            {
                // ** If date duplicate
                //CheckIfDuplicate();

                //if (G_LeaveDuplicate == true)
                //{
                //    string message = "There is same day had been applied before, please check again. ";
                //    MessageBoxUpdatePanel(message);
                //}
                //else if (Convert.ToDouble(txtDayApplied.Text) <= 0)
                //{
                //    string message = "Applied Day is 0 and invalid, please re-select your date from and to. ";
                //    MessageBoxUpdatePanel(message);
                //    //dtFrom.CalendarDate = DateTime.Now;
                //    //dtTo.CalendarDate = DateTime.Now;
                //    dtFrom.Text = DateTime.Now.ToLocalTime().ToString("yyyy-MM-dd");
                //    dtTo.Text = DateTime.Now.ToLocalTime().ToString("yyyy-MM-dd");
                //    txtDayApplied.Text = "0";
                //}
                //else if (Convert.ToDouble(txtLeaveBalance.Text) <= 0)
                //{
                //    string message = "Your Leave Balance is not enough for apply. please re-select your plan date.";
                //    MessageBoxUpdatePanel(message);
                //}
                //else if (Convert.ToDouble(txtDayApplied.Text) > Convert.ToDouble(txtLeaveBalance.Text))
                //{
                //    string message = "Your Leave Balance is not enough for apply. please re-select your plan date.";
                //    MessageBoxUpdatePanel(message);
                //    //dtFrom.CalendarDate = DateTime.Now;
                //    //dtTo.CalendarDate = DateTime.Now;
                //    //txtDayApplied.Text = "0";
                //}
                //else
                //{
                    // Need Generate New Leave Running No
                    string TempRunningNo = GF_GetRunningNumber(CUBIC_HRMS.GlobalVariable.G_User_BU, "L");

                    GF_UpdateRunningNumber(CUBIC_HRMS.GlobalVariable.G_User_BU, "L");


                    //Start saving into database
                    //Add into HDR
                    F_SaveIntoLeaveHDR(TempRunningNo);


                    // ** Update Sysrun

                    //Add into Details
                    F_SaveIntoLeaveDetails(TempRunningNo);

                    UploadFile("Leave Request", TempRunningNo);

                    BindGrid();


                    // **** Temporary Remove Send Email
                    //    #region "SEND EMAIL"
                    //    ////get employee approval and on behalf 
                    //    // ** SEND EMAIL
                    //    SqlConnection Conn1 = new SqlConnection(CUBIC_HRMS.GlobalVariable.ConnectionString);
                    //    GF_CheckConnectionStatus(Conn1);
                    //    Conn1.Open();

                    //    //try
                    //    //{
                    //    string SQLCommand1 = "-";
                    //    SQLCommand1 = "SELECT A.EMP_CODE, A.EMP_NICK_NAME, A.EMP_MAIL, ";
                    //    SQLCommand1 = SQLCommand1 + "B.EMP_NICK_NAME AS SUPERIOR_NICK_NAME, ";
                    //    SQLCommand1 = SQLCommand1 + "ISNULL(A.EMP_DIRECT_SUPERIOR,'-') AS EMPLOYEEDIRECT, B.EMP_MAIL AS SUPERIORMAIL, ";
                    //    SQLCommand1 = SQLCommand1 + "C.EMP_NICK_NAME AS ONBEHALF_NICK_NAME, ";
                    //    SQLCommand1 = SQLCommand1 + "ISNULL(A.EMP_APP_ONBEHALF,'-') AS ONBEHALF, C.EMP_MAIL AS ONBEHALFMAIL ";
                    //    SQLCommand1 = SQLCommand1 + "FROM[M_EMP_MASTER] A ";
                    //    SQLCommand1 = SQLCommand1 + "LEFT JOIN[M_EMP_MASTER] B ON B.EMP_CODE = A.EMP_DIRECT_SUPERIOR ";
                    //    SQLCommand1 = SQLCommand1 + "LEFT JOIN[M_EMP_MASTER] C ON C.EMP_CODE = A.EMP_APP_ONBEHALF ";
                    //    SQLCommand1 = SQLCommand1 + "WHERE A.EMP_CODE = '" + G_UserLogin + "'";


                    //    SqlCommand cmdDataBase1 = new SqlCommand(SQLCommand1, Conn1);
                    //    SqlDataReader myReader1;
                    //    myReader1 = cmdDataBase1.ExecuteReader();
                    //    if (myReader1.HasRows)
                    //    {
                    //        while (myReader1.Read())
                    //        {
                    //            ////here, we need check employee direct superior first.
                    //            ///and we need check employee direct superior eh email
                    //            ///need make sure got direct superior and this superior got email only can run the SendEmail Function
                    //            if (myReader1["EMPLOYEEDIRECT"].ToString() != "-" && myReader1["SUPERIORMAIL"].ToString() != "-")
                    //            {

                    //                //_TempType = Approval
                    //                //_TempType = Requester
                    //                //_TempLeaveStatus = Request
                    //                //_TempLeaveStatus = Approved
                    //                //_TempLeaveStatus = Rejected

                    //                ////SendEmail(string _TempRecipientMail, string _TempType, string _TempLeaveStatus, string _TempEmpCode, string _TempEmpNickName, string _TempLeaveNo, string _TempReceiverNickName)
                    //                ///so when we call this function, we will put the Superior Email Address into _TempRecipientMail
                    //                ///we can also pass in hardcode like 
                    //                /// SendEmail("hcnew@cubicsoftware.com.my", "Requester", "Request", G_UserLogin, myReader1["EMP_NICK_NAME"].ToString(), txtTransactionNo.Text);
                    //                SendEmail(myReader1["SUPERIORMAIL"].ToString(), "Requester", "Request", G_UserLogin, myReader1["EMP_NICK_NAME"].ToString(), TempRunningNo, myReader1["SUPERIOR_NICK_NAME"].ToString());
                    //            }


                    //            ////here, we need check employee on behalf eh ppl
                    //            ///and we need check employee behalf eh email
                    //            ///need make sure got behalf and this behalf got email only can run the SendEmail Function
                    //            if (myReader1["ONBEHALF"].ToString() != "-" && myReader1["ONBEHALFMAIL"].ToString() != "-")
                    //            {
                    //                SendEmail(myReader1["ONBEHALFMAIL"].ToString(), "Requester", "Request", G_UserLogin, myReader1["EMP_NICK_NAME"].ToString(), TempRunningNo, myReader1["ONBEHALF_NICK_NAME"].ToString());
                    //            }
                    //        }
                    //    }
                    //#endregion "SEND EMAIL"


                    // Display success message and clear the form.
                    string message = "Your Leave Apply Successfully.";
                    MessageBoxShow(message);

                    //ClearText();


                    //}
                    //catch (Exception ex)
                    //{
                    //    //ScriptManager.RegisterStartupScript(this, this.GetType(), "Message", "alert('Error occured : " + ex.Message.ToString() + "');", true);
                    //    string message = ex.Message.ToString();
                    //    MessageBoxShow(message);
                    //}
                    //finally
                    //{
                    //    Conn.Close();
                    //    Conn.Dispose();
                    //}


                //}
            }

            //}
            //catch (Exception ex)
            //{
            //string TempAudit = ex.ToString().Replace("'", "");
            //F_InsertAuditLog("-", "Catch Error", "FrmLeaveRequest", TempAudit, "btnSendRequest_Click");
            //}
            //finally
            //{
            //    Conn.Close();
            //    Conn.Dispose();

            //}
        }


        private void SendEmail(string _TempRecipientMail, string _TempType, string _TempLeaveStatus, string _TempEmpCode, string _TempEmpNickName, string _TempLeaveNo, string _TempReceiverNickName)
        {
            ////in this function, we need pass in all function 
            ////string _TempRecipientMail, string _TempType, string _TempLeaveStatus, string _TempEmpCode, string _TempEmpNickName, string _TempLeaveNo
            ///but most important is the _TempRecipientMail >> this is recipent email address

            //**********************************************************************
            //**********************************************************************
            //// from here, this email will send thru this mail server : mail.cubicsoftware.com.my
            /// and thru this email address hr@cubicsoftware.com.my
            ///  all this can be hardcode
            ///  then the recipent mail (string RecipientMail = _TempRecipientMail;) is the receiver email address , example if want send to me, then put hcnew@cubicsoftware.com.my
            ///  TempSubject, is what we can see while we receive the email
            ///  TempBodyMessage, is what we can see at the email content
            /// from here, i hard code the subject to "[Auto Generate From CUBIC_HRMS : LEAVE MANAGEMENT]";
            /// TempBodyMessage i put flexible


            //string TempDomain = "mail.cubicsoftware.com.my";  //next time need change to mail.trimulia.com
            //int OutgoingPort = 587;
            //string SenderMail = "hr@cubicsoftware.com.my"; //next time need change to it@trimulia.com
            //string SenderMailPassword = "@@HrCubic88@@"; //forgot hamik password. next time need change to IT@trimulia.com eh password


            string TempDomain = "mail.trimulia.com";  //next time need change to mail.trimulia.com
            int OutgoingPort = 587;
            string SenderMail = "it@trimulia.com"; //next time need change to it@trimulia.com
            string SenderMailPassword = "@@ITTrimulia@@"; //forgot hamik password. next time need change to IT@trimulia.com eh password


            string RecipientMail = _TempRecipientMail;
            string TempSubject = "[Auto Generate From CUBIC_HRMS : LEAVE MANAGEMENT] Transaction No: #" + _TempLeaveNo;
            //string TempBodyMessage = "-";
            StringBuilder sb = new StringBuilder();

            //**********************************************************************

            //_TempType = Approval
            //_TempType = Requester
            //_TempLeaveStatus = Approved
            //_TempLeaveStatus = Rejected

            if (_TempType == "Requester")
            {
                GetGenderFromMaster();

                //TempBodyMessage = "Action Required: Leave Application From " + System.Environment.NewLine + System.Environment.NewLine + "Employee Code : " + _TempEmpCode + System.Environment.NewLine + "Employee Name : " + _TempEmpNickName + System.Environment.NewLine + "Leave No : " + _TempLeaveNo ;
                sb.AppendLine("Dear <b>" + _TempReceiverNickName + "</b>,");
                sb.AppendLine("<br>");
                sb.AppendLine("<br>");
                sb.AppendLine("Leave Request has been submitted by <b> " + _TempEmpNickName + "</b> for your approval,");
                sb.AppendLine("<br>");
                sb.AppendLine("<br>");
                sb.AppendLine("Please click " + "<a href = http://trimulia_hr.com/login>Login</a>" + " to approve or reject.");
                sb.AppendLine("<br>");
                sb.AppendLine("<br>");
                sb.AppendLine("Regards,");
                sb.AppendLine("<br>");
                sb.AppendLine("<b>HR Management System</b>");
                sb.AppendLine("<br>");
                sb.AppendLine("<br>");
                //sb.AppendLine("Kindly " + "<a href=http://trimulia_hr.com/login>login</a>" + "to system to approve this leave request");

                sb.AppendLine("<br>");
                sb.Append("PLEASE DO NOT REPLY TO THIS MESSAGE AS IT IS FROM AN UNATTENDED MAILBOX. ANY REPLIES TO THIS EMAIL WILL NOT BE RESPONDED TO OR FORWARDED. THIS SERVICE IS USED FOR OUTGOING EMAILS ONLY AND CANNOT RESPOND TO INQUIRIES. ");

                //TempBodyMessage = sb.ToString();
            }
            //else if (_TempType == "Approval")
            //{
            //    TempBodyMessage = "Action Required: Leave No : " + _TempLeaveNo + "Your Leave Had Been : " + _TempLeaveStatus;
            //}


            //GlobalVariable.AutoGenerateMail(TempDomain, OutgoingPort, SenderMailPassword, SenderMail, RecipientMail, TempSubject, TempBodyMessage);
            //GlobalVariable.AutoGenerateMail(TempDomain, OutgoingPort, SenderMailPassword, SenderMail, RecipientMail, TempSubject, sb.ToString());

        }



        //Bind Data Grid
        private void GenerateDateTable(DateTime _StartDate, DateTime _EndDate)
        {
            Enumerable.Range(0, 1 + _EndDate.Subtract(_StartDate).Days).Select(offset => _StartDate.AddDays(offset)).ToArray();

            var dates = new List<DateTime>();

            for (var dt = _StartDate; dt <= _EndDate; dt = dt.AddDays(1))
            {
                dates.Add(dt);
                G_arrDateTable.Add(dt.ToShortDateString());
            }
        }


        private void CheckIfDuplicate()
        {

            //string DateFrom = dtFrom.CalendarDate.ToString();
            //string DateTo = dtTo.CalendarDate.ToString();

            string SQLCommand = "";
            DateTime DateCompare = DateTime.Today;
            string _TempEmployeeCode = CUBIC_HRMS.GlobalVariable.G_UserLogin;

            if (Page.IsValid)
            {

                DateTime DateFrom = Convert.ToDateTime(dtFrom.Text);
                DateTime DateTo = Convert.ToDateTime(dtTo.Text);

                GenerateDateTable(DateFrom, DateTo);

                for (int Outter = 0; Outter < G_arrDateTable.Count; Outter++)
                {

                    DateCompare = Convert.ToDateTime(G_arrDateTable[Outter]);
                    SqlConnection Conn = new SqlConnection(CUBIC_HRMS.GlobalVariable.ConnectionString);
                    GF_CheckConnectionStatus(Conn);
                    Conn.Open();

                    SQLCommand = "SELECT A.[LEAVEH_NO], [LEAVEH_EMP_CODE], [LEAVEH_CODE], A.[LEAVEH_STATUS], ";
                    SQLCommand = SQLCommand + "[LEAVED_DATE_FROM],[LEAVED_DATE_TO],[LEAVED_FULLDAYORNOT],[LEAVED_TOTAL_APPLY_DAY], ";
                    SQLCommand = SQLCommand + "[LEAVED_LEAVE_DAY],[LEAVED_REMARK],[LEAVED_APPROVED_REMARK],[LEAVE_APPROVED_BY],[LEAVE_APPROVED_DATE] ";
                    SQLCommand = SQLCommand + "FROM [dbo].[T_EMP_LEAVE_HDR] A, ";
                    SQLCommand = SQLCommand + "[dbo].[T_EMP_LEAVE_DET]  B ";
                    SQLCommand = SQLCommand + "WHERE A.[LEAVEH_NO] = B.[LEAVED_NO] ";
                    SQLCommand = SQLCommand + "AND A.[LEAVEH_EMP_CODE]= '" + _TempEmployeeCode + "' ";
                    //SQLCommand = SQLCommand + "AND [LEAVEH_CODE] = '" + txtLeaveCodeKeep.Text + "' ";
                    SQLCommand = SQLCommand + "AND A.[LEAVEH_STATUS] IN('P','A') ";

                    SQLCommand = SQLCommand + " AND LEAVED_DATE_FROM <= '" + DateCompare + "' AND LEAVED_DATE_TO >= '" + Convert.ToDateTime(DateCompare) + "' ";


                    SqlCommand cmdDataBase = new SqlCommand(SQLCommand, Conn);
                    SqlDataReader myReader;
                    myReader = cmdDataBase.ExecuteReader();
                    if (myReader.HasRows)
                    {
                        while (myReader.Read())
                        {
                            MessageBoxShow("Your data has found exist in database, please enter a new one. Transaction Exist : " + myReader["LEAVEH_NO"] + ".Date :" + DateCompare);
                            G_LeaveDuplicate = true;
                            break;
                        }

                    }
                    else
                    {
                        G_LeaveDuplicate = false;
                    }

                    Conn.Close();
                    Conn.Dispose();
                }
            }
        }


        private void F_SaveIntoLeaveHDR(string _TempRunningNo)
        {
            //Add new one in Leave HDR
            SqlConnection Conn = new SqlConnection(CUBIC_HRMS.GlobalVariable.ConnectionString);
            GF_CheckConnectionStatus(Conn);
            Conn.Open();


            //string LeaveType = ddlLeaveType.Text;
            string LeaveCode = txtLeaveCodeKeep.Text;
            string DateFrom = dtFrom.Text;
            string DateTo = dtTo.Text;
            string FullDayOrNot = ddlHalfDay.Text;
            string OtherDetails = txtOtherDet.Text.Trim();

            try
            {
                string SQLInsertHeader = "INSERT INTO [dbo].T_EMP_LEAVE_HDR";
                SQLInsertHeader = SQLInsertHeader + "( ";
                SQLInsertHeader = SQLInsertHeader + "[LEAVEH_BU]";
                SQLInsertHeader = SQLInsertHeader + ",[LEAVEH_NO]";
                SQLInsertHeader = SQLInsertHeader + ",[LEAVEH_EMP_CODE]";
                SQLInsertHeader = SQLInsertHeader + ",[LEAVEH_CODE]";
                SQLInsertHeader = SQLInsertHeader + ",[LEAVEH_STATUS]";
                SQLInsertHeader = SQLInsertHeader + ",[LEAVEH_CREATE_BY] ";
                SQLInsertHeader = SQLInsertHeader + ",[LEAVEH_CREATE_DATE]";
                SQLInsertHeader = SQLInsertHeader + ",[LEAVEH_MODIFY_BY]";
                SQLInsertHeader = SQLInsertHeader + ",[LEAVEH_MODIFY_DATE])";
                SQLInsertHeader = SQLInsertHeader + "VALUES ";
                SQLInsertHeader = SQLInsertHeader + "( ";
                SQLInsertHeader = SQLInsertHeader + "'" + CUBIC_HRMS.GlobalVariable.G_User_BU + "' ";
                SQLInsertHeader = SQLInsertHeader + ",'" + _TempRunningNo + "' ";
                SQLInsertHeader = SQLInsertHeader + ",'" + CUBIC_HRMS.GlobalVariable.G_UserLogin + "' ";
                SQLInsertHeader = SQLInsertHeader + ",'" + LeaveCode + "' ";
                //SQLInsertHeader = SQLInsertHeader + ",'Pending' ";
                SQLInsertHeader = SQLInsertHeader + ",'P' ";
                SQLInsertHeader = SQLInsertHeader + ",'" + CUBIC_HRMS.GlobalVariable.G_UserLogin + "' ";
                SQLInsertHeader = SQLInsertHeader + ",getdate() ";
                SQLInsertHeader = SQLInsertHeader + ",'" + CUBIC_HRMS.GlobalVariable.G_UserLogin + "' ";
                SQLInsertHeader = SQLInsertHeader + ",getdate() )";

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

        private void F_SaveIntoLeaveDetails(string _TempRunningNo)
        {
            //Add new one in Leave HDR
            SqlConnection Conn = new SqlConnection(CUBIC_HRMS.GlobalVariable.ConnectionString);
            GF_CheckConnectionStatus(Conn);
            Conn.Open();

            string DateFrom = dtFrom.Text;
            string DateTo = dtTo.Text;
            string FullDayOrNot = ddlHalfDay.Text;

            string OtherDetails = txtOtherDet.Text.Trim();

            try
            {
                string SQLInsertHeader = "INSERT INTO [dbo].[T_EMP_LEAVE_DET]";
                SQLInsertHeader = SQLInsertHeader + "([LEAVED_NO]";
                SQLInsertHeader = SQLInsertHeader + ",[LEAVED_BU]";
                SQLInsertHeader = SQLInsertHeader + ",[LEAVED_DATE_FROM]";
                SQLInsertHeader = SQLInsertHeader + ",[LEAVED_DATE_TO]";
                SQLInsertHeader = SQLInsertHeader + ",[LEAVED_FULLDAYORNOT]";
                SQLInsertHeader = SQLInsertHeader + ",[LEAVED_TOTAL_APPLY_DAY]";
                SQLInsertHeader = SQLInsertHeader + ",[LEAVED_LEAVE_DAY]";
                SQLInsertHeader = SQLInsertHeader + ",[LEAVED_REMARK]";

                SQLInsertHeader = SQLInsertHeader + ",[LEAVED_STATUS]";
                SQLInsertHeader = SQLInsertHeader + ",[LEAVE_APPROVED_BY]";
                SQLInsertHeader = SQLInsertHeader + ",[LEAVE_APPROVED_DATE])";
                SQLInsertHeader = SQLInsertHeader + "VALUES ";
                SQLInsertHeader = SQLInsertHeader + "('" + _TempRunningNo + "' ";
                SQLInsertHeader = SQLInsertHeader + ",'" + CUBIC_HRMS.GlobalVariable.G_User_BU + "' ";
                SQLInsertHeader = SQLInsertHeader + ",'" + DateFrom + "' ";
                SQLInsertHeader = SQLInsertHeader + ",'" + DateTo + "' ";
                SQLInsertHeader = SQLInsertHeader + ",'" + FullDayOrNot + "' ";
                SQLInsertHeader = SQLInsertHeader + ",'" + txtDayApplied.Text + "' ";
                SQLInsertHeader = SQLInsertHeader + ",'" + txtLeaveBalance.Text + "' ";
                SQLInsertHeader = SQLInsertHeader + ",'" + txtOtherDet.Text + "' ";


                SQLInsertHeader = SQLInsertHeader + ",'P' ";
                SQLInsertHeader = SQLInsertHeader + ",'" + CUBIC_HRMS.GlobalVariable.G_UserLogin + "' ";
                SQLInsertHeader = SQLInsertHeader + ",getdate() )";

                var cmd2 = new SqlCommand(SQLInsertHeader, Conn);
                cmd2.ExecuteNonQuery();

                //// Display success message and clear the form.
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




        protected void LBtnFileAtt_Click(object sender, EventArgs e)
        {
            string TempFileName = LBtnFileAtt.Text;
            string FilePath = Server.MapPath("~/Files/");
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


        public void MessageBoxShow(string AlarmMessage)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "My Alert", "alert('" + AlarmMessage + "');", true);
        }


        public void MessageBoxUpdatePanel(string AlarmMessage)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Message", "alert('" + AlarmMessage + "');", true);
        }


        protected void btnCancel_Click(object sender, EventArgs e)
        {
            //Update the Leave Request to Cancel

            SqlConnection Conn = new SqlConnection(CUBIC_HRMS.GlobalVariable.ConnectionString);
            GF_CheckConnectionStatus(Conn);
            Conn.Open();

            try
            {
                //Need check first the Status is Pending else write the message cannot cancel
                VerifyIsTransNoPending(txtLoadTransNo.Text);


                //|| G_CheckTransNoStatus == "Approved" || G_CheckTransNoStatus == "Rejected"
                if (G_CheckTransNoStatus.ToUpper() == "PENDING")
                {

                    string SQLUpdateHeader = "UPDATE [dbo].[T_EMP_LEAVE_HDR] ";
                    SQLUpdateHeader = SQLUpdateHeader + "SET ";
                    SQLUpdateHeader = SQLUpdateHeader + "[LEAVEH_STATUS] = 'Cancelled' ";
                    //SQLUpdateHeader = SQLUpdateHeader + "WHERE [TRANSACTION_NO] = '" + ParamTO + "' ";
                    SQLUpdateHeader = SQLUpdateHeader + "WHERE [LEAVEH_NO] = '" + txtLoadTransNo.Text + "' ";

                    var cmd2 = new SqlCommand(SQLUpdateHeader, Conn);
                    cmd2.ExecuteNonQuery();

                    //Later only find those which confirmation needed.
                    //MessageBoxShow("This Transaction has been cancel from database.");
                    string TransNo = txtLoadTransNo.Text;

                    if (Request.QueryString["Action"] == null)
                    {
                        MessageBoxUpdatePanel("Transaction  " + TransNo + "  will update  status as Cancelled in the system");

                        BindGrid();

                        ddlTransactionNo.SelectedIndex = 0;
                        populateLeaveType();

                        ddlHalfDay.SelectedIndex = -1;
                        txtLeaveBalance.Text = "0";
                        txtDayApplied.Text = "0";
                        txtOtherDet.Text = "";
                        LBtnFileAtt.Text = "";
                        txtLoadTransNo.Text = "";
                        txtLeaveCodeKeep.Text = "";

                        btnSendRequest.Visible = true;
                        btnCancel.Visible = false;

                        ddlLeaveType.Enabled = true;
                        dtFrom.Enabled = true;
                        dtTo.Enabled = true;

                        //dtFrom.CalendarDate = DateTime.Now;
                        //dtTo.CalendarDate = DateTime.Now;


                        dtFrom.Text = DateTime.Now.ToLocalTime().ToString("yyyy-MM-dd");
                        dtTo.Text = DateTime.Now.ToLocalTime().ToString("yyyy-MM-dd");
                        ddlHalfDay.Enabled = true;
                        txtOtherDet.Enabled = true;
                    }
                    else
                    {
                        Response.Redirect("MainPage.aspx");
                    }

                }
                else
                {
                    //cannot do cancelling in message
                    MessageBoxUpdatePanel("This Transaction already cancel before.");
                    btnSendRequest.Visible = true;
                    btnCancel.Visible = false;
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


        private void DateCount()
        {
            //DateTime DateFrom = dtFrom.CalendarDate;
            //DateTime DateTo = dtTo.CalendarDate.AddDays(1);


            DateTime DateFrom = Convert.ToDateTime(dtFrom.Text);
            DateTime DateTo = Convert.ToDateTime(dtTo.Text).AddDays(1);


            //DateTime firstPayDate = Convert.ToDateTime(myReader["PR_PAYREVIEW_DATE"]);
            //lblSVDate1stPayRev.Text = firstPayDate.ToString("dd/MMM/yyyy");

            Double Difference = (DateTo.Date - DateFrom.Date).Days;

            if (ddlHalfDay.SelectedIndex == 0)
            {
                txtDayApplied.Text = "0";
            }
            else if (ddlHalfDay.SelectedIndex == 1)
            {
                txtDayApplied.Text = Convert.ToString(Difference);
            }
            else
            {
                txtDayApplied.Text = Convert.ToString(Difference * 0.5);
            }
        }


        private void ClearText()
        {
            populateLeaveType();
            FillTransactionNo(CUBIC_HRMS.GlobalVariable.G_UserLogin);

            //dtFrom.CalendarDate = DateTime.Now;
            //dtTo.CalendarDate = DateTime.Now;

            dtFrom.Text = DateTime.Now.ToLocalTime().ToString("yyyy-MM-dd");
            dtTo.Text = DateTime.Now.ToLocalTime().ToString("yyyy-MM-dd");

            txtLeaveCodeKeep.Text = "";
            ddlHalfDay.SelectedIndex = -1;
            txtLeaveBalance.Text = "0";
            txtDayApplied.Text = "0";
            txtOtherDet.Text = "";
            txtStatus.Text = "";
            LBtnFileAtt.Text = "";
            txtLoadTransNo.Text = "";
        }


        protected void grdSummary_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdSummary.PageIndex = e.NewPageIndex;
            BindGrid();
        }


        private void BindGrid()
        {
            SqlConnection Conn = new SqlConnection(CUBIC_HRMS.GlobalVariable.ConnectionString);
            GF_CheckConnectionStatus(Conn);
            Conn.Open();

            SqlDataAdapter adp = new SqlDataAdapter();
            DataTable dt = new DataTable();
            try
            {
                string SQLCommand = "";

                SQLCommand = "SELECT A.[LEAVEH_NO],[LEAVEH_EMP_CODE],[LEAVEH_CODE],CONVERT(VARCHAR(12), ";
                SQLCommand = SQLCommand + "[LEAVED_DATE_FROM],106) AS [LEAVED_DATE_FROM], ";
                SQLCommand = SQLCommand + "CONVERT(VARCHAR(12), [LEAVED_DATE_TO],106) AS [LEAVED_DATE_TO], ";
                SQLCommand = SQLCommand + "[LEAVED_TOTAL_APPLY_DAY], [LEAVED_FULLDAYORNOT],[LEAVED_LEAVE_DAY],";
                SQLCommand = SQLCommand + "[LEAVED_STATUS], ";
                SQLCommand = SQLCommand + "LEAVE_APPROVED_BY,CONVERT(VARCHAR(12), LEAVE_APPROVED_DATE,106) AS LEAVE_APPROVED_DATE ";
                SQLCommand = SQLCommand + "FROM [dbo].[T_EMP_LEAVE_HDR] A,  [dbo].[T_EMP_LEAVE_DET] B ";
                SQLCommand = SQLCommand + "WHERE A.[LEAVEH_NO] = B.[LEAVED_NO] ";
                SQLCommand = SQLCommand + "AND A.LEAVEH_BU = B.LEAVED_BU ";
                SQLCommand = SQLCommand + "AND A.[LEAVEH_EMP_CODE]= '" + CUBIC_HRMS.GlobalVariable.G_UserLogin + "' ";
                SQLCommand = SQLCommand + "ORDER BY [LEAVEH_MODIFY_DATE] DESC ";

                adp = new SqlDataAdapter(SQLCommand, Conn);

                adp.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    //int counter = 1;

                    //foreach (DataRow row in dt.Rows)
                    //{
                    //    row[0] = counter;
                    //    counter = counter + 1;
                    //}

                    grdSummary.DataSource = dt;
                    grdSummary.DataBind();
                }
                else
                {
                    grdSummary.DataSource = null;
                    grdSummary.DataBind();
                }
            }
            catch (Exception ex)
            {
                string message = ex.Message.ToString();
                MessageBoxShow(message);
            }
            finally
            {
                dt.Clear();
                dt.Dispose();
                adp.Dispose();
                Conn.Close();
                Conn.Dispose();
            }
        }


        protected void ddlTransactionNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlTransactionNo.SelectedIndex == 0)
            {
                populateLeaveType();
                //dtFrom.CalendarDate = DateTime.Now;
                //dtTo.CalendarDate = DateTime.Now;

                dtFrom.Text = DateTime.Now.ToLocalTime().ToString("yyyy-MM-dd");
                dtTo.Text = DateTime.Now.ToLocalTime().ToString("yyyy-MM-dd");
                ddlHalfDay.SelectedIndex = -1;
                txtLeaveBalance.Text = "0";
                txtDayApplied.Text = "0";
                txtOtherDet.Text = "";
                txtLeaveCodeKeep.Text = "";
                LBtnFileAtt.Text = "";
                txtLoadTransNo.Text = "";

                btnSendRequest.Visible = true;
                btnCancel.Visible = false;

                ddlLeaveType.Enabled = true;
                dtFrom.Enabled = true;
                dtTo.Enabled = true;
                ddlHalfDay.Enabled = true;
                txtOtherDet.Enabled = true;
            }
            else
            {
                LBtnFileAtt.Text = "";
                txtLoadTransNo.Text = ddlTransactionNo.Text;
                PopulateLeaveDetails(ddlTransactionNo.Text);
                LoadAttachment(ddlTransactionNo.Text, "Leave Request");
                btnSendRequest.Visible = false;
                btnCancel.Visible = true;

                ddlLeaveType.Enabled = false;
                //dtFrom.Enabled = false;
                //dtTo.Enabled = false;
                ddlHalfDay.Enabled = false;
                txtOtherDet.Enabled = false;

            }
        }


        //private string LoadFile(string _TransNo, string _FileType)
        //{

        //    string TransNo = _TransNo;
        //    string FileType = _FileType;
        //    string _TempImageURL = "";
        //    string _TempImageRemark = "";

        //    SqlConnection Conn = new SqlConnection(CUBIC_HRMS.GlobalVariable.ConnectionString);
        //    GF_CheckConnectionStatus(Conn);
        //    Conn.Open();

        //    try
        //    {
        //        string SQLCommand = "SELECT ATT_FILE_PATH, [ATT_FILE_REMARK] FROM T_EMP_ATT WHERE [TRANSACTION_NO] = '" + TransNo + "' AND ATT_TYPE = '" + FileType + "' ";
        //        SqlCommand cmdDataBase = new SqlCommand(SQLCommand, Conn);
        //        SqlDataReader myReader;
        //        myReader = cmdDataBase.ExecuteReader();

        //        if (myReader.HasRows)
        //        {
        //            while (myReader.Read())
        //            {
        //                _TempImageURL = myReader["ATT_FILE_PATH"].ToString();
        //                _TempImageRemark = myReader["ATT_FILE_REMARK"].ToString();
        //                LBtnFileAtt.Text = _TempImageRemark;
        //            }
        //        }
        //        else
        //        {
        //            _TempImageURL = "";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        string message = ex.Message.ToString();
        //        MessageBoxShow(message);
        //    }
        //    finally
        //    {
        //        Conn.Close();
        //        Conn.Dispose();
        //    }

        //    return _TempImageURL;
        //}

        protected void Button1_Click(object sender, EventArgs e)
        {
            CheckIfDuplicate();
        }

        protected void ddlLeaveType_SelectedIndexChanged(object sender, EventArgs e)
        {

            dtFrom.Text = DateTime.Now.ToLocalTime().ToString("yyyy-MM-dd");
            dtTo.Text = DateTime.Now.ToLocalTime().ToString("yyyy-MM-dd");

            txtLeaveCodeKeep.Text = "";
            ddlHalfDay.SelectedIndex = -1;
            txtLeaveBalance.Text = "0";
            txtDayApplied.Text = "0";
            txtOtherDet.Text = "";
            txtStatus.Text = "Pending";
            LBtnFileAtt.Text = "";
            txtLoadTransNo.Text = "";

            GetLeaveCodeFrmDB(ddlLeaveType.Text);
            //G_LeaveCode = ddlLeaveType.Text;

            GetLeaveDayFrmDB(G_LeaveCode, CUBIC_HRMS.GlobalVariable.G_UserLogin);

            GetPendingLeaveFrmDb(G_LeaveCode, CUBIC_HRMS.GlobalVariable.G_UserLogin);

            GetApprovedLeaveFrmDb(G_LeaveCode, CUBIC_HRMS.GlobalVariable.G_UserLogin);

            G_LeaveAvailable = G_LeaveDay - G_LeavePending - G_LeaveApproved;

            txtLeaveBalance.Text = Convert.ToString(G_LeaveAvailable);

        }

        protected void ddlHalfDay_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}