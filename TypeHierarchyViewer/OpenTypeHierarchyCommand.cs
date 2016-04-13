using System;
using Microsoft.VisualStudio.Shell;

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
            throw new NotImplementedException();
        }
    }
}
