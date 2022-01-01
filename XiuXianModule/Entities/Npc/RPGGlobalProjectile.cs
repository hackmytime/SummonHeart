﻿using System;
using Terraria;
using Terraria.ModLoader;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria.ID;
using SummonHeart.Utilities;

namespace SummonHeart.XiuXianModule.Entities.Npc
{
    class RPGGlobalProjectile : GlobalProjectile
    {
        bool init = false;

        public Item itemOrigin;

        public override bool InstancePerEntity
        {
            get
            {
                return true;
            }
        }


        public override void ModifyHitPlayer(Projectile projectile, Player target, ref int damage, ref bool crit)
        {
            int projectilelevel = (int)((WorldManager.GetWorldLevelMultiplier(Config.NPCConfig.NPCProjectileDamageLevel) + WorldManager.GetWorldAdditionalLevel()) * Config.NPCConfig.NpclevelMultiplier);


            /*
            debug
            Main.NewText("projectile base level : " + Config.NPCConfig.NPCProjectileDamageLevel);
            Main.NewText("World Day : " + WorldManager.Day);
            Main.NewText("projectile day level : " + WorldManager.GetWorldLevelMultiplier(Config.NPCConfig.NPCProjectileDamageLevel));
            Main.NewText("projectile World level : " + WorldManager.GetWorldAdditionalLevel());
            Main.NewText("npc level multiplier : " + Config.NPCConfig.NpclevelMultiplier);

            Main.NewText("projectile level : " + (int)(WorldManager.GetWorldLevelMultiplier(Config.NPCConfig.NPCProjectileDamageLevel) * Config.NPCConfig.NpclevelMultiplier));
            Main.NewText("projectile base damage : " + projectile.damage);
            Main.NewText("projectile damage multiplier : " + Mathf.Pow(1 + projectilelevel * 0.02f, 0.95f) * Config.NPCConfig.NpcDamageMultiplier);
            */

            damage = Mathf.HugeCalc(Mathf.FloorInt(projectile.damage * (1 + projectilelevel * 0.05f) * Config.NPCConfig.NpcDamageMultiplier), projectile.damage);
        }

        public override void ModifyHitNPC(Projectile projectile, NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            if (projectile.npcProj)
            {
                int projectilelevel = (int)(WorldManager.GetWorldLevelMultiplier(Config.NPCConfig.NPCProjectileDamageLevel) * Config.NPCConfig.NpclevelMultiplier);

                damage = Mathf.HugeCalc(Mathf.FloorInt(projectile.damage * Mathf.Pow(1 + projectilelevel * 0.02f, 0.95f) * Config.NPCConfig.NpcDamageMultiplier), projectile.damage);
            }
        }

        /*
        public override void SetDefaults(Projectile projectile)
        {
            if (projectile.npcProj)
            {
                int projectilelevel = WorldManager.GetWorldLevelMultiplier(1);

                projectile.damage = Mathf.HugeCalc(Mathf.FloorInt(projectile.damage * Mathf.Pow(1 + projectilelevel * 0.02f, 0.95f)), projectile.damage);
            }
            base.SetDefaults(projectile);
        }
        */

        public override bool PreAI(Projectile projectile)
        {


            if (init)
                return base.PreAI(projectile);
            if (projectile.friendly)
                return base.PreAI(projectile);

            if (!projectile.npcProj && projectile.minion)
            {
                Player p = Main.player[projectile.owner];
                itemOrigin = p.HeldItem;
            }



            init = true;


            return base.PreAI(projectile);
        }
    }
}
