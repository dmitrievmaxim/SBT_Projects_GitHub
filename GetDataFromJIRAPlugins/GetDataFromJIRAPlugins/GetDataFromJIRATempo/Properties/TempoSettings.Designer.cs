﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18444
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace GetDataFromJIRATempo.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "12.0.0.0")]
    internal sealed partial class TempoSettings : global::System.Configuration.ApplicationSettingsBase {
        
        private static TempoSettings defaultInstance = ((TempoSettings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new TempoSettings())));
        
        public static TempoSettings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("2016-03-01")]
        public global::System.DateTime dateStart {
            get {
                return ((global::System.DateTime)(this["dateStart"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("2016-03-31")]
        public global::System.DateTime dateFinish {
            get {
                return ((global::System.DateTime)(this["dateFinish"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("https://atlasminsk.sbertech.by/jira/")]
        public string jiraProdBaseURL {
            get {
                return ((string)(this["jiraProdBaseURL"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("https://atlasminsk.sbertech.by/jira/rest/api/latest/")]
        public string jiraRESTProdBaseURL {
            get {
                return ((string)(this["jiraRESTProdBaseURL"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("TempoAdmin")]
        public string jiraProdUsername {
            get {
                return ((string)(this["jiraProdUsername"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Ntvgjflvby")]
        public string jiraProdPassword {
            get {
                return ((string)(this["jiraProdPassword"]));
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
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=192.168.99.102)(PORT=1500))" +
            "(CONNECT_DATA=(SERVICE_NAME=SBTJ2)));User Id=SBT_REP;Password=S8T_R3P;")]
        public string jiraTestConnectionString {
            get {
                return ((string)(this["jiraTestConnectionString"]));
            }
        }
    }
}
