namespace PeacefulMines
{
    public sealed class ModConfig
    {
        /// <summary>Whether the player gains Combat experience when monsters are converted into loot.</summary>
        public bool GainCombatExperience { get; set; } = true;

        /// <summary>Whether Gil's Monster Slayer goals at the Adventurer's Guild count towards completion.</summary>
        public bool ProgressMonsterSlayerGoals { get; set; } = true;

        /// <summary>Whether pet slimes raised inside the Slime Hutch are protected from being converted.</summary>
        public bool ProtectSlimeHutch { get; set; } = true;

        /// <summary>Whether visual particle effects (star sparkle / death animation) appear where the monster was converted.</summary>
        public bool SpawnVisualEffects { get; set; } = true;

        /// <summary>Whether the player is completely protected from taking monster damage as a safeguard.</summary>
        public bool PreventMonsterDamage { get; set; } = true;

        /// <summary>Whether to re-theme in-game dialogue, the Adventurer's Guild, and Combat skill into Paleontology and field research.</summary>
        public bool EnablePaleontologyTheme { get; set; } = true;
    }
}
