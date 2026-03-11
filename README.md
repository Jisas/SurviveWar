# 🛡️ Survive War | Combat & Systems Integration

![Unity](https://img.shields.io/badge/Unity-2021.3+-black?logo=unity)
![Render Pipeline](https://img.shields.io/badge/URP-Enabled-blue)
![Technical Art](https://img.shields.io/badge/Tech_Art-Compute_Shaders-orange)

**Survive War** is a technical showcase of shooting mechanics and environment systems. This project serves as a testing ground for integrating high-fidelity visual tools with robust gameplay architectures, focusing on performance and modularity.

## 🛠️ Key Technical Features

### 1. Grass System Integration & Tooling
The project features a highly optimized GPU-based grass rendering system. While the core compute shader and rendering logic were created by the amazing **MinionsArt** (see credits), my engineering focus was on the integration:
* **System Integration:** Adapted the GPU Instancing logic to coexist smoothly within the project's rendering pipeline and gameplay loops without causing overhead.

### 2. Combat & Ability System
Building upon a decoupled architecture, the combat in Survive War is highly modular:
* **Modular Inputs:** Clean integration of the *New Input System* to manage complex combat combos and abilities.
* **Mini-game Integration:** Implementation of sub-systems (like shooting minigames) that coexist within the main game loop without creating code dependency.

### 3. Network & Infrastructure
The project includes foundations for networked interactions, exploring:
* **Networked State Synchronization:** Managing game events and player states across a network.
* **Modular Manager Architecture:** A centralized system to handle game flow, UI (Altar UI), and world events efficiently.

## 🔧 Technical Deep Dive

* **Custom Property Drawers:** Optimized inspector windows for managing complex data sets like Altar requirements or Combat stats.
* **Event-Driven UI:** A reactive UI system that minimizes polling, ensuring the interface only updates when the game state changes.
* **Physics Optimization:** Fine-tuned collision layers to handle multiple enemies and projectiles simultaneously without performance drops.

## 🤝 Credits & Acknowledgments

A special thanks to **MinionsArt** for the incredible Interactive Grass system used in this project. 
* **Website:** [MinionsArt Tutorials](https://minionsart.github.io/tutorials/)
* **Grass Shader System:** [Interactive Grass Post](https://minionsart.github.io/tutorials/Posts.html?post=grass_system)
* **YouTube Tutorial:** [Watch on YouTube](https://www.youtube.com/watch?v=2OA9sicjj7E)

## 📂 Project Structure

* **/Scripts/Tools/Grass**: The custom editor and integration logic for the Compute Shader vegetation.
* **/Scripts/CombatSystem**: Core architecture for player and enemy interactions.
* **/Scripts/InputSystem**: Decoupled input handling for multi-platform support.
* **/Scripts/UI**: Modular HUD and interaction menus.

## 👨‍💻 Author
<div aling="left">  
  <h4>Jesús Carrero - Unity Gameplay Engineer</h1>
  <a href="https://jesuscarrero.netlify.app/">
    <img src="https://img.shields.io/badge/Portfolio-a83333?style=for-the-badge&logo=netlify&logoColor=white" width="150" />
  </a>
</div>
