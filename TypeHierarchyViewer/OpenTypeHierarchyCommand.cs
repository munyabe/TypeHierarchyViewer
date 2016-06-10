using System;
using System.Linq;
using EnvDTE;
using EnvDTE80;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.FindSymbols;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.ComponentModelHost;
using Microsoft.VisualStudio.LanguageServices;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using TypeHierarchyViewer.Views;

namespace TypeHierarchyViewer
{
    /// <summary>
    /// 型階層を表示する画面を開くコマンドです。
    /// </summary>
    internal sealed class OpenTypeHierarchyCommand : CommandBase
    {
        /// <summary>
        /// コマンドのIDです。
        /// </summary>
        public const int CommandId = 0x0100;

        /// <summary>
        /// コマンドメニューグループのIDです。
        /// </summary>
        public static readonly Guid CommandSet = new Guid("d0792db2-419d-4fe0-9215-49e33d72e0eb");

        /// <summary>
        /// シングルトンのインスタンスを取得します。
        /// </summary>
        public static OpenTypeHierarchyCommand Instance { get; private set; }

        /// <summary>
        /// インスタンスを初期化します。
        /// </summary>
        /// <param name="package">コマンドを提供するパッケージ</param>
        private OpenTypeHierarchyCommand(Package package) : base(package, CommandId, CommandSet)
        {
        }

        /// <summary>
        /// このコマンドのシングルトンのインスタンスを初期化します。
        /// </summary>
        /// <param name="package">コマンドを提供するパッケージ</param>
        public static void Initialize(Package package)
        {
            Instance = new OpenTypeHierarchyCommand(package);
        }

        /// <inheritdoc />
        protected override void Execute(object sender, EventArgs e)
        {
            ShowToolWindow();
        }

        /// <summary>
        /// 指定のシンボルから対象とする型を探します。
        /// </summary>
        private static INamedTypeSymbol FindTypeSymbol(ISymbol symbol)
        {
            var typeSymbol = symbol as INamedTypeSymbol;
            if (typeSymbol != null)
            {
                return typeSymbol;
            }

            var methodSymbol = symbol as IMethodSymbol;
            if (methodSymbol != null && methodSymbol.MethodKind == MethodKind.Constructor)
            {
                return methodSymbol.ContainingType;
            }

            return null;
        }

        /// <summary>
        /// 現在選択している型を取得します。
        /// </summary>
        private INamedTypeSymbol GetSelectedTypeSymbol(Microsoft.CodeAnalysis.Solution solution)
        {
            var dte = (DTE2)ServiceProvider.GetService(typeof(DTE));

            var activeDoc = dte.ActiveDocument;
            if (activeDoc == null)
            {
                return null;
            }

            var docId = solution.GetDocumentIdsWithFilePath(activeDoc.FullName).FirstOrDefault();
            if (docId == null)
            {
                return null;
            }

            var doc = solution.GetDocument(docId);
            var selection = (TextSelection)activeDoc.Selection;
            var position = (selection.ActivePoint.AbsoluteCharOffset - 1) + (selection.CurrentLine - 1);
            var symbol = SymbolFinder.FindSymbolAtPositionAsync(doc, position).Result;
            return FindTypeSymbol(symbol);
        }

        /// <summary>
        /// 現在のワークスペースを取得します。
        /// <returns></returns>
        private VisualStudioWorkspace GetWorkspace()
        {
            var componentModel = (IComponentModel)ServiceProvider.GetService(typeof(SComponentModel));
            return componentModel.GetService<VisualStudioWorkspace>();
        }

        /// <summary>
        /// 型階層を表示するウィンドウを開きます。
        /// </summary>
        private void ShowToolWindow()
        {
            var window = Package.FindToolWindow(typeof(TypeHierarchyWindow), 0, true) as TypeHierarchyWindow;
            if (null == window || null == window.Frame)
            {
                throw new NotSupportedException("Cannot create tool window");
            }

            var windowFrame = (IVsWindowFrame)window.Frame;
            ErrorHandler.ThrowOnFailure(windowFrame.Show());

            var workspace = GetWorkspace();
            window.SetTargetType(GetSelectedTypeSymbol(workspace.CurrentSolution), workspace);
        }
    }
}
