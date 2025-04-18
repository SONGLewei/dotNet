using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace Projet_2025_1
{
    /// <summary>
    /// 标签与图片管理类
    /// </summary>
    public class TagRepository
    {
        private readonly string _connectionString;

        public TagRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        /// <summary>
        /// 获取所有标签
        /// </summary>
        public List<Tag> GetAllTags()
        {
            var result = new List<Tag>();
            using (var conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                string sql = "SELECT Id, ParentId, TagName FROM tags";
                using (var cmd = new MySqlCommand(sql, conn))
                {
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

        /// <summary>
        /// 插入新标签
        /// </summary>
        public int InsertTag(int? parentId, string tagName)
        {
            using (var conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                string sql = @"
                    INSERT INTO tags (ParentId, TagName) 
                    VALUES (@parentId, @tagName);
                    SELECT LAST_INSERT_ID();
                ";
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@parentId",
                        parentId.HasValue ? (object)parentId.Value : DBNull.Value);
                    cmd.Parameters.AddWithValue("@tagName", tagName);

                    int insertedId = Convert.ToInt32(cmd.ExecuteScalar());
                    return insertedId;
                }
            }
        }

        /// <summary>
        /// 删除标签
        /// </summary>
        public void DeleteTag(int tagId)
        {
            using (var conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                string sql = "DELETE FROM tags WHERE Id = @id";
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id", tagId);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// 更新标签名称
        /// </summary>
        public void UpdateTag(int tagId, string newTagName)
        {
            using (var conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                string sql = "UPDATE tags SET TagName = @tagName WHERE Id = @id";
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@tagName", newTagName);
                    cmd.Parameters.AddWithValue("@id", tagId);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// 检查标签名称唯一性
        /// </summary>
        public bool IsTagNameUnique(int? parentId, string tagName, int? currentTagId = null)
        {
            using (var conn = new MySqlConnection(_connectionString))
            {
                conn.Open();

                string sql = @"
                    SELECT COUNT(*)
                    FROM tags
                    WHERE
                    (
                        (ParentId IS NULL AND @parentId IS NULL)
                        OR (ParentId = @parentId)
                    )
                    AND TagName = @tagName
                ";
                if (currentTagId.HasValue)
                {
                    sql += " AND Id <> @currentTagId";
                }

                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@parentId",
                        parentId.HasValue ? (object)parentId.Value : DBNull.Value);
                    cmd.Parameters.AddWithValue("@tagName", tagName);
                    if (currentTagId.HasValue)
                    {
                        cmd.Parameters.AddWithValue("@currentTagId", currentTagId.Value);
                    }

                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    return count == 0;
                }
            }
        }

        // ================= 新增功能 =================

        /// <summary>
        /// 获取或创建标签
        /// </summary>
        public int GetOrCreateTag(string tagName)
        {
            using (var conn = new MySqlConnection(_connectionString))
            {
                conn.Open();

                // 检查标签是否存在
                string checkSql = "SELECT Id FROM tags WHERE TagName = @tagName";
                using (var cmd = new MySqlCommand(checkSql, conn))
                {
                    cmd.Parameters.AddWithValue("@tagName", tagName);
                    var result = cmd.ExecuteScalar();

                    if (result != null)
                    {
                        // 返回已存在标签的ID
                        return Convert.ToInt32(result);
                    }
                }

                // 插入新标签
                string insertSql = "INSERT INTO tags (TagName) VALUES (@tagName); SELECT LAST_INSERT_ID();";
                using (var cmd = new MySqlCommand(insertSql, conn))
                {
                    cmd.Parameters.AddWithValue("@tagName", tagName);
                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
        }

        /// <summary>
        /// 关联图片与标签
        /// </summary>
        public void LinkImageToTag(int imageId, int tagId)
        {
            using (var conn = new MySqlConnection(_connectionString))
            {
                conn.Open();

                string sql = "INSERT IGNORE INTO image_tags (ImageId, TagId) VALUES (@imageId, @tagId)";
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@imageId", imageId);
                    cmd.Parameters.AddWithValue("@tagId", tagId);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// 解除图片与标签的关联
        /// </summary>
       

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
        public void UnlinkAllTagsFromImage(int imageId)
        {
            using (var conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                string sql = "DELETE FROM image_tags WHERE ImageId = @imageId";
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@imageId", imageId);
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public List<Images> GetImagesByTag(string tagName)
        {
            var result = new List<Images>();

            using (var conn = new MySqlConnection(_connectionString))
            {
                conn.Open();
                string sql = @"
            SELECT i.Id, i.FilePath 
            FROM images i
            INNER JOIN image_tags it ON i.Id = it.ImageId
            INNER JOIN tags t ON it.TagId = t.Id
            WHERE t.TagName = @tagName";

                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@tagName", tagName);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            result.Add(new Images
                            {
                                Id = reader.GetInt32("Id"),
                                FilePath = reader.GetString("FilePath")
                            });
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// 获取与图片关联的所有标签
        /// </summary>
        public List<Tag> GetTagsForImage(int imageId)
        {
            var tags = new List<Tag>();

            using (var conn = new MySqlConnection(_connectionString))
            {
                conn.Open();

                string sql = @"
                    SELECT t.Id, t.ParentId, t.TagName
                    FROM tags t
                    INNER JOIN image_tags it ON t.Id = it.TagId
                    WHERE it.ImageId = @imageId";
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
                            tags.Add(tag);
                        }
                    }
                }
            }

            return tags;
        }
    }
}
