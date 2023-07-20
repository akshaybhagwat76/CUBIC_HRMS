using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Web;

namespace CUBIC_HRMS
{

    // ******************************************
    // *********** LAST UPDATE 25062023

    public class GlobalVariable
    {

        /// *** this Class is ONLY for standard function/variable which able to use for ALL project
        /// *** all special function that frequently use, please put inside here, and update to server
        /// Project Listing, Client Class, GlobalVariable

        // ** Update 25062023
        // - update variable public static string G_UserName { get; set; }

        // ** Updated 28/May/2023 
        // - Add new features GF_HashPassword
        // - Add new features GF_VerifyPassword
        // - When a user sets a password, you call HashPassword, and when they attempt to log in, you call VerifyPassword with the password they entered and the hash stored in the database.

        /// ******** IMPORTANT ********
        /// *************************
        /// FOR NET 6.0, NEED MANUAL IMPORT FEW THING THRU NUGET
        /// SQL : System.Data.SqlClient
        /// CHART
        /// REPORT VIEWER

        /// ******** RULES ********
        /// **********************
        /// 1. All function in Global must start with GF_ >> Global Function example : GF_InsertAuditLog
        /// 2. All local passing parameter must start with _ >> example : GF_InsertAuditLog(string _AuditCode)
        /// 3. All Global Variable must start with G_ >> example G_User_BU
        /// 4. all SQL Select statement string must : SQLSelectCommand
        /// 5. all SQL Insert statement string must : SQLInsertCommand
        /// 6. all SQL Update statement string must : SQLUpdateCommand
        /// 7. all SQL Delete statement string must : SQLDeleteCommand

        // SQLSelectCmd = SQLSelectCmd + Change to :
        // SQLSelectCmd += 

        /// 8. all try catch must include function : GF_InsertAuditLog
        /// 9. Standard Code For SELECT/UPDATE/INSERT/DELETE refer to Function
        /// GF_InsertAuditLog
        /// GF_GetRunningNumber
        /// GF_UpdateRunningNumber
        /// GF_DeleteDuplicateData
        /// 10. Must Have Database Table : Refer to Database Standard
        /// 11. All Image Use For Specific Project, Must Import/Add into Image/ProjectImage
        /// 12. All Standard Icon Use, Must Import/Add into Image/Icon
        /// 
        /// 13. All Project Related Function, Variable, Put In GlobalProjectClass.
        /// Function Name start from GP_
        /// Variable start from G_
        /// 14. All Form Name must start from FRM
        /// 15. All Report Name must start from RPT
        /// 16. Tool that include event/key must have proper name. Example : txtUserName, ddlRecipeNo, lblStatus
        /// 17. Tool that only label, can no need rename. Example : label1
        /// 18. Form Call - Refer to FrmMainPage : Call FrmDashboard
        /// 19. Form Coding Arrangement Sequence 
        /// i. Form Load (Form)
        /// ii. Function
        /// iii. Timer
        /// iv. Button Click
        /// v. Text Change


        /// ******** REFERENCE DOCUMENT : Knowledge Base ********
        /// *************************************************
        /// 1. DATABASE STANDARD : Standard DB Structure.docx
        /// 2. SQL DATE TIME CONVERTION : MSSql DateTime Convertion.docx
        /// 3. Check Implicit Using for .NET 6. useful.



