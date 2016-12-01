using System;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using TypeHierarchyViewer.Views;
using TypeHierarchyViewer.Views.Commands;

namespace TypeHierarchyViewer
{
    /// <summary>
    /// 拡張機能として配置されるパッケージです。
    /// </summary>
    [PackageRegistration(UseManagedResourcesOnly = true)]
    [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)] // Visual Studio のヘルプ/バージョン情報に表示される情報です。
    [Guid(PackageGuidString)]
    [ProvideMenuResource("Menus.ctmenu", 1)]
    [ProvideToolWindow(typeof(TypeHierarchyWindow))]
    public sealed class TypeHierarchyViewerPackage : Package
    {
        private uint _solutionEventCoockie;

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
            GetVsSolutionService().AdviseSolutionEvents(new VsSolutionEventsHandler(this), out _solutionEventCoockie);
        }

        /// <inheritdoc />
        protected override WindowPane InstantiateToolWindow(Type toolWindowType)
        {
            var window = base.InstantiateToolWindow(toolWindowType);
            SwitchDisplayModeCommand.Initialize(this, window);
            SwitchIncludedMetadataCommand.Initialize(this, window);
            GetDisplayModeListCommand.Initialize(this, window);

            return window;
        }

        /// <inheritdoc />
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            GetVsSolutionService().UnadviseSolutionEvents(_solutionEventCoockie);
        }

        /// <summary>
        /// <see cref="IVsSolution"/>のインスタンスを取得します。
        /// </summary>
        private static IVsSolution GetVsSolutionService()
        {
            return (IVsSolution)ServiceProvider.GlobalProvider.GetService(typeof(SVsSolution));
        }
    }
}
