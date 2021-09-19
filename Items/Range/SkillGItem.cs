using Microsoft.Xna.Framework;
using SummonHeart.Extensions;
using SummonHeart.Projectiles.Range.Arrows;
using System;
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
        private bool ammoConsumed;

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

        public override float UseTimeMultiplier(Item item, Player player)
        {
            if (item.ranged && skillType == SkillType.MultiGun && item.useAmmo == AmmoID.Bullet)
            {
                return 0.5f;
            }
            else if(item.ranged && skillType == SkillType.PowerGun && item.useAmmo == AmmoID.Bullet)
            {
                return 0.33f;
            }
            return base.UseTimeMultiplier(item, player);
        }

        public override void ModifyWeaponDamage(Item item, Player player, ref float add, ref float mult, ref float flat)
        {
            float baseAdd = add;
            float heartAdd = 1f;
            if (skillType == SkillType.PowerGun && skillLevel > 0)
            {
                heartAdd += (skillLevel + 3) * 5f;
            }else if (skillType == SkillType.PowerBow && skillLevel > 0)
            {
                SummonHeartPlayer mp = Main.LocalPlayer.GetModPlayer<SummonHeartPlayer>();
                heartAdd += mp.bowCharge * 0.2f;
            }
            add = heartAdd * baseAdd;
            base.ModifyWeaponDamage(item, player, ref add, ref mult, ref flat);
        }

        public override void GetWeaponCrit(Item item, Player player, ref int crit)
        {
            if (skillType == SkillType.PowerGun && skillLevel > 0)
            {
                crit += 100;
            }
            else if (skillType == SkillType.PowerBow && skillLevel > 0)
            {
                SummonHeartPlayer mp = Main.LocalPlayer.GetModPlayer<SummonHeartPlayer>();
                crit += (int)mp.bowCharge;
            }
            base.GetWeaponCrit(item, player, ref crit);
        }

       /* public override void SetDefaults(Item item)
        {
            if (skillType == SkillType.PowerBow && skillLevel > 0)
            {
                item.autoReuse = true;
                item.channel = true;
            }
        }*/

        public override void HoldItem(Item item, Player player)
        {
            SummonHeartPlayer mp = player.GetModPlayer<SummonHeartPlayer>();

            if (skillType == SkillType.PowerBow && skillLevel > 0)
            {
                mp.bowChargeActive = true;
                mp.bowChargeMax = (skillLevel + 4) * 100;
                item.autoReuse = true;
                item.channel = true;
                item.UseSound = null;
            }

            if (mp.bowChargeActive)
            {
                float bowChargeMax = mp.bowChargeMax;
                int weaponDamage = player.GetWeaponDamage(item);
                mp.bowChargeAttack = weaponDamage;
                if (player.channel)
                {
                    item.useTime = 0;
                    item.useAnimation = 0;
                    mp.bowChargeActive = true;
                    if(mp.bowCharge < mp.bowChargeMax)
                    {
                        mp.bowCharge += skillLevel;
                        curPower -= skillLevel;
                        if (curPower <= 0 && skillLevel < 6)
                        {
                            CombatText.NewText(player.getRect(), Color.Red, "能量耗尽，武器已损坏");
                            item.TurnToAir();
                        }
                        if (curPower <= 0)
                        {
                            curPower = 0;
                        }
                        if (mp.bowCharge >= mp.bowChargeMax)
                            mp.bowCharge = mp.bowChargeMax;
                    }
                    if (mp.bowCharge % (skillLevel * 60) == 0)
                    {
                        Main.PlaySound(50, (int)player.Center.X, (int)player.Center.Y, base.mod.GetSoundSlot(SoundType.Custom, "Sounds/Custom/bowstring"), 0.5f, 0f);
                    }
                    if (mp.bowCharge == bowChargeMax-1)
                    {
                        for (int d = 0; d < 88; d++)
                        {
                            Dust.NewDust(player.Center, 0, 0, 43, 0f + (float)Main.rand.Next(-12, 12), 0f + (float)Main.rand.Next(-12, 12), 150, default(Color), 0.8f);
                        }
                        Main.PlaySound(SoundID.Item52, player.position);
                    }
                    if (mp.bowCharge < bowChargeMax)
                    {
                        for (int i = 0; i < 30; i++)
                        {
                            Vector2 offset = default(Vector2);
                            double angle = Main.rand.NextDouble() * 2.0 * 3.141592653589793;
                            offset.X += (float)(Math.Sin(angle) * (double)(bowChargeMax - mp.bowCharge));
                            offset.Y += (float)(Math.Cos(angle) * (double)(bowChargeMax - mp.bowCharge));
                            Dust dust = Dust.NewDustPerfect(player.MountedCenter + offset, 43, new Vector2?(player.velocity), 200, default(Color), 0.5f);
                            dust.fadeIn = 0.1f;
                            dust.noGravity = true;
                        }
                        Vector2 vector = new Vector2((float)Main.rand.Next(-28, 28) * -9.88f, (float)Main.rand.Next(-28, 28) * -9.88f);
                        Dust dust2 = Main.dust[Dust.NewDust(player.MountedCenter + vector, 1, 1, 43, 0f, 0f, 255, new Color(0.8f, 0.4f, 1f), 0.8f)];
                        dust2.velocity = -vector / 12f;
                        dust2.velocity -= player.velocity / 8f;
                        dust2.noLight = true;
                        dust2.noGravity = true;
                        return;
                    }
                    Dust.NewDust(player.Center, 0, 0, 20, 0f + (float)Main.rand.Next(-5, 5), 0f + (float)Main.rand.Next(-5, 5), 150, default(Color), 0.8f);
                    return;
                }
                else
                {
                    item.useTime = 10;
                    item.useAnimation = 10;
                    if (mp.bowCharge > 0f)
                    {
                        int num75 = item.shoot;
                        float num76 = item.shootSpeed;
                        int num77 = weaponDamage;
                        float num78 = item.knockBack;
                        if (item.useAmmo > 0)
                        {
                            bool flag10 = false;
                            this.ammoConsumed = false;
                            player.PickAmmo(item, ref num75, ref num76, ref flag10, ref num77, ref num78, false);
                        }
                        Vector2 vector2 = player.RotatedRelativePoint(player.MountedCenter, true);
                        if (item.type == 3094 || item.type == 3378 || item.type == 3543)
                        {
                            vector2.Y = player.position.Y + (float)(player.height / 3);
                        }
                        if (item.type == 3007)
                        {
                            vector2.X -= (float)(4 * player.direction);
                            vector2.Y -= 1f * player.gravDir;
                        }
                        float num82 = (float)Main.mouseX + Main.screenPosition.X - vector2.X;
                        float num83 = (float)Main.mouseY + Main.screenPosition.Y - vector2.Y;
                        if (player.gravDir == -1f)
                        {
                            num83 = Main.screenPosition.Y + (float)Main.screenHeight - (float)Main.mouseY - vector2.Y;
                        }
                        float num84 = (float)Math.Sqrt((double)(num82 * num82 + num83 * num83));
                        float num85 = num84;
                        if ((float.IsNaN(num82) && float.IsNaN(num83)) || (num82 == 0f && num83 == 0f))
                        {
                            num82 = (float)player.direction;
                            num83 = 0f;
                            num84 = num76;
                        }
                        else
                        {
                            num84 = num76 / num84;
                        }
                        if (item.type == 1929 || item.type == 2270)
                        {
                            num82 += (float)Main.rand.Next(-50, 51) * 0.03f / num84;
                            num83 += (float)Main.rand.Next(-50, 51) * 0.03f / num84;
                        }
                        num82 *= num84;
                        num83 *= num84;
                        float addSpeed = mp.bowCharge / 500f + 1;
                        num82 *= addSpeed;
                        num83 *= addSpeed;
                        Main.PlaySound(SoundID.Item5, player.position);
                        mp.bowChargeActive = false;
                        mp.bowCharge = 0f;
                        //this.Shoot(item, player, ref vector2, ref num82, ref num83, ref num75, ref num77, ref num78);
                        Projectile.NewProjectile(player.MountedCenter.X, player.MountedCenter.Y, num82, num83, num75, num77, num78, player.whoAmI, 0f, 0f);
                    }
                    return;
                }
            }
        }

        public override bool CanUseItem(Item item, Player player)
        {
            if (skillLevel > 0)
            {
                int num75 = item.shoot;
                float num76 = item.shootSpeed;
                int weaponDamage = player.GetWeaponDamage(item);
                int num77 = weaponDamage;
                float num78 = item.knockBack;
                if (item.useAmmo > 0)
                {
                    bool flag10 = false;
                    player.PickAmmo(item, ref num75, ref num76, ref flag10, ref num77, ref num78, false);
                }
                Vector2 vector2 = player.RotatedRelativePoint(player.MountedCenter, true);
                if (item.type == 3094 || item.type == 3378 || item.type == 3543)
                {
                    vector2.Y = player.position.Y + (float)(player.height / 3);
                }
                if (item.type == 3007)
                {
                    vector2.X -= (float)(4 * player.direction);
                    vector2.Y -= 1f * player.gravDir;
                }
                float num82 = (float)Main.mouseX + Main.screenPosition.X - vector2.X;
                float num83 = (float)Main.mouseY + Main.screenPosition.Y - vector2.Y;
                if (player.gravDir == -1f)
                {
                    num83 = Main.screenPosition.Y + (float)Main.screenHeight - (float)Main.mouseY - vector2.Y;
                }
                float num84 = (float)Math.Sqrt((double)(num82 * num82 + num83 * num83));
                float num85 = num84;
                if ((float.IsNaN(num82) && float.IsNaN(num83)) || (num82 == 0f && num83 == 0f))
                {
                    num82 = (float)player.direction;
                    num83 = 0f;
                    num84 = num76;
                }
                else
                {
                    num84 = num76 / num84;
                }
                if (item.type == 1929 || item.type == 2270)
                {
                    num82 += (float)Main.rand.Next(-50, 51) * 0.03f / num84;
                    num83 += (float)Main.rand.Next(-50, 51) * 0.03f / num84;
                }
                num82 *= num84;
                num83 *= num84;
            
                //Projectile.NewProjectile(vector2.X, vector2.Y, speedX, speedY2, num75, num77, num78, player.whoAmI, 0f, 0f);
                this.Shoot2(item, player, ref vector2, ref num82, ref num83, ref num75, ref num77, ref num78);
                //return false;
            }
            return base.CanUseItem(item, player);
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            Player player = Main.LocalPlayer;
            SummonHeartPlayer mp = player.GetModPlayer<SummonHeartPlayer>();

            int num = tooltips.Count - 1;

            if (skillType == SkillType.MultiBow && skillLevel > 0)
            {
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
                        costAmmo = "消耗弓箭数 " + (skillLevel + 1) + "支";
                        power = "能量 " + curPower + "/" + powerMax;
                        costPower = "消耗能量数 " + (5 * (skillLevel + 1));
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
            else if (skillType == SkillType.MultiGun && skillLevel > 0)
            {
                if (num != -1)
                {
                    string str = "";
                    string desp = "";
                    string costAmmo = "";
                    string power = "";
                    string costPower = "";
                    if (skillType == SkillType.MultiGun)
                    {
                        str = "组合散弹枪Lv" + skillLevel;
                        desp = "额外射弹量 " + (skillLevel * 2 + 6) + "发";
                        costAmmo = "耗弹量 " + (skillLevel * 2 + 6) + "发";
                        power = "当前能量 " + curPower + "/" + powerMax;
                        costPower = "消耗能量 " + (5 * (skillLevel + 1));
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
            else if (skillType == SkillType.PowerGun && skillLevel > 0)
            {
                if (num != -1)
                {
                    string str = "";
                    string desp = "";
                    string costAmmo = "";
                    string power = "";
                    string costPower = "";
                    if (skillType == SkillType.PowerGun)
                    {
                        str = "枪械强化Lv" + skillLevel;
                        desp = "伤害增加 " + (skillLevel + 3) * 5 + "倍";
                        costAmmo = "能量消耗 " + (skillLevel + 3) + "倍";
                        power = "当前能量 " + curPower + "/" + powerMax;
                        costPower = "消耗能量 " + (5 * (skillLevel + 1)) * (skillLevel + 3);
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
            else if (skillType == SkillType.PowerBow && skillLevel > 0)
            {
                if (num != -1)
                {
                    string str = "";
                    string desp = "";
                    string costAmmo = "";
                    string power = "";
                    string costPower = "";
                    if (skillType == SkillType.PowerBow)
                    {
                        str = "弓弩强化Lv" + skillLevel;
                        desp = "蓄力上限 " + (skillLevel + 4) * 100;
                        costAmmo = "蓄力速度 1帧"+skillLevel+"层";
                        power = "当前能量 " + curPower + "/" + powerMax;
                        costPower = "每1层蓄力消耗" + skillLevel + "点能量";
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
            if (skillType == SkillType.PowerBow && skillLevel > 0)
            {
                return false;
            }
            return base.Shoot(item, player, ref position, ref speedX, ref speedY, ref type, ref damage, ref knockBack);
        }

        public bool Shoot2(Item item, Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            SummonHeartPlayer mp = player.GetModPlayer<SummonHeartPlayer>();
            if (curPower <= 0 && skillLevel == 6)
            {
                curPower = 0;
                CombatText.NewText(player.getRect(), Color.Red, "能量耗尽，请更换能量核心");
                return false;
            }
            if (item.ranged && skillType == SkillType.MultiBow && item.useAmmo == AmmoID.Arrow)
            {
                speedX *= 1.25f;
                speedY *= 1.25f;
                {
                    int maxPro = skillLevel + 1;
                    for (int i = 2; i <= maxPro; i++)
                    {
                        int param = i / 2;
                        if (i % 2 == 0)
                        {
                            param *= -1;
                        }
                        Vector2 velocity = new Vector2(speedX, speedY).RotatedBy(MathHelper.Pi / 2);
                        velocity.Normalize();
                        velocity *= 10 * param;
                        Projectile.NewProjectile(position.X + velocity.X, position.Y + velocity.Y, speedX, speedY, type, damage, knockBack, player.whoAmI);
                        //Projectile.NewProjectile(player.position, Vector2.Zero, mod.ProjectileType("MultiBowPro"), 0, 0f, player.whoAmI);
                    }
                  
                    //计算能量消耗
                    curPower -= 5 * (skillLevel + 1);
                    player.CostItem(item.useAmmo, skillLevel);
                    if(curPower <= 0 && skillLevel < 6)
                    {
                        CombatText.NewText(player.getRect(), Color.Red, "能量耗尽，武器已损坏");
                        item.TurnToAir();
                    }
                    if (curPower <= 0)
                    {
                        curPower = 0;
                    }
                    //return false;
                }
            }
            else if (item.ranged && skillType == SkillType.MultiGun && item.useAmmo == AmmoID.Bullet)
            {
                /*speedX *= 1.25f;
                speedY *= 1.25f;*/
                {
                    /*int maxPro = skillLevel + 1;
                    for (int i = 2; i <= maxPro; i++)
                    {
                        int param = i / 2;
                        if (i % 2 == 0)
                        {
                            param *= -1;
                        }
                        Vector2 velocity = new Vector2(speedX, speedY).RotatedBy(MathHelper.Pi / 2);
                        velocity.Normalize();
                        velocity *= 10 * param;
                        Projectile.NewProjectile(position.X + velocity.X, position.Y + velocity.Y, speedX, speedY, type, damage, knockBack, player.whoAmI);
                        //Projectile.NewProjectile(player.position, Vector2.Zero, mod.ProjectileType("MultiBowPro"), 0, 0f, player.whoAmI);
                    }
*/
                    int numberProjectiles = skillLevel * 2 + 6;
                    for (int i = 0; i < numberProjectiles; i++)
                    {
                        Vector2 perturbedSpeed = Utils.RotatedByRandom(new Vector2(speedX, speedY), (double)MathHelper.ToRadians(10f));
                        Projectile.NewProjectile(position.X, position.Y, perturbedSpeed.X, perturbedSpeed.Y, type, damage, knockBack, player.whoAmI, 0f, 0f);
                    }

                    //计算能量消耗
                    curPower -= 5 * (skillLevel + 1);
                    player.CostItem(item.useAmmo, skillLevel);
                    if (curPower <= 0 && skillLevel < 6)
                    {
                        CombatText.NewText(player.getRect(), Color.Red, "能量耗尽，武器已损坏");
                        item.TurnToAir();
                    }
                    if(curPower <= 0)
                    {
                        curPower = 0;
                    }
                    //return false;
                }
            }
            else if (item.ranged && skillType == SkillType.PowerGun && item.useAmmo == AmmoID.Bullet)
            {
                /*speedX *= 2f;
                speedY *= 2f;*/
                //计算能量消耗
                curPower -= 5 * (skillLevel + 1) * (skillLevel + 3);
                player.CostItem(item.useAmmo, skillLevel);
                if (curPower <= 0 && skillLevel < 6)
                {
                    CombatText.NewText(player.getRect(), Color.Red, "能量耗尽，武器已损坏");
                    item.TurnToAir();
                }
                if (curPower <= 0)
                {
                    curPower = 0;
                }
            }
            return this.Shoot(item, player, ref position, ref speedX, ref speedY, ref type, ref damage, ref knockBack);
        }

        /*public override bool PreDrawInWorld(Item item, SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
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
        }*/

        public override bool ConsumeAmmo(Item item, Player player)
        {
            if (skillType == SkillType.PowerBow && skillLevel > 0)
            {
                if (this.ammoConsumed)
                {
                    return false;
                }
                this.ammoConsumed = true;
            }
            if (item.ammo > 0)
            {
                int costCount = player.HeldItem.GetGlobalItem<SkillGItem>().skillLevel;
                if (costCount > 0)
                {
                    SkillType type = player.HeldItem.GetGlobalItem<SkillGItem>().skillType;
                    if(type == SkillType.MultiBow)
                    {
                        costCount += 1;
                    }
                    else if(type == SkillType.MultiGun)
                    {
                        costCount = 6 + costCount * 2; 
                    }
                    costCount /= 2;
                    if (type == SkillType.PowerGun || type == SkillType.PowerBow)
                    {
                        costCount = 1;
                    }
                    player.CostItem(item.type, costCount);
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
