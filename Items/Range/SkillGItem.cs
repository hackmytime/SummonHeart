﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SummonHeart.Extensions;
using SummonHeart.Utilities;
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
                    string costAmmo = "";
                    string power = "";
                    string costPower = "";
                    if (skillType == SkillType.MultiBow)
                    {
                        str = "组合弓弩Lv" + skillLevel;
                        desp = "组合弓弩数 " + (skillLevel + 1) + "把";
                        costAmmo = "消耗弓箭数 " + (skillLevel + 1) + "枝";
                        power = "能量 " + curPower+ "/" + powerMax;
                        costPower = "消耗能量数 " + (5*(skillLevel+1));
                    }
                    tooltips.Insert(num + 1, new TooltipLine(mod, "SkillStr", str));
                    tooltips[num + 1].overrideColor = Color.LightSkyBlue;
                    tooltips.Insert(num + 2, new TooltipLine(mod, "SkillDesp", desp));
                    tooltips[num + 2].overrideColor = Color.LightSkyBlue;
                    tooltips.Insert(num + 3, new TooltipLine(mod, "CostAmmo", costAmmo));
                    tooltips[num + 3].overrideColor = Color.LightSkyBlue;
                    tooltips.Insert(num + 4, new TooltipLine(mod, "SkillPower", power));
                    tooltips[num + 4].overrideColor = Color.LightGreen;
                    tooltips.Insert(num + 5, new TooltipLine(mod, "CostPower", costPower));
                    tooltips[num + 5].overrideColor = Color.LightGreen;
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
                    /*for (int i = 1; i <= maxPro; i++)
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
                        //Projectile.NewProjectile(player.position, Vector2.Zero, mod.ProjectileType("MultiBowPro"), 0, 0f, player.whoAmI);
                    }*/
                    List<Projectile> lists = player.getOwnedProjectile(mod.ProjectileType("MultiBowPro"));
                    foreach(var p in lists)
                    {
                        Texture2D Weapon = Main.itemTexture[item.type];
                        Vector2 basePos = new Vector2(p.position.X, p.position.Y + Weapon.Height);
                        Vector2 baseV = Vector2.Normalize(Main.MouseWorld - basePos);
                        Vector2 speed = new Vector2(speedX, speedY);
                        float basespeed = speed.Length();
                        baseV *= basespeed;
                        baseV *= 1.25f;
                        Projectile.NewProjectile(basePos, baseV, type, damage, knockBack, player.whoAmI);
                    }
                    //计算能量消耗
                    curPower -= 5 * maxPro;
                    player.CostItem(item.useAmmo, skillLevel);
                    if(curPower <= 0)
                    {
                        CombatText.NewText(player.getRect(), Color.Red, "能量耗尽，武器已损坏");
                        item.TurnToAir();
                    }
                    //return false;
                }
            }
            return base.Shoot(item, player, ref position, ref speedX, ref speedY, ref type, ref damage, ref knockBack);
        }

        public override bool PreDrawInWorld(Item item, SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
        {
            if(skillType > 0)
            {
                Texture2D Weapon = Main.itemTexture[item.type];
                Vector2 origin = new Vector2(20, 26);
                for (int i = 0; i < 5; i++)
                {
                    //spriteBatch.Draw(Weapon, projectile.oldPos[i] + weaTruePos, null, color, WeaponRotation, weaorigin, 1f, weffects, 0f);
                }
                Vector2 weaorigin = Weapon.Size() / 2f;
                spriteBatch.Draw(Weapon, Main.LocalPlayer.position + origin, null, lightColor, rotation, weaorigin, 1f, 0, 0f);
            }
            return base.PreDrawInWorld(item, spriteBatch, lightColor, alphaColor, ref rotation, ref scale, whoAmI);
        }

        public override bool ConsumeAmmo(Item item, Player player)
        {
            if (item.ammo > 0)
            {
                int costCount = player.HeldItem.GetGlobalItem<SkillGItem>().skillLevel;
                if (costCount > 0)
                {
                    player.CostItem(item.type, costCount + 1);
                    return false;
                }
            }
            return base.ConsumeAmmo(item, player);
        }

        public override bool ConsumeItem(Item item, Player player)
        {
            return base.ConsumeItem(item, player);
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
