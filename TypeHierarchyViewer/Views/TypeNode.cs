using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace TypeHierarchyViewer.Views
{
    /// <summary>
    /// 型階層のノードを表すクラスです。
    /// </summary>
    public class TypeNode
    {
        /// <summary>
        /// 基点となるノードかどうかを取得します。
        /// </summary>
        public bool IsBaseNode { get; }

        /// <summary>
        /// 型の種類を取得します。
        /// </summary>
        public TypeKind Kind { get; }

        /// <summary>
        /// 型の名前を取得します。
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// 型の名前空間を取得します。
        /// </summary>
        public string Namespace { get; }

        /// <summary>
        /// 型の子ノードを取得します。
        /// </summary>
        public IList<TypeNode> Children { get; }

        /// <summary>
        /// 型のシンボルを取得します。
        /// </summary>
        public INamedTypeSymbol Source { get; }

        /// <summary>
        /// インスタンスを初期化します。
        /// </summary>
        /// <param name="source">元となる型</param>
        /// <param name="isBaseNode">基点となるノードの場合は<see langword="true" /></param>
        /// <param name="children">子ノード</param>
        public TypeNode(INamedTypeSymbol source, bool isBaseNode = false, IEnumerable<TypeNode> children = null)
        {
            if (source.IsGenericType)
            {
                var parameters = source.TypeParameters.Select(x => x.Name);
                Name = $"{source.Name}<{string.Join(", ", parameters)}>";
            }
            else
            {
                Name = source.Name;
            }

            Kind = source.TypeKind;
            Namespace = source.ContainingNamespace.ToString();
            Source = source;
            IsBaseNode = isBaseNode;

            Children = children != null ? new List<TypeNode>(children) : new List<TypeNode>();
        }
    }
}
