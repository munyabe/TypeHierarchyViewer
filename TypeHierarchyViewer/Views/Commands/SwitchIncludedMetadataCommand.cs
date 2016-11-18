using System;
using Microsoft.VisualStudio.Shell;

namespace TypeHierarchyViewer.Views.Commands
{
    /// <summary>
    /// 子クラスの検索にメタデータを含めるかどうかを切り替えるコマンドです。
    /// </summary>
    internal sealed class SwitchIncludedMetadataCommand : CommandBase
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
        private SwitchIncludedMetadataCommand(Package package) : base(package, CommandId, TypeHierarchyWindow.ToolbarCommandSetId)
        {
        }

        /// <summary>
        /// このコマンドのシングルトンのインスタンスを初期化します。
        /// </summary>
        /// <param name="package">コマンドを提供するパッケージ</param>
        public static void Initialize(Package package)
        {
            Instance = new SwitchIncludedMetadataCommand(package);
        }

        /// <inheritdoc />
        protected override void Execute(object sender, EventArgs e)
        {
            var viewModel = TypeHierarchyWindow.GetWindow(Package).ViewModel;
            viewModel.IncludedMetadata = !viewModel.IncludedMetadata;
        }
    }
}
