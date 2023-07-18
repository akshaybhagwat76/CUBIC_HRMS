
using System;
using System.Data.SqlClient;
using static CUBIC_HRMS.GlobalVariable;


namespace CUBIC_HRMS
{

    
    public class GlobalProjectClass
    {

        // ** HERE USE FOR PAYROLL 
        //
        public static string G_TempBu = "-";
        public static string G_TempPAHNo = "-";
        public static string G_TempEmpCode = "-";
        public static string G_TempEmpName = "-";
        public static string G_TempPosition = "-";
        public static string G_TempNationality = "-";
        public static string G_TempJoinDate = "-";

        public static double G_TempBasicSalary = 0;
        public static double G_TempSalary = 0; // ** base on reach say, this always is 0
        public static int G_TempWorkingDay = 0;
        public static int G_TempActWorkingDay = 0;

        public static double G_TempWagesPerDay = 0;
        public static double G_TempWagesPerHour = 0;

        public static int G_TempTotalOTHour = 0;
        public static double G_TempTotalOTWages = 0;

        public static int G_TempTotalOTExtHour = 0;
        public static double G_TempTotalOTExtWages = 0;

        public static int G_TempTotalOffDayWorkHour = 0;
        public static double G_TempTotalOffDayWorkWages = 0;

        public static int G_TempTotalPHWorkHour = 0;
        public static double G_TempTotalPHWorkWages = 0;

        public static double G_TempSeniorityBonus = 0;
        public static double G_TempOtherBonus = 0;

        public static double G_TempGrossSalary = 0;

        public static double G_TempNSSF = 0;
        public static double G_TempTax = 0;
        public static double G_TempMealAllowance = 0;
        public static double G_TempTerminationCompensate = 0;
        public static double G_TempSeniorityPreviousYear = 0;
        public static double G_TempSeniorityYear = 0;
        public static double G_TempOtherAdd = 0;
        public static double G_TempOtherDeduct = 0;
        public static double G_TempNetSalary = 0;
        public static string G_IsEntitleOT = "N";
        public static string G_IsResident = "N";



    }
}