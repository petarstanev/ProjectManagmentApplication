namespace ProjectManagementApplication.Models
{
    public class FavoriteBoard
    {
        public int FavoriteBoardId { get; set; }

        public int BoardId { get; set; }
        public Board Board { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }
    }
}