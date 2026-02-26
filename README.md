# Action RPG Adventure : Warrior Quest

#### 🗡️ Warrior Quest
> A serious technical project built with Unity and C# focusing on architecture, systems design, and scalable gameplay implementation.

---

## ⚙️ Technical Highlights
- Engine: Unity 6 (6000.2.9f1)
- Programming Language: C#

---

---

## 🎮 Core Gameplay

* 2D Pixel RPG
* Skill-based combat system
* Dash movement mechanic
* Skill unlock / lock progression
* Multi-path skill tree support

Gameplay is built around structured systems rather than hardcoded logic.

---

## 🏗 Architecture Design

* Heavy Object-Oriented Programming (OOP) structure
* Shared `BaseCharacter` class for Player and Enemy
* Modular system separation
* Clear responsibility division between:

  * Combat logic
  * Movement logic
  * AI logic
  * UI logic

The design emphasizes scalability and maintainability rather than quick implementation.

---

## 🤖 Enemy AI System

* State-machine driven behavior
* Detection using `OverlapSphere` based sensing
* Follow / Chase state
* Attack state
* Retreat / Backup state
* Direction control
* Layered state transitions

The AI is implemented using structured state management instead of large conditional branching.

This makes behavior expansion easier and safer for future updates.

---

## ⚔ Combat Calculation System

* Attack value calculation
* Defense value calculation
* Elemental damage processing
* HP deduction formula based on parameters
* Configurable balance tuning

The combat system is built with parameter-driven calculations so balancing can be adjusted without rewriting core logic.

---

## 🔓 Skill & Progression System

* Skill unlock / lock functionality
* Multi-path skill tree support
* Single-path restriction option
* Dynamic UI path coloring
* Visual feedback when selecting progression routes

I am particularly proud of the skill UI implementation because it dynamically reflects player progression and path selection visually.

---

## 🛠 Custom Unity Tools

* Inspector utility extensions
* Custom editor scripts
* Workflow optimization tools

These tools improve development efficiency and reduce repetitive setup tasks.

---

## 🧠 Technical Takeaways

* Designing scalable gameplay architecture
* Building AI with state machines
* Structuring large OOP-based systems
* Implementing reusable mechanics
* Managing system complexity as project size grows

---

## 🔮 Future Improvements

* Performance optimization
* Additional enemy behavior states
* Expanded skill tree depth
* Visual polish and feedback improvements
* Refactoring for further modularity

---

*Built in Unity using C# as a serious system engineering practice project.*
