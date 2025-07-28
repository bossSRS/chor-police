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
Scripts/
├── Core/
├── Player/
├── Systems/
│ └── Scoring/
├── Utilities/
└── Testing/


---

## 🛠️ Development Plan

### ✅ Phase 1: Core Logic + AI Prototype (In Progress)
- [x] Player & Role system
- [x] Round simulation
- [x] Scoring via ScriptableObject
- [ ] Simple AI system (target selection logic)
- [ ] UI for player roles, round status, and scores

### 🔄 Phase 2: Online Multiplayer (Photon Fusion 2)
- Sync player roles & scores
- Room creation & matchmaking
- Sync game state across clients

### 🔄 Phase 3: Local Multiplayer (WiFi/Bluetooth)
- Local peer-to-peer discovery
- Sync actions over LAN/Bluetooth

---

## 🧰 Tech Stack

- **Engine:** Unity (2022.3 LTS or newer)
- **Language:** C#
- **Networking:** Photon Fusion 2 (Planned)
- **Version Control:** Git + GitHub

---

## 📸 Screenshots / Demos

*To be added as development progresses.*

---

## 🤝 Contributions & Feedback

This is a solo educational project but feedback is always welcome.  
If you're a Bangladeshi dev or board game fan — feel free to star or fork!

---

## 📅 Timeline

🧪 Week 1–2: Core logic + AI  
🕹️ Week 3–4: Multiplayer mode with Photon Fusion  
📡 Week 5–6: Bluetooth/WiFi Local Multiplayer

---

## 👤 Author

**Sadikur Rahman** – [LinkedIn](https://www.linkedin.com/in/sadikur-rahman-385232159)  
Game Developer | CTO | OOP Practitioner | Unity Enthusiast

---

> Built with ❤️ to revive our childhood games using modern multiplayer tech.


