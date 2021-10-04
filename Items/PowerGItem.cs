using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace SummonHeart.Items.Range
{
    class PowerGItem : GlobalItem
    {
        public int powerLevel;
        public int itemRare;

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

        public override void ModifyWeaponDamage(Item item, Player player, ref float add, ref float mult, ref float flat)
        {
            float baseAdd = add;
            float heartAdd = 1f;
            if (powerLevel > 0)
            {
                SummonHeartPlayer mp = Main.LocalPlayer.GetModPlayer<SummonHeartPlayer>();
                heartAdd += itemRare * powerLevel * 0.01f;
                flat += itemRare * itemRare * powerLevel;
            }
            add = heartAdd * baseAdd;
            base.ModifyWeaponDamage(item, player, ref add, ref mult, ref flat);
        }

        public override void GetWeaponCrit(Item item, Player player, ref int crit)
        {
            if (powerLevel > 0)
            {
                crit += itemRare * powerLevel;
            }
            base.GetWeaponCrit(item, player, ref crit);
        }

        public override void SetDefaults(Item item)
        {
            if(item.stack == 1 && item.damage > 0)
                setItemRare(item);
            base.SetDefaults(item);
        }

        public void setItemRare(Item item)
        {
            SkillGItem skillGItem = item.GetGlobalItem<SkillGItem>();
            if(skillGItem.skillLevel > 0)
            {
                itemRare = skillGItem.skillLevel;
            }
            else
            {
                int rare = item.rare;
                if (rare >= 0 && rare <= 2)
                    itemRare = 1;
                if (rare >= 3 && rare <= 4)
                    itemRare = 2;
                if (rare >= 5 && rare <= 6)
                    itemRare = 3;
                if (rare >= 7 && rare <= 8)
                    itemRare = 4;
                if (rare >= 9 && rare <= 10)
                    itemRare = 5;
                if (rare >= 11)
                    itemRare = 6;
                if(rare == -12)
                    itemRare = 6;
            }
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            Player player = Main.LocalPlayer;
            SummonHeartPlayer mp = player.GetModPlayer<SummonHeartPlayer>();

            int num = tooltips.Count - 1;

            if (powerLevel > 0)
            {
                int nameNun = tooltips.FindIndex((TooltipLine t) => t.Name.Equals("ItemName"));
                string text;
                text = tooltips[nameNun].text;
                text += "+" + powerLevel;
                tooltips[nameNun].text = text;
                if (num >= 0)
                {
                    string str = "";
                    string desp = "";
                    string upDesp = "";
                    {
                        str = "基础攻击+" + itemRare * powerLevel + "%";
                        desp = "暴击率+" + itemRare * powerLevel + "%";
                        upDesp = "面板攻击+" + itemRare * itemRare * powerLevel + "点";
                    }
                    tooltips.Insert(num + 1, new TooltipLine(mod, "PowerStr", str));
                    tooltips[num + 1].overrideColor = Color.LightSkyBlue;
                    tooltips.Insert(num + 2, new TooltipLine(mod, "PowerDesp1", desp));
                    tooltips[num + 2].overrideColor = Color.LightGreen;
                    tooltips.Insert(num + 3, new TooltipLine(mod, "PowerDesp2", upDesp));
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
            tagCompound.Add("powerLevel", powerLevel);
            tagCompound.Add("itemRare", itemRare);
            return tagCompound;
        }

        public override void Load(Item item, TagCompound data)
        {
            int powerLevel = data.GetInt("powerLevel");
            int itemRare = data.GetInt("itemRare");
            this.powerLevel = powerLevel;
            this.itemRare = itemRare;
        }

        public override void NetSend(Item item, BinaryWriter writer)
        {
            writer.Write(powerLevel);
            writer.Write(itemRare);
        }

        public override void NetReceive(Item item, BinaryReader reader)
        {
            int powerLevel = reader.ReadByte();
            int itemRare = reader.ReadByte();
            this.powerLevel = powerLevel;
            this.itemRare = itemRare;
        }
    }
}
