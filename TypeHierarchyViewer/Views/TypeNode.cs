using Microsoft.CodeAnalysis;

namespace TypeHierarchyViewer.Views
{
    /// <summary>
    /// 型階層のノードを表すクラスです。
    /// </summary>
    public class TypeNode
    {
        /// <summary>
        /// ノードの種類を取得します。
        /// </summary>
        public TypeKind Kind { get; }

        /// <summary>
        /// 型の名前を取得します。
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// 型の子を取得または設定します。
        /// </summary>
        public TypeNode[] Children { get; set; }

        /// <summary>
        /// インスタンスを初期化します。
        /// </summary>
        /// <param name="source">元となる型</param>
        public TypeNode(INamedTypeSymbol source)
        {
            Name = source.Name;
            Kind = source.TypeKind;
        }
    }
}
