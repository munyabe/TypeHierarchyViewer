using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
        /// 型階層の表示モードを切り替えます。
        /// </summary>
        private void ChangeDisplayMode(object sender, RoutedEventArgs e)
        {
            var isChecked = ((ToggleButton)sender).IsChecked;
            var viewModel = (TypeHierarchyViewModel)DataContext;
            viewModel.DisplayMode = isChecked.HasValue && isChecked.Value ?
                DisplayMode.BaseDetail : DisplayMode.BaseSummaryAndChildren;
        }

        /// <summary>
        /// 子クラスの検索にメタデータを含めるかどうかを切り替えます。
        /// </summary>
        private void ChangeIncludedMetadata(object sender, RoutedEventArgs e)
        {
            var isChecked = ((ToggleButton)sender).IsChecked;
            var viewModel = (TypeHierarchyViewModel)DataContext;
            viewModel.IncludedMetadata = isChecked.HasValue && isChecked.Value;
        }

        /// <summary>
        /// マウスボタンの動作を抑制します。
        /// </summary>
        private void HandledMouseButtonEvent(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
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