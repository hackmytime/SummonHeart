﻿using Microsoft.Xna.Framework;
using SummonHeart.Extensions;
using Terraria;
using Terraria.GameInput;

namespace SummonHeart
{
    public class ModPlayerEffects
	{
        public static void PostUpdateRunSpeeds(Player player)
        {
            SummonHeartPlayer mplayer = player.SH();
            if (mplayer.MyAccelerationMult > 2f) mplayer.MyAccelerationMult = 2f;
            if (mplayer.MyMoveSpeedMult > 1.5f) mplayer.MyMoveSpeedMult = 1.5f;
            player.runAcceleration *= mplayer.MyAccelerationMult;
            player.accRunSpeed *= mplayer.MyMoveSpeedMult;
            player.maxRunSpeed *= mplayer.MyMoveSpeedMult;
        }

		public static void UpdateMax(Player player)
		{
			SummonHeartPlayer mp = player.SH();
			int bodyMax = 10000;
			int swordMax = 200;
			//1、2W - king slime5 %
			if (NPC.downedSlimeKing)
			{
				bodyMax = 20000;
				swordMax = 300;
			}
			//2、3W - bigEye10
			if (NPC.downedBoss1)
			{
				bodyMax = 30000;
				swordMax = 400;
			}
			//3、4W - 世吞 / 克脑20
			if (NPC.downedBoss2)
			{
				bodyMax = 40000;
				swordMax = 500;
			}
			//4、6W - 蜂王30
			if (NPC.downedQueenBee)
			{
				bodyMax = 60000;
				swordMax = 600;
			}
			//5、7W - 吴克40
			if (NPC.downedBoss3)
			{
				bodyMax = 70000;
				swordMax = 700;
			}
			//6、8W - 肉山50
			if (Main.hardMode)
			{
				bodyMax = 80000;
				swordMax = 1000;
			}
			//7、10W-新三王80
			if (NPC.downedMechBossAny)
			{
				bodyMax = 100000;
				swordMax = 1500;
			}
			//8、12W - 小花100
			if (NPC.downedPlantBoss)
			{
				bodyMax = 120000;
				swordMax = 2000;
			}
			//9、14W - 小怪120
			if (NPC.downedFishron)
			{
				bodyMax = 140000;
				swordMax = 2500;
			}
			//10、16W - 石头150
			if (NPC.downedGolemBoss)
			{
				bodyMax = 160000;
				swordMax = 3000;
			}
			//11、18W - 教徒200
			if (NPC.downedAncientCultist)
			{
				bodyMax = 180000;
				swordMax = 3500;
			}
			
			//12、20W - 月总无上限*/
			if (NPC.downedMoonlord)
			{
				bodyMax = 200000;
				swordMax = 10000;
			}
			mp.bloodGasMax = bodyMax;
		}

		public static void UpdateColors(Player player)
        {
            SummonHeartPlayer mplayer = player.SH();
            if (!mplayer.colorsInitialized)
            {
                InitializeColors(null, player, mplayer);
            }
            if (mplayer.useOscGradient)
            {
                mplayer.oscColor = mplayer.oscGradient.Sample(Helper.OscSaw(0f, 1f, 2f * mplayer.oscGradient.length, (float)player.whoAmI));
            }
        }
		public static void UpdatePoints(Player player)
        {
            SummonHeartPlayer mp = player.SH();
			if (mp.MysteriousCrystal && Main.mapFullscreen && Main.netMode == 1 && Main.myPlayer == player.whoAmI && player.team > 0 && Main.mouseLeft && Main.mouseLeftRelease)
			{
				for (int k = 0; k < 255; k++)
				{
					if (Main.player[k].active && !Main.player[k].dead && k != Main.myPlayer && player.team == Main.player[k].team)
					{
						float mapFullscreenScale = Main.mapFullscreenScale;
						float num3 = (Main.player[k].position.X + (float)(Main.player[k].width / 2)) / 16f * mapFullscreenScale;
						float num4 = (Main.player[k].position.Y + Main.player[k].gfxOffY + (float)(Main.player[k].height / 2)) / 16f * mapFullscreenScale;
						num3 += -(Main.mapFullscreenPos.X * mapFullscreenScale) + (float)(Main.screenWidth / 2) - 6f;
						float num5 = num4 + (-(Main.mapFullscreenPos.Y * mapFullscreenScale) + (float)(Main.screenHeight / 2) - 4f - mapFullscreenScale / 5f * 2f);
						float num6 = num3 + 4f - 14f * Main.UIScale;
						float num7 = num5 + 2f - 14f * Main.UIScale;
						float num8 = num6 + 28f * Main.UIScale;
						float num9 = num7 + 28f * Main.UIScale;
						int mouseX = PlayerInput.MouseX;
						int mouseY = PlayerInput.MouseY;
						if ((float)mouseX >= num6 && (float)mouseX <= num8 && (float)mouseY >= num7 && (float)mouseY <= num9)
						{
							Main.PlaySound(12, -1, -1, 1, 1f, 0f);
							Main.mouseLeftRelease = false;
							Main.mapFullscreen = false;
							player.UnityTeleport(Main.player[k].position);
							break;
						}
					}
				}
			}
		}

