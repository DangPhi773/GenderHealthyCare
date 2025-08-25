using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessObjects.Migrations
{
    /// <inheritdoc />
    public partial class dbbb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Clinic",
                columns: table => new
                {
                    clinic_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValue: false),
                    image = table.Column<byte[]>(type: "varbinary(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Clinic__A0C8D19B9D0B187E", x => x.clinic_id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    user_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    role = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    gender = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    dob = table.Column<DateOnly>(type: "date", nullable: true),
                    username = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    password_hash = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    full_name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    phone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    updated_at = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Users__B9BE370F4F19A88E", x => x.user_id);
                });

            migrationBuilder.CreateTable(
                name: "Blog",
                columns: table => new
                {
                    blog_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    title = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    author_id = table.Column<int>(type: "int", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    updated_at = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Blog__2975AA2849F7A4FE", x => x.blog_id);
                    table.ForeignKey(
                        name: "FK__Blog__author_id__440B1D61",
                        column: x => x.author_id,
                        principalTable: "Users",
                        principalColumn: "user_id");
                });

            migrationBuilder.CreateTable(
                name: "ConsultantInfo",
                columns: table => new
                {
                    consultant_id = table.Column<int>(type: "int", nullable: false),
                    qualifications = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    experience_years = table.Column<int>(type: "int", nullable: true),
                    specialization = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    profile_image = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Consulta__680695C48048D0A4", x => x.consultant_id);
                    table.ForeignKey(
                        name: "FK__Consultan__consu__693CA210",
                        column: x => x.consultant_id,
                        principalTable: "Users",
                        principalColumn: "user_id");
                });

            migrationBuilder.CreateTable(
                name: "Consultations",
                columns: table => new
                {
                    consultation_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    user_id = table.Column<int>(type: "int", nullable: false),
                    consultant_id = table.Column<int>(type: "int", nullable: false),
                    appointment_time = table.Column<DateTime>(type: "datetime", nullable: false),
                    status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true, defaultValue: "Pending"),
                    meeting_link = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Consulta__650FE0FBB43EB8B4", x => x.consultation_id);
                    table.ForeignKey(
                        name: "FK__Consultat__consu__5812160E",
                        column: x => x.consultant_id,
                        principalTable: "Users",
                        principalColumn: "user_id");
                    table.ForeignKey(
                        name: "FK__Consultat__user___571DF1D5",
                        column: x => x.user_id,
                        principalTable: "Users",
                        principalColumn: "user_id");
                });

            migrationBuilder.CreateTable(
                name: "MenstrualCycle",
                columns: table => new
                {
                    cycle_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    user_id = table.Column<int>(type: "int", nullable: false),
                    start_date = table.Column<DateOnly>(type: "date", nullable: true),
                    end_date = table.Column<DateOnly>(type: "date", nullable: true),
                    ovulation_date = table.Column<DateOnly>(type: "date", nullable: true),
                    pill_reminder_time = table.Column<TimeOnly>(type: "time", nullable: true),
                    notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Menstrua__5D955881C4465051", x => x.cycle_id);
                    table.ForeignKey(
                        name: "FK__Menstrual__user___4AB81AF0",
                        column: x => x.user_id,
                        principalTable: "Users",
                        principalColumn: "user_id");
                });

            migrationBuilder.CreateTable(
                name: "Questions",
                columns: table => new
                {
                    question_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    user_id = table.Column<int>(type: "int", nullable: false),
                    consultant_id = table.Column<int>(type: "int", nullable: true),
                    question_text = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    answer_text = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true, defaultValue: "Pending"),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValue: false),
                    answered_at = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Question__2EC2154903394F92", x => x.question_id);
                    table.ForeignKey(
                        name: "FK__Questions__consu__5EBF139D",
                        column: x => x.consultant_id,
                        principalTable: "Users",
                        principalColumn: "user_id");
                    table.ForeignKey(
                        name: "FK__Questions__user___5DCAEF64",
                        column: x => x.user_id,
                        principalTable: "Users",
                        principalColumn: "user_id");
                });

            migrationBuilder.CreateTable(
                name: "Reports",
                columns: table => new
                {
                    report_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    report_type = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    generated_by = table.Column<int>(type: "int", nullable: true),
                    report_data = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Reports__779B7C58D022B354", x => x.report_id);
                    table.ForeignKey(
                        name: "FK__Reports__generat__797309D9",
                        column: x => x.generated_by,
                        principalTable: "Users",
                        principalColumn: "user_id");
                });

            migrationBuilder.CreateTable(
                name: "Service",
                columns: table => new
                {
                    service_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false),
                    consultant_id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Service__3E0DB8AF1C8C5C85", x => x.service_id);
                    table.ForeignKey(
                        name: "FK_Service_Users_ConsultantId",
                        column: x => x.consultant_id,
                        principalTable: "Users",
                        principalColumn: "user_id");
                });

            migrationBuilder.CreateTable(
                name: "UserHistory",
                columns: table => new
                {
                    history_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    user_id = table.Column<int>(type: "int", nullable: false),
                    action_type = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    action_id = table.Column<int>(type: "int", nullable: false),
                    action_date = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__UserHist__096AA2E9F19E0CB4", x => x.history_id);
                    table.ForeignKey(
                        name: "FK__UserHisto__user___74AE54BC",
                        column: x => x.user_id,
                        principalTable: "Users",
                        principalColumn: "user_id");
                });

            migrationBuilder.CreateTable(
                name: "Reminders",
                columns: table => new
                {
                    reminder_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    user_id = table.Column<int>(type: "int", nullable: false),
                    cycle_id = table.Column<int>(type: "int", nullable: true),
                    reminder_type = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    reminder_time = table.Column<DateTime>(type: "datetime", nullable: false),
                    message = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true, defaultValue: "Pending"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Reminder__E27A362831954370", x => x.reminder_id);
                    table.ForeignKey(
                        name: "FK__Reminders__cycle__5165187F",
                        column: x => x.cycle_id,
                        principalTable: "MenstrualCycle",
                        principalColumn: "cycle_id");
                    table.ForeignKey(
                        name: "FK__Reminders__user___5070F446",
                        column: x => x.user_id,
                        principalTable: "Users",
                        principalColumn: "user_id");
                });

            migrationBuilder.CreateTable(
                name: "Tests",
                columns: table => new
                {
                    test_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    user_id = table.Column<int>(type: "int", nullable: false),
                    service_id = table.Column<int>(type: "int", nullable: false),
                    appointment_time = table.Column<DateTime>(type: "datetime", nullable: false),
                    status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true, defaultValue: "Pending"),
                    result = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    cancel_reason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "(getdate())"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Tests__F3FF1C026A3B788C", x => x.test_id);
                    table.ForeignKey(
                        name: "FK__Tests__service_i__656C112C",
                        column: x => x.service_id,
                        principalTable: "Service",
                        principalColumn: "service_id");
                    table.ForeignKey(
                        name: "FK__Tests__user_id__6477ECF3",
                        column: x => x.user_id,
                        principalTable: "Users",
                        principalColumn: "user_id");
                });

            migrationBuilder.CreateTable(
                name: "Feedback",
                columns: table => new
                {
                    feedback_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    user_id = table.Column<int>(type: "int", nullable: false),
                    consultant_id = table.Column<int>(type: "int", nullable: true),
                    service_id = table.Column<int>(type: "int", nullable: true),
                    rating = table.Column<int>(type: "int", nullable: true),
                    feedback_text = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true, defaultValue: false),
                    test_id = table.Column<int>(type: "int", nullable: true),
                    consultation_id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Feedback__7A6B2B8C73DE0888", x => x.feedback_id);
                    table.ForeignKey(
                        name: "FK_Feedback_Consultation",
                        column: x => x.consultation_id,
                        principalTable: "Consultations",
                        principalColumn: "consultation_id");
                    table.ForeignKey(
                        name: "FK_Feedback_Test",
                        column: x => x.test_id,
                        principalTable: "Tests",
                        principalColumn: "test_id");
                    table.ForeignKey(
                        name: "FK__Feedback__consul__6EF57B66",
                        column: x => x.consultant_id,
                        principalTable: "Users",
                        principalColumn: "user_id");
                    table.ForeignKey(
                        name: "FK__Feedback__servic__6FE99F9F",
                        column: x => x.service_id,
                        principalTable: "Service",
                        principalColumn: "service_id");
                    table.ForeignKey(
                        name: "FK__Feedback__user_i__6E01572D",
                        column: x => x.user_id,
                        principalTable: "Users",
                        principalColumn: "user_id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Blog_author_id",
                table: "Blog",
                column: "author_id");

            migrationBuilder.CreateIndex(
                name: "IX_Consultations_consultant_id",
                table: "Consultations",
                column: "consultant_id");

            migrationBuilder.CreateIndex(
                name: "IX_Consultations_user_id",
                table: "Consultations",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_Feedback_consultant_id",
                table: "Feedback",
                column: "consultant_id");

            migrationBuilder.CreateIndex(
                name: "IX_Feedback_consultation_id",
                table: "Feedback",
                column: "consultation_id");

            migrationBuilder.CreateIndex(
                name: "IX_Feedback_service_id",
                table: "Feedback",
                column: "service_id");

            migrationBuilder.CreateIndex(
                name: "IX_Feedback_test_id",
                table: "Feedback",
                column: "test_id");

            migrationBuilder.CreateIndex(
                name: "IX_Feedback_user_id",
                table: "Feedback",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_MenstrualCycle_user_id",
                table: "MenstrualCycle",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_Questions_consultant_id",
                table: "Questions",
                column: "consultant_id");

            migrationBuilder.CreateIndex(
                name: "IX_Questions_user_id",
                table: "Questions",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_Reminders_cycle_id",
                table: "Reminders",
                column: "cycle_id");

            migrationBuilder.CreateIndex(
                name: "IX_Reminders_user_id",
                table: "Reminders",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_generated_by",
                table: "Reports",
                column: "generated_by");

            migrationBuilder.CreateIndex(
                name: "IX_Service_consultant_id",
                table: "Service",
                column: "consultant_id");

            migrationBuilder.CreateIndex(
                name: "IX_Tests_service_id",
                table: "Tests",
                column: "service_id");

            migrationBuilder.CreateIndex(
                name: "IX_Tests_user_id",
                table: "Tests",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_UserHistory_user_id",
                table: "UserHistory",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "UQ__Users__AB6E6164289B1418",
                table: "Users",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ__Users__F3DBC57270882F4A",
                table: "Users",
                column: "username",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Blog");

            migrationBuilder.DropTable(
                name: "Clinic");

            migrationBuilder.DropTable(
                name: "ConsultantInfo");

            migrationBuilder.DropTable(
                name: "Feedback");

            migrationBuilder.DropTable(
                name: "Questions");

            migrationBuilder.DropTable(
                name: "Reminders");

            migrationBuilder.DropTable(
                name: "Reports");

            migrationBuilder.DropTable(
                name: "UserHistory");

            migrationBuilder.DropTable(
                name: "Consultations");

            migrationBuilder.DropTable(
                name: "Tests");

            migrationBuilder.DropTable(
                name: "MenstrualCycle");

            migrationBuilder.DropTable(
                name: "Service");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