        /// ******** DATE TIME, CONVERT *******
        /// ********************************
        #region DateTimeFormula
        //DateTime.Now.ToString("MM/dd/yyyy")	05/29/2015
        //DateTime.Now.ToString("dddd, dd MMMM yyyy")	Friday, 29 May 2015
        //DateTime.Now.ToString("dddd, dd MMMM yyyy")	Friday, 29 May 2015 05:50
        //DateTime.Now.ToString("dddd, dd MMMM yyyy")	Friday, 29 May 2015 05:50 AM
        //DateTime.Now.ToString("dddd, dd MMMM yyyy") Friday, 29 May 2015 5:50
        //DateTime.Now.ToString("dddd, dd MMMM yyyy")	Friday, 29 May 2015 5:50 AM
        //DateTime.Now.ToString("dddd, dd MMMM yyyy HH:mm:ss")    Friday, 29 May 2015 05:50:06
        //DateTime.Now.ToString("MM/dd/yyyy HH:mm")	05/29/2015 05:50
        //DateTime.Now.ToString("MM/dd/yyyy hh:mm tt")	05/29/2015 05:50 AM
        //DateTime.Now.ToString("MM/dd/yyyy H:mm")    05/29/2015 5:50
        //DateTime.Now.ToString("MM/dd/yyyy h:mm tt")	05/29/2015 5:50 AM
        //DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss")    05/29/2015 05:50:06
        //DateTime.Now.ToString("MMMM dd")	May 29
        //DateTime.Now.ToString("HH:mm")	05:50
        //DateTime.Now.ToString("hh:mm tt")	05:50 AM
        //DateTime.Now.ToString("H:mm")   5:50
        //DateTime.Now.ToString("h:mm tt")	5:50 AM
        //DateTime.Now.ToString("HH:mm:ss")   05:50:06
        //DateTime.Now.ToString("yyyy MMMM")	2015 May
        #endregion DateTimeFormula



        //public static string ConnectionString = "Data Source=LOCALHOST\\SQLEXPRESS;Initial Catalog=TEMP_CUBIC_HRMS;User ID=sa;Password=@@DMS1208@@;Connection Timeout=30;Max Pool Size=30000";
        public static string ConnectionString = "Data Source=DESKTOP-20GK6OE\\SQLEXP2008;Initial Catalog=TEMP_CUBIC_HRMS;User ID=sa;Password=vips@5619;Connection Timeout=30;Max Pool Size=30000";

        // ** Message Box Use
        // ** FrmMessageDisplay
        public static bool G_Message_Trigger = false;
        public static bool G_Message_Close = false;
        public static string G_MessageBoxResult = "-";
        public static string G_MessageBoxReturn = "-";


        public static bool G_Export = false;

        // ** Attachment Path
        // Server.MapPath("~/Files/");
        public static string G_AttFilePath = "C:\\www\\CUBIC_HRMS\\AttFiles\\"; //"C:\\www\\CUBIC_HRMS\\Files\\";
        public static string G_ExportFilePath = "C:\\www\\CUBIC_HRMS\\ExportFiles\\"; //"C:\\www\\GORE_HRMS\\Files\\";

        public static string G_EventDisplay = "-";
        public static string G_TimeoutReading = "-";


   
        public static string G_User_BU { get; set; }


        // ** User Name, can be employee code
        public static string G_UserLogin { get; set; }


        // ** User Name, can be employee name
        public static string G_UserName { get; set; }

        public static int G_UserLevel { get; set; }

        public static string G_Recipe { get; set; }


        // ** Level Admin Or Eng
        public static string G_UserRole { get; set; }

        public static bool G_UserLoginStatus { get; set; }

        // ** If first time Login, then this will set to Y
        public static bool G_UserChangePasswordRequired { get; set; }

        public static string G_PackageCode{ get; set; }


        //For determine Alarm  has been hit
        public static bool G_IsAlarmHit { get; set; }


        //For keep the lot number after enter package
        public static string G_LotNumber { get; set; }


        //For keep increasing the Alarm Total Error
        public static int G_TotalErrorOccur { get; set; }


        // ** Example to get info PackageName
        public static string G_PackageName { get; set; }


        // ** Package Code
        public static string G_SPackageCode { get; set; }


        //Determine New lot/End lot
        public static bool G_InLot { get; set; }





        /// <summary>
        /// ********************************
        /// ******** DATABASE **************
        /// ********************************
        /// </summary>


