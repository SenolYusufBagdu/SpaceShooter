# 🚀 Space Shooter

A 2D space shooter built with **MonoGame** and **C#**.  
Fight through endless waves of enemies, defeat bosses, collect power-ups, and survive as long as you can.  
Every boss you defeat makes the next wave faster and more dangerous.

---

## 🎮 Gameplay

- Enemies spawn from the top and move downward
- Kill **10 enemies** to trigger a **Boss fight**
- Boss moves left and right and shoots directly at you
- Defeat the boss to start a new wave — harder each time
- Collect **heart power-ups** to restore lost health
- You have **3 lives** — lose them all and it's game over

---

## 📸 Preview

![gameplay](Content/space.png)

---

## 📈 Difficulty Scaling

| Wave | Enemy Speed | Boss Fire Rate |
|------|-------------|----------------|
| 1    | Normal      | Every 1.5s     |
| 2    | +20         | Every 1.2s     |
| 3    | +40         | Every 0.9s     |
| 4+   | +60         | Every 0.6s (min) |

---

## 🕹️ Controls

| Key | Action |
|-----|--------|
| `← → ↑ ↓` | Move player |
| `Space` | Shoot |
| `ESC` | Quit |
| `R` | Restart after game over |

---

## ❤️ Power-Up System

| Power-Up | How to get | Effect |
|----------|------------|--------|
| Heart | Appears after every boss kill (if health < 3) | +1 health |

- Touch the heart or shoot it to collect
- Only spawns when you have less than 3 lives

---

## 🛠️ Getting Started

### Requirements
- [.NET 8.0](https://dotnet.microsoft.com/)
- [MonoGame 3.8+](https://monogame.net/)
- Visual Studio 2022

### Run
1. Clone the repository
```bash
