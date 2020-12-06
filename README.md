# Particle Collision C# Library
C# library for event-driven simulation of N colliding particles
###### Developed on the basis of Java implementation from [Algorithms, 4th Edition by Robert Sedwick and Kevin Wayne](https://algs4.cs.princeton.edu/61event/)

1.Main goal of this library is to provide C# methods for **simulating the motion of N colliding particles according** to the laws of elastic collision using event-driven simulation.
These simulations are widely used in various fields of life i.e.:
- Computer graphics
- Computer games
- Molecular dynamics
- Robotics

2. This simulation needs to meet model assumption in order to give a proper output data

- Hard sphere model
```
> This is an idealized model of the motion of atoms or molecules in a container.
The properties of this model are:
- N particles in motion
- Particle *i* has position (rx, ry), velocity (vx, vy), mass *m* and radius *σ*
- Particles interact with elastic collisions with each other
- There are no other forces exerted on the particles
```
