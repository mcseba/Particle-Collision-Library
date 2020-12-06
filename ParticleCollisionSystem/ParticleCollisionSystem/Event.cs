using System;
using System.Collections.Generic;
using System.Text;

namespace ParticleCollisionSystem
{
    public class Event : IComparable<Event>
    {
        /// <summary>
        /// Time that event is scheduled to occur
        /// </summary>
        public readonly double time;
        /// <summary>
        /// Particles involved in event
        /// </summary>
        public Particle a, b;
        /// <summary>
        /// Collision counts at event creation
        /// </summary>
        public readonly int countA, countB;
        /// <summary>
        /// Creates new Event
        /// </summary>
        /// <param name="time">Time at which event occurs</param>
        /// <param name="a">Particle A involved in event</param>
        /// <param name="b">Particle B involved in event</param>
        public Event(double time, Particle a, Particle b)
        {
            this.a = a;
            this.b = b;
            this.time = time;

            if (a != null) countA = a.Count;
            else countA = -1;

            if (b != null) countB = b.Count;
            else countB = -1;
        }

        public Event(Event e)
        {
            if (e != null)
            {
                this.a = e.a;
                this.b = e.b;
                this.time = e.time;
                this.countA = e.countA;
                this.countB = e.countB;
            }
        }

        public int CompareTo(Event other)
        {
            return this.time.CompareTo(other.time);
        }
        /// <summary>
        /// Checks if any event occured between when event was created and now
        /// </summary>
        /// <returns>Bool</returns>
        public bool IsValid()
        {
            if (a != null && a.Count != this.countA) return false;
            if (b != null && b.Count != this.countB) return false;
            return true;
        }
    }
}
