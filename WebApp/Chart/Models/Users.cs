namespace Chart.Models
{
    public class Users
    {
        public int UserID { get; set; }
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public string? NewPassword { get; set; }
        public string? IsActive { get; set; }
        public string? UserType { get; set; }
    }
}
