using System;
using System.Collections.Generic;

namespace CSE212.HW.Week02
{
    // PriorityQueue implementation meeting the requirements
    public class PriorityQueue
    {
        // Internal list of entries in enqueue order
        private readonly List<(string Value, int Priority, long Seq)> _items = new List<(string, int, long)>();
        private long _nextSeq = 0;

        // Enqueue: add to back preserving sequence
        public void Enqueue(string value, int priority)
        {
            if (value == null) throw new ArgumentNullException(nameof(value));
            _items.Add((value, priority, _nextSeq++));
        }

        // Dequeue: remove the item with highest priority; if tie, remove lowest Seq (earliest enqueued)
        public string Dequeue()
        {
            if (_items.Count == 0)
                throw new InvalidOperationException("The queue is empty.");

            int bestIndex = 0;
            int bestPriority = _items[0].Priority;
            long bestSeq = _items[0].Seq;

            for (int i = 1; i < _items.Count; i++)
            {
                var it = _items[i];
                if (it.Priority > bestPriority || (it.Priority == bestPriority && it.Seq < bestSeq))
                {
                    bestIndex = i;
                    bestPriority = it.Priority;
                    bestSeq = it.Seq;
                }
            }

            var value = _items[bestIndex].Value;
            _items.RemoveAt(bestIndex);
            return value;
        }

        // For tests: helper to know how many items are in queue
        public int Count => _items.Count;
    }
}