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

        public void SyncPlayerVariables(Player player)
		{			
			SummonHeartPlayer modPlayer = player.GetModPlayer<SummonHeartPlayer>();
			ModPacket packet = mod.GetPacket();
			packet.Write((byte)0);
			packet.Write((byte)player.whoAmI);
			packet.Write(modPlayer.BBP);
			packet.Write(modPlayer.SummonCrit);
			packet.Write(modPlayer.exp);
			packet.Write(modPlayer.PlayerClass);
			packet.Write(modPlayer.bodyDef);
			packet.Write(modPlayer.eyeBloodGas);
			packet.Write(modPlayer.handBloodGas);
			packet.Write(modPlayer.bodyBloodGas);
			packet.Write(modPlayer.footBloodGas);
			packet.Write(modPlayer.bloodGasMax);
			packet.Write(modPlayer.swordBlood);
			packet.Write(modPlayer.shortSwordBlood);
			packet.Write(modPlayer.swordBloodMax);
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

			if(player.HeldItem.modItem != null && player.HeldItem.modItem.Name == "DemonSword")
            {
				if (npc.boss)
				{
					int swordMax = npc.lifeMax / 200;
					if (modPlayer.swordBloodMax < swordMax)
					{
						modPlayer.swordBloodMax = swordMax;
						string curMax = (modPlayer.swordBloodMax * 1.0f / 100f).ToString("0.00");
						string text = $"{player.name}手持魔剑·弑神吞噬了{npc.FullName}的血肉，突破觉醒上限，当前觉醒上限：{curMax}%";
						Main.NewText(text, Color.Green);
						if (Main.netMode == NetmodeID.Server)
						{
							NetMessage.BroadcastChatMessage(NetworkText.FromLiteral(text), Color.Green);
						}
					}
				}
				if (modPlayer.swordBlood < modPlayer.swordBloodMax)
				{
					modPlayer.swordBlood++;
				}
				if (Main.netMode == 2)
				{
					SyncPlayerVariables(player);
				}
			}
			if (player.HeldItem.modItem != null && player.HeldItem.modItem.Name == "Raiden")
			{
				if (npc.boss)
				{
					int swordMax = npc.lifeMax / 200;
					if (modPlayer.swordBloodMax < swordMax)
					{
						modPlayer.swordBloodMax = swordMax;
						string curMax = (modPlayer.swordBloodMax * 1.0f / 100f).ToString("0.00");
						string text = $"{player.name}手持魔剑·神陨吞噬了{npc.FullName}的血肉，突破觉醒上限，当前觉醒上限：{curMax}%";
						Main.NewText(text, Color.Green);
						if (Main.netMode == NetmodeID.Server)
						{
							NetMessage.BroadcastChatMessage(NetworkText.FromLiteral(text), Color.Green);
						}
					}
				}
				if (modPlayer.shortSwordBlood < modPlayer.swordBloodMax)
				{
					modPlayer.shortSwordBlood++;
				}
				if(modPlayer.PlayerClass == 2)
                {
					int heal = 0;
					if (modPlayer.boughtbuffList[0])
                    {
						heal += (modPlayer.eyeBloodGas / 2000 + 5);
					}
					if (modPlayer.boughtbuffList[1])
					{
						heal += (modPlayer.handBloodGas / 2000);
					}
					modPlayer.killResourceCurrent += heal;
				}
				if (Main.netMode == 2)
				{
					SyncPlayerVariables(player);
				}
			}

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
					if (powerLevel >= 99)
						powerLevel = 99;
					addExp *= (powerLevel + 1);
				}

				//处理灵魂
				//处理难度额外灵魂
				int hardMulti = SummonHeartWorld.WorldLevel;
				if (hardMulti > 0 && !npc.boss)
                {
					addExp *= hardMulti;
                }
				modPlayer.BBP += addExp;
				if (modPlayer.BBP > 5000000)
					modPlayer.BBP = 5000000;

				if (npc.boss)
                {
					string text = "";
					if(powerLevel == -1)
                    {
						text = $"{player.name}的战力碾压{npc.FullName}，可惜，其血肉灵魂已于{player.name}无用！灵魂之力+{addExp}";
					}
					if(powerLevel == 0)
                    {
						text = $"{player.name}吞噬了{npc.FullName}的灵魂，灵魂之力+{addExp}";
                    }
					if (powerLevel > 0)
					{
						text = $"{player.name}越级吞噬了{npc.getPowerLevelText()}强者{npc.FullName}的灵魂，获得额外{powerLevel}倍灵魂之力，+{addExp}灵魂之力";
					}

					Main.NewText(text, Color.Green);
					if (Main.netMode == NetmodeID.Server)
					{
						NetMessage.BroadcastChatMessage(NetworkText.FromLiteral(text), Color.Green);
					}
				}

				//处理突破
				//最大血气上限【规则】
				int WORLDMAXBLOODGAS = SummonHeartWorld.WorldBloodGasMax;
				//int MAXBLOODGAS = 200000;
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
					if (modPlayer.bloodGasMax > WORLDMAXBLOODGAS)
						modPlayer.bloodGasMax = WORLDMAXBLOODGAS;
                    string text = $"{player.name}越级斩杀了{npc.getPowerLevelText()}强者{npc.FullName}，于生死之间突破肉身极限，+{addMax}肉身极限";
					Main.NewText(text, Color.Green);
					if (Main.netMode == NetmodeID.Server)
					{
						NetMessage.BroadcastChatMessage(NetworkText.FromLiteral(text), Color.Red);
					}
				}

				//修炼开始
				int bloodGasMax = modPlayer.bloodGasMax;
				
				if(modPlayer.getAllBloodGas() < bloodGasMax)
                {
					addBloodGas = addExp;
					
					//修炼魔神之眼
					if (modPlayer.practiceEye)
					{
						if (modPlayer.eyeBloodGas < modPlayer.eyeMax)
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
								if (modPlayer.eyeBloodGas > modPlayer.eyeMax)
									modPlayer.eyeBloodGas = modPlayer.eyeMax;
								if (npc.boss)
								{
									string text = "";
									if (powerLevel == 0)
									{
										text = $"{player.name}修炼魔神之眼消耗{addExp}灵魂之力吞噬了{npc.FullName}的气血，魔神之眼气血+{addBloodGas}";
									}
									if (powerLevel > 0)
									{
										text = $"{player.name}修炼魔神之眼消耗{addExp}灵魂之力越级吞噬了{npc.getPowerLevelText()}强者{npc.FullName}的气血，额外吸收{powerLevel}倍气血，魔神之眼气血+{addBloodGas}";
									}
									Main.NewText(text, Color.Green);
									if (Main.netMode == NetmodeID.Server)
									{
										NetMessage.BroadcastChatMessage(NetworkText.FromLiteral(text), Color.Green);
									}
								}
							}
						}
					}
					//修炼魔神之手
					if (modPlayer.practiceHand)
					{
						int handMaxBloodGas = modPlayer.handMax;
						if (modPlayer.PlayerClass == 2)
							handMaxBloodGas = modPlayer.handMax * 2;
						if (modPlayer.handBloodGas < modPlayer.handMax)
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
								if (modPlayer.handBloodGas > handMaxBloodGas)
									modPlayer.handBloodGas = handMaxBloodGas;
								if (npc.boss)
                                {
									string text = "";
									if (powerLevel == 0)
									{
										text = $"{player.name}修炼魔神之手消耗{addExp}灵魂之力吞噬了{npc.FullName}的气血，魔神之手气血+{addBloodGas}";
									}
									if (powerLevel > 0)
									{
										text = $"{player.name}修炼魔神之手消耗{addExp}灵魂之力越级吞噬了{npc.getPowerLevelText()}强者{npc.FullName}的气血，额外吸收{powerLevel}倍气血，魔神之手气血+{addBloodGas}";
									}
									Main.NewText(text, Color.Green);
									if (Main.netMode == NetmodeID.Server)
									{
										NetMessage.BroadcastChatMessage(NetworkText.FromLiteral(text), Color.Green);
									}
								}
							}
						}
					}
					//修炼魔神之躯
					if (modPlayer.practiceBody)
					{
						int bodyMaxBloodGas = modPlayer.bodyMax;
						if (modPlayer.PlayerClass == 1 || modPlayer.PlayerClass == 2)
							bodyMaxBloodGas = modPlayer.bodyMax * 2;
						if (modPlayer.bodyBloodGas < bodyMaxBloodGas)
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
								if (modPlayer.bodyBloodGas > bodyMaxBloodGas)
									modPlayer.bodyBloodGas = bodyMaxBloodGas;
								if (npc.boss)
                                {
									string text = "";
									if (powerLevel == 0)
									{
										text = $"{player.name}修炼魔神之躯消耗{addExp}灵魂之力吞噬了{npc.FullName}的气血，魔神之躯气血+{addBloodGas}";
									}
									if (powerLevel > 0)
									{
										text = $"{player.name}修炼魔神之躯消耗{addExp}灵魂之力越级吞噬了{npc.getPowerLevelText()}强者{npc.FullName}的气血，额外吸收{powerLevel}倍气血，魔神之躯气血+{addBloodGas}";
									}
									Main.NewText(text, Color.Green);
									if (Main.netMode == NetmodeID.Server)
									{
										NetMessage.BroadcastChatMessage(NetworkText.FromLiteral(text), Color.Green);
									}
								}
							}
						}
					}
					//修炼魔神之腿
					if (modPlayer.practiceFoot)
					{
						if (modPlayer.footBloodGas < modPlayer.footMax)
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
								if (modPlayer.footBloodGas > modPlayer.footMax)
									modPlayer.footBloodGas = modPlayer.footMax;
								if (npc.boss)
								{
									string text = "";
									if (powerLevel == 0)
									{
										text = $"{player.name}修炼魔神之腿消耗{addExp}灵魂之力吞噬了{npc.FullName}的气血，魔神之腿气血+{addBloodGas}";
									}
									if (powerLevel > 0)
									{
										text = $"{player.name}修炼魔神之腿消耗{addExp}灵魂之力越级吞噬了{npc.getPowerLevelText()}强者{npc.FullName}的气血，额外吸收{powerLevel}倍气血，魔神之腿气血+{addBloodGas}";
									}
									Main.NewText(text, Color.Green);
									if (Main.netMode == NetmodeID.Server)
									{
										NetMessage.BroadcastChatMessage(NetworkText.FromLiteral(text), Color.Green);
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

		public void CauseDirectDamage(NPC npc, int originalDamage, bool crit)
		{
			Player player = Main.player[Main.myPlayer];
			SummonHeartPlayer modPlayer = player.GetModPlayer<SummonHeartPlayer>();

			int num = 0;
			if (crit)
				originalDamage *= 2;

			if (modPlayer.SummonHeart)
				num = originalDamage * modPlayer.SummonCrit / 5000 + modPlayer.SummonCrit / 5 + SummonHeartWorld.WorldLevel * 5;

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

		public override void ModifyHitByItem(NPC npc, Player player, Item item, ref int damage, ref float knockback, ref bool crit)
        {
			SummonHeartPlayer modPlayer = player.GetModPlayer<SummonHeartPlayer>();
			
			if (modPlayer.soulSplit)
			{
				if (!npc.HasBuff(mod.BuffType("SoulSplit")))
				{
					soulSplitCount = 1;
				}
				npc.AddBuff(mod.BuffType("SoulSplit"), 200);
			}
			if (item.modItem != null && item.modItem.Name == "Raiden")
			{
				if (modPlayer.showRadius && modPlayer.PlayerClass == 2)
				{
					damage += modPlayer.killResourceCostCount * modPlayer.killResourceMulti;
				}
			}
			this.CauseDirectDamage(npc, damage, crit);
		}

        public override void ModifyHitByProjectile(NPC npc, Projectile projectile, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
			Mod Calamity = ModLoader.GetMod("CalamityMod");
			Player player = Main.player[projectile.owner];
            SummonHeartPlayer modPlayer = player.GetModPlayer<SummonHeartPlayer>();
			
			this.CauseDirectDamage(npc, damage, crit);
			if (modPlayer.soulSplit)
			{
				if (!npc.HasBuff(mod.BuffType("SoulSplit")))
				{
					soulSplitCount = 1;
				}
				npc.AddBuff(mod.BuffType("SoulSplit"), 200);
			}
            /*if (modPlayer.boughtbuffList[0])
            {
				if (projectile.minion && Main.rand.Next(101) <= (modPlayer.eyeBloodGas + 30000) / 1500)
				{
					crit = true;
				}
            }*/
		}
    }
}
