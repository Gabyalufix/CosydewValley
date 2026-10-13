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
            helper.Events.Content.AssetRequested += this.OnAssetRequested;
        }

        private void OnGameLaunched(object? sender, GameLaunchedEventArgs e)
        {
            var configMenu = this.Helper.ModRegistry.GetApi<IGenericModConfigMenuApi>("spacechase0.GenericModConfigMenu");
            if (configMenu != null)
            {
                configMenu.Register(
                    mod: this.ModManifest,
                    reset: () => this.Config = new ModConfig(),
                    save: () =>
                    {
                        this.Helper.WriteConfig(this.Config);
                        this.Helper.GameContent.InvalidateCache(asset => true);
                    }
                );

                configMenu.AddBoolOption(
                    mod: this.ModManifest,
                    name: () => "Paleontology Theme",
                    tooltip: () => "Re-theme Combat into Paleontology and the Adventurer's Guild into the Paleontological Society.",
                    getValue: () => this.Config.EnablePaleontologyTheme,
                    setValue: value => this.Config.EnablePaleontologyTheme = value
                );

                configMenu.AddBoolOption(
                    mod: this.ModManifest,
                    name: () => "Gain Combat XP",
                    tooltip: () => "Gain Combat/Paleontology experience points when monsters are converted into loot.",
                    getValue: () => this.Config.GainCombatExperience,
                    setValue: value => this.Config.GainCombatExperience = value
                );

                configMenu.AddBoolOption(
                    mod: this.ModManifest,
                    name: () => "Progress Slayer Goals",
                    tooltip: () => "Count defeated monsters towards the Paleontological Society research catalog goals.",
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

        private void OnAssetRequested(object? sender, AssetRequestedEventArgs e)
        {
            if (!this.Config.EnablePaleontologyTheme)
                return;

            // 1. Skill Names in Menus and Character Tab
            if (e.NameWithoutLocale.IsEquivalentTo("Strings/StringsFromCSFiles"))
            {
                e.Edit(asset =>
                {
                    var data = asset.AsDictionary<string, string>().Data;
                    data["Farmer.cs.1996"] = "Paleontology";
                    data["SkillsPage.cs.11608"] = "Paleontology";
                    data["Event.cs.1848"] = "The crystal ball has moved on... AH! You're in the deep strata! You're excavating an ancient discovery in the dark, and you seem more than ready to document it.";
                });
            }

            // 2. Skill Buffs and Profession Names
            else if (e.NameWithoutLocale.IsEquivalentTo("Strings/UI"))
            {
                e.Edit(asset =>
                {
                    var data = asset.AsDictionary<string, string>().Data;
                    data["Character_combat"] = "paleontology";
                    data["ItemHover_Buff3"] = "{0} Paleontology";
                    data["LevelUp_ProfessionName_Fighter"] = "Excavator";
                    data["LevelUp_ProfessionDescription_Fighter"] = "All digs unearth higher quality fossil samples.";
                    data["LevelUp_ProfessionName_Scout"] = "Surveyor";
                    data["LevelUp_ProfessionDescription_Scout"] = "Greatly increases critical relic discovery chance.";
                    data["LevelUp_ProfessionName_Brute"] = "Deep Chiseled";
                    data["LevelUp_ProfessionDescription_Brute"] = "Excavation efficiency increased.";
                    data["LevelUp_ProfessionName_Defender"] = "Preservationist";
                    data["LevelUp_ProfessionDescription_Defender"] = "+25 Max Health for extended field surveys.";
                    data["LevelUp_ProfessionName_Acrobat"] = "Agile Researcher";
                    data["LevelUp_ProfessionDescription_Acrobat"] = "Field maneuver cooldowns cut in half.";
                    data["LevelUp_ProfessionName_Desperado"] = "Master Paleontologist";
                    data["LevelUp_ProfessionDescription_Desperado"] = "Specimen discoveries yield extraordinary rewards.";
                });
            }

            // 3. Adventurer's Guild -> Paleontological Society Sign & Eradication Board
            else if (e.NameWithoutLocale.IsEquivalentTo("Strings/Locations"))
            {
                e.Edit(asset =>
                {
                    var data = asset.AsDictionary<string, string>().Data;
                    data["Mountain_AdventurersGuildNote"] = "      * Paleontological Society *\n\n    Proven field researchers only.";
                    data["AdventureGuild_Greeting"] = "Welcome to the Paleontological Society. Reviewing the field catalog?";
                    data["AdventureGuild_ItemRecovery"] = "Field Recovery Service";
                    data["AdventureGuild_KillList_Header"] = "------------------------------\n--Paleontological Excavation Goals--\n\"Help us catalog the ancient valley specimens.\"\n------------------------------";
                    data["AdventureGuild_KillList_Footer"] = "\n  -See Gil to claim your research grants and rewards.";
                    data["AdventureGuild_KillList_Slimes"] = "Preserved Slime Specimens";
                    data["AdventureGuild_KillList_VoidSpirits"] = "Shadow Essence Crystals";
                    data["AdventureGuild_KillList_Bats"] = "Fossilized Chiroptera Remains";
                    data["AdventureGuild_KillList_Skeletons"] = "Calcified Skeletal Relics";
                    data["AdventureGuild_KillList_CaveInsects"] = "Ancient Insect Exoskeletons";
                    data["AdventureGuild_KillList_Duggies"] = "Subterranean Duggy Casts";
                    data["AdventureGuild_KillList_DustSprites"] = "Crystalline Dust Cores";
                    data["AdventureGuild_KillList_RockCrabs"] = "Petrified Carapace Samples";
                    data["AdventureGuild_KillList_Mummies"] = "Preserved Crypt Relics";
                    data["AdventureGuild_KillList_PepperRex"] = "Primeval Dinosaur Fossils";
                    data["AdventureGuild_KillList_Serpent"] = "Ancient Serpent Bones";
                    data["AdventureGuild_KillList_MagmaSprite"] = "Volcanic Magma Formations";
                    data["Gil_Telephone"] = "Hey. 150 Volcanic Magma Formations cataloged, huh? I'm impressed.#$b#Tell you what... I'm gonna give you Marlon's tellie-phone number. He don't like me givin' it out, but I think you've earned it.#$b#If you ever need to use the Field Recovery Service, but don't wanna run all the way out here... just give us a ring. Sound good?";
                });
            }

            // 4. Marlon's Mine Introduction Event
            else if (e.NameWithoutLocale.IsEquivalentTo("Data/Events/Mine"))
            {
                e.Edit(asset =>
                {
                    var data = asset.AsDictionary<string, string>().Data;
                    foreach (string key in data.Keys.ToList())
                    {
                        if (key.StartsWith("100162"))
                        {
                            string eventStr = data[key];
                            eventStr = eventStr.Replace(
                                "Still, there's probably good ore down there.#$b#But a dark place, undisturbed for so long... I'm afraid ore isn't the only thing you'll find...",
                                "Still, there's probably good ore and ancient strata down there.#$b#A subterranean site undisturbed for so long... who knows what prehistoric fossils and ancient specimens you'll uncover!"
                            );
                            eventStr = eventStr.Replace(
                                "Here, take this. You might need it.",
                                "Here, take this scraping blade. You might need it to carefully excavate rock faces and unearth specimens."
                            );
                            eventStr = eventStr.Replace(
                                "Name's Marlon, by the way. I run the adventurer's guild right outside.#$b#I'll keep my eye on you. Prove yourself and I might think about making you a member.",
                                "Name's Marlon, by the way. I run the Paleontological Society right outside.#$b#I'll keep my eye on your field work. Catalog some specimens and I might think about making you a member."
                            );
                            data[key] = eventStr;
                        }
                    }
                });
            }

            // 5. Initiation & Mine Exploration Quests
            else if (e.NameWithoutLocale.IsEquivalentTo("Data/Quests"))
            {
                e.Edit(asset =>
                {
                    var data = asset.AsDictionary<string, string>().Data;
                    if (data.ContainsKey("14"))
                    {
                        data["14"] = "Basic/Explore The Mine/There's an old mine shaft in the mountains north of town. There could be valuable minerals and ancient prehistoric strata inside, where Marlon suggested fascinating discoveries await./Reach level 5 in the mines./17/17/0/-1/false";
                    }
                    if (data.ContainsKey("15"))
                    {
                        data["15"] = "Monster/Initiation/If you can catalog 10 slime fossil specimens, you'll have earned your place in the Paleontological Society./0 of 10 slime specimens cataloged./Green_Slime 10 null false/16/0/-1/false";
                    }
                });
            }

            // 6. Marlon's Invitation Letter
            else if (e.NameWithoutLocale.IsEquivalentTo("Data/Mail"))
            {
                e.Edit(asset =>
                {
                    var data = asset.AsDictionary<string, string>().Data;
                    if (data.ContainsKey("guildQuest"))
                    {
                        data["guildQuest"] = "I see you've been exploring the old mine. You've got a keen eye for field excavation, that much I can tell.^If you can catalog 10 slime fossil specimens, you'll have earned your place in our Paleontological Society. ^Happy exploring!    -Marlon %item quest 15 true %%[#]Paleontological Society Invitation";
                    }
                });
            }

            // 7. Gil's Dialogue
            else if (e.NameWithoutLocale.IsEquivalentTo("Characters/Dialogue/Gil"))
            {
                e.Edit(asset =>
                {
                    var data = asset.AsDictionary<string, string>().Data;
                    data["ComeBackLater"] = "...Come back when you've got some excavated specimens to show me, kid.";
                });
            }

            // 8. Special Orders (Clint's Cave Patrol & Wizard's Prismatic Jelly)
            else if (e.NameWithoutLocale.IsEquivalentTo("Strings/SpecialOrderStrings"))
            {
                e.Edit(asset =>
                {
                    var data = asset.AsDictionary<string, string>().Data;
                    data["Clint_Name"] = "Specimen Survey";
                    data["Clint_Text"] = "The presence of {Monster:LocalizedName} in the local caves offers valuable research material for our mineral studies! There's good money for anyone willing to catalog and collect samples from 50 of them.          -Clint";
                    data["Clint_Objective_0_Text"] = "Catalog 50 {Monster:LocalizedName}.";
                    data["Wizard2_Text"] = "I require assistance in tracking down the rare and elusive prismatic slime within the deep cave strata. Unearth a jar of its ancient prismatic jelly and you will be duly rewarded.        -M. Rasmodius";
                    data["Wizard2_Objective_0_Text"] = "Catalog a prismatic slime specimen.";
                    data["Wizard2_Objective_1_Message"] = "Ah... you've returned from the expedition. I sense that you were successful!$h#$b#Now, hand over the jelly and begone! I have important research to complete.";
                });
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

            // Handle combat/paleontology experience toggle
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