        // ** Database Connection
        public static void GF_CheckConnectionStatus(SqlConnection _TempConn)
        {
            try
            {
                if (_TempConn.State == ConnectionState.Open)
                {
                    _TempConn.Close();
                    _TempConn.Dispose();
                }
            }
            catch (Exception ex)
            {
                GF_InsertAuditLog("-", "Catch Error", "GlobalVariable", "GF_CheckConnectionStatus", ex.ToString().Replace("'", ""));
            }
        }

        public static string GF_GetFromURL(string _TempString, HttpRequest request)
        {
            string paramReturn = request.QueryString[_TempString];
            return paramReturn;
        }

        // ** insert Audit Log
        public static void GF_InsertAuditLog(string _AuditCode, string _AuditType, string _AuditForm, string _AuditDesc, string _AuditRemark)
        {
            try
            {
                SqlConnection Conn = new SqlConnection(GlobalVariable.ConnectionString);
                GF_CheckConnectionStatus(Conn);
                Conn.Open();

                string SQLInsertCommand = "INSERT INTO AUDIT_LOG ";
                SQLInsertCommand = SQLInsertCommand + "([AUDIT_BU]";
                SQLInsertCommand = SQLInsertCommand + ",[AUDIT_CODE]";
                SQLInsertCommand = SQLInsertCommand + ",[AUDIT_TYPE] ";
                SQLInsertCommand = SQLInsertCommand + ",[AUDIT_FORM] ";
                SQLInsertCommand = SQLInsertCommand + ",[AUDIT_DESC] ";
                SQLInsertCommand = SQLInsertCommand + ",[AUDIT_REMARK] ";
                SQLInsertCommand = SQLInsertCommand + ",[AUDIT_CREATE_BY] ";
                SQLInsertCommand = SQLInsertCommand + ",[AUDIT_CREATE_DATE])";
                SQLInsertCommand = SQLInsertCommand + "VALUES ";
                SQLInsertCommand = SQLInsertCommand + "('" + GlobalVariable.G_User_BU + "' ";
                SQLInsertCommand = SQLInsertCommand + ",'" + _AuditCode + "' ";
                SQLInsertCommand = SQLInsertCommand + ",'" + _AuditType + "' ";
                SQLInsertCommand = SQLInsertCommand + ",'" + _AuditForm + "' ";
                SQLInsertCommand = SQLInsertCommand + ",'" + _AuditDesc + "' ";
                SQLInsertCommand = SQLInsertCommand + ",'" + _AuditRemark + "' ";
                SQLInsertCommand = SQLInsertCommand + ",'" + GlobalVariable.G_UserLogin + "' ";
                SQLInsertCommand = SQLInsertCommand + ",getdate()) ";

                var SQLCmd = new SqlCommand(SQLInsertCommand, Conn);
                SQLCmd.ExecuteNonQuery();

                Conn.Close();
                Conn.Dispose();

                string TempOLDName = "AU_" + DateTime.Now.ToShortDateString().ToString();
                string TempNewName = TempOLDName.Replace('/', '_');
                string TempAuditRemark = _AuditForm + " >> " + _AuditRemark;
                //GF_WriteToTextFile("D:\\AuditLog\\", TempNewName, TempAuditRemark);

            }
            catch (Exception ex)
            {
                GF_InsertAuditLog("-", "Catch Error", "GlobalVariable", "GF_InsertAuditLog", ex.ToString().Replace("'", ""));
            }

        }

