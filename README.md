# Speed Warping #

### About ###

It's a little plugin that allows you to dynamically stretch step length in humanoid movement animations. With this technique you can easily implement character movement with whole range of speeds (e.g. movement for gamepad) without need of speeding up or slowing down the animation (what would result in ridiculously fast or slow steps). Faster movement will simply result in longer steps and slower in shorter steps.

![Alt text](/Documentation~/Preview3.gif?raw=true)

### How it works ###

Feet and hand positions are grabbed from the animation, modified according to `SpeedWarping.Spread` and applied back to the model using IK.

### Usage guide ###

1. Import the package via Package Manager using git URL: https://github.com/Janskia/speed-warping-package.git
2. Setup your character model's Rig Animation Type to 'Humanoid'.
3. Setup `Animator` on the character's game object. Attach avatar. Have some running/walking animation.
4. Enable `IK Pass` in animator's layer settings.
5. Attach `SpeedWarping` component to the game object with `Animator`.
6. Verify if model parts transforms were automatically set up properly. If it didn't work for some reason, you'll have to attach model parts transforms manually.

![Alt text](/Documentation~/SpeedWarpingComponent.png?raw=true)

6. Update desired movement speed using `SpeedWarping.Spread` from your code.
7. [optional] Set custom `LegsOrigin` (explained below)

### Custom Legs Origin ###

`LegsOrigin` is center point around which legs rotate used in calculations of final feet destination. By default legs origin is set to hips transform of the model. This seems correct, however if position of this point is slightly ofset, you can alter front and back leg shape in final pose and likely get nicer results.

In order to do this:
1. Add empty game object.
2. Place it around hips. If you don't make it child of hips it won't be affects by hips animation movement and you'll likely get better results.
3. Tweak its x and y position - this will change the shape of the legs when they're spread. You can for example make back leg more straight and front leg more bent. Position slightly below and behind hips should work fine.
