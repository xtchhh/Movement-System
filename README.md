# 3rd Person Movement System

This is my first 3rd person movement system made in Unity Engine. It features camera-relative movement, a simple animation system, and custom gravity.

## Player Input

I used Unity's new input system for player input. The move action provides a `Vector2` representing the X and Y input from the player.

## Camera-Relative Movement

I created camera-relative movement for the movement system, which is common camera logic used in a lot of 3rd person games. The core functionality works by taking the X and Y input of the move action and performing scalar multiplication against the forward and right directions of the camera's transform. This binds direction to player input and converts it into a `Vector3`, which I named `forwardRelativeInput` and `rightRelativeInput`. I could go off on a tangent explaining this camera logic further, but I feel that's a strong fundamental part of it.

After all of this, we get a definitive movement vector. I multiply this vector by `walkSpeed` and `Time.deltaTime` for consistent movement. In `Update()`, the system is constantly awaiting player input and moving in the direction based upon that input.

My two biggest challenges creating this type of camera were understanding the difference between local and global space, and understanding what the magnitude (length) of a vector is.

## Animation

I also implemented a simple animation system. I used the Animator (it is a mess, haha) and used `Animator.SetBool` and `Animator.SetTrigger` as declarations when the player presses Space or WASD. The animations incorrectly transition often. For example when I am running and stop completely in my tracks, the walk animation is briefly played while the player is idle, and then the "idle" animation loops as long as the player is stationary. This could be at the fault of how my animator is configured or how I implemented my logic in the sprint action method.

## Custom Gravity & Jumping

My third challenge, which was arguably harder than the first two, was creating my own custom gravity. I basically had to touch up on some high school level physics, which was quite humbling. This is another thing I could go a whole tangent on, but the core part of the gravity system was understanding that velocity is the rate of change of an object's position.

The jump and gravity logic go hand in hand. In the `Jump()` method, I implemented a `transform.Translate` function that exists outside of the `if` statement that checks whether the player is grounded. This `transform.Translate` takes a `new Vector3` with its X and Z values set to 0 and the Y value set to a `velocity` float variable. A good responsive jump comes with a good ground check, so I created my own boolean method that uses a spherecast, I find this to be a better use case than a raycast as it is very linear and checks less ground.

When the player isn't jumping, velocity is zero - so despite this being called every frame (because `Jump()` is called in `Update()`), if the player isn't grounded and isn't pressing Space, the translate is effectively zero. When the player IS grounded and presses Space, the `if` statement sets `velocity = jumpSpeed` (a defined private variable at the top of the class). So `transform.Translate` becomes `new Vector3(0, 5, 0)` (where 5 is the `jumpSpeed` value). This remains true until the player lands on the ground again and velocity resets to 0. I wanted to simulate my own gravity to further understand how Unity's Rigidbody works, which is also a product of understanding Newton's three basic laws.

The gravity ties it all together: I created a conditional where if the player isn't grounded, gravity is added (`+=`) onto velocity, multiplied by `Time.deltaTime` for consistency. If the player is grounded, velocity equals zero, so the translate vector is just `(0, 0, 0)`.

## My Assessment
There is definitely a better way to explain all of this, but this is how I articulate it as of now. I'm sure as I progress as a programmer I'll be better spoken.
