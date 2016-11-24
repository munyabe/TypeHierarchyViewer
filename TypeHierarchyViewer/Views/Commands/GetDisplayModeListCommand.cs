using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.Shell;

namespace TypeHierarchyViewer.Views.Commands
{
    /// <summary>
    /// 型階層の表示モードの一覧を取得するコマンドです。
    /// </summary>
    internal sealed class GetDisplayModeListCommand : WindowPaneCommandBase<TypeHierarchyWindow>
    {
        /// <summary>
        /// コマンドのIDです。
        /// </summary>
        public const int CommandId = 0x0112;

        /// <summary>
        /// 表示モードの選択肢の一覧です。
        /// </summary>
        private readonly string[] _displayModeList;

        /// <summary>
        /// シングルトンのインスタンスを取得します。
        /// </summary>
        public static GetDisplayModeListCommand Instance { get; private set; }

        /// <summary>
        /// 表示モードの一覧を取得します。
        /// </summary>
        public IDictionary<string, DisplayMode> DisplayModes { get; }

        /// <summary>
        /// インスタンスを初期化します。
        /// </summary>
        /// <param name="package">コマンドを提供するパッケージ</param>
        /// <param name="windowPane">配置するツールウィンドウ</param>
        private GetDisplayModeListCommand(Package package, TypeHierarchyWindow windowPane)
            : base(package, windowPane, CommandId, TypeHierarchyWindow.ToolbarCommandSetId)
        {
            DisplayModes = new Dictionary<string, DisplayMode>
            {
                { "Show the Type Hierarchy", DisplayMode.BaseSummaryAndChildren },
                { "Show the Supertype Hierarchy", DisplayMode.BaseDetail }
            };

            _displayModeList = DisplayModes.Keys.ToArray();
        }

        /// <summary>
        /// このコマンドのシングルトンのインスタンスを初期化します。
        /// </summary>
        /// <param name="package">コマンドを提供するパッケージ</param>
        /// <param name="windowPane">配置するツールウィンドウ</param>
        public static void Initialize(Package package, WindowPane windowPane)
        {
            Instance = new GetDisplayModeListCommand(package, (TypeHierarchyWindow)windowPane);
        }

        /// <inheritdoc />
        protected override void Execute(object sender, EventArgs e)
        {
            var eventArgs = e as OleMenuCmdEventArgs;
            if (eventArgs == null)
            {
                return;
            }

            var outValue = eventArgs.OutValue;
            if (outValue != IntPtr.Zero)
            {
                Marshal.GetNativeVariantForObject(_displayModeList, outValue);
            }
        }
    }
}
