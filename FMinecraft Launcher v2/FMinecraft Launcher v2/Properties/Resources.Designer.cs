﻿//------------------------------------------------------------------------------
// <auto-generated>
//     這段程式碼是由工具產生的。
//     執行階段版本:4.0.30319.42000
//
//     對這個檔案所做的變更可能會造成錯誤的行為，而且如果重新產生程式碼，
//     變更將會遺失。
// </auto-generated>
//------------------------------------------------------------------------------

namespace FMinecraft_Launcher_v2.Properties {
    using System;
    
    
    /// <summary>
    ///   用於查詢當地語系化字串等的強類型資源類別。
    /// </summary>
    // 這個類別是自動產生的，是利用 StronglyTypedResourceBuilder
    // 類別透過 ResGen 或 Visual Studio 這類工具。
    // 若要加入或移除成員，請編輯您的 .ResX 檔，然後重新執行 ResGen
    // (利用 /str 選項)，或重建您的 VS 專案。
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   傳回這個類別使用的快取的 ResourceManager 執行個體。
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("FMinecraft_Launcher_v2.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   覆寫目前執行緒的 CurrentUICulture 屬性，對象是所有
        ///   使用這個強類型資源類別的資源查閱。
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
        ///   查詢類似 https://github.com/Nickyangtpe/FMinecraft-Launcher-v2/raw/refs/heads/main/Resources/Banner%20Area.zip 的當地語系化字串。
        /// </summary>
        internal static string Covers_Url_zip {
            get {
                return ResourceManager.GetString("Covers_Url_zip", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查詢類似 {&quot;manifest&quot;:{&quot;gamecore&quot;:{&quot;java-runtime-alpha&quot;:[],&quot;java-runtime-beta&quot;:[],&quot;java-runtime-delta&quot;:[],&quot;java-runtime-gamma&quot;:[],&quot;java-runtime-gamma-snapshot&quot;:[],&quot;jre-legacy&quot;:[],&quot;minecraft-java-exe&quot;:[]},&quot;linux&quot;:{&quot;java-runtime-alpha&quot;:[{&quot;availability&quot;:{&quot;group&quot;:5851,&quot;progress&quot;:100},&quot;manifest&quot;:{&quot;sha1&quot;:&quot;3bfc5fdcc28d8897aa12f372ea98a9afeb11a813&quot;,&quot;size&quot;:82665,&quot;url&quot;:&quot;https://piston-meta.mojang.com/v1/packages/3bfc5fdcc28d8897aa12f372ea98a9afeb11a813/manifest.json&quot;},&quot;version&quot;:{&quot;name&quot;:&quot;16.0.1.9.1&quot;,&quot;released&quot;:&quot;2021-05-10T16:43 [字串的其餘部分已遭截斷]&quot;; 的當地語系化字串。
        /// </summary>
        internal static string jre_manifest {
            get {
                return ResourceManager.GetString("jre_manifest", resourceCulture);
            }
        }
    }
}
