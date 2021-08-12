﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SummonHeart.Extensions;
using System;
using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using static SummonHeart.SummonHeartMod;

namespace SummonHeart.NPCs
{
    public class SummonHeartGlobalNPC : GlobalNPC
    {
		public override bool InstancePerEntity
		{
			get
			{
				return true;
			}
		}

		public int soulSplitCount = 0;

		public override void SetDefaults(NPC npc)
		{
            bool overLoadConfig = SummonHeartConfig.Instance.OverLoadConfig;
			if(SummonHeartWorld.WorldLevel <= 1)
            {
				npc.lifeMax *= 5;
				npc.defense *= 5;
				npc.damage *= 2;
            }
			else if(SummonHeartWorld.WorldLevel == 2)
			{
				if(overLoadConfig)
					npc.lifeMax *= 10;
				else if (npc.lifeMax * 10 >= 2100000000)
					npc.lifeMax = 2100000000;
				else
					npc.lifeMax *= 10;
				npc.defense *= 10;
				npc.damage *= 5;
			}
			else if (SummonHeartWorld.WorldLevel == 3)
			{
				if (overLoadConfig)
					npc.lifeMax *= 15;
				else if (npc.lifeMax * 15 >= 2100000000)
					npc.lifeMax = 2100000000;
				else
					npc.lifeMax *= 15;
				npc.defense *= 15;
				npc.damage *= 8;
			}
			else if (SummonHeartWorld.WorldLevel == 4)
			{
				if (overLoadConfig)
					npc.lifeMax *= 20;
				else if (npc.lifeMax * 20 >= 2100000000)
					npc.lifeMax = 2100000000;
				else
					npc.lifeMax *= 20;
				npc.defense *= 20;
				npc.damage *= 10;
			}
			else if (SummonHeartWorld.WorldLevel == 5)
			{
				if (overLoadConfig)
					npc.lifeMax *= 30;
				else if (npc.lifeMax * 30 >= 2100000000)
					npc.lifeMax = 2100000000;
				else
					npc.lifeMax *= 30;
				npc.defense *= 30;
				npc.damage *= 30;
			}
            
		}

		public void SyncPlayerNpcVar(Player player, NPC npc)
		{
			SummonHeartPlayer modPlayer = player.GetModPlayer<SummonHeartPlayer>();
			ModPacket packet = mod.GetPacket();
			packet.Write((byte)4);
			packet.Write((byte)player.whoAmI);
			packet.Write((byte)npc.whoAmI);
			packet.Send();
		}

		public override void NPCLoot(NPC npc)
		{
			Player player = Main.player[npc.lastInteraction];
			SummonHeartPlayer modPlayer = player.GetModPlayer<SummonHeartPlayer>();

			if (Main.netMode == NetmodeID.Server)
			{
				SyncPlayerNpcVar(player, npc);
            }
            else
            {
				player.doKillNpcExp(npc);
			}
		}

		private void dealLevel(SummonHeartPlayer modPlayer)
        {
			int lvExp = 10;
			int exp = lvExp;
			int level = 0;
            while (exp <= modPlayer.BBP)
            {
				exp += lvExp;
				level++;
				lvExp += 10;
            }
			int needExp = exp - modPlayer.BBP;
			modPlayer.exp = needExp;
			modPlayer.SummonCrit = level;
			if (!Main.hardMode && modPlayer.SummonCrit > 299)
				modPlayer.SummonCrit = 299;
			if (Main.hardMode && modPlayer.SummonCrit > 499)
				modPlayer.SummonCrit = 500;
		}

        public override void EditSpawnRate(Player player, ref int spawnRate, ref int maxSpawns)
        {
            SummonHeartPlayer modPlayer = player.GetModPlayer<SummonHeartPlayer>();
            if (modPlayer.BattleCry)
            {
                spawnRate = (int)(spawnRate * 1.0f / 20);
                maxSpawns = (int)(maxSpawns * 20f);
            }
        }

