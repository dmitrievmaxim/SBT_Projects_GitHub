﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18444
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace HPSM_client.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "12.0.0.0")]
    internal sealed partial class HPSM_Settings : global::System.Configuration.ApplicationSettingsBase {
        
        private static HPSM_Settings defaultInstance = ((HPSM_Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new HPSM_Settings())));
        
        public static HPSM_Settings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("СП_ИНЦ_*")]
        public string fileMask {
            get {
                return ((string)(this["fileMask"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=192.168.99.102)(PORT=1500))" +
            "(CONNECT_DATA=(SERVICE_NAME=SBTJ2)));User Id=SBT_REP;Password=S8T_R3P;")]
        public string jiraTestConnectionString {
            get {
                return ((string)(this["jiraTestConnectionString"]));
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("HPSM_labor_01.01.2016")]
        public string prevFileName {
            get {
                return ((string)(this["prevFileName"]));
            }
            set {
                this["prevFileName"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("2016-01-01")]
        public global::System.DateTime prevFileCreatedDate {
            get {
                return ((global::System.DateTime)(this["prevFileCreatedDate"]));
            }
            set {
                this["prevFileCreatedDate"] = value;
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("C:\\Users\\SBT-Dmitriev-MV\\Desktop\\tmp\\")]
        public string pathToFiles {
            get {
                return ((string)(this["pathToFiles"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=192.168.99.114)(PORT=1500))" +
            "(CONNECT_DATA=(SERVICE_NAME=SBTJ2)));User Id=SBT_REP;Password=S8T_R3P;")]
        public string jiraProdConnectionString {
            get {
                return ((string)(this["jiraProdConnectionString"]));
            }
        }
    }
}
