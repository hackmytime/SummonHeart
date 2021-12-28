﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria.GameContent.UI.Elements;
using Terraria.UI;
using Terraria.ID;
using Terraria;
using System;
using Terraria.ModLoader;
using System.Reflection;
using Terraria.GameInput;
using Terraria.Localization;
using SummonHeart;
using SummonHeart.RPGModule.Entities;
using SummonHeart.Utilities;
using SummonHeart.RPGModule.Enum;

namespace SummonHeart.ui
{
    class OpenSTButton : UIState
    {
        public static bool visible = true;
        public UIElement OpenSTPanel;
        public float scale = Config.vConfig.HealthBarScale;
        public float yOffSet = Config.vConfig.HealthBarYoffSet;
        public bool hiden = false;
        Texture2D Button;

        public void Erase()
        {

            RemoveAllChildren();
        }

        public override void Update(GameTime gameTime)
        {
            if (!Config.gpConfig.RPGPlayer)
            {
                RemoveAllChildren();
                hiden = true;
                return;
            }

            if (hiden)
            {
                hiden = false;
                OnInitialize();
            }
            base.Update(gameTime);
        }

        public override void OnInitialize()
        {
            LoadTexture();
            Reset();
        }

        public void LoadTexture()
        {
            Button = ModContent.GetTexture("SummonHeart/Textures/UI/skill_tree");
        }

        public void Reset()
        {
            Erase();
            yOffSet = Config.vConfig.HealthBarYoffSet;
            scale = Config.vConfig.HealthBarScale;

            OpenSTPanel = new UIElement();
            OpenSTPanel.SetPadding(0);
            OpenSTPanel.Left.Set(90 * scale, 0f);
            OpenSTPanel.Top.Set(Main.screenHeight - 175 * scale - yOffSet, 0f);
            OpenSTPanel.Width.Set(32 * scale, 0f);
            OpenSTPanel.Height.Set(64 * scale, 0f);
            OpenSTPanel.HAlign = 0;
            OpenSTPanel.VAlign = 0;
        }
    }
    class OpenStatsButton : UIState
    {
        public static bool visible = true;
        public UIElement OpenStatsPanel;
        public float scale = Config.vConfig.HealthBarScale;
        public float yOffSet = Config.vConfig.HealthBarYoffSet;
        Texture2D Button;
        public bool hiden = false;


        public void Erase()
        {

            RemoveAllChildren();
        }

        public override void Update(GameTime gameTime)
        {
            if (!Config.gpConfig.RPGPlayer)
            {
                RemoveAllChildren();
                hiden = true;
                return;
            }

            if (hiden)
            {
                hiden = false;
                OnInitialize();
            }
            base.Update(gameTime);
        }

        public override void OnInitialize()
        {
            LoadTexture();
            Reset();
        }

        public void LoadTexture()
        {
            Button = ModContent.GetTexture("SummonHeart/Textures/UI/character");
        }

        public void Reset()
        {
            Erase();
            yOffSet = Config.vConfig.HealthBarYoffSet;
            scale = Config.vConfig.HealthBarScale;

            OpenStatsPanel = new UIElement();
            OpenStatsPanel.SetPadding(0);
            OpenStatsPanel.Left.Set((57 + 32 + 4) * scale, 0f);
            OpenStatsPanel.Top.Set(Main.screenHeight - 175 * scale - yOffSet, 0f);
            OpenStatsPanel.Width.Set(32 * scale, 0f);
            OpenStatsPanel.Height.Set(64 * scale, 0f);
            OpenStatsPanel.HAlign = 0;
            OpenStatsPanel.VAlign = 0;

            OpenStatButton OpenButton = new OpenStatButton(Button);
            OpenButton.Left.Set(0, 0f);
            OpenButton.Top.Set(0, 0f);
            OpenButton.ImageScale = scale;
            OpenButton.Width.Set(32 * scale, 0f);
            OpenButton.Height.Set(64 * scale, 0f);
            OpenButton.OnClick += new MouseEvent(OpenStatMenu);
            OpenButton.HAlign = 0;
            OpenButton.VAlign = 0;
            OpenStatsPanel.Append(OpenButton);
            Recalculate();
            Append(OpenStatsPanel);
        }
        public void OpenStatMenu(UIMouseEvent evt, UIElement listeningElement)
        {
            if (!Config.gpConfig.RPGPlayer)
                return;
            Main.PlaySound(SoundID.MenuOpen);
            Stats.Instance.LoadChar();
            Stats.visible = !Stats.visible;
        }
    }
    class Stats : UIState
    {
        private float SizeMultiplier = 1;

