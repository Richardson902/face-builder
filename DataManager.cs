using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace face_builder
{
    public class DataManager
    {
        private string GetFilePath(string firstName, string lastName)
        {
            return $"{firstName}_{lastName}.txt";
        }

        public void SaveFaceData(ViewModel model)
        {
            string filePath = GetFilePath(model.FirstName, model.LastName);

            var faceData = new StringBuilder();
            faceData.AppendLine($"First Name: {model.FirstName}");
            faceData.AppendLine($"Last Name: {model.LastName}");
            faceData.AppendLine($"Address: {model.Address}");
            faceData.AppendLine($"Occupation: {model.SelectedOccupation}");
            faceData.AppendLine($"Hobby: {model.SelectedHobby}");
            faceData.AppendLine($"Dog Lover: {model.IsDogLover}");
            faceData.AppendLine($"Cat Lover: {model.IsCatLover}");
            faceData.AppendLine($"Hair Index: {FaceBuilder.ImageManager.HairIndex}");
            faceData.AppendLine($"Eye Index: {FaceBuilder.ImageManager.EyeIndex}");
            faceData.AppendLine($"Nose Index: {FaceBuilder.ImageManager.NoseIndex}");
            faceData.AppendLine($"Mouth Index: {FaceBuilder.ImageManager.MouthIndex}");

            File.WriteAllText(filePath, faceData.ToString());
        }
    }
}
