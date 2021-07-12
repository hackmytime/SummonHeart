using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ModLoader;
using Terraria.Utilities;
using Microsoft.Xna.Framework;
using System;

namespace SummonHeart
{
    public abstract class KillItem : ModItem
    {
        internal Player player;
        private NPC _npc;
        public bool isFistWeapon;
        public bool canUseHeavyHit;
        public float kiDrain;
        public string weaponType;
        #region Boss bool checks
        public bool eyeDowned;
        public bool beeDowned;
        public bool wallDowned;
        public bool plantDowned;
        public bool dukeDowned;
        public bool moodlordDowned;
        public override void PostUpdate()
        {
            if (NPC.downedBoss1)
            {
                eyeDowned = true;
            }
            if (NPC.downedQueenBee)
            {
                beeDowned = true;
            }
            if (Main.hardMode)
            {
                wallDowned = true;
            }
            if (NPC.downedPlantBoss)
            {
                plantDowned = true;
            }
            if (NPC.downedFishron)
            {
                dukeDowned = true;
            }
            if (NPC.downedMoonlord)
            {
                moodlordDowned = true;
            }
            if (item.channel)
            {
                item.autoReuse = false;
            }
        }
        #endregion
        #region FistAdditions 

        #endregion

        public override void SetDefaults()
        {
            item.melee = false;
            item.ranged = false;
            item.magic = false;
            item.thrown = false;
            item.summon = false;
        }

        public override bool CloneNewInstances
        {
            get
            {
                return true;
            }
        }

        public override bool AllowPrefix(int pre)
        {
            return false;
        }

        public override bool? PrefixChance(int pre, UnifiedRandom rand)
        {
            return new bool?(false);
        }

        public override void GetWeaponDamage(Player player, ref int damage)
        {
            SummonHeartPlayer modPlayer = player.GetModPlayer<SummonHeartPlayer>();
            damage = modPlayer.shortSwordBlood;
        }

        public override void GetWeaponCrit(Player player, ref int crit)
        {
            crit = 0;
        }

        /*public override float UseTimeMultiplier(Player player)
        {
            return MyPlayer.ModPlayer(player).kiSpeedAddition;
        }*/


        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            TooltipLine tt = tooltips.FirstOrDefault(x => x.Name == "Damage" && x.mod == "Terraria");
            if (tt != null)
            {
                string[] splitText = tt.text.Split(' ');
                string damageValue = splitText.First();
                string damageWord = splitText.Last();
                tt.text = damageValue + " 刺杀" + damageWord;
            }
        }
    }
}
