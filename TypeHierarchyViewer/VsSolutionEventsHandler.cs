using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using TypeHierarchyViewer.Views;

namespace TypeHierarchyViewer
{
    /// <summary>
    /// <see cref="IVsSolutionEvents"/>のイベントを購読するクラスです。
    /// </summary>
    public class VsSolutionEventsHandler : IVsSolutionEvents
    {
        /// <summary>
        /// 拡張機能のパッケージです。
        /// </summary>
        private readonly Package _package;

        /// <summary>
        /// インスタンスを初期化します。
        /// </summary>
        /// <param name="package">拡張機能のパッケージ</param>
        public VsSolutionEventsHandler(Package package)
        {
            _package = package;
        }

        /// <inheritdoc />
        public int OnAfterCloseSolution(object pUnkReserved)
        {
            return VSConstants.S_OK;
        }

        /// <inheritdoc />
        public int OnAfterLoadProject(IVsHierarchy pStubHierarchy, IVsHierarchy pRealHierarchy)
        {
            return VSConstants.S_OK;
        }

        /// <inheritdoc />
        public int OnAfterOpenProject(IVsHierarchy pHierarchy, int fAdded)
        {
            return VSConstants.S_OK;
        }

        /// <inheritdoc />
        public int OnAfterOpenSolution(object pUnkReserved, int fNewSolution)
        {
            return VSConstants.S_OK;
        }

        /// <inheritdoc />
        public int OnBeforeCloseProject(IVsHierarchy pHierarchy, int fRemoved)
        {
            return VSConstants.S_OK;
        }

        /// <inheritdoc />
        public int OnBeforeCloseSolution(object pUnkReserved)
        {
            TypeHierarchyWindow.GetWindow(_package).ViewModel.Clear();
            return VSConstants.S_OK;
        }

        /// <inheritdoc />
        public int OnBeforeUnloadProject(IVsHierarchy pRealHierarchy, IVsHierarchy pStubHierarchy)
        {
            return VSConstants.S_OK;
        }

        /// <inheritdoc />
        public int OnQueryCloseProject(IVsHierarchy pHierarchy, int fRemoving, ref int pfCancel)
        {
            return VSConstants.S_OK;
        }

        /// <inheritdoc />
        public int OnQueryCloseSolution(object pUnkReserved, ref int pfCancel)
        {
            return VSConstants.S_OK;
        }

        /// <inheritdoc />
        public int OnQueryUnloadProject(IVsHierarchy pRealHierarchy, ref int pfCancel)
        {
            return VSConstants.S_OK;
        }
    }
}
