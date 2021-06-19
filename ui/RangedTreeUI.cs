/*using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using Terraria.UI;

namespace SummonHeart.ui
{
    internal class RangedSkillTreeUI : UIState
    {
        UIPanel panel = new UIPanel();
        UIText SkillPointsLeftText = new UIText("");
        UIText LevelText = new UIText("");
        UIText XPText = new UIText("");
        UIText DescriptionText = new UIText("");
        UIImageButton AchillesHeel = new UIImageButton(ModContent.GetTexture("UntoldLegends/Sprites/Ranged/AchillesHeel0"));
        UIImageButton BetterGunpowder = new UIImageButton(ModContent.GetTexture("UntoldLegends/Sprites/Ranged/BetterGunpowder0"));
        UIImageButton DimensionalMagazines = new UIImageButton(ModContent.GetTexture("UntoldLegends/Sprites/Ranged/DimensionalMagazines0"));
        UIImageButton Speedy = new UIImageButton(ModContent.GetTexture("UntoldLegends/Sprites/Ranged/Speedy0"));
        public override void OnInitialize()
        {
            panel.Height.Set(800, 0);
            panel.Width.Set(600, 0);
            panel.HAlign = 0.5f;
            panel.VAlign = 0.2f;
            panel.Top.Set(0f, 0f);
            panel.BackgroundColor = new Color(30, 30, 30, 150);
            Append(panel);

            LevelText.HAlign = 0.01f;
            LevelText.VAlign = 0.02f;
            LevelText.Height.Set(0, 0);
            LevelText.Width.Set(0, 0);

            SkillPointsLeftText.HAlign = 0.01f;
            SkillPointsLeftText.VAlign = 0.055f;
            SkillPointsLeftText.Height.Set(0, 0);
            SkillPointsLeftText.Width.Set(0, 0);

            XPText.HAlign = 0.01f;
            XPText.VAlign = 0.09f;
            XPText.Height.Set(0, 0);
            XPText.Width.Set(0, 0);

            DescriptionText.HAlign = 0.8f;
            DescriptionText.VAlign = 0.02f;
            DescriptionText.Height.Set(0, 0);
            DescriptionText.Width.Set(0, 0);

            //HAlign +18
            //VAlign -12

            AchillesHeel.Width.Set(64, 0);
            AchillesHeel.Height.Set(64, 0);
            AchillesHeel.HAlign = 0.32f;
            AchillesHeel.VAlign = 0.26f;
            AchillesHeel.OnClick += new MouseEvent(OnAchillesHeel);
            panel.Append(AchillesHeel);

            BetterGunpowder.Width.Set(64, 0);
            BetterGunpowder.Height.Set(64, 0);
            BetterGunpowder.HAlign = 0.32f;
            BetterGunpowder.VAlign = 0.5f;
            BetterGunpowder.OnClick += new MouseEvent(OnBetterGunpowder);
            panel.Append(BetterGunpowder);

            DimensionalMagazines.Width.Set(64, 0);
            DimensionalMagazines.Height.Set(64, 0);
            DimensionalMagazines.HAlign = 0.32f;
            DimensionalMagazines.VAlign = 0.38f;
            DimensionalMagazines.OnClick += new MouseEvent(OnDimensionalMagazines);
            panel.Append(DimensionalMagazines);

            Speedy.Width.Set(64, 0);
            Speedy.Height.Set(64, 0);
            Speedy.HAlign = 0.32f;
            Speedy.VAlign = 0.62f;
            Speedy.OnClick += new MouseEvent(OnSpeedy);
            panel.Append(Speedy);

            panel.Append(LevelText);
            panel.Append(SkillPointsLeftText);
            panel.Append(XPText);
            panel.Append(DescriptionText);
        }
        public override void Update(GameTime gameTime)
        {
            RangersDexterity.SetImage(ModContent.GetTexture("UntoldLegends/Sprites/Ranged/RangersDexterity1"));
            HunterAcrobatics.SetImage(ModContent.GetTexture("UntoldLegends/Sprites/Ranged/HunterAcrobatics0"));
            AerialTakeover.SetImage(ModContent.GetTexture("UntoldLegends/Sprites/Ranged/AerialTakeover0"));
            HunterInstincts.SetImage(ModContent.GetTexture("UntoldLegends/Sprites/Ranged/HunterInstincts0"));
            SharpenedArrows.SetImage(ModContent.GetTexture("UntoldLegends/Sprites/Ranged/SharpenedArrows0"));
            SuperSharpenedArrows.SetImage(ModContent.GetTexture("UntoldLegends/Sprites/Ranged/SuperSharpenedArrows0"));
            AchillesHeel.SetImage(ModContent.GetTexture("UntoldLegends/Sprites/Ranged/AchillesHeel0"));
            PoisonedArrows.SetImage(ModContent.GetTexture("UntoldLegends/Sprites/Ranged/PoisonedArrows0"));
            EaglesEyes.SetImage(ModContent.GetTexture("UntoldLegends/Sprites/Ranged/EaglesEyes0"));
            MarksmansConcentration.SetImage(ModContent.GetTexture("UntoldLegends/Sprites/Ranged/MarksmansConcentration0"));
            HuntersFocus.SetImage(ModContent.GetTexture("UntoldLegends/Sprites/Ranged/HuntersFocus0"));
            Camouflage.SetImage(ModContent.GetTexture("UntoldLegends/Sprites/Ranged/Camouflage0"));
            ShadowForm.SetImage(ModContent.GetTexture("UntoldLegends/Sprites/Ranged/ShadowForm0"));
            ShadowArrows.SetImage(ModContent.GetTexture("UntoldLegends/Sprites/Ranged/ShadowArrows0"));
            ShadowBullets.SetImage(ModContent.GetTexture("UntoldLegends/Sprites/Ranged/ShadowBullets0"));
            BetterGunpowder.SetImage(ModContent.GetTexture("UntoldLegends/Sprites/Ranged/BetterGunpowder0"));
            DimensionalMagazines.SetImage(ModContent.GetTexture("UntoldLegends/Sprites/Ranged/DimensionalMagazines0"));
            DimensionalBullets.SetImage(ModContent.GetTexture("UntoldLegends/Sprites/Ranged/DimensionalBullets0"));
            LuckyShots.SetImage(ModContent.GetTexture("UntoldLegends/Sprites/Ranged/LuckyShots0"));
            GoldenFingers.SetImage(ModContent.GetTexture("UntoldLegends/Sprites/Ranged/GoldenFingers0"));
            Speedy.SetImage(ModContent.GetTexture("UntoldLegends/Sprites/Ranged/Speedy0"));
            FasterGelCombustion.SetImage(ModContent.GetTexture("UntoldLegends/Sprites/Ranged/FasterGelCombustion0"));
            Stress.SetImage(ModContent.GetTexture("UntoldLegends/Sprites/Ranged/Stress0"));
            BulletStorm.SetImage(ModContent.GetTexture("UntoldLegends/Sprites/Ranged/BulletStorm0"));
            BulletHell.SetImage(ModContent.GetTexture("UntoldLegends/Sprites/Ranged/BulletHell0"));


            SummonHeartPlayer SummonHeartPlayer = Main.LocalPlayer.GetModPlayer<SummonHeartPlayer>();

            if (Main.LocalPlayer.GetModPlayer<SummonHeartPlayer>().Level == 15)
            {
                LevelText.SetText("Level: MAX");
            }
            else
            {
                LevelText.SetText("Level: " + SummonHeartPlayer.Level);
            }
            SkillPointsLeftText.SetText("Skill Points: " + SummonHeartPlayer.SkillPoints);
            XPText.SetText("XP: " + SummonHeartPlayer.Experience + "/" + SummonHeartPlayer.XPLimit);

            if (panel.IsMouseHovering)
            {
                Main.LocalPlayer.mouseInterface = true;
            }
            if (RangersDexterity.IsMouseHovering)
            {
                DescriptionText.SetText("[c/ffec00:Ranger's Dexterity]\nMultiplies your movement speed by 20%\nwhile holding a Ranged Weapon");
            }
            if (HunterAcrobatics.IsMouseHovering)
            {
                DescriptionText.SetText("[c/ffec00:Hunter Acrobatics]\nIncreased jump height while holding a Bow\n[c/b40000:Requires: Ranger's Dexterity]");
            }
            if (AerialTakeover.IsMouseHovering)
            {
                DescriptionText.SetText("[c/ffec00:Aerial Takeover]\nPress the Binded Key to propel backwards\nand upwards\n[c/b40000:Requires: Hunter Acrobatics]");
            }
            if (HunterInstincts.IsMouseHovering)
            {
                DescriptionText.SetText("[c/ffec00:Hunter Instincts]\nWhile holding a bow, you can clearly see\nall traps around you\n[c/b40000:Requires: Hunter Acrobatics]");
            }
            if (SharpenedArrows.IsMouseHovering)
            {
                DescriptionText.SetText("[c/ffec00:Sharpened Arrows]\nIncreases armor penetration by 5 while\nwhile holding a Bow\n[c/b40000:Requires: Ranger's Dexterity]");
            }
            if (SuperSharpenedArrows.IsMouseHovering)
            {
                DescriptionText.SetText("[c/ffec00:Super Sharpened Arrows]\nWooden Arrows penetrate 1 enemy\n[c/b40000:Requires: Sharpened Arrows]");
            }
            if (AchillesHeel.IsMouseHovering)
            {
                DescriptionText.SetText("[c/ffec00:Achilles' Heel]\nWhen hitting a non-Boss enemy, you have\na 2% chance of instakilling them\n[c/b40000:Requires: SS Arrows & Lucky Shots]");
            }
            if (PoisonedArrows.IsMouseHovering)
            {
                DescriptionText.SetText("[c/ffec00:Poisoned Arrows]\nArrows have a 15% chance of inflicting\nPoisoned\n[c/b40000:Requires: Sharpened Arrows]");
            }
            if (EaglesEyes.IsMouseHovering)
            {
                DescriptionText.SetText("[c/ffec00:Eagle's Eyes]\n3% increased critical chance\n[c/b40000:Requires: Ranger's Dexterity]");
            }
            if (MarksmansConcentration.IsMouseHovering)
            {
                DescriptionText.SetText("[c/ffec00:Marksman's Concentration]\nWhile holding a Bow and not running fast,\nvision decreased but damage increased by 5%\n[c/b40000:Requires: Eagle's Eyes]");
            }
            if (HuntersFocus.IsMouseHovering)
            {
                DescriptionText.SetText("[c/ffec00:Hunter's Focus]\nWhile holding a Bow and not running fast,\nyou can clearly see all enemies near you\n[c/b40000:Requires: Marksman's Concentration]");
            }
            if (Camouflage.IsMouseHovering)
            {
                DescriptionText.SetText("[c/ffec00:Camouflage]\nWhile standing still or walking very\nslowly, enemy agro is reduced\n[c/b40000:Requires: Hunter's Focus]");
            }
            if (ShadowForm.IsMouseHovering)
            {
                DescriptionText.SetText("[c/ffec00:Shadow Form (Active Ability)]\nEnemy aggro is greatly reduced\nand movement speed is increased\nDuration: 5 Seconds | Cooldown: 20 Seconds\n[c/b40000:Requires: Camouflage]");
            }
            if (ShadowArrows.IsMouseHovering)
            {
                DescriptionText.SetText("[c/ffec00:Shadow Arrows]\nWooden Arrows fired while in Shadow Form\nget empowered with shadows\n[c/b40000:Requires: Shadow Form]");
            }
            if (ShadowBullets.IsMouseHovering)
            {
                DescriptionText.SetText("[c/ffec00:Shadow Bullets]\nMusket Balls fired while in Shadow Form\nget empowered with shadows\n[c/b40000:Requires: Shadow Form]");
            }
            if (BetterGunpowder.IsMouseHovering)
            {
                DescriptionText.SetText("[c/ffec00:Better Gunpowder]\nGuns deal 5% more damage\n[c/b40000:Requires: Ranger's Dexterity]");
            }
            if (DimensionalMagazines.IsMouseHovering)
            {
                DescriptionText.SetText("[c/ffec00:Dimensional Magazines]\nYou have a 15% chance of getting Musket Balls\nback after hitting an enemy with a bullet\n[c/b40000:Requires: Better Gunpowder]");
            }
            if (DimensionalBullets.IsMouseHovering)
            {
                DescriptionText.SetText("[c/ffec00:Dimensional Bullets]\nBullets gotten back from Dimensional Magazines\nhave a 50% chance of being automatically\nfired in a weaker form\n[c/b40000:Requires: Dimensional Magazines]");
            }
            if (LuckyShots.IsMouseHovering)
            {
                DescriptionText.SetText("[c/ffec00:Lucky Shots]\nCritical strikes grant a temporary buff\nthat increases damage by 10%\n[c/b40000:Requires: Better Gunpowder]");
            }
            if (GoldenFingers.IsMouseHovering)
            {
                DescriptionText.SetText("[c/ffec00:Golden Fingers]\nGuns have a 10% chance to inflict Midas\n[c/b40000:Requires: Lucky Shots]");
            }
            if (Speedy.IsMouseHovering)
            {
                DescriptionText.SetText("[c/ffec00:Speedy]\nGuns deal 4% more damage while you are\nrunning fast\n[c/b40000:Requires: Better Gunpowder]");
            }
            if (FasterGelCombustion.IsMouseHovering)
            {
                DescriptionText.SetText("[c/ffec00:Faster Gel Combustion]\nFlamethrowers are 8% faster\n[c/b40000:Requires: Speedy]");
            }
            if (Stress.IsMouseHovering)
            {
                DescriptionText.SetText("[c/ffec00:Stress]\nWhen below 150 health, you deal 10% more damage\n[c/b40000:Requires: Faster Gel Combustion]");
            }
            if (BulletStorm.IsMouseHovering)
            {
                DescriptionText.SetText("[c/ffec00:Bullet Storm]\nWhen below 200 health, Guns are 10% faster\n[c/b40000:Requires: Stress]");
            }
            if (BulletHell.IsMouseHovering)
            {
                DescriptionText.SetText("[c/ffec00:Bullet Hell]\nWhen below 250 health, Guns have a\n33% chance of inflicting On Fire!\n[c/b40000:Requires: Bullet Storm]");
            }

            if (SummonHeartPlayer.RangerDexterity == false)
            {
                RangersDexterity.SetImage(ModContent.GetTexture("UntoldLegends/Sprites/Ranged/RangersDexterity1"));
            }
            else
            {
                RangersDexterity.SetImage(ModContent.GetTexture("UntoldLegends/Sprites/Ranged/RangersDexterity2"));
                BetterGunpowder.SetImage(ModContent.GetTexture("UntoldLegends/Sprites/Ranged/BetterGunpowder1"));
                if (SummonHeartPlayer.BetterGunpowder)
                {
                    BetterGunpowder.SetImage(ModContent.GetTexture("UntoldLegends/Sprites/Ranged/BetterGunpowder2"));
                    DimensionalMagazines.SetImage(ModContent.GetTexture("UntoldLegends/Sprites/Ranged/DimensionalMagazines1"));
                    LuckyShots.SetImage(ModContent.GetTexture("UntoldLegends/Sprites/Ranged/LuckyShots1"));
                    Speedy.SetImage(ModContent.GetTexture("UntoldLegends/Sprites/Ranged/Speedy1"));
                    if (SummonHeartPlayer.Speedy)
                    {
                        Speedy.SetImage(ModContent.GetTexture("UntoldLegends/Sprites/Ranged/Speedy2"));
                        FasterGelCombustion.SetImage(ModContent.GetTexture("UntoldLegends/Sprites/Ranged/FasterGelCombustion1"));
                        if (SummonHeartPlayer.FasterGelCombustion)
                        {
                            FasterGelCombustion.SetImage(ModContent.GetTexture("UntoldLegends/Sprites/Ranged/FasterGelCombustion2"));
                            Stress.SetImage(ModContent.GetTexture("UntoldLegends/Sprites/Ranged/Stress1"));
                            if (SummonHeartPlayer.Stress)
                            {
                                Stress.SetImage(ModContent.GetTexture("UntoldLegends/Sprites/Ranged/Stress2"));
                                BulletStorm.SetImage(ModContent.GetTexture("UntoldLegends/Sprites/Ranged/BulletStorm1"));
                                if (SummonHeartPlayer.BulletStorm)
                                {
                                    BulletStorm.SetImage(ModContent.GetTexture("UntoldLegends/Sprites/Ranged/BulletStorm2"));
                                    BulletHell.SetImage(ModContent.GetTexture("UntoldLegends/Sprites/Ranged/BulletHell1"));
                                    if (SummonHeartPlayer.BulletHell)
                                    {
                                        BulletHell.SetImage(ModContent.GetTexture("UntoldLegends/Sprites/Ranged/BulletHell2"));
                                    }
                                }
                            }
                        }
                    }
                    if (SummonHeartPlayer.LuckyShots)
                    {
                        LuckyShots.SetImage(ModContent.GetTexture("UntoldLegends/Sprites/Ranged/LuckyShots2"));
                        GoldenFingers.SetImage(ModContent.GetTexture("UntoldLegends/Sprites/Ranged/GoldenFingers1"));
                        if (SummonHeartPlayer.GoldenFingers)
                        {
                            GoldenFingers.SetImage(ModContent.GetTexture("UntoldLegends/Sprites/Ranged/GoldenFingers2"));
                        }
                    }
                    if (SummonHeartPlayer.DimensionalMagazines)
                    {
                        DimensionalMagazines.SetImage(ModContent.GetTexture("UntoldLegends/Sprites/Ranged/DimensionalMagazines2"));
                        DimensionalBullets.SetImage(ModContent.GetTexture("UntoldLegends/Sprites/Ranged/DimensionalBullets1"));
                        if (SummonHeartPlayer.DimensionalBullets)
                        {
                            DimensionalBullets.SetImage(ModContent.GetTexture("UntoldLegends/Sprites/Ranged/DimensionalBullets2"));
                        }
                    }
                }
                if (SummonHeartPlayer.EaglesEyes == false)
                {
                    EaglesEyes.SetImage(ModContent.GetTexture("UntoldLegends/Sprites/Ranged/EaglesEyes1"));
                }
                else
                {
                    EaglesEyes.SetImage(ModContent.GetTexture("UntoldLegends/Sprites/Ranged/EaglesEyes2"));
                    if (SummonHeartPlayer.MarksmansConcentration == false)
                    {
                        MarksmansConcentration.SetImage(ModContent.GetTexture("UntoldLegends/Sprites/Ranged/MarksmansConcentration1"));
                    }
                    else
                    {
                        MarksmansConcentration.SetImage(ModContent.GetTexture("UntoldLegends/Sprites/Ranged/MarksmansConcentration2"));
                        if (SummonHeartPlayer.HuntersFocus == false)
                        {
                            HuntersFocus.SetImage(ModContent.GetTexture("UntoldLegends/Sprites/Ranged/HuntersFocus1"));
                        }
                        else
                        {
                            HuntersFocus.SetImage(ModContent.GetTexture("UntoldLegends/Sprites/Ranged/HuntersFocus2"));
                            if (SummonHeartPlayer.Camouflage == false)
                            {
                                Camouflage.SetImage(ModContent.GetTexture("UntoldLegends/Sprites/Ranged/Camouflage1"));
                            }
                            else
                            {
                                Camouflage.SetImage(ModContent.GetTexture("UntoldLegends/Sprites/Ranged/Camouflage2"));
                                if (SummonHeartPlayer.ShadowFormSkill == false)
                                {
                                    ShadowForm.SetImage(ModContent.GetTexture("UntoldLegends/Sprites/Ranged/ShadowForm1"));
                                }
                                else
                                {
                                    ShadowForm.SetImage(ModContent.GetTexture("UntoldLegends/Sprites/Ranged/ShadowForm2"));
                                    ShadowArrows.SetImage(ModContent.GetTexture("UntoldLegends/Sprites/Ranged/ShadowArrows1"));
                                    ShadowBullets.SetImage(ModContent.GetTexture("UntoldLegends/Sprites/Ranged/ShadowBullets1"));
                                    if (SummonHeartPlayer.ShadowArrows)
                                    {
                                        ShadowArrows.SetImage(ModContent.GetTexture("UntoldLegends/Sprites/Ranged/ShadowArrows2"));
                                    }
                                    if (SummonHeartPlayer.ShadowBullets)
                                    {
                                        ShadowBullets.SetImage(ModContent.GetTexture("UntoldLegends/Sprites/Ranged/ShadowBullets2"));
                                    }
                                }
                            }
                        }
                    }
                }
                if (SummonHeartPlayer.SharpenedArrows == false)
                {
                    SharpenedArrows.SetImage(ModContent.GetTexture("UntoldLegends/Sprites/Ranged/SharpenedArrows1"));
                }
                else
                {
                    SharpenedArrows.SetImage(ModContent.GetTexture("UntoldLegends/Sprites/Ranged/SharpenedArrows2"));
                    if (SummonHeartPlayer.PoisonedArrows == false)
                    {
                        PoisonedArrows.SetImage(ModContent.GetTexture("UntoldLegends/Sprites/Ranged/PoisonedArrows1"));
                    }
                    else
                    {
                        PoisonedArrows.SetImage(ModContent.GetTexture("UntoldLegends/Sprites/Ranged/PoisonedArrows2"));
                    }
                    if (SummonHeartPlayer.SuperSharpenedArrows == false)
                    {
                        SuperSharpenedArrows.SetImage(ModContent.GetTexture("UntoldLegends/Sprites/Ranged/SuperSharpenedArrows1"));
                    }
                    else
                    {
                        SuperSharpenedArrows.SetImage(ModContent.GetTexture("UntoldLegends/Sprites/Ranged/SuperSharpenedArrows2"));
                        if (SummonHeartPlayer.AchillesHeel)
                        {
                            AchillesHeel.SetImage(ModContent.GetTexture("UntoldLegends/Sprites/Ranged/AchillesHeel2"));
                        }
                        else if (!SummonHeartPlayer.AchillesHeel && SummonHeartPlayer.LuckyShots)
                        {
                            AchillesHeel.SetImage(ModContent.GetTexture("UntoldLegends/Sprites/Ranged/AchillesHeel1"));
                        }
                    }
                }
                if (SummonHeartPlayer.HunterAcrobatics == false)
                {
                    HunterAcrobatics.SetImage(ModContent.GetTexture("UntoldLegends/Sprites/Ranged/HunterAcrobatics1"));
                }
                else
                {
                    HunterAcrobatics.SetImage(ModContent.GetTexture("UntoldLegends/Sprites/Ranged/HunterAcrobatics2"));
                    if (SummonHeartPlayer.AerialTakeover == false)
                    {
                        AerialTakeover.SetImage(ModContent.GetTexture("UntoldLegends/Sprites/Ranged/AerialTakeover1"));
                    }
                    else
                    {
                        AerialTakeover.SetImage(ModContent.GetTexture("UntoldLegends/Sprites/Ranged/AerialTakeover2"));
                    }
                    if (SummonHeartPlayer.HunterInstincts == false)
                    {
                        HunterInstincts.SetImage(ModContent.GetTexture("UntoldLegends/Sprites/Ranged/HunterInstincts1"));
                    }
                    else
                    {
                        HunterInstincts.SetImage(ModContent.GetTexture("UntoldLegends/Sprites/Ranged/HunterInstincts2"));
                    }
                }
            }
        }
        private void OnRangersDexterity(UIMouseEvent evt, UIElement listeningElement)
        {
            SummonHeartPlayer SummonHeartPlayer = Main.LocalPlayer.GetModPlayer<SummonHeartPlayer>();
            if (!SummonHeartPlayer.RangerDexterity && SummonHeartPlayer.SkillPoints >= 1)
            {
                SummonHeartPlayer.RangerDexterity = true;
                SummonHeartPlayer.SkillPoints--;
            }
        }
        private void OnHunterAcrobatics(UIMouseEvent evt, UIElement listeningElement)
        {
            SummonHeartPlayer SummonHeartPlayer = Main.LocalPlayer.GetModPlayer<SummonHeartPlayer>();
            if (!SummonHeartPlayer.HunterAcrobatics && SummonHeartPlayer.SkillPoints >= 1 && SummonHeartPlayer.RangerDexterity)
            {
                SummonHeartPlayer.HunterAcrobatics = true;
                SummonHeartPlayer.SkillPoints--;
            }
        }
        private void OnAerialTakeover(UIMouseEvent evt, UIElement listeningElement)
        {
            SummonHeartPlayer SummonHeartPlayer = Main.LocalPlayer.GetModPlayer<SummonHeartPlayer>();
            if (!SummonHeartPlayer.AerialTakeover && SummonHeartPlayer.SkillPoints >= 1 && SummonHeartPlayer.HunterAcrobatics)
            {
                SummonHeartPlayer.AerialTakeover = true;
                SummonHeartPlayer.SkillPoints--;
            }
        }
        private void OnHunterInstincts(UIMouseEvent evt, UIElement listeningElement)
        {
            SummonHeartPlayer SummonHeartPlayer = Main.LocalPlayer.GetModPlayer<SummonHeartPlayer>();
            if (!SummonHeartPlayer.HunterInstincts && SummonHeartPlayer.SkillPoints >= 1 && SummonHeartPlayer.HunterAcrobatics)
            {
                SummonHeartPlayer.HunterInstincts = true;
                SummonHeartPlayer.SkillPoints--;
            }
        }
        private void OnSharpenedArrows(UIMouseEvent evt, UIElement listeningElement)
        {
            SummonHeartPlayer SummonHeartPlayer = Main.LocalPlayer.GetModPlayer<SummonHeartPlayer>();
            if (!SummonHeartPlayer.SharpenedArrows && SummonHeartPlayer.SkillPoints >= 1 && SummonHeartPlayer.RangerDexterity)
            {
                SummonHeartPlayer.SharpenedArrows = true;
                SummonHeartPlayer.SkillPoints--;
            }
        }
        private void OnSuperSharpenedArrows(UIMouseEvent evt, UIElement listeningElement)
        {
            SummonHeartPlayer SummonHeartPlayer = Main.LocalPlayer.GetModPlayer<SummonHeartPlayer>();
            if (!SummonHeartPlayer.SuperSharpenedArrows && SummonHeartPlayer.SkillPoints >= 1 && SummonHeartPlayer.SharpenedArrows)
            {
                SummonHeartPlayer.SuperSharpenedArrows = true;
                SummonHeartPlayer.SkillPoints--;
            }
        }
        private void OnAchillesHeel(UIMouseEvent evt, UIElement listeningElement)
        {
            SummonHeartPlayer SummonHeartPlayer = Main.LocalPlayer.GetModPlayer<SummonHeartPlayer>();
            if (!SummonHeartPlayer.AchillesHeel && SummonHeartPlayer.SkillPoints >= 1 && SummonHeartPlayer.SuperSharpenedArrows && SummonHeartPlayer.LuckyShots)
            {
                SummonHeartPlayer.AchillesHeel = true;
                SummonHeartPlayer.SkillPoints--;
            }
        }
        private void OnPoisonedArrows(UIMouseEvent evt, UIElement listeningElement)
        {
            SummonHeartPlayer SummonHeartPlayer = Main.LocalPlayer.GetModPlayer<SummonHeartPlayer>();
            if (!SummonHeartPlayer.PoisonedArrows && SummonHeartPlayer.SkillPoints >= 1 && SummonHeartPlayer.SharpenedArrows)
            {
                SummonHeartPlayer.PoisonedArrows = true;
                SummonHeartPlayer.SkillPoints--;
            }
        }
        private void OnEaglesEyes(UIMouseEvent evt, UIElement listeningElement)
        {
            SummonHeartPlayer SummonHeartPlayer = Main.LocalPlayer.GetModPlayer<SummonHeartPlayer>();
            if (!SummonHeartPlayer.EaglesEyes && SummonHeartPlayer.SkillPoints >= 1 && SummonHeartPlayer.RangerDexterity)
            {
                SummonHeartPlayer.EaglesEyes = true;
                SummonHeartPlayer.SkillPoints--;
            }
        }
        private void OnMarksmansConcentration(UIMouseEvent evt, UIElement listeningElement)
        {
            SummonHeartPlayer SummonHeartPlayer = Main.LocalPlayer.GetModPlayer<SummonHeartPlayer>();
            if (!SummonHeartPlayer.MarksmansConcentration && SummonHeartPlayer.SkillPoints >= 1 && SummonHeartPlayer.EaglesEyes)
            {
                SummonHeartPlayer.MarksmansConcentration = true;
                SummonHeartPlayer.SkillPoints--;
            }
        }
        private void OnHuntersFocus(UIMouseEvent evt, UIElement listeningElement)
        {
            SummonHeartPlayer SummonHeartPlayer = Main.LocalPlayer.GetModPlayer<SummonHeartPlayer>();
            if (!SummonHeartPlayer.HuntersFocus && SummonHeartPlayer.SkillPoints >= 1 && SummonHeartPlayer.MarksmansConcentration)
            {
                SummonHeartPlayer.HuntersFocus = true;
                SummonHeartPlayer.SkillPoints--;
            }
        }
        private void OnCamouflage(UIMouseEvent evt, UIElement listeningElement)
        {
            SummonHeartPlayer SummonHeartPlayer = Main.LocalPlayer.GetModPlayer<SummonHeartPlayer>();
            if (!SummonHeartPlayer.Camouflage && SummonHeartPlayer.SkillPoints >= 1 && SummonHeartPlayer.HuntersFocus)
            {
                SummonHeartPlayer.Camouflage = true;
                SummonHeartPlayer.SkillPoints--;
            }
        }
        private void OnShadowForm(UIMouseEvent evt, UIElement listeningElement)
        {
            SummonHeartPlayer SummonHeartPlayer = Main.LocalPlayer.GetModPlayer<SummonHeartPlayer>();
            if (!SummonHeartPlayer.ShadowFormSkill && SummonHeartPlayer.SkillPoints >= 1 && SummonHeartPlayer.Camouflage)
            {
                SummonHeartPlayer.ShadowFormSkill = true;
                SummonHeartPlayer.SkillPoints--;
            }
        }
        private void OnShadowArrows(UIMouseEvent evt, UIElement listeningElement)
        {
            SummonHeartPlayer SummonHeartPlayer = Main.LocalPlayer.GetModPlayer<SummonHeartPlayer>();
            if (!SummonHeartPlayer.ShadowArrows && SummonHeartPlayer.SkillPoints >= 1 && SummonHeartPlayer.ShadowFormSkill)
            {
                SummonHeartPlayer.ShadowArrows = true;
                SummonHeartPlayer.SkillPoints--;
            }
        }
        private void OnShadowBullets(UIMouseEvent evt, UIElement listeningElement)
        {
            SummonHeartPlayer SummonHeartPlayer = Main.LocalPlayer.GetModPlayer<SummonHeartPlayer>();
            if (!SummonHeartPlayer.ShadowBullets && SummonHeartPlayer.SkillPoints >= 1 && SummonHeartPlayer.ShadowFormSkill)
            {
                SummonHeartPlayer.ShadowBullets = true;
                SummonHeartPlayer.SkillPoints--;
            }
        }
        private void OnBetterGunpowder(UIMouseEvent evt, UIElement listeningElement)
        {
            SummonHeartPlayer SummonHeartPlayer = Main.LocalPlayer.GetModPlayer<SummonHeartPlayer>();
            if (!SummonHeartPlayer.BetterGunpowder && SummonHeartPlayer.SkillPoints >= 1 && SummonHeartPlayer.RangerDexterity)
            {
                SummonHeartPlayer.BetterGunpowder = true;
                SummonHeartPlayer.SkillPoints--;
            }
        }
        private void OnDimensionalMagazines(UIMouseEvent evt, UIElement listeningElement)
        {
            SummonHeartPlayer SummonHeartPlayer = Main.LocalPlayer.GetModPlayer<SummonHeartPlayer>();
            if (!SummonHeartPlayer.DimensionalMagazines && SummonHeartPlayer.SkillPoints >= 1 && SummonHeartPlayer.BetterGunpowder)
            {
                SummonHeartPlayer.DimensionalMagazines = true;
                SummonHeartPlayer.SkillPoints--;
            }
        }
        private void OnDimensionalBullets(UIMouseEvent evt, UIElement listeningElement)
        {
            SummonHeartPlayer SummonHeartPlayer = Main.LocalPlayer.GetModPlayer<SummonHeartPlayer>();
            if (!SummonHeartPlayer.DimensionalBullets && SummonHeartPlayer.SkillPoints >= 1 && SummonHeartPlayer.DimensionalMagazines)
            {
                SummonHeartPlayer.DimensionalBullets = true;
                SummonHeartPlayer.SkillPoints--;
            }
        }
        private void OnLuckyShots(UIMouseEvent evt, UIElement listeningElement)
        {
            SummonHeartPlayer SummonHeartPlayer = Main.LocalPlayer.GetModPlayer<SummonHeartPlayer>();
            if (!SummonHeartPlayer.LuckyShots && SummonHeartPlayer.SkillPoints >= 1 && SummonHeartPlayer.BetterGunpowder)
            {
                SummonHeartPlayer.LuckyShots = true;
                SummonHeartPlayer.SkillPoints--;
            }
        }
        private void OnGoldenFingers(UIMouseEvent evt, UIElement listeningElement)
        {
            SummonHeartPlayer SummonHeartPlayer = Main.LocalPlayer.GetModPlayer<SummonHeartPlayer>();
            if (!SummonHeartPlayer.GoldenFingers && SummonHeartPlayer.SkillPoints >= 1 && SummonHeartPlayer.LuckyShots)
            {
                SummonHeartPlayer.GoldenFingers = true;
                SummonHeartPlayer.SkillPoints--;
            }
        }
        private void OnSpeedy(UIMouseEvent evt, UIElement listeningElement)
        {
            SummonHeartPlayer SummonHeartPlayer = Main.LocalPlayer.GetModPlayer<SummonHeartPlayer>();
            if (!SummonHeartPlayer.Speedy && SummonHeartPlayer.SkillPoints >= 1 && SummonHeartPlayer.BetterGunpowder)
            {
                SummonHeartPlayer.Speedy = true;
                SummonHeartPlayer.SkillPoints--;
            }
        }
        private void OnFasterGelCombustion(UIMouseEvent evt, UIElement listeningElement)
        {
            SummonHeartPlayer SummonHeartPlayer = Main.LocalPlayer.GetModPlayer<SummonHeartPlayer>();
            if (!SummonHeartPlayer.FasterGelCombustion && SummonHeartPlayer.SkillPoints >= 1 && SummonHeartPlayer.Speedy)
            {
                SummonHeartPlayer.FasterGelCombustion = true;
                SummonHeartPlayer.SkillPoints--;
            }
        }
        private void OnStress(UIMouseEvent evt, UIElement listeningElement)
        {
            SummonHeartPlayer SummonHeartPlayer = Main.LocalPlayer.GetModPlayer<SummonHeartPlayer>();
            if (!SummonHeartPlayer.Stress && SummonHeartPlayer.SkillPoints >= 1 && SummonHeartPlayer.FasterGelCombustion)
            {
                SummonHeartPlayer.Stress = true;
                SummonHeartPlayer.SkillPoints--;
            }
        }
        private void OnBulletStorm(UIMouseEvent evt, UIElement listeningElement)
        {
            SummonHeartPlayer SummonHeartPlayer = Main.LocalPlayer.GetModPlayer<SummonHeartPlayer>();
            if (!SummonHeartPlayer.BulletStorm && SummonHeartPlayer.SkillPoints >= 1 && SummonHeartPlayer.Stress)
            {
                SummonHeartPlayer.BulletStorm = true;
                SummonHeartPlayer.SkillPoints--;
            }
        }
        private void OnBulletHell(UIMouseEvent evt, UIElement listeningElement)
        {
            SummonHeartPlayer SummonHeartPlayer = Main.LocalPlayer.GetModPlayer<SummonHeartPlayer>();
            if (!SummonHeartPlayer.BulletHell && SummonHeartPlayer.SkillPoints >= 1 && SummonHeartPlayer.BulletStorm)
            {
                SummonHeartPlayer.BulletHell = true;
                SummonHeartPlayer.SkillPoints--;
            }
        }
    }
}*/