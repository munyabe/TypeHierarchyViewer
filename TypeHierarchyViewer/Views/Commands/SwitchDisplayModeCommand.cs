using System;
using System.Linq;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.Shell;

namespace TypeHierarchyViewer.Views.Commands
{
    /// <summary>
    /// 型階層の表示モードを切り替えるコマンドです。
    /// </summary>
    internal sealed class SwitchDisplayModeCommand : WindowPaneCommandBase<TypeHierarchyWindow>
    {
        /// <summary>
        /// コマンドのIDです。
        /// </summary>
        public const int CommandId = 0x0110;

        /// <summary>
        /// 現在の表示モードです。
        /// </summary>
        private DisplayMode _currentMode = DisplayMode.BaseSummaryAndChildren;

        /// <summary>
        /// シングルトンのインスタンスを取得します。
        /// </summary>
        public static SwitchDisplayModeCommand Instance { get; private set; }

        /// <summary>
        /// インスタンスを初期化します。
        /// </summary>
        /// <param name="package">コマンドを提供するパッケージ</param>
        /// <param name="windowPane">配置するツールウィンドウ</param>
        private SwitchDisplayModeCommand(Package package, TypeHierarchyWindow windowPane)
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
            Instance = new SwitchDisplayModeCommand(package, (TypeHierarchyWindow)windowPane);
        }

        /// <inheritdoc />
        protected override void Execute(object sender, EventArgs e)
        {
            var eventArgs = e as OleMenuCmdEventArgs;
            if (eventArgs == null)
            {
                return;
            }

            var newChoice = eventArgs.InValue as string;
            var outValue = eventArgs.OutValue;
            var displayModes = GetDisplayModeListCommand.Instance.DisplayModes;

            if (outValue != IntPtr.Zero)
            {
                var currentValue = displayModes.First(x => x.Value == _currentMode).Key;
                Marshal.GetNativeVariantForObject(currentValue, outValue);
            }
            else if (newChoice != null)
            {
                DisplayMode mode;
                if (displayModes.TryGetValue(newChoice, out mode))
                {
                    SwitchDisplayMode(mode);
                    _currentMode = mode;
                }
                else
                {
                    throw (new InvalidOperationException($"This value {newChoice} is invalid."));
                }
            }
        }

        /// <summary>
        /// 型階層の表示モードを切り替えます。
        /// </summary>
        private void SwitchDisplayMode(DisplayMode mode)
        {
            WindowPane.ViewModel.DisplayMode = mode;
        }
    }
}
