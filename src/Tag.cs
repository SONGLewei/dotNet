using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace Projet_2025_1
{
    public class Tag
    {
        public int Id { get; set; }
        public int? ParentId { get; set; }
        public string TagName { get; set; }
    }

}
