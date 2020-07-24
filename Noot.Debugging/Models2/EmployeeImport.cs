using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;

namespace Noot.DataAccess.Models
{
    [Table("EmployeeImport", Schema = "com")]
    public partial class EmployeeImport
    {
        public EmployeeImport()
        {
        }
        [StringLength(16, ErrorMessage = "Engagement Date cannot exceed more than 16 characters.")]
        public string Engagement_Date { get; set; }

        [StringLength(6, ErrorMessage = "Territory cannot exceed more than 6 characters.")]
        public string Territory { get; set; }

        [StringLength(6, ErrorMessage = "Homeland cannot exceed more than 6 characters.")]
        public string Homeland { get; set; }

        [StringLength(4, ErrorMessage = "Acclimatisation cannot exceed more than 4 characters.")]
        public string Acclimatisation { get; set; }

        [StringLength(6, ErrorMessage = "Nation cannot exceed more than 6 characters.")]
        public string Nation { get; set; }

        [StringLength(40, ErrorMessage = "Surname cannot exceed more than 40 characters.")]
        public string Surname { get; set; }

        [StringLength(36, ErrorMessage = "First name cannot exceed more than 36 characters.")]
        public string FirstName { get; set; }

        [StringLength(8, ErrorMessage = "Initials cannot exceed more than 8 characters.")]
        public string Initials { get; set; }

        [StringLength(4, ErrorMessage = "Age cannot exceed more than 4 characters.")]
        public string Age { get; set; }

        [StringLength(12, ErrorMessage = "DPO code cannot exceed more than 12 characters.")]
        public string DPOCode { get; set; }

        [StringLength(36, ErrorMessage = "Current gang cannot exceed more than 36 characters.")]
        public string CurrentGang { get; set; }

        [StringLength(10, ErrorMessage = "Wage code cannot exceed more than 10 characters.")]
        public string WageCode { get; set; }

        [StringLength(2, ErrorMessage = "Work Type cannot exceed more than 2 characters.")]
        public string Work_Type { get; set; }

        [StringLength(12, ErrorMessage = "Hostel cannot exceed more than 12 characters.")]
        public string Hostel { get; set; }

        [StringLength(16, ErrorMessage = "Repatriation cannot exceed more than 16 characters.")]
        public string Repatriation { get; set; }

        [StringLength(6, ErrorMessage = "Patterson Grade cannot exceed more than 6 characters.")]
        public string Patterson_Grade { get; set; }

        [StringLength(16, ErrorMessage = "Leave Due cannot exceed more than 16 characters.")]
        public string Leave_Due { get; set; }

        [StringLength(16, ErrorMessage = "Appointed Date cannot exceed more than 16 characters.")]
        public string Appointed_Date { get; set; }

        [StringLength(32, ErrorMessage = "Current cost code cannot exceed more than 32 characters.")]
        public string CurrentCostCode { get; set; }

        [StringLength(2, ErrorMessage = "Acting Indicator cannot exceed more than 2 characters.")]
        public string Acting_Indicator { get; set; }

        [StringLength(16, ErrorMessage = "Acting End cannot exceed more than 16 characters.")]
        public string Acting_End { get; set; }

        [StringLength(2, ErrorMessage = "Employee Class cannot exceed more than 2 characters.")]
        public string Employee_Class { get; set; }

        [StringLength(36, ErrorMessage = "Appointed gang cannot exceed more than 36 characters.")]
        public string AppointedGang { get; set; }

        [StringLength(16, ErrorMessage = "Activity cannot exceed more than 16 characters.")]
        public string Activity { get; set; }

        [StringLength(16, ErrorMessage = "Reengagement cannot exceed more than 16 characters.")]
        public string Reengagement { get; set; }

        [StringLength(4, ErrorMessage = "Gang Shift Type cannot exceed more than 4 characters.")]
        public string Gang_Shift_Type { get; set; }

        [StringLength(2, ErrorMessage = "Gender cannot exceed more than 2 characters.")]
        public string Gender { get; set; }

        [StringLength(6, ErrorMessage = "Union Code cannot exceed more than 6 characters.")]
        public string Union_Code { get; set; }

