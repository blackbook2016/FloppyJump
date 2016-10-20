Manipulatr - Audio Analyzer script package
Author: Teppo "itekaappine" Kauppinen
Year: 2014-2016

This package contains few scripts and prefabs that help you to create elements that react to music to your game. If you have always wanted to make a rhythm game, this is just what you have been looking for to get you started!

Included in this package are:
- AudioAnalyzer
- Frequency based spawnpoint
- Loudness based spawnpoint
- Object transform manipulator
- Light dimmer

Basic usage:
- Put AudioAnalyzer prefab to your scene
- Configure audio file used for input or use microphone input
- Set desired sample rate, bit depth and resolution. Note that with higher sample rate, the precision on lower frequencies suffer.
- Drag spawnpoints or other prefabs to your scene and attach a script that utilizes parameters provided by AudioAnalyzer to them
- Enjoy and tweak - tweak and enjoy!

Known limitations:
- Audio files needs to be set as Uncompress on load
- Sample rate for mic input and/or audio file must be 11025, 22050 or 44100 

There is a tutorial series about the basic inner workings of this plugin at http://kaappine.fi, however the Unity3D specific tutorials are a bit outdated due to recen changes in Unity 5.3 - they still apply to Unity 4.X and 5.0. Theory behind the functionality is still valid though.

Music included in the demo was made by me under the pseudonym eimink for Alternative Party 2010 Free Music Compo. You can check out my other tunes on soundcloud at https://soundcloud.com/eimink

This package uses Exocortex DSP C# library for Fast Fourier Transform
