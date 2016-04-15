namespace TypeHierarchyViewer.Views
{
    /// <summary>
    /// 型階層のノードを表すクラスです。
    /// </summary>
    public class TypeNode
    {
        /// <summary>
        /// 型の名前を取得または設定します。
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 型の子を取得または設定します。
        /// </summary>
        public TypeNode[] Children { get; set; }
    }
}
