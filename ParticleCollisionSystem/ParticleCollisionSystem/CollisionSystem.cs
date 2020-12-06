using System;
using System.Collections.Generic;
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


    }
}