        [StringLength(16, ErrorMessage = "Methane cannot exceed more than 16 characters.")]
        public string Methane { get; set; }

        [StringLength(16, ErrorMessage = "First aid cannot exceed more than 16 characters.")]
        public string FirstAid { get; set; }

        [StringLength(16, ErrorMessage = "Last Transfer cannot exceed more than 16 characters.")]
        public string Last_Transfer { get; set; }

        [StringLength(16, ErrorMessage = "Act Return cannot exceed more than 16 characters.")]
        public string Act_Return { get; set; }

        [StringLength(4, ErrorMessage = "Mine Service cannot exceed more than 4 characters.")]
        public string Mine_Service { get; set; }

        [StringLength(6, ErrorMessage = "Leave Days cannot exceed more than 6 characters.")]
        public string Leave_Days { get; set; }

        [StringLength(16, ErrorMessage = "Leave End cannot exceed more than 16 characters.")]
        public string Leave_End { get; set; }

        [StringLength(6, ErrorMessage = "Unpaid Days cannot exceed more than 6 characters.")]
        public string Unpaid_Days { get; set; }

        [StringLength(62, ErrorMessage = "Other name cannot exceed more than 62 characters.")]
        public string OtherName { get; set; }

        [StringLength(16, ErrorMessage = "Birth date cannot exceed more than 16 characters.")]
        public string BirthDate { get; set; }

        [StringLength(2, ErrorMessage = "Maritsta cannot exceed more than 2 characters.")]
        public string Maritsta { get; set; }

        [StringLength(16, ErrorMessage = "Xry date cannot exceed more than 16 characters.")]
        public string XryDate { get; set; }

        [StringLength(26, ErrorMessage = "Id doc no cannot exceed more than 26 characters.")]
        public string IdDocNo { get; set; }

        [StringLength(16, ErrorMessage = "Xry dud t cannot exceed more than 16 characters.")]
        public string XryDudT { get; set; }

        [StringLength(2, ErrorMessage = "Emp type cannot exceed more than 2 characters.")]
        public string EmpType { get; set; }

        [StringLength(16, ErrorMessage = "Burcrdte cannot exceed more than 16 characters.")]
        public string Burcrdte { get; set; }

        [StringLength(60, ErrorMessage = "Addr 1 cannot exceed more than 60 characters.")]
        public string Addr1 { get; set; }

        [StringLength(60, ErrorMessage = "Addr 2 cannot exceed more than 60 characters.")]
        public string Addr2 { get; set; }

        [StringLength(60, ErrorMessage = "Addr 3 cannot exceed more than 60 characters.")]
        public string Addr3 { get; set; }

        [StringLength(60, ErrorMessage = "Addr 4 cannot exceed more than 60 characters.")]
        public string Addr4 { get; set; }

        [StringLength(8, ErrorMessage = "P code cannot exceed more than 8 characters.")]
        public string PCode { get; set; }

        [StringLength(20, ErrorMessage = "Tel no cannot exceed more than 20 characters.")]
        public string TelNo { get; set; }

        [StringLength(4, ErrorMessage = "Ind serv cannot exceed more than 4 characters.")]
        public string Ind_serv { get; set; }

        [StringLength(2, ErrorMessage = "Mine Emp cannot exceed more than 2 characters.")]
        public string Mine_Emp { get; set; }

        [StringLength(12, ErrorMessage = "Room No cannot exceed more than 12 characters.")]
        public string Room_No { get; set; }

        [StringLength(16, ErrorMessage = "Pln lve dt cannot exceed more than 16 characters.")]
        public string PlnLveDt { get; set; }

        [StringLength(16, ErrorMessage = "Pln rtn dt cannot exceed more than 16 characters.")]
        public string PlnRtnDt { get; set; }

        [StringLength(16, ErrorMessage = "Lv est dt cannot exceed more than 16 characters.")]
        public string LvEstDt { get; set; }

        [StringLength(4, ErrorMessage = "Anglo Serv cannot exceed more than 4 characters.")]
        public string Anglo_Serv { get; set; }

        [StringLength(2, ErrorMessage = "Roster ind cannot exceed more than 2 characters.")]
        public string RosterInd { get; set; }

        [StringLength(14, ErrorMessage = "Hospital cannot exceed more than 14 characters.")]
        public string Hospital { get; set; }

