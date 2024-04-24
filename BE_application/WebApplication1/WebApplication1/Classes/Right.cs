namespace WebApplication1.Classes
{
    public class Right(int UserId, string Username, bool isEmployee)
    {
        public int UserId { get; set; } = UserId;
        public string Username { get; set; } = Username;
        public bool isEmployee { get; set; } = isEmployee;
    }
}
