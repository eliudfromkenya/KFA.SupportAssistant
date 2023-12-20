using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KFA.SupportAssistant.Web.Migrations
{
    /// <inheritdoc />
    public partial class UpdatingLedgerAccountsRelationships : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_tbl_ledger_accounts_ledger_account_code",
                table: "tbl_ledger_accounts");

            migrationBuilder.DropColumn(
                name: "role_number",
                table: "tbl_user_roles");

            migrationBuilder.DropColumn(
                name: "supplier_ledger_account_id",
                table: "tbl_suppliers");

            migrationBuilder.DropColumn(
                name: "account_number",
                table: "tbl_let_properties_accounts");

            migrationBuilder.DropColumn(
                name: "account_number",
                table: "tbl_leased_properties_accounts");

            migrationBuilder.RenameColumn(
                name: "ledger_account_id",
                table: "tbl_let_properties_accounts",
                newName: "ledger_account_code");

            migrationBuilder.RenameIndex(
                name: "IX_tbl_let_properties_accounts_ledger_account_id",
                table: "tbl_let_properties_accounts",
                newName: "IX_tbl_let_properties_accounts_ledger_account_code");

            migrationBuilder.RenameColumn(
                name: "ledger_account_id",
                table: "tbl_leased_properties_accounts",
                newName: "ledger_account_code");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_ledger_accounts_ledger_account_code",
                table: "tbl_ledger_accounts",
                column: "ledger_account_code");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_leased_properties_accounts_ledger_account_code",
                table: "tbl_leased_properties_accounts",
                column: "ledger_account_code");

            migrationBuilder.AddForeignKey(
                name: "FK_tbl_leased_properties_accounts_tbl_ledger_accounts_ledger_ac~",
                table: "tbl_leased_properties_accounts",
                column: "ledger_account_code",
                principalTable: "tbl_ledger_accounts",
                principalColumn: "ledger_account_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tbl_leased_properties_accounts_tbl_ledger_accounts_ledger_ac~",
                table: "tbl_leased_properties_accounts");

            migrationBuilder.DropIndex(
                name: "IX_tbl_ledger_accounts_ledger_account_code",
                table: "tbl_ledger_accounts");

            migrationBuilder.DropIndex(
                name: "IX_tbl_leased_properties_accounts_ledger_account_code",
                table: "tbl_leased_properties_accounts");

            migrationBuilder.RenameColumn(
                name: "ledger_account_code",
                table: "tbl_let_properties_accounts",
                newName: "ledger_account_id");

            migrationBuilder.RenameIndex(
                name: "IX_tbl_let_properties_accounts_ledger_account_code",
                table: "tbl_let_properties_accounts",
                newName: "IX_tbl_let_properties_accounts_ledger_account_id");

            migrationBuilder.RenameColumn(
                name: "ledger_account_code",
                table: "tbl_leased_properties_accounts",
                newName: "ledger_account_id");

            migrationBuilder.AddColumn<short>(
                name: "role_number",
                table: "tbl_user_roles",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<string>(
                name: "supplier_ledger_account_id",
                table: "tbl_suppliers",
                type: "varchar(10)",
                maxLength: 10,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "account_number",
                table: "tbl_let_properties_accounts",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "account_number",
                table: "tbl_leased_properties_accounts",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_tbl_ledger_accounts_ledger_account_code",
                table: "tbl_ledger_accounts",
                column: "ledger_account_code",
                unique: true);
        }
    }
}
