using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SummonHeart.body;
using SummonHeart.costvalues;
using SummonHeart.Extensions;
using SummonHeart.Items.Material;
using SummonHeart.RPGModule.Entities;
using SummonHeart.RPGModule.Enum;
using SummonHeart.ui.layout;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;

namespace SummonHeart.ui
{
    class PanelXiuXian : UIState
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
            panel.panelHeight = (int)(Main.screenHeight * 0.80);

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
            var mp = Main.player[Main.myPlayer].GetModPlayer<RPGPlayer>();
            panelwrapper.children.Clear();

            int buffIndex = 0;
            {
                var modbuffpanel = new Layout(10, 0, 0, 0, 10, new LayoutVertical());

                var modlabel_max = new UIText("姓名：" + mp.player.name);
                modlabel_max.TextColor = Color.LightBlue;
                modbuffpanel.children.Add(new LayoutElementWrapperUIElement(modlabel_max));

                modlabel_max = new UIText("身份：高维宇宙仙道势力入侵者");
                modlabel_max.TextColor = Color.Red;
                modbuffpanel.children.Add(new LayoutElementWrapperUIElement(modlabel_max));

                modlabel_max = new UIText("境界：" + mp.GetLevelText());
                modlabel_max.TextColor = Color.Red;
                modbuffpanel.children.Add(new LayoutElementWrapperUIElement(modlabel_max));

                var modlabel = new UIText("年龄：" + mp.GetAgeText());
                modlabel.TextColor = Color.LightBlue;
                modbuffpanel.children.Add(new LayoutElementWrapperUIElement(modlabel));

                modlabel = new UIText("剩余寿元："+mp.GetLifeText()+"『随时间流逝减少，破境时增加寿命』");
                modbuffpanel.children.Add(new LayoutElementWrapperUIElement(modlabel));

                modlabel_max = new UIText("灵根：" + mp.GetStat(Stat.灵根));
                modlabel_max.TextColor = Color.Magenta;
                modbuffpanel.children.Add(new LayoutElementWrapperUIElement(modlabel_max));

                modlabel = new UIText("悟性：" + mp.GetStat(Stat.灵根));
                modlabel.TextColor = Color.Magenta;
                modbuffpanel.children.Add(new LayoutElementWrapperUIElement(modlabel));

                modlabel = new UIText("魅力：" + mp.GetStat(Stat.灵根));
                modlabel.TextColor = Color.Magenta;
                modbuffpanel.children.Add(new LayoutElementWrapperUIElement(modlabel));

                modlabel = new UIText("气运：" + mp.GetStat(Stat.灵根));
                modlabel.TextColor = Color.Magenta;
                modbuffpanel.children.Add(new LayoutElementWrapperUIElement(modlabel));

                modlabel = new UIText("道心：" + mp.GetStat(Stat.道心) + "咸鱼『肝度和难度，道心决定了道源转换倍率，道心越高转换的道源越多，最高提升10倍』");
                modlabel.TextColor = Color.Magenta;
                modbuffpanel.children.Add(new LayoutElementWrapperUIElement(modlabel));
              
                modlabel = new UIText("气血：" + mp.player.statLifeMax2);
                modbuffpanel.children.Add(new LayoutElementWrapperUIElement(modlabel));

                modlabel = new UIText("灵力：" + mp.GetLinliMax());
                modbuffpanel.children.Add(new LayoutElementWrapperUIElement(modlabel));

                modlabel = new UIText("灵防：1");
                modbuffpanel.children.Add(new LayoutElementWrapperUIElement(modlabel));


                var modlabel_level = new UIText("所修功法：轮回仙经 当前等阶：黄阶下品");
                modlabel_level.TextColor = Color.Red;
                modbuffpanel.children.Add(new LayoutElementWrapperUIElement(modlabel_level));

                modlabel = new UIText("功法介绍：原本是由超越究极层次的至强道境因果大道所创，在你降维入侵泰拉世界时");
                modlabel.TextColor = new Color(232, 181, 16);
                modbuffpanel.children.Add(new LayoutElementWrapperUIElement(modlabel));
                modlabel = new UIText("受到泰拉世界意志的疯狂攻击从而失去绝大部分力量，因果大道不得不降阶到轮回大道，并且你的功法受");
                modlabel.TextColor = new Color(232, 181, 16);
                modbuffpanel.children.Add(new LayoutElementWrapperUIElement(modlabel));
                modlabel = new UIText("到了泰拉世界意志的疯狂压制，现在只能发挥出黄阶下品功法的威力，但依然保持了轮回之道的部分力量。");
                modlabel.TextColor = new Color(232, 181, 16);
                modbuffpanel.children.Add(new LayoutElementWrapperUIElement(modlabel));
                

                modlabel_level = new UIText("战斗灵力回复：1/s 修炼状态加速：10倍加速 升级所需道源：1");
                modlabel_level.TextColor = Color.SkyBlue;
                modbuffpanel.children.Add(new LayoutElementWrapperUIElement(modlabel_level));

               /* modlabel = new UIText("功法力量掌控境界：力境Ⅰ");
                modlabel.TextColor = new Color(232, 181, 16);
                modbuffpanel.children.Add(new LayoutElementWrapperUIElement(modlabel));

                modlabel = new UIText("功法气血加成：1.2倍 灵攻加成：1.2倍 灵防加成：1.2倍");
                modlabel.TextColor = new Color(232, 181, 16);
                modbuffpanel.children.Add(new LayoutElementWrapperUIElement(modlabel));*/

                modlabel = new UIText("功法特殊能力：生死轮回、九转道源、因果逆转·燃元秘术");
                modlabel.TextColor = new Color(232, 181, 16);
                modbuffpanel.children.Add(new LayoutElementWrapperUIElement(modlabel));

                modlabel = new UIText("生死轮回，不死之身：当玩家气血归0死亡时，轮回仙经会逆转轮回之道，玩家1年后会复活，消耗玩家1年寿元。");
                modlabel.TextColor = new Color(232, 181, 16);
                modbuffpanel.children.Add(new LayoutElementWrapperUIElement(modlabel));

                modlabel = new UIText("如果寿元归0而死，轮回之道全开，轮回仙经无力逆转轮回，只能舍弃掉95%的道源和全部肉身，保留5%道源和灵魂记忆不灭，走轮回之道轮回。");
                modlabel.TextColor = new Color(232, 181, 16);
                modbuffpanel.children.Add(new LayoutElementWrapperUIElement(modlabel));

                modlabel = new UIText("九转道源：同一个境界可以轮回修炼9次，夺天地造化提炼道源，道源可逆天改命提升你的天资。");
                modlabel.TextColor = new Color(232, 181, 16);
                modbuffpanel.children.Add(new LayoutElementWrapperUIElement(modlabel));

                modlabel = new UIText("因果逆转·燃元秘术：开启时提升10倍修炼速度，寿元消耗提升20倍，额外寿元消耗100%");
                modlabel.TextColor = new Color(232, 181, 16);
                modbuffpanel.children.Add(new LayoutElementWrapperUIElement(modlabel));



                var modbuffgridpanel = new Layout(0, 0, 0, 0, 10, buffGrid);
                modbuffpanel.children.Add(modbuffgridpanel);
                panelwrapper.children.Add(modbuffpanel);
            }

           /* buffIndex = 0;
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
            }*/

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
