using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace face_builder
{
    public partial class MainWindow : Window
    {
        //Handle in data manager

        public MainWindow()
        {
            InitializeComponent();

            // Assigns the invokable to call update canvas method
            FaceBuilder.OnImagesUpdated += UpdateCanvas;

            // Loads the images at runtime
            FaceBuilder.LoadImages();

            // Sets the commands and keybindings
            InitializeKeyBindings();
        }

        // Clears canvas, and then draws image based on what flags are true
        public void UpdateCanvas()
        {

            myCanvas.Children.Clear();
            summaryCanvas.Children.Clear();

            if (FaceBuilder.IsUpdateHair)
            {
                DrawImage(FaceBuilder.GetHairImage(), 0, 0);
            }

            if (FaceBuilder.IsUpdateEyes)
            {
                DrawImage(FaceBuilder.GetEyeImage(), 0, 250);
            }

            if (FaceBuilder.IsUpdateNose)
            {
                DrawImage(FaceBuilder.GetNoseImage(), 0, 500);
            }

            if (FaceBuilder.IsUpdateMouth)
            {
                DrawImage(FaceBuilder.GetMouthImage(), 0, 750);
            }
        }

        // Sets the image and location to draw on the canvas
        private void DrawImage(BitmapImage image, double left, double top)
        {
            if (image != null)
            {
                Image imgCanvas = new Image { Source = image };
                Image imgSummary = new Image { Source = image };
                Canvas.SetLeft(imgCanvas, left);
                Canvas.SetTop(imgCanvas, top);
                Canvas.SetLeft(imgSummary, left);
                Canvas.SetTop(imgSummary, top);
                myCanvas.Children.Add(imgCanvas);
                summaryCanvas.Children.Add(imgSummary);
            }
        }

        // Block of code that sets up the key bindings (could prob move into some sort of list lmfao)
        private void InitializeKeyBindings()
        {
            InputBindings.Add(new KeyBinding(((ViewModel)DataContext).RandomizeCommand, new KeyGesture(Key.R, ModifierKeys.Control)));
            InputBindings.Add(new KeyBinding(((ViewModel)DataContext).ClearFaceCommand, new KeyGesture(Key.C, ModifierKeys.Control)));
            InputBindings.Add(new KeyBinding(((ViewModel)DataContext).HairNextCommand, new KeyGesture(Key.D2, ModifierKeys.Control)));
            InputBindings.Add(new KeyBinding(((ViewModel)DataContext).HairPrevCommand, new KeyGesture(Key.D1, ModifierKeys.Control)));
            InputBindings.Add(new KeyBinding(((ViewModel)DataContext).EyesNextCommand, new KeyGesture(Key.W, ModifierKeys.Control)));
            InputBindings.Add(new KeyBinding(((ViewModel)DataContext).EyesPrevCommand, new KeyGesture(Key.Q, ModifierKeys.Control)));
            InputBindings.Add(new KeyBinding(((ViewModel)DataContext).NoseNextCommand, new KeyGesture(Key.S, ModifierKeys.Control)));
            InputBindings.Add(new KeyBinding(((ViewModel)DataContext).NosePrevCommand, new KeyGesture(Key.A, ModifierKeys.Control)));
            InputBindings.Add(new KeyBinding(((ViewModel)DataContext).MouthNextCommand, new KeyGesture(Key.X, ModifierKeys.Control)));
            InputBindings.Add(new KeyBinding(((ViewModel)DataContext).MouthPrevCommand, new KeyGesture(Key.Z, ModifierKeys.Control)));
        }
    }
}
