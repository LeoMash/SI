﻿using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace SIGame.Converters;

public sealed class MainBackgroundConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        try
        {
            var image = new BitmapImage();
            image.BeginInit();

            image.CreateOptions = BitmapCreateOptions.IgnoreColorProfile;
            image.CacheOption = BitmapCacheOption.OnLoad;

            if (value is string uriString && Uri.TryCreate(uriString, UriKind.Absolute, out Uri uri))
                image.UriSource = uri;
            else
                image.StreamSource = Application.GetResourceStream(new Uri("/SIGame;component/Theme/main_background.jpg", UriKind.Relative)).Stream;

            image.EndInit();

            return image;
        }
        catch (Exception)
        {
            return DependencyProperty.UnsetValue;
        }
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
        throw new NotImplementedException();
}
