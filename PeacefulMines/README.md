# Peaceful Mines - No Combat & Paleontology Mod for Stardew Valley (1.6+)

A SMAPI mod that completely removes combat from Stardew Valley and re-themes dungeon crawling into **Paleontology** and ancient field excavation!

Whenever a monster appears, the mod immediately defeats it and scatters its normal dropped loot onto the floor as collectible specimens. The Mines, Skull Cavern, and Volcano Dungeon become peaceful archaeological expeditions filled with fossil relics!

---

## ✨ Features

### 1. Peaceful Exploration & Specimen Drops
- **Instant Drop Conversion:** Any monster that appears immediately bursts into its dropped loot upon spawn.
- **Full Drop Table Support:** Includes standard loot (bat wings, slime, solar/void essences, bug meat), rare drops (prismatic shards, dwarf scrolls), and bonuses from the **Burglar's Ring** and **Monster Compendium**.
- **Infested Mine Floor Automation:** Automatically unlocks and spawns the ladder down on infested floors once all monsters are converted, ensuring you never get stuck.
- **Off-Screen Swarm Safeguard:** Clamps loot drops within playable map bounds so flying swarms don't drop items into the void.
- **Zero Enemy Damage:** Removes enemy projectiles (fireballs, squid ink) and includes a health safeguard.
- **Slime Hutch Protection:** Keeps pet slimes inside your farm's Slime Hutch safe.

### 2. Paleontology & The Paleontological Society Overhaul
- **Skill Name:** Changes **Combat** $\rightarrow$ **Paleontology** in the skills menu, pause tab, and level-up screens.
- **Professions:**
  - *Fighter* $\rightarrow$ **Excavator** (All digs unearth higher quality fossil samples)
  - *Scout* $\rightarrow$ **Surveyor** (Greatly increases critical relic discovery chance)
  - *Brute* $\rightarrow$ **Deep Chiseled** (Excavation efficiency increased)
  - *Defender* $\rightarrow$ **Preservationist** (+25 Max Health for extended field surveys)
  - *Acrobat* $\rightarrow$ **Agile Researcher** (Field maneuver cooldowns cut in half)
  - *Desperado* $\rightarrow$ **Master Paleontologist** (Specimen discoveries yield extraordinary rewards)
- **The Adventurer's Guild $\rightarrow$ Paleontological Society:**
  - Outside sign: *"Paleontological Society - Proven field researchers only."*
  - Eradication Board: *"Paleontological Excavation Goals - Help us catalog the ancient valley specimens."*
  - Goals re-framed as fossil cataloging (e.g. *Preserved Slime Specimens*, *Calcified Skeletal Relics*, *Primeval Dinosaur Fossils*, *Ancient Serpent Bones*).
  - Gil and Marlon's dialogue re-themed to field expeditions and research grants.
- **Cutscenes & Quests:**
  - Marlon's Floor 5 Mine intro cutscene gives you the scraping blade to unearth fossils in the deep strata.
  - Initiation Quest (Quest 15): Catalog 10 slime fossil specimens to earn your place in the Paleontological Society.
  - Clint's Cave Patrol $\rightarrow$ *Specimen Survey*.
  - Wizard's Prismatic Jelly $\rightarrow$ Unearthing ancient prismatic slime specimens.

---

## ⚙️ Configuration (`config.json`)

Created automatically inside `Mods/PeacefulMines/` on first launch, or configurable in-game via **Generic Mod Config Menu (GMCM)**:

| Setting | Default | Description |
|---|---|---|
| `EnablePaleontologyTheme` | `true` | Enables the Paleontology skill and Paleontological Society text re-theme. |
| `GainCombatExperience` | `true` | Grants Paleontology/Combat XP when monsters drop loot. |
| `ProgressMonsterSlayerGoals` | `true` | Credits Gil's Paleontological Society specimen quotas. |
| `ProtectSlimeHutch` | `true` | Prevents pet slimes in the Slime Hutch from being converted. |
| `SpawnVisualEffects` | `true` | Displays sparkle/poof effects on defeat. |
| `PreventMonsterDamage` | `true` | Safeguard ensuring you never take monster damage. |

---

## 🚀 How to Launch & Play

1. Run **`StardewModdingAPI.exe`** inside your Stardew Valley installation folder:
   `C:\Program Files (x86)\Steam\steamapps\common\Stardew Valley\StardewModdingAPI.exe`
2. **Steam Integration (Optional):**
   - Open Steam, right-click **Stardew Valley** -> **Properties...**
   - In the **General** tab under **Launch Options**, paste:
     ```text
     "C:\Program Files (x86)\Steam\steamapps\common\Stardew Valley\StardewModdingAPI.exe" %command%
     ```
   - Launch through Steam to keep tracking achievements and hours!

---

## 🛠️ Rebuilding the Mod

To make edits to the code:
1. Edit files in [`C:/Users/steph/PeacefulMines`](file:///C:/Users/steph/PeacefulMines).
2. Open PowerShell in `C:\Users\steph\PeacefulMines` and run:
   ```powershell
   & "$env:USERPROFILE\.dotnet\dotnet.exe" build
   ```
3. It will compile and deploy automatically to your game's `Mods/` directory.