        public static Stats Instance;
        public UIPanel statsPanel;
        private RPGPlayer Char;
        public static bool visible = false;

        public void LoadChar()
        {
            Char = Main.player[Main.myPlayer].GetModPlayer<RPGPlayer>();

        }

        private UIText[] UpgradeStatText = new UIText[8];
        private UIText[] UpgradeStatDetails = new UIText[12];
        private UIText[] UpgradeStatOver = new UIText[12];
        private UIText[] StatProgress = new UIText[8];

        public StatProgress[] progressStatsBar = new StatProgress[8];
        public ProgressBG[] progressStatsBarBG = new ProgressBG[8];

        private UIText PointsLeft = new UIText("");

        UIText ResetText;
        UIText InfoStat;
        private float baseYOffset = 100;
        private float baseXOffset = 100;
        private float YOffset = 35;
        private float XOffset = 120;

        private void ResetTextHover(UIMouseEvent evt, UIElement listeningElement)
        {
            ResetText.TextColor = Color.White;
        }
        private void ResetTextOut(UIMouseEvent evt, UIElement listeningElement)
        {
            ResetText.TextColor = Color.Gray;
        }

        public override void OnInitialize()
        {


            SizeMultiplier = Main.screenHeight / 1080f;
            baseYOffset *= SizeMultiplier;
            baseXOffset *= SizeMultiplier;
            YOffset *= SizeMultiplier;
            XOffset *= SizeMultiplier;


            Instance = this;
            statsPanel = new UIPanel();
            statsPanel.SetPadding(0);
            statsPanel.Left.Set(400f * SizeMultiplier, 0f);
            statsPanel.Top.Set(100f * SizeMultiplier, 0f);
            statsPanel.Width.Set(1000 * SizeMultiplier, 0f);
            statsPanel.Height.Set(600 * SizeMultiplier, 0f);
            statsPanel.BackgroundColor = new Color(73, 94, 171, 150);

            statsPanel.OnMouseDown += new MouseEvent(DragStart);
            statsPanel.OnMouseUp += new MouseEvent(DragEnd);

            PointsLeft = new UIText("轮回道源 : 0 / 0", SizeMultiplier);
            PointsLeft.Left.Set(250 * SizeMultiplier, 0f);
            PointsLeft.Top.Set(20 * SizeMultiplier, 0f);
            PointsLeft.Width.Set(0, 0f);
            PointsLeft.Height.Set(0, 0f);
            statsPanel.Append(PointsLeft);



            ResetText = new UIText("轮回", SizeMultiplier, true)
            {
                TextColor = Color.Gray
            };
            ResetText.Left.Set(50 * SizeMultiplier, 0f);
            ResetText.Top.Set(20 * SizeMultiplier, 0f);
            ResetText.Width.Set(0, 0f);
            ResetText.Height.Set(0, 0f);
            ResetText.OnClick += new MouseEvent(ResetStats);
            ResetText.OnMouseOver += new MouseEvent(ResetTextHover);
            ResetText.OnMouseOut += new MouseEvent(ResetTextOut);
            statsPanel.Append(ResetText);

            Texture2D Button = ModContent.GetTexture("Terraria/UI/ButtonPlay");
            for (int i = 0; i < 12; i++)
            {
                if (i < 8)
                {
                    UIImageButton UpgradeStatButton = new UIImageButton(Button);

                    UpgradeStatButton.Left.Set(baseXOffset + XOffset * 2, 0f);
                    UpgradeStatButton.Top.Set(baseYOffset + YOffset * i, 0f);
                    UpgradeStatButton.Width.Set(22 * SizeMultiplier, 0f);
                    UpgradeStatButton.Height.Set(22 * SizeMultiplier, 0f);
                    Stat Statused = (Stat)i;
                    UpgradeStatButton.OnMouseOver += new MouseEvent((UIMouseEvent, UIElement) => UpdateStat(UIMouseEvent, UIElement, Statused));
                    UpgradeStatButton.OnMouseOut += new MouseEvent(ResetOver);
                    UpgradeStatButton.OnClick += new MouseEvent((UIMouseEvent, UIElement) => UpgradeStat(UIMouseEvent, UIElement, Statused, 1));
                    UpgradeStatButton.OnRightClick += new MouseEvent((UIMouseEvent, UIElement) => UpgradeStat(UIMouseEvent, UIElement, Statused, 5));
                    UpgradeStatButton.OnMiddleClick += new MouseEvent((UIMouseEvent, UIElement) => UpgradeStat(UIMouseEvent, UIElement, Statused, 25));
                    UpgradeStatButton.OnScrollWheel += new ScrollWheelEvent((UIMouseEvent, UIElement) => UpgradeStatWheel(UIMouseEvent, UIElement, Statused));
                    statsPanel.Append(UpgradeStatButton);


                    progressStatsBar[i] = new StatProgress((Stat)i, ModContent.GetTexture("SummonHeart/Textures/UI/Blank"));
                    progressStatsBar[i].Left.Set(baseXOffset + XOffset * 1.0f, 0f);
                    progressStatsBar[i].Top.Set(baseYOffset + YOffset * i + 6, 0f);
                    progressStatsBar[i].Width.Set(105, 0);
                    progressStatsBar[i].HAlign = 0;
                    progressStatsBar[i].Height.Set(10, 0);
                    progressStatsBar[i].width = 105;
                    progressStatsBar[i].left = baseYOffset + YOffset * i;
                    statsPanel.Append(progressStatsBar[i]);

                    progressStatsBarBG[i] = new ProgressBG(ModContent.GetTexture("SummonHeart/Textures/UI/Blank"));
                    progressStatsBarBG[i].Left.Set(baseXOffset + XOffset * 1.0f, 0f);
                    progressStatsBarBG[i].Top.Set(baseYOffset + YOffset * i + 6, 0f);
                    progressStatsBarBG[i].Width.Set(105, 0);
                    progressStatsBarBG[i].HAlign = 0;
                    progressStatsBarBG[i].Height.Set(10, 0);
                    progressStatsBarBG[i].color = new Color(10, 0, 0, 128);
                    progressStatsBar[i].left = baseYOffset + YOffset * i;

                    statsPanel.Append(progressStatsBarBG[i]);

                    StatProgress[i] = new UIText("0", SizeMultiplier);
                    StatProgress[i].SetText("0/2");
                    StatProgress[i].Left.Set(baseXOffset + XOffset * 2.3f, 0f);
                    StatProgress[i].Top.Set(baseYOffset + YOffset * i, 0f);
                    StatProgress[i].HAlign = 0f;
                    StatProgress[i].VAlign = 0f;
                    StatProgress[i].MinWidth.Set(150 * SizeMultiplier, 0);
                    StatProgress[i].MaxWidth.Set(150 * SizeMultiplier, 0);

                    statsPanel.Append(StatProgress[i]);


                    UpgradeStatText[i] = new UIText("0", SizeMultiplier);
                    UpgradeStatText[i].SetText("Mana : 10 + 10");
                    UpgradeStatText[i].Left.Set(baseXOffset - 75, 0f);
                    UpgradeStatText[i].Top.Set(baseYOffset + YOffset * i, 0f);
                    UpgradeStatText[i].HAlign = 0f;
                    UpgradeStatText[i].VAlign = 0f;
                    UpgradeStatText[i].MinWidth.Set(150 * SizeMultiplier, 0);
                    UpgradeStatText[i].MaxWidth.Set(150 * SizeMultiplier, 0);
                    statsPanel.Append(UpgradeStatText[i]);


                }

                InfoStat = new UIText("0", SizeMultiplier);
                InfoStat.SetText("");
                InfoStat.Left.Set(baseXOffset - 75 * SizeMultiplier, 0f);
                InfoStat.Top.Set(baseYOffset + 300 * SizeMultiplier, 0f);
                InfoStat.HAlign = 0f;
                InfoStat.VAlign = 0f;
                InfoStat.MinWidth.Set(150 * SizeMultiplier, 0);
                InfoStat.MaxWidth.Set(150 * SizeMultiplier, 0);
                statsPanel.Append(InfoStat);

                UpgradeStatDetails[i] = new UIText("", SizeMultiplier);
                if (i < 3 || i > 7)
                {

                    UpgradeStatDetails[i].SetText("Health : 100 - 5 Heart x 20 Health Per Heart");
                }
                UpgradeStatDetails[i].SetText("Melee Damage Multiplier : 1");
                UpgradeStatDetails[i].Left.Set(baseXOffset + XOffset * 2.9f, 0f);
                UpgradeStatDetails[i].Top.Set(baseYOffset + YOffset * i, 0f);
                UpgradeStatDetails[i].HAlign = 0f;
                UpgradeStatDetails[i].VAlign = 0f;
                UpgradeStatDetails[i].MinWidth.Set(300f * SizeMultiplier, 0);
                UpgradeStatDetails[i].MaxWidth.Set(300f * SizeMultiplier, 0);
                statsPanel.Append(UpgradeStatDetails[i]);

                UpgradeStatOver[i] = new UIText("", SizeMultiplier)
                {
                    TextColor = Color.Aqua
                };
                UpgradeStatOver[i].SetText("");
                UpgradeStatOver[i].Left.Set(baseXOffset + XOffset * 6.7f, 0f);
                UpgradeStatOver[i].Top.Set(baseYOffset + YOffset * i, 0f);
                UpgradeStatOver[i].HAlign = 0f;
                UpgradeStatOver[i].VAlign = 0f;
                UpgradeStatOver[i].MinWidth.Set(20f * SizeMultiplier, 0);
                UpgradeStatOver[i].MaxWidth.Set(20f * SizeMultiplier, 0);
                statsPanel.Append(UpgradeStatOver[i]);
            }
            Append(statsPanel);



        }

