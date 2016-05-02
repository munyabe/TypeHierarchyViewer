using System;
using System.Globalization;
using System.Windows.Data;
using Microsoft.CodeAnalysis;

namespace TypeHierarchyViewer.Views.Converters
{
    /// <summary>
    /// 型のシンボルを型名に変換するコンバーターです。
    /// </summary>
    public class TypeNameConverter : IValueConverter
    {
        /// <summary>
        /// シングルトンのインスタンスです。
        /// </summary>
        public static readonly TypeNameConverter Instance = new TypeNameConverter();

        /// <summary>
        /// インスタンスを初期化します。
        /// </summary>
        private TypeNameConverter()
        {
        }

        /// <inheritdoc />
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var symbol = value as INamedTypeSymbol;
            return symbol != null ? symbol.ToString() : string.Empty;
        }

        /// <inheritdoc />
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
