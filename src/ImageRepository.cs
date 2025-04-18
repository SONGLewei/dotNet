using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace Projet_2025_1
{
    public class ImageRepository
    {
        private readonly string _connectionString;

        public ImageRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<Images> GetAllImages()
        {
            var result = new List<Images>();
            using (var conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                string sql = "SELECT Id, FilePath FROM images";
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var image = new Images
                            {
                                Id = reader.GetInt32("Id"),
                                FilePath = reader.GetString("FilePath")
                            };
                            result.Add(image);
                        }
                    }
                }
            }
            return result;
        }

        public int InsertImage(string filePath)
        {
            using (var conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                string sql = @"
                    INSERT INTO images (FilePath)
                    VALUES (@filePath);
                    SELECT LAST_INSERT_ID();
                ";
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@filePath", filePath);
                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
        }

        public void LinkImageToTag(int imageId, int tagId)
        {
            using (var conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                string sql = "INSERT INTO image_tags (ImageId, TagId) VALUES (@imageId, @tagId)";
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@imageId", imageId);
                    cmd.Parameters.AddWithValue("@tagId", tagId);
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public void DeleteImage(int imageId)
        {
            using (var conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                string sql = "DELETE FROM images WHERE Id = @imageId";
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@imageId", imageId);
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public void UnlinkImageFromTag(int imageId, int tagId)
        {
            using (var conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                string sql = "DELETE FROM image_tags WHERE ImageId = @imageId AND TagId = @tagId";
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@imageId", imageId);
                    cmd.Parameters.AddWithValue("@tagId", tagId);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public List<Tag> GetTagsForImage(int imageId)
        {
            var result = new List<Tag>();
            using (var conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                string sql = @"
                    SELECT t.Id, t.ParentId, t.TagName
                    FROM tags t
                    INNER JOIN image_tags it ON t.Id = it.TagId
                    WHERE it.ImageId = @imageId
                ";
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@imageId", imageId);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var tag = new Tag
                            {
                                Id = reader.GetInt32("Id"),
                                ParentId = reader.IsDBNull(reader.GetOrdinal("ParentId"))
                                    ? (int?)null
                                    : reader.GetInt32("ParentId"),
                                TagName = reader.GetString("TagName")
                            };
                            result.Add(tag);
                        }
                    }
                }
            }
            return result;
        }
    }
}
