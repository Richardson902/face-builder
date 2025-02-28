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

        public int SaveFaceData(ViewModel model)
        {
            int newFaceId = -1;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO Person (firstName, lastName, city, hair, eyes, nose, mouth) " +
                    "VALUES (@firstName, @lastName, @city, @hair, @eyes, @nose, @mouth); " +
                    "SELECT SCOPE_IDENTITY();"; // returns newly created id

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
                        var result = command.ExecuteScalar();

                        if (result != null && result != DBNull.Value)
                        {
                            newFaceId = Convert.ToInt32(result);
                            MessageBox.Show("Face data saved successfully.");
                        }
                        else
                        {
                            MessageBox.Show("Error saving face data.");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
            return newFaceId;
        }

        public List<KeyValuePair<int, string>> LoadFacesComboBox()
        {
            List<KeyValuePair<int, string>> faces = new List<KeyValuePair<int, string>>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT id, firstName, lastName FROM Person ORDER BY id DESC";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                int id = (int)reader["id"];
                                string fullName = $"{reader["firstName"]} {reader["lastName"]}";
                                faces.Add(new KeyValuePair<int, string>(id, fullName));
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

        public void LoadSelectedFaceData(ViewModel model, int faceId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT firstName, lastName, city, hair, eyes, nose, mouth FROM Person WHERE id = @id";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", faceId);

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

        public void UpdateFaceData(ViewModel model, int faceId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "UPDATE Person SET firstName = @firstName, lastName = @lastName, city = @city, hair = @hair, eyes = @eyes, nose = @nose, mouth = @mouth WHERE id = @id";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", faceId);
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
                        {
                            MessageBox.Show("Face data updated successfully.");
                        }
                        else
                        {
                            MessageBox.Show("Error updating face data.");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }
    }
}
