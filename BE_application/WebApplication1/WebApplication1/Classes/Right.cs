namespace WebApplication1.Classes
{
    public class Right(string Username, bool isEmployee)
    {
        public string Username { get; set; } = Username;
        public bool isEmployee { get; set; } = isEmployee;
    }
}
