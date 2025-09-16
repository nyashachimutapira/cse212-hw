using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CSE212.HW.Week02
{
    // PriorityQueue implementation meeting the requirements:
    // - Enqueue adds an item (value + priority) to the back.
    // - Dequeue removes and returns the value of the item with the highest priority.
    // - If multiple items share the highest priority, the one closest to the front (FIFO) is removed.
    // - Dequeue from an empty queue throws InvalidOperationException with message "The queue is empty."
    public class PriorityQueue
    {
        // We keep an internal list of entries in enqueue order.
        // Each entry stores value, priority, and enqueue sequence index to preserve strict FIFO ties.
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
                if (it.Priority > bestPriority)
                {
                    bestIndex = i;
                    bestPriority = it.Priority;
                    bestSeq = it.Seq;
                }
                else if (it.Priority == bestPriority)
                {
                    // choose earlier seq (smaller seq) to maintain FIFO among equals
                    if (it.Seq < bestSeq)
                    {
                        bestIndex = i;
                        bestSeq = it.Seq;
                    }
                }
            }

            var value = _items[bestIndex].Value;
            _items.RemoveAt(bestIndex);
            return value;
        }

        // For tests: helper to know how many items are in queue
        public int Count => _items.Count;
    }

    [TestClass]
    public class PriorityQueueTests
    {
        [TestMethod]
        // Scenario:
        //   Enqueue three items with distinct priorities in the order: Low, Medium, High.
        // Expected Result:
        //   Dequeue should return the highest-priority item ("High") first.
        // Defect(s) Found:
        //   (This test will detect if Dequeue does not choose the highest priority.)
        public void TestPriorityQueue_HighestPriorityDequeuedFirst()
        {
            var pq = new PriorityQueue();
            pq.Enqueue("Low", 1);
            pq.Enqueue("Medium", 5);
            pq.Enqueue("High", 10);

            var result = pq.Dequeue();
            Assert.AreEqual("High", result);
            // ensure remaining items still present (order between remaining is not specified by requirement)
            Assert.AreEqual(2, pq.Count);
        }

        [TestMethod]
        // Scenario:
        //   Enqueue multiple items that share the same highest priority; verify FIFO tie-breaking.
        //   Enqueue order: A(priority=5), B(priority=5), C(priority=10), D(priority=5)
        // Expected Result:
        //   First Dequeue -> C (priority 10)
        //   Next Dequeue -> A (first enqueued among priority 5)
        //   Next Dequeue -> B
        //   Next Dequeue -> D
        // Defect(s) Found:
        //   (This test will detect incorrect tie-breaking or wrong selection logic.)
        public void TestPriorityQueue_TieBreakingFIFO()
        {
            var pq = new PriorityQueue();
            pq.Enqueue("A", 5);  // seq0
            pq.Enqueue("B", 5);  // seq1
            pq.Enqueue("C", 10); // seq2 highest
            pq.Enqueue("D", 5);  // seq3

            Assert.AreEqual("C", pq.Dequeue(), "Expected highest-priority item 'C' first.");
            Assert.AreEqual("A", pq.Dequeue(), "Expected FIFO among priority-5 items: 'A' then 'B' then 'D'.");
            Assert.AreEqual("B", pq.Dequeue());
            Assert.AreEqual("D", pq.Dequeue());
            Assert.AreEqual(0, pq.Count);
        }

        [TestMethod]
        // Scenario:
        //   Enqueue several items with equal priorities only, verify overall FIFO behavior.
        //   Enqueue order: one, two, three (all same priority)
        // Expected Result:
        //   Dequeue returns them in the same order they were enqueued.
        public void TestPriorityQueue_AllSamePriorityFIFO()
        {
            var pq = new PriorityQueue();
            pq.Enqueue("one", 7);
            pq.Enqueue("two", 7);
            pq.Enqueue("three", 7);

            Assert.AreEqual("one", pq.Dequeue());
            Assert.AreEqual("two", pq.Dequeue());
            Assert.AreEqual("three", pq.Dequeue());
        }

        [TestMethod]
        // Scenario:
        //   Attempt to Dequeue from an empty queue.
        // Expected Result:
        //   InvalidOperationException thrown with message "The queue is empty."
        public void TestPriorityQueue_DequeueEmptyThrows()
        {
            var pq = new PriorityQueue();

            try
            {
                pq.Dequeue();
                Assert.Fail("Expected InvalidOperationException when dequeuing empty queue.");
            }
            catch (InvalidOperationException ex)
            {
                Assert.AreEqual("The queue is empty.", ex.Message);
            }
        }

        [TestMethod]
        // Scenario:
        //   Interleave Enqueue and Dequeue operations to ensure dynamic behavior is correct.
        //   Steps:
        //     Enqueue A(3), B(2), C(3)
        //     Dequeue -> (A or C with priority 3) but A is earlier
        //     Enqueue D(4)
        //     Dequeue -> D (priority 4)
        //     Dequeue -> C (remaining priority 3)
        //     Dequeue -> B (priority 2)
        // Expected Result:
        //   Sequence: A, D, C, B
        public void TestPriorityQueue_InterleavedOperations()
        {
            var pq = new PriorityQueue();
            pq.Enqueue("A", 3); // seq0
            pq.Enqueue("B", 2); // seq1
            pq.Enqueue("C", 3); // seq2

            Assert.AreEqual("A", pq.Dequeue(), "Expected A (3) before C (3) due to FIFO among equals.");

            pq.Enqueue("D", 4); // seq3

            Assert.AreEqual("D", pq.Dequeue(), "Expected D (4) as highest priority.");
            Assert.AreEqual("C", pq.Dequeue(), "Expected remaining priority-3 item C.");
            Assert.AreEqual("B", pq.Dequeue(), "Expected B (2) last.");
        }
    }
}