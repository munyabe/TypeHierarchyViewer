using System;
using System.Runtime.InteropServices;
using Microsoft.CodeAnalysis;
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
            Content = new TypeHierarchyView();
        }

        /// <summary>
        /// 階層を表示します。
        /// </summary>
        /// <param name="targetType">表示する型</param>
        public void SetTargetType(INamedTypeSymbol targetType)
        {
            ((TypeHierarchyView)Content).ShowTypeHierarchy(targetType);
        }
    }
}
