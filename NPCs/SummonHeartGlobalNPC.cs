using Microsoft.Xna.Framework;
using Terraria;
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
			npc.lifeMax *= SummonHeartConfig.Instance.hpDefMultiplier;
			npc.defense *= SummonHeartConfig.Instance.hpDefMultiplier;
			npc.damage *= SummonHeartConfig.Instance.atkMultiplier;
			base.SetDefaults(npc);
		}

		public void SyncPlayerVariables(Player player)
		{			
			SummonHeartPlayer modPlayer = player.GetModPlayer<SummonHeartPlayer>();
			ModPacket packet = mod.GetPacket();
			packet.Write((byte)player.whoAmI);
			packet.Write(modPlayer.BBP);
			packet.Write(modPlayer.SummonCrit);
			packet.Write(modPlayer.exp);
			packet.Write(modPlayer.bodyDef);
			packet.Write(modPlayer.eyeBloodGas);
			packet.Write(modPlayer.handBloodGas);
			packet.Write(modPlayer.bodyBloodGas);
			packet.Write(modPlayer.footBloodGas);
			packet.Write(modPlayer.practiceEye);
			packet.Write(modPlayer.practiceHand);
			packet.Write(modPlayer.practiceBody);
			packet.Write(modPlayer.practiceFoot);
			packet.Write(modPlayer.soulSplit);
			for (int i = 0; i < modPlayer.boughtbuffList.Count; i++)
			{
				packet.Write(modPlayer.boughtbuffList[i]);
			}
			packet.Send();
		}

        public override void NPCLoot(NPC npc)
		{
			Player player = Main.player[npc.lastInteraction];
			SummonHeartPlayer modPlayer = player.GetModPlayer<SummonHeartPlayer>();

			if (modPlayer.SummonHeart)
			{
				int addExp = 0;
				int addBloodGas = 0;
				if (!Main.hardMode && modPlayer.SummonCrit < 299)
				{
					if (npc.boss)
					{
						addExp = npc.lifeMax / 100;
						if (addExp > modPlayer.SummonCrit * 20)
							addExp = modPlayer.SummonCrit * 20;
						if (addExp < modPlayer.SummonCrit * 5)
							addExp = modPlayer.SummonCrit * 5;
                    }
                    else
                    {
						addExp = 1;
					}
				}
				if (Main.hardMode && modPlayer.SummonCrit < 500)
				{
					if (npc.boss)
					{
						addExp = npc.lifeMax / 100;
						if (addExp > modPlayer.SummonCrit * 10)
							addExp = modPlayer.SummonCrit * 10;
					}
					else
					{
						addExp = 1;
					}
				}
				//处理灵魂
				if (SummonHeartWorld.GoddessMode)
					addExp *= 10;
				modPlayer.BBP += addExp;
				addBloodGas = addExp;
				if (SummonHeartWorld.GoddessMode)
					addBloodGas /= 10;
				//最大血气上限
				int maxBloodGas = 100000;
				//修炼魔神之眼
				if (modPlayer.practiceEye)
				{
					if (modPlayer.eyeBloodGas < maxBloodGas)
					{
						if (modPlayer.CheckSoul(addExp)){
							modPlayer.BuySoul(addExp);
							modPlayer.eyeBloodGas += addBloodGas;
                        }
					}
				}
				//修炼魔神之手
				if (modPlayer.practiceHand)
				{
					if (modPlayer.handBloodGas < maxBloodGas)
					{
						if (modPlayer.CheckSoul(addExp))
						{
							modPlayer.BuySoul(addExp);
							modPlayer.handBloodGas += addBloodGas;
						}
					}
				}
				//修炼魔神之躯
				if (modPlayer.practiceBody)
				{
					if (modPlayer.bodyBloodGas < maxBloodGas)
					{
						if (modPlayer.CheckSoul(addExp))
						{
							modPlayer.BuySoul(addExp);
							modPlayer.bodyBloodGas += addBloodGas;
						}
					}
				}
				//修炼魔神之腿
				if (modPlayer.practiceFoot)
				{
					if (modPlayer.footBloodGas < maxBloodGas)
					{
						if (modPlayer.CheckSoul(addExp))
						{
							modPlayer.BuySoul(addExp);
							modPlayer.footBloodGas += addBloodGas;
						}
					}
				}

				dealLevel(modPlayer);
				if (Main.netMode == 2)
				{
					SyncPlayerVariables(player);
				}
				if (npc.boss)
                {
					Main.NewText($"你吞噬了{npc.FullName}的灵魂，灵魂之力+{addExp}", Color.Green);
					if(modPlayer.practiceEye)
						Main.NewText($"你修炼魔神之眼消耗{addExp}灵魂之力吞噬了{npc.FullName}的气血，魔神之眼气血+{addExp}", Color.Green);
					if (modPlayer.practiceHand)
						Main.NewText($"你修炼魔神之手消耗{addExp}灵魂之力吞噬了{npc.FullName}的气血，魔神之手气血+{addBloodGas}", Color.Green);
					if (modPlayer.practiceBody)
						Main.NewText($"你修炼魔神之躯消耗{addExp}灵魂之力吞噬了{npc.FullName}的气血，魔神之躯气血+{addBloodGas}", Color.Green);
					if (modPlayer.practiceFoot)
						Main.NewText($"你修炼魔神之腿消耗{addExp}灵魂之力吞噬了{npc.FullName}的气血，魔神之腿气血+{addBloodGas}", Color.Green);
				}
			}
		}

		private void dealLevel(SummonHeartPlayer modPlayer)
        {
			int lvExp = 1;
			int exp = lvExp;
			int level = 0;
            while (exp <= modPlayer.BBP)
            {
				exp += lvExp;
				level++;
				lvExp += 1;
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

        public override void ModifyHitByItem(NPC npc, Player player, Item item, ref int damage, ref float knockback, ref bool crit)
        {
			SummonHeartPlayer modPlayer = player.GetModPlayer<SummonHeartPlayer>();
			if (modPlayer.SummonHeart)
			{
				npc.defense *= (1 - modPlayer.SummonCrit / 500);
			}

			if (modPlayer.soulSplit)
			{
				if (!npc.HasBuff(mod.BuffType("SoulSplit")))
				{
					soulSplitCount = 1;
				}
				npc.AddBuff(mod.BuffType("SoulSplit"), 2);
			}
		}

        public override void ModifyHitByProjectile(NPC npc, Projectile projectile, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
			Mod Calamity = ModLoader.GetMod("CalamityMod");
			Player player = Main.player[projectile.owner];
            SummonHeartPlayer modPlayer = player.GetModPlayer<SummonHeartPlayer>();

			if (modPlayer.SummonHeart)
            {
				npc.defense *= (1 - modPlayer.SummonCrit / 500);

				//欺负大幻海妖蛇
				if (Calamity != null)
				{
					if (npc.type == 594)
					{
						damage *= 100;
						damage *= 10000;
					}
				}
			}

			if (modPlayer.soulSplit)
			{
				if (!npc.HasBuff(mod.BuffType("SoulSplit")))
				{
					soulSplitCount = 1;
				}
				npc.AddBuff(mod.BuffType("SoulSplit"), 2);
			}

			if (projectile.minion && Main.rand.Next(101) <= modPlayer.eyeBloodGas / 1000)
			{
				crit = true;
			}
		}
    }
}
