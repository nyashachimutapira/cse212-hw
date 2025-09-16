using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

// Define the Person class in its own namespace
namespace YourNamespace
{
    public class Person
    {
        public string Name { get; }
        public int Turns { get; set; }

        public Person(string name, int turns)
        {
            Name = name;
            Turns = turns;
        }
    }
}

// Define the TakingTurnsQueue class in the same namespace
namespace YourNamespace
{
    public class TakingTurnsQueue
    {
        private Queue<Person> queue = new Queue<Person>();

        public void AddPerson(string name, int turns)
        {
            queue.Enqueue(new Person(name, turns));
        }

        public Person GetNextPerson()
        {
            if (queue.Count == 0)
            {
                throw new InvalidOperationException("No one in the queue.");
            }

            Person person = queue.Dequeue();

            if (person.Turns > 0)
            {
                person.Turns--;
            }

            // Re-add person if they still have turns left or have infinite turns
            if (person.Turns >= 0) // Includes 0 turns (infinite)
            {
                queue.Enqueue(person);
            }

            return person;
        }

        public int Length => queue.Count;
    }
}

// Define the test class in a separate namespace
namespace YourNamespace.Tests
{
    [TestClass]
    public class TakingTurnsQueueTests
    {
        [TestMethod]
        // Scenario: Create a queue with the following people and turns: Bob (2), Tim (5), Sue (3) and
        // run until the queue is empty
        // Expected Result: Bob, Tim, Sue, Bob, Tim, Sue, Tim, Sue, Tim, Tim
        public void TestTakingTurnsQueue_FiniteRepetition()
        {
            var bob = new YourNamespace.Person("Bob", 2);
            var tim = new YourNamespace.Person("Tim", 5);
            var sue = new YourNamespace.Person("Sue", 3);

            Person[] expectedResult = { bob, tim, sue, bob, tim, sue, tim, sue, tim, tim };

            var players = new YourNamespace.TakingTurnsQueue();
            players.AddPerson(bob.Name, bob.Turns);
            players.AddPerson(tim.Name, tim.Turns);
            players.AddPerson(sue.Name, sue.Turns);

            int i = 0;
            while (players.Length > 0)
            {
                if (i >= expectedResult.Length)
                {
                    Assert.Fail("Queue should have ran out of items by now.");
                }

                var person = players.GetNextPerson();
                Assert.AreEqual(expectedResult[i].Name, person.Name);
                i++;
            }
        }

        [TestMethod]
        // Scenario: Create a queue with the following people and turns: Bob (2), Tim (5), Sue (3)
        // After running 5 times, add George with 3 turns. Run until the queue is empty.
        // Expected Result: Bob, Tim, Sue, Bob, Tim, Sue, Tim, George, Sue, Tim, George, Tim, George
        public void TestTakingTurnsQueue_AddPlayerMidway()
        {
            var bob = new YourNamespace.Person("Bob", 2);
            var tim = new YourNamespace.Person("Tim", 5);
            var sue = new YourNamespace.Person("Sue", 3);
            var george = new YourNamespace.Person("George", 3);

            Person[] expectedResult = { bob, tim, sue, bob, tim, sue, tim, george, sue, tim, george, tim, george };

            var players = new YourNamespace.TakingTurnsQueue();
            players.AddPerson(bob.Name, bob.Turns);
            players.AddPerson(tim.Name, tim.Turns);
            players.AddPerson(sue.Name, sue.Turns);

            int i = 0;
            for (; i < 5; i++)
            {
                var person = players.GetNextPerson();
                Assert.AreEqual(expectedResult[i].Name, person.Name);
            }

            players.AddPerson("George", 3);

            while (players.Length > 0)
            {
                if (i >= expectedResult.Length)
                {
                    Assert.Fail("Queue should have ran out of items by now.");
                }

                var person = players.GetNextPerson();
                Assert.AreEqual(expectedResult[i].Name, person.Name);

                i++;
            }
        }

        [TestMethod]
        // Scenario: Create a queue with the following people and turns: Bob (2), Tim (Forever), Sue (3)
        // Run 10 times.
        // Expected Result: Bob, Tim, Sue, Bob, Tim, Sue, Tim, Sue, Tim, Tim
        public void TestTakingTurnsQueue_ForeverZero()
        {
            var timTurns = 0;

            var bob = new YourNamespace.Person("Bob", 2);
            var tim = new YourNamespace.Person("Tim", timTurns);
            var sue = new YourNamespace.Person("Sue", 3);

            Person[] expectedResult = { bob, tim, sue, bob, tim, sue, tim, sue, tim, tim };

            var players = new YourNamespace.TakingTurnsQueue();
            players.AddPerson(bob.Name, bob.Turns);
            players.AddPerson(tim.Name, tim.Turns);
            players.AddPerson(sue.Name, sue.Turns);

            for (int i = 0; i < 10; i++)
            {
                var person = players.GetNextPerson();
                Assert.AreEqual(expectedResult[i].Name, person.Name);
            }

            // Verify that the people with infinite turns really do have infinite turns.
            var infinitePerson = players.GetNextPerson();
            Assert.AreEqual(timTurns, infinitePerson.Turns, "People with infinite turns should not have their turns parameter modified to a very big number. A very big number is not infinite.");
        }

        [TestMethod]
        // Scenario: Create a queue with the following people and turns: Tim (Forever), Sue (3)
        // Run 10 times.
        // Expected Result: Tim, Sue, Tim, Sue, Tim, Sue, Tim, Tim, Tim, Tim
        public void TestTakingTurnsQueue_ForeverNegative()
        {
            var timTurns = -3;
            var tim = new YourNamespace.Person("Tim", timTurns);
            var sue = new YourNamespace.Person("Sue", 3);

            Person[] expectedResult = { tim, sue, tim, sue, tim, sue, tim, tim, tim, tim };

            var players = new YourNamespace.TakingTurnsQueue();
            players.AddPerson(tim.Name, tim.Turns);
            players.AddPerson(sue.Name, sue.Turns);

            for (int i = 0; i < 10; i++)
            {
                var person = players.GetNextPerson();
                Assert.AreEqual(expectedResult[i].Name, person.Name);
            }

            // Verify that the people with infinite turns really do have infinite turns.
            var infinitePerson = players.GetNextPerson();
            Assert.AreEqual(timTurns, infinitePerson.Turns, "People with infinite turns should not have their turns parameter modified to a very big number. A very big number is not infinite.");
        }

        [TestMethod]
        // Scenario: Try to get the next person from an empty queue
        // Expected Result: Exception should be thrown with appropriate error message.
        public void TestTakingTurnsQueue_Empty()
        {
            var players = new YourNamespace.TakingTurnsQueue();

            try
            {
                players.GetNextPerson();
                Assert.Fail("Exception should have been thrown.");
            }
            catch (InvalidOperationException e)
            {
                Assert.AreEqual("No one in the queue.", e.Message);
            }
            catch (AssertFailedException)
            {
                throw;
            }
            catch (Exception e)
            {
                Assert.Fail(
                     string.Format("Unexpected exception of type {0} caught: {1}",
                                    e.GetType(), e.Message)
                );
            }
        }
    }
}