        private Color MainColor = new Color(75, 75, 255);
        private Color SecondaryColor = new Color(150, 150, 255);

        float GetCritImprov()
        {
            return Math.Abs(Mathf.Round(Mathf.Pow(Char.GetStatImproved(Stat.道心) + Char.GetStatImproved(Stat.气运) + 1, 0.8f) * 0.005f - Char.GetCriticalDamage(), 2));
        }

        float GetCritChanceImprov()
        {
            return Math.Abs(Mathf.Round(Mathf.Pow(Char.GetStatImproved(Stat.悟性) + Char.GetStatImproved(Stat.功法) + 1, 0.8f) * 0.05f - Char.GetCriticalChanceBonus(), 3));
        }

        public void UpdateStat(UIMouseEvent evt, UIElement listeningElement, Stat stat)
        {

            Recalculate();
            if (Char == null)
            {
                LoadChar();
            }

            for (int i = 0; i < 12; i++)
            {
                UpgradeStatOver[i].TextColor = SecondaryColor;
            }
            switch (stat)
            {
                case Stat.灵根:
                    UpgradeStatOver[11].SetText("+ 0.05倍");
                    UpgradeStatOver[11].TextColor = MainColor;
                    break;
                case Stat.悟性:
                    UpgradeStatOver[1].SetText("+ " + Char.player.statManaMax / 20f * 0.02f * Char.statMultiplier + " Mana");
                    UpgradeStatOver[1].TextColor = MainColor;
                    UpgradeStatOver[7].SetText("+ " + RPGPlayer.SECONDARYTATSMULT * Char.statMultiplier + " Multiplier");
                    UpgradeStatOver[7].TextColor = SecondaryColor;
                    UpgradeStatOver[8].SetText("+ " + GetCritChanceImprov() + " %");

                    break;
                case Stat.魅力:
                    UpgradeStatOver[0].SetText("+ " + Char.player.statLifeMax / 20f * 0.325f * Char.statMultiplier + " Hp");
                    UpgradeStatOver[0].TextColor = SecondaryColor;
                    UpgradeStatOver[2].SetText("+ " + Char.BaseArmor * 0.006f * Char.statMultiplier + " Armor");
                    UpgradeStatOver[2].TextColor = MainColor;
                    UpgradeStatOver[10].SetText("+ " + 0.02f * Char.statMultiplier + " HP/Sec");
                    break;
                case Stat.气运:
                    UpgradeStatOver[3].SetText("+ " + RPGPlayer.MAINSTATSMULT * Char.statMultiplier + " Multiplier");
                    UpgradeStatOver[3].TextColor = MainColor;
                    UpgradeStatOver[5].SetText("+ " + RPGPlayer.SECONDARYTATSMULT * Char.statMultiplier + " Multiplier");
                    UpgradeStatOver[5].TextColor = SecondaryColor;
                    UpgradeStatOver[9].SetText("+ " + GetCritImprov() * 0.01f + " %");
                    break;
                case Stat.道心:
                    break;
                case Stat.功法:
                    UpgradeStatOver[11].SetText("+ 0.1");
                    UpgradeStatOver[11].TextColor = MainColor;
                    UpgradeStatOver[6].SetText("+ 0.01倍");
                    UpgradeStatOver[6].TextColor = SecondaryColor;
                    break;
                case Stat.体质:
                    UpgradeStatOver[0].SetText("+ " + (((float)Char.player.statLifeMax / 20f) * Char.statMultiplier) + " Hp");
                    UpgradeStatOver[0].TextColor = MainColor;
                    UpgradeStatOver[10].SetText("+ " + 0.1f * Char.statMultiplier + " HP/Sec");
                    UpgradeStatOver[10].TextColor = MainColor;
                    break;
                case Stat.力量:
                    UpgradeStatOver[7].SetText("+ " + RPGPlayer.MAINSTATSMULT * Char.statMultiplier + " Multiplier");
                    UpgradeStatOver[7].TextColor = MainColor;
                    UpgradeStatOver[6].SetText("+ " + RPGPlayer.SECONDARYTATSMULT * Char.statMultiplier + " Multiplier");
                    UpgradeStatOver[6].TextColor = SecondaryColor;
                    UpgradeStatOver[11].SetText("+ " + 0.02f * Char.statMultiplier + " MP/Sec");
                    break;
            }
            InfoStat.SetText(AdditionalInfo.GetAdditionalStatInfo(stat));
        }
        public void ResetOver(UIMouseEvent evt, UIElement listeningElement)
        {
            for (int i = 0; i < 12; i++)
            {
                UpgradeStatOver[i].SetText("");
            }
            InfoStat.SetText("");
        }
        private void ResetStats(UIMouseEvent evt, UIElement listeningElement)
        {
            if (!visible)
                return;
            Main.PlaySound(SoundID.MenuOpen);
            Char.ResetStats();
        }


