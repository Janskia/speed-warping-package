# Speed Warping #

### About ###

It's a little plugin that allows you to dynamically stretch step length in humanoid movement animations. You can use single running animation and change step length dynamically from the code. With this technique you can easily implement character movement with whole range of speeds (e.g. movement for gamepad) without need of speeding up or slowing down the animation (what would result in ridiculously fast or slow steps).

![Alt text](/Documentation/Preview2.gif?raw=true)

### How it works ###

Feet and hand positions are grabbed from the animation, modified according to `SpeedWarping.Spread` parameter and updated position are applied it to the model using IK. Faster movement will result in longer steps and slower in shorter steps. 

### Usage guide ###

1. Import the package via Package Manager using git URL: https://github.com/Janskia/speed-warping-package.git
2. Setup your character model's Rig Animation Type to 'Humanoid'.

![Alt text](/Documentation/AnimationType.png?raw=true)

3. Attach `SpeedWarping` component to the root game object of your character (this game object should also have `Animator` component on it).
4. Verify if model parts transforms were automatically set up properly (it requires correct avatar set up in `Animator`). If it didn't work for some reason, you'll have to attach model parts transforms manually.

![Alt text](/Documentation/SpeedWarpingComponent.png?raw=true)

5. Update desired movement speed using `SpeedWarping.Spread` property from your code.
[optional] 6. Set custom `LegsOrigin` (explained below)

### Custom Legs Origin ###

LegsOrigin is center point around which legs rotate (hips), used in calculations of final feet destination. By default legs origin is set to hips transform of the model. This seems correct, however if position of this point is slightly ofset, you can alter legs shape in final pose and likely get nicer results.

In order to do this:
1. Add empty game object.
2. Place it around hips.
3. Tweak its x and y position - this will change the shape of the legs when they're spread. You can for example make back leg more straight and front leg more bent. Usually position slightly below and behind hips should work fine.
