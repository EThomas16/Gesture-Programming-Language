using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Gesture3
{
    //Static class that any other can access which formats a given UI element based on the given parameters
    static class Formatting
    {
        public static void buttonInit(Button btn, double left, double top, double width, double height, double fontSize, Brush fontColour, Brush backColour, HorizontalAlignment ha = HorizontalAlignment.Center)
        {
            //Initialises any button components with the give parameters when called
            btn.FontSize = fontSize;
            btn.Background = backColour;
            btn.Foreground = fontColour;
            btn.Width = width;
            btn.Height = height;
            btn.FontFamily = new FontFamily("Century Gothic");
            Canvas.SetTop(btn, top);
            Canvas.SetLeft(btn, left);
        }

        public static void titleInit(Label lbl, double left, double top, double width, double height, double fontSize, Brush fontColor, Brush backColor, HorizontalAlignment ha = HorizontalAlignment.Center)
        {
            //Initialises title components with the given parameters when called
            lbl.Width = width;
            lbl.Height = height;
            lbl.Background = backColor;
            lbl.FontSize = fontSize;
            lbl.Foreground = fontColor;
            lbl.FontWeight = FontWeights.Bold;
            lbl.HorizontalContentAlignment = ha;
            lbl.FontFamily = new FontFamily("Century Gothic");
            Canvas.SetLeft(lbl, left);
            Canvas.SetTop(lbl, top);
        }

        public static void labelInit(Label lbl, double left, double top, double width, double height, double fontSize, Brush backColor, Brush fontColor, HorizontalAlignment ha = HorizontalAlignment.Center)
        {
            //Initialises labels with the given parameters when called
            lbl.Width = width;
            lbl.Height = height;
            lbl.Background = backColor;
            lbl.FontSize = fontSize;
            lbl.Foreground = fontColor;
            lbl.HorizontalContentAlignment = ha;
            lbl.FontFamily = new FontFamily("Century Gothic");
            Canvas.SetLeft(lbl, left);
            Canvas.SetTop(lbl, top);
        }

        public static void textBoxInit(TextBox text, double left, double top, double width, double height, double fontSize, HorizontalAlignment ha = HorizontalAlignment.Center)
        {
            //Initialises text boxes with the given parameters when called
            //For borders, they are defaulted as being black borders with a width of 4
            text.FontSize = fontSize;
            text.Width = width;
            text.Height = height;
            Canvas.SetTop(text, top);
            Canvas.SetLeft(text, left);
        }

    }
}