		private static void InitializeColors(string overrideName, Player player, SummonHeartPlayer mplayer)
		{
			Color? oscColor = null;
			Gradient? oscGradient = null;
			string text = overrideName ?? player.name.ToLower();
			uint num = PrivateImplementationDetails.ComputeStringHash(text);
			if (num <= 1921498563U)
			{
				if (num <= 676988930U)
				{
					if (num != 280864780U)
					{
						if (num != 517505375U)
						{
							if (num != 676988930U)
							{
								goto IL_446;
							}
							if (!(text == "dakotaspine"))
							{
								goto IL_446;
							}
						}
						else
						{
							if (!(text == "archie"))
							{
								goto IL_446;
							}
							oscColor = new Color?(new Color(158, 185, 228));
							goto IL_446;
						}
					}
					else
					{
						if (!(text == "instrex"))
						{
							goto IL_446;
						}
						goto IL_392;
					}
				}
				else if (num != 1457548489U)
				{
					if (num != 1481114271U)
					{
						if (num != 1921498563U)
						{
							goto IL_446;
						}
						if (!(text == "hummer"))
						{
							goto IL_446;
						}
						goto IL_3F1;
					}
					else if (!(text == "dakota"))
					{
						goto IL_446;
					}
				}
				else
				{
					if (!(text == "\\\\\\magician///"))
					{
						goto IL_446;
					}
					goto IL_2DB;
				}
				Gradient gradient = Gradient.Linear(true, new Color[]
				{
					new Color(30, 144, 255),
					new Color(140, 198, 131),
					new Color(255, 255, 150),
					new Color(140, 198, 131)
				});
				gradient.length *= 0.5f;
				oscGradient = new Gradient?(gradient);
				goto IL_446;
			}
			if (num <= 2843705437U)
			{
				if (num != 2118795694U)
				{
					if (num != 2570413878U)
					{
						if (num != 2843705437U)
						{
							goto IL_446;
						}
						if (!(text == "vasique"))
						{
							goto IL_446;
						}
						goto IL_392;
					}
					else
					{
						if (!(text == "romass"))
						{
							goto IL_446;
						}
						oscColor = new Color?(new Color(119, 89, 41));
						goto IL_446;
					}
				}
				else if (!(text == "000magician000"))
				{
					goto IL_446;
				}
			}
			else if (num != 2997673620U)
			{
				if (num != 3130953910U)
				{
					if (num != 4280408106U)
					{
						goto IL_446;
					}
					if (!(text == "vastrex"))
					{
						goto IL_446;
					}
					goto IL_392;
				}
				else
				{
					if (!(text == "ass trigger"))
					{
						goto IL_446;
					}
					goto IL_3F1;
				}
			}
			else
			{
				if (!(text == "derpling"))
				{
					goto IL_446;
				}
				oscGradient = new Gradient?(Gradient.Linear(true, new Color[]
				{
					new Color(255, 204, 77),
					Color.Pink
				}));
				goto IL_446;
			}
		IL_2DB:
			oscGradient = new Gradient?(Gradient.Linear(true, new Color[]
			{
				new Color(255, 82, 66),
				new Color(255, 154, 66),
				new Color(255, 236, 66),
				new Color(66, 255, 69),
				new Color(66, 207, 255),
				new Color(66, 66, 255),
				new Color(195, 66, 255)
			}));
			goto IL_446;
		IL_392:
			oscGradient = new Gradient?(Gradient.Linear(true, new Color[]
			{
				new Color(0, 192, 226),
				new Color(176, 98, 224),
				new Color(255, 244, 147)
			}));
			goto IL_446;
		IL_3F1:
			oscGradient = new Gradient?(Gradient.Linear(true, new Color[]
			{
				new Color(105, 39, 143),
				new Color(47, 39, 143),
				new Color(155, 39, 143)
			}));
		IL_446:
			if (oscColor != null)
			{
				mplayer.oscColor = oscColor.Value;
			}
			if (oscGradient != null)
			{
				mplayer.oscGradient = oscGradient.Value;
				mplayer.oscColor = oscGradient.Value.Sample(0f);
			}
			mplayer.useOscColor = (oscColor != null || oscGradient != null);
			mplayer.useOscGradient = (oscGradient != null);
			mplayer.colorsInitialized = true;
		}

	}
}