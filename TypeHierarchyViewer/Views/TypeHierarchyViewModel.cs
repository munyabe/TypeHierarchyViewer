using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace TypeHierarchyViewer.Views
{
    /// <summary>
    /// <see cref="TypeHierarchyView"/>の ViewModel です。
    /// </summary>
    public class TypeHierarchyViewModel : INotifyPropertyChanged
    {
        /// <inheritdoc />
        public event PropertyChangedEventHandler PropertyChanged;

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

                var topNode = CreateTypeNode(value);
                TypeNodes = new[] { topNode }
                    .Concat(value.AllInterfaces
                        .Select(x => new TypeNode(x)))
                    .ToArray();
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
        private static TypeNode CreateTypeNode(INamedTypeSymbol targetType)
        {
            var baseTypes = GetBaseTypes(targetType);
            var result = new TypeNode(baseTypes.Pop());

            var current = result;
            foreach (var type in baseTypes)
            {
                var child = new TypeNode(type);
                current.Children = new[] { child };
                current = child;
            }

            current.Children = new[] { new TypeNode(targetType) };

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
