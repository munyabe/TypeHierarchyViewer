using System;
using Microsoft.VisualStudio.Shell;

namespace TypeHierarchyViewer.Views.Commands
{
    /// <summary>
    /// 型階層の表示モードを切り替えるコマンドです。
    /// </summary>
    internal sealed class SwitchDisplayModeCommand : CommandBase
    {
        /// <summary>
        /// コマンドのIDです。
        /// </summary>
        public const int CommandId = 0x0110;

        /// <summary>
        /// シングルトンのインスタンスを取得します。
        /// </summary>
        public static SwitchDisplayModeCommand Instance { get; private set; }

        /// <summary>
        /// インスタンスを初期化します。
        /// </summary>
        /// <param name="package">コマンドを提供するパッケージ</param>
        private SwitchDisplayModeCommand(Package package) : base(package, CommandId, TypeHierarchyWindow.ToolbarCommandSetId)
        {
        }

        /// <summary>
        /// このコマンドのシングルトンのインスタンスを初期化します。
        /// </summary>
        /// <param name="package">コマンドを提供するパッケージ</param>
        public static void Initialize(Package package)
        {
            Instance = new SwitchDisplayModeCommand(package);
        }

        /// <inheritdoc />
        protected override void Execute(object sender, EventArgs e)
        {
            var viewModel = TypeHierarchyWindow.GetWindow(Package).ViewModel;
            viewModel.DisplayMode = viewModel.DisplayMode == DisplayMode.BaseDetail ?
                DisplayMode.BaseSummaryAndChildren : DisplayMode.BaseDetail;
        }
    }
}