        // ** insert Event Log
        public static void GF_InsertEventLog(string _EventCode, string _EventRemark, string _EventSolution, string _EventCreateBy)
        {
            G_EventDisplay = _EventRemark;
            SqlConnection Conn = new SqlConnection(ConnectionString);

            try
            {
                GF_CheckConnectionStatus(Conn);
                Conn.Open();

                string SQLInsertCommand = "INSERT INTO [EVENT_LOG] ";
                SQLInsertCommand = SQLInsertCommand + "(";
                SQLInsertCommand = SQLInsertCommand + "[EVENT_CODE],[EVENT_REMARK],[EVENT_SOLUTION],";
                SQLInsertCommand = SQLInsertCommand + "[EVENT_CREATED_BY], [EVENT_CREATED_DATE] ";
                SQLInsertCommand = SQLInsertCommand + ")";
                SQLInsertCommand = SQLInsertCommand + "VALUES ";
                SQLInsertCommand = SQLInsertCommand + "( ";
                SQLInsertCommand = SQLInsertCommand + "'" + _EventCode + "','" + _EventRemark + "','" + _EventSolution + "',";
                SQLInsertCommand = SQLInsertCommand + "'" + _EventCreateBy + "', GETDATE() ";
                SQLInsertCommand = SQLInsertCommand + ")";

                var SQLCmd = new SqlCommand(SQLInsertCommand, Conn);
                SQLCmd.ExecuteNonQuery();

                Conn.Close();
                Conn.Dispose();
            }
            catch (Exception ex)
            {
                GF_InsertAuditLog("-", "Catch Error", "GlobalVariable", "GF_InsertEventLog", ex.ToString().Replace("'", ""));
            }
            finally
            {
                Conn.Close();
                Conn.Dispose();
            }
        }


        // ** Password Hasher
        public static string GF_HashPassword(string password)
        {
            byte[] salt;
            new RNGCryptoServiceProvider().GetBytes(salt = new byte[16]);

            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000);
            byte[] hash = pbkdf2.GetBytes(20);

            byte[] hashBytes = new byte[36];
            Array.Copy(salt, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, 20);

            return Convert.ToBase64String(hashBytes);
        }

        public static bool GF_VerifyPassword(string enteredPassword, string storedHash)
        {
            byte[] hashBytes = Convert.FromBase64String(storedHash);
            byte[] salt = new byte[16];
            Array.Copy(hashBytes, 0, salt, 0, 16);

            var pbkdf2 = new Rfc2898DeriveBytes(enteredPassword, salt, 10000);
            byte[] hash = pbkdf2.GetBytes(20);

            for (int i = 0; i < 20; i++)
                if (hashBytes[i + 16] != hash[i])
                    return false;

            return true;
        }


        // ** get Running Number With Prefix
        public static string GF_GetRunningNumber(string _TempBU, string _TempPrefix)
        {
            string TempReturn = "-";
            string NextRunningNumber = "-";
            SqlConnection Conn = new SqlConnection(GlobalVariable.ConnectionString);
            GF_CheckConnectionStatus(Conn);
            Conn.Open();

            try
            {
                string SQLSelectCommand = "SELECT ISNULL(M_SYS_NEXT,0) AS NEXTNUMBER ";
                SQLSelectCommand = SQLSelectCommand + "FROM[dbo].[M_SYSRUN_NO] ";
                SQLSelectCommand = SQLSelectCommand + "WHERE ";
                SQLSelectCommand = SQLSelectCommand + "M_SYS_BU = '" + _TempBU + "' ";
                SQLSelectCommand = SQLSelectCommand + "AND M_SYS_PREFIX = '" + _TempPrefix + "'";

                SqlCommand cmdDataBase = new SqlCommand(SQLSelectCommand, Conn);
                SqlDataReader myReader;

                myReader = cmdDataBase.ExecuteReader();
                if (myReader.HasRows)
                {
                    while (myReader.Read())
                    {
                        NextRunningNumber = myReader["NEXTNUMBER"].ToString();
                    }
                }
                else
                {
                    NextRunningNumber = "0";
                }
                myReader.Close();

                //** How many digit of a standard
                int CodeLength = 7;

                //** get the number of 0s we needed
                int NumZeros = CodeLength - NextRunningNumber.Length;

                string newCode = null;

                for (int i = 0; i < NumZeros; i++)
                {
                    newCode += "0";
                }

                newCode += NextRunningNumber;

                //** Combine to 7 Digit String

                NextRunningNumber = _TempPrefix + newCode;
                TempReturn = NextRunningNumber;

            }
            catch (Exception ex)
            {
                GF_InsertAuditLog("-", "Catch Error", "GlobalVariable", "GF_GetRunningNumber", ex.ToString().Replace("'", ""));
            }
            finally
            {
                Conn.Close();
                Conn.Dispose();
            }

            return TempReturn;
        }

