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
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("2016-03-01")]
        public global::System.DateTime dateStart {
            get {
                return ((global::System.DateTime)(this["dateStart"]));
            }
            set {
                this["dateStart"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("2016-03-31")]
        public global::System.DateTime dateFinish {
            get {
                return ((global::System.DateTime)(this["dateFinish"]));
            }
            set {
                this["dateFinish"] = value;
            }
        }
    }
}