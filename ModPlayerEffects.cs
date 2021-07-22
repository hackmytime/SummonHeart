using Microsoft.Xna.Framework;
using SummonHeart.Extensions;
using Terraria;

namespace SummonHeart
{
    public class ModPlayerEffects
	{
        public static void PostUpdateRunSpeeds(Player player)
        {
            SummonHeartPlayer mplayer = player.SummonHeart();
            if (mplayer.MyAccelerationMult > 2f) mplayer.MyAccelerationMult = 2f;
            if (mplayer.MyMoveSpeedMult > 1.5f) mplayer.MyMoveSpeedMult = 1.5f;
            player.runAcceleration *= mplayer.MyAccelerationMult;
            player.accRunSpeed *= mplayer.MyMoveSpeedMult;
            player.maxRunSpeed *= mplayer.MyMoveSpeedMult;
        }

        public static void UpdateColors(Player player)
        {
            SummonHeartPlayer mplayer = player.SummonHeart();
            if (!mplayer.colorsInitialized)
            {
                InitializeColors(null, player, mplayer);
            }
            if (mplayer.useOscGradient)
            {
                mplayer.oscColor = mplayer.oscGradient.Sample(Helper.OscSaw(0f, 1f, 2f * mplayer.oscGradient.length, (float)player.whoAmI));
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