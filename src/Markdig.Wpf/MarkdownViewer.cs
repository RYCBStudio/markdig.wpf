// Copyright (c) Nicolas Musset. All rights reserved.
// This file is licensed under the MIT license.
// See the LICENSE.md file in the project root for more information.

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace Markdig.Wpf
{
    /// <summary>
    /// A markdown viewer control.
    /// </summary>
    public class MarkdownViewer : Control
    {
        protected static readonly MarkdownPipeline DefaultPipeline = new MarkdownPipelineBuilder().UseSupportedExtensions().Build();

        private static readonly DependencyPropertyKey DocumentPropertyKey =
            DependencyProperty.RegisterReadOnly(nameof(Document), typeof(FlowDocument), typeof(MarkdownViewer), new FrameworkPropertyMetadata());

        /// <summary>
        /// Defines the <see cref="Document"/> property.
        /// </summary>
        public static readonly DependencyProperty DocumentProperty = DocumentPropertyKey.DependencyProperty;

        /// <summary>
        /// Defines the <see cref="Theme"/> property.
        /// </summary>
        public static readonly DependencyProperty ThemeProperty = 
            DependencyProperty.Register(nameof(Theme), typeof(string), typeof(MarkdownViewer), new PropertyMetadata(ThemeChanged));

        /// <summary>
        /// Defines the <see cref="Markdown"/> property.
        /// </summary>
        public static readonly DependencyProperty MarkdownProperty =
            DependencyProperty.Register(nameof(Markdown), typeof(string), typeof(MarkdownViewer), new FrameworkPropertyMetadata(MarkdownChanged));

        /// <summary>
        /// Defines the <see cref="Pipeline"/> property.
        /// </summary>
        public static readonly DependencyProperty PipelineProperty =
            DependencyProperty.Register(nameof(Pipeline), typeof(MarkdownPipeline), typeof(MarkdownViewer), new FrameworkPropertyMetadata(PipelineChanged));

        static MarkdownViewer()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MarkdownViewer), new FrameworkPropertyMetadata(typeof(MarkdownViewer)));
        }

        public static void SetTheme(string theme, FrameworkElement element)
        {
            switch (theme)
            {
                case "light":
                    element.Resources.MergedDictionaries.Add(new ResourceDictionary() { Source = new Uri($"pack://application:,,,/Markdig.Wpf;component/Themes/Light.xaml") });
                    break;
                case "dark":
                    element.Resources.MergedDictionaries.Add(new ResourceDictionary() { Source = new Uri($"pack://application:,,,/Markdig.Wpf;component/Themes/Dark.xaml") });
                    break;
                default:
                    element.Resources.MergedDictionaries.Add(new ResourceDictionary() { Source = new Uri($"pack://application:,,,/Markdig.Wpf;component/Themes/Dark.xaml") });
                    break;
            }
        }

        /// <summary>
        /// Get the theme to display.
        /// </summary>
        public string? Theme
        {
            get => (string)GetValue(ThemeProperty);
            set => SetValue(ThemeProperty, value);
        }

        /// <summary>
        /// Gets the flow document to display.
        /// </summary>
        public FlowDocument? Document
        {
            get => (FlowDocument)GetValue(DocumentProperty);
            protected set => SetValue(DocumentPropertyKey, value);
        }

        /// <summary>
        /// Gets or sets the markdown to display.
        /// </summary>
        public string? Markdown
        {
            get => (string)GetValue(MarkdownProperty);
            set => SetValue(MarkdownProperty, value);
        }

        /// <summary>
        /// Gets or sets the markdown pipeline to use.
        /// </summary>
        public MarkdownPipeline Pipeline
        {
            get => (MarkdownPipeline)GetValue(PipelineProperty);
            set => SetValue(PipelineProperty, value);
        }

        private static void MarkdownChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var control = (MarkdownViewer)sender;
            control.RefreshDocument();
        }

        private static void PipelineChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var control = (MarkdownViewer)sender;
            control.RefreshDocument();
        }

        private static void ThemeChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var control = (MarkdownViewer)sender;
            SetTheme(control.Theme, control);
        }

        protected virtual void RefreshDocument()
        {
            Document = Markdown != null ? Wpf.Markdown.ToFlowDocument(Markdown, Pipeline ?? DefaultPipeline) : null;
        }
    }
}
