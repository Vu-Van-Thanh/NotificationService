using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Math;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.ComponentModel.DataAnnotations;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;



namespace NotificationService.Core.Domain.Entities
{
    public class EmailTemplate
    {
        [Key]
        public Guid TemplateId { get; set; } 
        public string TemplateName { get; set; }

        public string TemplateBody { get; set; }  
        public string TemplateHeader { get; set; }
        public string? SearchSQLCMD { get; set; }
        public string? DepartmentId { get; set; }

        public virtual ICollection<EmailLog>? EmailLogs { get; set; }
    }

   /* TemplateId TemplateName    TemplateBody TemplateHeader  SearchSQLCMD DepartmentId
F282C36D-8E6A-49C3-9CD4-46AB090D1615 Payment Notification<!DOCTYPE html>
<html lang = "vi" >
< head >
  < meta charset="UTF-8">
  <title>Thông báo lương</title>
</head>
<body style = "font-family: Arial, sans-serif; line-height: 1.6; color: #333;" >
  < h2 > Thông báo Lương Tháng[Day] – [EmployeeName]</h2>

  <p>Kính gửi Anh/Chị<strong>[EmployeeName]</strong>,</p>

  <p>Phòng Nhân sự/Phòng Tài chính xin thông báo đến Anh/Chị thông tin chi tiết về lương tháng<strong>[Day]</strong> như sau:</p>

  <h3>1. Thông tin chung:</h3>
  <ul>
    <li><strong>Họ và tên:</strong> [EmployeeName] </li>
    <li><strong>Chức vụ:</strong> [Position] </li>
    <li><strong>Phòng ban:</strong> [DepartmentId] </li>
    <li><strong>Mã nhân viên:</strong> [EmployeeID] </li>
  </ul>

  <h3>2. Chi tiết lương:</h3>
  <ul>
    <li><strong>Lương cơ bản:</strong> [BaseSalary] </li>
    <li><strong>Phụ cấp:</strong> [Adjustment] </li>
    <li><strong>Thưởng (nếu có):</strong> [Bonus] </li>
    <li><strong>Khấu trừ (nếu có):</strong> [Deduction] </li>
    <li><strong>Lương thực nhận:</strong> <span style = "color: green;" >< strong > [NetSalary] </ strong ></ span ></ li >
  </ ul >

  < p > Mọi thắc mắc liên quan đến bảng lương, vui lòng liên hệ phòng Nhân sự hoặc phòng Kế toán để được hỗ trợ.</p>

  <p>Trân trọng,<br>
  <strong>Phòng Nhân sự / Tài chính</strong></p>
</body>
</html>	Thông báo lương[Day] stage 0 : { SELECT firstname +lastname as EmployeeName, FORMAT(GETDATE(), 'dd/MM/yyyy') AS Day, Position, DepartmentId from Employees where EmployeeID = @EmployeeID} {}
{ EmployeeName,Position,DepartmentId,Day}
{ } || stage 1 : {
    SELECT
    sb.BaseSalary,
    sb.EmployeeID,
    MAX(sb.baseindex) AS BaseIndex,
    SUM(CASE
            WHEN sa.AdjustType = 1 THEN
                CASE
                    WHEN sa.Percentage IS NOT NULL THEN sb.BaseSalary * (sa.Percentage / 100)
                    WHEN sa.Amount IS NOT NULL THEN sa.Amount
                    ELSE 0
                END
            ELSE 0
        END) AS Deduction,
    SUM(CASE
            WHEN sa.AdjustType = 0 THEN
                CASE
                    WHEN sa.Percentage IS NOT NULL THEN sb.BaseSalary * (sa.Percentage / 100)
                    WHEN sa.Amount IS NOT NULL THEN sa.Amount
                    ELSE 0
                END
            ELSE 0
        END) AS Bonus,
    SUM(CASE
            WHEN sa.AdjustType = 2 THEN
                CASE
                    WHEN sa.Percentage IS NOT NULL THEN sb.BaseSalary * (sa.Percentage / 100)
                    WHEN sa.Amount IS NOT NULL THEN sa.Amount
                    ELSE 0
                END
            ELSE 0
        END) AS Adjustment,
    sb.BaseSalary* MAX(sb.baseindex) +
    SUM(CASE
            WHEN sa.AdjustType = 0 THEN
                CASE
                    WHEN sa.Percentage IS NOT NULL THEN sb.BaseSalary * (sa.Percentage / 100)
                    WHEN sa.Amount IS NOT NULL THEN sa.Amount
                    ELSE 0
                END
            ELSE 0
        END) +
    SUM(CASE
            WHEN sa.AdjustType = 2 THEN
                CASE
                    WHEN sa.Percentage IS NOT NULL THEN sb.BaseSalary * (sa.Percentage / 100)
                    WHEN sa.Amount IS NOT NULL THEN sa.Amount
                    ELSE 0
                END
            ELSE 0
        END) -
    SUM(CASE
            WHEN sa.AdjustType = 1 THEN
                CASE
                    WHEN sa.Percentage IS NOT NULL THEN sb.BaseSalary * (sa.Percentage / 100)
                    WHEN sa.Amount IS NOT NULL THEN sa.Amount
                    ELSE 0
                END
            ELSE 0
        END) AS NetSalary
from
   SalaryBases sb
JOIN
    SalaryAdjustments sa
    ON sb.salaryId = sa.BaseId
WHERE
    sb.EmployeeID = @EmployeeID
GROUP BY
    sb.BaseSalary, sb.EmployeeID}
{ }
{ BaseSalary,EmployeeID,BaseIndex,NetSalary,Adjustment,Deduction,Bonus}
{ } || NULL*/
}
