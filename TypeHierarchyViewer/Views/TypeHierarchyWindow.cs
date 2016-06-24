using System;
using System.Runtime.InteropServices;
using Microsoft.CodeAnalysis;
using Microsoft.VisualStudio.LanguageServices;
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
            Content = new TypeHierarchyView
            {
                DataContext = new TypeHierarchyViewModel()
            };
        }

        /// <summary>
        /// 階層を表示します。
        /// </summary>
        /// <param name="targetType">対象の型</param>
        /// <param name="workspace">現在のワークスペース</param>
        public void SetTargetType(INamedTypeSymbol targetType, VisualStudioWorkspace workspace)
        {
            var view = ((TypeHierarchyView)Content);
            var viewModel = (TypeHierarchyViewModel)view.DataContext;
            viewModel.InitializeTargetType(targetType, workspace);
        }
    }
}
