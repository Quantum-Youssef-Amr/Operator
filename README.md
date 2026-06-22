# OPERATOR ✈

**A radar management game by Axiom Studio**

---

## Overview

OPERATOR is a radar management simulation where players guide aircraft through contested airspace by drawing waypoints on a retro radar display. Aircraft enter from the edges, each with unique size, speed, and value characteristics. The player must route them safely to their desired exit direction while avoiding hazards including enemy aircraft, military no-fly zones, storms, and jamming fields.

The game features a progression system with nine upgradable radar capabilities, including scan speed, identification tiers, and range expansion. Score earned from successful aircraft exits converts to upgrade points, enabling permanent progression across sessions.

---

## Features

**Radar System**
- Sweeping scan line with 12 RPM base speed
- Phosphor green aesthetic with afterglow trails
- Upgradeable scan speed up to 20 RPM

**Aircraft Types**
- Nine distinct aircraft across three categories
- Military (triangular): Fighter, Bomber, Helicopter
- Commercial (square): Regional Jet, Passenger Liner, Cargo
- Private (circular): Cessna, Business Jet, Luxury Jet

**Waypoint System**
- Click-to-select aircraft interface
- Drag to draw flight paths
- Aircraft follow waypoints sequentially
- Right-click to clear paths

**Hazard System**
- Enemy aircraft: Destroys nearby planes
- Military zones: Restricted for non-military aircraft
- Storm zones: Random teleport for commercial, destruction for private
- Jamming zones: Disorients aircraft with random repositioning

**Upgrade System**
- Nine upgrades across three tiers
- Identification: Shape, size, direction arrow
- Scan speed: Three levels of improvement
- Radar range: Two levels of expansion
- Utility: Waypoint memory, collision warning, hazard prediction

**Progression**
- Score-based upgrade point conversion
- Persistent save system for upgrades and high scores
- Dynamic difficulty scaling with score thresholds

---

## Controls

| Action | Input |
|--------|-------|
| Select aircraft | Left click on radar icon |
| Draw waypoints | Left click and drag |
| Clear aircraft path | Right click |
| Zoom radar | Mouse wheel |
| Pause | ESC |

---

## Development

**Technology Stack**
- Unity 2022.3 LTS
- C# scripting
- ScriptableObject architecture
- Unity UI system

**Project Structure**

```
Assets/
├── Scripts/
│   ├── Managers/
│   │   ├── GameManager.cs
│   │   ├── RadarManager.cs
│   │   ├── Spawner.cs
│   │   ├── PathManager.cs
│   │   ├── UpgradeManager.cs
│   │   ├── UIManager.cs
│   │   └── SaveManager.cs
│   ├── Aircraft/
│   │   ├── AircraftSO.cs
│   │   └── AircraftController.cs
│   ├── Hazards/
│   │   ├── DangerZone.cs
│   │   └── EnemyAircraft.cs
│   └── UI/
│       ├── ScoreDisplay.cs
│       ├── UpgradeMenu.cs
│       └── AircraftInfoPanel.cs
├── ScriptableObjects/
│   ├── Aircraft/ (9 .asset files)
│   ├── Hazards/ (4 .asset files)
│   └── Upgrades/ (9 .asset files)
├── Prefabs/
├── Scenes/
│   ├── MainMenu.unity
│   └── Game.unity
├── Art/
└── Audio/
```
---

## Play the Game

**WebGL**  
- Available at: [axiom.itch.io/operator](https://axiom.itch.io/operator)

**Windows Standalone**  
- Download from Itch.io page

---

## License

**UNLICENSED**

This software is proprietary and confidential.

Copyright © 2026 Axiom Studio represented by Youssef Amr. All rights reserved.

Unauthorized copying, distribution, modification, or use of this software, via any medium, is strictly prohibited without prior written permission from Axiom Studio.

NO License
Copyright (c) 2026 Axiom Studio represented by Youssef Amr

Permission is NOT granted to use, copy, modify, merge, publish, distribute, 
sublicense, or sell copies of this software without prior written permission 
from Axiom Studio.


---

## Contact
[Axiom Studio Email](mailto:axiomstudio.gamedev@gmail.com)
[Youssef Amr Email](mailto:amry14003@gmail.com)
