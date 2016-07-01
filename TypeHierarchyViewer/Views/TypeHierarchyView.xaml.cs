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
            var button = (ToggleButton)sender;
            var viewModel = (TypeHierarchyViewModel)DataContext;
            viewModel.DisplayMode = button.IsChecked.HasValue && button.IsChecked.Value ?
                DisplayMode.BaseDetail : DisplayMode.BaseSummaryAndChildren;
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