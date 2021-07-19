using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SummonHeart.Models;
using SummonHeart.Utilities;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SummonHeart.Extensions
{
    /// <summary>
    ///     A class housing all the player/ModPlayer/MyPlayer based extensions
    /// </summary>
    public static class PlayerExtensions
    {
        public static void DrawAura(this SummonHeartPlayer modPlayer, AuraAnimationInfo aura)
        {
            Player player = modPlayer.player;
            Texture2D texture = aura.GetTexture();
            Rectangle textureRectangle = new Rectangle(0, aura.GetHeight() * modPlayer.auraCurrentFrame, texture.Width, aura.GetHeight());
            float scale = aura.GetAuraScale(modPlayer);
            Tuple<float, Vector2> rotationAndPosition = aura.GetAuraRotationAndPosition(modPlayer);
            float rotation = rotationAndPosition.Item1;
            Vector2 position = rotationAndPosition.Item2;

            AnimationHelper.SetSpriteBatchForPlayerLayerCustomDraw(aura.blendState, player.GetPlayerSamplerState());

            // custom draw routine
            Main.spriteBatch.Draw(texture, position - Main.screenPosition, textureRectangle, Color.White, rotation, new Vector2(aura.GetWidth(), aura.GetHeight()) * 0.5f, scale, SpriteEffects.None, 0f);

            AnimationHelper.ResetSpriteBatchForPlayerDrawLayers(player.GetPlayerSamplerState());
        }

        public static SamplerState GetPlayerSamplerState(this Player player)
        {
            return player.mount.Active ? Main.MountedSamplerState : Main.DefaultSamplerState;
        }

        public static int getPower(this SummonHeartPlayer modPlayer)
        {
            int power = 0;
            int x = 1;
            if (Main.hardMode)
            {
                x = 2;
            }
            if (NPC.downedMoonlord)
            {
                x = 5;
            }
            power = modPlayer.eyeBloodGas + modPlayer.handBloodGas + modPlayer.bodyBloodGas + modPlayer.footBloodGas;
            power += modPlayer.player.statLifeMax2 * x;
            power += modPlayer.player.statDefense * 30;
            power += modPlayer.SummonCrit * 20 * x;
            power += modPlayer.killResourceMax2 * 10;
            Item item = modPlayer.player.HeldItem;
            if (item.damage > 0)
            {
                power += (int)(item.damage * 5 * (60f / (float)item.useTime));
            }
           
            return power;
        }

        public static int getAllBloodGas(this SummonHeartPlayer modPlayer)
        {
            int all = 0;
            all = modPlayer.eyeBloodGas + modPlayer.handBloodGas + modPlayer.bodyBloodGas + modPlayer.footBloodGas;
            return all;
        }

        public static int HasItemInAcc(this Player player, int type)
        {
            for (int i = 3; i < 8 + player.extraAccessorySlots; i++)
            {
                if (player.armor[i].type == type)
                {
                    return i;
                }
            }
            return -1;
        } 
        
        public static void doKillNpcExp(this Player player, NPC npc)
        {
			SummonHeartPlayer modPlayer = player.GetModPlayer<SummonHeartPlayer>();

			if (player.HeldItem.modItem != null && player.HeldItem.modItem.Name == "DemonSword")
			{
				if (npc.boss)
				{
					int swordMax = npc.getPower() / 200;
					if (modPlayer.swordBloodMax < swordMax)
					{
						modPlayer.swordBloodMax = swordMax;
						string curMax = (modPlayer.swordBloodMax * 1.0f / 100f).ToString("0.00");
						string text = $"{player.name}手持魔剑·弑神吞噬了{npc.FullName}的血肉，突破觉醒上限，当前觉醒上限：{curMax}%";
						Main.NewText(text, Color.Green);
					}
				}
				if (modPlayer.swordBlood < modPlayer.swordBloodMax)
				{
					modPlayer.swordBlood += (modPlayer.swordBloodMax / 10000 + 1);
					if (modPlayer.swordBlood > modPlayer.swordBloodMax)
						modPlayer.swordBlood = modPlayer.swordBloodMax;
				}
			}
			if (player.HeldItem.modItem != null && player.HeldItem.modItem.Name == "Raiden")
			{
				if (npc.boss)
				{
					int swordMax = npc.getPower() / 200;
					if (modPlayer.swordBloodMax < swordMax)
					{
						modPlayer.swordBloodMax = swordMax;
						string curMax = (modPlayer.swordBloodMax * 1.0f / 100f).ToString("0.00");
						string text = $"{player.name}手持魔剑·神陨吞噬了{npc.FullName}的血肉，突破觉醒上限，当前觉醒上限：{curMax}%";
						Main.NewText(text, Color.Green);
					}
				}
				if (modPlayer.shortSwordBlood < modPlayer.swordBloodMax)
				{
					modPlayer.shortSwordBlood += (modPlayer.swordBloodMax / 10000 + 1);
					if (modPlayer.shortSwordBlood > modPlayer.swordBloodMax)
						modPlayer.shortSwordBlood = modPlayer.swordBloodMax;
				}
				if (modPlayer.PlayerClass == 2)
				{
					int heal = 5 * SummonHeartWorld.WorldLevel;
					if (modPlayer.boughtbuffList[1])
					{
						heal += (modPlayer.handBloodGas / 400);
					}
					modPlayer.killResourceCurrent += heal;
					CombatText.NewText(player.getRect(), new Color(0, 255, 0), "+" + heal + "杀意值");
					if (modPlayer.killResourceCurrent > modPlayer.killResourceMax2)
						modPlayer.killResourceCurrent = modPlayer.killResourceMax2;
					if (Main.netMode == NetmodeID.Server)
					{
						modPlayer.KillResourceCountMsg();
					}
				}
			}

			if (modPlayer.SummonHeart)
			{
				int addExp = 0;
				int addBloodGas = 0;
				int powerLevel = npc.getPowerLevel();

				if (npc.boss)
				{
					if (powerLevel == -1)
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
							addExp = npc.getPower() / 500;
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
					if (powerLevel == -1)
					{
						text = $"{player.name}的战力碾压{npc.FullName}，可惜，其血肉灵魂已于{player.name}无用！灵魂之力+{addExp}";
					}
					if (powerLevel == 0)
					{
						text = $"{player.name}吞噬了{npc.FullName}的灵魂，灵魂之力+{addExp}";
					}
					if (powerLevel > 0)
					{
						text = $"{player.name}越级吞噬了{npc.getPowerLevelText()}强者{npc.FullName}的灵魂，获得额外{powerLevel}倍灵魂之力，+{addExp}灵魂之力";
					}

					Main.NewText(text, Color.Green);
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
				}

				//修炼开始
				int bloodGasMax = modPlayer.bloodGasMax;

				if (modPlayer.getAllBloodGas() < bloodGasMax)
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
								}
							}
						}
					}
				}
				modPlayer.dealLevel();
			}
		}
    }
}