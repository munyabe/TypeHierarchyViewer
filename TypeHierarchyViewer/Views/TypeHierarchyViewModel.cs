using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.FindSymbols;
using Microsoft.VisualStudio.LanguageServices;

namespace TypeHierarchyViewer.Views
{
    /// <summary>
    /// <see cref="TypeHierarchyView"/>の ViewModel です。
    /// </summary>
    public class TypeHierarchyViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// 現在のワークスペースです。
        /// </summary>
        private VisualStudioWorkspace _workspace;

        /// <inheritdoc />
        public event PropertyChangedEventHandler PropertyChanged;

        private DisplayMode _displayMode;
        /// <summary>
        /// 型階層の表示モードを取得または設定します。
        /// </summary>
        public DisplayMode DisplayMode
        {
            get { return _displayMode; }
            set
            {
                if (_displayMode != value)
                {
                    _displayMode = value;
                    TypeNodes = CreateTypeNodes(TargetType);
                }
            }
        }

        private bool _includedMetadata;
        /// <summary>
        /// 子クラスの検索にメタデータを含めるかどうかを取得または設定します。
        /// </summary>
        public bool IncludedMetadata
        {
            get { return _includedMetadata; }
            set
            {
                if (_includedMetadata != value)
                {
                    _includedMetadata = value;
                    TypeNodes = CreateTypeNodes(TargetType);
                }
            }
        }

        private INamedTypeSymbol _targetType;
        /// <summary>
        /// 型階層のターゲットを取得します。
        /// </summary>
        public INamedTypeSymbol TargetType
        {
            get { return _targetType; }
            private set
            {
                if (_targetType != value)
                {
                    _targetType = value;
                    OnPropertyChanged(nameof(TargetType));
                }
            }
        }

        private TypeNode[] _typeNodes;
        /// <summary>
        /// 型階層のノードを取得します。
        /// </summary>
        public TypeNode[] TypeNodes
        {
            get { return _typeNodes; }
            private set
            {
                if (_typeNodes != value)
                {
                    _typeNodes = value;
                    OnPropertyChanged(nameof(TypeNodes));
                }
            }
        }

        /// <summary>
        /// 型階層をクリアします。
        /// </summary>
        public void Clear()
        {
            TargetType = null;
            TypeNodes = new TypeNode[0];
        }

        /// <summary>
        /// 型階層を初期化します。
        /// </summary>
        /// <param name="targetType">対象の型</param>
        /// <param name="workspace">現在のワークスペース</param>
        public void InitializeTargetType(INamedTypeSymbol targetType, VisualStudioWorkspace workspace)
        {
            _workspace = workspace;
            TargetType = targetType;
            TypeNodes = CreateTypeNodes(targetType);
        }

