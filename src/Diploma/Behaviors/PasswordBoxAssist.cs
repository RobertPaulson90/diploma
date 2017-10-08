using System.Diagnostics.CodeAnalysis;
using System.Windows;
using System.Windows.Controls;

namespace Diploma.Behaviors
{
    [SuppressMessage("ReSharper", "PossibleNullReferenceException", Justification = "Dependency property will never be null.")]
    internal static class PasswordBoxAssist
    {
        public static readonly DependencyProperty PasswordProperty = DependencyProperty.RegisterAttached(
            "Password",
            typeof(string),
            typeof(PasswordBoxAssist),
            new FrameworkPropertyMetadata(string.Empty, OnPasswordPropertyChanged));

        public static readonly DependencyProperty AttachProperty = DependencyProperty.RegisterAttached(
            "OnAttachChanged",
            typeof(bool),
            typeof(PasswordBoxAssist),
            new PropertyMetadata(false, OnAttachChanged));

        public static readonly DependencyProperty IsUpdatingProperty = DependencyProperty.RegisterAttached(
            "IsUpdating",
            typeof(bool),
            typeof(PasswordBoxAssist));

        public static bool GetAttach(DependencyObject d)
        {
            return (bool)d.GetValue(AttachProperty);
        }

        public static string GetPassword(DependencyObject d)
        {
            return (string)d.GetValue(PasswordProperty);
        }

        public static void SetAttach(DependencyObject d, bool value)
        {
            d.SetValue(AttachProperty, value);
        }

        public static void SetPassword(DependencyObject d, string value)
        {
            d.SetValue(PasswordProperty, value);
        }

        private static bool GetIsUpdating(DependencyObject dp)
        {
            return (bool)dp.GetValue(IsUpdatingProperty);
        }

        private static void OnAttachChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var passwordBox = d as PasswordBox;

            if ((bool)e.OldValue)
            {
                passwordBox.PasswordChanged -= PasswordChanged;
            }

            if ((bool)e.NewValue)
            {
                passwordBox.PasswordChanged += PasswordChanged;
            }
        }

        private static void OnPasswordPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var passwordBox = d as PasswordBox;

            passwordBox.PasswordChanged -= PasswordChanged;

            if (!GetIsUpdating(passwordBox))
            {
                passwordBox.Password = (string)e.NewValue;
            }

            passwordBox.PasswordChanged += PasswordChanged;
        }

        private static void PasswordChanged(object sender, RoutedEventArgs e)
        {
            var passwordBox = sender as PasswordBox;
            SetIsUpdating(passwordBox, true);
            SetPassword(passwordBox, passwordBox.Password);
            SetIsUpdating(passwordBox, false);
        }

        private static void SetIsUpdating(DependencyObject d, bool value)
        {
            d.SetValue(IsUpdatingProperty, value);
        }
    }
}
