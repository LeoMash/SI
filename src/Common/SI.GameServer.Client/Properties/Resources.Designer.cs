﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.42000
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SI.GameServer.Client.Properties {
    using System;
    
    
    /// <summary>
    ///   Класс ресурса со строгой типизацией для поиска локализованных строк и т.д.
    /// </summary>
    // Этот класс создан автоматически классом StronglyTypedResourceBuilder
    // с помощью такого средства, как ResGen или Visual Studio.
    // Чтобы добавить или удалить член, измените файл .ResX и снова запустите ResGen
    // с параметром /str или перестройте свой проект VS.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Возвращает кэшированный экземпляр ResourceManager, использованный этим классом.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("SI.GameServer.Client.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Перезаписывает свойство CurrentUICulture текущего потока для всех
        ///   обращений к ресурсу с помощью этого класса ресурса со строгой типизацией.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Слишком большой файл!.
        /// </summary>
        internal static string FileTooLarge {
            get {
                return ResourceManager.GetString("FileTooLarge", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на право на ошибку.
        /// </summary>
        internal static string GameRule_IgnoreWrong {
            get {
                return ResourceManager.GetString("GameRule_IgnoreWrong", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на без фальстартов.
        /// </summary>
        internal static string GameRule_NoFalseStart {
            get {
                return ResourceManager.GetString("GameRule_NoFalseStart", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на устная.
        /// </summary>
        internal static string GameRule_Oral {
            get {
                return ResourceManager.GetString("GameRule_Oral", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на спортивная.
        /// </summary>
        internal static string GameRule_Sport {
            get {
                return ResourceManager.GetString("GameRule_Sport", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Создана.
        /// </summary>
        internal static string GameStage_Created {
            get {
                return ResourceManager.GetString("GameStage_Created", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Финал.
        /// </summary>
        internal static string GameStage_Final {
            get {
                return ResourceManager.GetString("GameStage_Final", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Игра окончена.
        /// </summary>
        internal static string GameStage_Finished {
            get {
                return ResourceManager.GetString("GameStage_Finished", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Раунд.
        /// </summary>
        internal static string GameStage_Round {
            get {
                return ResourceManager.GetString("GameStage_Round", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Начата.
        /// </summary>
        internal static string GameStage_Started {
            get {
                return ResourceManager.GetString("GameStage_Started", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на На сервере запрещено использование такого логина!.
        /// </summary>
        internal static string LoginForbidden {
            get {
                return ResourceManager.GetString("LoginForbidden", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Пользователь с таким именем уже вошёл на сервер!.
        /// </summary>
        internal static string OnlineUserConflict {
            get {
                return ResourceManager.GetString("OnlineUserConflict", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Истекло время ожидания отправки пакета. Попробуйте ещё раз или уменьшите размер файла..
        /// </summary>
        internal static string UploadPackageTimeout {
            get {
                return ResourceManager.GetString("UploadPackageTimeout", resourceCulture);
            }
        }
    }
}