        // ** get Running Number Without Prefix
        public static string GF_GetRunningNumberWoPrefix(string _TempBU, string _TempPrefix)
        {
            string TempReturn = "-";
            string NextRunningNumber = "-";
            SqlConnection Conn = new SqlConnection(GlobalVariable.ConnectionString);
            GF_CheckConnectionStatus(Conn);
            Conn.Open();

            try
            {
                string SQLSelectCommand = "SELECT ISNULL(M_SYS_NEXT,0) AS NEXTNUMBER ";
                SQLSelectCommand = SQLSelectCommand + "FROM[dbo].[M_SYSRUN_NO] ";
                SQLSelectCommand = SQLSelectCommand + "WHERE ";
                SQLSelectCommand = SQLSelectCommand + "M_SYS_BU = '" + _TempBU + "' ";
                SQLSelectCommand = SQLSelectCommand + "AND M_SYS_PREFIX = '" + _TempPrefix + "'";

                SqlCommand cmdDataBase = new SqlCommand(SQLSelectCommand, Conn);
                SqlDataReader myReader;

                myReader = cmdDataBase.ExecuteReader();
                if (myReader.HasRows)
                {
                    while (myReader.Read())
                    {
                        NextRunningNumber = myReader["NEXTNUMBER"].ToString();
                    }
                }
                else
                {
                    NextRunningNumber = "0";
                }

                TempReturn = NextRunningNumber;
                myReader.Close();


            }
            catch (Exception ex)
            {
                GF_InsertAuditLog("-", "Catch Error", "GlobalVariable", "GF_GetRunningNumberWoPrefix", ex.ToString().Replace("'", ""));
            }
            finally
            {
                Conn.Close();
                Conn.Dispose();
            }

            return TempReturn;
        }

        // ** Update Running Number With Prefix
        public static void GF_UpdateRunningNumber(string _TempBU, string _TempPrefix)
        {
            SqlConnection Conn = new SqlConnection(GlobalVariable.ConnectionString);
            GF_CheckConnectionStatus(Conn);
            Conn.Open();

            try
            {
                string SQLUpdateCommand = "UPDATE [dbo].[M_SYSRUN_NO] SET ";
                SQLUpdateCommand = SQLUpdateCommand + "[M_SYS_NEXT]= M_SYS_NEXT+1 ";
                SQLUpdateCommand = SQLUpdateCommand + "WHERE [M_SYS_PREFIX] ='" + _TempPrefix + "'";
                SQLUpdateCommand = SQLUpdateCommand + "AND [M_SYS_BU] ='" + _TempBU + "'";
                SqlDataAdapter SDAUpdate = new SqlDataAdapter(SQLUpdateCommand, Conn);
                SDAUpdate.SelectCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                GF_InsertAuditLog("-", "Catch Error", "GlobalVariable", "GF_UpdateRunningNumber", ex.ToString().Replace("'", ""));
            }
            finally
            {
                Conn.Dispose();
                Conn.Close();
            }
        }

        // ** Delete Duplicate Data

        public static void GF_DeleteDuplicateData(string _TempDatabaseName)
        {
            SqlConnection Conn = new SqlConnection(GlobalVariable.ConnectionString);
            GF_CheckConnectionStatus(Conn);
            Conn.Open();
            try
            {
                string SQLDeleteCommand = "DELETE ";
                SQLDeleteCommand = SQLDeleteCommand + "FROM " + _TempDatabaseName + " ";
                SQLDeleteCommand = SQLDeleteCommand + "WHERE ST_DATA NOT IN ";
                SQLDeleteCommand = SQLDeleteCommand + "( ";
                SQLDeleteCommand = SQLDeleteCommand + "SELECT MAX(ST_DATA) AS MaxRecordID ";
                SQLDeleteCommand = SQLDeleteCommand + "FROM " + _TempDatabaseName + " ";
                SQLDeleteCommand = SQLDeleteCommand + "GROUP BY ST_DATA ";
                SQLDeleteCommand = SQLDeleteCommand + ") ";

                var SQLCmd = new SqlCommand(SQLDeleteCommand, Conn);
                SQLCmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
                GF_InsertAuditLog("-", "Catch Error", "GlobalVariable", "GF_DeleteDuplicateData", ex.ToString().Replace("'", ""));

            }
            finally
            {
                Conn.Close();
                Conn.Dispose();
            }
        }


