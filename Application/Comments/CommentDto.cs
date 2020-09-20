using System;

namespace Application.Comments
{
    public class CommentDto
    {
        public Guid Id { get; set; }

        public string Data { get; set; }

        public DateTime CreateDate { get; set; }

        public string UserName { get; set; }

        public string DisplayName { get; set; }
    }
}