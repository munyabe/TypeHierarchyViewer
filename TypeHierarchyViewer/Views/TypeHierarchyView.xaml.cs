using System.Windows.Controls;
using System.Windows.Input;

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
        /// 選択した項目の定義を開きます。
        /// </summary>
        private void OpenItemSymbol(object sender, MouseButtonEventArgs e)
        {
            var item = sender as TreeViewItem;
            if (item?.IsSelected ?? false)
            {
                e.Handled = true;

                var viewModel = (TypeHierarchyViewModel)DataContext;
                viewModel.OpenSymbol(item.DataContext as TypeNode);
            }
        }
    }
}