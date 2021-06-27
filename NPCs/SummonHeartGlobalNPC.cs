using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SummonHeart.Extensions;
using Terraria;
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
			packet.Write(modPlayer.bloodGasMax);
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
				int powerLevel = npc.getPowerLevel();
				
				if (npc.boss)
				{
					if(powerLevel == -1)
                    {
						addExp = 1;
						
					}
                    else
                    {
						addExp = npc.getPower() / 100;
						if (Main.hardMode)
						{
							addExp = npc.getPower() / 200;
						}
						if (NPC.downedMoonlord)
						{
							addExp = npc.getPower() / 1000;
						}
                    }
                }
                else
                {
					addExp = 1;
				}
				//越阶战斗奖励
				if (powerLevel > 0)
				{
					addExp *= (powerLevel + 1);
				}

				//处理灵魂
				//处理难度额外灵魂
				int hardMulti = SummonHeartConfig.Instance.hpDefMultiplier / 5;
				addExp *= hardMulti;
				modPlayer.BBP += addExp;

                if (npc.boss)
                {
					if(powerLevel == -1)
                    {
						Main.NewText($"你的战力碾压{npc.FullName}，可惜，其血肉灵魂已于你无用！灵魂之力+{addExp}", Color.Green);
					}
					if(powerLevel == 0)
                    {
						Main.NewText($"你吞噬了{npc.FullName}的灵魂，灵魂之力+{addExp}", Color.Green);
                    }
					if (powerLevel > 0)
					{
						Main.NewText($"你越级吞噬了{npc.getPowerLevelText()}强者{npc.FullName}的灵魂，获得额外{powerLevel}倍灵魂之力，+{addExp}灵魂之力", Color.Green);
					}
				}

				//处理突破
				//最大血气上限【规则】
				int MAXBLOODGAS = 100000 + (SummonHeartConfig.Instance.atkMultiplier * 10000);
				if (powerLevel > 0 && npc.getPower() > modPlayer.bloodGasMax)
                {
					//突破的数值=（敌人战力-玩家肉身极限）/  5 * (阶位)
					int addMax = (npc.getPower() - modPlayer.bloodGasMax) / 5;
					
					if (powerLevel >= 5)
						addMax = npc.getPower() - modPlayer.bloodGasMax;
                    else
                    {
						addMax *= (powerLevel);
					}
					modPlayer.bloodGasMax += addMax;
					//判断是否超过世界上限
					if (modPlayer.bloodGasMax > MAXBLOODGAS * 4)
						modPlayer.bloodGasMax = MAXBLOODGAS * 4;
					Main.NewText($"你越级斩杀了{npc.getPowerLevelText()}强者{npc.FullName}，于生死之间突破肉身极限，+{addMax}肉身极限", Color.Red);
				}

				//修炼开始
				int bloodGasMax = modPlayer.bloodGasMax;
				
				if(modPlayer.getAllBloodGas() < bloodGasMax)
                {
					addBloodGas = addExp;
					
					//修炼魔神之眼
					if (modPlayer.practiceEye)
					{
						if (modPlayer.eyeBloodGas < MAXBLOODGAS)
						{
							//判断是否超上限
							if (modPlayer.getAllBloodGas() + addBloodGas > bloodGasMax)
							{
								addBloodGas = bloodGasMax - modPlayer.getAllBloodGas();
							}
							if (modPlayer.CheckSoul(addExp))
							{
								modPlayer.BuySoul(addExp);
								modPlayer.eyeBloodGas += addBloodGas;
								if (npc.boss)
								{
									if (powerLevel == 0)
									{
										Main.NewText($"你修炼魔神之眼消耗{addExp}灵魂之力吞噬了{npc.FullName}的气血，魔神之眼气血+{addBloodGas}", Color.Green);
									}
									if (powerLevel > 0)
									{
										Main.NewText($"你修炼魔神之眼消耗{addExp}灵魂之力越级吞噬了{npc.getPowerLevelText()}强者{npc.FullName}的气血，额外吸收{powerLevel}倍气血，魔神之眼气血+{addBloodGas}", Color.Green);
									}
								}
							}
						}
					}
					//修炼魔神之手
					if (modPlayer.practiceHand)
					{
						if (modPlayer.handBloodGas < MAXBLOODGAS)
						{
							//判断是否超上限
							if (modPlayer.getAllBloodGas() + addBloodGas > bloodGasMax)
							{
								addBloodGas = bloodGasMax - modPlayer.getAllBloodGas();
							}
							if (modPlayer.CheckSoul(addExp))
							{
								modPlayer.BuySoul(addExp);
                                modPlayer.handBloodGas += addBloodGas;
								if (npc.boss)
                                {
									if (powerLevel == 0)
									{
										Main.NewText($"你修炼魔神之手消耗{addExp}灵魂之力吞噬了{npc.FullName}的气血，魔神之手气血+{addBloodGas}", Color.Green);
									}
									if (powerLevel > 0)
									{
										Main.NewText($"你修炼魔神之手消耗{addExp}灵魂之力越级吞噬了{npc.getPowerLevelText()}强者{npc.FullName}的气血，额外吸收{powerLevel}倍气血，魔神之手气血+{addBloodGas}", Color.Green);
									}
								}
							}
						}
					}
					//修炼魔神之躯
					if (modPlayer.practiceBody)
					{
						if (modPlayer.bodyBloodGas < MAXBLOODGAS)
						{
							//判断是否超上限
							if (modPlayer.getAllBloodGas() + addBloodGas > bloodGasMax)
							{
								addBloodGas = bloodGasMax - modPlayer.getAllBloodGas();
							}
							if (modPlayer.CheckSoul(addExp))
							{
								modPlayer.BuySoul(addExp);
                                modPlayer.bodyBloodGas += addBloodGas;
								if (npc.boss)
                                {
									if (powerLevel == 0)
									{
										Main.NewText($"你修炼魔神之躯消耗{addExp}灵魂之力吞噬了{npc.FullName}的气血，魔神之躯气血+{addBloodGas}", Color.Green);
									}
									if (powerLevel > 0)
									{
										Main.NewText($"你修炼魔神之躯消耗{addExp}灵魂之力越级吞噬了{npc.getPowerLevelText()}强者{npc.FullName}的气血，额外吸收{powerLevel}倍气血，魔神之躯气血+{addBloodGas}", Color.Green);
									}
								}
							}
						}
					}
					//修炼魔神之腿
					if (modPlayer.practiceFoot)
					{
						if (modPlayer.footBloodGas < MAXBLOODGAS)
						{
							//判断是否超上限
							if (modPlayer.getAllBloodGas() + addBloodGas > bloodGasMax)
							{
								addBloodGas = bloodGasMax - modPlayer.getAllBloodGas();
							}
							if (modPlayer.CheckSoul(addExp))
							{
								modPlayer.BuySoul(addExp);
                                modPlayer.footBloodGas += addBloodGas;
								if (npc.boss)
								{
									if (powerLevel == 0)
									{
										Main.NewText($"你修炼魔神之腿消耗{addExp}灵魂之力吞噬了{npc.FullName}的气血，魔神之腿气血+{addBloodGas}", Color.Green);
									}
									if (powerLevel > 0)
									{
										Main.NewText($"你修炼魔神之腿消耗{addExp}灵魂之力越级吞噬了{npc.getPowerLevelText()}强者{npc.FullName}的气血，额外吸收{powerLevel}倍气血，魔神之腿气血+{addBloodGas}", Color.Green);
									}
								}
							}
						}
					}
				}

				dealLevel(modPlayer);
				if (Main.netMode == 2)
				{
					SyncPlayerVariables(player);
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

			if (modPlayer.soulSplit)
			{
				if (!npc.HasBuff(mod.BuffType("SoulSplit")))
				{
					soulSplitCount = 1;
				}
				npc.AddBuff(mod.BuffType("SoulSplit"), 2);
			}
            if (modPlayer.boughtbuffList[0])
            {
				if (projectile.minion && Main.rand.Next(101) <= (modPlayer.eyeBloodGas + 30000) / 1500)
				{
					crit = true;
				}
            }
		}
    }
}
