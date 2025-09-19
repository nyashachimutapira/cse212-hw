using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CSE212.HW.Week02
{
    [TestClass]
    public class PriorityQueueTests
    {
        [TestMethod]
        public void TestPriorityQueue_HighestPriorityDequeuedFirst()
        {
            var pq = new PriorityQueue();
            pq.Enqueue("Low", 1);
            pq.Enqueue("Medium", 5);
            pq.Enqueue("High", 10);

            var result = pq.Dequeue();
            Assert.AreEqual("High", result);
            // Defect Found: If Dequeue does not choose the highest priority.
            Assert.AreEqual(2, pq.Count);
        }

        [TestMethod]
        public void TestPriorityQueue_TieBreakingFIFO()
        {
            var pq = new PriorityQueue();
            pq.Enqueue("A", 5);  // seq0
            pq.Enqueue("B", 5);  // seq1
            pq.Enqueue("C", 10); // seq2 highest
            pq.Enqueue("D", 5);  // seq3

            Assert.AreEqual("C", pq.Dequeue(), "Expected highest-priority item 'C' first.");
            Assert.AreEqual("A", pq.Dequeue(), "Expected FIFO among priority-5 items: 'A' first.");
            Assert.AreEqual("B", pq.Dequeue());
            Assert.AreEqual("D", pq.Dequeue());
            Assert.AreEqual(0, pq.Count);
            // Defect Found: Incorrect tie-breaking or wrong selection logic.
        }

        [TestMethod]
        public void TestPriorityQueue_AllSamePriorityFIFO()
        {
            var pq = new PriorityQueue();
            pq.Enqueue("one", 7);
            pq.Enqueue("two", 7);
            pq.Enqueue("three", 7);

            Assert.AreEqual("one", pq.Dequeue());
            Assert.AreEqual("two", pq.Dequeue());
            Assert.AreEqual("three", pq.Dequeue());
            // Defect Found: Ensure overall FIFO behavior with equal priorities.
        }

        [TestMethod]
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
                // Defect Found: Exception message does not match the expected message.
            }
        }

        [TestMethod]
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
            // Defect Found: Ensure dynamic behavior is correct during interleaved operations.
        }
    }
}