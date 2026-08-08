using Chart.Models;

namespace Chart.Interface
{
    public interface IUsers
    {
        List<Users> GetUsers();
        string Save(Users oUsers);
        int GetUserID();
        Users Add(Users user);
        Users Get(int id);
        Users Update(Users user);
        Users ChangePassword(Users oUsers);
        string Delete (int id);
    }
}
