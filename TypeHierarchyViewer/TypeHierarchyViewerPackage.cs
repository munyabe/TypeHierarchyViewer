using System;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.Shell;

namespace TypeHierarchyViewer
{
    /// <summary>
    /// 拡張機能として配置されるパッケージです。
    /// </summary>
    [PackageRegistration(UseManagedResourcesOnly = true)]
    [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)] // Visual Studio のヘルプ/バージョン情報に表示される情報です。
    [Guid(PackageGuidString)]
    [ProvideMenuResource("Menus.ctmenu", 1)]
    public sealed class TypeHierarchyViewerPackage : Package
    {
        /// <summary>
        /// パッケージのIDです。
        /// </summary>
        public const string PackageGuidString = "6d8929af-093e-4b52-a6ea-d0dc93d5a30d";

        /// <summary>
        /// パッケージを初期化します。
        /// </summary>
        public TypeHierarchyViewerPackage()
        {
            base.Initialize();
            OpenTypeHierarchyCommand.Initialize(this);
        }
    }
}
