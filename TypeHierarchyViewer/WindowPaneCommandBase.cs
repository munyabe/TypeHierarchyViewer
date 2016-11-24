using System;
using Microsoft.VisualStudio.Shell;

namespace TypeHierarchyViewer
{
    /// <summary>
    /// ツールウィンドウに配置するコマンドの基底クラスです。
    /// </summary>
    /// <typeparam name="TWindowPane">配置するツールウィンドウの型</typeparam>
    internal abstract class WindowPaneCommandBase<TWindowPane> : CommandBase where TWindowPane : WindowPane
    {
        /// <summary>
        /// 配置するツールウィンドウを取得します。
        /// </summary>
        protected TWindowPane WindowPane { get; }

        /// <summary>
        /// インスタンスを初期化します。
        /// </summary>
        /// <param name="package">コマンドを提供するパッケージ</param>
        /// <param name="windowPane">配置するツールウィンドウ</param>
        /// <param name="commandId">コマンドのID</param>
        /// <param name="commandSetId">コマンドメニューグループのID</param>
        public WindowPaneCommandBase(Package package, TWindowPane windowPane, int commandId, Guid commandSetId) : base(package, commandId, commandSetId)
        {
            WindowPane = windowPane;
        }
    }
}
