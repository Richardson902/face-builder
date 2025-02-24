using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace face_builder
{
    public class ImageManager
    {
        public enum Category
        {
            Hair,
            Eye,
            Nose,
            Mouth
        }
        public int HairIndex { get; private set; } = -1;
        public int EyeIndex { get; private set; } = -1;
        public int NoseIndex { get; private set; } = -1;
        public int MouthIndex { get; private set; } = -1;

        public List<BitmapImage> hairImages { get; private set; } = new List<BitmapImage>();
        public List<BitmapImage> eyeImages { get; private set; } = new List<BitmapImage>();
        public List<BitmapImage> noseImages { get; private set; } = new List<BitmapImage>();
        public List<BitmapImage> mouthImages { get; private set; } = new List<BitmapImage>();

        public bool UpdateHair { get; private set; }
        public bool UpdateEyes { get; private set; }
        public bool UpdateNose { get; private set; }
        public bool UpdateMouth { get; private set; }


        private readonly Random random = new Random();

        public void LoadImages(string hairPath, string eyePath, string nosePath, string mouthPath)
        {
            LoadCategoryImages(hairPath, hairImages);
            LoadCategoryImages(eyePath, eyeImages);
            LoadCategoryImages(nosePath, noseImages);
            LoadCategoryImages(mouthPath, mouthImages);

        }

        // Method to populate the lists with BM images based on whatever is in the folders.
        private static void LoadCategoryImages(string folderPath, List<BitmapImage> imageList)
        {
            try
            {
                string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
                string fullFolderPath = Path.Combine(baseDirectory, folderPath);

                // Populate imageFiles with all of the files with specified file extensions in folder path
                string[] imageFiles = Directory.GetFiles(fullFolderPath, "*.*", SearchOption.TopDirectoryOnly)
                                                .Where(file => file.EndsWith(".png") ||
                                                               file.EndsWith(".jpg"))
                                                .ToArray();

                // For each image file, create a bitmap image and add it to the list that was passed
                foreach (string file in imageFiles)
                {
                    BitmapImage bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.UriSource = new Uri(file, UriKind.Absolute);
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.EndInit();
                    imageList.Add(bitmap);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading images from {folderPath}: {ex.Message}");
            }
        }

        public void RandomizeIndexes()
        {
            HairIndex = random.Next(0, hairImages.Count);
            EyeIndex = random.Next(0, eyeImages.Count);
            NoseIndex = random.Next(0, noseImages.Count);
            MouthIndex = random.Next(0, mouthImages.Count);

            UpdateHair = UpdateEyes = UpdateNose = UpdateMouth = true;
        }

        // Get current images based on the indexes
        public BitmapImage GetHairImage() => HairIndex >= 0 ? hairImages[HairIndex] : null;
        public BitmapImage GetEyeImage() => EyeIndex >= 0 ? eyeImages[EyeIndex] : null;
        public BitmapImage GetNoseImage() => NoseIndex >= 0 ? noseImages[NoseIndex] : null;
        public BitmapImage GetMouthImage() => MouthIndex >= 0 ? mouthImages[MouthIndex] : null;

        public void Increment(Category category)
        {

            //Get list and index based on enum
            GetReferences(category, out List<BitmapImage> imageList, out int index);

            //Increment the index
            index++;

            // Wrap if index exceeds list count
            if (index > imageList.Count - 1 || index < 0)
            {
                index = 0;
            }

            UpdateIndex(category, index);
        }

        public void Decrement(Category category)
        {
            GetReferences(category, out List<BitmapImage> imageList, out int index);

            index--;

            // Wrap if index is less than 0
            if (index < 0)
            {
                index = imageList.Count - 1;
            }

            UpdateIndex(category, index);
        }

        // Updates indexes and sets corresponding flags to true
        private void UpdateIndex(Category category, int newIndex)
        {
            switch (category)
            {
                case Category.Hair:
                    HairIndex = newIndex;
                    UpdateHair = true;
                    break;
                case Category.Eye:
                    EyeIndex = newIndex;
                    UpdateEyes = true;
                    break;
                case Category.Nose:
                    NoseIndex = newIndex;
                    UpdateNose = true;
                    break;
                case Category.Mouth:
                    MouthIndex = newIndex;
                    UpdateMouth = true;
                    break;
            }
        }

        // Retrieves the image list and current index for the specified category
        public void GetReferences(Category category, out List<BitmapImage> imageList, out int index)
        {
            imageList = null;
            index = 0;

            switch (category)
            {
                case Category.Hair:
                    imageList = hairImages;
                    index = HairIndex;
                    break;
                case Category.Eye:
                    imageList = eyeImages;
                    index = EyeIndex;
                    break;
                case Category.Nose:
                    imageList = noseImages;
                    index = NoseIndex;
                    break;
                case Category.Mouth:
                    imageList = mouthImages;
                    index = MouthIndex;
                    break;
            }

        }

        // Sets flags to false and resets indexes to -1 (prob bad idea, works for now)
        public void ClearImages()
        {
            UpdateHair = UpdateEyes = UpdateNose = UpdateMouth = false;

            HairIndex = EyeIndex = NoseIndex = MouthIndex = -1;
        }
    }
}
