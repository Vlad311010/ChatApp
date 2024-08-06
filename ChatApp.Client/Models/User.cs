using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace ChatApp.Client.Models
{
    public class User
    {
        public int Id { get; set; }

        [Column(TypeName = "VARCHAR(255)")]
        public string Login { get; set; }
        
        [Column(TypeName = "VARCHAR(255)")]
        public string Password { get; set; }
        public User() { }
        public User(string login, string password)
        {
            Login = login;
            Password = password;
        }

        public override bool Equals(object? obj)
        {
            User? other = obj as User;
            if (other == null)
                return false;

            return other.Id == Id && other.Login == Login;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode() ^ (Login.GetHashCode() * 31);
        }

    }
}
