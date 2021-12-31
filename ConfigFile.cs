using System;
using Terraria;
using Terraria.ModLoader.Config;
using System.ComponentModel;
using Terraria.ID;


namespace SummonHeart
{
    [Flags]
    public enum GamePlayFlag
    {
        NONE = 0x0,
        NPCPROGRESS = 0x1,
        XPREDUTION = 0x2,
        NPCRARITY = 0x4,
        NPCMODIFIER = 0x8,
        BOSSMODIFIER = 0x10,
        BOSSCLUSTERED = 0x20,
        RPGPLAYER = 0x40,
        ITEMRARITY = 0x80,
        ITEMMODIFIER = 0x100,
        LIMITNPCGROWTH = 0x200,
        DISPLAYNPCNAME = 0x400
    }

    [Label("修仙者显示配置")]
    public class VisualConfig : ModConfig
    {
        // You MUST specify a MultiplayerSyncMode.
        public override ConfigScope Mode => ConfigScope.ClientSide;

        [Label("HealthBar Offset")]
        [Tooltip("Health Bar Offset on the Y axis")]
        [Range(-500f, 1500f)]
        [Increment(10f)]
        [DefaultValue(100)]
        public float HealthBarYoffSet;

        [Label("Health Bar Scale")]
        [Range(0.1f, 3f)]
        [Increment(.25f)]
        [DefaultValue(0.75f)]
        public float HealthBarScale;

        [Label("Other UI Scale")]
        [Tooltip("Used for the scale of all other UI element from Another RPG Mod")]
        [Range(0.1f, 3f)]
        [Increment(.25f)]
        [DefaultValue(0.75f)]
        public float UI_Scale;

        [Label("Display npc name")]
        [Tooltip("Display NPC information at all time and detailed information when mouse over")]
        [DefaultValue(true)]
        public bool DisplayNpcName;

        [Label("Display Town names")]
        [Tooltip("Display Town Npc information at all time and detailed information when mouse over")]
        [DefaultValue(true)]
        public bool DisplayTownName;

        [Label("Hide Old Bar")]
        [Tooltip("Hide The vanilla game HealthBar")]
        [DefaultValue(true)]
        public bool HideVanillaHB;


        public override void OnLoaded()
        {
            SummonHeartMod.visualConfig = this;
        }
        public override void OnChanged()
        {
            if (Main.netMode != NetmodeID.Server)
            {
                SummonHeartMod.visualConfig = this;
                if (SummonHeartMod.Instance != null && SummonHeartMod.Instance.healthBar != null)
                {
                    SummonHeartMod.Instance.healthBar.Reset();
                    SummonHeartMod.Instance.openStatMenu.Reset();
                }
            }
        }
    }

    [Label("修仙者配置")]
    public class GamePlayConfig : ModConfig
    {

        // You MUST specify a MultiplayerSyncMode.
        public override ConfigScope Mode => ConfigScope.ServerSide;

      
        [Label("开启修仙")]
        [Tooltip("Enable all player related RPG elements")]
        [DefaultValue(true)]
        public bool RPGPlayer;
      

        public override void OnLoaded()
        {
            SummonHeartMod.gpConfig = this;
        }
        public override void OnChanged()
        {
            SummonHeartMod.gpConfig = this;
        }
    }
    [Label("修仙者敌人配置")]
    public class NPCConfig : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ServerSide;

        [Label("Npc Progression")]
        [Tooltip("Npc level enable/Disable")]
        [DefaultValue(true)]
        public bool NPCProgress;

        [Label("NPC Rank")]
        [Tooltip("Enable NPC rank, like : Alpha, Legendary ect ...")]
        [DefaultValue(true)]
        public bool NPCRarity;

        [Label("NPC Modifier")]
        [Tooltip("Enable NPC modifier, like : The golden, Clustered ect ...")]
        [DefaultValue(true)]
        public bool NPCModifier;

        [Label("Boss Rarity")]
        [Tooltip("Apply Rarity to boss")]
        [DefaultValue(true)]
        public bool BossRarity;