        [StringLength(4, ErrorMessage = "Dependants cannot exceed more than 4 characters.")]
        public string Dependants { get; set; }

        [StringLength(6, ErrorMessage = "District cannot exceed more than 6 characters.")]
        public string District { get; set; }

        [StringLength(6, ErrorMessage = "UAO Code cannot exceed more than 6 characters.")]
        public string UAO_Code { get; set; }

        [StringLength(4, ErrorMessage = "Med Class cannot exceed more than 4 characters.")]
        public string Med_Class { get; set; }

        [StringLength(16, ErrorMessage = "Ind Number cannot exceed more than 16 characters.")]
        public string Ind_Number { get; set; }

        [StringLength(16, ErrorMessage = "Bur no cannot exceed more than 16 characters.")]
        public string BurNo { get; set; }

        [StringLength(2, ErrorMessage = "Emp Status cannot exceed more than 2 characters.")]
        public string Emp_Status { get; set; }

        [StringLength(6, ErrorMessage = "Work Know cannot exceed more than 6 characters.")]
        public string Work_Know { get; set; }

        [StringLength(2, ErrorMessage = "Rost ind 2 cannot exceed more than 2 characters.")]
        public string RostInd2 { get; set; }

        [StringLength(16, ErrorMessage = "Contract Expiry cannot exceed more than 16 characters.")]
        public string Contract_Expiry { get; set; }

        [StringLength(10, ErrorMessage = "Num Activity cannot exceed more than 10 characters.")]
        public string Num_Activity { get; set; }

        [StringLength(16, ErrorMessage = "Num Act Date cannot exceed more than 16 characters.")]
        public string Num_Act_Date { get; set; }

        [StringLength(32, ErrorMessage = "Appointed cost code cannot exceed more than 32 characters.")]
        public string AppointedCostCode { get; set; }

        [StringLength(18, ErrorMessage = "ID Date cannot exceed more than 18 characters.")]
        public string ID_Date { get; set; }

        [StringLength(16, ErrorMessage = "Alloc Dte cannot exceed more than 16 characters.")]
        public string Alloc_Dte { get; set; }

        [StringLength(8, ErrorMessage = "Arithmetic cannot exceed more than 8 characters.")]
        public string Arithmetic { get; set; }

        [StringLength(8, ErrorMessage = "Language cannot exceed more than 8 characters.")]
        public string Language { get; set; }

        [StringLength(2, ErrorMessage = "Ravens Stanine cannot exceed more than 2 characters.")]
        public string Ravens_Stanine { get; set; }

        [StringLength(4, ErrorMessage = "Ravens Result cannot exceed more than 4 characters.")]
        public string Ravens_Result { get; set; }

        [StringLength(2, ErrorMessage = "Emp Eq Ind cannot exceed more than 2 characters.")]
        public string Emp_Eq_Ind { get; set; }

        [StringLength(6, ErrorMessage = "Potential Pat cannot exceed more than 6 characters.")]
        public string Potential_Pat { get; set; }

        [StringLength(16, ErrorMessage = "Patterson Dte cannot exceed more than 16 characters.")]
        public string Patterson_Dte { get; set; }

        [StringLength(10, ErrorMessage = "Potential Wge cannot exceed more than 10 characters.")]
        public string Potential_Wge { get; set; }

        [StringLength(2, ErrorMessage = "Medical Fit cannot exceed more than 2 characters.")]
        public string Medical_Fit { get; set; }

        [StringLength(2, ErrorMessage = "Disability cannot exceed more than 2 characters.")]
        public string Disability { get; set; }

        [StringLength(68, ErrorMessage = "Wage Description cannot exceed more than 68 characters.")]
        public string Wage_Description { get; set; }

        [StringLength(18, ErrorMessage = "Contract Date cannot exceed more than 18 characters.")]
        public string Contract_Date { get; set; }

        [StringLength(6, ErrorMessage = "Qual High cannot exceed more than 6 characters.")]
        public string Qual_High { get; set; }

        [StringLength(16, ErrorMessage = "Dte App DP cannot exceed more than 16 characters.")]
        public string Dte_App_DP { get; set; }

    }
}

