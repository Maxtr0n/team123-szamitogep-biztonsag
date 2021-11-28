namespace CAFFAdapterClient.ViewModels.Comments
{
    public class CommentByGifViewModel
    {
        public int CommentId { get; set; }
        public int? UserId { get; set; }
        public string Username { get; set; }
        public string Comment { get; set; }
    }
}