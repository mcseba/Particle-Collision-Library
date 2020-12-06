using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ParticleCollisionSystem
{
    class CollisionSystem
    {
        #region props
        /// <summary>
        /// Number of event redraws per clock tick
        /// </summary>
        private static readonly double HZ = 0.5;
        /// <summary>
        /// Priority Queue of events
        /// </summary>
        private PriorityQueue<Event> pq;
        /// <summary>
        /// simulation time
        /// </summary>
        private double t = 0.0;
        /// <summary>
        /// Array of particles
        /// </summary>
        private Particle[] particles;
        #endregion

        #region ctors
        /// <summary>
        /// Initializes collision system with specific particles.
        /// </summary>
        /// <param name="particles">Array of particles</param>
        public CollisionSystem(Particle[] particles)
        {
            if (particles != null)
                this.particles = particles;
        }

        /// <summary>
        /// Reads particles data from path. File must containt particle data in standard text format.
        /// </summary>
        /// <param name="path">Path to the data file</param>
        public CollisionSystem(string path)
        {
            using (var reader = new StreamReader(path))
            {
                int count = int.Parse(reader.ReadLine());
                List<Particle> partcls = new List<Particle>(count);

                for (int i = 0; i < count; i++)
                {
                    string particleData = reader.ReadLine();
                    Particle particle = new Particle(particleData);
                    partcls.Add(particle);
                }

                this.particles = partcls.ToArray();

            }
        }
        #endregion

        /// <summary>
        /// Updates priority queue if any new events for particle a will occur
        /// </summary>
        /// <param name="a">Predict collisions for this particle</param>
        /// <param name="limit">Time for simulation</param>
        private void Predict(Particle a, double limit)
        {
            if (a == null) return;

            for (int i = 0; i < particles.Length; i++)
            {
                double dt = a.TimeToCollision(particles[i]);
                if (t + dt <= limit)
                    pq.Enqueue(t + dt, new Event(t + dt, a, particles[i]));
            }

            double dtX = a.TimeToHitVerticalWall();
            double dtY = a.TimeToHitHorizontalWall();
            if (t + dtX <= limit) pq.Enqueue(t + dtX, new Event(t + dtX, a, null));
            if (t + dtY <= limit) pq.Enqueue(t + dtY, new Event(t + dtY, null, a));
        }

        public void Simulate(double limit)
        {
            pq = new PriorityQueue<Event>();
            for (int i = 0; i < particles.Length; i++)
            {
                Predict(particles[i], limit);
            }
            pq.Enqueue(0, new Event(0, null, null));

            while (!pq.IsEmpty())
            {
                Event e = new Event(pq.Dequeue());
                if (!e.IsValid())
                    continue;
                Particle a = e.a;
                Particle b = e.b;

                for (int i = 0; i < particles.Length; i++)
                {
                    particles[i].Move(e.time - t);
                }
                t = e.time;

                if (a != null && b != null) a.BounceOff(b);
                else if (a != null && b == null) a.BounceOffVerticalWall();
                else if (a == null && b != null) b.BounceOffHorizontalWall();

                Predict(a, limit);
                Predict(b, limit);
            }
        }

    }
}
