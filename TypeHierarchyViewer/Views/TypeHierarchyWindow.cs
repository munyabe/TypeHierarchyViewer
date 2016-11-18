using System;
using System.ComponentModel.Design;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.Shell;

namespace TypeHierarchyViewer.Views
{
    /// <summary>
    /// 型階層を表示するウィンドウをホストするクラスです。
    /// </summary>
    [Guid("d3721598-933c-40e8-abc6-39470fb141a3")]
    public class TypeHierarchyWindow : ToolWindowPane
    {
        /// <summary>
        /// ツールバーのメニューIDです。
        /// </summary>
        public const int ToolbarMenuId = 0x1000;

        /// <summary>
        /// ツールバーのコマンドセットIDです。
        /// </summary>
        public static readonly Guid ToolbarCommandSetId = new Guid("0ce5d1ff-906d-4e42-9433-e23abe8af06e");

        /// <summary>
        /// データを格納するViewModelを取得します。
        /// </summary>
        public TypeHierarchyViewModel ViewModel
        {
            get
            {
                var view = ((TypeHierarchyView)Content);
                return (TypeHierarchyViewModel)view.DataContext;
            }
        }

        /// <summary>
        /// インスタンスを初期化します。
        /// </summary>
        public TypeHierarchyWindow() : base(null)
        {
            Caption = "Type Hierarchy";
            Content = new TypeHierarchyView
            {
                DataContext = new TypeHierarchyViewModel()
            };

            ToolBar = new CommandID(ToolbarCommandSetId, ToolbarMenuId);
        }

        /// <summary>
        /// <see cref="TypeHierarchyWindow"/>のインスタンスを取得します。
        /// </summary>
        /// <param name="package">拡張機能のパッケージ</param>
        /// <returns>ウィンドウのインスタンス</returns>
        public static TypeHierarchyWindow GetWindow(Package package)
        {
            var window = package.FindToolWindow(typeof(TypeHierarchyWindow), 0, true) as TypeHierarchyWindow;
            if (window == null || window.Frame == null)
            {
                throw new NotSupportedException("Cannot create tool window");
            }

            return window;
        }
    }
}
