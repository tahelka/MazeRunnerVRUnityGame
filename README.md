# Maze Runner VR for Oculus Quest 2
# VR Maze Explorer

## About
**VR Maze Explorer** is a fully immersive virtual reality game built with Unity. It challenges players to navigate procedurally generated mazes, avoid or defeat dynamic enemies, and make it to the end as fast as possible. It features difficulty scaling, dynamic obstacle placement, and an in-game leaderboard system to keep track of top performers.

## Features
- 🧩 Procedurally generated 3D mazes using DFS algorithm
- 🕷️ Dynamic enemy spawning with NavMesh pathfinding
- ⚔️ Combat system with hit detection, slicing mechanics, and damage animations
- 🧠 AI-controlled enemies that seek the player
- 🕒 In-game timer and health systems
- 🥇 Score-based leaderboard for Easy, Medium, and Hard levels
- 🎮 VR integration with hand animation, gripping, and locomotion settings
- 🔇 Audio and sound effects triggered by user interactions and physics
- ⛳ Different maze endings with win/loss/quit outcomes

## Technology Stack
- **Unity** (URP, XR Interaction Toolkit)
- **C#** for game logic
- **NavMesh** for AI pathfinding
- **TextMeshPro** for UI text rendering
- **EzySlice** for in-game slicing mechanics
- **XR Toolkit** for VR interactions (Oculus, etc.)

## Structure
- `GameManager.cs` – Handles game state and flow
- `MazeGenerator.cs` – Builds maze logic, nodes, and obstacles
- `EnemiesSpawnerManager.cs` – Controls enemy pool and spawn behavior
- `HealthManager.cs`, `Timer.cs` – Handles player health and game timing
- `LeaderboardManager.cs` – Stores and displays scores
- `SliceObject.cs`, `ToolSoundController.cs` – Adds physics-based slicing and sound interaction
- `NavMeshBaker.cs` – Dynamically bakes NavMeshes on runtime-generated maze tiles

## Getting Started
1. Clone the repository into your Unity project directory.
2. Open in Unity (recommended Unity version: 2022.3+ with XR Toolkit support).
3. Connect and configure your VR headset.
4. Play the scene and select a difficulty level.
5. Navigate through the maze and reach the goal before the enemies do!

To play the game on your Oculus Quest 2 or in XR Device Simulator on your PC, follow the steps below.


### Playing on Oculus Quest 2

1. **Oculus Desktop App**: 
   - Open the Oculus desktop app on your PC.

2. **Connect Your Oculus Quest 2**:
   - Via Air Link: Ensure your Oculus Quest 2 and your PC are on the same Wi-Fi network. Use Air Link to connect your Quest 2 to your PC wirelessly.
   - Using Link Cable: Connect your Oculus Quest 2 to your PC using a Link cable.

3. **Activate Quest Link**:
   - On your Oculus Quest 2, navigate to the Quest Link settings and activate it.

4. **Launch the Game on Unity**:
   - ensure that the `XR Device Simulator` GameObject is **disabled**.
   - Start the game within the Unity editor.
   - Put on your Oculus Quest 2 headset and play the game.


### Playing with XR Device Simulator (PC)

1. **Unity**: 
   - Open the Unity project.

2. **XR Device Simulator GameObject**:
   - To play the game in XR Device Simulator on your PC, ensure that the `XR Device Simulator` GameObject is **enabled**. You can find this GameObject in your 
     Unity hierarchy.
   - The controls will be shown in the game on the bottom left side of the screen.

3. **Start the Game**: 
   - Start the game within the Unity editor.

Have fun playing the game and enjoy your VR experience! If you encounter any issues or have feedback, feel free to reach out for support.

## License
This project is provided for educational and prototyping purposes. Commercial use is not permitted without explicit permission from the authors.

---

Made with ❤️ and a bit of frustration while debugging Unity NavMesh.
