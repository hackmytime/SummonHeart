﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SummonHeart.body;
using SummonHeart.costvalues;
using SummonHeart.Extensions;
using SummonHeart.Items.Material;
using SummonHeart.ui.layout;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;

namespace SummonHeart.ui
{
    class PanelKill : UIState
    {
        public static bool visible = false;
        public UIScrollPanel panel;

        public LayoutGrid buffGrid = new LayoutGrid(10);

        LayoutWrapperUIElement panelwrapper;

        Texture2D buttonPlayTexture1;
        Texture2D buttonPlayTexture2;

        List<Texture2D> demonTextureList = new List<Texture2D>();

        bool created = false;
        public override void OnInitialize()
        {


        }

        public bool needValidate = false;

        public void create()
        {
            created = true;
            TooltipPanel.Instance = new TooltipPanel();
            TooltipPanel.Instance.Init();


            // if you set this to true, it will show up in game
            //visible = false;

            buttonPlayTexture1 = ModContent.GetTexture("SummonHeart/ui/checkbox");
            buttonPlayTexture2 = ModContent.GetTexture("SummonHeart/ui/checkboxunchecked");

            demonTextureList.Add(ModContent.GetTexture("SummonHeart/Buffs/DemonEye"));
            demonTextureList.Add(ModContent.GetTexture("SummonHeart/Buffs/DemonHand"));
            demonTextureList.Add(ModContent.GetTexture("SummonHeart/Buffs/DemonBody"));
            demonTextureList.Add(ModContent.GetTexture("SummonHeart/Buffs/DemonFoot"));
            demonTextureList.Add(ModContent.GetTexture("SummonHeart/Buffs/SoulSplit"));

            panel = new UIScrollPanel(); //initialize the panel
                                         // ignore these extra 0s
            Append(panel);

            panelwrapper = new LayoutWrapperUIElement(panel, 0, 0, 0, 0, 10, new LayoutVertical());

            updateSize();

            Main.OnResolutionChanged += delegate (Vector2 newSize)
            {
                updateSize();
            };
        }

        public void updateSize()
        {
            int maxWidth = (int)(Main.screenWidth / 1.8);
            int columnCount = maxWidth / (32 + 10);
            maxWidth = columnCount * (32 + 10);
            panel.panelWidth = maxWidth;
            panel.panelHeight = (int)(Main.screenHeight * 0.83);

            buffGrid.SetColumnCount(columnCount);

            panel.Left.Set(Main.screenWidth / 2 - panel.panelWidth / 2, 0); //this makes the distance between the left of the screen and the left of the panel 500 pixels (somewhere by the middle)
            panel.Top.Set((float)(Main.screenHeight - panel.panelHeight) / 2, 0); //this is the distance between the top of the screen and the top of the panel


            Revalidate();
        }

