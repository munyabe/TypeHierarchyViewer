using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        /// DTE のインスタンスです。
        /// </summary>
        private readonly DTE2 _dte;

        /// <summary>
        /// サポートする言語の一覧です。
        /// </summary>
        private readonly ISet<string> _supportedLanguage = new HashSet<string> { "CSharp", "Basic" };

        /// <summary>
        /// コマンドのIDです。
        /// </summary>
        public const int CommandId = 0x0100;

        /// <summary>
        /// 所属するコマンドセットのIDです。
        /// </summary>
        public static readonly Guid CommandSetId = new Guid("d0792db2-419d-4fe0-9215-49e33d72e0eb");

        /// <summary>
        /// シングルトンのインスタンスを取得します。
        /// </summary>
        public static OpenTypeHierarchyCommand Instance { get; private set; }

        /// <summary>
        /// インスタンスを初期化します。
        /// </summary>
        /// <param name="package">コマンドを提供するパッケージ</param>
        private OpenTypeHierarchyCommand(Package package) : base(package, CommandId, CommandSetId)
        {
            _dte = (DTE2)ServiceProvider.GetService(typeof(DTE));

            var menu = MenuCommand;
            if (menu != null)
            {
                menu.BeforeQueryStatus += SetVisibility;
            }
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
            ShowToolWindowAsync();
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
        private async Task<INamedTypeSymbol> GetSelectedTypeSymbolAsync(Microsoft.CodeAnalysis.Solution solution)
        {
            var activeDoc = _dte.ActiveDocument;
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
            var symbol = await SymbolFinder.FindSymbolAtPositionAsync(doc, position);
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
        /// メニュー項目を表示するかどうかを示す値を設定します。
        /// </summary>
        private void SetVisibility(object sender, EventArgs e)
        {
            var command = sender as OleMenuCommand;
            if (command != null)
            {
                command.Visible = _supportedLanguage.Contains(_dte.ActiveDocument.Language);
            }
        }

        /// <summary>
        /// 型階層を表示するウィンドウを開きます。
        /// </summary>
        private async void ShowToolWindowAsync()
        {
            var workspace = GetWorkspace();
            var symbol = await GetSelectedTypeSymbolAsync(workspace.CurrentSolution);

            var window = TypeHierarchyWindow.GetWindow(Package);
            var windowFrame = (IVsWindowFrame)window.Frame;
            ErrorHandler.ThrowOnFailure(windowFrame.Show());

            window.ViewModel.InitializeTargetType(symbol, workspace);
        }
    }
}
