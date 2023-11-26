using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KFA.SupportAssistant.Web.Migrations
{
  /// <inheritdoc />
  public partial class InitialSetUp : Migration
  {
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.CreateTable(
          name: "Contributors",
          columns: table => new
          {
            date_added = table.Column<long>(type: "INTEGER", nullable: true),
            date_updated = table.Column<long>(type: "INTEGER", nullable: true),
            modification_status = table.Column<byte>(type: "INTEGER", nullable: true),
            id = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
            Name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
            Status = table.Column<int>(type: "INTEGER", nullable: false)
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_Contributors", x => x.id);
          });

      migrationBuilder.CreateTable(
          name: "tbl_command_details",
          columns: table => new
          {
            date_added = table.Column<long>(type: "INTEGER", nullable: true),
            date_updated = table.Column<long>(type: "INTEGER", nullable: true),
            modification_status = table.Column<byte>(type: "INTEGER", nullable: true),
            command_id = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
            action = table.Column<string>(type: "TEXT", maxLength: 25, nullable: false),
            active_state = table.Column<string>(type: "TEXT", maxLength: 10, nullable: false),
            category = table.Column<string>(type: "TEXT", maxLength: 25, nullable: false),
            command_name = table.Column<string>(type: "TEXT", maxLength: 25, nullable: false),
            command_text = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
            image_id = table.Column<long>(type: "INTEGER", nullable: false),
            image_path = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
            is_enabled = table.Column<bool>(type: "INTEGER", nullable: false),
            is_published = table.Column<bool>(type: "INTEGER", nullable: false),
            narration = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
            shortcut_key = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false)
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_tbl_command_details", x => x.command_id);
          });

      migrationBuilder.CreateTable(
          name: "tbl_communication_messages",
          columns: table => new
          {
            date_added = table.Column<long>(type: "INTEGER", nullable: true),
            date_updated = table.Column<long>(type: "INTEGER", nullable: true),
            modification_status = table.Column<byte>(type: "INTEGER", nullable: true),
            message_id = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
            attachments = table.Column<byte[]>(type: "BLOB", nullable: true),
            details = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true),
            from = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true),
            message = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
            message_type = table.Column<byte>(type: "INTEGER", maxLength: 255, nullable: false),
            narration = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
            status = table.Column<byte>(type: "INTEGER", maxLength: 255, nullable: true),
            title = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true),
            to = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true)
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_tbl_communication_messages", x => x.message_id);
          });

      migrationBuilder.CreateTable(
          name: "tbl_cost_centres",
          columns: table => new
          {
            date_added = table.Column<long>(type: "INTEGER", nullable: true),
            date_updated = table.Column<long>(type: "INTEGER", nullable: true),
            modification_status = table.Column<byte>(type: "INTEGER", nullable: true),
            cost_centre_code = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
            description = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
            narration = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
            region = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true),
            supplier_code_prefix = table.Column<string>(type: "TEXT", maxLength: 10, nullable: true)
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_tbl_cost_centres", x => x.cost_centre_code);
          });

      migrationBuilder.CreateTable(
          name: "tbl_device_guids",
          columns: table => new
          {
            date_added = table.Column<long>(type: "INTEGER", nullable: true),
            date_updated = table.Column<long>(type: "INTEGER", nullable: true),
            modification_status = table.Column<byte>(type: "INTEGER", nullable: true),
            guid = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false)
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_tbl_device_guids", x => x.guid);
          });

      migrationBuilder.CreateTable(
          name: "tbl_item_groups",
          columns: table => new
          {
            date_added = table.Column<long>(type: "INTEGER", nullable: true),
            date_updated = table.Column<long>(type: "INTEGER", nullable: true),
            modification_status = table.Column<byte>(type: "INTEGER", nullable: true),
            group_id = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
            name = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
            parent_group_id = table.Column<string>(type: "TEXT", nullable: true),
            ParentGroup_Caption = table.Column<string>(type: "TEXT", nullable: true)
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_tbl_item_groups", x => x.group_id);
            table.ForeignKey(
                      name: "FK_tbl_item_groups_tbl_item_groups_parent_group_id",
                      column: x => x.parent_group_id,
                      principalTable: "tbl_item_groups",
                      principalColumn: "group_id");
          });

      migrationBuilder.CreateTable(
          name: "tbl_password_safes",
          columns: table => new
          {
            date_added = table.Column<long>(type: "INTEGER", nullable: true),
            date_updated = table.Column<long>(type: "INTEGER", nullable: true),
            modification_status = table.Column<byte>(type: "INTEGER", nullable: true),
            password_id = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
            details = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
            name = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
            password = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
            reminder = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true),
            users_visible_to = table.Column<string>(type: "TEXT", nullable: true)
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_tbl_password_safes", x => x.password_id);
          });

      migrationBuilder.CreateTable(
          name: "tbl_system_rights",
          columns: table => new
          {
            date_added = table.Column<long>(type: "INTEGER", nullable: true),
            date_updated = table.Column<long>(type: "INTEGER", nullable: true),
            modification_status = table.Column<byte>(type: "INTEGER", nullable: true),
            right_id = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
            is_compulsory = table.Column<bool>(type: "INTEGER", nullable: false),
            narration = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
            right_name = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false)
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_tbl_system_rights", x => x.right_id);
          });

      migrationBuilder.CreateTable(
          name: "tbl_tims_machines",
          columns: table => new
          {
            date_added = table.Column<long>(type: "INTEGER", nullable: true),
            date_updated = table.Column<long>(type: "INTEGER", nullable: true),
            modification_status = table.Column<byte>(type: "INTEGER", nullable: true),
            machine_id = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
            class_type = table.Column<string>(type: "TEXT", maxLength: 5, nullable: true),
            current_status = table.Column<byte>(type: "INTEGER", nullable: false),
            domain_name = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true),
            external_ip_address = table.Column<string>(type: "TEXT", maxLength: 20, nullable: true),
            external_port_number = table.Column<string>(type: "TEXT", maxLength: 8, nullable: true),
            internal_ip_address = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
            internal_port_number = table.Column<string>(type: "TEXT", maxLength: 8, nullable: true),
            narration = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
            ready_for_use = table.Column<bool>(type: "INTEGER", nullable: false),
            serial_number = table.Column<string>(type: "TEXT", maxLength: 25, nullable: true),
            tims_name = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true)
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_tbl_tims_machines", x => x.machine_id);
          });

      migrationBuilder.CreateTable(
          name: "tbl_user_roles",
          columns: table => new
          {
            date_added = table.Column<long>(type: "INTEGER", nullable: true),
            date_updated = table.Column<long>(type: "INTEGER", nullable: true),
            modification_status = table.Column<byte>(type: "INTEGER", nullable: true),
            role_id = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
            expiration_date = table.Column<DateTime>(type: "TEXT", nullable: false),
            maturity_date = table.Column<DateTime>(type: "TEXT", nullable: false),
            narration = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
            role_name = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
            role_number = table.Column<short>(type: "INTEGER", nullable: false),
            UserRole_Caption = table.Column<string>(type: "TEXT", nullable: true)
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_tbl_user_roles", x => x.role_id);
          });

      migrationBuilder.CreateTable(
          name: "tbl_verification_types",
          columns: table => new
          {
            date_added = table.Column<long>(type: "INTEGER", nullable: true),
            date_updated = table.Column<long>(type: "INTEGER", nullable: true),
            modification_status = table.Column<byte>(type: "INTEGER", nullable: true),
            verification_type_id = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
            category = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true),
            narration = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
            verification_type_name = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false)
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_tbl_verification_types", x => x.verification_type_id);
          });

      migrationBuilder.CreateTable(
          name: "tbl_computer_anydesks",
          columns: table => new
          {
            date_added = table.Column<long>(type: "INTEGER", nullable: true),
            date_updated = table.Column<long>(type: "INTEGER", nullable: true),
            modification_status = table.Column<byte>(type: "INTEGER", nullable: true),
            anydesk_id = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
            anydesk_number = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
            password = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
            name_of_user = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
            cost_centre_code = table.Column<string>(type: "TEXT", nullable: false),
            CostCentre_Caption = table.Column<string>(type: "TEXT", nullable: true),
            device_name = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
            narration = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
            type = table.Column<byte>(type: "INTEGER", maxLength: 255, nullable: true)
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_tbl_computer_anydesks", x => x.anydesk_id);
            table.ForeignKey(
                      name: "FK_tbl_computer_anydesks_tbl_cost_centres_cost_centre_code",
                      column: x => x.cost_centre_code,
                      principalTable: "tbl_cost_centres",
                      principalColumn: "cost_centre_code",
                      onDelete: ReferentialAction.Cascade);
          });

      migrationBuilder.CreateTable(
          name: "tbl_data_devices",
          columns: table => new
          {
            date_added = table.Column<long>(type: "INTEGER", nullable: true),
            date_updated = table.Column<long>(type: "INTEGER", nullable: true),
            modification_status = table.Column<byte>(type: "INTEGER", nullable: true),
            device_id = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
            device_caption = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true),
            device_code = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
            device_name = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
            device_number = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true),
            device_right = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true),
            station_id = table.Column<string>(type: "TEXT", nullable: false),
            Station_Caption = table.Column<string>(type: "TEXT", nullable: true),
            type_of_device = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true)
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_tbl_data_devices", x => x.device_id);
            table.ForeignKey(
                      name: "FK_tbl_data_devices_tbl_cost_centres_station_id",
                      column: x => x.station_id,
                      principalTable: "tbl_cost_centres",
                      principalColumn: "cost_centre_code",
                      onDelete: ReferentialAction.Cascade);
          });

      migrationBuilder.CreateTable(
          name: "tbl_leased_properties_accounts",
          columns: table => new
          {
            date_added = table.Column<long>(type: "INTEGER", nullable: true),
            date_updated = table.Column<long>(type: "INTEGER", nullable: true),
            modification_status = table.Column<byte>(type: "INTEGER", nullable: true),
            leased_property_account_id = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
            account_number = table.Column<string>(type: "TEXT", nullable: true),
            commencement_rent = table.Column<decimal>(type: "TEXT", nullable: false),
            cost_centre_code = table.Column<string>(type: "TEXT", nullable: true),
            CostCentre_Caption = table.Column<string>(type: "TEXT", nullable: true),
            current_rent = table.Column<decimal>(type: "TEXT", nullable: false),
            landlord_address = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true),
            last_review_date = table.Column<DateTime>(type: "TEXT", nullable: false),
            leased_on = table.Column<DateTime>(type: "TEXT", nullable: false),
            ledger_account_id = table.Column<string>(type: "TEXT", maxLength: 25, nullable: true),
            narration = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
            LedgerAccount_Caption = table.Column<string>(type: "TEXT", nullable: true)
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_tbl_leased_properties_accounts", x => x.leased_property_account_id);
            table.ForeignKey(
                      name: "FK_tbl_leased_properties_accounts_tbl_cost_centres_cost_centre_code",
                      column: x => x.cost_centre_code,
                      principalTable: "tbl_cost_centres",
                      principalColumn: "cost_centre_code");
          });

      migrationBuilder.CreateTable(
          name: "tbl_qr_codes_requests",
          columns: table => new
          {
            date_added = table.Column<long>(type: "INTEGER", nullable: true),
            date_updated = table.Column<long>(type: "INTEGER", nullable: true),
            modification_status = table.Column<byte>(type: "INTEGER", nullable: true),
            qr_code_request_id = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
            cost_centre_code = table.Column<string>(type: "TEXT", nullable: true),
            CostCentre_Caption = table.Column<string>(type: "TEXT", nullable: true),
            is_duplicate = table.Column<bool>(type: "INTEGER", nullable: false),
            narration = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
            request_data = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
            response_data = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true),
            response_status = table.Column<byte>(type: "INTEGER", maxLength: 255, nullable: true),
            time = table.Column<DateTime>(type: "TEXT", nullable: false),
            tims_machine_used = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true),
            vat_class = table.Column<string>(type: "TEXT", maxLength: 5, nullable: true)
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_tbl_qr_codes_requests", x => x.qr_code_request_id);
            table.ForeignKey(
                      name: "FK_tbl_qr_codes_requests_tbl_cost_centres_cost_centre_code",
                      column: x => x.cost_centre_code,
                      principalTable: "tbl_cost_centres",
                      principalColumn: "cost_centre_code");
          });

      migrationBuilder.CreateTable(
          name: "tbl_stock_items",
          columns: table => new
          {
            date_added = table.Column<long>(type: "INTEGER", nullable: true),
            date_updated = table.Column<long>(type: "INTEGER", nullable: true),
            modification_status = table.Column<byte>(type: "INTEGER", nullable: true),
            item_code = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
            barcode = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true),
            group_id = table.Column<string>(type: "TEXT", nullable: true),
            Group_Caption = table.Column<string>(type: "TEXT", nullable: true),
            item_name = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
            narration = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true)
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_tbl_stock_items", x => x.item_code);
            table.ForeignKey(
                      name: "FK_tbl_stock_items_tbl_item_groups_group_id",
                      column: x => x.group_id,
                      principalTable: "tbl_item_groups",
                      principalColumn: "group_id");
          });

      migrationBuilder.CreateTable(
          name: "tbl_system_users",
          columns: table => new
          {
            date_added = table.Column<long>(type: "INTEGER", nullable: true),
            date_updated = table.Column<long>(type: "INTEGER", nullable: true),
            modification_status = table.Column<byte>(type: "INTEGER", nullable: true),
            user_id = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
            contact = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
            email_address = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
            expiration_date = table.Column<DateTime>(type: "TEXT", nullable: false),
            is_active = table.Column<bool>(type: "INTEGER", nullable: false),
            maturity_date = table.Column<DateTime>(type: "TEXT", nullable: false),
            name_of_the_user = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
            narration = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
            password_hash = table.Column<byte[]>(type: "BLOB", nullable: false),
            password_salt = table.Column<byte[]>(type: "BLOB", nullable: false),
            role_id = table.Column<string>(type: "TEXT", nullable: false),
            Role_Caption = table.Column<string>(type: "TEXT", nullable: true),
            username = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false)
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_tbl_system_users", x => x.user_id);
            table.ForeignKey(
                      name: "FK_tbl_system_users_tbl_user_roles_role_id",
                      column: x => x.role_id,
                      principalTable: "tbl_user_roles",
                      principalColumn: "role_id",
                      onDelete: ReferentialAction.Cascade);
          });

      migrationBuilder.CreateTable(
          name: "tbl_ledger_accounts",
          columns: table => new
          {
            date_added = table.Column<long>(type: "INTEGER", nullable: true),
            date_updated = table.Column<long>(type: "INTEGER", nullable: true),
            modification_status = table.Column<byte>(type: "INTEGER", nullable: true),
            ledger_account_id = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
            cost_centre_code = table.Column<string>(type: "TEXT", nullable: true),
            CostCentre_Caption = table.Column<string>(type: "TEXT", nullable: true),
            description = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
            group_name = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true),
            increase_with_debit = table.Column<bool>(type: "INTEGER", nullable: false),
            ledger_account_code = table.Column<string>(type: "TEXT", nullable: false),
            LedgerAccount_Caption = table.Column<string>(type: "TEXT", nullable: true),
            main_group = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true),
            narration = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
            Supplier_Caption = table.Column<string>(type: "TEXT", nullable: true)
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_tbl_ledger_accounts", x => x.ledger_account_id);
            table.ForeignKey(
                      name: "FK_tbl_ledger_accounts_tbl_cost_centres_cost_centre_code",
                      column: x => x.cost_centre_code,
                      principalTable: "tbl_cost_centres",
                      principalColumn: "cost_centre_code");
            table.ForeignKey(
                      name: "FK_tbl_ledger_accounts_tbl_leased_properties_accounts_ledger_account_code",
                      column: x => x.ledger_account_code,
                      principalTable: "tbl_leased_properties_accounts",
                      principalColumn: "leased_property_account_id",
                      onDelete: ReferentialAction.Cascade);
          });

      migrationBuilder.CreateTable(
          name: "tbl_qr_request_items",
          columns: table => new
          {
            date_added = table.Column<long>(type: "INTEGER", nullable: true),
            date_updated = table.Column<long>(type: "INTEGER", nullable: true),
            modification_status = table.Column<byte>(type: "INTEGER", nullable: true),
            sale_id = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
            cash_sale_number = table.Column<string>(type: "TEXT", maxLength: 30, nullable: true),
            cost_centre_code = table.Column<string>(type: "TEXT", nullable: true),
            CostCentre_Caption = table.Column<string>(type: "TEXT", nullable: true),
            hs_code = table.Column<string>(type: "TEXT", maxLength: 25, nullable: true),
            hs_name = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true),
            item_code = table.Column<string>(type: "TEXT", maxLength: 16, nullable: true),
            item_name = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true),
            narration = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
            percentage_discount = table.Column<decimal>(type: "TEXT", nullable: false),
            quantity = table.Column<decimal>(type: "TEXT", nullable: false),
            request_id = table.Column<string>(type: "TEXT", nullable: true),
            time = table.Column<DateTime>(type: "TEXT", nullable: false),
            total_amount = table.Column<decimal>(type: "TEXT", nullable: false),
            unit_of_measure = table.Column<string>(type: "TEXT", maxLength: 25, nullable: true),
            unit_price = table.Column<decimal>(type: "TEXT", nullable: false),
            vat_amount = table.Column<decimal>(type: "TEXT", nullable: false),
            vat_class = table.Column<string>(type: "TEXT", maxLength: 4, nullable: true)
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_tbl_qr_request_items", x => x.sale_id);
            table.ForeignKey(
                      name: "FK_tbl_qr_request_items_tbl_cost_centres_cost_centre_code",
                      column: x => x.cost_centre_code,
                      principalTable: "tbl_cost_centres",
                      principalColumn: "cost_centre_code");
            table.ForeignKey(
                      name: "FK_tbl_qr_request_items_tbl_qr_codes_requests_request_id",
                      column: x => x.request_id,
                      principalTable: "tbl_qr_codes_requests",
                      principalColumn: "qr_code_request_id");
          });

      migrationBuilder.CreateTable(
          name: "tbl_user_logins",
          columns: table => new
          {
            date_added = table.Column<long>(type: "INTEGER", nullable: true),
            date_updated = table.Column<long>(type: "INTEGER", nullable: true),
            modification_status = table.Column<byte>(type: "INTEGER", nullable: true),
            login_id = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
            device_id = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
            from_date = table.Column<DateTime>(type: "TEXT", nullable: false),
            narration = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
            upto_date = table.Column<DateTime>(type: "TEXT", nullable: false),
            user_id = table.Column<string>(type: "TEXT", nullable: false),
            User_Caption = table.Column<string>(type: "TEXT", nullable: true)
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_tbl_user_logins", x => x.login_id);
            table.ForeignKey(
                      name: "FK_tbl_user_logins_tbl_system_users_user_id",
                      column: x => x.user_id,
                      principalTable: "tbl_system_users",
                      principalColumn: "user_id",
                      onDelete: ReferentialAction.Cascade);
          });

      migrationBuilder.CreateTable(
          name: "tbl_user_rights",
          columns: table => new
          {
            date_added = table.Column<long>(type: "INTEGER", nullable: true),
            date_updated = table.Column<long>(type: "INTEGER", nullable: true),
            modification_status = table.Column<byte>(type: "INTEGER", nullable: true),
            user_right_id = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
            description = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
            narration = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
            object_name = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
            command_id = table.Column<string>(type: "TEXT", nullable: true),
            Command_Caption = table.Column<string>(type: "TEXT", nullable: true),
            right_id = table.Column<string>(type: "TEXT", nullable: true),
            Right_Caption = table.Column<string>(type: "TEXT", nullable: true),
            role_id = table.Column<string>(type: "TEXT", nullable: false),
            Role_Caption = table.Column<string>(type: "TEXT", nullable: true),
            user_id = table.Column<string>(type: "TEXT", nullable: false),
            User_Caption = table.Column<string>(type: "TEXT", nullable: true)
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_tbl_user_rights", x => x.user_right_id);
            table.ForeignKey(
                      name: "FK_tbl_user_rights_tbl_command_details_command_id",
                      column: x => x.command_id,
                      principalTable: "tbl_command_details",
                      principalColumn: "command_id");
            table.ForeignKey(
                      name: "FK_tbl_user_rights_tbl_system_rights_right_id",
                      column: x => x.right_id,
                      principalTable: "tbl_system_rights",
                      principalColumn: "right_id");
            table.ForeignKey(
                      name: "FK_tbl_user_rights_tbl_system_users_user_id",
                      column: x => x.user_id,
                      principalTable: "tbl_system_users",
                      principalColumn: "user_id",
                      onDelete: ReferentialAction.Cascade);
            table.ForeignKey(
                      name: "FK_tbl_user_rights_tbl_user_roles_role_id",
                      column: x => x.role_id,
                      principalTable: "tbl_user_roles",
                      principalColumn: "role_id",
                      onDelete: ReferentialAction.Cascade);
          });

      migrationBuilder.CreateTable(
          name: "tbl_verification_rights",
          columns: table => new
          {
            date_added = table.Column<long>(type: "INTEGER", nullable: true),
            date_updated = table.Column<long>(type: "INTEGER", nullable: true),
            modification_status = table.Column<byte>(type: "INTEGER", nullable: true),
            verification_right_id = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
            device_id = table.Column<string>(type: "TEXT", nullable: true),
            Device_Caption = table.Column<string>(type: "TEXT", nullable: true),
            user_id = table.Column<string>(type: "TEXT", nullable: true),
            User_Caption = table.Column<string>(type: "TEXT", nullable: true),
            user_role_id = table.Column<string>(type: "TEXT", nullable: false),
            UserRole_Caption = table.Column<string>(type: "TEXT", nullable: true),
            verification_type_id = table.Column<long>(type: "INTEGER", nullable: false)
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_tbl_verification_rights", x => x.verification_right_id);
            table.ForeignKey(
                      name: "FK_tbl_verification_rights_tbl_data_devices_device_id",
                      column: x => x.device_id,
                      principalTable: "tbl_data_devices",
                      principalColumn: "device_id");
            table.ForeignKey(
                      name: "FK_tbl_verification_rights_tbl_system_users_user_id",
                      column: x => x.user_id,
                      principalTable: "tbl_system_users",
                      principalColumn: "user_id");
            table.ForeignKey(
                      name: "FK_tbl_verification_rights_tbl_user_roles_user_role_id",
                      column: x => x.user_role_id,
                      principalTable: "tbl_user_roles",
                      principalColumn: "role_id",
                      onDelete: ReferentialAction.Cascade);
          });

      migrationBuilder.CreateTable(
          name: "tbl_let_properties_accounts",
          columns: table => new
          {
            date_added = table.Column<long>(type: "INTEGER", nullable: true),
            date_updated = table.Column<long>(type: "INTEGER", nullable: true),
            modification_status = table.Column<byte>(type: "INTEGER", nullable: true),
            let_property_account_id = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
            account_number = table.Column<string>(type: "TEXT", nullable: true),
            commencement_rent = table.Column<decimal>(type: "TEXT", nullable: false),
            cost_centre_code = table.Column<string>(type: "TEXT", nullable: true),
            CostCentre_Caption = table.Column<string>(type: "TEXT", nullable: true),
            current_rent = table.Column<decimal>(type: "TEXT", nullable: false),
            last_review_date = table.Column<DateTime>(type: "TEXT", nullable: false),
            ledger_account_id = table.Column<string>(type: "TEXT", maxLength: 25, nullable: true),
            let_on = table.Column<DateTime>(type: "TEXT", nullable: false),
            narration = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
            tenant_address = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true),
            LedgerAccount_Caption = table.Column<string>(type: "TEXT", nullable: true)
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_tbl_let_properties_accounts", x => x.let_property_account_id);
            table.ForeignKey(
                      name: "FK_tbl_let_properties_accounts_tbl_cost_centres_cost_centre_code",
                      column: x => x.cost_centre_code,
                      principalTable: "tbl_cost_centres",
                      principalColumn: "cost_centre_code");
            table.ForeignKey(
                      name: "FK_tbl_let_properties_accounts_tbl_ledger_accounts_ledger_account_id",
                      column: x => x.ledger_account_id,
                      principalTable: "tbl_ledger_accounts",
                      principalColumn: "ledger_account_id");
          });

      migrationBuilder.CreateTable(
          name: "tbl_suppliers",
          columns: table => new
          {
            date_added = table.Column<long>(type: "INTEGER", nullable: true),
            date_updated = table.Column<long>(type: "INTEGER", nullable: true),
            modification_status = table.Column<byte>(type: "INTEGER", nullable: true),
            supplier_id = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
            address = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true),
            contact_person = table.Column<string>(type: "TEXT", maxLength: 25, nullable: true),
            cost_centre_code = table.Column<string>(type: "TEXT", nullable: true),
            CostCentre_Caption = table.Column<string>(type: "TEXT", nullable: true),
            description = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
            email = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true),
            narration = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
            postal_code = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true),
            supplier_code = table.Column<string>(type: "TEXT", nullable: false),
            Supplier_Caption = table.Column<string>(type: "TEXT", nullable: true),
            supplier_ledger_account_id = table.Column<string>(type: "TEXT", maxLength: 10, nullable: true),
            telephone = table.Column<string>(type: "TEXT", maxLength: 25, nullable: true),
            town = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true)
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_tbl_suppliers", x => x.supplier_id);
            table.ForeignKey(
                      name: "FK_tbl_suppliers_tbl_cost_centres_cost_centre_code",
                      column: x => x.cost_centre_code,
                      principalTable: "tbl_cost_centres",
                      principalColumn: "cost_centre_code");
            table.ForeignKey(
                      name: "FK_tbl_suppliers_tbl_ledger_accounts_supplier_code",
                      column: x => x.supplier_code,
                      principalTable: "tbl_ledger_accounts",
                      principalColumn: "ledger_account_id",
                      onDelete: ReferentialAction.Cascade);
          });

      migrationBuilder.CreateTable(
          name: "tbl_user_audit_trails",
          columns: table => new
          {
            date_added = table.Column<long>(type: "INTEGER", nullable: true),
            date_updated = table.Column<long>(type: "INTEGER", nullable: true),
            modification_status = table.Column<byte>(type: "INTEGER", nullable: true),
            audit_id = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
            activity_date = table.Column<DateTime>(type: "TEXT", nullable: false),
            activity_enum_number = table.Column<short>(type: "INTEGER", nullable: false),
            category = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
            command_id = table.Column<string>(type: "TEXT", nullable: false),
            Command_Caption = table.Column<string>(type: "TEXT", nullable: true),
            data = table.Column<string>(type: "TEXT", nullable: false),
            description = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
            login_id = table.Column<string>(type: "TEXT", nullable: false),
            Login_Caption = table.Column<string>(type: "TEXT", nullable: true),
            narration = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
            old_values = table.Column<string>(type: "TEXT", nullable: false)
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_tbl_user_audit_trails", x => x.audit_id);
            table.ForeignKey(
                      name: "FK_tbl_user_audit_trails_tbl_command_details_command_id",
                      column: x => x.command_id,
                      principalTable: "tbl_command_details",
                      principalColumn: "command_id",
                      onDelete: ReferentialAction.Cascade);
            table.ForeignKey(
                      name: "FK_tbl_user_audit_trails_tbl_user_logins_login_id",
                      column: x => x.login_id,
                      principalTable: "tbl_user_logins",
                      principalColumn: "login_id",
                      onDelete: ReferentialAction.Cascade);
          });

      migrationBuilder.CreateTable(
          name: "tbl_verifications",
          columns: table => new
          {
            date_added = table.Column<long>(type: "INTEGER", nullable: true),
            date_updated = table.Column<long>(type: "INTEGER", nullable: true),
            modification_status = table.Column<byte>(type: "INTEGER", nullable: true),
            verification_id = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
            date_of_verification = table.Column<DateTime>(type: "TEXT", nullable: false),
            login_id = table.Column<string>(type: "TEXT", nullable: false),
            Login_Caption = table.Column<string>(type: "TEXT", nullable: true),
            narration = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
            record_id = table.Column<long>(type: "INTEGER", nullable: false),
            table_name = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
            verification_name = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
            verification_record_id = table.Column<long>(type: "INTEGER", nullable: false),
            verification_type_id = table.Column<long>(type: "INTEGER", nullable: false)
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_tbl_verifications", x => x.verification_id);
            table.ForeignKey(
                      name: "FK_tbl_verifications_tbl_user_logins_login_id",
                      column: x => x.login_id,
                      principalTable: "tbl_user_logins",
                      principalColumn: "login_id",
                      onDelete: ReferentialAction.Cascade);
          });

      migrationBuilder.CreateIndex(
          name: "IX_tbl_computer_anydesks_cost_centre_code",
          table: "tbl_computer_anydesks",
          column: "cost_centre_code");

      migrationBuilder.CreateIndex(
          name: "IX_tbl_data_devices_station_id",
          table: "tbl_data_devices",
          column: "station_id");

      migrationBuilder.CreateIndex(
          name: "IX_tbl_item_groups_parent_group_id",
          table: "tbl_item_groups",
          column: "parent_group_id");

      migrationBuilder.CreateIndex(
          name: "IX_tbl_leased_properties_accounts_cost_centre_code",
          table: "tbl_leased_properties_accounts",
          column: "cost_centre_code");

      migrationBuilder.CreateIndex(
          name: "IX_tbl_ledger_accounts_cost_centre_code",
          table: "tbl_ledger_accounts",
          column: "cost_centre_code");

      migrationBuilder.CreateIndex(
          name: "IX_tbl_ledger_accounts_ledger_account_code",
          table: "tbl_ledger_accounts",
          column: "ledger_account_code",
          unique: true);

      migrationBuilder.CreateIndex(
          name: "IX_tbl_let_properties_accounts_cost_centre_code",
          table: "tbl_let_properties_accounts",
          column: "cost_centre_code");

      migrationBuilder.CreateIndex(
          name: "IX_tbl_let_properties_accounts_ledger_account_id",
          table: "tbl_let_properties_accounts",
          column: "ledger_account_id");

      migrationBuilder.CreateIndex(
          name: "IX_tbl_qr_codes_requests_cost_centre_code",
          table: "tbl_qr_codes_requests",
          column: "cost_centre_code");

      migrationBuilder.CreateIndex(
          name: "IX_tbl_qr_request_items_cost_centre_code",
          table: "tbl_qr_request_items",
          column: "cost_centre_code");

      migrationBuilder.CreateIndex(
          name: "IX_tbl_qr_request_items_request_id",
          table: "tbl_qr_request_items",
          column: "request_id");

      migrationBuilder.CreateIndex(
          name: "IX_tbl_stock_items_group_id",
          table: "tbl_stock_items",
          column: "group_id");

      migrationBuilder.CreateIndex(
          name: "IX_tbl_suppliers_cost_centre_code",
          table: "tbl_suppliers",
          column: "cost_centre_code");

      migrationBuilder.CreateIndex(
          name: "IX_tbl_suppliers_supplier_code",
          table: "tbl_suppliers",
          column: "supplier_code",
          unique: true);

      migrationBuilder.CreateIndex(
          name: "IX_tbl_system_users_role_id",
          table: "tbl_system_users",
          column: "role_id");

      migrationBuilder.CreateIndex(
          name: "IX_tbl_user_audit_trails_command_id",
          table: "tbl_user_audit_trails",
          column: "command_id");

      migrationBuilder.CreateIndex(
          name: "IX_tbl_user_audit_trails_login_id",
          table: "tbl_user_audit_trails",
          column: "login_id");

      migrationBuilder.CreateIndex(
          name: "IX_tbl_user_logins_user_id",
          table: "tbl_user_logins",
          column: "user_id");

      migrationBuilder.CreateIndex(
          name: "IX_tbl_user_rights_command_id",
          table: "tbl_user_rights",
          column: "command_id");

      migrationBuilder.CreateIndex(
          name: "IX_tbl_user_rights_right_id",
          table: "tbl_user_rights",
          column: "right_id");

      migrationBuilder.CreateIndex(
          name: "IX_tbl_user_rights_role_id",
          table: "tbl_user_rights",
          column: "role_id");

      migrationBuilder.CreateIndex(
          name: "IX_tbl_user_rights_user_id",
          table: "tbl_user_rights",
          column: "user_id");

      migrationBuilder.CreateIndex(
          name: "IX_tbl_verification_rights_device_id",
          table: "tbl_verification_rights",
          column: "device_id");

      migrationBuilder.CreateIndex(
          name: "IX_tbl_verification_rights_user_id",
          table: "tbl_verification_rights",
          column: "user_id");

      migrationBuilder.CreateIndex(
          name: "IX_tbl_verification_rights_user_role_id",
          table: "tbl_verification_rights",
          column: "user_role_id",
          unique: true);

      migrationBuilder.CreateIndex(
          name: "IX_tbl_verifications_login_id",
          table: "tbl_verifications",
          column: "login_id");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DropTable(
          name: "Contributors");

      migrationBuilder.DropTable(
          name: "tbl_communication_messages");

      migrationBuilder.DropTable(
          name: "tbl_computer_anydesks");

      migrationBuilder.DropTable(
          name: "tbl_device_guids");

      migrationBuilder.DropTable(
          name: "tbl_let_properties_accounts");

      migrationBuilder.DropTable(
          name: "tbl_password_safes");

      migrationBuilder.DropTable(
          name: "tbl_qr_request_items");

      migrationBuilder.DropTable(
          name: "tbl_stock_items");

      migrationBuilder.DropTable(
          name: "tbl_suppliers");

      migrationBuilder.DropTable(
          name: "tbl_tims_machines");

      migrationBuilder.DropTable(
          name: "tbl_user_audit_trails");

      migrationBuilder.DropTable(
          name: "tbl_user_rights");

      migrationBuilder.DropTable(
          name: "tbl_verification_rights");

      migrationBuilder.DropTable(
          name: "tbl_verification_types");

      migrationBuilder.DropTable(
          name: "tbl_verifications");

      migrationBuilder.DropTable(
          name: "tbl_qr_codes_requests");

      migrationBuilder.DropTable(
          name: "tbl_item_groups");

      migrationBuilder.DropTable(
          name: "tbl_ledger_accounts");

      migrationBuilder.DropTable(
          name: "tbl_command_details");

      migrationBuilder.DropTable(
          name: "tbl_system_rights");

      migrationBuilder.DropTable(
          name: "tbl_data_devices");

      migrationBuilder.DropTable(
          name: "tbl_user_logins");

      migrationBuilder.DropTable(
          name: "tbl_leased_properties_accounts");

      migrationBuilder.DropTable(
          name: "tbl_system_users");

      migrationBuilder.DropTable(
          name: "tbl_cost_centres");

      migrationBuilder.DropTable(
          name: "tbl_user_roles");
    }
  }
}
