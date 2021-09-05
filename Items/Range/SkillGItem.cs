using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace SummonHeart.Items.Range
{
    class SkillGItem : GlobalItem
    {
        public SkillType skillType;
        public int skillLevel;
        public int curPower;
        public int powerMax;

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

            if (skillType > 0 && skillLevel > 0)
            {
                item.autoReuse = true;
                if (num != -1)
                {
                    string str = "";
                    string desp = "";
                    string power = "";
                    if (skillType == SkillType.MultiBow)
                    {
                        str = "组合弓弩Lv" + skillLevel;
                        desp = "组合弓弩数 " + (skillLevel + 1) + "把";
                        power = "能量 " + curPower+ "/" + powerMax;
                    }
                    tooltips.Insert(num + 1, new TooltipLine(mod, "SkillStr", str));
                    tooltips[num + 1].overrideColor = Color.LightSkyBlue;
                    tooltips.Insert(num + 2, new TooltipLine(mod, "SkillDesp", desp));
                    tooltips[num + 2].overrideColor = Color.LightSkyBlue;
                    tooltips.Insert(num + 3, new TooltipLine(mod, "SkillPower", power));
                    tooltips[num + 3].overrideColor = Color.LightGreen;
                }
            }
        }

        public override bool Shoot(Item item, Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            SummonHeartPlayer mp = player.GetModPlayer<SummonHeartPlayer>();
            if (item.ranged && skillType == SkillType.MultiBow && item.useAmmo == AmmoID.Arrow)
            {
                {
                    int maxPro = skillLevel + 1;
                    for (int i = 1; i <= maxPro; i++)
                    {
                        int param = i / 2;
                        if (i % 2 == 0)
                        {
                            param *= -1;
                        }
                        Vector2 velocity = new Vector2(speedX, speedY).RotatedBy(MathHelper.Pi / 2);
                        velocity.Normalize();
                        velocity *= 10 * param;
                        Projectile.NewProjectile(position.X + velocity.X, position.Y + velocity.Y, speedX * 1.25f, speedY * 1.25f, type, damage, knockBack, player.whoAmI);
                    }
                    curPower -= 5;
                    if(curPower <= 0)
                    {
                        CombatText.NewText(player.getRect(), Color.Red, "能量耗尽，武器已损坏");
                        item.TurnToAir();
                    }
                    return false;
                }
            }
            return base.Shoot(item, player, ref position, ref speedX, ref speedY, ref type, ref damage, ref knockBack);
        }

        public override bool NeedsSaving(Item item)
        {
            return skillType > SkillType.None;
        }

        public override TagCompound Save(Item item)
        {
            TagCompound tagCompound = new TagCompound();
            tagCompound.Add("skillType", (int)skillType);
            tagCompound.Add("skillLevel", skillLevel);
            tagCompound.Add("curPower", curPower);
            tagCompound.Add("powerMax", powerMax);
            return tagCompound;
        }

        public override void Load(Item item, TagCompound data)
        {
            SkillType skillType = (SkillType)data.GetInt("skillType");
            int skillLevel = data.GetInt("skillLevel");
            int curPower = data.GetInt("curPower");
            int powerMax = data.GetInt("powerMax");
            if (skillType > SkillType.None && skillType < SkillType.Count)
            {
                this.skillType = skillType;
                this.skillLevel = skillLevel;
                this.curPower = curPower;
                this.powerMax = powerMax;

            }
            else
            {
                this.skillType = SkillType.None;
                this.skillLevel = 0;
                this.curPower = 0;
                this.powerMax = 0;
            }
        }

        public override void NetSend(Item item, BinaryWriter writer)
        {
            writer.Write((byte)skillType);
            writer.Write(skillLevel);
            writer.Write(curPower);
            writer.Write(powerMax);
        }

        public override void NetReceive(Item item, BinaryReader reader)
        {
            SkillType skillType = (SkillType)reader.ReadByte();
            int skillLevel = reader.ReadByte();
            int curPower = reader.ReadByte();
            int powerMax = reader.ReadByte();
            if (skillType > SkillType.None && skillType < SkillType.Count)
            {
                this.skillType = skillType;
                this.skillLevel = skillLevel;
                this.curPower = curPower;
                this.powerMax = powerMax;
            }
            else
            {
                this.skillType = SkillType.None;
                this.skillLevel = 0;
                this.curPower = 0;
                this.powerMax = 0;
            }
        }
    }
}
