using ChatApp.Client.Pages;
using ChatApp.Client.ViewModels;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChatApp.Client.Models
{
    public class ChatGroup
    {
        public int Id { get; set; }

        [Column(TypeName = "VARCHAR(255)")]
        public string Name { get; set; } = string.Empty;

        [Column(TypeName = "VARCHAR(3000)")]
        public string? Description { get; set; }
        public bool IsPublic { get; set; } = false;
        public int OwnerId { get; set; }
        public virtual ChatGroupMembersCollection Memebers { get; set; }

        public ChatGroup() { }

        public ChatGroup(ChatGroupView viewModel) 
        {
            Name = viewModel.Name;
            Description = viewModel.Description;
            IsPublic = viewModel.IsPublic;
        }


        public override bool Equals(object? obj)
        {
            ChatGroup? other = obj as ChatGroup;
            if (other == null)
                return false;

            return other.Id == Id && other.Name == Name;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode() ^ (Name.GetHashCode() * 41);
        }
    }
}
