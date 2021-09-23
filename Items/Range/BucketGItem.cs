using Microsoft.Xna.Framework;
using SummonHeart.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace SummonHeart.Items.Range
{
    class BucketGItem : GlobalItem
    {
        public int liquidType;
        public int liquidCount;

        public override bool InstancePerEntity
        {
            get
            {
                return true;
            }
        }

        public override bool CloneNewInstances
        {
            get
            {
                return true;
            }
        }

        public override bool UseItem(Item item, Player player)
        {
            if(liquidType > 0)
            {
                if (player.altFunctionUse == 2)
                {
                    if (liquidCount <= 0)
                    {
                        liquidCount = 0;
                        CombatText.NewText(player.getRect(), Color.Red, "储存量不足");
                        return true;
                    }
                    if (SHUtils.PlaceLiquid(player, liquidType - 1))
                    {
                        liquidCount--;
                    }
                }
                else
                {
                    if (liquidCount >= 9999)
                    {
                        liquidCount = 9999;
                        CombatText.NewText(player.getRect(), Color.Red, "次元空间已满");
                        return true;
                    }
                    if (Math.Abs(player.position.X / 16f - (float)Player.tileTargetX) > (float)(Player.tileRangeX + 20 * 16) || Math.Abs(player.position.Y / 16f - (float)Player.tileTargetY) > (float)(Player.tileRangeY + 20 * 16))
                    {
                        return false;
                    }
                    //1次吸9格
                    int tileTargetX = Player.tileTargetX;
                    int tileTargetY = Player.tileTargetY;
                    for (int i = -2; i <= 2; i++)
                    {
                        for (int j = -2; j <= 2; j++)
                        {
                            if (SHUtils.Sponge(player, tileTargetX + i, tileTargetY + j, liquidType - 1))
                            {
                                liquidCount++;
                                CombatText.NewText(player.getRect(), Color.LightGreen, $"+1储存量");
                            }
                        }
                    }
                }
                return true;
            }
            return base.UseItem(item, player);
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (liquidType > 0)
            {
                Player player = Main.LocalPlayer;
                SummonHeartPlayer mp = player.GetModPlayer<SummonHeartPlayer>();
                int num = tooltips.Count - 1;
                item.autoReuse = true;
                if (num >= 0)
                {
                    string str = "";
                    {
                        str = "当前储存量 " + liquidCount + "格";
                    }
                    tooltips.Insert(num + 1, new TooltipLine(mod, "LiquidCount", str));
                    tooltips[num + 1].overrideColor = Color.LightSkyBlue;
                }
            }
        }

        public override bool NeedsSaving(Item item)
        {
            return true;
        }

        public override TagCompound Save(Item item)
        {
            TagCompound tagCompound = new TagCompound();
            tagCompound.Add("liquidType", liquidType);
            tagCompound.Add("liquidCount", liquidCount);
            return tagCompound;
        }

        public override void Load(Item item, TagCompound data)
        {
            int liquidType = data.GetInt("liquidType");
            int liquidCount = data.GetInt("liquidCount");
            this.liquidType = liquidType;
            this.liquidCount = liquidCount;
        }

        public override void NetSend(Item item, BinaryWriter writer)
        {
            writer.Write(liquidType);
            writer.Write(liquidCount);
        }

        public override void NetReceive(Item item, BinaryReader reader)
        {
            int liquidType = reader.ReadByte();
            int liquidCount = reader.ReadByte();
            this.liquidType = liquidType;
            this.liquidCount = liquidCount;
        }
    }
}
