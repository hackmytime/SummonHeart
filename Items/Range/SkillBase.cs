using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace SummonHeart.Items.Range
{
    class SkillBase : GlobalItem
    {
        public int skillUseCount;
        public int levelUpCount;

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

            if (mp.PlayerClass == 7 && levelUpCount > 0)
            {
                item.autoReuse = true;
                if (num >= 0)
                {
                    string str = "";
                    string desp = "";
                    string upDesp = "";
                    {
                        str = "科技使用次数 " + skillUseCount;
                        desp = "科技升级条件 " + levelUpCount + "次科技使用";
                        upDesp = "已满足升级条件，右键使用可以升级核心科技等级";
                    }
                    tooltips.Insert(num + 1, new TooltipLine(mod, "SkillStr", str));
                    tooltips[num + 1].overrideColor = Color.LightSkyBlue;
                    tooltips.Insert(num + 2, new TooltipLine(mod, "SkillDesp", desp));
                    tooltips[num + 2].overrideColor = Color.LightGreen;
                    if(skillUseCount >= levelUpCount)
                    {
                        tooltips.Insert(num + 3, new TooltipLine(mod, "SkillDesp", upDesp));
                        tooltips[num + 3].overrideColor = Color.LightGreen;
                    }
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
            tagCompound.Add("skillUseCount", skillUseCount);
            tagCompound.Add("levelUpCount", levelUpCount);
            return tagCompound;
        }

        public override void Load(Item item, TagCompound data)
        {
            int skillUseCount = data.GetInt("skillUseCount");
            int levelUpCount = data.GetInt("powerMax");
            this.skillUseCount = skillUseCount;
            this.levelUpCount = levelUpCount;
        }

        public override void NetSend(Item item, BinaryWriter writer)
        {
            writer.Write(skillUseCount);
            writer.Write(levelUpCount);
        }

        public override void NetReceive(Item item, BinaryReader reader)
        {
            int skillUseCount = reader.ReadByte();
            int levelUpCount = reader.ReadByte();
            this.skillUseCount = skillUseCount;
            this.levelUpCount = levelUpCount;
        }
    }
}
