# 🚀 Space Explorer

> **2D Arcade Space Shooter** — Built with Unity 6 (C#)  
> Course: PRU213 — SE19B.NET | FPTU

---

## 📋 Table of Contents

- [Overview](#-overview)
- [Gameplay](#-gameplay)
- [Features](#-features)
- [Project Structure](#-project-structure)
- [Scene Flow](#-scene-flow)
- [Scripts Reference](#-scripts-reference)
- [Controls](#-controls)
- [Level Progression](#-level-progression)
- [Power-Up System](#-power-up-system)
- [Boss System](#-boss-system)
- [Setup & Requirements](#-setup--requirements)
- [How to Run](#-how-to-run)
- [Team](#-team)

---

## 🌌 Overview

**Space Explorer** is a 2D endless arcade space-shooter where the player pilots a spaceship through an increasingly dangerous asteroid field. The game features score-based level progression, a parallax scrolling background, periodic Boss encounters, a power-up system, and instant-death collision mechanics.

| Property | Value |
|---|---|
| Engine | Unity 6 |
| Language | C# |
| Platform | PC (Windows) |
| Aspect Ratio | 16:9 (1920×1080) |
| Camera | Orthographic |
| Target FPS | 60 |

---

## 🎮 Gameplay

- **Dodge** falling asteroids — any collision ends the run immediately
- **Shoot** asteroids with your laser cannon to score points
- **Collect** floating Energy Stars for bonus points
- **Defeat** Boss enemies that spawn every 5 levels for big score rewards
- **Survive** as long as possible and reach the highest level

### Scoring

| Action | Points |
|---|---|
| Destroy asteroid with laser | +5 |
| Collect Energy Star | +10 |
| Defeat Boss | +200 |

---

## ✨ Features

- ⚡ **Instant-death collision** — no HP bar, pure skill-based survival
- 📈 **Score-based level system** — every 100 points = +1 level
- 👾 **Boss encounters** — every 5 levels, a Boss with 30 HP appears
- 🌠 **Parallax scrolling backgrounds** — 3 depth layers (Moving / Mid / Near) switch per level tier
- 🛡️ **Power-up system** — Shield, Speed Boost, Double Shot
- 🎵 **Full audio** — looping BGM + per-event SFX
- 📷 **Camera shake** — Coroutine-driven impact feedback
- 💾 **Persistent high score** — saved via PlayerPrefs across sessions
- 🎨 **Ship skin selector** — choose appearance from the main menu

---

## 📁 Project Structure

```
Space_Explorer/
├── Assets/
│   ├── Prefabs/                  # All reusable GameObjects
│   │   ├── Asteroid.prefab
│   │   ├── Boss.prefab
│   │   ├── BossHealthBar.prefab
│   │   ├── EnemyBullet.prefab
│   │   ├── LaserBullet 1.prefab
│   │   ├── Player.prefab
│   │   ├── PowerUp_Shield.prefab
│   │   ├── Star.prefab
│   │   ├── Moving_Background.prefab
│   │   ├── Mid_Background.prefab
│   │   └── Near_Background.prefab
│   │
│   ├── Scenes/
│   │   ├── Audio/                # BGM + SFX clips (.wav/.mp3)
│   │   ├── Backgrounds/          # Background sprites
│   │   ├── Scripts/              # All C# MonoBehaviours
│   │   │   ├── GameManager.cs
│   │   │   ├── PlayerController.cs
│   │   │   ├── Asteroid.cs
│   │   │   ├── Boss.cs
│   │   │   ├── Bullet.cs
│   │   │   ├── EnemyBullet.cs
│   │   │   ├── Star.cs
│   │   │   ├── PowerUp.cs
│   │   │   ├── PowerUpType.cs
│   │   │   ├── ShieldEffect.cs
│   │   │   ├── BackgroundManager.cs
│   │   │   ├── BackgroundScroller.cs
│   │   │   ├── CameraShake.cs
│   │   │   ├── EndGameManager.cs
│   │   │   ├── MainMenuController.cs
│   │   │   ├── MainMenuUI.cs
│   │   │   ├── ParallaxLayer.cs
│   │   │   ├── PauseMenu.cs
│   │   │   └── SkinSelector.cs
│   │   ├── Sprites/              # All 2D sprites
│   │   ├── MainMenu.unity
│   │   ├── Gameplay.unity
│   │   └── EndGame.unity
│   │
│   └── _Recovery/                # Script backups
│
├── Packages/                     # Unity package manifest
├── ProjectSettings/              # Unity project settings
└── README.md
```

---

## 🎬 Scene Flow

```
┌─────────────┐     Play      ┌─────────────┐    Game Over    ┌─────────────┐
│  MainMenu   │ ──────────► │  Gameplay   │ ──────────────► │   EndGame   │
│  (index 0)  │              │  (index 1)  │                  │  (index 2)  │
└─────────────┘              └─────────────┘ ◄────────────── └─────────────┘
                                                  Play Again
```

| Scene | Description |
|---|---|
| `MainMenu` | Title screen, Play button, skin selector, instructions panel |
| `Gameplay` | Core game loop — spawning, scoring, leveling, boss fights |
| `EndGame` | Final score display, rank title, high score update, replay |

---

## 📜 Scripts Reference

| Script | Attached To | Responsibility |
|---|---|---|
| `GameManager.cs` | GameManager object | Global state: score, level, spawning, boss logic, UI update, game over |
| `PlayerController.cs` | Player prefab | Movement, shooting, power-up application, shield logic |
| `Asteroid.cs` | Asteroid prefab | Fall + rotate, laser hit → score, player hit → instant game over |
| `Boss.cs` | Boss prefab | Sine-wave patrol, shooting pattern, HP tracking, invincibility window |
| `EnemyBullet.cs` | EnemyBullet prefab | Downward movement, player hit → instant game over |
| `Bullet.cs` | LaserBullet prefab | Upward movement, auto-destroy on exit |
| `Star.cs` | Star prefab | Fall, player collect → +10 score |
| `PowerUp.cs` | PowerUp prefabs | Fall, player collect → apply buff |
| `ShieldEffect.cs` | Player Shield child | Sinusoidal alpha blink while shield is active |
| `BackgroundManager.cs` | Background manager | Switches background layer set per level tier |
| `BackgroundScroller.cs` | Background layers | Infinite vertical scroll for parallax effect |
| `CameraShake.cs` | Main Camera | Coroutine-based screen shake on impact |
| `EndGameManager.cs` | EndGame scene | Reads PlayerPrefs, displays final score + rank title |
| `PauseMenu.cs` | Pause UI | Pause / resume via Escape key |
| `SkinSelector.cs` | Main Menu | Ship skin preview and selection via PlayerPrefs |

---

## 🕹️ Controls

| Input | Action |
|---|---|
| `A` / `D` or `←` / `→` | Move left / right |
| `W` / `S` or `↑` / `↓` | Move up / down |
| `Space` | Fire laser |
| `Escape` | Pause / Resume |

> Movement is **normalized** — diagonal movement is not faster than straight movement.

---

## 📊 Level Progression

Level increases automatically as score grows — **every 100 points = +1 level**.

```
Level 1  :    0 – 99   pts   (start)
Level 2  :  100 – 199  pts
Level 3  :  200 – 299  pts
Level 4  :  300 – 399  pts
Level 5  :  400 – 499  pts   ← BOSS spawns
Level 6  :  500 – 599  pts   ← MILESTONE 🔴 (speed + density boost)
Level 10 :  900 – 999  pts   ← BOSS spawns
Level 11 : 1000 – 1099 pts   ← MILESTONE 🔴
...
```

### Difficulty Scaling

| Trigger | Effect |
|---|---|
| Every level | Asteroid spawn delay −0.15 s |
| Level 5, 10, 15… | Boss spawns (spawn delay reduced during boss fight) |
| Level 6, 11, 16, 21… | **Milestone**: fall speed +0.8, extra spawn density burst |
| Max fall speed cap | 12 units/s |
| Min spawn delay cap | 0.3 s |

---

## 🛡️ Power-Up System

Power-ups fall from the top of the screen and can be collected by flying into them.

| Icon | Type | Effect | Duration |
|---|---|---|---|
| 🛡️ | **Shield** | Absorbs 1 fatal asteroid/bullet hit | Until consumed |
| ⚡ | **Speed Boost** | Movement speed ×1.8 | 6 seconds |
| 🔫 | **Double Shot** | Fires 2 parallel lasers simultaneously | 7 seconds |

> **Note:** The player starts each run with **1 free Shield** automatically applied.  
> A blue glow on the ship indicates the shield is active.

---

## 👾 Boss System

A **Boss (Alien Dreadnought)** spawns at every **5th level** (Level 5, 10, 15…).

### Boss Behaviour

```
┌─────────────────────────────────────────────────────┐
│  Sine-wave horizontal patrol at top of screen       │
│  Fires arrow projectiles downward at fireRate       │
│  30 HP — each laser hit = −1 HP                     │
│  50 ms invincibility window between hits            │
│  Boss Health Bar UI visible during encounter        │
│  Asteroid spawning PAUSED while boss is alive       │
│  On death: +200 score + health bar hides            │
└─────────────────────────────────────────────────────┘
```

### Collision Rules (during Boss fight)

| Hit | Result |
|---|---|
| Player laser → Boss | −1 Boss HP |
| Boss arrow → Player | **Instant Game Over** |
| Asteroid → Player | **Instant Game Over** |

---

## ⚙️ Setup & Requirements

### Requirements

- **Unity** 6.x (6000.x LTS recommended)
- **TextMeshPro** (included via Unity Package Manager)
- **Input System** package (already configured)
- Windows 10/11

### Unity Package Dependencies

```json
{
  "com.unity.textmeshpro": "3.x",
  "com.unity.inputsystem": "1.x",
  "com.unity.2d.sprite": "included"
}
```

---

## ▶️ How to Run

### Option A — Play in Unity Editor

1. Clone or download this repository
2. Open **Unity Hub** → **Open Project** → select the `Space_Explorer/` folder
3. Wait for Unity to import all assets
4. Open `Assets/Scenes/MainMenu.unity`
5. Press **Play ▶**

### Option B — Build & Run

1. In Unity: **File → Build Settings**
2. Ensure scenes are in order:
   - Index 0: `MainMenu`
   - Index 1: `Gameplay`
   - Index 2: `EndGame`
3. Select **PC, Mac & Linux Standalone** → **Windows**
4. Click **Build And Run**

### Tag Setup (if scenes reset)

Ensure the following tags exist in **Edit → Project Settings → Tags & Layers**:

```
Player  |  Laser  |  Enemy  |  Star
```

---

## 👤 Team

| Role | Name | Student ID |
|---|---|---|
| Developer / Designer | Phạm Hồng Phúc | QE190133 |

**Course:** PRU213 — Game Development with Unity  
**Class:** SE19B.NET  
**Institution:** FPT University

---

## 📄 License

This project was developed for academic purposes as part of the PRU213 lab assignment.  
All game assets (sprites, audio) are used for educational purposes only.

---

*Space Explorer © 2025 — Phạm Hồng Phúc | QE190133*
"# SpaceExplorer_NguyenCongNhat_SE19BNET_Lab01" 
