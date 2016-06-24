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
        /// マウスボタンの動作を抑制します。
        /// </summary>
        private void HandledMouseButtonEvent(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
        }
    }
}