# ALTERIT

## A 2D multiplatform ability-based game

<img src="../readme-assets/alterit.gif" width="200"/>

[Full demo on Youtube](https://www.youtube.com/watch?v=LH0LfCJr0ig)

A multiplatform 2D game demo centered around ability-based mechanics, showcasing sophisticated underlying systems. Features include:

* [Custom Touch Controls](game-ALTERIT-demo/Assets/Scripts/PlayerController.cs): Designed for an engaging gameplay experience. While intuitive and responsive, mastering the controls offers a rewarding challenge. Full keyboard support is also available for PC gameplay.
* [Automatic Level Builder](game-ALTERIT-demo/Assets/Scripts/WallCrafter.cs): This system dynamically constructs levels using a designated set of anchors, allowing for diverse map layouts and supporting a variety of art styles.
* Numerous code-animated graphical effects: [Player animator](game-ALTERIT-demo/Assets/Scripts/PlayerAnimator.cs), [smooth camera tracking](game-ALTERIT-demo/Assets/Scripts/CameraTracking.cs), a [lever finisher](game-ALTERIT-demo/Assets/Scripts/LevelEnder.cs), [level component rotator](game-ALTERIT-demo/Assets/Scripts/StageRotator.cs) and space-themed dynamic background (featuring [parallax](game-ALTERIT-demo/Assets/Scripts/Parallaxer.cs) for depth and [dynamic twinkling effects](game-ALTERIT-demo/Assets/Scripts/StarTwinkler.cs) for [randomly generated stars](game-ALTERIT-demo/Assets/Scripts/StarLord.cs).
* Minimalist, fully functional UI: Offering a level selection tool, stats, high scores and other settings.
* [Camera and effects synchronization system](game-ALTERIT-demo/Assets/Scripts/FXController.cs): A prototype system that makes the camera bounce in sync with the music's tempo and modulates other effects accordingly.
