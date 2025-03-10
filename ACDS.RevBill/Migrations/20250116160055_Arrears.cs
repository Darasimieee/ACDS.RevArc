using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ACDS.RevBill.Migrations
{
    /// <inheritdoc />
    public partial class Arrears : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ActivityBillStatusId",
                table: "Billing",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ApprovalBillStatusId",
                table: "Billing",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "BillPreApprovalId",
                table: "Billing",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Billing",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Arrears",
                columns: table => new
                {
                    ArrearId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrganisationId = table.Column<int>(type: "int", nullable: false),
                    Year = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Percentage = table.Column<int>(type: "int", nullable: false),
                    ArrearsApplicable = table.Column<bool>(type: "bit", nullable: false),
                    InterestApplicable = table.Column<bool>(type: "bit", nullable: false),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TenantName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Arrears", x => x.ArrearId);
                    table.ForeignKey(
                        name: "FK_Arrears_Organisations_OrganisationId",
                        column: x => x.OrganisationId,
                        principalTable: "Organisations",
                        principalColumn: "OrganisationId",
                        onDelete: ReferentialAction.Cascade);
                });

            //migrationBuilder.CreateTable(
            //    name: "BillPreApproval",
            //    columns: table => new
            //    {
            //        BillPreApprovalId = table.Column<long>(type: "bigint", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        BillId = table.Column<long>(type: "bigint", nullable: false),
            //        OrganisationId = table.Column<int>(type: "int", nullable: false),
            //        BillReferenceNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        HarmonizedBillReferenceNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        PropertyId = table.Column<int>(type: "int", nullable: true),
            //        CustomerId = table.Column<int>(type: "int", nullable: false),
            //        AgencyId = table.Column<int>(type: "int", nullable: false),
            //        BusinessSizeId = table.Column<int>(type: "int", nullable: false),
            //        RevenueId = table.Column<int>(type: "int", nullable: false),
            //        BillAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
            //        BillArrears = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
            //        Billbf = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
            //        AmountPaid = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
            //        FrequencyId = table.Column<int>(type: "int", nullable: true),
            //        BillTypeId = table.Column<int>(type: "int", nullable: false),
            //        BusinessTypeId = table.Column<int>(type: "int", nullable: false),
            //        AppliedDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        Category = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        BillStatusId = table.Column<int>(type: "int", nullable: false),
            //        ApprovalBillStatusId = table.Column<int>(type: "int", nullable: true),
            //        ActivityBillStatusId = table.Column<int>(type: "int", nullable: false),
            //        Year = table.Column<int>(type: "int", nullable: false),
            //        DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
            //        CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        DateModified = table.Column<DateTime>(type: "datetime2", nullable: true),
            //        ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        UserId = table.Column<int>(type: "int", nullable: true),
            //        SerialNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        HarmonizeStore = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        TenantName = table.Column<string>(type: "nvarchar(max)", nullable: true)
            //    },
                //constraints: table =>
                //{
                //    table.PrimaryKey("PK_BillPreApproval", x => x.BillPreApprovalId);
                //    table.ForeignKey(
                //        name: "FK_BillPreApproval_Agencies_AgencyId",
                //        column: x => x.AgencyId,
                //        principalTable: "Agencies",
                //        principalColumn: "AgencyId",
                //        onDelete: ReferentialAction.Cascade);
                //    table.ForeignKey(
                //        name: "FK_BillPreApproval_BillStatus_BillStatusId",
                //        column: x => x.BillStatusId,
                //        principalTable: "BillStatus",
                //        principalColumn: "BillStatusId",
                //        onDelete: ReferentialAction.Cascade);
                //    table.ForeignKey(
                //        name: "FK_BillPreApproval_BillType_BillTypeId",
                //        column: x => x.BillTypeId,
                //        principalTable: "BillType",
                //        principalColumn: "BillTypeId",
                //        onDelete: ReferentialAction.Cascade);
                //    table.ForeignKey(
                //        name: "FK_BillPreApproval_BusinessSizes_BusinessSizeId",
                //        column: x => x.BusinessSizeId,
                //        principalTable: "BusinessSizes",
                //        principalColumn: "BusinessSizeId",
                //        onDelete: ReferentialAction.Cascade);
                //    table.ForeignKey(
                //        name: "FK_BillPreApproval_BusinessTypes_BusinessTypeId",
                //        column: x => x.BusinessTypeId,
                //        principalTable: "BusinessTypes",
                //        principalColumn: "BusinessTypeId",
                //        onDelete: ReferentialAction.Cascade);
                //    table.ForeignKey(
                //        name: "FK_BillPreApproval_Customers_CustomerId",
                //        column: x => x.CustomerId,
                //        principalTable: "Customers",
                //        principalColumn: "CustomerId",
                //        onDelete: ReferentialAction.Cascade);
                //    table.ForeignKey(
                //        name: "FK_BillPreApproval_Frequencies_FrequencyId",
                //        column: x => x.FrequencyId,
                //        principalTable: "Frequencies",
                //        principalColumn: "FrequencyId");
                //    table.ForeignKey(
                //        name: "FK_BillPreApproval_Organisations_OrganisationId",
                //        column: x => x.OrganisationId,
                //        principalTable: "Organisations",
                //        principalColumn: "OrganisationId",
                //        onDelete: ReferentialAction.Cascade);
                //    table.ForeignKey(
                //        name: "FK_BillPreApproval_Properties_PropertyId",
                //        column: x => x.PropertyId,
                //        principalTable: "Properties",
                //        principalColumn: "PropertyId");
                //    table.ForeignKey(
                //        name: "FK_BillPreApproval_Revenues_RevenueId",
                //        column: x => x.RevenueId,
                //        principalTable: "Revenues",
                //        principalColumn: "RevenueId",
                //        onDelete: ReferentialAction.Cascade);
                //    table.ForeignKey(
                //        name: "FK_BillPreApproval_Users_UserId",
                //        column: x => x.UserId,
                //        principalTable: "Users",
                //        principalColumn: "UserId");
                //});

            //migrationBuilder.CreateTable(
            //    name: "OrganisationCustomers",
            //    columns: table => new
            //    {
            //        OrganisationCustomerId = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        OrganisationId = table.Column<int>(type: "int", nullable: false),
            //        DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
            //        CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        DateModified = table.Column<DateTime>(type: "datetime2", nullable: true),
            //        ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        TenantName = table.Column<string>(type: "nvarchar(max)", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_OrganisationCustomers", x => x.OrganisationCustomerId);
            //    });

            migrationBuilder.UpdateData(
                table: "BillStatus",
                keyColumn: "BillStatusId",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2025, 1, 16, 17, 0, 54, 910, DateTimeKind.Local).AddTicks(3590));

            migrationBuilder.UpdateData(
                table: "BillStatus",
                keyColumn: "BillStatusId",
                keyValue: 2,
                column: "DateCreated",
                value: new DateTime(2025, 1, 16, 17, 0, 54, 910, DateTimeKind.Local).AddTicks(3594));

            migrationBuilder.UpdateData(
                table: "BillStatus",
                keyColumn: "BillStatusId",
                keyValue: 3,
                column: "DateCreated",
                value: new DateTime(2025, 1, 16, 17, 0, 54, 910, DateTimeKind.Local).AddTicks(3595));

            migrationBuilder.UpdateData(
                table: "BusinessSizes",
                keyColumn: "BusinessSizeId",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2025, 1, 16, 17, 0, 54, 910, DateTimeKind.Local).AddTicks(3284));

            migrationBuilder.UpdateData(
                table: "BusinessSizes",
                keyColumn: "BusinessSizeId",
                keyValue: 2,
                column: "DateCreated",
                value: new DateTime(2025, 1, 16, 17, 0, 54, 910, DateTimeKind.Local).AddTicks(3288));

            migrationBuilder.UpdateData(
                table: "BusinessSizes",
                keyColumn: "BusinessSizeId",
                keyValue: 3,
                column: "DateCreated",
                value: new DateTime(2025, 1, 16, 17, 0, 54, 910, DateTimeKind.Local).AddTicks(3289));

            migrationBuilder.UpdateData(
                table: "BusinessTypes",
                keyColumn: "BusinessTypeId",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2025, 1, 16, 17, 0, 54, 910, DateTimeKind.Local).AddTicks(2679));

            migrationBuilder.UpdateData(
                table: "BusinessTypes",
                keyColumn: "BusinessTypeId",
                keyValue: 2,
                column: "DateCreated",
                value: new DateTime(2025, 1, 16, 17, 0, 54, 910, DateTimeKind.Local).AddTicks(2682));

            migrationBuilder.UpdateData(
                table: "BusinessTypes",
                keyColumn: "BusinessTypeId",
                keyValue: 3,
                column: "DateCreated",
                value: new DateTime(2025, 1, 16, 17, 0, 54, 910, DateTimeKind.Local).AddTicks(2683));

            migrationBuilder.UpdateData(
                table: "BusinessTypes",
                keyColumn: "BusinessTypeId",
                keyValue: 4,
                column: "DateCreated",
                value: new DateTime(2025, 1, 16, 17, 0, 54, 910, DateTimeKind.Local).AddTicks(2685));

            migrationBuilder.UpdateData(
                table: "BusinessTypes",
                keyColumn: "BusinessTypeId",
                keyValue: 5,
                column: "DateCreated",
                value: new DateTime(2025, 1, 16, 17, 0, 54, 910, DateTimeKind.Local).AddTicks(2686));

            migrationBuilder.UpdateData(
                table: "BusinessTypes",
                keyColumn: "BusinessTypeId",
                keyValue: 6,
                column: "DateCreated",
                value: new DateTime(2025, 1, 16, 17, 0, 54, 910, DateTimeKind.Local).AddTicks(2687));

            migrationBuilder.UpdateData(
                table: "EmailTemplateCategory",
                keyColumn: "EmailTemplateCategoryId",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2025, 1, 16, 17, 0, 54, 910, DateTimeKind.Local).AddTicks(1080));

            migrationBuilder.UpdateData(
                table: "EmailTemplateCategory",
                keyColumn: "EmailTemplateCategoryId",
                keyValue: 2,
                column: "DateCreated",
                value: new DateTime(2025, 1, 16, 17, 0, 54, 910, DateTimeKind.Local).AddTicks(1083));

            migrationBuilder.UpdateData(
                table: "EmailTemplateCategory",
                keyColumn: "EmailTemplateCategoryId",
                keyValue: 3,
                column: "DateCreated",
                value: new DateTime(2025, 1, 16, 17, 0, 54, 910, DateTimeKind.Local).AddTicks(1084));

            migrationBuilder.UpdateData(
                table: "Frequencies",
                keyColumn: "FrequencyId",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2025, 1, 16, 17, 0, 54, 910, DateTimeKind.Local).AddTicks(3865));

            migrationBuilder.UpdateData(
                table: "Frequencies",
                keyColumn: "FrequencyId",
                keyValue: 2,
                column: "DateCreated",
                value: new DateTime(2025, 1, 16, 17, 0, 54, 910, DateTimeKind.Local).AddTicks(3868));

            migrationBuilder.UpdateData(
                table: "Frequencies",
                keyColumn: "FrequencyId",
                keyValue: 3,
                column: "DateCreated",
                value: new DateTime(2025, 1, 16, 17, 0, 54, 910, DateTimeKind.Local).AddTicks(3869));

            migrationBuilder.UpdateData(
                table: "Frequencies",
                keyColumn: "FrequencyId",
                keyValue: 4,
                column: "DateCreated",
                value: new DateTime(2025, 1, 16, 17, 0, 54, 910, DateTimeKind.Local).AddTicks(3871));

            migrationBuilder.UpdateData(
                table: "Frequencies",
                keyColumn: "FrequencyId",
                keyValue: 5,
                column: "DateCreated",
                value: new DateTime(2025, 1, 16, 17, 0, 54, 910, DateTimeKind.Local).AddTicks(3872));

            migrationBuilder.UpdateData(
                table: "Frequencies",
                keyColumn: "FrequencyId",
                keyValue: 6,
                column: "DateCreated",
                value: new DateTime(2025, 1, 16, 17, 0, 54, 910, DateTimeKind.Local).AddTicks(3873));

            migrationBuilder.UpdateData(
                table: "Genders",
                keyColumn: "GenderId",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2025, 1, 16, 17, 0, 54, 910, DateTimeKind.Local).AddTicks(1421));

            migrationBuilder.UpdateData(
                table: "Genders",
                keyColumn: "GenderId",
                keyValue: 2,
                column: "DateCreated",
                value: new DateTime(2025, 1, 16, 17, 0, 54, 910, DateTimeKind.Local).AddTicks(1424));

            migrationBuilder.UpdateData(
                table: "MaritalStatuses",
                keyColumn: "MaritalStatusId",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2025, 1, 16, 17, 0, 54, 910, DateTimeKind.Local).AddTicks(1715));

            migrationBuilder.UpdateData(
                table: "MaritalStatuses",
                keyColumn: "MaritalStatusId",
                keyValue: 2,
                column: "DateCreated",
                value: new DateTime(2025, 1, 16, 17, 0, 54, 910, DateTimeKind.Local).AddTicks(1719));

            migrationBuilder.UpdateData(
                table: "PayerTypes",
                keyColumn: "PayerTypeId",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2025, 1, 16, 17, 0, 54, 910, DateTimeKind.Local).AddTicks(2350));

            migrationBuilder.UpdateData(
                table: "PayerTypes",
                keyColumn: "PayerTypeId",
                keyValue: 2,
                column: "DateCreated",
                value: new DateTime(2025, 1, 16, 17, 0, 54, 910, DateTimeKind.Local).AddTicks(2356));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2025, 1, 16, 17, 0, 54, 910, DateTimeKind.Local).AddTicks(587));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 2,
                column: "DateCreated",
                value: new DateTime(2025, 1, 16, 17, 0, 54, 910, DateTimeKind.Local).AddTicks(605));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 3,
                column: "DateCreated",
                value: new DateTime(2025, 1, 16, 17, 0, 54, 910, DateTimeKind.Local).AddTicks(606));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 4,
                column: "DateCreated",
                value: new DateTime(2025, 1, 16, 17, 0, 54, 910, DateTimeKind.Local).AddTicks(608));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 5,
                column: "DateCreated",
                value: new DateTime(2025, 1, 16, 17, 0, 54, 910, DateTimeKind.Local).AddTicks(609));

            migrationBuilder.UpdateData(
                table: "SpaceIdentifiers",
                keyColumn: "SpaceIdentifierId",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2025, 1, 16, 17, 0, 54, 910, DateTimeKind.Local).AddTicks(2962));

            migrationBuilder.UpdateData(
                table: "SpaceIdentifiers",
                keyColumn: "SpaceIdentifierId",
                keyValue: 2,
                column: "DateCreated",
                value: new DateTime(2025, 1, 16, 17, 0, 54, 910, DateTimeKind.Local).AddTicks(2965));

            migrationBuilder.UpdateData(
                table: "SpaceIdentifiers",
                keyColumn: "SpaceIdentifierId",
                keyValue: 3,
                column: "DateCreated",
                value: new DateTime(2025, 1, 16, 17, 0, 54, 910, DateTimeKind.Local).AddTicks(2975));

            migrationBuilder.UpdateData(
                table: "SpaceIdentifiers",
                keyColumn: "SpaceIdentifierId",
                keyValue: 4,
                column: "DateCreated",
                value: new DateTime(2025, 1, 16, 17, 0, 54, 910, DateTimeKind.Local).AddTicks(2986));

            migrationBuilder.UpdateData(
                table: "SpaceIdentifiers",
                keyColumn: "SpaceIdentifierId",
                keyValue: 5,
                column: "DateCreated",
                value: new DateTime(2025, 1, 16, 17, 0, 54, 910, DateTimeKind.Local).AddTicks(2987));

            migrationBuilder.UpdateData(
                table: "SpaceIdentifiers",
                keyColumn: "SpaceIdentifierId",
                keyValue: 6,
                column: "DateCreated",
                value: new DateTime(2025, 1, 16, 17, 0, 54, 910, DateTimeKind.Local).AddTicks(2989));

            migrationBuilder.UpdateData(
                table: "SpaceIdentifiers",
                keyColumn: "SpaceIdentifierId",
                keyValue: 7,
                column: "DateCreated",
                value: new DateTime(2025, 1, 16, 17, 0, 54, 910, DateTimeKind.Local).AddTicks(2996));

            migrationBuilder.UpdateData(
                table: "Titles",
                keyColumn: "TitleId",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2025, 1, 16, 17, 0, 54, 910, DateTimeKind.Local).AddTicks(2002));

            migrationBuilder.UpdateData(
                table: "Titles",
                keyColumn: "TitleId",
                keyValue: 2,
                column: "DateCreated",
                value: new DateTime(2025, 1, 16, 17, 0, 54, 910, DateTimeKind.Local).AddTicks(2005));

            migrationBuilder.UpdateData(
                table: "Titles",
                keyColumn: "TitleId",
                keyValue: 3,
                column: "DateCreated",
                value: new DateTime(2025, 1, 16, 17, 0, 54, 910, DateTimeKind.Local).AddTicks(2007));

            migrationBuilder.UpdateData(
                table: "Titles",
                keyColumn: "TitleId",
                keyValue: 4,
                column: "DateCreated",
                value: new DateTime(2025, 1, 16, 17, 0, 54, 910, DateTimeKind.Local).AddTicks(2008));

            migrationBuilder.UpdateData(
                table: "Titles",
                keyColumn: "TitleId",
                keyValue: 5,
                column: "DateCreated",
                value: new DateTime(2025, 1, 16, 17, 0, 54, 910, DateTimeKind.Local).AddTicks(2009));

            migrationBuilder.UpdateData(
                table: "Titles",
                keyColumn: "TitleId",
                keyValue: 6,
                column: "DateCreated",
                value: new DateTime(2025, 1, 16, 17, 0, 54, 910, DateTimeKind.Local).AddTicks(2011));

            migrationBuilder.UpdateData(
                table: "Titles",
                keyColumn: "TitleId",
                keyValue: 7,
                column: "DateCreated",
                value: new DateTime(2025, 1, 16, 17, 0, 54, 910, DateTimeKind.Local).AddTicks(2012));

            migrationBuilder.CreateIndex(
                name: "IX_Billing_BillPreApprovalId",
                table: "Billing",
                column: "BillPreApprovalId");

            migrationBuilder.CreateIndex(
                name: "IX_Billing_UserId",
                table: "Billing",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Arrears_OrganisationId",
                table: "Arrears",
                column: "OrganisationId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_BillPreApproval_AgencyId",
            //    table: "BillPreApproval",
            //    column: "AgencyId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_BillPreApproval_BillStatusId",
            //    table: "BillPreApproval",
            //    column: "BillStatusId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_BillPreApproval_BillTypeId",
            //    table: "BillPreApproval",
            //    column: "BillTypeId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_BillPreApproval_BusinessSizeId",
            //    table: "BillPreApproval",
            //    column: "BusinessSizeId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_BillPreApproval_BusinessTypeId",
            //    table: "BillPreApproval",
            //    column: "BusinessTypeId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_BillPreApproval_CustomerId",
            //    table: "BillPreApproval",
            //    column: "CustomerId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_BillPreApproval_FrequencyId",
            //    table: "BillPreApproval",
            //    column: "FrequencyId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_BillPreApproval_OrganisationId",
            //    table: "BillPreApproval",
            //    column: "OrganisationId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_BillPreApproval_PropertyId",
            //    table: "BillPreApproval",
            //    column: "PropertyId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_BillPreApproval_RevenueId",
            //    table: "BillPreApproval",
            //    column: "RevenueId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_BillPreApproval_UserId",
            //    table: "BillPreApproval",
            //    column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Billing_BillPreApproval_BillPreApprovalId",
                table: "Billing",
                column: "BillPreApprovalId",
                principalTable: "BillPreApproval",
                principalColumn: "BillPreApprovalId");

            migrationBuilder.AddForeignKey(
                name: "FK_Billing_Users_UserId",
                table: "Billing",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_Billing_BillPreApproval_BillPreApprovalId",
            //    table: "Billing");

            //migrationBuilder.DropForeignKey(
            //    name: "FK_Billing_Users_UserId",
            //    table: "Billing");

            //migrationBuilder.DropTable(
            //    name: "Arrears");

            //migrationBuilder.DropTable(
            //    name: "BillPreApproval");

            //migrationBuilder.DropTable(
            //    name: "OrganisationCustomers");

            //migrationBuilder.DropIndex(
            //    name: "IX_Billing_BillPreApprovalId",
            //    table: "Billing");

            //migrationBuilder.DropIndex(
            //    name: "IX_Billing_UserId",
            //    table: "Billing");

            //migrationBuilder.DropColumn(
            //    name: "ActivityBillStatusId",
            //    table: "Billing");

            //migrationBuilder.DropColumn(
            //    name: "ApprovalBillStatusId",
            //    table: "Billing");

            //migrationBuilder.DropColumn(
            //    name: "BillPreApprovalId",
            //    table: "Billing");

            //migrationBuilder.DropColumn(
            //    name: "UserId",
            //    table: "Billing");

            migrationBuilder.UpdateData(
                table: "BillStatus",
                keyColumn: "BillStatusId",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2025, 1, 16, 16, 49, 56, 531, DateTimeKind.Local).AddTicks(1513));

            migrationBuilder.UpdateData(
                table: "BillStatus",
                keyColumn: "BillStatusId",
                keyValue: 2,
                column: "DateCreated",
                value: new DateTime(2025, 1, 16, 16, 49, 56, 531, DateTimeKind.Local).AddTicks(1516));

            migrationBuilder.UpdateData(
                table: "BillStatus",
                keyColumn: "BillStatusId",
                keyValue: 3,
                column: "DateCreated",
                value: new DateTime(2025, 1, 16, 16, 49, 56, 531, DateTimeKind.Local).AddTicks(1517));

            migrationBuilder.UpdateData(
                table: "BusinessSizes",
                keyColumn: "BusinessSizeId",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2025, 1, 16, 16, 49, 56, 531, DateTimeKind.Local).AddTicks(1186));

            migrationBuilder.UpdateData(
                table: "BusinessSizes",
                keyColumn: "BusinessSizeId",
                keyValue: 2,
                column: "DateCreated",
                value: new DateTime(2025, 1, 16, 16, 49, 56, 531, DateTimeKind.Local).AddTicks(1188));

            migrationBuilder.UpdateData(
                table: "BusinessSizes",
                keyColumn: "BusinessSizeId",
                keyValue: 3,
                column: "DateCreated",
                value: new DateTime(2025, 1, 16, 16, 49, 56, 531, DateTimeKind.Local).AddTicks(1190));

            migrationBuilder.UpdateData(
                table: "BusinessTypes",
                keyColumn: "BusinessTypeId",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2025, 1, 16, 16, 49, 56, 531, DateTimeKind.Local).AddTicks(592));

            migrationBuilder.UpdateData(
                table: "BusinessTypes",
                keyColumn: "BusinessTypeId",
                keyValue: 2,
                column: "DateCreated",
                value: new DateTime(2025, 1, 16, 16, 49, 56, 531, DateTimeKind.Local).AddTicks(595));

            migrationBuilder.UpdateData(
                table: "BusinessTypes",
                keyColumn: "BusinessTypeId",
                keyValue: 3,
                column: "DateCreated",
                value: new DateTime(2025, 1, 16, 16, 49, 56, 531, DateTimeKind.Local).AddTicks(596));

            migrationBuilder.UpdateData(
                table: "BusinessTypes",
                keyColumn: "BusinessTypeId",
                keyValue: 4,
                column: "DateCreated",
                value: new DateTime(2025, 1, 16, 16, 49, 56, 531, DateTimeKind.Local).AddTicks(598));

            migrationBuilder.UpdateData(
                table: "BusinessTypes",
                keyColumn: "BusinessTypeId",
                keyValue: 5,
                column: "DateCreated",
                value: new DateTime(2025, 1, 16, 16, 49, 56, 531, DateTimeKind.Local).AddTicks(599));

            migrationBuilder.UpdateData(
                table: "BusinessTypes",
                keyColumn: "BusinessTypeId",
                keyValue: 6,
                column: "DateCreated",
                value: new DateTime(2025, 1, 16, 16, 49, 56, 531, DateTimeKind.Local).AddTicks(600));

            migrationBuilder.UpdateData(
                table: "EmailTemplateCategory",
                keyColumn: "EmailTemplateCategoryId",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2025, 1, 16, 16, 49, 56, 530, DateTimeKind.Local).AddTicks(9098));

            migrationBuilder.UpdateData(
                table: "EmailTemplateCategory",
                keyColumn: "EmailTemplateCategoryId",
                keyValue: 2,
                column: "DateCreated",
                value: new DateTime(2025, 1, 16, 16, 49, 56, 530, DateTimeKind.Local).AddTicks(9101));

            migrationBuilder.UpdateData(
                table: "EmailTemplateCategory",
                keyColumn: "EmailTemplateCategoryId",
                keyValue: 3,
                column: "DateCreated",
                value: new DateTime(2025, 1, 16, 16, 49, 56, 530, DateTimeKind.Local).AddTicks(9102));

            migrationBuilder.UpdateData(
                table: "Frequencies",
                keyColumn: "FrequencyId",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2025, 1, 16, 16, 49, 56, 531, DateTimeKind.Local).AddTicks(1793));

            migrationBuilder.UpdateData(
                table: "Frequencies",
                keyColumn: "FrequencyId",
                keyValue: 2,
                column: "DateCreated",
                value: new DateTime(2025, 1, 16, 16, 49, 56, 531, DateTimeKind.Local).AddTicks(1796));

            migrationBuilder.UpdateData(
                table: "Frequencies",
                keyColumn: "FrequencyId",
                keyValue: 3,
                column: "DateCreated",
                value: new DateTime(2025, 1, 16, 16, 49, 56, 531, DateTimeKind.Local).AddTicks(1797));

            migrationBuilder.UpdateData(
                table: "Frequencies",
                keyColumn: "FrequencyId",
                keyValue: 4,
                column: "DateCreated",
                value: new DateTime(2025, 1, 16, 16, 49, 56, 531, DateTimeKind.Local).AddTicks(1798));

            migrationBuilder.UpdateData(
                table: "Frequencies",
                keyColumn: "FrequencyId",
                keyValue: 5,
                column: "DateCreated",
                value: new DateTime(2025, 1, 16, 16, 49, 56, 531, DateTimeKind.Local).AddTicks(1800));

            migrationBuilder.UpdateData(
                table: "Frequencies",
                keyColumn: "FrequencyId",
                keyValue: 6,
                column: "DateCreated",
                value: new DateTime(2025, 1, 16, 16, 49, 56, 531, DateTimeKind.Local).AddTicks(1801));

            migrationBuilder.UpdateData(
                table: "Genders",
                keyColumn: "GenderId",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2025, 1, 16, 16, 49, 56, 530, DateTimeKind.Local).AddTicks(9390));

            migrationBuilder.UpdateData(
                table: "Genders",
                keyColumn: "GenderId",
                keyValue: 2,
                column: "DateCreated",
                value: new DateTime(2025, 1, 16, 16, 49, 56, 530, DateTimeKind.Local).AddTicks(9394));

            migrationBuilder.UpdateData(
                table: "MaritalStatuses",
                keyColumn: "MaritalStatusId",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2025, 1, 16, 16, 49, 56, 530, DateTimeKind.Local).AddTicks(9679));

            migrationBuilder.UpdateData(
                table: "MaritalStatuses",
                keyColumn: "MaritalStatusId",
                keyValue: 2,
                column: "DateCreated",
                value: new DateTime(2025, 1, 16, 16, 49, 56, 530, DateTimeKind.Local).AddTicks(9682));

            migrationBuilder.UpdateData(
                table: "PayerTypes",
                keyColumn: "PayerTypeId",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2025, 1, 16, 16, 49, 56, 531, DateTimeKind.Local).AddTicks(306));

            migrationBuilder.UpdateData(
                table: "PayerTypes",
                keyColumn: "PayerTypeId",
                keyValue: 2,
                column: "DateCreated",
                value: new DateTime(2025, 1, 16, 16, 49, 56, 531, DateTimeKind.Local).AddTicks(309));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2025, 1, 16, 16, 49, 56, 530, DateTimeKind.Local).AddTicks(8637));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 2,
                column: "DateCreated",
                value: new DateTime(2025, 1, 16, 16, 49, 56, 530, DateTimeKind.Local).AddTicks(8658));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 3,
                column: "DateCreated",
                value: new DateTime(2025, 1, 16, 16, 49, 56, 530, DateTimeKind.Local).AddTicks(8660));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 4,
                column: "DateCreated",
                value: new DateTime(2025, 1, 16, 16, 49, 56, 530, DateTimeKind.Local).AddTicks(8661));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "RoleId",
                keyValue: 5,
                column: "DateCreated",
                value: new DateTime(2025, 1, 16, 16, 49, 56, 530, DateTimeKind.Local).AddTicks(8663));

            migrationBuilder.UpdateData(
                table: "SpaceIdentifiers",
                keyColumn: "SpaceIdentifierId",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2025, 1, 16, 16, 49, 56, 531, DateTimeKind.Local).AddTicks(888));

            migrationBuilder.UpdateData(
                table: "SpaceIdentifiers",
                keyColumn: "SpaceIdentifierId",
                keyValue: 2,
                column: "DateCreated",
                value: new DateTime(2025, 1, 16, 16, 49, 56, 531, DateTimeKind.Local).AddTicks(890));

            migrationBuilder.UpdateData(
                table: "SpaceIdentifiers",
                keyColumn: "SpaceIdentifierId",
                keyValue: 3,
                column: "DateCreated",
                value: new DateTime(2025, 1, 16, 16, 49, 56, 531, DateTimeKind.Local).AddTicks(892));

            migrationBuilder.UpdateData(
                table: "SpaceIdentifiers",
                keyColumn: "SpaceIdentifierId",
                keyValue: 4,
                column: "DateCreated",
                value: new DateTime(2025, 1, 16, 16, 49, 56, 531, DateTimeKind.Local).AddTicks(893));

            migrationBuilder.UpdateData(
                table: "SpaceIdentifiers",
                keyColumn: "SpaceIdentifierId",
                keyValue: 5,
                column: "DateCreated",
                value: new DateTime(2025, 1, 16, 16, 49, 56, 531, DateTimeKind.Local).AddTicks(894));

            migrationBuilder.UpdateData(
                table: "SpaceIdentifiers",
                keyColumn: "SpaceIdentifierId",
                keyValue: 6,
                column: "DateCreated",
                value: new DateTime(2025, 1, 16, 16, 49, 56, 531, DateTimeKind.Local).AddTicks(896));

            migrationBuilder.UpdateData(
                table: "SpaceIdentifiers",
                keyColumn: "SpaceIdentifierId",
                keyValue: 7,
                column: "DateCreated",
                value: new DateTime(2025, 1, 16, 16, 49, 56, 531, DateTimeKind.Local).AddTicks(908));

            migrationBuilder.UpdateData(
                table: "Titles",
                keyColumn: "TitleId",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2025, 1, 16, 16, 49, 56, 530, DateTimeKind.Local).AddTicks(9977));

            migrationBuilder.UpdateData(
                table: "Titles",
                keyColumn: "TitleId",
                keyValue: 2,
                column: "DateCreated",
                value: new DateTime(2025, 1, 16, 16, 49, 56, 530, DateTimeKind.Local).AddTicks(9982));

            migrationBuilder.UpdateData(
                table: "Titles",
                keyColumn: "TitleId",
                keyValue: 3,
                column: "DateCreated",
                value: new DateTime(2025, 1, 16, 16, 49, 56, 530, DateTimeKind.Local).AddTicks(9984));

            migrationBuilder.UpdateData(
                table: "Titles",
                keyColumn: "TitleId",
                keyValue: 4,
                column: "DateCreated",
                value: new DateTime(2025, 1, 16, 16, 49, 56, 530, DateTimeKind.Local).AddTicks(9985));

            migrationBuilder.UpdateData(
                table: "Titles",
                keyColumn: "TitleId",
                keyValue: 5,
                column: "DateCreated",
                value: new DateTime(2025, 1, 16, 16, 49, 56, 530, DateTimeKind.Local).AddTicks(9986));

            migrationBuilder.UpdateData(
                table: "Titles",
                keyColumn: "TitleId",
                keyValue: 6,
                column: "DateCreated",
                value: new DateTime(2025, 1, 16, 16, 49, 56, 530, DateTimeKind.Local).AddTicks(9988));

            migrationBuilder.UpdateData(
                table: "Titles",
                keyColumn: "TitleId",
                keyValue: 7,
                column: "DateCreated",
                value: new DateTime(2025, 1, 16, 16, 49, 56, 530, DateTimeKind.Local).AddTicks(9989));
        }
    }
}
