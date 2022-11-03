namespace StackOverflow_Search.Models
{
    public class Post
    {
        public int Id { get; set; }
        public short PostTypeId { get; set; }
        public int AcceptedAnswerId { get; set; }
        public int? ParentId { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime DeletionDate { get; set; }
        public int Score { get; set; }
        public int ViewCount { get; set; }
        public string Body { get; set; } = "";
        public int OwnerUserId { get; set; }
        public string OwnerDisplayName { get; set; } = "";
        public int LastEditorUserId { get; set; } = 0;
        public string LastEditorDisplayName { get; set; } = "";
        public DateTime LastEditDate { get; set; }
        public DateTime LastActivityDate { get; set; }
        public string Title { get; set; } = "";
        public List<string>? Tags { get; set; } = null;
        public int AnswerCount { get; set; }
        public int CommentCount { get; set; }
        public int FavoriteCount { get; set; }
        public DateTime ClosedDate { get; set; }
        public DateTime CommunityOwnedDate { get; set; }
        public string ContentLicense { get; set; } = "";
    }
}
