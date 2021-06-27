using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SummonHeart.body;
using SummonHeart.costvalues;
using SummonHeart.Extensions;
using SummonHeart.ui.layout;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ModLoader;
using Terraria.UI;

namespace SummonHeart.ui
{
    class Panel : UIState
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
            panel.panelHeight = (int)(Main.screenHeight * 0.7);

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

                var modlabel = new UIText("魔神炼体法-魔神传承自动领悟：魔神自创的魔神级炼体功法，不断突破极限直达炼体第七境。");
                modlabel.TextColor = new Color(232, 181, 16);
                modbuffpanel.children.Add(new LayoutElementWrapperUIElement(modlabel));
                modlabel = new UIText("崩碎身躯把灵魂之力炼化溶于肉身，大幅提升肉身极限，使得肉身拥有吞血肉凝练的逆天之力。");
                modlabel.TextColor = new Color(232, 181, 16);
                modbuffpanel.children.Add(new LayoutElementWrapperUIElement(modlabel));
                modlabel = new UIText("炼体七境：淬体入门、炼皮成牛、炼骨如钢、练髓如汞、金刚之躯、肉身圆满、不死之身七大境界");
                modlabel.TextColor = new Color(232, 181, 16);
                modbuffpanel.children.Add(new LayoutElementWrapperUIElement(modlabel));

                string level = "凡体【无等阶】";
                int totalBoodGas = mp.eyeBloodGas + mp.handBloodGas + mp.bodyBloodGas + mp.footBloodGas;
                if (totalBoodGas >= 100)
                    level = "炼体第一境【淬体入门】";
                if (totalBoodGas >= 5000)
                    level = "炼体第二境【炼皮成牛】";
                if (totalBoodGas >= 10000)
                    level = "炼体第三境【炼骨如刚】";
                if (totalBoodGas >= 50000)
                    level = "炼体第四境【练髓如汞】";
                if (totalBoodGas >= 100000)
                    level = "炼体第五境【金刚之躯】";
                if (totalBoodGas >= 200000)
                    level = "炼体第六境【肉身圆满】";
                if (totalBoodGas >= 400000)
                    level = "炼体第七境【不死之身】";
                var modlabel_level = new UIText("当前炼体境界：" + level + " 总气血：" + totalBoodGas + " 战斗力：" + mp.getPower());
                modlabel_level.TextColor = Color.Red;
                modbuffpanel.children.Add(new LayoutElementWrapperUIElement(modlabel_level));
                var modlabel_max = new UIText("当前肉身极限：" + mp.bloodGasMax + "(与战力大于你肉身极限的斩命1重或以上强敌战斗可突破极限)");
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
                if (mp.boughtbuffList[0])
                    modlabel_level = new UIText("命运之路，勘破妄虚：召唤暴击+" + (mp.eyeBloodGas / 1500 + 20)+ "%(初始20%，每1500气血+1)");
                else
                    modlabel_level = new UIText("命运之路，勘破妄虚：召唤暴击+" + mp.eyeBloodGas / 1500 + "%(初始20%，每1500气血+1)");
                modlabel_level.TextColor = Color.SkyBlue;
                modbuffpanel.children.Add(new LayoutElementWrapperUIElement(modlabel_level));
                modlabel_level = new UIText("生死轮转，皆入吾眼：暴击伤害+" + mp.eyeBloodGas / 1000 + "%(每1000气血+1)");
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
                if (mp.boughtbuffList[1])
                    modlabel_level = new UIText("掌凝万灵肇天撼地：攻击速度+" + (mp.handBloodGas / 1000 + 30) + "%(初始30%，每1000气血+1) 攻击范围+" + (mp.handBloodGas / 500 + 100) + "%(初始100%，每500气血+1)");
                else
                    modlabel_level = new UIText("掌凝万灵肇天撼地：攻击速度+" + mp.handBloodGas / 1000 + "%(初始30%，每1000气血+1) 攻击范围+" + (mp.handBloodGas / 500) + "%(初始100%，每500气血+1)");
                modlabel_level.TextColor = Color.SkyBlue;
                modbuffpanel.children.Add(new LayoutElementWrapperUIElement(modlabel_level));
                modlabel_level = new UIText("指天天裂划地地崩：所有攻击+" + mp.handBloodGas / 200 + "%(每200气血+1)");
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
                modlabel_level = new UIText("万灵炼体，不死不灭：生命上限+" + mp.bodyBloodGas / 100 + "(每100气血+1) 每秒回复自身" + (mp.handBloodGas / 10000 + 2) + "%生命(初始2%，每10000气血+1)");
                modlabel_level.TextColor = Color.SkyBlue;
                modbuffpanel.children.Add(new LayoutElementWrapperUIElement(modlabel_level));
                modlabel_level = new UIText("干凝万锻，魔体終成：防御+" + mp.bodyDef + " 反伤+" + mp.bodyDef);
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
                modlabel_level = new UIText("风驰电掣，雷厉风行：跳跃速度+" + (mp.footBloodGas / 500 + 100) + "%(初始100%，每400气血+1)");
                modlabel_level.TextColor = Color.SkyBlue;
                modbuffpanel.children.Add(new LayoutElementWrapperUIElement(modlabel_level));
                modlabel_level = new UIText("天涯海角，一步跨之：飞行时间+" + (mp.footBloodGas/1000 + 10) + "秒(初始10，每1000气血+1，修炼圆满时无限飞行)");
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
                                        mp.bodyDef += buff.def;
                                        needValidate = true;
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

            buffIndex = 0;
            {
                var modbuffpanel = new Layout(10, 0, 0, 0, 10, new LayoutVertical());

                var modlabel = new UIText("干凝万锻，魔体終成：可消耗灵魂之力用外物淬体，整体提升肉身强度");
                modlabel.TextColor = new Color(232, 181, 16);
                modbuffpanel.children.Add(new LayoutElementWrapperUIElement(modlabel));

                var modbuffgridpanel = new Layout(0, 0, 0, 0, 10, buffGrid);
                modbuffpanel.children.Add(modbuffgridpanel);

                //populate modbuffgridpanel

                foreach (var buffValue in SummonHeartMod.modBuffValues)
                {
                    var currentBuffIndex = buffIndex;
                    buffIndex += 1;
                    if (currentBuffIndex <= 4)
                        continue;

                    var buffpanel = new Layout(0, 0, 0, 0, 10, new LayoutVertical());

                    var buff = SummonHeartMod.getBuff(currentBuffIndex);

                    buff.texture = Main.itemTexture[buff.id];

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
                                    if (!mp.boughtbuffList[2])
                                    {
                                        Main.NewText("你还没有修炼魔神之躯，无法炼化外物淬体，请先修炼魔神之躯!", new Color(255, 0, 0));
                                        return;
                                    }
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
                                        mp.bodyDef += buff.def;
                                        needValidate = true;
                                    }
                                    else
                                    {
                                        Main.NewText("你没有足够的材料炼化!", new Color(255, 0, 0));
                                    }
                                }
                            };

                            ownedImages.OnMouseOver += delegate (UIMouseEvent evt, UIElement listeningElement)
                            {
                                var tip = "修炼魔神淬体法炼化";
                                TooltipPanel.Instance.SetInfo(buff.cost, buff, buff.name, buff.effect, buff.texture, tip);
                            };
                        }
                        else
                        {

                            var toggleButtons = new UIHoverImageToggleButton(buttonPlayTexture1, buttonPlayTexture2, "Disable buff " + buff.name, "Use buff " + buff.name, false);

                            toggleButtons.SetImage(buttonPlayTexture1);

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
