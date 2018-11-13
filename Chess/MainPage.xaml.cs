﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Media.Imaging;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Chess
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            Image img = new Image();
            BitmapImage bitmapImage = new BitmapImage();
            Uri uri = new Uri("ms-appx:///Assets/wrook2.png");
            bitmapImage.UriSource = uri;
            img.Source = bitmapImage;
            img.Width = bitmapImage.DecodePixelWidth = 72;
            Grid.SetColumn(img, 1);
            Grid.SetRow(img, 3);
        }

        private void Select(object sender, RoutedEventArgs e)
        {
            Grid.SetColumn(wrook1, 1);
            Grid.SetRow(wrook1, 3);
        }
    }
}
