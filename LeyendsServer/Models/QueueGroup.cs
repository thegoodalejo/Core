using System.Collections.Generic;

namespace LeyendsServer
{
    public class QueueGroup
    {
        public string matchType { get; set; }
        public List<PlayerQueue> groupMembers { get; set; }
        public QueueGroup(string _matchType,List<PlayerQueue> _groupMembers){
            matchType = _matchType;
            groupMembers = _groupMembers;
        }
    }
}