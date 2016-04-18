using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using Microsoft.CodeAnalysis;

namespace TypeHierarchyViewer.Views.Converters
{
    /// <summary>
    /// 型の種類をアイコンのパスに変換するコンバーターです。
    /// </summary>
    public class TypeKindToIconConverter : IValueConverter
    {
        /// <summary>
        /// アイコンのキャッシュです。
        /// </summary>
        private readonly Dictionary<TypeKind, BitmapImage> _icons;

        /// <summary>
        /// シングルトンのインスタンスです。
        /// </summary>
        public static readonly TypeKindToIconConverter Instance = new TypeKindToIconConverter();

        /// <summary>
        /// インスタンスを初期化します。
        /// </summary>
        private TypeKindToIconConverter()
        {
            _icons = new Dictionary<TypeKind, BitmapImage>
            {
                [TypeKind.Class] = CreateImage("Class.png"),
                [TypeKind.Interface] = CreateImage("Interface.png")
            };
        }

        /// <inheritdoc />
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            BitmapImage result;
            return _icons.TryGetValue((TypeKind)value, out result) ? result : _icons[TypeKind.Class];
        }

        /// <inheritdoc />
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// アイコンのイメージを作成します。
        /// </summary>
        private static BitmapImage CreateImage(string fileName)
        {
            var uri = new Uri("pack://application:,,,/TypeHierarchyViewer;component/Resources/" + fileName);
            return new BitmapImage(uri);
        }
    }
}