        public void Revalidate()
        {
            needValidate = false;
            var buffSize = SummonHeartMod.getBuffLength();

            var unownedTexture = ModContent.GetTexture("SummonHeart/ui/unowned");
            var mp = Main.player[Main.myPlayer].GetModPlayer<SummonHeartPlayer>();
            panelwrapper.children.Clear();

            int buffIndex = 0;
            {
                var modbuffpanel = new Layout(10, 0, 0, 0, 10, new LayoutVertical());

                var modlabel = new UIText("弑灵戮神陨-魔神刺客传承：最强爆发职业，刺客之道，向死而生。死气护体，不死不休。");
                modlabel.TextColor = new Color(232, 181, 16);
                modbuffpanel.children.Add(new LayoutElementWrapperUIElement(modlabel));


                int totalBoodGas = mp.eyeBloodGas + mp.handBloodGas + mp.bodyBloodGas + mp.footBloodGas;
                var modlabel_level = new UIText("刺客被动：杀戮法则，每有1个生物死亡+1杀意值上限【上限999999】【可以被贪婪词缀加速】");
                modlabel_level.TextColor = Color.SkyBlue;
                modbuffpanel.children.Add(new LayoutElementWrapperUIElement(modlabel_level));
                modlabel_level = new UIText("每44444点杀意值上限+1倍刺杀技能伤害，最高22倍。每100点杀意值+1点额外真实伤害，最大值为9999");
                modlabel_level.TextColor = Color.SkyBlue;
                modbuffpanel.children.Add(new LayoutElementWrapperUIElement(modlabel_level));
                modlabel_level = new UIText("刺客被动：死气护体，不死不休，每100点杀意值上限转化为1点死气值上限【上限9999】");
                modlabel_level.TextColor = Color.SkyBlue;
                modbuffpanel.children.Add(new LayoutElementWrapperUIElement(modlabel_level));
                modlabel_level = new UIText("每消耗100点杀意增加1点死气值，每1点死气值可以抵挡1点伤害。");
                modlabel_level.TextColor = Color.SkyBlue;
                modbuffpanel.children.Add(new LayoutElementWrapperUIElement(modlabel_level));
                modlabel_level = new UIText("当前杀意上限：" + mp.killResourceMax2 + " 总气血：" + totalBoodGas + " 击杀生物数：" + mp.killNpcCount + " 死亡次数：" + mp.deathCount + " 战斗力：" + mp.getPower());
                modlabel_level.TextColor = Color.Red;
                modbuffpanel.children.Add(new LayoutElementWrapperUIElement(modlabel_level));
                modlabel_level = new UIText("杀意伤害倍率：" + mp.killResourceMulti + " 神通附加伤害：" + mp.player.HeldItem.damage * mp.killResourceMulti + " 当前减伤倍率" + mp.myDamageReduceMult + "倍");
                modlabel_level.TextColor = Color.Red;
                modbuffpanel.children.Add(new LayoutElementWrapperUIElement(modlabel_level));

                string worldLevel = "Lv1魔神之子";
                if (SummonHeartWorld.WorldLevel == 2)
                    worldLevel = "Lv2魔神之路";
                if (SummonHeartWorld.WorldLevel == 3)
                    worldLevel = "Lv3弑神屠魔";
                if (SummonHeartWorld.WorldLevel == 4)
                    worldLevel = "Lv4逆天而行";
                if (SummonHeartWorld.WorldLevel == 5)
                    worldLevel = "Lv5？？？？";
                var modlabel_max = new UIText("世界难度："+worldLevel+" 世界炼体气血上限：" + SummonHeartWorld.WorldBloodGasMax + "(由世界难度所决定)");
                modlabel_max.TextColor = Color.Red;
                modbuffpanel.children.Add(new LayoutElementWrapperUIElement(modlabel_max));

                modlabel_max = new UIText("当前单部位肉身极限：" + mp.bloodGasMax + "(击败原版特定boss可突破极限)");
                modlabel_max.TextColor = Color.Magenta;
                modbuffpanel.children.Add(new LayoutElementWrapperUIElement(modlabel_max));

                int soulCount = 0;
                if (mp.practiceEye)
                    soulCount++;
                if (mp.practiceHand)
                    soulCount++;
                if (mp.practiceBody)
                    soulCount++;
                if (mp.practiceFoot)
                    soulCount++;
                modlabel_level = new UIText("炼体灵魂消耗：" + soulCount + "倍吞噬气血值 说明：修炼魔神之躯吞噬气血时会消耗等量灵魂之力");
                modlabel_level.TextColor = Color.Magenta;
                modbuffpanel.children.Add(new LayoutElementWrapperUIElement(modlabel_level));
                var praticeText = "";

                if (!mp.boughtbuffList[0])
                    praticeText = "【未修炼】";
                else if (mp.practiceEye)
                    praticeText = "【修炼中】";
                else
                    praticeText = "【暂停修炼】";
                modlabel_level = new UIText("魔神之眼" + praticeText + " 气血值：" + mp.eyeBloodGas);
                modlabel_level.TextColor = Color.Orange;
                modbuffpanel.children.Add(new LayoutElementWrapperUIElement(modlabel_level));
                modlabel_level = new UIText("神念摄魂心为眼：刺杀范围+" + (mp.eyeBloodGas / 800 + 30)+ "格(初始30，每800气血+1)");
                modlabel_level.TextColor = Color.SkyBlue;
                modbuffpanel.children.Add(new LayoutElementWrapperUIElement(modlabel_level));
                modlabel_level = new UIText("以杀入道，看破轮回：击杀敌人+" + (mp.eyeBloodGas / 400) + "点杀意值(每100000气血+1)");
                modlabel_level.TextColor = Color.SkyBlue;
                modbuffpanel.children.Add(new LayoutElementWrapperUIElement(modlabel_level));

                if (!mp.boughtbuffList[1])
                    praticeText = "【未修炼】";
                else if (mp.practiceHand)
                    praticeText = "【修炼中】";
                else
                    praticeText = "【暂停修炼】";
                modlabel_level = new UIText("魔神之手"+ praticeText + " 气血值：" + mp.handBloodGas);
                modlabel_level.TextColor = Color.Orange;
                modbuffpanel.children.Add(new LayoutElementWrapperUIElement(modlabel_level));
                modlabel_level = new UIText("凝练杀意，动若崩雷：刺杀技能储存上限：" + (mp.handBloodGas / 5000 + 10) + "(初始10，每5000气血+1)");
                modlabel_level.TextColor = Color.SkyBlue;
                modbuffpanel.children.Add(new LayoutElementWrapperUIElement(modlabel_level));
               /* modlabel_level = new UIText("凝练杀意，动若崩雷：刺杀技能伤害倍率+" + (mp.handBloodGas / 10000) + "倍(每10000+1)");
                modlabel_level.TextColor = Color.SkyBlue;
                modbuffpanel.children.Add(new LayoutElementWrapperUIElement(modlabel_level));*/
                modlabel_level = new UIText("刺客之手，投掷精通：投掷武器基础攻击+" + (mp.handBloodGas / 500 + 100) + "%(初始100%，每500气血+1%) 投掷速度+" + (mp.handBloodGas / 4000 + 30) + "%(初始30%，每4000气血+1%)");
                modlabel_level.TextColor = Color.SkyBlue;
                modbuffpanel.children.Add(new LayoutElementWrapperUIElement(modlabel_level));
                modlabel_level = new UIText("刺客之手，杀意灌注：投掷武器攻击消耗0.1%杀意上限的杀意值造成额外" + mp.killResourceMax2/100 + "点真实伤害");
                modlabel_level.TextColor = Color.SkyBlue;
                modbuffpanel.children.Add(new LayoutElementWrapperUIElement(modlabel_level));
               
                if (!mp.boughtbuffList[2])
                    praticeText = "【未修炼】";
                else if (mp.practiceBody)
                    praticeText = "【修炼中】";
                else
                    praticeText = "【暂停修炼】";
                modlabel_level = new UIText("魔神之躯" + praticeText + " 气血值：" + mp.bodyBloodGas);
                modlabel_level.TextColor = Color.Orange;
                modbuffpanel.children.Add(new LayoutElementWrapperUIElement(modlabel_level));
                modlabel_level = new UIText("万杀炼体，杀意滔天：每秒回复自身" + (mp.bodyBloodGas / 200 + 15) + "杀意值(初始15，每200+1)");
                modlabel_level.TextColor = Color.SkyBlue;
                modbuffpanel.children.Add(new LayoutElementWrapperUIElement(modlabel_level));
                modlabel_level = new UIText("万杀炼体，杀意滔天：每秒回复自身+" + (mp.eyeBloodGas / 50000 + 5) + "%杀意值(初始5%，每50000气血+1)");
                modlabel_level.TextColor = Color.SkyBlue;
                modbuffpanel.children.Add(new LayoutElementWrapperUIElement(modlabel_level));

                if (!mp.boughtbuffList[3])
                    praticeText = "【未修炼】";
                else if (mp.practiceFoot)
                    praticeText = "【修炼中】";
                else
                    praticeText = "【暂停修炼】";
                modlabel_level = new UIText("魔神之腿" + praticeText + " 气血值：" + mp.footBloodGas);
                modlabel_level.TextColor = Color.Orange;
                modbuffpanel.children.Add(new LayoutElementWrapperUIElement(modlabel_level));
                modlabel_level = new UIText("风驰电掣，雷厉风行：移动速度+" + (mp.footBloodGas / 5000 + 33) + "%(初始33%，每5000气血+1)");
                modlabel_level.TextColor = Color.SkyBlue;
                modbuffpanel.children.Add(new LayoutElementWrapperUIElement(modlabel_level));
                modlabel_level = new UIText("风驰电掣，雷厉风行：跳跃速度+" + (mp.footBloodGas / 500 + 100) + "%(初始100%，每500气血+1)免疫摔落伤害");
                modlabel_level.TextColor = Color.SkyBlue;
                modbuffpanel.children.Add(new LayoutElementWrapperUIElement(modlabel_level));
                modlabel_level = new UIText("天涯海角，一步跨之：飞行时间+" + (mp.footBloodGas/1000 + 5) + "秒(初始5，每1000气血+1)");
                modlabel_level.TextColor = Color.SkyBlue;
                modbuffpanel.children.Add(new LayoutElementWrapperUIElement(modlabel_level));

                var modbuffgridpanel = new Layout(0, 0, 0, 0, 10, buffGrid);
                modbuffpanel.children.Add(modbuffgridpanel);
                //populate modbuffgridpanel

                foreach (var buffValue in VanilaBuffs.getVanilla())
                {
                    var currentBuffIndex = buffIndex;
                    if (currentBuffIndex == 5)
                        break;
                    buffIndex += 1;

                    var buffpanel = new Layout(0, 0, 0, 0, 10, new LayoutVertical());

                    var buff = buffValue;
                    buff.texture = demonTextureList[currentBuffIndex];
                    {
                        LayoutWrapperUIElement lv = new LayoutWrapperUIElement(panel, 0, 0, 0, 0, 32, new LayoutVertical());
                        UIImage icon = new UIImage(buff.texture);
                        
                        int w = buff.texture.Width;
                        int h = buff.texture.Height;
                        lv.PaddingTop = (32 - h) / 2;
                        lv.PaddingLeft = (32 - w) / 2;

                        lv.children.Add(new LayoutElementWrapperUIElement(icon));
                        buffpanel.children.Add(lv);

                        if (!mp.boughtbuffList[currentBuffIndex])
                        {
                            var ownedImages = new UIImageButtonLabel(unownedTexture, "Buy buff " + buff.name);

                            buffpanel.children.Add(new LayoutElementWrapperUIElement(ownedImages));

                            ownedImages.OnClick += delegate (UIMouseEvent evt, UIElement listeningElement)
                            {
                                var tempBuff = SummonHeartMod.getBuff(currentBuffIndex);

                                if (!mp.boughtbuffList[currentBuffIndex])
                                {
                                    bool canbuy = true;
                                    foreach (var v in tempBuff.cost)
                                    {
                                        if (!v.CheckBuy())
                                        {
                                            canbuy = false;
                                            break;
                                        }
                                    }

                                    if (canbuy)
                                    {
                                        foreach (var v in tempBuff.cost)
                                        {
                                            v.Buy();
                                        }
                                        mp.boughtbuffList[currentBuffIndex] = true;
                                        if (currentBuffIndex == 3)
                                        {
                                            mp.player.QuickSpawnItem(ItemID.JimsWings, 1);
                                        }
                                        if (currentBuffIndex == 4)
                                        {
                                            mp.player.QuickSpawnItem(ModContent.ItemType<DemonBuff>(), 1);
                                        }
                                        mp.bodyDef += buff.def;
                                        needValidate = true;
                                        if (Main.netMode == NetmodeID.MultiplayerClient)
                                            mp.SendClientChanges(mp);
                                    }
                                    else
                                    {
                                        Main.NewText("你没有足够的灵魂之力和材料修炼!", new Color(255, 0, 0));
                                    }
                                }
                            };

                            ownedImages.OnMouseOver += delegate (UIMouseEvent evt, UIElement listeningElement)
                            {
                                var tip = "修炼魔神炼体法";
                                TooltipPanel.Instance.SetInfo(buff.cost, buff, buff.name, buff.effect, buff.texture, tip);
                            };
                        }
                        else
                        {
                            var toggleButtons = new UIHoverImageToggleButton(buttonPlayTexture1, buttonPlayTexture2, "Disable buff " + buff.name, "Use buff " + buff.name, true);

                            var praticeBool = mp.GetPratice(currentBuffIndex);
                            toggleButtons.IsChecked = praticeBool;
                            if (praticeBool)
                                toggleButtons.SetImage(buttonPlayTexture1);
                            else
                                toggleButtons.SetImage(buttonPlayTexture2);

                            toggleButtons.OnChecked += delegate (bool val)
                            {
                                mp.SetPratice(currentBuffIndex, val);
                                needValidate = true;
                            };

                            toggleButtons.OnMouseOver += delegate (UIMouseEvent evt, UIElement listeningElement)
                            {
                                TooltipPanel.Instance.SetInfo(buff, buff.name, buff.effect, buff.texture);
                            };

                            buffpanel.children.Add(new LayoutElementWrapperUIElement(toggleButtons));
                        }
                    }

                    modbuffgridpanel.children.Add(buffpanel);
                }

                panelwrapper.children.Add(modbuffpanel);
            }
            panelwrapper.Recalculate();
        }


        public override void Update(GameTime gameTime)
        {
            if (!created)
            {
                create();
            }
            base.Update(gameTime);
            TooltipPanel.Instance.Update(this);

            if (needValidate)
            {
                Revalidate();
            }
        }
    }
}
