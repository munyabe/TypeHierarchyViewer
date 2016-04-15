using System.ComponentModel;
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

        private INamedTypeSymbol[] _targetType;
        /// <summary>
        /// 階層を表示する型を取得または設定します。
        /// </summary>
        public INamedTypeSymbol[] TargetType
        {
            get { return _targetType; }
            set
            {
                if (_targetType != value)
                {
                    _targetType = value;
                    OnPropertyChanged(nameof(TargetType));
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
    }
}
