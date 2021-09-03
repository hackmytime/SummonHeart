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

            int num = tooltips.FindIndex((TooltipLine t) => t.Name.Equals("ItemName"));

            if (mp.PlayerClass == 7 && skillType > 0 && skillLevel > 0)
            {
                if (num != -1)
                {
                    string str = "";
                    string desp = "";
                    if (skillType == SkillType.MultiBow)
                    {
                        str = "组合弓弩Lv" + skillLevel;
                        desp = "组合弓弩数 " + (skillLevel + 4) + "把";
                    }
                    tooltips[num+1].text = str;
                    tooltips[num+1].overrideColor = Color.LightSkyBlue;
                    tooltips[num + 2].text = desp;
                    tooltips[num + 2].overrideColor = Color.LightSkyBlue;
                }
            }
        }

        public override bool Shoot(Item item, Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            SummonHeartPlayer mp = player.GetModPlayer<SummonHeartPlayer>();
            if (item.ranged && mp.PlayerClass == 7 && skillType == SkillType.MultiBow && item.useAmmo == AmmoID.Arrow)
            {
                {
                    /*int maxPro = skillLevel + 5;
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
                        Projectile.NewProjectile(position.X + velocity.X, position.Y + velocity.Y, speedX * 1.5f, speedY * 1.5f, type, damage, knockBack, player.whoAmI);
                    }*/
                    int maxPro = skillLevel + 4;
                    player.statMana -= mp.costMana;
                    for (int i = 1; i <= maxPro; i++)
                    {
                        int param = i / 2;
                        if (i % 2 == 0)
                        {
                            param *= -1;
                        }
                        Vector2 velocity = new Vector2(speedX, speedY).RotatedBy(MathHelper.Pi / 180 * 2 * param);
                        Projectile.NewProjectile(position.X, position.Y, velocity.X * 1.5f, velocity.Y * 1.5f, type, damage, knockBack, player.whoAmI);
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
            return tagCompound;
        }

        public override void Load(Item item, TagCompound data)
        {
            SkillType skillType = (SkillType)data.GetInt("skillType");
            int skillLevel = data.GetInt("skillLevel");
            if (skillType > SkillType.None && skillType < SkillType.Count)
            {
                this.skillType = skillType;
                this.skillLevel = skillLevel;

            }
            else
            {
                this.skillType = SkillType.None;
                this.skillLevel = 0;
            }
        }

        public override void NetSend(Item item, BinaryWriter writer)
        {
            writer.Write((byte)skillType);
            writer.Write(skillLevel);
        }

        public override void NetReceive(Item item, BinaryReader reader)
        {
            SkillType skillType = (SkillType)reader.ReadByte();
            int skillLevel = reader.ReadByte();
            if (skillType > SkillType.None && skillType < SkillType.Count)
            {
                this.skillType = skillType;
                this.skillLevel = skillLevel;
            }
            else
            {
                this.skillType = SkillType.None;
                this.skillLevel = 0;
            }
        }
    }
}
