﻿using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.WinForms;
using System;
using System.Diagnostics;
using System.Windows;

namespace SIUI.Behaviors
{
    /// <summary>
    /// Attaches additional behavior for WebView2 control.
    /// </summary>
    internal static class WebView2Behavior
    {
        public static bool GetIsAttached(DependencyObject obj) => (bool)obj.GetValue(IsAttachedProperty);

        public static void SetIsAttached(DependencyObject obj, bool value) => obj.SetValue(IsAttachedProperty, value);

        public static readonly DependencyProperty IsAttachedProperty =
            DependencyProperty.RegisterAttached("IsAttached", typeof(bool), typeof(WebView2), new PropertyMetadata(false, OnIsAttachedChanged));

        public static void OnIsAttachedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(bool)e.NewValue)
            {
                return;
            }

            var webView2 = (Microsoft.Web.WebView2.Wpf.WebView2)d;

            UpdateWebView2Environment(webView2);
        }

        private static async void UpdateWebView2Environment(Microsoft.Web.WebView2.Wpf.WebView2 webView2)
        {
            try
            {
                var options = new CoreWebView2EnvironmentOptions("--autoplay-policy=no-user-gesture-required");
                var environment = await CoreWebView2Environment.CreateAsync(null, null, options);

                await webView2.EnsureCoreWebView2Async(environment);
            }
            catch (Exception exc)
            {
                Trace.TraceError(exc.ToString());
            }
        }
    }
}