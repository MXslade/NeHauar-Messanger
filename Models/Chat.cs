namespace NeHauar.Models
{
    public class Chat
    {
        public int Id { get; set; }
        public User FirstUser { get; set; }
        public int FirstUserId { get; set; }
        public User SecondUser { get; set; }
        public int SecondUserId { get; set; }
    }
}