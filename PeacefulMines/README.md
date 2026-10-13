# Peaceful Mines - No Combat Mod for Stardew Valley (1.6+)

A SMAPI mod that completely removes combat from Stardew Valley. Whenever a monster or enemy spawns, the mod immediately defeats it and scatters its normal dropped loot onto the floor as collectible debris.

The Mines, Skull Cavern, Volcano Dungeon, and nighttime farm become peaceful foraging and exploration areas filled with monster loot!

---

## ✨ Features

- **Automatic Monster Pacification:** Any monster that appears (slimes, bats, skeletons, serpents, ghosts, pepper rexes, etc.) immediately bursts into its dropped loot upon spawn.
- **Full Drop Table Compatibility:** Drops include standard loot (bat wings, slime, solar/void essences, bug meat), rare drops (prismatic shards, dwarf scrolls), and bonuses from the **Burglar's Ring** and **Monster Compendium** double-drop mechanics.
- **Combat XP Progression (Enabled by Default):** Awards full combat experience points so you still level up Combat naturally and unlock crafting recipes (like the Iridium Band, Quality Bobber, and Bombs).
- **Gil's Monster Slayer Goals (Enabled by Default):** Automatically counts defeated monsters toward the Adventurer's Guild Monster Slayer quotas.
- **Infested Mine Floor Support:** Automatically unlocks and spawns the ladder down on infested/monster floors once all monsters are pacified, ensuring you never get stuck.
- **Off-Screen Swarm Safeguard:** Clamps loot drops within playable map bounds so bat/ghost mist swarms don't drop items into the void.
- **Zero Enemy Damage:** Removes enemy projectiles (fireballs, squid ink) and includes a health safeguard so exploration is 100% stress-free.
- **Slime Hutch Protection:** Keeps pet slimes inside your farm's Slime Hutch safe.
- **Generic Mod Config Menu (GMCM) Support:** Fully configurable in-game if GMCM is installed.

---

## ⚙️ Configuration (`config.json`)

The first time the game launches, SMAPI creates a `config.json` inside `Mods/PeacefulMines/`. You can also configure it in-game via Generic Mod Config Menu:

| Setting | Default | Description |
|---|---|---|
| `GainCombatExperience` | `true` | Grants combat XP when monsters drop loot. |
| `ProgressMonsterSlayerGoals` | `true` | Credits Gil's monster slayer quotas. |
| `ProtectSlimeHutch` | `true` | Prevents pet slimes in the Slime Hutch from being converted. |
| `SpawnVisualEffects` | `true` | Displays sparkle/poof effects on defeat. |
| `PreventMonsterDamage` | `true` | Safeguard ensuring you never take monster damage. |

---

## 🚀 How to Launch & Play

1. Run **`StardewModdingAPI.exe`** inside your Stardew Valley installation folder:
   `C:\Program Files (x86)\Steam\steamapps\common\Stardew Valley\StardewModdingAPI.exe`
2. **Steam Integration (Optional, to track hours and achievements):**
   - Open Steam, right-click **Stardew Valley** -> **Properties...**
   - In the **General** tab under **Launch Options**, paste:
     ```text
     "C:\Program Files (x86)\Steam\steamapps\common\Stardew Valley\StardewModdingAPI.exe" %command%
     ```
   - Now launching the game through Steam will automatically start SMAPI with your mods loaded!

---

## 🛠️ Rebuilding the Mod

If you ever want to make changes to the code:
1. Edit `ModEntry.cs` in `C:\Users\steph\PeacefulMines`.
2. Open PowerShell in `C:\Users\steph\PeacefulMines` and run:
   ```powershell
   & "$env:USERPROFILE\.dotnet\dotnet.exe" build
   ```
3. The build script will automatically compile and deploy the updated mod directly to your Stardew Valley `Mods/` folder!
