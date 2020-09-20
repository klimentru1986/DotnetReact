using System;

namespace Domain
{
    public class Comment
    {
        public Guid Id { get; set; }
    
        public string Data { get; set; }

        public Guid ActivityId { get; set; }
    
        public virtual Activity Activity { get; set; }

        public string AuthorId { get; set; }

        public virtual AppUser Author { get; set; }
    
        public DateTime CreateDate { get; set; }    
        
    }
}