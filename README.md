# ⚔️ Warrior Quest (2D Pixel RPG System Architecture)

#### ⚔️🛡️ Warrior Quest
A 2D pixel RPG focused heavily on gameplay system architecture and structured OOP design.
Although visually simple, this project contains a deeply structured character system, modular combat calculations, and an extensible skill framework.
This project is being developed as part of advanced Unity architecture study (OOP state systems and AI structure), with a focus on applying learned concepts into scalable gameplay systems.
**Status:** In active development (February 2026).

<img width="854" height="480" alt="image" src="https://github.com/user-attachments/assets/7659c8ca-9ca1-4614-8489-1c01ae126592" />

---

## ⚙️ Technical Highlights
- Engine: Unity 6 (6000.2.9f1)
- Programming Language: C#
- State-driven animation control
- Modular combat systems
- Skill-based ability framework
- Shared character base logic
- Scalable RPG calculations
- Configurable skill progression options for designers

---

## 🎥 Gameplay Video
[Watch Gameplay Video](https://youtu.be/Pw0lexJriPE)

---

## 🧠 Advanced State Machine (OOP Architecture Highlight)

The animation state flow itself is straightforward, but the strength of this project lies in the script structure.

<img width="854" height="480" alt="image" src="https://github.com/user-attachments/assets/b961c4d1-e5e4-4db0-a42f-bda6517baf63" />

- Fully OOP-driven state system
- Clear separation between state logic and animation transitions
- Expandable architecture for adding new states
- Player and enemies operate under structured state management

This design allows flexibility without rewriting core systems.

---

## 🧬 Unified Character Base Class

Both player and enemies inherit from the same base character class.

Shared systems such as:

- Entity_Health
- Entity_Combat
- Entity_Stats
- Entity_VFX (manager)

This ensures scalability, consistency, and easier extension for future characters.

---

## ⚔️ Combat & Damage Calculation System

A parameter-driven combat calculation system including:

- Physical/Elemental attack
- Elemental effects
- Defense scaling
- Elemental attack modifiers
- Damage reduction formulas
- Dynamic HP reduction pipeline

<img width="348" height="679" alt="image" src="https://github.com/user-attachments/assets/ffbdb08b-069e-4511-9218-26b712cd2a7f" />

The damage calculation considers multiple parameters and modifiers to determine final HP loss.

The structure supports future expansion for additional elemental types and status effects.

---

## 🌀 Skill System Framework

- Modular skill-based architecture
- Active movement skill (Dash implemented)
- Lock / Unlock skill logic
- Expandable system for additional abilities

<img width="854" height="480" alt="image" src="https://github.com/user-attachments/assets/2461d8e5-0527-4401-aa48-b6611e20626e" />

Skills are structured to allow clean integration without rewriting the core character logic.

---

## 🌳 Configurable Skill Unlock System (Designer-Friendly)

One of the core highlights of this project is the flexible skill progression system. The skill tree progression mode is configurable via Inspector boolean flag.

### Key Features:

- Configurable unlock mode (Single-path or Multi-path)
- Designer-controlled behavior via Inspector checkbox
- No code changes required to switch progression type
- UI dynamically updates when a skill is unlocked
- Visual connection paths change color based on chosen progression

  - Head unlocked
  <img width="854" height="480" alt="image" src="https://github.com/user-attachments/assets/95683da3-b0b6-4738-9ef9-b4f0bc023211" />

  - 2nd unlocked: if skill tree is set to one path, this will lock another path skill node.
  <img width="854" height="480" alt="image" src="https://github.com/user-attachments/assets/97e04727-3931-4f67-8a08-f18d332c1454" />

  - 3rd unlocked
  <img width="854" height="480" alt="image" src="https://github.com/user-attachments/assets/2d1e4d25-70e6-4494-8f99-1fae0ce7ebf8" />

If the designer enables **Single-Path Mode**, players must commit to one progression branch.
<img width="854" height="480" alt="image" src="https://github.com/user-attachments/assets/09faa78e-0e39-4ddc-907a-8c2bbf4e4b8a" />
<img width="854" height="480" alt="image" src="https://github.com/user-attachments/assets/27e6ab84-ba59-4da8-a918-1f8b9895100d" />

If disabled, players can unlock multiple paths freely.
<img width="854" height="480" alt="image" src="https://github.com/user-attachments/assets/21aacca5-f105-4673-92f5-805ad5186a85" />

All branching logic and validation are handled internally by the system.

This approach ensures flexibility while keeping the implementation clean and scalable.

---

## 🤖 Enemy AI System (C# Only)

The enemy AI system includes:

- Player detection using Raycast and optimized OverlapSphere for attack (player also use this system)
- Follow behavior
- Attack transitions
- Direction switching
- Back-up logic for attack distance
- State-based behavior management

<img width="854" height="480" alt="image" src="https://github.com/user-attachments/assets/1d96b2ba-8e77-47bf-b0f3-d6d7bdf3c338" />

The AI logic separates detection, decision-making, and execution to maintain clarity and expandability.

---

## 📌 Project Emphasis

While the gameplay may appear simple externally, the internal architecture is designed to demonstrate:

- System scalability
- Clean object-oriented structure
- Maintainable and extensible system design
- Combat logic complexity
- Flexible skill progression design
- Expandable state-driven gameplay systems
