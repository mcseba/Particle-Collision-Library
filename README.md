# Particle Collision C# Library
## C# library for event-driven simulation of N colliding particles
###### Developed on the basis of Java implementation from [Algorithms, 4th Edition by Robert Sedwick and Kevin Wayne](https://algs4.cs.princeton.edu/61event/)

**1.** Main goal of this library is to provide C# methods for **simulating the motion of N colliding particles** according to the laws of elastic collision using event-driven simulation.
  These simulations are widely used in various fields of life i.e.:
  - Computer graphics
  - Computer games
  - Molecular dynamics
  - Robotics

**2. This simulation needs to meet model assumption in order to give a proper output data.**
  ***- Hard sphere model***
    ```
    This is an idealized model of the motion of atoms or molecules in a container.
    The properties of this model are:
    - N particles in motion
    - Particle *i* has position (rx, ry), velocity (vx, vy), mass *m* and radius *σ*
    - Particles interact with elastic collisions with each other
    - There are no other forces exerted on the particles
    ```

**3. Simulation.**

  This is an event-driven simulation. All particles move in a straight line at constant speed so we can predict at what time the two particles will collide. 
  Thus, these collision events are added to the **priority queue** of future event ordered by time. As particles collide and change direction, some of the events may   become invalidated and priority queue will update it's events to respond to the actual state of particles.

**4. Implementation.**
  - ***MinPriorityQueue*** - generic data structure of type Event allowing to order collision events by time
  - ***Particle*** - data type, represents single particle moving in the unit box
  - ***CollisionSystem*** - provides methods for simulating *N particles* in a box for a *t* time
