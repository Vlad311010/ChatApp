
using System.Collections;

namespace ChatApp.Client.Models
{
    public class ChatGroupMembers : IEquatable<ChatGroupMembers>
    {
        public int UserId { get; set; }
        public int ChatGroupId { get; set; }
        public ChatGroupMembers() { }
        public ChatGroupMembers(ChatGroup chatGroup, User user) 
        { 
            ChatGroupId = chatGroup.Id;
            UserId = user.Id;
        }

        public bool Equals(ChatGroupMembers other)
        {
            return other.UserId == UserId && other.ChatGroupId == ChatGroupId;
        }

        public override bool Equals(object? obj)
        {
            ChatGroupMembers? other = obj as ChatGroupMembers;
            if (other == null)
                return false;

            return other.UserId == UserId && other.ChatGroupId == ChatGroupId;
        }

        public override int GetHashCode()
        {
            return UserId.GetHashCode() ^ (ChatGroupId.GetHashCode() * 67);
        }

    }

    public class ChatGroupMembersCollection : ICollection<ChatGroupMembers>
    {
        private List<ChatGroupMembers> innerCol;
        public int Count => innerCol.Count;

        public bool IsReadOnly => false;

        public ChatGroupMembersCollection()
        {
            innerCol = new List<ChatGroupMembers>();
        }

        public ChatGroupMembers this[int index]
        {
            get { return innerCol[index]; }
            set { innerCol[index] = value; }
        }

        public void Add(ChatGroupMembers item)
        {
            if (!Contains(item))
            {
                innerCol.Add(item);
            }
        }

        public void Clear()
        {
            innerCol.Clear();
        }

        public bool Contains(ChatGroupMembers item)
        {
            return innerCol.Contains(item);
        }

        public void CopyTo(ChatGroupMembers[] array, int arrayIndex)
        {
            if (array == null)
                throw new ArgumentNullException("The array cannot be null.");
            if (arrayIndex < 0)
                throw new ArgumentOutOfRangeException("The starting array index cannot be negative.");
            if (Count > array.Length - arrayIndex)
                throw new ArgumentException("The destination array has fewer elements than the collection.");

            for (int i = 0; i < innerCol.Count; i++)
            {
                array[i + arrayIndex] = innerCol[i];
            }
        }

        public IEnumerator<ChatGroupMembers> GetEnumerator()
        {
            return innerCol.GetEnumerator();
        }

        public bool Remove(ChatGroupMembers item)
        {
            return innerCol.Remove(item);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return innerCol.GetEnumerator();    
        }
    }
}
