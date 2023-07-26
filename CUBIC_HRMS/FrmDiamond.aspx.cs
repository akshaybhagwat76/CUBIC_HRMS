using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CUBIC_HRMS
{
    public partial class FrmDiamond : System.Web.UI.Page
    {
        int[] NUmbers = new int[21] { 3, 6, 9, 1, 9, 1, 7, 4, 2, 3, 4, 5, 6, 7, 3, 6, 9, 1, 9, 8, 8 };

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btn_Number_Click(object sender, EventArgs e)
        {
            //btnNo1.Text = "3";
            //btnNo2.Text = "6";
            //btnNo3.Text = "9";
            //btnNo4.Text = "1";
            //btnNo5.Text = "9";
            //btnNo6.Text = "1";
            //btnNo7.Text = "7";
            //btnNo8.Text = "4";
            //btnNo9.Text = "2";
            //btnNo10.Text = "3";
            //btnNo11.Text = "4";
            //btnNo12.Text = "5";
            //btnNo13.Text = "6";
            //btnNo14.Text = "7";
            //btnNo15.Text = "3";
            //btnNo16.Text = "6";
            //btnNo17.Text = "9";
            //btnNo18.Text = "1";
            //btnNo19.Text = "9";
            //btnNo20.Text = "8";
            //btnNo21.Text = "8";

            btnNo1.Text = Convert.ToString(NUmbers[0]);
            btnNo2.Text = Convert.ToString(NUmbers[1]);
            btnNo3.Text = Convert.ToString(NUmbers[2]);
            btnNo4.Text = Convert.ToString(NUmbers[3]);
            btnNo5.Text = Convert.ToString(NUmbers[4]);
            btnNo6.Text = Convert.ToString(NUmbers[5]);
            btnNo7.Text = Convert.ToString(NUmbers[6]);
            btnNo8.Text = Convert.ToString(NUmbers[7]);
            btnNo9.Text = Convert.ToString(NUmbers[8]);
            btnNo10.Text = Convert.ToString(NUmbers[9]);
            btnNo11.Text = Convert.ToString(NUmbers[10]);
            btnNo12.Text = Convert.ToString(NUmbers[11]);
            btnNo13.Text = Convert.ToString(NUmbers[12]);
            btnNo14.Text = Convert.ToString(NUmbers[13]);
            btnNo15.Text = Convert.ToString(NUmbers[14]);
            btnNo16.Text = Convert.ToString(NUmbers[15]);
            btnNo17.Text = Convert.ToString(NUmbers[16]);
            btnNo18.Text = Convert.ToString(NUmbers[17]);
            btnNo19.Text = Convert.ToString(NUmbers[18]);
            btnNo20.Text = Convert.ToString(NUmbers[19]);
            btnNo21.Text = Convert.ToString(NUmbers[20]);
        }
    }
}