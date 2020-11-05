using System.Collections.Generic;

namespace LeyendsServer
{
    public class QueueGroup
    {
        public string matchType { get; set; }
        public List<PlayerQueue> groupMembers { get; set; }
        public QueueGroup(string _matchType, List<PlayerQueue> _groupMembers)
        {
            matchType = _matchType;
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