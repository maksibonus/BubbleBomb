﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.18408
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TestEditor.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "10.0.0.0")]
    internal sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase {
        
        private static Settings defaultInstance = ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));
        
        public static Settings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Нове запитання")]
        public string NewQuestionString {
            get {
                return ((string)(this["NewQuestionString"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Ви впевнені, що бажаєте видалити запитання?")]
        public string RemoveQuestionMessage {
            get {
                return ((string)(this["RemoveQuestionMessage"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Створено порожній файл, так як файл із запитаннями відсутній.")]
        public string FileNotFoundMessage {
            get {
                return ((string)(this["FileNotFoundMessage"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Замінити пошкоджений файл порожнім? В разі відмови програма буде зачинена.")]
        public string FileIsCorruptedMessage {
            get {
                return ((string)(this["FileIsCorruptedMessage"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Текст запитання не може бути порожнім.")]
        public string QuestionTextIsEmptyMessage {
            get {
                return ((string)(this["QuestionTextIsEmptyMessage"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Текст відповіді не може бути порожнім.")]
        public string AnswerTextIsEmptyMessage {
            get {
                return ((string)(this["AnswerTextIsEmptyMessage"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Запитання не може не мати жодної відповіді.")]
        public string QuestionHasNoAnswerMessage {
            get {
                return ((string)(this["QuestionHasNoAnswerMessage"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Запитання не може не мати жодної правильної відповіді.")]
        public string QuestionHasNoRightAnswer {
            get {
                return ((string)(this["QuestionHasNoRightAnswer"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Внесені зміни не були збережені. Зберегти внесені зміни?")]
        public string HasChangesMessage {
            get {
                return ((string)(this["HasChangesMessage"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Програма буде зачинена.")]
        public string ApplicationExitMessage {
            get {
                return ((string)(this["ApplicationExitMessage"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Додаткова інформація:")]
        public string AdditionalInfoMessage {
            get {
                return ((string)(this["AdditionalInfoMessage"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("C++1983")]
        public string Password {
            get {
                return ((string)(this["Password"]));
            }
        }
        
        [global::System.Configuration.ApplicationScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Пароль невірний. Закривайте програму і не намагайтеся надурити викладача ;)")]
        public string IncorrectPasswordMessage {
            get {
                return ((string)(this["IncorrectPasswordMessage"]));
            }
        }
    }
}