        private void UpgradeStat(UIMouseEvent evt, UIElement listeningElement, Stat stat, int amount)
        {
            if (!visible)
                return;
            Main.PlaySound(SoundID.MenuOpen);
            if (Main.keyState.PressingShift())
            {
                if (Main.keyState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.LeftControl))
                {
                    while (Char.FreePtns > 0)
                        Char.SpendPoints(stat, Char.GetStatXPMax(stat) - Char.GetStatXP(stat));
                    return;
                }

                for (int i = 0; i < amount; i++)
                    Char.SpendPoints(stat, Char.GetStatXPMax(stat) - Char.GetStatXP(stat));
                return;
            }
            Char.SpendPoints(stat, amount);
        }
        private void UpgradeStatWheel(UIScrollWheelEvent evt, UIElement listeningElement, Stat stat)
        {
            if (!visible)
                return;
            int amount = 0;
            if (evt.ScrollWheelValue > 0)
                amount = 20;
            if (evt.ScrollWheelValue < 0)
                amount = 150;
            Main.PlaySound(SoundID.MenuOpen);
            if (Main.keyState.PressingShift())
            {
                if (Main.keyState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.LeftControl))
                {
                    while (Char.FreePtns > 0)
                        Char.SpendPoints(stat, Char.GetStatXPMax(stat) - Char.GetStatXP(stat));
                    return;
                }

                for (int i = 0; i < amount; i++)
                    Char.SpendPoints(stat, Char.GetStatXPMax(stat) - Char.GetStatXP(stat));
                return;
            }
            Char.SpendPoints(stat, amount);
        }

        public override void Update(GameTime gameTime)
        {
            if (visible)
                UpdateStats();
            Recalculate();
            base.Update(gameTime);
        }

        void UpdateStats()
        {
            float statprogresscolor = 0;
            for (int i = 0; i < 8; i++)
            {
                string pre = "";
                if (Char.GetStatImproved((Stat)i) > Char.GetStat((Stat)i))
                    pre = "+";
                UpgradeStatText[i].SetText((Stat)i + " : " + Char.GetNaturalStat((Stat)i) + " + " + Char.GetAddStat((Stat)i) + " (" + Char.GetStat((Stat)i) + ")");
                statprogresscolor = Char.GetStatXP((Stat)i) / (float)Char.GetStatXPMax((Stat)i);
                StatProgress[i].TextColor = new Color(127, (int)(280 * statprogresscolor), (int)(243 * statprogresscolor));
                StatProgress[i].SetText(Char.GetStatXP((Stat)i) + " / " + Char.GetStatXPMax((Stat)i));
                progressStatsBar[i].color = new Color((int)(200 * (1 - statprogresscolor)), (int)(280 * statprogresscolor), (int)(130 * statprogresscolor) + 50, 1); ;
            }
            /*for (int i = 0; i < 5; i++)
            {
                UpgradeStatDetails[i + 3].SetText((DamageType)i + " Damage Multiplier : " + Math.Round(Char.GetDamageMult((DamageType)i), 2) + " x " + Math.Round(Char.GetDamageMult((DamageType)i, 1), 2) + " = " + Math.Round(Char.GetDamageMult((DamageType)i, 2) * 100, 2) + " %");
            }*/
            UpgradeStatDetails[0].SetText("气血 : " + Char.player.statLifeMax2 + " ( " + Char.player.statLifeMax / 20 + " 心 x " + Math.Round(Char.GetHealthMult(), 2) + " x " + Math.Round(Char.GetHealthPerHeart(), 2) + " 血/心 + "+ 10 * Char.GetLevel() + " )");
            UpgradeStatDetails[1].SetText("灵力 : " + Char.player.statManaMax2);
            UpgradeStatDetails[2].SetText("防御 : " + Char.player.statDefense + " ( " + Char.BaseArmor + " 护甲 x " + Math.Round(Char.GetDefenceMult(), 2) + " x " + Math.Round(Char.GetArmorMult(), 2) + " 防御/护甲 + 0 )");
            UpgradeStatDetails[3].SetText("灵攻 : " + Math.Round(Char.GetDamageMult((DamageType)1), 2) + " x " + Math.Round(Char.GetDamageMult((DamageType)1, 1), 2) + " = " + Math.Round(Char.GetDamageMult((DamageType)1, 2) * 100, 2) + " %");
            UpgradeStatDetails[4].SetText("购买价格 : " + "100倍");
            UpgradeStatDetails[5].SetText("物品掉落 : " + "0.1倍");
            UpgradeStatDetails[6].SetText("修炼加速 : " + (10 + Char.GetStat(Stat.功法) * 0.01) + "倍");
            UpgradeStatDetails[7].SetText("灵技加成 : " + "0.1倍");
            UpgradeStatDetails[8].SetText("暴击几率 : + " + Math.Round(Char.GetCriticalChanceBonus(), 2) + "%");
            UpgradeStatDetails[9].SetText("暴击伤害 : " + Math.Round(Char.GetCriticalDamage() * 100, 2) + "%");
            UpgradeStatDetails[10].SetText("气血回复 : +" + Math.Round((double)Char.player.lifeRegen, 2) + "/s");
            UpgradeStatDetails[11].SetText("灵力回复 : +" + (1 + Char.GetStat(Stat.功法) * 0.1) + " x " + Char.GetStat(Stat.灵根) * 0.05 + "=" + Char.GetLinliReply() + "/s");
            PointsLeft.SetText("道源 : " + Char.FreePtns + " / " + Char.TotalPtns, 1, true);

        }

        Vector2 offset;
        public bool dragging = false;
        private void DragStart(UIMouseEvent evt, UIElement listeningElement)
        {
            if (!visible)
                return;
            offset = new Vector2(evt.MousePosition.X - statsPanel.Left.Pixels, evt.MousePosition.Y - statsPanel.Top.Pixels);
            dragging = true;
        }

        private void DragEnd(UIMouseEvent evt, UIElement listeningElement)
        {
            if (!visible)
                return;
            Vector2 end = evt.MousePosition;
            dragging = false;

            statsPanel.Left.Set(end.X - offset.X, 0f);
            statsPanel.Top.Set(end.Y - offset.Y, 0f);

            Recalculate();
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            Vector2 MousePosition = new Vector2(Main.mouseX, Main.mouseY);
            if (statsPanel.ContainsPoint(MousePosition))
            {
                Main.LocalPlayer.mouseInterface = true;
            }
            if (dragging)
            {
                statsPanel.Left.Set(MousePosition.X - offset.X, 0f);
                statsPanel.Top.Set(MousePosition.Y - offset.Y, 0f);
                Recalculate();
            }
        }
    }

    class StatProgress : UIElement
    {
        private Texture2D _texture;
        public float ImageScale = 1f;
        public Color color;

        private Stat stat;
        public float width;
        public float left;

        public StatProgress(Stat stat, Texture2D texture)
        {
            _texture = texture;
            Width.Set(_texture.Width, 0f);
            Height.Set(_texture.Height, 0f);
            width = _texture.Width;
            Left.Set(0, 0f);
            Top.Set(0, 0f);
            color = Color.White;
            this.stat = stat;
        }

        public void SetImage(Texture2D texture)
        {
            _texture = texture;
            Width.Set(_texture.Width, 0f);
            Height.Set(_texture.Height, 0f);


        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            RPGPlayer player = Main.player[Main.myPlayer].GetModPlayer<RPGPlayer>();
            float quotient = 1f;
            //Calculate quotient


            quotient = player.GetStatXP(stat) / (float)player.GetStatXPMax(stat);

            Width.Set(quotient * width, 0f);
            //Left.Set((1 - quotient) * width, 0);
            Recalculate(); // recalculate the position and size

            base.Draw(spriteBatch);
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            CalculatedStyle dimensions = GetDimensions();
            Point point1 = new Point((int)dimensions.X, (int)dimensions.Y);
            int width = (int)Math.Ceiling(dimensions.Width);
            int height = (int)Math.Ceiling(dimensions.Height);

            spriteBatch.Draw(_texture, dimensions.Position() + _texture.Size() * (1f - ImageScale) / 2f, new Rectangle(point1.X, point1.Y, width, height), color, 0f, Vector2.Zero, ImageScale, SpriteEffects.None, 0f);
        }
    }

    class ProgressBG : UIElement
    {
        private Texture2D _texture;
        public float ImageScale = 1f;
        public Color color;

        public ProgressBG(Texture2D texture)
        {
            _texture = texture;
            Width.Set(_texture.Width, 0f);
            Height.Set(_texture.Height, 0f);
            color = Color.White;
        }


        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            CalculatedStyle dimensions = GetDimensions();
            Point point1 = new Point((int)dimensions.X, (int)dimensions.Y);
            int width = (int)Math.Ceiling(dimensions.Width);
            int height = (int)Math.Ceiling(dimensions.Height);

            spriteBatch.Draw(_texture, dimensions.Position() + _texture.Size() * (1f - ImageScale) / 2f, new Rectangle(point1.X, point1.Y, width, height), color, 0f, Vector2.Zero, ImageScale, SpriteEffects.None, 0f);
        }
    }


    class OpenStatButton : UIElement
    {
        private Texture2D _texture;
        public float ImageScale = 1f;

        public OpenStatButton(Texture2D texture)
        {
            _texture = texture;
            Width.Set(_texture.Width, 0f);
            Height.Set(_texture.Height, 0f);
            Left.Set(0, 0f);
            Top.Set(0, 0f);
            VAlign = 0;
            HAlign = 0;
        }

        public void SetImage(Texture2D texture)
        {
            _texture = texture;
            Width.Set(_texture.Width, 0f);
            Height.Set(_texture.Height, 0f);
        }


        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            CalculatedStyle dimensions = GetDimensions();

            spriteBatch.Draw(_texture, dimensions.Position(), null, Color.White, 0f, Vector2.Zero, ImageScale, SpriteEffects.None, 0f);
        }
    }

}
