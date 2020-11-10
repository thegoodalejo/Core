using System.Collections.Generic;

namespace LeyendsServer
{
    public class QueueGroup
    {
        public int id { get; set; }
        public string matchType { get; set; }
        public bool isOnRoom { get; set; }
        public List<PlayerQueue> groupMembers { get; set; }
        public QueueGroup(string _matchType, List<PlayerQueue> _groupMembers)
        {
            id = _groupMembers[0].id;
            matchType = _matchType;
            isOnRoom = false;
            groupMembers = _groupMembers;
        }
        public List<int> GroupMembers()
        {
            List<int> _members = new List<int>();
            foreach (PlayerQueue item in groupMembers)
            {
                _members.Add(item.id);
            }
            return _members;
        }

        public int GroupSize()
        {
            return groupMembers.Count;
        }

        public void RemoveSingle(int _groupMember)
        {
            foreach (PlayerQueue item in groupMembers)
            {
                if (item.id == _groupMember)
                {
                    groupMembers.Remove(item);
                    return;
                }
            }
        }
    }
}