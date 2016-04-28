using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.FindSymbols;

namespace TypeHierarchyViewer.Views
{
    /// <summary>
    /// <see cref="TypeHierarchyView"/>の ViewModel です。
    /// </summary>
    public class TypeHierarchyViewModel : INotifyPropertyChanged
    {
        /// <inheritdoc />
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// 現在のソリューションを取得または設定します。
        /// </summary>
        public Solution CurrentSolution { get; set; }

        private INamedTypeSymbol _targetType;
        /// <summary>
        /// 階層を表示する型を取得または設定します。
        /// </summary>
        public INamedTypeSymbol TargetType
        {
            get { return _targetType; }
            set
            {
                _targetType = value;
                TypeNodes = CreateTypeNodes(value);
            }
        }

        private TypeNode[] _typeNodes;
        /// <summary>
        /// 型階層のノードを取得または設定します。
        /// </summary>
        public TypeNode[] TypeNodes
        {
            get { return _typeNodes; }
            set
            {
                if (_typeNodes != value)
                {
                    _typeNodes = value;
                    OnPropertyChanged(nameof(TypeNodes));
                }
            }
        }

        /// <summary>
        /// <see cref="PropertyChangedEventHandler"/>イベントを発生させます。
        /// </summary>
        /// <param name="propertyName">変更されたプロパティ名</param>
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// 型階層のノードを作成します。
        /// </summary>
        private TypeNode[] CreateTypeNodes(INamedTypeSymbol targetType)
        {
            if (targetType == null)
            {
                return new TypeNode[0];
            }

            if (targetType.TypeKind == TypeKind.Interface)
            {
                var node = new TypeNode(targetType);
                node.Children = SymbolFinder.FindImplementationsAsync(targetType, CurrentSolution).Result
                    .OfType<INamedTypeSymbol>()
                    .Where(x => x.Locations.Any(y => y.IsInSource))
                    .Select(x => new TypeNode(x))
                    .ToArray();

                return new[] { node };
            }

            var baseTypes = GetBaseTypes(targetType);
            var topNode = CreateTopNode(targetType, baseTypes);
            return new[] { topNode }
                .Concat(targetType.AllInterfaces
                    .Select(x => new TypeNode(x)))
                .ToArray();
        }

        /// <summary>
        /// 型階層の最上位ノードを作成します。
        /// </summary>
        private TypeNode CreateTopNode(INamedTypeSymbol targetType, Stack<INamedTypeSymbol> baseTypes)
        {
            if (baseTypes.Count == 0)
            {
                // MEMO : object の場合
                return new TypeNode(targetType);
            }

            var result = new TypeNode(baseTypes.Pop());

            var current = result;
            foreach (var type in baseTypes)
            {
                var child = new TypeNode(type);
                current.Children = new[] { child };
                current = child;
            }

            var leafNode = new TypeNode(targetType);
            leafNode.Children = SymbolFinder.FindDerivedClassesAsync(targetType, CurrentSolution).Result
                .Where(x => x.Locations.Any(y => y.IsInSource))
                .Select(x => new TypeNode(x))
                .ToArray();

            current.Children = new[] { leafNode };

            return result;
        }

        /// <summary>
        /// 親クラスの一覧を最上位から順に取得します。
        /// </summary>
        private static Stack<INamedTypeSymbol> GetBaseTypes(INamedTypeSymbol type)
        {
            var result = new Stack<INamedTypeSymbol>();

            var current = type.BaseType;
            while (current != null)
            {
                result.Push(current);
                current = current.BaseType;
            }

            return result;
        }
    }
}
