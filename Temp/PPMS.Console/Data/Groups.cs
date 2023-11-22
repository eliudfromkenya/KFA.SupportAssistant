using PPMS.Console.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace PPMS.Console.Data
{
    public struct Tables
    {
        public string RecId { get; set; }
        public string TableName { get; set; }
        public string NewName { get; set; }
        public string ImagePath { get; set; }
        public string GroupName { get; set; }
    }


    class Groups
    {
        public static Tables[] GetTables()
        {
            using var reader = new StringReader(names);
            var serializer = new XmlSerializer(typeof(Tables[]));
            var sb = serializer.Deserialize(reader);
            return sb as Tables[];
        }


        internal const string names = @"'1'~'Acceptance Creterias'~'AcceptanceCreterias'~'E:\Inspiration Materials\Images\Icons\open_icon_library-standard-0.11\open_icon_library-standard\icons\png\64x64\actions\dialog-ok-apply.png'~'Requirements/Development Tests'~
'2'~'Audit Activities'~'AuditActivities'~'E:\Inspiration Materials\Images\Icons\open_icon_library-standard-0.11\open_icon_library-standard\icons\png\64x64\actions\run-build-file.png'~'System/User Management/Audit'~
'3'~'Autoincrement Types'~'AutoincrementTypes'~'E:\Inspiration Materials\Images\Icons\open_icon_library-standard-0.11\open_icon_library-standard\icons\png\64x64\actions\arrow-left-double-3.png'~'Settings/Project/Data Settings'~
'4'~'Binary Objects'~'BinaryObjects'~'E:\Inspiration Materials\Images\Icons\open_icon_library-standard-0.11\open_icon_library-standard\icons\png\64x64\actions\document-print.png'~'System/General'~
'5'~'Cache Types'~'CacheTypes'~'E:\Inspiration Materials\Images\Icons\open_icon_library-standard-0.11\open_icon_library-standard\icons\png\64x64\actions\document-save-7.png'~'Settings/Project/Data Settings'~
'6'~'Column Tests'~'ColumnTests'~'E:\Inspiration Materials\Images\Icons\open_icon_library-standard-0.11\open_icon_library-standard\icons\png\64x64\actions\view-calendar-tasks.png'~'Requirements/Development Tests'~
'7'~'Column User Language Translation Types'~'ColumnUserLanguageTranslationTypes'~'E:\Inspiration Materials\Images\Icons\open_icon_library-standard-0.11\open_icon_library-standard\icons\png\64x64\actions\document-import-2.png'~'Settings/Entry Settings'~
'8'~'Command Details'~'CommandDetails'~'E:\Inspiration Materials\Images\Icons\open_icon_library-standard-0.11\open_icon_library-standard\icons\png\64x64\actions\tools-wizard.png'~'System/General'~
'9'~'Comments And Notes'~'CommentsAndNotes'~'E:\Inspiration Materials\Images\Icons\open_icon_library-standard-0.11\open_icon_library-standard\icons\png\64x64\actions\appointment-new-4.png'~'Project Data/Project Data'~
'10'~'Communication Columns'~'CommunicationColumns'~'E:\Inspiration Materials\Images\Icons\open_icon_library-standard-0.11\open_icon_library-standard\icons\png\64x64\actions\format-justify-left-4.png'~'Project Data/General Data'~
'11'~'Communication Types'~'CommunicationTypes'~'E:\Inspiration Materials\Images\Icons\open_icon_library-standard-0.11\open_icon_library-standard\icons\png\64x64\actions\view-web-browser-dom-tree.png'~'Settings/Entry Settings'~
'12'~'Contacts'~'Contacts'~'E:\Inspiration Materials\Images\Icons\open_icon_library-standard-0.11\open_icon_library-standard\icons\png\64x64\actions\contact-new-2.png'~'System/Contacts'~'1'
'13'~'Control Locations'~'ControlLocations'~'E:\Inspiration Materials\Images\Icons\open_icon_library-standard-0.11\open_icon_library-standard\icons\png\64x64\actions\document-page-setup.png'~'Project Data/Forms/Controls'~
'14'~'Control Properties'~'ControlProperties'~'E:\Inspiration Materials\Images\Icons\open_icon_library-standard-0.11\open_icon_library-standard\icons\png\64x64\actions\document-sign-3.png'~'Project Data/Forms/Controls'~
'15'~'Control Types'~'ControlTypes'~'E:\Inspiration Materials\Images\Icons\open_icon_library-standard-0.11\open_icon_library-standard\icons\png\64x64\actions\go-jump-4.png'~'Settings/Entry Settings'~
'16'~'Current Project Development Methodology Stages'~'CurrentProjectDevelopmentMethodologyStages'~'E:\Inspiration Materials\Images\Icons\open_icon_library-standard-0.11\open_icon_library-standard\icons\png\64x64\actions\object-flip-horizontal-2.png'~'Project Option/Development Guidelines'~
'17'~'Custom Form Columns'~'CustomFormColumns'~'E:\Inspiration Materials\Images\Icons\open_icon_library-standard-0.11\open_icon_library-standard\icons\png\64x64\actions\document-export-2.png'~'Project Data/Forms/Forms'~
'18'~'Data Devices'~'DataDevices'~'E:\Inspiration Materials\Images\Icons\open_icon_library-standard-0.11\open_icon_library-standard\icons\png\64x64\actions\help-contents-3.png'~'System/General'~
'19'~'Data Objects'~'DataObjects'~'E:\Inspiration Materials\Images\Icons\open_icon_library-standard-0.11\open_icon_library-standard\icons\png\64x64\actions\view-list-icon-2.png'~'Project Data/General Data'~
'20'~'Data Of Solution Sub Templates'~'DataOfSolutionSubTemplates'~'E:\Inspiration Materials\Images\Icons\open_icon_library-standard-0.11\open_icon_library-standard\icons\png\64x64\actions\project-development.png'~'Project Data/Solution Templates'~
'21'~'Data Types'~'DataTypes'~'E:\Inspiration Materials\Images\Icons\open_icon_library-standard-0.11\open_icon_library-standard\icons\png\64x64\actions\view-list-tree-2.png'~'Settings/Project/Data Settings'~
'22'~'Data Validations'~'DataValidations'~'E:\Inspiration Materials\Images\Icons\open_icon_library-standard-0.11\open_icon_library-standard\icons\png\64x64\actions\tools-check-spelling.png'~'Settings/Project/Data Settings'~
'23'~'Database Access Layers'~'DatabaseAccessLayers'~'E:\Inspiration Materials\Images\Icons\open_icon_library-standard-0.11\open_icon_library-standard\icons\png\64x64\actions\document-save-as-3.png'~'Settings/Project/Data Settings'~
'24'~'Database Data Types'~'DatabaseDataTypes'~'E:\Inspiration Materials\Images\Icons\open_icon_library-standard-0.11\open_icon_library-standard\icons\png\64x64\actions\tools-check-spelling-5.png'~'Settings/Project/Data Settings'~
'25'~'Database Tables'~'DatabaseTables'~'E:\Inspiration Materials\Images\Icons\open_icon_library-standard-0.11\open_icon_library-standard\icons\png\64x64\actions\games-config-board.png'~'Project Data/Project Data'~
'26'~'Datagrid Columns'~'DatagridColumns'~'E:\Inspiration Materials\Images\Icons\open_icon_library-standard-0.11\open_icon_library-standard\icons\png\64x64\actions\format-justify-center-6.png'~'System/General'~
'27'~'Default Records'~'DefaultRecords'~'E:\Inspiration Materials\Images\Icons\open_icon_library-standard-0.11\open_icon_library-standard\icons\png\64x64\actions\project-development-new-template.png'~'Project Data/General Data'~
'28'~'Development Methodologies'~'DevelopmentMethodologies'~'E:\Inspiration Materials\Images\Icons\open_icon_library-standard-0.11\open_icon_library-standard\icons\png\64x64\actions\archive-insert-2.png'~'Project Data/Development Guidelines'~
'29'~'Development Methodologies Details'~'DevelopmentMethodologiesDetails'~'E:\Inspiration Materials\Images\Icons\open_icon_library-standard-0.11\open_icon_library-standard\icons\png\64x64\actions\edit.png'~'Project Data/Development Guidelines'~
'30'~'Development Role Assignments'~'DevelopmentRoleAssignments'~'E:\Inspiration Materials\Images\Icons\open_icon_library-standard-0.11\open_icon_library-standard\icons\png\64x64\actions\mail-reply-all-3.png'~'Project Data/Development Guidelines'~
'31'~'Development States'~'DevelopmentStates'~'E:\Inspiration Materials\Images\Icons\open_icon_library-standard-0.11\open_icon_library-standard\icons\png\64x64\actions\view-process-all-tree.png'~'Project Data/Development Guidelines'~
'32'~'Development Team General Role Assignments'~'DevelopmentTeamGeneralRoleAssignments'~'E:\Inspiration Materials\Images\Icons\open_icon_library-standard-0.11\open_icon_library-standard\icons\png\64x64\actions\mail-forward-6.png'~'Design And Planning/Development Team'~
'33'~'Development Team Members'~'DevelopmentTeamMembers'~'E:\Inspiration Materials\Images\Icons\open_icon_library-standard-0.11\open_icon_library-standard\icons\png\64x64\actions\user-group-new-2.png'~'Design And Planning/Development Team'~
'34'~'Development Team Roles'~'DevelopmentTeamRoles'~'E:\Inspiration Materials\Images\Icons\open_icon_library-standard-0.11\open_icon_library-standard\icons\png\64x64\actions\user-properties.png'~'Design And Planning/Development Team'~
'35'~'Development Teams'~'DevelopmentTeams'~'E:\Inspiration Materials\Images\Icons\open_icon_library-standard-0.11\open_icon_library-standard\icons\png\64x64\actions\user-properties.png'~'Design And Planning/Development Team'~
'36'~'Diagram Connector Types'~'DiagramConnectorTypes'~'E:\Inspiration Materials\Images\Icons\open_icon_library-standard-0.11\open_icon_library-standard\icons\png\64x64\actions\object-flip-horizontal.png'~'Implimentations/Diagrams'~
'37'~'Diagram Connectors'~'DiagramConnectors'~'E:\Inspiration Materials\Images\Icons\open_icon_library-standard-0.11\open_icon_library-standard\icons\png\64x64\actions\object-flip-horizontal.png'~'Implimentations/Diagrams'~
'38'~'Diagram Entities'~'DiagramEntities'~'E:\Inspiration Materials\Images\Icons\open_icon_library-standard-0.11\open_icon_library-standard\icons\png\64x64\actions\select-rectangular.png'~'Implimentations/Diagrams'~
'39'~'Diagram Types'~'DiagramTypes'~'E:\Inspiration Materials\Images\Icons\open_icon_library-standard-0.11\open_icon_library-standard\icons\png\64x64\actions\transform-move.png'~'Implimentations/Diagrams'~
'40'~'Diagrams'~'Diagrams'~'E:\Inspiration Materials\Images\Icons\open_icon_library-standard-0.11\open_icon_library-standard\icons\png\64x64\actions\transform-crop-and-resize.png'~'Implimentations/Diagrams'~
'41'~'Disabled Columns'~'DisabledColumns'~'E:\Inspiration Materials\Images\Icons\open_icon_library-standard-0.11\open_icon_library-standard\icons\png\64x64\actions\format-justify-right-6.png'~'Project Data/General Data'~
'42'~'Encryption Types'~'EncryptionTypes'~'E:\Inspiration Materials\Images\Icons\open_icon_library-standard-0.11\open_icon_library-standard\icons\png\64x64\actions\halfencrypted.png'~'Settings/Entry Settings'~
'43'~'Enum Values'~'EnumValues'~'E:\Inspiration Materials\Images\Icons\open_icon_library-standard-0.11\open_icon_library-standard\icons\png\64x64\actions\view-fullscreen-4.png'~'Project Data/Project Data'~
'44'~'Epic Details'~'EpicDetails'~'E:\Inspiration Materials\Images\Icons\open_icon_library-standard-0.11\open_icon_library-standard\icons\png\64x64\actions\mail-attach.png'~'Requirements/User Stories'~
'45'~'Epics Current States'~'EpicsCurrentStates'~'E:\Inspiration Materials\Images\Icons\open_icon_library-standard-0.11\open_icon_library-standard\icons\png\64x64\actions\new-tab.png'~'Requirements/User Stories'~
'46'~'Form Columns'~'FormColumns'~'E:\Inspiration Materials\Images\Icons\open_icon_library-standard-0.11\open_icon_library-standard\icons\png\64x64\actions\format-justify-center-4.png'~'Project Data/Forms/Forms'~
'47'~'Form Controls'~'FormControls'~'E:\Inspiration Materials\Images\Icons\open_icon_library-standard-0.11\open_icon_library-standard\icons\png\64x64\actions\insert-text-3.png'~'Project Data/Forms/Forms'~
'48'~'Form Tables'~'FormTables'~'E:\Inspiration Materials\Images\Icons\open_icon_library-standard-0.11\open_icon_library-standard\icons\png\64x64\actions\games-config-board.png'~'Project Data/Forms/Forms'~
'49'~'Form Types'~'FormTypes'~'E:\Inspiration Materials\Images\Icons\open_icon_library-standard-0.11\open_icon_library-standard\icons\png\64x64\actions\help-contents-2.png'~'Project Data/Forms/Forms'~
'50'~'Forms'~'Forms'~'E:\Inspiration Materials\Images\Icons\open_icon_library-standard-0.11\open_icon_library-standard\icons\png\64x64\actions\bookmark-5.png'~'Project Data/Forms/Forms'~
'51'~'General Project Encryptions'~'GeneralProjectEncryptions'~'E:\Inspiration Materials\Images\Icons\open_icon_library-standard-0.11\open_icon_library-standard\icons\png\64x64\actions\halfencrypted.png'~'Project Data/General Data/Encryptions'~
'52'~'Groups'~'Groups'~'E:\Inspiration Materials\Images\Icons\open_icon_library-standard-0.11\open_icon_library-standard\icons\png\64x64\actions\view-process-all.png'~'Project Data/Structures'~'196'
'53'~'Hardware Requirements'~'HardwareRequirements'~'E:\Inspiration Materials\Images\Icons\open_icon_library-standard-0.11\open_icon_library-standard\icons\png\64x64\actions\tool-animator.png'~'Requirements/General'~
'54'~'Images'~'Images'~'E:\Inspiration Materials\Images\Icons\open_icon_library-standard-0.11\open_icon_library-standard\icons\png\64x64\actions\insert-image-2.png'~'System/General'~
'55'~'Menu Types'~'MenuTypes'~'E:\Inspiration Materials\Images\Icons\open_icon_library-standard-0.11\open_icon_library-standard\icons\png\64x64\actions\games-solve.png'~'Project Data/Forms/Controls'~
'56'~'Menus'~'Menus'~'E:\Inspiration Materials\Images\Icons\open_icon_library-standard-0.11\open_icon_library-standard\icons\png\64x64\actions\bookmark-new-3.png'~'Project Data/Forms/Controls'~
'57'~'Method Arguments'~'MethodArguments'~'E:\Inspiration Materials\Images\Icons\open_icon_library-standard-0.11\open_icon_library-standard\icons\png\64x64\actions\bookmark-toolbar-4.png'~'Project Data/Structures'~
'58'~'Module Functions'~'ModuleFunctions'~'E:\Inspiration Materials\Images\Icons\open_icon_library-standard-0.11\open_icon_library-standard\icons\png\64x64\actions\project-open.png'~'Project Data/Structures'~
'59'~'Module Services'~'ModuleServices'~'E:\Inspiration Materials\Images\Icons\open_icon_library-standard-0.11\open_icon_library-standard\icons\png\64x64\actions\view-list-icon-4.png'~'Project Data/Structures'~
'60'~'Platforms'~'Platforms'~'E:\Inspiration Materials\Images\Icons\open_icon_library-standard-0.11\open_icon_library-standard\icons\png\64x64\actions\view-list-icons.png'~'Settings/General'~'16'
'61'~'Primary Keys'~'PrimaryKeys'~'E:\Inspiration Materials\Images\Icons\open_icon_library-standard-0.11\open_icon_library-standard\icons\png\64x64\actions\insert-table.png'~'Project Data/Project Data'~
'62'~'Programming Language Data Types'~'ProgrammingLanguageDataTypes'~'E:\Inspiration Materials\Images\Icons\open_icon_library-standard-0.11\open_icon_library-standard\icons\png\64x64\actions\format-wordwrap.png'~'Project Option/Project Options'~
'63'~'Programming Languages'~'ProgrammingLanguages'~'E:\Inspiration Materials\Images\Icons\open_icon_library-standard-0.11\open_icon_library-standard\icons\png\64x64\actions\tools-check-spelling-3.png'~'Settings/Entry Settings'~
'64'~'Project Budget Estimations'~'ProjectBudgetEstimations'~'E:\Inspiration Materials\Images\Icons\open_icon_library-standard-0.11\open_icon_library-standard\icons\png\64x64\actions\office-chart-bar-percentage.png'~'Project Option/Estimations'~
'65'~'Project Cache Types'~'ProjectCacheTypes'~'E:\Inspiration Materials\Images\Icons\open_icon_library-standard-0.11\open_icon_library-standard\icons\png\64x64\actions\db_update.png'~'Project Option/Structures'~
'66'~'Project Data Access Layers'~'ProjectDataAccessLayers'~'E:\Inspiration Materials\Images\Icons\open_icon_library-standard-0.11\open_icon_library-standard\icons\png\64x64\actions\document-save-as-3.png'~'Project Option/Structures'~
'67'~'Project Databases'~'ProjectDatabases'~'E:\Inspiration Materials\Images\Icons\open_icon_library-standard-0.11\open_icon_library-standard\icons\png\64x64\actions\db_add-2.png'~'Project Option/Structures'~
'68'~'Project Development Methodologies'~'ProjectDevelopmentMethodologies'~'E:\Inspiration Materials\Images\Icons\open_icon_library-standard-0.11\open_icon_library-standard\icons\png\64x64\actions\transform-scale-2.png'~'Project Option/Development Guidelines'~
'69'~'Project Groups'~'ProjectGroups'~'E:\Inspiration Materials\Images\Icons\open_icon_library-standard-0.11\open_icon_library-standard\icons\png\64x64\actions\view-list-icon.png'~'Project Option/Structures'~
'70'~'Project Images'~'ProjectImages'~'E:\Inspiration Materials\Images\Icons\open_icon_library-standard-0.11\open_icon_library-standard\icons\png\64x64\actions\insert-image-2.png'~'System/General'~
'71'~'Project Options'~'ProjectOptions'~'E:\Inspiration Materials\Images\Icons\open_icon_library-standard-0.11\open_icon_library-standard\icons\png\64x64\actions\format-indent-less-3.png'~'Project Option/Project Options'~
'72'~'Project Platforms'~'ProjectPlatforms'~'E:\Inspiration Materials\Images\Icons\open_icon_library-standard-0.11\open_icon_library-standard\icons\png\64x64\actions\document-properties-2.png'~'Project Option/Project Options'~
'73'~'Project Programming Languages'~'ProjectProgrammingLanguages'~'E:\Inspiration Materials\Images\Icons\open_icon_library-standard-0.11\open_icon_library-standard\icons\png\64x64\actions\view-list-text-3.png'~'Project Option/Project Options'~
'74'~'Project Releases'~'ProjectReleases'~'E:\Inspiration Materials\Images\Icons\open_icon_library-standard-0.11\open_icon_library-standard\icons\png\64x64\actions\appointment-new-3.png'~'Project Option/Estimations'~
'75'~'Project Report Formats'~'ProjectReportFormats'~'E:\Inspiration Materials\Images\Icons\open_icon_library-standard-0.11\open_icon_library-standard\icons\png\64x64\actions\document-properties-3.png'~'Project Option/Project Options'~
'76'~'Project Scopes'~'ProjectScopes'~'E:\Inspiration Materials\Images\Icons\open_icon_library-standard-0.11\open_icon_library-standard\icons\png\64x64\actions\trash-empty-2.png'~'Project Option/Project Options'~
'77'~'Project Software Solutions Types'~'ProjectSoftwareSolutionsTypes'~'E:\Inspiration Materials\Images\Icons\open_icon_library-standard-0.11\open_icon_library-standard\icons\png\64x64\actions\configure-4.png'~'Project Option/Development Guidelines'~
'78'~'Project System Types'~'ProjectSystemTypes'~'E:\Inspiration Materials\Images\Icons\open_icon_library-standard-0.11\open_icon_library-standard\icons\png\64x64\actions\view-fullscreen-3.png'~'Project Option/Project Options'~
'79'~'Project Test Frameworks'~'ProjectTestFrameworks'~'E:\Inspiration Materials\Images\Icons\open_icon_library-standard-0.11\open_icon_library-standard\icons\png\64x64\actions\view-calendar-tasks.png'~'Project Option/Development Guidelines'~
'80'~'Project Time Estimations'~'ProjectTimeEstimations'~'E:\Inspiration Materials\Images\Icons\open_icon_library-standard-0.11\open_icon_library-standard\icons\png\64x64\actions\office-chart-line-percentage.png'~'Project Option/Estimations'~
'81'~'Project User Languages'~'ProjectUserLanguages'~'E:\Inspiration Materials\Images\Icons\open_icon_library-standard-0.11\open_icon_library-standard\icons\png\64x64\actions\tools-check-spelling-3.png'~'Project Option/Project Options'~
'82'~'Projects'~'Projects'~'E:\Inspiration Materials\Images\Icons\open_icon_library-standard-0.11\open_icon_library-standard\icons\png\64x64\actions\project-open-3.png'~'System/General'~'1'
'83'~'Queries'~'Queries'~'E:\Inspiration Materials\Images\Icons\open_icon_library-standard-0.11\open_icon_library-standard\icons\png\64x64\actions\edit-find-4.png'~'Project Data/Queries'~
'84'~'Query Columns'~'QueryColumns'~'E:\Inspiration Materials\Images\Icons\open_icon_library-standard-0.11\open_icon_library-standard\icons\png\64x64\actions\format-justify-center-4.png'~'Project Data/Queries'~
'85'~'Query Tables'~'QueryTables'~'E:\Inspiration Materials\Images\Icons\open_icon_library-standard-0.11\open_icon_library-standard\icons\png\64x64\actions\edit-find-mail.png'~'Project Data/Queries'~
'86'~'Record Modes'~'RecordModes'~'E:\Inspiration Materials\Images\Icons\open_icon_library-standard-0.11\open_icon_library-standard\icons\png\64x64\actions\document-page-setup.png'~'Settings/General'~
'87'~'Refference Dependancies'~'RefferenceDependancies'~'E:\Inspiration Materials\Images\Icons\open_icon_library-standard-0.11\open_icon_library-standard\icons\png\64x64\actions\view-restore.png'~'Project Data/General Data'~
'88'~'Relationship Types'~'RelationshipTypes'~'E:\Inspiration Materials\Images\Icons\open_icon_library-standard-0.11\open_icon_library-standard\icons\png\64x64\actions\view-media-playlist.png'~'Settings/Entry Settings'~
'89'~'Relationships'~'Relationships'~'E:\Inspiration Materials\Images\Icons\open_icon_library-standard-0.11\open_icon_library-standard\icons\png\64x64\actions\view-media-equalizer.png'~'Project Data/Project Data'~
'90'~'Report Columns'~'ReportColumns'~'E:\Inspiration Materials\Images\Icons\open_icon_library-standard-0.11\open_icon_library-standard\icons\png\64x64\actions\edit-copy-4.png'~'Project Data/Reports'~
'91'~'Report Formats'~'ReportFormats'~'E:\Inspiration Materials\Images\Icons\open_icon_library-standard-0.11\open_icon_library-standard\icons\png\64x64\actions\document-print-frame-3.png'~'Project Data/Reports'~
'92'~'Reports'~'Reports'~'E:\Inspiration Materials\Images\Icons\open_icon_library-standard-0.11\open_icon_library-standard\icons\png\64x64\actions\edit-copy-3.png'~'Project Data/Reports'~
'93'~'Requirement Categories'~'RequirementCategories'~'E:\Inspiration Materials\Images\Icons\open_icon_library-standard-0.11\open_icon_library-standard\icons\png\64x64\actions\transform-move.png'~'Requirements/General'~
'94'~'Requirement Platforms'~'RequirementPlatforms'~'E:\Inspiration Materials\Images\Icons\open_icon_library-standard-0.11\open_icon_library-standard\icons\png\64x64\actions\edit-paste-2.png'~'Requirements/General'~
'95'~'Requirement Types'~'RequirementTypes'~'E:\Inspiration Materials\Images\Icons\open_icon_library-standard-0.11\open_icon_library-standard\icons\png\64x64\actions\document-open-4.png'~'Requirements/General'~
'96'~'Setting Values'~'SettingValues'~'E:\Inspiration Materials\Images\Icons\open_icon_library-standard-0.11\open_icon_library-standard\icons\png\64x64\actions\configure-toolbars.png'~'System/General'~
'97'~'Software Solution Project Types'~'SoftwareSolutionProjectTypes'~'E:\Inspiration Materials\Images\Icons\open_icon_library-standard-0.11\open_icon_library-standard\icons\png\64x64\actions\address-book-new-4.png'~'Project Data/Solution Templates'~
'98'~'Solution Sub Template Files'~'SolutionSubTemplateFiles'~'E:\Inspiration Materials\Images\Icons\open_icon_library-standard-0.11\open_icon_library-standard\icons\png\64x64\actions\address-book-new.png'~'Project Data/Solution Templates'~
'99'~'Solution Sub Templates'~'SolutionSubTemplates'~'E:\Inspiration Materials\Images\Icons\open_icon_library-standard-0.11\open_icon_library-standard\icons\png\64x64\actions\address-book-new-2.png'~'Project Data/Solution Templates'~
'100'~'Solution Templates'~'SolutionTemplates'~'E:\Inspiration Materials\Images\Icons\open_icon_library-standard-0.11\open_icon_library-standard\icons\png\64x64\actions\insert-object.png'~'Project Data/Solution Templates'~
'101'~'Special Encryptions'~'SpecialEncryptions'~'E:\Inspiration Materials\Images\Icons\open_icon_library-standard-0.11\open_icon_library-standard\icons\png\64x64\actions\lock.png'~'Project Data/General Data/Encryptions'~
'102'~'Sync Triggers Control'~'SyncTriggersControl'~'E:\Inspiration Materials\Images\Icons\open_icon_library-standard-0.11\open_icon_library-standard\icons\png\64x64\actions\edit-find-and-replace.png'~'System/General'~
'103'~'Synchronization Columns'~'SynchronizationColumns'~'E:\Inspiration Materials\Images\Icons\open_icon_library-standard-0.11\open_icon_library-standard\icons\png\64x64\actions\view-refresh-5.png'~'Settings/Project/Data Settings'~
'104'~'Synchronization Types'~'SynchronizationTypes'~'E:\Inspiration Materials\Images\Icons\open_icon_library-standard-0.11\open_icon_library-standard\icons\png\64x64\actions\view-refresh-4.png'~'Settings/Entry Settings'~
'105'~'System Data'~'SystemData'~'E:\Inspiration Materials\Images\Icons\open_icon_library-standard-0.11\open_icon_library-standard\icons\png\64x64\actions\document-open-5.png'~'Settings/Project/Data Settings'~
'106'~'System Data Columns'~'SystemDataColumns'~'E:\Inspiration Materials\Images\Icons\open_icon_library-standard-0.11\open_icon_library-standard\icons\png\64x64\actions\view-list-icon.png'~'Project Data/General Data'~
'107'~'System Databases'~'SystemDatabases'~'E:\Inspiration Materials\Images\Icons\open_icon_library-standard-0.11\open_icon_library-standard\icons\png\64x64\actions\db.png'~'Settings/Project/Data Settings'~
'108'~'System Menu Items'~'SystemMenuItems'~'E:\Inspiration Materials\Images\Icons\open_icon_library-standard-0.11\open_icon_library-standard\icons\png\64x64\actions\character-set.png'~'Settings/Entry Settings'~
'109'~'System Modules'~'SystemModules'~'E:\Inspiration Materials\Images\Icons\open_icon_library-standard-0.11\open_icon_library-standard\icons\png\64x64\actions\project-open-3.png'~'Project Data/Structures'~
'110'~'System Requirements'~'SystemRequirements'~'E:\Inspiration Materials\Images\Icons\open_icon_library-standard-0.11\open_icon_library-standard\icons\png\64x64\actions\edit-4.png'~'Requirements/General'~
'111'~'System Settings'~'SystemSettings'~'E:\Inspiration Materials\Images\Icons\open_icon_library-standard-0.11\open_icon_library-standard\icons\png\64x64\actions\system-run-3.png'~'Settings/General'~
'112'~'System StakeHolders'~'SystemStakeHolders'~'E:\Inspiration Materials\Images\Icons\open_icon_library-standard-0.11\open_icon_library-standard\icons\png\64x64\actions\view-process-own.png'~'Project Option/Estimations'~
'113'~'System Test Cases'~'SystemTestCases'~'E:\Inspiration Materials\Images\Icons\open_icon_library-standard-0.11\open_icon_library-standard\icons\png\64x64\actions\games-endturn-2.png'~'Requirements/Development Tests'~
'114'~'System Test Frameworks'~'SystemTestFrameworks'~'E:\Inspiration Materials\Images\Icons\open_icon_library-standard-0.11\open_icon_library-standard\icons\png\64x64\actions\view-calendar-tasks.png'~'Requirements/Development Tests'~
'115'~'System Test Types'~'SystemTestTypes'~'E:\Inspiration Materials\Images\Icons\open_icon_library-standard-0.11\open_icon_library-standard\icons\png\64x64\actions\network-connect-3.png'~'Requirements/Development Tests'~
'116'~'System Types'~'SystemTypes'~'E:\Inspiration Materials\Images\Icons\open_icon_library-standard-0.11\open_icon_library-standard\icons\png\64x64\actions\format-text-strikethrough-5.png'~'System/General'~
'117'~'System Users'~'SystemUsers'~'E:\Inspiration Materials\Images\Icons\open_icon_library-standard-0.11\open_icon_library-standard\icons\png\64x64\actions\mail-reply-all-3.png'~'System/User Management'~
'118'~'System Validation Types'~'SystemValidationTypes'~'E:\Inspiration Materials\Images\Icons\open_icon_library-standard-0.11\open_icon_library-standard\icons\png\64x64\actions\klipper_doc.png'~'Settings/General'~
'119'~'Table Columns'~'TableColumns'~'E:\Inspiration Materials\Images\Icons\open_icon_library-standard-0.11\open_icon_library-standard\icons\png\64x64\actions\format-justify-center-6.png'~'Project Data/Project Data'~
'120'~'Table Inter-Dependancies'~'TableInterDependancies'~'E:\Inspiration Materials\Images\Icons\open_icon_library-standard-0.11\open_icon_library-standard\icons\png\64x64\actions\insert-link.png'~'Project Data/Project Data'~
'121'~'Test Edge Cases'~'TestEdgeCases'~'E:\Inspiration Materials\Images\Icons\open_icon_library-standard-0.11\open_icon_library-standard\icons\png\64x64\actions\news-subscribe-2.png'~'Requirements/Development Tests'~
'122'~'Unique Column Combination Masters'~'UniqueColumnCombinationMasters'~'E:\Inspiration Materials\Images\Icons\open_icon_library-standard-0.11\open_icon_library-standard\icons\png\64x64\actions\insert-pause.png'~'Project Data/General Data/Unique Combinations'~
'123'~'Unique Combination Details'~'UniqueCombinationDetails'~'E:\Inspiration Materials\Images\Icons\open_icon_library-standard-0.11\open_icon_library-standard\icons\png\64x64\actions\mail-send-receive-2.png'~'Project Data/General Data/Unique Combinations'~
'124'~'Use Case Actor Types'~'UseCaseActorTypes'~'E:\Inspiration Materials\Images\Icons\open_icon_library-standard-0.11\open_icon_library-standard\icons\png\64x64\actions\project-open.png'~'Requirements/Use Cases And UML'~
'125'~'Use Case Actors'~'UseCaseActors'~'E:\Inspiration Materials\Images\Icons\open_icon_library-standard-0.11\open_icon_library-standard\icons\png\64x64\actions\im-invisible-user.png'~'Requirements/Use Cases And UML'~
'126'~'Use Case Processes'~'UseCaseProcesses'~'E:\Inspiration Materials\Images\Icons\open_icon_library-standard-0.11\open_icon_library-standard\icons\png\64x64\actions\appointment-new-4.png'~'Requirements/Use Cases And UML'~
'127'~'Use Case Relationship Types'~'UseCaseRelationshipTypes'~'E:\Inspiration Materials\Images\Icons\open_icon_library-standard-0.11\open_icon_library-standard\icons\png\64x64\actions\insert-link-2.png'~'Requirements/Use Cases And UML'~
'128'~'Use Cases'~'UseCases'~'E:\Inspiration Materials\Images\Icons\open_icon_library-standard-0.11\open_icon_library-standard\icons\png\64x64\actions\view-process-own.png'~'Requirements/Use Cases And UML'~
'129'~'User Audit Trails'~'UserAuditTrails'~'E:\Inspiration Materials\Images\Icons\open_icon_library-standard-0.11\open_icon_library-standard\icons\png\64x64\actions\user-group-properties.png'~'System/User Management/Audit'~
'130'~'User Epic Reviews'~'UserEpicReviews'~'E:\Inspiration Materials\Images\Icons\open_icon_library-standard-0.11\open_icon_library-standard\icons\png\64x64\actions\transform-crop.png'~'Requirements/User Stories'~
'131'~'User Language Translations'~'UserLanguageTranslations'~'E:\Inspiration Materials\Images\Icons\open_icon_library-standard-0.11\open_icon_library-standard\icons\png\64x64\actions\document-open-remote.png'~'Project Data/Project Data'~
'132'~'User Language Translations Types'~'UserLanguageTranslationsTypes'~'E:\Inspiration Materials\Images\Icons\open_icon_library-standard-0.11\open_icon_library-standard\icons\png\64x64\actions\help-about-kde-2.png'~'Settings/General'~
'133'~'User Languages'~'UserLanguages'~'E:\Inspiration Materials\Images\Icons\open_icon_library-standard-0.11\open_icon_library-standard\icons\png\64x64\actions\format-text-underline-2.png'~'Settings/General'~
'134'~'User Logins'~'UserLogins'~'E:\Inspiration Materials\Images\Icons\open_icon_library-standard-0.11\open_icon_library-standard\icons\png\64x64\actions\document-decrypt-4.png'~'System/User Management/Audit'~
'135'~'User Requirement Stories'~'UserRequirementStories'~'E:\Inspiration Materials\Images\Icons\open_icon_library-standard-0.11\open_icon_library-standard\icons\png\64x64\actions\personal.png'~'Requirements/General'~
'136'~'User Rights'~'UserRights'~'E:\Inspiration Materials\Images\Icons\open_icon_library-standard-0.11\open_icon_library-standard\icons\png\64x64\actions\document-encrypt.png'~'System/User Management'~
'137'~'User Roles'~'UserRoles'~'E:\Inspiration Materials\Images\Icons\open_icon_library-standard-0.11\open_icon_library-standard\icons\png\64x64\actions\im-user.png'~'System/User Management'~
'138'~'User Story Epics'~'UserStoryEpics'~'E:\Inspiration Materials\Images\Icons\open_icon_library-standard-0.11\open_icon_library-standard\icons\png\64x64\actions\mail-reply-all-6.png'~'Requirements/User Stories'~
";
    }
}
