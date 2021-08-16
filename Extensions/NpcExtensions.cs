using Microsoft.Xna.Framework;
using Terraria;

namespace SummonHeart.Extensions
{
    /// <summary>
    ///     A class housing all the player/ModPlayer/MyPlayer based extensions
    /// </summary>
    public static class NpcExtensions
    {
		public static void LoseLife(this NPC npc, int num, Color? textColor)
		{
			if (textColor != null)
			{
				CombatText.NewText(new Rectangle((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height), textColor.Value, string.Concat(num), false, true);
			}
			npc.life -= num;
			if (npc.life <= 0)
			{
				npc.life = 1;
				npc.StrikeNPC(9999, 0f, 0, false, false, false);
			}
		}

		public static int getPower(this NPC npc)
        {
			int power = 0;
			int x = 1;
			if (NPC.downedMoonlord)
			{
				x = 2;
			}
			if (npc.boss)
			{
				int lifePower = npc.lifeMax / x;
				int dmagePower = npc.damage * 100 * x;
				if (dmagePower > lifePower * 5)
					dmagePower = lifePower;
				int defPower = npc.defense * 150;
				if (defPower > lifePower * 5)
					defPower = lifePower;
				power = lifePower + dmagePower + defPower;
            }
            else
            {
				power = npc.lifeMax + npc.damage * 30 * x + npc.defense * 30;
            }
			power *= SummonHeartWorld.WorldLevel;
			if (SummonHeartWorld.GoddessMode)
				power *= 2;
			return power;
		}

		public static int getPowerLevel(this NPC npc)
		{
			int result = 0;
			Player player = Main.player[Main.myPlayer];
			SummonHeartPlayer modPlayer = player.GetModPlayer<SummonHeartPlayer>();
			
			float level = 0;
			if (npc.boss)
			{
				level = npc.getPower() * 1.0f / modPlayer.getPower();
			}
			else
			{
				level = npc.getPower() * 1.0f / modPlayer.getPower();
			}

			if(level <= 0.5)
            {
				result = -1;
            }else if(level < 2)
            {
				result = 0;
            }
            else
            {
				result = (int)level / 2;
            }

			return result;
		}

		public static string getPowerLevelText(this NPC npc)
        {
			string powerLevel = "";
			float level = getPowerLevel(npc);
			
			if (level == 0)
				powerLevel = "【同阶】";
			else if(level == -1)
				powerLevel = "【战力碾压】";
			else
				powerLevel = "【斩命"+level+"重】";

			Player player = Main.player[Main.myPlayer];
			SummonHeartPlayer modPlayer = player.GetModPlayer<SummonHeartPlayer>();
			level = npc.getPower() * 1.0f / modPlayer.getPower();
			powerLevel += level.ToString("0.0") + "倍战力";
			return powerLevel;
		}
	}
}