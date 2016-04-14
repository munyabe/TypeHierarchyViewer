using System;
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
        /// インスタンスを初期化します。
        /// </summary>
        public TypeHierarchyWindow() : base(null)
        {
            Caption = "Type Hierarchy";
            Content = new TypeHierarchyWindowControl();
        }
    }
}
