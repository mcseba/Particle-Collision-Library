using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Text;

namespace ParticleCollisionSystem
{
    public class Particle
    {
        #region props
        private static readonly double INFINITY = Double.PositiveInfinity;

        /// <summary>
        /// Position x,y
        /// </summary>
        private double rx, ry;

        /// <summary>
        /// Velocity of the particle
        /// </summary>
        private double vx, vy;

        private int count;

        /// <summary>
        /// Radius of the particle
        /// </summary>
        private readonly double radius;

        /// <summary>
        /// Particle's mass
        /// </summary>
        private readonly double mass;

        /// <summary>
        /// Particle's color
        /// </summary>
        private readonly Color color;

        /// <summary>
        /// Number of collisions so far.
        /// Includes collisions with horizontal and vertical wall and with other particles.
        /// </summary>
        public int Count
        {
            get => count;
        }
        #endregion

        #region ctors
        public Particle(double rx, double ry, double vx, double vy, double radius, double mass, Color color)
        {
            this.rx = rx;
            this.ry = ry;
            this.vx = vx;
            this.vy = vy;
            this.radius = radius;
            this.mass = mass;
            this.color = color;
        }

        /// <summary>
        /// Particle data line read from text file.
        /// String text must fit proper format: RX RY VX VY RADIUS MASS R G B
        /// </summary>
        /// <param name="particleData"></param>
        public Particle(string particleData)
        {
            string[] arr = particleData.Split(' ');

            this.rx = double.Parse(arr[0], CultureInfo.InvariantCulture);
            this.ry = double.Parse(arr[1], CultureInfo.InvariantCulture);
            this.vx = double.Parse(arr[2], CultureInfo.InvariantCulture);
            this.vy = double.Parse(arr[3], CultureInfo.InvariantCulture);
            this.radius = double.Parse(arr[4], CultureInfo.InvariantCulture);
            this.mass = double.Parse(arr[5], CultureInfo.InvariantCulture);
            this.color = Color.FromArgb(int.Parse(arr[6]), int.Parse(arr[7]), int.Parse(arr[8]));
        }

        public Particle()
        {
            rx = GenerateRandom(0.0, 1.0);
            ry = GenerateRandom(0.0, 1.0);
            vx = GenerateRandom(-0.005, 0.005);
            vy = GenerateRandom(-0.005, 0.005);
            radius = 0.02;
            mass = 0.5;
            color = Color.Black;
        }
        #endregion

        /// <summary>
        /// Moves particle in a straight line for a specified amount of time.
        /// </summary>
        /// <param name="dt">Time</param>
        public void Move(double dt)
        {
            rx += vx * dt;
            ry += vy * dt;
        }

        /// <summary>
        /// Returns amount of time for this particle to collide with other particle.
        /// </summary>
        /// <param name="other">The other particle</param>
        /// <returns></returns>
        public double TimeToCollision(Particle other)
        {
            if (this == other) return INFINITY;
            double dx = other.rx - this.rx;
            double dy = other.ry - this.ry;
            double dvx = other.vx - this.vx;
            double dvy = other.vy - this.vy;
            double dvdr = dx * dvx + dy * dvy;
            if (dvdr > 0) return INFINITY;
            double dvdv = dvx * dvx + dvy * dvy;
            if (dvdv == 0) return INFINITY;
            double drdr = dx * dx + dy * dy;
            double sigma = this.radius + other.radius;
            double d = (dvdr * dvdr) - dvdv * (drdr - sigma * sigma);
            if (d < 0) return INFINITY;
            return -(dvdr + Math.Sqrt(d)) / dvdv;
        }

        /// <summary>
        /// Counts time till this particle collision with vertical wall.
        /// </summary>
        /// <returns>Time to hit vertical wall</returns>
        public double TimeToHitVerticalWall()
        {
            if (vx > 0) return (1.0 - rx - radius) / vx;
            else if (vx < 0) return (radius - rx) / vx;
            else return INFINITY;
        }

        /// <summary>
        /// Count time till this particle collision with horizontal wall.
        /// </summary>
        /// <returns>Time to hit horizontal wall</returns>
        public double TimeToHitHorizontalWall()
        {
            if (vy > 0) return (1.0 - ry - radius) / vy;
            else if (vy < 0) return (radius - ry) / vy;
            else return INFINITY;
        }

        /// <summary>
        /// Behaviour of a particle after collision according to the laws of elastic collision.
        /// </summary>
        /// <param name="other">Other particle to collide with.</param>
        public void BounceOff(Particle other)
        {
            double dx = other.rx - this.rx;
            double dy = other.ry - this.ry;
            double dvx = other.vx - this.vx;
            double dvy = other.vy - this.vy;
            double dvdr = dx * dvx + dy * dvy;
            double dist = this.radius + other.radius;

            // normal force
            double magnitude = 2 * this.mass * other.mass * dvdr / ((this.mass + other.mass) * dist);

            // impulse due to the normal force in the x and y directions
            double fx = magnitude * dx / dist;
            double fy = magnitude * dy / dist;

            // update velocities according to the normal force
            this.vx += fx / this.mass;
            this.vy += fy / this.mass;
            other.vx -= fx / other.mass;
            other.vy -= fy / other.mass;

            // increment number of collisions
            this.count++;
            other.count++;
        }

        /// <summary>
        /// Velocity of the particle after hitting vertical wall.
        /// </summary>
        public void BounceOffVerticalWall()
        {
            vx = -vx;
            count++;
        }

        /// <summary>
        /// Velocity of the particle after hitting horizontal wall.
        /// </summary>
        public void BounceOffHorizontalWall()
        {
            vy = -vy;
            count++;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>The kinetic energy of this particle</returns>
        public double KineticEnergy()
        {
            return 0.5 * mass * (vx * vx + vy * vy);
        }

        /// <summary>
        /// Helper function. Generates random number in specified range
        /// </summary>
        /// <param name="x">Left endpoint of the range</param>
        /// <param name="y">Right endpoint of the range</param>
        /// <returns>Double random</returns>
        public double GenerateRandom(double x, double y)
        {
            Random rd = new Random();
            return x + rd.NextDouble() * (y - x);
        }
    }
}
