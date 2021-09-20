using Microsoft.Xna.Framework;
using SummonHeart.Extensions;
using SummonHeart.Items.Weapons.Melee;
using SummonHeart.Projectiles.Melee;
using SummonHeart.Utilities;
using System;
using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

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
		public int extraUpdate = 0;

		public override void SetDefaults(NPC npc)
		{
			Mod fargowiltasSouls = ModLoader.GetMod("FargowiltasSouls");
			if (fargowiltasSouls != null && npc.type != 0 && npc.type == fargowiltasSouls.NPCType("MutantBoss"))
			{
				npc.lifeMax = 7000000;
			}
			if(SummonHeartWorld.WorldLevel <= 1)
            {
				npc.lifeMax *= 2;
				npc.value *= 1;
				npc.damage *= 4;
            }
			else if(SummonHeartWorld.WorldLevel == 2)
			{
				npc.lifeMax = (int)(npc.lifeMax * 3f);
				npc.value *= 2;
				npc.damage *= 8;
			}
			else if (SummonHeartWorld.WorldLevel == 3)
			{
				npc.lifeMax *= 4;
				npc.value *= 3;
				npc.damage *= 12;
			}
			else if (SummonHeartWorld.WorldLevel == 4)
			{
				npc.lifeMax = (int)(npc.lifeMax * 5f);
				npc.value *= 4;
				npc.damage *= 16;
			}
			else if (SummonHeartWorld.WorldLevel == 5)
			{
				npc.lifeMax *= 6;
				npc.value *= 5;
				npc.damage *= 30;
			}
            
		}

        public override void ResetEffects(NPC npc)
        {
			if(SummonHeartWorld.GoddessMode)
				extraUpdate = SHUtils.TransFloatToInt(0.33f);
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

        public override void PostAI(NPC npc)
        {
			if (extraUpdate > 0)
			{
				extraUpdate--;
				if (npc.active)
				{
					npc.AI();
					float num = 0.5f;
					Vector2 vector = npc.position + npc.velocity * num;
					if (!Collision.SolidCollision(vector, npc.width, npc.height))
					{
						npc.position = vector;
					}
				}
			}
		}

        public override void NPCLoot(NPC npc)
		{
			//处理物品掉落
			if (true)
			{
				/*if (npc.boss)
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, SummonHeartMod.Instance.ItemType("Loot1"), Main.rand.Next(10, 100));
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, SummonHeartMod.Instance.ItemType("Loot2"), Main.rand.Next(1, 5));
					if (Main.rand.Next(100) == 0)
					{
						Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, SummonHeartMod.Instance.ItemType("Loot3"), Main.rand.Next(1, 2));
					}
					if (Main.rand.Next(100) == 0)
					{
						Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, SummonHeartMod.Instance.ItemType("Loot4"), Main.rand.Next(1, 2));
					}
					if (Main.rand.Next(10000) == 0)
					{
						Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, SummonHeartMod.Instance.ItemType("Loot5"), Main.rand.Next(1, 2));
					}
					if (Main.rand.Next(10000) == 0)
					{
						Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, SummonHeartMod.Instance.ItemType("Loot6"), 1);
					}
				}
				else
				{
					Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, SummonHeartMod.Instance.ItemType("Loot1"), Main.rand.Next(1, 5));
				}*/
			}
			if (Main.netMode == NetmodeID.Server)
			{
				for (int k = 0; k < 255; k++)
				{
					Player player = Main.player[k];
					if (player.active)
					{
						SyncPlayerNpcVar(player, npc);
					}
				}
            }
            else
            {
				Player player = Main.player[npc.lastInteraction];
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
            {
				num = originalDamage * modPlayer.SummonCrit / 5000;
				if (modPlayer.PlayerClass == 3 && modPlayer.boughtbuffList[1])
					num = (int)(num * (2 + modPlayer.handBloodGas / 250 * 0.01f));

				num += modPlayer.SummonCrit / 50 + SummonHeartWorld.WorldLevel;
			}

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
			Player player = Main.player[Main.myPlayer];
			SummonHeartPlayer modPlayer = player.GetModPlayer<SummonHeartPlayer>();
			if (SummonHeartWorld.GoddessMode)
            {
				/*int heal = 0;
				//女神模式回血
				if (npc.boss)
                {
					int bossIndex = player.getDownedBossIndex();
					int healTime = 1000;
					if (bossIndex > 0)
                    {
						healTime -= bossIndex * 50;
						heal = (int)(npc.lifeMax / healTime);
                    }
                }
                else
                {
					heal = (int)Math.Ceiling(npc.lifeMax * SummonHeartWorld.WorldLevel * 0.01);
				}
				npc.lifeRegen += heal * 2;
				if (npc.HasBuff(mod.BuffType("SoulSplit")))
				{
					int lifeDmage = 2 * modPlayer.SummonCrit / 50 * soulSplitCount;
					if (lifeDmage < 2)
						lifeDmage = 2;
					npc.lifeRegen -= lifeDmage;
				}*/
			}
			else if (npc.HasBuff(mod.BuffType("SoulSplit")))
            {

				int lifeDmage = 2 * modPlayer.SummonCrit / 50 * soulSplitCount;
				if (lifeDmage < 2)
					lifeDmage = 2;
				npc.lifeRegen -= lifeDmage;
				damage = soulSplitCount;
				
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
			}
		}

        public override void ModifyHitByItem(NPC npc, Player player, Item item, ref int damage, ref float knockback, ref bool crit)
        {
			SummonHeartPlayer modPlayer = player.GetModPlayer<SummonHeartPlayer>();
			if (npc.HasBuff(mod.BuffType("EyeBuff")))
			{
				crit = true;
			}
			/*if (modPlayer.soulSplit)
			{
				if (!npc.HasBuff(mod.BuffType("SoulSplit")))
				{
					soulSplitCount = 1;
				}
				npc.AddBuff(mod.BuffType("SoulSplit"), 200);
			}*/
			if (crit)
			{
				damage = (int)(damage * modPlayer.MyCritDmageMult);
			}
			if (item.modItem != null && item.modItem.Name == "Raiden")
			{
				if (modPlayer.PlayerClass == 2 && modPlayer.chargeAttack)
				{
					damage *= modPlayer.killResourceMulti;
				}
			}
			int addRealDmage = modPlayer.addRealDamage;
			if (modPlayer.onDoubleDamage && modPlayer.PlayerClass == 1)
			{
				addRealDmage += modPlayer.damageResourceCurrent * 2;
				modPlayer.damageResourceCurrent = 0;
				modPlayer.onDoubleDamage = false;
			}
			//计算嫉妒词缀
			int addDamage = (int)(npc.damage * modPlayer.attackDamage);
			damage += addDamage;
			damage = (int)Math.Ceiling(damage / modPlayer.enemyDamageReduceMult);
			addRealDmage = (int)Math.Ceiling(addRealDmage / modPlayer.enemyDamageReduceMult);
			this.CauseDirectDamage(npc, damage, crit, addRealDmage);
		}

        public override void ModifyHitByProjectile(NPC npc, Projectile projectile, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
			Mod Calamity = ModLoader.GetMod("CalamityMod");
			Player player = Main.player[projectile.owner];
            SummonHeartPlayer modPlayer = player.GetModPlayer<SummonHeartPlayer>();
			if (player.HeldItem.modItem is DemonSword && projectile.type == ModContent.ProjectileType<DragonLegacyRed>())
			{
				if (modPlayer.PlayerClass == 1)
				{
					crit = true;
				}
				if (modPlayer.PlayerClass == 4)
                {
					int critChance = modPlayer.angerResourceCurrent;
					if(Main.rand.Next(101) <= critChance)
						crit = true;
                }
			}
			if (projectile.minion && modPlayer.PlayerClass == 3 && modPlayer.boughtbuffList[0])
			{
				int critChance = modPlayer.eyeBloodGas / 2500 + 20;
				if (Main.rand.Next(101) <= critChance)
					crit = true;
			}
			if (npc.HasBuff(mod.BuffType("EyeBuff")))
            {
				crit = true;
            }
			/*if (modPlayer.soulSplit)
			{
				if (!npc.HasBuff(mod.BuffType("SoulSplit")))
				{
					soulSplitCount = 1;
				}
				npc.AddBuff(mod.BuffType("SoulSplit"), 200);
			}*/
			if (crit)
			{
				damage = (int)(damage * modPlayer.MyCritDmageMult);
			}
			int addRealDmage = modPlayer.addRealDamage;
			//投手附加伤害
			if (modPlayer.PlayerClass == 2 && player.HeldItem.thrown == true && projectile.thrown)
			{
				int killCost = modPlayer.killResourceMax2 / 1000;
				if (modPlayer.killResourceCurrent >= killCost)
				{
					modPlayer.killResourceCurrent -= killCost;
					//转换死气值
					float addDeath = killCost / 100;
					if (addDeath < 1)
						addDeath = 1;
					modPlayer.deathResourceCurrent += (int)addDeath;
					if (modPlayer.deathResourceCurrent > modPlayer.deathResourceMax)
						modPlayer.deathResourceCurrent = modPlayer.deathResourceMax;
					int realDmage = killCost * modPlayer.killResourceMulti;
					addRealDmage += realDmage;
				}
			}
			if (modPlayer.onDoubleDamage && modPlayer.PlayerClass == 1 && projectile.owner == player.whoAmI)
			{
				addRealDmage += modPlayer.damageResourceCurrent * 2;
				modPlayer.damageResourceCurrent = 0;
				modPlayer.onDoubleDamage = false;
			}
			//计算嫉妒词缀
			int addDamage = (int)(npc.damage * modPlayer.attackDamage);
			damage += addDamage;
			damage = (int)Math.Ceiling(damage / modPlayer.enemyDamageReduceMult);
			addRealDmage = (int)Math.Ceiling(addRealDmage / modPlayer.enemyDamageReduceMult);
			this.CauseDirectDamage(npc, damage, crit, addRealDmage);
		}
    }
}
