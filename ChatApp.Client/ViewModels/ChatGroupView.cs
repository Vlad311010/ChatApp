using System.ComponentModel.DataAnnotations;

namespace ChatApp.Client.ViewModels
{
    public class ChatGroupView
    {
        [Required, StringLength(5, MinimumLength = 2, ErrorMessage = "Chat group name must be between 2 and 255 characters in length.")]
        public string Name { get; set; }

        public string? Description { get; set; }
        public int OwnerId { get; set; }

        [Required]
        public bool IsPublic { get; set; }
    }
}
