using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace face_builder
{
    public class DataManager
    {
        string connectionString = Utility.GetConnectionString();

        public void SaveFaceData(ViewModel model)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO Person (firstName, lastName, city, hair, eyes, nose, mouth) VALUES (@firstName, @lastName, @city, @hair, @eyes, @nose, @mouth)";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@firstName", model.FirstName);
                    command.Parameters.AddWithValue("@lastName", model.LastName);
                    command.Parameters.AddWithValue("@city", model.Address);
                    command.Parameters.AddWithValue("@hair", FaceBuilder.ImageManager.HairIndex);
                    command.Parameters.AddWithValue("@eyes", FaceBuilder.ImageManager.EyeIndex);
                    command.Parameters.AddWithValue("@nose", FaceBuilder.ImageManager.NoseIndex);
                    command.Parameters.AddWithValue("@mouth", FaceBuilder.ImageManager.MouthIndex);

                    try
                    {
                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                            MessageBox.Show("Face data saved successfully.");
                        else
                            MessageBox.Show("Error saving face data.");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }

        public List<string> LoadFacesComboBox()
        {
            List<string> faces = new List<string>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT firstName, lastName FROM Person ORDER BY id DESC";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string fullName = $"{reader["firstName"]} {reader["lastName"]}";
                                faces.Add(fullName);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error loading faces: {ex.Message}");
                    }
                }
            }
            return faces;
        }

        public void LoadSelectedFaceData(ViewModel model, string fullName)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT firstName, lastName, city, hair, eyes, nose, mouth FROM Person WHERE firstname + ' ' + lastName = @fullName";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@fullName", fullName);

                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                model.FirstName = reader["firstName"].ToString();
                                model.LastName = reader["lastName"].ToString();
                                model.Address = reader["city"].ToString();
                                FaceBuilder.LoadIndexes((int)reader["hair"], (int)reader["eyes"], (int)reader["nose"], (int)reader["mouth"]);


                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error loading face data: {ex.Message}");
                    }

                }
            }
        }
    }
}
