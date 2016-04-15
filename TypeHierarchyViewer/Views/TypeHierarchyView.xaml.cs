using System.Windows.Controls;
using Microsoft.CodeAnalysis;

namespace TypeHierarchyViewer.Views
{
    /// <summary>
    /// 型階層を表示する View です。
    /// </summary>
    public partial class TypeHierarchyView : UserControl
    {
        /// <summary>
        /// インスタンスを初期化します。
        /// </summary>
        public TypeHierarchyView()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 階層を表示します。
        /// </summary>
        /// <param name="TargetType">表示する型</param>
        public void ShowTypeHierarchy(INamedTypeSymbol targetType)
        {
            _typeTree.ItemsSource = new[] { targetType };
        }
    }
}