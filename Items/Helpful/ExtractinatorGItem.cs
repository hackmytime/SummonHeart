using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SummonHeart.Items.Helpful
{
    public class ExtractinatorGItem : GlobalItem
    {
        public override void SetDefaults(Item item)
        {
			if (item.type == 2)
			{
				ItemID.Sets.ExtractinatorMode[item.type] = item.type;
			}
		}

		public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
		{
			if (item.type == 997)
			{
				tooltips.Add(new TooltipLine(base.mod, "Extractinator", "可以从土块中提取很多有用的东西")
				{
					overrideColor = new Color?(new Color(200, 100, 100))
				});
				return;
			}
			if (item.type == 2)
			{
				tooltips.Add(new TooltipLine(base.mod, "DirtBlock", "可以用提炼机提炼")
				{
					overrideColor = new Color?(new Color(200, 100, 100))
				});
				return;
			}
		}

		public override void ExtractinatorUse(int extractType, ref int resultType, ref int resultStack)
        {
			if (extractType == 2)
			{
				resultType = 0;
				if (Main.rand.Next(0, 5) == 0)
				{
					int num13 = Item.NewItem(Player.tileTargetX * 16, Player.tileTargetY * 16, 0, 0, 169, Main.rand.Next(0, 9), false, 0, false, false);
					NetMessage.SendData(21, -1, -1, null, num13, 1f, 0f, 0f, 0, 0, 0);
				}
				if (Main.rand.Next(0, 3) == 0)
				{
					if (Main.rand.Next(0, 7) == 0)
					{
						resultType = 309;
						resultStack = 1;
					}
					if (Main.rand.Next(0, 7) == 0)
					{
						resultType = 307;
						resultStack = 1;
					}
					if (Main.rand.Next(0, 7) == 0)
					{
						resultType = 310;
						resultStack = 1;
					}
					if (Main.rand.Next(0, 7) == 0)
					{
						resultType = 312;
						resultStack = 1;
					}
					if (Main.rand.Next(0, 7) == 0)
					{
						resultType = 308;
						resultStack = 1;
					}
					if (Main.rand.Next(0, 7) == 0)
					{
						resultType = 311;
						resultStack = 1;
					}
					if (Main.rand.Next(0, 7) == 0)
					{
						resultType = 2357;
						resultStack = 1;
					}
				}
				if (Main.rand.Next(0, 7) == 0)
				{
					if (Main.rand.Next(0, 5) == 0)
					{
						resultType = 59;
						resultStack = 1;
					}
					if (Main.rand.Next(0, 7) == 0)
					{
						resultType = 369;
						resultStack = 1;
					}
					if (Main.rand.Next(0, 7) == 0)
					{
						resultType = 62;
						resultStack = 1;
					}
					if (Main.rand.Next(0, 5) == 0)
					{
						resultType = 195;
						resultStack = 1;
					}
					if (Main.rand.Next(0, 5) == 0)
					{
						resultType = 2171;
						resultStack = 1;
					}
					if (Main.rand.Next(0, 7) == 0)
					{
						resultType = 194;
						resultStack = 1;
					}
					if (Main.rand.Next(0, 10) == 0)
					{
						resultType = 1828;
						resultStack = Main.rand.Next(1, 4);
					}
				}
				if (Main.rand.Next(0, 2) == 0)
				{
					int num14 = Item.NewItem(Player.tileTargetX * 16, Player.tileTargetY * 16, 0, 0, 71, Main.rand.Next(0, 75), false, 0, false, false);
					NetMessage.SendData(21, -1, -1, null, num14, 1f, 0f, 0f, 0, 0, 0);
				}
			}
			else
            {
                base.ExtractinatorUse(extractType, ref resultType, ref resultStack);
            }
        }
    }
}
