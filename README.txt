Physics Demonstration Project

By Andrew Spencer 2018

This project demonstrates the use of physics systems in Unity.

A git repository for the project can be found at https://github.com/LASpencer/AIE-Physics-Demo

====USE====

In this game the player controls a robot who can shoot at skeletons with a laser.

Use wasd or direction keys to move
Click on a skeleton to fire a laser
Left Control to Crouch
Spacebar to Jump
Esc to quit

Moving the mouse near the edges of the screen to rotate the camera


There is a teleporter pad, with blue energy around it. Walk onto the pad to teleport to the exit.


====DESIGN====

This project was made to demonstrate the use of several advanced physics systems.

The moving platforms use Sliding Joints to move along their paths.

The Skeletons use ragdolling to collapse when killed. They can be pushed around by walking into them or shooting them again

A raycast from the camera is used to detect where the player has clicked, and then from the robot to that point to determine where the laser hits

The teleporter uses a trigger to detect when the player walks into it

The player is controlled with a custom character controller class, called MovingCharacter. This applies forces to a rigidbody based on the intended 
velocity given to it, moving along the surface it's placed on

On hitting a skeleton, particles are produced for sparks. These particles can collide with the walls and ground

The robot character has a cape attached to it. The cape uses Unity's cloth component. To avoid it passing through the player, several colliders have been
added to its body and limbs which are set to collide with the cape.