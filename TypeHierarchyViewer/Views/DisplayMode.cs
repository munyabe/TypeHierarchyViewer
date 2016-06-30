namespace TypeHierarchyViewer.Views
{
    /// <summary>
    /// 型階層の表示モードを表します。
    /// </summary>
    public enum DisplayMode
    {
        /// <summary>
        /// 親クラスとインターフェースのサマリー、子クラスを表示するモードです。
        /// </summary>
        BaseSummaryAndChildren,

        /// <summary>
        /// 親クラスとインターフェースの詳細を表示するモードです。
        /// </summary>
        BaseDetail
    }
}
