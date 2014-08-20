﻿using Catrobat.Paint.Phone;
using Catrobat.Paint.Phone.Data;
using Catrobat.Paint.WindowsPhone.View;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;

// Die Elementvorlage "Leere Seite" ist unter http://go.microsoft.com/fwlink/?LinkID=390556 dokumentiert.

namespace Catrobat.Paint.WindowsPhone.Controls.UserControls
{
    /// <summary>
    /// Eine leere Seite, die eigenständig verwendet werden kann oder auf die innerhalb eines Rahmens navigiert werden kann.
    /// </summary>
    public sealed partial class ucRecEll
    {
        private int _last_valid_height;
        private int _last_valid_width;
        public ucRecEll()
        {
            this.InitializeComponent();

            PocketPaintApplication.GetInstance().CurrentShape = rectTransDisplayForeground;
            rectTransDisplayForeground.Visibility = Visibility.Visible;
            ellDisplayForeground.Visibility = Visibility.Collapsed;

            tbStrokeThicknessValue.Text = PocketPaintApplication.GetInstance().PaintData.BorderThicknessRecEll.ToString();
            sldStrokeThickness.Value = PocketPaintApplication.GetInstance().PaintData.BorderThicknessRecEll;

            _last_valid_height = Convert.ToInt32(tbHeightValue.Text);
            _last_valid_width = Convert.ToInt32(tbWidthValue.Text);
            rectTransDisplayForeground.ManipulationMode = ManipulationModes.All;

            PocketPaintApplication.GetInstance().PaintData.BorderColorChanged += ColorStrokeChanged;
            PocketPaintApplication.GetInstance().PaintData.FillColorChanged += ColorFillChanged;
            PocketPaintApplication.GetInstance().BarRecEllShape = this;
        }

        private void btnSelectedColor_Click(object sender, RoutedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;

            if(rootFrame != null)
            {
                rootFrame.Navigate(typeof(ViewBorderColorPicker));
            }
        }

        private void btnSelectedFillColor_Click(object sender, RoutedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;

            if (rootFrame != null)
            {
                rootFrame.Navigate(typeof(ViewFillColorPicker));
            }
        }

        private void ColorFillChanged(SolidColorBrush color)
        {
            SolidColorBrush selected_color = new SolidColorBrush();
            selected_color.Color = color.Color != Colors.Transparent ? color.Color : Colors.Transparent;
            btnSelectedFillColor.Background = selected_color;
            PocketPaintApplication.GetInstance().CurrentShape.Fill = selected_color;
        }

        private void ColorStrokeChanged(SolidColorBrush color)
        {
            SolidColorBrush selected_color = new SolidColorBrush();
            selected_color.Color = color.Color != Colors.Transparent ? color.Color : Colors.Transparent;
            btnSelectedBorderColor.Background = selected_color;
            PocketPaintApplication.GetInstance().CurrentShape.Stroke = selected_color;
        }

        public Ellipse EllipseForeground
        {
            get 
            {
                ellDisplayForeground.Visibility = Visibility.Visible;
                rectTransDisplayForeground.Visibility = Visibility.Collapsed;
                return ellDisplayForeground; 
            }
        }

        private void sldStrokeThickness_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
           
        }

        private void sldSliderThickness_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
           //  sldSliderThickness.Background.ToString();
           //  sldSliderThickness.Value.ToString(); 
        }

        public int getHeight()
        {
            int return_value = 0;
            return return_value = tbHeightValue.Text != string.Empty ? Convert.ToInt32(tbHeightValue.Text) : _last_valid_height;
        }

        public int getWidth()
        {
            int return_value = 0;
            return return_value = tbWidthValue.Text != string.Empty ? Convert.ToInt32(tbWidthValue.Text) : _last_valid_width;
        }

        public Rectangle RectangleForeground
        {
            get 
            {
                ellDisplayForeground.Visibility = Visibility.Collapsed;
                rectTransDisplayForeground.Visibility = Visibility.Visible;
                return rectTransDisplayForeground; 
            }
        }

        private void sldSlidersChanged_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            int strokeThickness = (int)sldStrokeThickness.Value;
            tbStrokeThicknessValue.Text = strokeThickness.ToString();
            PocketPaintApplication.GetInstance().PaintData.BorderThicknessRecEll = strokeThickness;

            PocketPaintApplication.GetInstance().CurrentShape.StrokeThickness = strokeThickness;
        }

        private void tbHeightValue_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            tbHeightValue.Foreground = new SolidColorBrush(Colors.Black);
        }

        private void tbHeightValue_LostFocus(object sender, RoutedEventArgs e)
        {
            tbHeightValue.Foreground = new SolidColorBrush(Colors.White);

            _last_valid_height = tbHeightValue.Text != string.Empty ? Convert.ToInt32(tbHeightValue.Text) : _last_valid_width;
            tbHeightValue.Text = _last_valid_height.ToString();
        }

        private void tbWidthValue_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            tbWidthValue.Foreground = new SolidColorBrush(Colors.Black);
        }

        private void tbWidthValue_LostFocus(object sender, RoutedEventArgs e)
        {
            tbWidthValue.Foreground = new SolidColorBrush(Colors.White);

            _last_valid_width = tbWidthValue.Text != string.Empty ? Convert.ToInt32(tbWidthValue.Text) : _last_valid_width;
            tbWidthValue.Text = _last_valid_width.ToString();
        }

        private void tbHeightValue_TextChanged(object sender, TextChangedEventArgs e)
        {
            char[] comma = new char[1];
            comma[0] = ',';
            tbHeightValue.Text =  tbHeightValue.Text.Trim(comma);

            if(tbHeightValue.Text != string.Empty)
            {
                PocketPaintApplication.GetInstance().CurrentShape.Height = Convert.ToDouble(tbHeightValue.Text);
            }
        }

        private void tbWidthValue_TextChanged(object sender, TextChangedEventArgs e)
        {
            char[] comma = new char[1];
            comma[0] = ',';
            tbWidthValue.Text = tbWidthValue.Text.Trim(comma);

            if (tbWidthValue.Text != string.Empty)
            {
                PocketPaintApplication.GetInstance().CurrentShape.Width = Convert.ToDouble(tbWidthValue.Text);
            }
        }
    }
}