        // ** Thread Stable Timer
        public static bool GF_StableTimer(int _SleepTimerms)
        {
            //Thread.Sleep(_SleepTimerms);

            bool _StableTimer1 = false;
            Stopwatch sw = new Stopwatch();
            sw.Start();

            //_InputCheck either true or false
            //when _InputCheck not = state we want, after some _TimeOutms
            while (_StableTimer1 == false)
            {
                //G_TimeoutReading = sw.ElapsedMilliseconds;
                if (sw.ElapsedMilliseconds > _SleepTimerms)
                {
                    sw.Stop();
                    _StableTimer1 = true;
                }
            }

            return _StableTimer1;
        }

        public static bool GF_TimeoutTimer(int _TimeOutms)
        {
            bool _TimeOut = false;
            Stopwatch sw = new Stopwatch();
            sw.Start();

            //_InputCheck either true or false
            //when _InputCheck not = state we want, after some _TimeOutms
            while (_TimeOut == false)
            {
                G_TimeoutReading = sw.ElapsedMilliseconds.ToString();
                if (sw.ElapsedMilliseconds > _TimeOutms)
                {
                    sw.Stop();
                    _TimeOut = true;
                }
            }

            return _TimeOut;
        }


        // ** Combine Date and Time
        public static string GF_CombineDateTime(string _TempYear, string _TempMonth, string _TempDay, string _TempHour, string _TempMin, string _TempSec)
        {
            // ** date format from string : 211116215732
            // ** convert to 11/16/2021 9:57:32 PM
            string TempDateCombine = "-";
            try
            {
                string TempLongYear = _TempYear;
                TempLongYear = "20" + _TempYear;

                TempDateCombine = TempLongYear + "-" + _TempMonth + "-" + _TempDay + " " + _TempHour + ":" + _TempMin + ":" + _TempSec;
            }
            catch (Exception ex)
            {
                GF_InsertAuditLog("-", "Catch Error", "GlobalVariable", "GF_CombineDateTime", ex.ToString().Replace("'", ""));
            }

            return TempDateCombine;

        }


        // ** Convert Second to Time
        public static string GF_ConvertSecToTime(string _TempSecond)
        {
            string TempResult = "-";

            try
            {
                double TempSecond = Convert.ToInt16(_TempSecond);
                TimeSpan time = TimeSpan.FromSeconds(0);

                if (_TempSecond == "" || _TempSecond is null)
                {
                    time = TimeSpan.FromSeconds(0);
                }
                else
                {
                    time = TimeSpan.FromSeconds(TempSecond);
                }

                // ** if want to have ms
                //string str = time.ToString(@"hh\:mm\:ss\:fff");

                // ** normal time
                string str = time.ToString(@"hh\:mm\:ss");

                //// ** if you want date time format then you can also do this
                //TimeSpan time = TimeSpan.FromSeconds(seconds);
                //DateTime dateTime = DateTime.Today.Add(time);

                //string displayTime = dateTime.ToString("hh:mm:tt");
                TempResult = str;
            }
            catch (Exception ex)
            {
                GF_InsertAuditLog("-", "Catch Error", "GlobalVariable", "GF_ConvertSecToTime", ex.ToString().Replace("'", ""));
            }

            return TempResult;
        }

        // ** Convert String to Two Decimal
        public static string GF_ConvertToTwoDecimal(string _TempString)
        {
            string x = _TempString;
            string TempReturn = "-";

            if (int.TryParse(x, out int value))
            {
                TempReturn = Math.Round((Decimal)value, 2).ToString();
            }
            else if (double.TryParse(x, out double value1))
            {
                TempReturn = Math.Round((Decimal)value1, 2).ToString();
            }
            else
            {
                TempReturn = "0";
            }

            return TempReturn;
        }

