using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace NotificationService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EmailTemplates",
                columns: table => new
                {
                    TemplateId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TemplateName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TemplateBody = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TemplateHeader = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SearchSQLCMD = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DepartmentId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailTemplates", x => x.TemplateId);
                });

            migrationBuilder.CreateTable(
                name: "Mailboxes",
                columns: table => new
                {
                    MailboxId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EmployeeEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EmployeeId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mailboxes", x => x.MailboxId);
                });

            migrationBuilder.CreateTable(
                name: "EmailLog",
                columns: table => new
                {
                    EmailLogId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReceiveMail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TemplateId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EmailHeader = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EmailBody = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailLog", x => x.EmailLogId);
                    table.ForeignKey(
                        name: "FK_EmailLog_EmailTemplates_TemplateId",
                        column: x => x.TemplateId,
                        principalTable: "EmailTemplates",
                        principalColumn: "TemplateId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Emails",
                columns: table => new
                {
                    EmailId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Subject = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Body = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Sender = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReceivedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsRead = table.Column<bool>(type: "bit", nullable: false),
                    IsStarred = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    MailboxId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Emails", x => x.EmailId);
                    table.ForeignKey(
                        name: "FK_Emails_Mailboxes_MailboxId",
                        column: x => x.MailboxId,
                        principalTable: "Mailboxes",
                        principalColumn: "MailboxId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EmailManagers",
                columns: table => new
                {
                    EmailManagerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EmailId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ManagerEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ManagerName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Action = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ActionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailManagers", x => x.EmailManagerId);
                    table.ForeignKey(
                        name: "FK_EmailManagers_Emails_EmailId",
                        column: x => x.EmailId,
                        principalTable: "Emails",
                        principalColumn: "EmailId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "EmailTemplates",
                columns: new[] { "TemplateId", "DepartmentId", "SearchSQLCMD", "TemplateBody", "TemplateHeader", "TemplateName" },
                values: new object[,]
                {
                    { new Guid("08695ab8-9021-4564-857a-029c848912e7"), null, "SELECT EmployeeName, RemainingLeaves FROM EmployeeLeaves WHERE EmployeeId = @EmployeeId", "Chào [EmployeeName],<br><br>Bạn hiện có [RemainingLeaves] ngày nghỉ phép còn lại trong năm.<br>Hãy lên kế hoạch nghỉ phép sớm để tránh hết hạn!<br><br>Trân trọng,<br>Phòng Nhân sự", "Nhắc nhở ngày nghỉ phép", "Annual Leave Reminder" },
                    { new Guid("bcb713da-c191-44e7-af41-1a2c82a6f892"), null, "SELECT EmployeeName, WorkingDays, Salary, PayrollLink FROM PayrollData WHERE DepartmentId = @DepartmentId", "Chào [EmployeeName],<br><br>Bạn đã làm việc được [WorkingDays] ngày trong tháng này.<br>Tổng lương của bạn: [Salary] VND.<br>Hãy kiểm tra bảng lương chi tiết tại: <a href=\"[PayrollLink]\">Bảng lương</a><img src=\"[PayrollUrl]\" alt=\"Avatar\" width=\"120\" height=\"120\" style=\"border-radius:50%;\"><br><br>Trân trọng,<br>Phòng Nhân sự", "Thông báo lương tháng", "Payroll Notification" },
                    { new Guid("cf3ddcf8-2f5c-400a-ba5f-b57c5bb9ceea"), null, "SELECT EmployeeName, CompanyName, StartDate, Department, HRContact FROM Employees WHERE EmployeeId = @EmployeeId", "Chào [EmployeeName],<br><br>Chào mừng bạn đến với công ty [CompanyName]!<br>Ngày làm việc đầu tiên của bạn là: [StartDate].<br>Bạn sẽ làm việc tại phòng ban: [Department].<br><br>Hãy liên hệ [HRContact] nếu bạn có bất kỳ thắc mắc nào.<br><br>Chúc bạn một khởi đầu thuận lợi!<br>Phòng Nhân sự", "Chào mừng nhân viên mới", "Welcome New Employee" },
                    { new Guid("f727edc7-b524-4322-946c-578117f6b434"), null, "SELECT EmployeeName, IPAddress FROM SecurityLogs WHERE EmployeeId = @EmployeeId", "Chào [EmployeeName],<br><br>Chúng tôi phát hiện một hoạt động đăng nhập đáng ngờ từ [IPAddress].<br>Nếu đây không phải là bạn, vui lòng đổi mật khẩu ngay lập tức và báo cáo với IT.<br><br>Trân trọng,<br>Phòng IT", "Cảnh báo bảo mật", "IT Security Alert" }
                });

            migrationBuilder.InsertData(
                table: "Mailboxes",
                columns: new[] { "MailboxId", "CreatedAt", "Description", "EmployeeEmail", "EmployeeId", "IsActive", "Name", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("7ae3d307-a006-4b26-b072-54455cf1a63c"), new DateTime(2025, 5, 3, 0, 0, 0, 0, DateTimeKind.Utc), "Mailbox for John Doe", "tranthib@hrm.tv.com", "F5ECDC17-5382-4A81-470C-08DD7F1BD68D", true, "Tran Thi B Mailbox", null },
                    { new Guid("a780e17c-f230-436f-b82e-14407d10ec06"), new DateTime(2025, 5, 3, 0, 0, 0, 0, DateTimeKind.Utc), "Mailbox for Jane Smith", "nguyenvana@hrm.tv.com", "540195F8-B154-4D15-618F-08DD85841FA5", true, "Nguyen Van A Mailbox", null }
                });

            migrationBuilder.InsertData(
                table: "EmailLog",
                columns: new[] { "EmailLogId", "CreatedAt", "EmailBody", "EmailHeader", "ReceiveMail", "Status", "TemplateId" },
                values: new object[,]
                {
                    { new Guid("77f282b8-f20b-46b1-b732-1053b2452ab1"), new DateTime(2025, 4, 4, 9, 0, 0, 0, DateTimeKind.Unspecified), "Chào Alice Smith,<br><br>Chào mừng bạn đến với công ty ABC Corp!<br>Ngày làm việc đầu tiên của bạn là: 2025-04-05.<br>Bạn sẽ làm việc tại phòng ban: Marketing.<br><br>Hãy liên hệ hr@example.com nếu bạn có bất kỳ thắc mắc nào.<br><br>Chúc bạn một khởi đầu thuận lợi!<br>Phòng Nhân sự", "Chào mừng nhân viên mới", "alice.smith@example.com", 0, new Guid("f727edc7-b524-4322-946c-578117f6b434") },
                    { new Guid("a0477076-4085-439c-97c7-4c5a3d87c6f4"), new DateTime(2025, 4, 4, 10, 15, 0, 0, DateTimeKind.Unspecified), "Chào David Nguyen,<br><br>Bạn hiện có 5 ngày nghỉ phép còn lại trong năm.<br>Hãy lên kế hoạch nghỉ phép sớm để tránh hết hạn!<br><br>Trân trọng,<br>Phòng Nhân sự", "Nhắc nhở ngày nghỉ phép", "david.nguyen@example.com", 0, new Guid("08695ab8-9021-4564-857a-029c848912e7") },
                    { new Guid("c032e3a5-3c33-4eb7-98c8-82aad3247005"), new DateTime(2025, 4, 4, 8, 30, 0, 0, DateTimeKind.Unspecified), "Chào John Doe,<br><br>Bạn đã làm việc được 22 ngày trong tháng này.<br>Tổng lương của bạn: 25,000,000 VND.<br>Hãy kiểm tra bảng lương chi tiết tại: <a href=\"https://payroll.example.com\">Bảng lương</a><br><br>Trân trọng,<br>Phòng Nhân sự", "Thông báo lương tháng", "john.doe@example.com", 0, new Guid("bcb713da-c191-44e7-af41-1a2c82a6f892") },
                    { new Guid("ec41061d-570f-4213-bf4a-fc5fe74aabe9"), new DateTime(2025, 4, 4, 11, 0, 0, 0, DateTimeKind.Unspecified), "Chào Security Team,<br><br>Chúng tôi phát hiện một hoạt động đăng nhập đáng ngờ từ IP: 192.168.1.101.<br>Nếu đây không phải là bạn, vui lòng đổi mật khẩu ngay lập tức và báo cáo với IT.<br><br>Trân trọng,<br>Phòng IT", "Cảnh báo bảo mật", "security.team@example.com", 0, new Guid("cf3ddcf8-2f5c-400a-ba5f-b57c5bb9ceea") }
                });

            migrationBuilder.InsertData(
                table: "Emails",
                columns: new[] { "EmailId", "Body", "IsDeleted", "IsRead", "IsStarred", "MailboxId", "ReceivedAt", "Sender", "Status", "Subject" },
                values: new object[,]
                {
                    { new Guid("1fc13791-7ca7-4bbf-8eae-245666ffbcc1"), "Your payroll for April 2025 has been processed.", false, true, true, new Guid("7ae3d307-a006-4b26-b072-54455cf1a63c"), new DateTime(2025, 5, 1, 12, 0, 0, 0, DateTimeKind.Utc), "nguyenvana@hrm.tv.com", 1, "Payroll Processed" },
                    { new Guid("62d38853-4303-425b-af87-1c41a9607c1c"), "Reminder: Weekly team meeting on Monday at 9 AM.", false, false, false, new Guid("a780e17c-f230-436f-b82e-14407d10ec06"), new DateTime(2025, 5, 3, 7, 45, 0, 0, DateTimeKind.Utc), "tranthib@hrm.tv.com", 0, "Weekly Meeting Reminder" },
                    { new Guid("edc6ce3a-f39d-4046-8492-566ff41ac305"), "Hi Tran Thi B, welcome to HRM-THANHVU!", false, false, false, new Guid("7ae3d307-a006-4b26-b072-54455cf1a63c"), new DateTime(2025, 5, 2, 8, 30, 0, 0, DateTimeKind.Utc), "nguyenvana@hrm.tv.com", 0, "Welcome to the team" }
                });

            migrationBuilder.InsertData(
                table: "EmailManagers",
                columns: new[] { "EmailManagerId", "Action", "ActionDate", "EmailId", "ManagerEmail", "ManagerName", "Notes" },
                values: new object[,]
                {
                    { new Guid("6d7bfcf4-26e0-4f18-bd3e-c2f616e1e509"), "Reply", new DateTime(2025, 5, 3, 8, 0, 0, 0, DateTimeKind.Utc), new Guid("62d38853-4303-425b-af87-1c41a9607c1c"), "nguyenvana@hrm.tv.com", "Nguyen Van A", "Confirmed attendance" },
                    { new Guid("8727d68f-52cb-4e1e-ac38-ac16b195d9c5"), "Read", new DateTime(2025, 5, 2, 9, 0, 0, 0, DateTimeKind.Utc), new Guid("edc6ce3a-f39d-4046-8492-566ff41ac305"), "nguyenvana@hrm.tv.com", "Pham Van A", "Opened email upon arrival" },
                    { new Guid("ce9a87d5-11ac-4179-b78e-dc87c6dc8759"), "Forward", new DateTime(2025, 5, 1, 12, 30, 0, 0, DateTimeKind.Utc), new Guid("1fc13791-7ca7-4bbf-8eae-245666ffbcc1"), "nguyenvana@hrm.tv.com", "Nguyen Van A", "Forwarded to finance department" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmailLog_TemplateId",
                table: "EmailLog",
                column: "TemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_EmailManagers_EmailId",
                table: "EmailManagers",
                column: "EmailId");

            migrationBuilder.CreateIndex(
                name: "IX_Emails_MailboxId",
                table: "Emails",
                column: "MailboxId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmailLog");

            migrationBuilder.DropTable(
                name: "EmailManagers");

            migrationBuilder.DropTable(
                name: "EmailTemplates");

            migrationBuilder.DropTable(
                name: "Emails");

            migrationBuilder.DropTable(
                name: "Mailboxes");
        }
    }
}
