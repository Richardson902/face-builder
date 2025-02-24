using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace face_builder
{
    public static class FaceBuilder
    {
        private static ImageManager _imageManager = new ImageManager();

        public static ImageManager ImageManager
        {
            get { return _imageManager; }
        }

        // Invokable to tell the main window to update canvas
        public static event Action OnImagesUpdated;

        public static void LoadImages()
        {
            string basePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "images");
            ImageManager.LoadImages(
                Path.Combine(basePath, "hair"),
                Path.Combine(basePath, "eyes"),
                Path.Combine(basePath, "nose"),
                Path.Combine(basePath, "mouth")
            );
        }

        public static void HairNext()
        {
            ImageManager.Increment(ImageManager.Category.Hair);
            OnImagesUpdated?.Invoke();
        }

        public static void HairPrev()
        {
            ImageManager.Decrement(ImageManager.Category.Hair);
            OnImagesUpdated?.Invoke();
        }

        public static void EyesNext()
        {
            ImageManager.Increment(ImageManager.Category.Eye);
            OnImagesUpdated?.Invoke();
        }

        public static void EyesPrev()
        {
            ImageManager.Decrement(ImageManager.Category.Eye);
            OnImagesUpdated?.Invoke();
        }

        public static void NoseNext()
        {
            ImageManager.Increment(ImageManager.Category.Nose);
            OnImagesUpdated?.Invoke();
        }

        public static void NosePrev()
        {
            ImageManager.Decrement(ImageManager.Category.Nose);
            OnImagesUpdated?.Invoke();
        }

        public static void MouthNext()
        {
            ImageManager.Increment(ImageManager.Category.Mouth);
            OnImagesUpdated?.Invoke();
        }

        public static void MouthPrev()
        {
            ImageManager.Decrement(ImageManager.Category.Mouth);
            OnImagesUpdated?.Invoke();
        }

        public static void Randomize()
        {
            ImageManager.RandomizeIndexes();
            OnImagesUpdated?.Invoke();
        }

        public static void ClearCanvas()
        {
            ImageManager.ClearImages();
            OnImagesUpdated?.Invoke();
        }

        // Gets the images at current indexes from image manager
        public static BitmapImage GetHairImage() => ImageManager.GetHairImage();
        public static BitmapImage GetEyeImage() => ImageManager.GetEyeImage();
        public static BitmapImage GetNoseImage() => ImageManager.GetNoseImage();
        public static BitmapImage GetMouthImage() => ImageManager.GetMouthImage();

        // Gets current flags from image manager
        public static bool IsUpdateHair => ImageManager.UpdateHair;
        public static bool IsUpdateEyes => ImageManager.UpdateEyes;
        public static bool IsUpdateNose => ImageManager.UpdateNose;
        public static bool IsUpdateMouth => ImageManager.UpdateMouth;
    }
}
