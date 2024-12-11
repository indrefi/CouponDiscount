using System;

namespace Domain.Entities
{
    public class User
    {
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string UserPassword { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
