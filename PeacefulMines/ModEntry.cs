using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Locations;
using StardewValley.Monsters;

namespace PeacefulMines
{
    public class ModEntry : Mod
    {
        private ModConfig Config = null!;

        public override void Entry(IModHelper helper)
        {
            this.Config = helper.ReadConfig<ModConfig>();

            helper.Events.GameLoop.GameLaunched += this.OnGameLaunched;
            helper.Events.Player.Warped += this.OnWarped;
            helper.Events.World.NpcListChanged += this.OnNpcListChanged;
            helper.Events.GameLoop.UpdateTicked += this.OnUpdateTicked;
        }

        private void OnGameLaunched(object? sender, GameLaunchedEventArgs e)
        {
            // Optional integration with Generic Mod Config Menu
            var configMenu = this.Helper.ModRegistry.GetApi<IGenericModConfigMenuApi>("spacechase0.GenericModConfigMenu");
            if (configMenu != null)
            {
                configMenu.Register(
                    mod: this.ModManifest,
                    reset: () => this.Config = new ModConfig(),
                    save: () => this.Helper.WriteConfig(this.Config)
                );

                configMenu.AddBoolOption(
                    mod: this.ModManifest,
                    name: () => "Gain Combat XP",
                    tooltip: () => "Gain Combat experience points when monsters are converted into loot.",
                    getValue: () => this.Config.GainCombatExperience,
                    setValue: value => this.Config.GainCombatExperience = value
                );

                configMenu.AddBoolOption(
                    mod: this.ModManifest,
                    name: () => "Progress Slayer Goals",
                    tooltip: () => "Count defeated monsters towards Gil's Monster Slayer goals at the Adventurer's Guild.",
                    getValue: () => this.Config.ProgressMonsterSlayerGoals,
                    setValue: value => this.Config.ProgressMonsterSlayerGoals = value
                );

                configMenu.AddBoolOption(
                    mod: this.ModManifest,
                    name: () => "Protect Slime Hutch",
                    tooltip: () => "Do not convert pet slimes living inside the Slime Hutch.",
                    getValue: () => this.Config.ProtectSlimeHutch,
                    setValue: value => this.Config.ProtectSlimeHutch = value
                );

                configMenu.AddBoolOption(
                    mod: this.ModManifest,
                    name: () => "Spawn Visual Effects",
                    tooltip: () => "Display star particles and sound effects when monsters are converted.",
                    getValue: () => this.Config.SpawnVisualEffects,
                    setValue: value => this.Config.SpawnVisualEffects = value
                );

                configMenu.AddBoolOption(
                    mod: this.ModManifest,
                    name: () => "Prevent Monster Damage",
                    tooltip: () => "Ensure the player is immune to any monster attacks or lingering damage.",
                    getValue: () => this.Config.PreventMonsterDamage,
                    setValue: value => this.Config.PreventMonsterDamage = value
                );
            }
        }

        private void OnWarped(object? sender, WarpedEventArgs e)
        {
            this.ConvertAllMonsters(e.NewLocation);
        }

        private void OnNpcListChanged(object? sender, NpcListChangedEventArgs e)
        {
            foreach (NPC npc in e.Added)
            {
                if (npc is Monster monster)
                {
                    this.PacifyMonster(e.Location, monster);
                }
            }
        }

        private void OnUpdateTicked(object? sender, UpdateTickedEventArgs e)
        {
            if (!Context.IsWorldReady || Game1.currentLocation == null)
                return;

            GameLocation location = Game1.currentLocation;

            // Clear rogue projectiles (e.g. fireballs, squid ink, shaman curses)
            if (location.projectiles.Count > 0)
            {
                location.projectiles.Clear();
            }

            // Health protection fail-safe
            if (this.Config.PreventMonsterDamage && Game1.player.health < Game1.player.maxHealth)
            {
                Game1.player.health = Game1.player.maxHealth;
            }

            // Periodically check if any monster spawned without triggering NpcListChanged
            if (e.IsMultipleOf(2))
            {
                this.ConvertAllMonsters(location);
            }
        }

        private void ConvertAllMonsters(GameLocation location)
        {
            if (this.Config.ProtectSlimeHutch && location is SlimeHutch)
                return;

            List<Monster> monsters = location.characters.OfType<Monster>().ToList();
            if (monsters.Count == 0)
                return;

            foreach (Monster monster in monsters)
            {
                this.PacifyMonster(location, monster);
            }

            // After clearing, check if this is an infested mine floor that needs its ladder down
            if (location is MineShaft mine && mine.mustKillAllMonstersToAdvance() && !mine.ladderHasSpawned)
            {
                if (mine.EnemyCount == 0)
                {
                    mine.createLadderAt(mine.tileBeneathLadder, "newArtifact");
                    Game1.showGlobalMessage(Game1.content.LoadString("Strings\\StringsFromCSFiles:MineShaft.cs.9484"));
                }
            }
        }

        private void PacifyMonster(GameLocation location, Monster monster)
        {
            if (this.Config.ProtectSlimeHutch && location is SlimeHutch)
                return;

            // If monster spawned outside map boundaries (e.g. flying bat swarm from off-screen),
            // clamp its position to within the map so dropped items are reachable
            if (location.Map?.Layers?.Count > 0)
            {
                int mapWidth = location.Map.Layers[0].LayerWidth * 64;
                int mapHeight = location.Map.Layers[0].LayerHeight * 64;
                int dropX = (int)monster.Position.X;
                int dropY = (int)monster.Position.Y;

                if (dropX < 64 || dropX > mapWidth - 64 || dropY < 64 || dropY > mapHeight - 64)
                {
                    dropX = Math.Clamp(dropX, 64, Math.Max(64, mapWidth - 128));
                    dropY = Math.Clamp(dropY, 64, Math.Max(64, mapHeight - 128));
                    monster.Position = new Vector2(dropX, dropY);
                }
            }

            Rectangle bbox = monster.GetBoundingBox();
            if (bbox.Width == 0 || bbox.Height == 0)
            {
                bbox = new Rectangle((int)monster.Position.X, (int)monster.Position.Y, 64, 64);
            }

            // Handle combat experience toggle
            int originalExp = monster.ExperienceGained;
            if (!this.Config.GainCombatExperience)
            {
                monster.ExperienceGained = 0;
            }

            // Treat monster as defeated: triggers item drops, special mine drops,
            // compendium double drops, and monster slayer progress
            monster.Health = 0;
            this.Helper.Reflection.GetMethod(location, "onMonsterKilled").Invoke(Game1.player, monster, bbox, false);

            // Restore original XP property on monster
            monster.ExperienceGained = originalExp;

            // Remove living monster from map immediately
            location.characters.Remove(monster);

            // Safeguard for infested floor ladder
            if (location is MineShaft mine && mine.mustKillAllMonstersToAdvance() && !mine.ladderHasSpawned)
            {
                if (mine.EnemyCount == 0)
                {
                    mine.createLadderAt(mine.tileBeneathLadder, "newArtifact");
                    Game1.showGlobalMessage(Game1.content.LoadString("Strings\\StringsFromCSFiles:MineShaft.cs.9484"));
                }
            }
        }
    }
}