        [Label("Boss Modifier")]
        [Tooltip("Apply modifier to boss")]
        [DefaultValue(true)]
        public bool BossModifier;

        [Label("Boss Clustered")]
        [Tooltip("Enable Clustered modifier on boss, it's disable since most people don't want an army of boss spawning after killing one")]
        [DefaultValue(false)]
        public bool BossClustered;

        [Label("Limit NPC growth")]
        [Tooltip("If activated prevent npc to have level too high than player based on Limit NPC growth Value")]
        [DefaultValue(true)]
        public bool LimitNPCGrowth;


        [Label("Limit NPC growth Value")]
        [Tooltip("If Limit Npc Growth is actiaved, limit npc level by your level + this value + level X Growth Percent")]
        [Range(0, int.MaxValue)]
        [Increment(10)]
        [DefaultValue(20)]
        public int LimitNPCGrowthValue;


        [Label("Limit NPC growth Percent")]
        [Tooltip("If Limit Npc Growth is actiaved, limit npc level by your level + Growth Value + level X Growth Percent ")]
        [Range(0f, 200f)]
        [Increment(5f)]
        [DefaultValue(20f)]
        public float LimitNPCGrowthPercent;

        [Label("Npc Level Multiplier")]
        [Tooltip("Multiply all npc level by this value")]
        [Range(0.1F, 50F)]
        [Increment(.25f)]
        [DefaultValue(1f)]
        public float NpclevelMultiplier;

        [Label("Npc Projectile Level")]
        [Tooltip("Used as a workd-arround to scale NPC projectile, tweak this value if needed")]
        [Range(1, 2500)]
        [Increment(10)]
        [DefaultValue(10)]
        public int NPCProjectileDamageLevel;

        [Label("Npc Damage Multiplier")]
        [Tooltip("Multiply all npc Damage by this value")]
        [Range(0.1F, 50F)]
        [Increment(.25f)]
        [DefaultValue(1f)]
        public float NpcDamageMultiplier;

        [Label("Npc Health Multiplier")]
        [Tooltip("Multiply all npc Health by this value")]
        [Range(0.1F, 50F)]
        [Increment(.25f)]
        [DefaultValue(1f)]
        public float NpcHealthMultiplier;

        [Label("Boss Health multiplier")]
        [Tooltip("Multiplier to boss health")]
        [Range(0.1f, 10f)]
        [Increment(.1f)]
        [DefaultValue(1f)]
        public float BossHealthMultiplier;


        [Label("NPC growth Per Boss")]
        [Tooltip("How many level the world gain when killing a boss")]
        [Range(1, int.MaxValue)]
        [Increment(5)]
        [DefaultValue(15)]
        public int NPCGrowthValue;

        [Label("NPC growth OnHardMode")]
        [Tooltip("How many level the world gain when entering hardmode")]
        [Range(1, int.MaxValue)]
        [Increment(5)]
        [DefaultValue(50)]
        public int NPCGrowthHardMode;

        [Label("NPC growth Hard Mode Percent")]
        [Tooltip("Multiply the world level by this value (applied before \"NPC growth OnHardMode\")")]
        [Range(1f, 10f)]
        [Increment(0.1f)]
        [DefaultValue(1.1f)]
        public float NPCGrowthHardModePercent;

        public override void OnLoaded()
        {
            SummonHeartMod.NPCConfig = this;
        }
        public override void OnChanged()
        {
            SummonHeartMod.NPCConfig = this;

            //JsonSkillTree.Load();
            //JsonCharacterClass.Load();
        }
    }


    public class Config
    {
        static public VisualConfig vConfig
        {
            get { return SummonHeartMod.visualConfig; }
        }
        static public GamePlayConfig gpConfig
        {
            get { return SummonHeartMod.gpConfig; }
        }
        static public NPCConfig NPCConfig
        {
            get { return SummonHeartMod.NPCConfig; }
        }

    }
}
