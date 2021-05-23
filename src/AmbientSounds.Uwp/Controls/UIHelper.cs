﻿using AmbientSounds.Animations;
using AmbientSounds.ViewModels;
using Windows.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

namespace AmbientSounds.Controls
{
    public static class UIHelper
    {
        public static void RemoveOnMiddleClick(object sender, PointerRoutedEventArgs e)
        {
            if (sender is Image fe && fe.DataContext is ActiveTrackViewModel vm)
            {
                var pointer = e.GetCurrentPoint(fe);
                if (pointer.Properties.PointerUpdateKind == PointerUpdateKind.MiddleButtonPressed)
                {
                    vm.RemoveCommand.Execute(vm.Sound);
                    e.Handled = true;
                }
            }
        }

        public static void GridScaleUp(object sender, PointerRoutedEventArgs e)
            => SoundItemAnimations.ItemScaleUp((UIElement)sender, 1.1f, e.Pointer);

        public static void GridScaleNormal(object sender, PointerRoutedEventArgs e)
            => SoundItemAnimations.ItemScaleNormal((UIElement)sender);

        public static T FindControl<T>(this UIElement parent, string ControlName) where T : FrameworkElement
        {
            // Source: https://stackoverflow.com/a/58091583/10953422

            if (parent == null)
                return null;

            if (parent.GetType() == typeof(T) && ((T)parent).Name == ControlName)
            {
                return (T)parent;
            }
            T result = null;
            int count = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < count; i++)
            {
                UIElement child = (UIElement)VisualTreeHelper.GetChild(parent, i);

                if (FindControl<T>(child, ControlName) != null)
                {
                    result = FindControl<T>(child, ControlName);
                    break;
                }
            }
            return result;
        }
    }
}
