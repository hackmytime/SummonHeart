using Microsoft.Xna.Framework;
using SummonHeart.Utilities;
using SummonHeart.XiuXianModule.Entities;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace SummonHeart.XiuXianModule.Weapon
{
    // This class handles everything for our custom damage class
    // Any class that we wish to be using our custom damage class will derive from this class, instead of ModItem
    public abstract class LinliDamageItem : ModItem
    {
        public override bool CloneNewInstances => true;
        public int linliCost = 0;
        public int level = 0;
        private int useCount;
        public int useMax = 10000;
        public int baseDamage = 1;
        string[] levelTexts = new string[] { "", "黄阶初级", "黄阶中级", "黄阶高级", "玄阶初级", "玄阶中级", "玄阶高级", "地阶初级", "地阶中级", "地阶高级", "天阶初级", "天阶中级", "天阶高级"};


        // Custom items should override this to set their defaults
        public virtual void SafeSetDefaults()
        {
        }

        // By making the override sealed, we prevent derived classes from further overriding the method and enforcing the use of SafeSetDefaults()
        // We do this to ensure that the vanilla damage types are always set to false, which makes the custom damage type work
        public sealed override void SetDefaults()
        {
            SafeSetDefaults();
            // all vanilla damage types must be false for custom damage types to work
            item.melee = false;
            item.ranged = false;
            item.magic = false;
            item.thrown = false;
            item.summon = false;
        }

        // As a modder, you could also opt to make these overrides also sealed. Up to the modder
        public override void ModifyWeaponDamage(Player player, ref float add, ref float mult, ref float flat)
        {
            var rp = player.GetModPlayer<RPGPlayer>();
            flat += rp.lingliDamageAdd;
            float useMult = useCount / (float)2000;
            mult *= useMult;
            if (mult == 0)
                mult = 0.01f;
        }

        public override void GetWeaponKnockback(Player player, ref float knockback)
        {
            var rp = player.GetModPlayer<RPGPlayer>();
            knockback += rp.lingliDamageKnockback;
        }

        public override void GetWeaponCrit(Player player, ref int crit)
        {
            // Adds crit bonuses
            var rp = player.GetModPlayer<RPGPlayer>();
            float useMult = useCount / (float)20;
            crit += (int)useMult;
        }

        // Because we want the damage tooltip to show our custom damage, we need to modify it
        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            // Get the vanilla damage tooltip
            TooltipLine tt = tooltips.FirstOrDefault(x => x.Name == "Damage" && x.mod == "Terraria");
            if (tt != null)
            {
                // We want to grab the last word of the tooltip, which is the translated word for 'damage' (depending on what language the player is using)
                // So we split the string by whitespace, and grab the last word from the returned arrays to get the damage word, and the first to get the damage shown in the tooltip
                string[] splitText = tt.text.Split(' ');
                string damageValue = splitText.First();
                string damageWord = splitText.Last();
                // Change the tooltip text
                tt.text = damageValue + " 灵力" + damageWord;
            }

            if (linliCost > 0)
            {
                tooltips.Add(new TooltipLine(mod, "LinliCost", $"使用{linliCost}灵力"));
            }

            int num = tooltips.Count - 1;
            Player player = Main.LocalPlayer;
            var rp = player.GetModPlayer<RPGPlayer>();
            if (num >= 0)
            {
                string str = "";
                string desp = "";
                string upDesp = "";
                string levelDesp = "";
                {
                    str = "品阶 " + levelTexts[level];
                    desp = "掌控进度 " + useCount + "/" + useMax;
                    upDesp = "灵攻加成 " + useCount / 20 + "%("+ baseDamage + "基础伤害)";
                    levelDesp = "境界加成 +" + rp.lingliDamageAdd + "面板伤害";
                }
                tooltips.Insert(num + 1, new TooltipLine(mod, "SkillStr", str));
                tooltips[num + 1].overrideColor = Color.LightSkyBlue;
                tooltips.Insert(num + 2, new TooltipLine(mod, "SkillDesp", desp));
                tooltips[num + 2].overrideColor = Color.LightGreen;
                tooltips.Insert(num + 3, new TooltipLine(mod, "upDesp", upDesp));
                tooltips[num + 3].overrideColor = Color.LightGreen;
                tooltips.Insert(num + 4, new TooltipLine(mod, "LvelDesp", levelDesp));
                tooltips[num + 4].overrideColor = Color.Gold;
            }
        }

        // Make sure you can't use the item if you don't have enough resource and then use 10 resource otherwise.
        public override bool CanUseItem(Player player)
        {
            var rp = player.GetModPlayer<RPGPlayer>();

            if (rp.lingli >= linliCost)
            {
                rp.lingli -= linliCost;
                if (useCount < useMax)
                    useCount += Mathf.RoundInt(rp.GetLinjiXiuLianAddMult());
                if (useCount > useMax)
                    useCount = useMax;
                return true;
            }
            return false;
        }

       /* public override bool UseItem(Player player)
        {
            useCount++;
            return base.UseItem(player);
        }*/

        public override TagCompound Save()
        {
            TagCompound tagCompound = new TagCompound();
            tagCompound.Add("useCount", useCount);
            return tagCompound;
        }

        public override void Load(TagCompound tag)
        {
            useCount = tag.GetInt("useCount");
        }

        public override void NetSend(BinaryWriter writer)
        {
            writer.Write(useCount);
        }

        public override void NetRecieve(BinaryReader reader)
        {
            useCount = reader.ReadInt32();
        }
    }
}
