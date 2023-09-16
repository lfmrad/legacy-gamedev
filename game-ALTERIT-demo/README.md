# ALTERIT

## A 2D multiplatform ability-based game

***Disclaimer: This project was undertaken as a technical exercise in the past. As such, <span style="color:red">the code may not reflect my current level of expertise or best practices.</span>***

<img src="../readme-assets/alterit.gif" width="200"/>

[Full demo on Youtube](https://www.youtube.com/watch?v=LH0LfCJr0ig)

An advanced multiplatform 2D game demo centered around ability-based mechanics, showcasing a variety of sophisticated underlying systems. Features include:

* [Custom Touch Controls](Assets/Scripts/PlayerController.cs): Designed for an engaging gameplay experience. While intuitive and responsive, mastering the controls offers a rewarding challenge. Full keyboard support is also available for PC gameplay.
* [Automatic Level Builder](Assets/Scripts/WallCrafter.cs): This system dynamically constructs levels using a designated set of anchors, allowing for diverse map layouts and supporting a variety of art styles.
* Numerous code-animated graphical effects: [Player animator](Assets/Scripts/PlayerAnimator.cs), [smooth camera tracking](game-ALTERIT-demo/Assets/Scripts/CameraTracking.cs), a [lever finisher](Assets/Scripts/LevelEnder.cs), [level component rotator](Assets/Scripts/StageRotator.cs) and space-themed dynamic background (featuring [parallax](Assets/Scripts/Parallaxer.cs) for depth and [dynamic twinkling effects](Assets/Scripts/StarTwinkler.cs) for [randomly generated stars](Assets/Scripts/StarLord.cs)).
* Minimalist, fully functional UI: Offering a level selection tool, stats, high scores and other settings.
* [Camera and effects synchronization system](Assets/Scripts/FXController.cs): A prototype system that makes the camera bounce in sync with the music's tempo and modulates other effects accordingly.