        // ** Convert String to Two Int
        public static int GF_ConvertStringToINT(string _TempString)
        {
            string x = _TempString;
            int TempReturn = 0;

            if (double.TryParse(x, out double value))
            {
                TempReturn = Convert.ToInt16(value);
            }

            return TempReturn;
        }

        public static string GF_ConvertDateTime_yyyyMMMdd(string _TempDate)
        {
            string TempReturn = "-";

            TempReturn = Convert.ToDateTime(_TempDate).ToString("yyyy-MMM-dd").Replace("-", "");

            return TempReturn;
        }

        // ** Generate Date Table
        // ** Generate from date 1 to 31 base on date selection
        // ** sample calling :     GenerateDateTable(Convert.ToDateTime(_StartDate), Convert.ToDateTime(_EndDate), G_arrDateTable);
        public static void GenerateDateTable(DateTime _StartDate, DateTime _EndDate, ArrayList _arrDateTable)
        {
            try
            {
                _arrDateTable.Clear();

                Enumerable.Range(0, 1 + _EndDate.Subtract(_StartDate).Days).Select(offset => _StartDate.AddDays(offset)).ToArray();

                List<DateTime> dates = new List<DateTime>();

                for (var dt = _StartDate; dt <= _EndDate; dt = dt.AddDays(1))
                {
                    dates.Add(dt);
                    _arrDateTable.Add(dt.ToShortDateString());
                }
            }
            catch (Exception ex)
            {
                GF_InsertAuditLog("-", "Catch Error", "GlobalVariable", "GenerateDateTable", ex.ToString().Replace("'", ""));
            }

        }


        /// <summary>
        /// ********************************
        /// ******** OTHER **************
        /// ********************************
        /// </summary>

        public static void GF_WriteToTextFile(string _TempPath, string _TempFileName, string _TempStringWrite)
        {
            try
            {
                string path = _TempPath + _TempFileName + ".txt";
                if (!File.Exists(path))
                {
                    // Create a file to write to.
                    using (StreamWriter sw = File.CreateText(path))
                    {
                        sw.WriteLine(_TempStringWrite);
                    }
                }
                else
                {
                    using (StreamWriter sw = File.AppendText(path))
                    {
                        sw.WriteLine(_TempStringWrite);
                    }
                }
            }
            catch (Exception ex)
            {
                GF_InsertAuditLog("-", "Catch Error", "GlobalVariable", "GF_WriteToTextFile", ex.ToString().Replace("'", ""));
            }

        }


        public static void GF_EmailSender(string _TempDomain, int _TempPort, string _TempPassword, string _TempSendFromMail, string _TempSendToMail, string _TempSubject, string _TempBodyMsg)
        {
            //domain use back original domain. Example if cubicsoftware.com.my then use back cubicsoftware.com.my
            //port use back smtp port. example normally output port is
            //example outgoing domain = mail.cubicsoftware.com.my
            //example outgoing port = 465
            try
            {
                SmtpClient smtpClient = new SmtpClient(_TempDomain, _TempPort);

                smtpClient.Credentials = new System.Net.NetworkCredential(_TempSendFromMail, _TempPassword);
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtpClient.EnableSsl = false;
                smtpClient.Timeout = 10000;

                MailMessage mailMessage = new MailMessage(_TempSendFromMail, _TempSendToMail);
                mailMessage.Subject = _TempSubject;
                mailMessage.Body = _TempBodyMsg;
                mailMessage.IsBodyHtml = true;

                smtpClient.Send(mailMessage);
                //Label1.Text = "Message sent";
            }
            catch (Exception ex)
            {
                GF_InsertAuditLog("-", "Catch Error", "GlobalVariable", "GF_EmailSender", ex.ToString().Replace("'", ""));

            }
        }


    }

}