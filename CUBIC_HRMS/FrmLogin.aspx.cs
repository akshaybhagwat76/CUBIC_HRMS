using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using static CUBIC_HRMS.GlobalVariable;
using System.Configuration;
using System.Web.Security;

namespace CUBIC_HRMS
{
    public partial class FrmLogin : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //populateBU();
            }

            if(this.Page.User.Identity.IsAuthenticated)
            {
                Response.Redirect(FormsAuthentication.DefaultUrl);
            }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {

            if (txtUsername.Text != string.Empty && txtPassword.Text != string.Empty)
            {
                if (ChkRememberMe.Checked) // If "Remember me" is checked
                {
                    HttpCookie usernameCookie = new HttpCookie("Username");
                    usernameCookie.Value = txtUsername.Text;
                    usernameCookie.Expires = DateTime.Now.AddDays(15); // Cookie will be stored for 15 days
                    Response.Cookies.Add(usernameCookie);
                }
                else
                {
                    if (Request.Cookies["Username"] != null) // If cookie exists
                    {
                        HttpCookie usernameCookie = new HttpCookie("Username");
                        usernameCookie.Expires = DateTime.Now.AddDays(-1); // Expire the cookie
                        Response.Cookies.Add(usernameCookie);
                    }
                }

                F_Login();
            }
        }
        
        protected void btnCheck_Click(object sender, EventArgs e) 
        {
            // 1 is Yaj5+/PS8OqI9HISFbwle0uYCwmap/XTUzU7tmha3DY4WBwT
            // can hardcode this for admin password
            string TempHash = GF_HashPassword("1");

        }

        private void F_Login()
        {
          //try
          //  {
                var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
                var stringChars = new char[8]; //if you want generate random 8 character, then use 8, if 10 then use 10. 
                var random = new Random();

                for (int i = 0; i < stringChars.Length; i++)
                {
                    stringChars[i] = chars[random.Next(chars.Length)];
                }

                var finalString = new String(stringChars); //here to generate random string / token

                SqlConnection Conn = new SqlConnection(CUBIC_HRMS.GlobalVariable.ConnectionString);
                GF_CheckConnectionStatus(Conn);
                Conn.Open();

                string SQLSelectCommand = "SELECT A.EMP_BU,A.EMP_CODE, B.EMP_NICK_NAME,EMP_PASSWORD, EMP_LEVEL  ";
                SQLSelectCommand = SQLSelectCommand + "FROM [dbo].[M_EMP_LOGIN] A ";
                SQLSelectCommand = SQLSelectCommand + "LEFT JOIN [dbo].[M_EMP_MASTER] B ON A.EMP_CODE = B.EMP_CODE ";
                SQLSelectCommand = SQLSelectCommand + "WHERE ";
                SQLSelectCommand = SQLSelectCommand + "(A.EMP_CODE = '" + txtUsername.Text + "') ";
                SQLSelectCommand = SQLSelectCommand + "AND A.EMP_STATUS = 'Y' ";

                SqlCommand cmdDataBase = new SqlCommand(SQLSelectCommand, Conn);
                SqlDataReader myReader;
                myReader = cmdDataBase.ExecuteReader();

                // step 1 : when login, use back this.
                // step 2 : if hasrows, then select EMP_ISLOGIN FROM M_EMP_LOGIN
                // step 3 : if EMP_ISLOGIN == "Y" show duplicate login.
                // step 4 : Update back to N
                // step 5 :
                if (myReader.HasRows)
                {
                    while (myReader.Read())
                    {
                        bool TempVerification = GF_VerifyPassword(txtPassword.Text, myReader["EMP_PASSWORD"].ToString());

                        if (TempVerification == true)
                        {
                            Session["UserID"] = txtUsername.Text;
                            Session["UserLogin"] = myReader["EMP_CODE"].ToString();
                            Session["UserName"] = myReader["EMP_CODE"].ToString();
                            Session["BU"] = myReader["EMP_BU"].ToString();
                            Session["UserLevel"] = myReader["EMP_LEVEL"].ToString();

                            Session["TokenSessionID"] = finalString;
                            CUBIC_HRMS.GlobalVariable.G_UserLogin = txtUsername.Text;
                            CUBIC_HRMS.GlobalVariable.G_UserName = myReader["EMP_CODE"].ToString();
                            CUBIC_HRMS.GlobalVariable.G_UserLevel = Convert.ToInt32(myReader["EMP_LEVEL"]);
                            CUBIC_HRMS.GlobalVariable.G_User_BU = myReader["EMP_BU"].ToString();

                            Response.Redirect("FrmDashboard.aspx");
                            //Response.Redirect("FrmSub2WS1Dashboard.aspx?ID=HPSub2WS1Dashboard&SubProcess=2&Workstation=1");
                        }
                        else
                        {
                            string message = "User Not Found, kindly look for administration";
                            System.Text.StringBuilder sb = new System.Text.StringBuilder();
                            sb.Append("<script type = 'text/javascript'>");
                            sb.Append("window.onload=function(){");
                            sb.Append("alert('");
                            sb.Append(message);
                            sb.Append("')};");
                            sb.Append("</script>");
                            this.Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", sb.ToString());
                        }
                    }
                }
                else
                {
                    string message = "User Not Found, kindly look for administration";
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    sb.Append("<script type = 'text/javascript'>");
                    sb.Append("window.onload=function(){");
                    sb.Append("alert('");
                    sb.Append(message);
                    sb.Append("')};");
                    sb.Append("</script>");
                    this.Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "alert", sb.ToString());
                }
            //}
            //catch (Exception ex)
            //{
            //    GF_InsertAuditLog("-", "Catch Error", "FrmEmployeeMaintenance", ex.Message.ToString(), "btnSave");

            //}
            //finally
            //{

            //}
            
        }


        //private void populateBU()
        //{
        //    SqlConnection Conn = new SqlConnection(CUBIC_HRMS.GlobalVariable.ConnectionString);
        //    GF_CheckConnectionStatus(Conn);
        //    Conn.Open();

        //    try
        //    {
        //        SqlDataAdapter da = new SqlDataAdapter("SELECT DISTINCT[BU_DESCRIPTION] FROM [dbo].[M_BU] WHERE [BU_STATUS]='Y' ORDER BY [BU_DESCRIPTION]", Conn);
        //        DataTable dt = new DataTable();

        //        this.ddlBU.Items.Clear();
        //        this.ddlBU.SelectedValue = null;
        //        da.Fill(dt);
        //        ddlBU.DataSource = dt;
        //        ddlBU.DataBind();
        //        ddlBU.DataTextField = "BU_DESCRIPTION";
        //        ddlBU.DataValueField = "BU_DESCRIPTION";
        //        ddlBU.DataBind();
        //        // Then add your first item
        //        ddlBU.Items.Insert(0, "- Select  BU -");

        //    }
        //    catch (Exception ex)
        //    {
        //        //MessageBoxShow("There is no Data");

        //    }
        //    finally
        //    {
        //        Conn.Dispose();
        //        Conn.Close();
        //    }
        //}
    }
}