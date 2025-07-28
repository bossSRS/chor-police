# 🎮 Chor Police – Unity Multiplayer Board Game

A 4-player social deduction board game made in Unity, based on the popular Bangladeshi game "Chor Police".  
This project is built following solid OOP principles, ScriptableObjects, and will support both AI and multiplayer gameplay (Photon Fusion 2).

---

## 🧠 Game Overview

- **Players:** 4
- **Roles:**  
  - 👮 **Police**  
  - 🕵️ **Chor (Thief)**  
  - 💼 **Babu (Innocent)**  
  - 💣 **Dakat (Robber)**

Each round:
- One player is assigned each role (randomly).
- The **Police** is tasked with catching either the **Chor** or **Dakat**, depending on the round target.
- The round target switches every round: `CHOR ⇄ DAKAT`.

### 🏆 Scoring Rules

| Role     | Condition        | Points |
|----------|------------------|--------|
| **Babu**   | Survives         | 1000   |
| **Police** | Catches target   | 800    |
| **Chor**   | Escapes target round | 700    |
| **Dakat**  | Escapes target round | 600    |
| **Any**    | If caught or not in target | 0      |

---

## 🏗️ Project Architecture

- **OOP Concepts Used:**
  - `Encapsulation`: Each system is self-contained (e.g., `PlayerData`, `ScoringSystem`)
  - `Inheritance`: Base class `BaseManager` for shared manager behavior
  - `Polymorphism`: `IScorable` strategy pattern for role-based scoring logic

- **ScriptableObject**
  - `ScoreConfig`: Easily adjust scores without touching code

### 📁 Folder Structure