		public void SyncNpcVariables(NPC npc)
		{
			ModPacket packet = mod.GetPacket();
			packet.Write((byte)1);
			packet.Write((byte)npc.whoAmI);
			packet.Write(npc.life);
			packet.Send();
		}

		public void SyncKillResourceCount(Player player, int killResourceCount)
		{
			ModPacket packet = mod.GetPacket();
			packet.Write((byte)3);
			packet.Write((byte)player.whoAmI);
			packet.Write(killResourceCount);
			packet.Send();

		}

		public void CauseDirectDamage(NPC npc, int originalDamage, bool crit, int addRealDmage)
		{
			Player player = Main.player[Main.myPlayer];
			SummonHeartPlayer modPlayer = player.GetModPlayer<SummonHeartPlayer>();

			int num = 0;
			if (crit)
				originalDamage *= 2;

			if (modPlayer.SummonHeart)
				num = originalDamage * modPlayer.SummonCrit / 5000 + modPlayer.SummonCrit / 5 + SummonHeartWorld.WorldLevel * 5;

			if (modPlayer.PlayerClass == 3 && modPlayer.boughtbuffList[1])
				num += 10 + modPlayer.handBloodGas / 2000;

			num += addRealDmage;

			if (num >= 1)
			{
				CombatText.NewText(new Rectangle((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height), new Color(240, 20, 20, 255), string.Concat(num), false, true);
				npc.life -= num;
				if (npc.life <= 0)
				{
					npc.life = 1;
					/*npc.StrikeNPC(9999, 0f, 0, false, false, false);
					if (Main.netMode == 2)
					{
						NetMessage.SendData(28, -1, -1, null, npc.whoAmI, 9999f);
					}*/
				}
				if (Main.netMode == 1)
				{
					SyncNpcVariables(npc);
				}
			}
		}

		public void CauseRealDamage(NPC npc, int realDamage)
		{
			Player player = Main.player[Main.myPlayer];
			SummonHeartPlayer modPlayer = player.GetModPlayer<SummonHeartPlayer>();
			if (realDamage >= 1)
			{
				CombatText.NewText(new Rectangle((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height), new Color(240, 20, 20, 255), string.Concat(realDamage), false, true);
				npc.life -= realDamage;
				if (npc.life <= 0)
				{
					npc.life = 1;
					npc.AddBuff(mod.BuffType("SoulSplit"), 200);
				}
				if (Main.netMode == 1)
				{
					SyncNpcVariables(npc);
				}
			}
		}

		public override void UpdateLifeRegen(NPC npc, ref int damage)
		{
			if (npc.HasBuff(mod.BuffType("SoulSplit")))
            {
				Player player = Main.player[Main.myPlayer];
				SummonHeartPlayer modPlayer = player.GetModPlayer<SummonHeartPlayer>();

				int lifeDmage = 2 * modPlayer.SummonCrit / 50 * soulSplitCount;
				if (lifeDmage < 2)
					lifeDmage = 2;
				npc.lifeRegen -= lifeDmage;
				damage = soulSplitCount;
				/*if (npc.life == 1 && Main.netMode == 1 || Main.netMode == 2)
				{
					NetMessage.SendData(28, -1, -1, null, npc.whoAmI, 9999f);
				}*/
				if (Main.netMode == 1)
				{
					SyncNpcVariables(npc);
				}
			}
		}

        public override void DrawEffects(NPC npc, ref Color drawColor)
        {
			if (npc.HasBuff(mod.BuffType("EyeBuff")))
			{
				if (Main.rand.Next(4) < 3)
				{
					int dust = Dust.NewDust(new Vector2(npc.position.X - 2f, npc.position.Y - 2f), npc.width + 4, npc.height + 4, DustID.Blood, npc.velocity.X * 0.4f, npc.velocity.Y * 0.4f, 100);
					Main.dust[dust].noGravity = true;
					Main.dust[dust].shader = GameShaders.Armor.GetSecondaryShader(56, Main.LocalPlayer);

					Dust expr_1CCF_cp_0 = Main.dust[dust];
					expr_1CCF_cp_0.velocity.Y = expr_1CCF_cp_0.velocity.Y - 0.5f;
					if (Main.rand.Next(4) == 0)
					{
						Main.dust[dust].noGravity = false;
						Main.dust[dust].scale *= 0.5f;
					}
				}
				Player player = Main.player[Main.myPlayer];
				SummonHeartPlayer modPlayer = player.GetModPlayer<SummonHeartPlayer>();
				if (Main.time % 6 == 0)
                {
                    int dmage = modPlayer.eyeBloodGas / 1000 + 1;
					this.CauseRealDamage(npc, dmage);
					if (modPlayer.soulSplit)
					{
						if (!npc.HasBuff(mod.BuffType("SoulSplit")))
						{
							soulSplitCount = 1;
						}
						npc.AddBuff(mod.BuffType("SoulSplit"), 200);
					}
				}
			}
		}

        public override void ModifyHitByItem(NPC npc, Player player, Item item, ref int damage, ref float knockback, ref bool crit)
        {
			SummonHeartPlayer modPlayer = player.GetModPlayer<SummonHeartPlayer>();
			if (npc.HasBuff(mod.BuffType("EyeBuff")))
			{
				crit = true;
			}
			if (modPlayer.soulSplit)
			{
				if (!npc.HasBuff(mod.BuffType("SoulSplit")))
				{
					soulSplitCount = 1;
				}
				npc.AddBuff(mod.BuffType("SoulSplit"), 200);
			}
			if (crit)
			{
				damage = (int)(damage * modPlayer.MyCritDmageMult);
			}
			if (item.modItem != null && item.modItem.Name == "Raiden")
			{
				if (modPlayer.PlayerClass == 2 && modPlayer.chargeAttack)
				{
					damage += modPlayer.shortSwordBlood * modPlayer.killResourceMulti;
				}
			}
			if (SummonHeartWorld.GoddessMode)
			{
				damage /= 3;
			}
			this.CauseDirectDamage(npc, damage, crit, 0);
		}

        public override void ModifyHitByProjectile(NPC npc, Projectile projectile, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
			Mod Calamity = ModLoader.GetMod("CalamityMod");
			Player player = Main.player[projectile.owner];
            SummonHeartPlayer modPlayer = player.GetModPlayer<SummonHeartPlayer>();
			if (npc.HasBuff(mod.BuffType("EyeBuff")))
            {
				crit = true;
            }
			if (modPlayer.soulSplit)
			{
				if (!npc.HasBuff(mod.BuffType("SoulSplit")))
				{
					soulSplitCount = 1;
				}
				npc.AddBuff(mod.BuffType("SoulSplit"), 200);
			}
			if (crit)
			{
				damage = (int)(damage * modPlayer.MyCritDmageMult);
			}
			//神灭
			int addRealDmage = 0;
			if (projectile.modProjectile != null && projectile.modProjectile.Name == "DemonFlySwordMinion")
			{
				if (modPlayer.PlayerClass == 3)
				{
					addRealDmage += modPlayer.flySwordBlood;
				}
			}
			//投手附加伤害
			if (modPlayer.PlayerClass == 2 && player.HeldItem.thrown == true && projectile.thrown)
			{
				int killCost = modPlayer.killResourceMax2 / 100;
				if (modPlayer.killResourceCurrent >= killCost)
				{
					modPlayer.killResourceCurrent -= killCost;
					//转换死气值
					float addDeath = killCost / 2;
					if (addDeath < 1)
						addDeath = 1;
					modPlayer.deathResourceCurrent += (int)addDeath;
					if (modPlayer.deathResourceCurrent > modPlayer.deathResourceMax)
						modPlayer.deathResourceCurrent = modPlayer.deathResourceMax;
					int realDmage = killCost * modPlayer.killResourceMulti;
					addRealDmage += realDmage;
				}
			}
			if (SummonHeartWorld.GoddessMode)
			{
				damage /= 3;
				addRealDmage /= 3;
			}
			this.CauseDirectDamage(npc, damage, crit, addRealDmage);
		}
    }
}
