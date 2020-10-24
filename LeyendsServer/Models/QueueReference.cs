using System.Collections.Generic;

namespace LeyendsServer
{
    public partial class QueueReference
    {
        public int groupSize  { get; set; }
        public List<PlayerQueue> newQueue  { get; set; }

        public QueueReference (List<int> _newQueue, bool _isLeader){
            newQueue = new List<PlayerQueue>();
            foreach (int _id in _newQueue)
            {
                newQueue.Add(new PlayerQueue(_id));
            }
        }

        public override string ToString()
        {
            return $"Reference: {groupSize} Players queue as group";
        }
    }
}

