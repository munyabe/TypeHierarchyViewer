using System;
using Microsoft.VisualStudio.Shell;

namespace TypeHierarchyViewer.Views.Commands
{
    /// <summary>
    /// 子クラスの検索にメタデータを含めるかどうかを切り替えるコマンドです。
    /// </summary>
    internal sealed class SwitchIncludedMetadataCommand : WindowPaneCommandBase<TypeHierarchyWindow>
    {
        /// <summary>
        /// コマンドのIDです。
        /// </summary>
        public const int CommandId = 0x0111;

        /// <summary>
        /// シングルトンのインスタンスを取得します。
        /// </summary>
        public static SwitchIncludedMetadataCommand Instance { get; private set; }

        /// <summary>
        /// インスタンスを初期化します。
        /// </summary>
        /// <param name="package">コマンドを提供するパッケージ</param>
        /// <param name="windowPane">配置するツールウィンドウ</param>
        private SwitchIncludedMetadataCommand(Package package, TypeHierarchyWindow windowPane)
            : base(package, windowPane, CommandId, TypeHierarchyWindow.ToolbarCommandSetId)
        {
        }

        /// <summary>
        /// このコマンドのシングルトンのインスタンスを初期化します。
        /// </summary>
        /// <param name="package">コマンドを提供するパッケージ</param>
        /// <param name="windowPane">配置するツールウィンドウ</param>
        public static void Initialize(Package package, WindowPane windowPane)
        {
            var window = (TypeHierarchyWindow)windowPane;
            Instance = new SwitchIncludedMetadataCommand(package, window);

            var viewModel = window.ViewModel;
            viewModel.PropertyChanged += (sender, e) =>
            {
                Instance.MenuCommand.Enabled = viewModel.DisplayMode == DisplayMode.BaseSummaryAndChildren;
            };
        }

        /// <inheritdoc />
        protected override void Execute(object sender, EventArgs e)
        {
            var command = sender as OleMenuCommand;
            if (command == null)
            {
                return;
            }

            command.Checked = !command.Checked;

            var viewModel = WindowPane.ViewModel;
            viewModel.IncludedMetadata = !viewModel.IncludedMetadata;
        }
    }
}
