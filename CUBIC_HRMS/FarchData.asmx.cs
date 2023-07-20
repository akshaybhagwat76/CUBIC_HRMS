using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using static CUBIC_HRMS.GlobalVariable;
using System.Web.Script.Serialization;

namespace CUBIC_HRMS
{
    /// <summary>
    /// Summary description for FarchData
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class FarchData : System.Web.Services.WebService
    {

        [WebMethod]
        public dynamic CalenderData(string myUserName)
        {
            List<LeaveRecord> employeelist = new List<LeaveRecord>();

            //String BU = ddlBU.Text;
            SqlConnection Conn = new SqlConnection(CUBIC_HRMS.GlobalVariable.ConnectionString);
            GF_CheckConnectionStatus(Conn);
            Conn.Open();
            string SQLCommand = "-";

            try
            {

                SQLCommand = "exec USP_VP_GET_LEAVE_DATA ";
                SqlDataAdapter adpt = new SqlDataAdapter(SQLCommand, Conn);
                DataTable dt = new DataTable();
                adpt.Fill(dt);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    LeaveRecord _l = new LeaveRecord(); ;

                    _l.LEAVEH_NO = Convert.ToString(dt.Rows[i]["LEAVEH_NO"]);
                    _l.LEAVED_DATE_FROM = DateTime.Parse(Convert.ToString(dt.Rows[i]["LEAVED_DATE_FROM"])).ToString("yyyy-MM-dd");
                    _l.LEAVED_DATE_TO = DateTime.Parse(Convert.ToString(dt.Rows[i]["LEAVED_DATE_TO"])).ToString("yyyy-MM-dd");
                    _l.LEAVE_NAME = Convert.ToString(dt.Rows[i]["LEAVE_NAME"]);
                    _l.COLOR = Convert.ToString(dt.Rows[i]["COLOR"]);

                    employeelist.Add(_l);
                }

                JavaScriptSerializer js = new JavaScriptSerializer();
                return js.Serialize(employeelist);
            }
            catch (Exception ex)
            {
                string TempAudit = ex.ToString().Replace("'", "");
                GF_InsertAuditLog("-", "Data Check", "FrmEmployeeMaster", TempAudit, "populateDepartment");
                return null;
            }
            finally
            {
                Conn.Dispose();
                Conn.Close();
            }


        }



    }
    public class LeaveRecord
    {
        public string LEAVEH_NO { get; set; }
        public string LEAVED_DATE_FROM { get; set; }
        public string LEAVED_DATE_TO { get; set; }
        public string LEAVE_NAME { get; set; }
        public string COLOR { get; set; }


    }

}