        /// <summary>
        /// 指定されたノードの定義を開きます。
        /// </summary>
        public void OpenSymbol(TypeNode node)
        {
            if (node == null)
            {
                return;
            }

            var currentProject = _workspace.CurrentSolution.GetProject(TargetType.ContainingAssembly);
            var candidateProjects = currentProject != null ?
                GetCandidateProjects(currentProject) : _workspace.CurrentSolution.Projects;

            foreach (var project in candidateProjects)
            {
                if (_workspace.TryGoToDefinition(node.Source, project, CancellationToken.None))
                {
                    break;
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
        /// 指定のノードが実装するインターフェースを子ノードに追加します。
        /// </summary>
        private static void AddInterfaceNodes(TypeNode node)
        {
            var children = node.Source.Interfaces.Select(x => new TypeNode(x));
            foreach (var child in children)
            {
                AddInterfaceNodes(child);
                node.Children.Add(child);
            }
        }

        /// <summary>
        /// 親クラスとインターフェースの詳細の型階層を表すノードを作成します。
        /// </summary>
        private static TypeNode[] CreateBaseDetailNodes(INamedTypeSymbol targetType)
        {
            var topNode = new TypeNode(targetType, true);

            if (targetType.TypeKind == TypeKind.Interface)
            {
                AddInterfaceNodes(topNode);
            }
            else
            {
                var current = topNode;
                foreach (var baseType in GetBaseTypes(targetType))
                {
                    var child = new TypeNode(baseType);
                    current.Children.Add(child);
                    AddInterfaceNodes(current);

                    current = child;
                }
            }

            return new[] { topNode };
        }

        /// <summary>
        /// 親クラスとインターフェースのサマリー、子クラスの型階層を表すノードを作成します。
        /// </summary>
        private TypeNode[] CreateBaseSummaryAndChildrenNodes(INamedTypeSymbol targetType)
        {
            if (targetType.TypeKind == TypeKind.Interface)
            {
                var children = SymbolFinder.FindImplementationsAsync(targetType, _workspace.CurrentSolution).Result
                    .OfType<INamedTypeSymbol>()
                    .Where(CreateSubtypeFilter())
                    .Select(x => new TypeNode(x));

                var topNode = new TypeNode(targetType, true, children);
                return new[] { topNode };
            }
            else if (targetType.SpecialType == SpecialType.System_Object)
            {
                return new[] { new TypeNode(targetType, true) };
            }
            else
            {
                var leafNode = CreateDerivedTypeNodes(targetType);
                var topNode = CreateBaseTypeNodes(targetType, leafNode);

                return new[] { topNode }
                    .Concat(targetType.AllInterfaces
                        .Select(x => new TypeNode(x)))
                    .ToArray();
            }
        }

        /// <summary>
        /// 親クラスの型階層を表すノードを作成します。
        /// </summary>
        private static TypeNode CreateBaseTypeNodes(INamedTypeSymbol targetType, TypeNode leafNode)
        {
            var baseTypes = GetBaseTypesByDesc(targetType);
            if (baseTypes.Count == 0)
            {
                return new TypeNode(targetType);
            }

            var result = new TypeNode(baseTypes.Pop());

            var current = result;
            foreach (var type in baseTypes)
            {
                var child = new TypeNode(type);
                current.Children.Add(child);
                current = child;
            }

            if (leafNode != null)
            {
                current.Children.Add(leafNode);
            }

            return result;
        }

        /// <summary>
        /// 子クラスを含んだノードを作成します。
        /// </summary>
        private TypeNode CreateDerivedTypeNodes(INamedTypeSymbol targetType)
        {
            var children = SymbolFinder.FindDerivedClassesAsync(targetType, _workspace.CurrentSolution).Result
                .Where(CreateSubtypeFilter())
                .Select(x => new TypeNode(x));

            return new TypeNode(targetType, true, children);
        }

        /// <summary>
        /// 子クラスをフィルタリングする処理を取得します。
        /// </summary>
        private Func<INamedTypeSymbol, bool> CreateSubtypeFilter()
        {
            if (IncludedMetadata)
            {
                return x => true;
            }
            else
            {
                return x => x.Locations.Any(y => y.IsInSource);
            }
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

            if (DisplayMode == DisplayMode.BaseSummaryAndChildren)
            {
                return CreateBaseSummaryAndChildrenNodes(targetType);
            }
            else if (DisplayMode == DisplayMode.BaseDetail)
            {
                return CreateBaseDetailNodes(targetType);
            }
            else
            {
                return new TypeNode[0];
            }
        }

        /// <summary>
        /// 親クラスの一覧を取得します。
        /// </summary>
        private static IEnumerable<INamedTypeSymbol> GetBaseTypes(INamedTypeSymbol type)
        {
            var current = type.BaseType;
            while (current != null)
            {
                yield return current;
                current = current.BaseType;
            }
        }

        /// <summary>
        /// 親クラスの一覧を最上位から順に取得します。
        /// </summary>
        private static Stack<INamedTypeSymbol> GetBaseTypesByDesc(INamedTypeSymbol type)
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

        /// <summary>
        /// 定義の探索候補となるプロジェクト一覧を取得します。
        /// </summary>
        private IEnumerable<Project> GetCandidateProjects(Project currentProject)
        {
            yield return currentProject;

            var solution = _workspace.CurrentSolution;
            var projectIds = solution.GetProjectDependencyGraph()
                .GetProjectsThatThisProjectDirectlyDependsOn(currentProject.Id)
                .Select(solution.GetProject);

            foreach (var project in projectIds)
            {
                yield return project;
            }
        }
    }
}
