## Welcome to ProjectClub

### Created by: Alberto Scicali and Brian Mansfield
### Class: CSCI-711 Global Illumination

## Proposal

### Summary
- Construct a 3D Scene/Environment
- 10-100 objects in the scene generated via music frequencies
- Objects consist of multi-colored emissive light blocks and various shapes/objects/fractals that will change along with the music.

### Goals
1. Demonstrate an aspect of interactivity, producing a constantly changing scene based on a user inputted song.
2. Demonstrate interactivity with the user being able to change the materials/lighting parameters involved in the scene.
3. Demonstrate an extensive modification to the existing ray tracer project, adding a component to consume and process a given audio file and input that data into a 3D rendering pipeline.
4.  Demonstrate the implementation of emissive light sources and global reflectivity (Emissive objects with different colored lights will reflect off each others' surfaces and reflective floor surfaces will demonstrate reflection and blending of multiple light sources).
5.  Implementation of a module to consume and process audio files into a usable format.
6.  Implementation of an algorithm to generate objects and scenes using audio data.
7.  Implementation of a real time camera control system to allowed user interactivity and exploration with the scene.

### System & Software
- MacOS & Windows 10
- IDE Xcode and Visual Studio
- OpenGL & freeGLUT
- Essentia - Component to analyze music data
	- http://essentia.upf.edu/
- Trello for project management
- C++

### Project Components
- Main project Directory to maintain the program
- Audio analyzer module
- Object/scene generator module

### Project Responsibilities
- **Alberto**
	- Responsible for researching, designing and implementing the audio frequency analyzer module
- **Brian**
	- Responsible for designing and implementing the object generating manage module
- **Both**
	- Responsible for connecting both modules together, designing and implementing the 3D framework and UI

### Milestones
- Compile a list of all resources needed for the project
- Design UI
- Create/Reuse a 3D Object scene rendering program using OpenGL
- Produce a static scene containing a sample (arbitrary) emissive object
- Design Audio Frequency Analyzer (AFA) module
- Design Object Generator/Manager for rendering program
- Produce a static scene containing a sample (arbitrary) 3D frequency visualization 
- Design FPS system to make live scenes
- Implement FPS system
- Implement Audio Frequency Analyzer Module
 Hook AFA module into rendering program
- Design Protocol to transfer frequency data to rendering program for object generation
- Implement protocol
- Run tests, clean up code
- Create example for presentation
- Document final algorithms, modules, and important code for presentation

### Final presentation
- Introduction - explain the project and it’s goals
- Live Example of program
- Rundown of modules used in program
- Algorithms used for each module/step of program
- Problems encountered during the project
- Potential extensions/fixes that could be made to projects
