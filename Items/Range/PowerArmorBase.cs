using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace SummonHeart.Items.Range
{
    class PowerArmorBase : GlobalItem
    {
        public int powerArmorCount = 0;
        public int powerArmorMax = 0;

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

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            Player player = Main.LocalPlayer;
            SummonHeartPlayer mp = player.GetModPlayer<SummonHeartPlayer>();

            int num = tooltips.Count - 1;

            if (powerArmorCount >= 0 && powerArmorMax > 10000)
            {
                item.autoReuse = true;
                if (num >= 0)
                {
                    string str = "";
                    string desp = "";
                    string upDesp = "";
                    {
                        str = "当前能量护盾值 " + powerArmorCount;
                        desp = "能量护盾上限 " + powerArmorMax;
                        upDesp = "已吸收伤害：" + (powerArmorMax - powerArmorCount);
                    }
                    tooltips.Insert(num + 1, new TooltipLine(mod, "SkillStr", str));
                    tooltips[num + 1].overrideColor = Color.LightSkyBlue;
                    tooltips.Insert(num + 2, new TooltipLine(mod, "SkillDesp", desp));
                    tooltips[num + 2].overrideColor = Color.LightGreen;
                    tooltips.Insert(num + 3, new TooltipLine(mod, "SkillDesp", upDesp));
                    tooltips[num + 3].overrideColor = Color.LightGreen;
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
            tagCompound.Add("powerArmorCount", powerArmorCount);
            tagCompound.Add("powerArmorMax", powerArmorMax);
            return tagCompound;
        }

        public override void Load(Item item, TagCompound data)
        {
            int powerArmorCount = data.GetInt("powerArmorCount");
            int powerArmorMax = data.GetInt("powerArmorMax");
            this.powerArmorCount = powerArmorCount;
            this.powerArmorMax = powerArmorMax;
        }

        public override void NetSend(Item item, BinaryWriter writer)
        {
            writer.Write(powerArmorCount);
            writer.Write(powerArmorMax);
        }

        public override void NetReceive(Item item, BinaryReader reader)
        {
            int powerArmorCount = reader.ReadByte();
            int powerArmorMax = reader.ReadByte();
            this.powerArmorCount = powerArmorCount;
            this.powerArmorMax = powerArmorMax;
        }
    }
}
