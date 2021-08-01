using Microsoft.Xna.Framework;
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
    class PanelSummon : UIState
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

                var modlabel = new UIText("魔神练灵诀-魔神召唤传承：凝魂躯，斩化身，练灵眼，控万灵。");
                modlabel.TextColor = new Color(232, 181, 16);
                modbuffpanel.children.Add(new LayoutElementWrapperUIElement(modlabel));
                modlabel = new UIText("踏足灵魂之道，深掘灵魂之力。凝魂躯，介于虚实之间。练灵眼，可控万物");
                modlabel.TextColor = new Color(232, 181, 16);
                modbuffpanel.children.Add(new LayoutElementWrapperUIElement(modlabel));
                modlabel = new UIText("有道是灵魂之路道法妙，魂躯初凝挡万劫，神魂之眸慑八荒，掌控万灵夷四方！");
                modlabel.TextColor = new Color(232, 181, 16);
                modbuffpanel.children.Add(new LayoutElementWrapperUIElement(modlabel));

                int totalBoodGas = mp.eyeBloodGas + mp.handBloodGas + mp.bodyBloodGas + mp.footBloodGas;
               
                var modlabel_level = new UIText("当前召唤栏：" + mp.player.maxMinions + " 总气血：" + totalBoodGas + " 死亡次数：" + mp.deathCount + " 战斗力：" + mp.getPower());
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

                if (mp.bloodGasMax < SummonHeartWorld.PlayerBloodGasMax && SummonHeartWorld.PlayerBloodGasMax <= 30000)
                    mp.bloodGasMax = SummonHeartWorld.PlayerBloodGasMax;
                modlabel_max = new UIText("当前肉身极限：" + mp.bloodGasMax + "(与战力大于你肉身极限的斩命1重或以上强敌战斗可突破极限)");
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
                modlabel_level = new UIText("神魂之眸" + praticeText + " 气血值：" + mp.eyeBloodGas);
                modlabel_level.TextColor = Color.Orange;
                modbuffpanel.children.Add(new LayoutElementWrapperUIElement(modlabel_level));
                modlabel_level = new UIText("环视八方：神魂之眸视线范围+" + (mp.eyeBloodGas / 400 + 100)+ "格(初始100，每400气血+1)");
                modlabel_level.TextColor = Color.SkyBlue;
                modbuffpanel.children.Add(new LayoutElementWrapperUIElement(modlabel_level));
                modlabel_level = new UIText("死亡之眼：视线范围内的敌人移动速度和弹幕飞行速度降低" + (mp.eyeBloodGas / 5000 + 10) + "%(初始10%，每5000气血+1)");
                modlabel_level.TextColor = Color.SkyBlue;
                modbuffpanel.children.Add(new LayoutElementWrapperUIElement(modlabel_level));
                modlabel_level = new UIText("震慑神魂：视线范围内的敌人每秒受到10次灵魂攻击，每次攻击造成" + (mp.eyeBloodGas / 1000 + 1) + "点真实伤害(初始1，每1000气血+1)");
                modlabel_level.TextColor = Color.SkyBlue;
                modbuffpanel.children.Add(new LayoutElementWrapperUIElement(modlabel_level));
                modlabel_level = new UIText("灵魂颤栗：视线范围内的敌人受到攻击必定暴击");
                modlabel_level.TextColor = Color.SkyBlue;
                modbuffpanel.children.Add(new LayoutElementWrapperUIElement(modlabel_level));
                if (!mp.boughtbuffList[1])
                    praticeText = "【未修炼】";
                else if (mp.practiceHand)
                    praticeText = "【修炼中】";
                else
                    praticeText = "【暂停修炼】";
                modlabel_level = new UIText("神魂之手"+ praticeText + " 气血值：" + mp.handBloodGas);
                modlabel_level.TextColor = Color.Orange;
                modbuffpanel.children.Add(new LayoutElementWrapperUIElement(modlabel_level));
                modlabel_level = new UIText("灵手附身：灵手分裂附身召唤物，召唤物真实伤害+" + (mp.handBloodGas / 2000 + 10) + "点(初始10，每2000气血+1)");
                modlabel_level.TextColor = Color.SkyBlue;
                modbuffpanel.children.Add(new LayoutElementWrapperUIElement(modlabel_level));
                modlabel_level = new UIText("万灵狂暴：以灵魂之力强化召唤物，召唤武器基础攻击+" + (mp.handBloodGas / 200) + "%(每200气血+1%) ");
                modlabel_level.TextColor = Color.SkyBlue;
                modbuffpanel.children.Add(new LayoutElementWrapperUIElement(modlabel_level));

                if (!mp.boughtbuffList[2])
                    praticeText = "【未修炼】";
                else if (mp.practiceBody)
                    praticeText = "【修炼中】";
                else
                    praticeText = "【暂停修炼】";
                modlabel_level = new UIText("神魂之躯" + praticeText + " 气血值：" + mp.bodyBloodGas);
                modlabel_level.TextColor = Color.Orange;
                modbuffpanel.children.Add(new LayoutElementWrapperUIElement(modlabel_level));
                modlabel_level = new UIText("化身初斩，魂躯初凝：修炼需斩第一身外化身，血量永久减半。魂躯介于虚实之间，免疫96%物理接触伤害");
                modlabel_level.TextColor = Color.SkyBlue;
                modbuffpanel.children.Add(new LayoutElementWrapperUIElement(modlabel_level));
                modlabel_level = new UIText("化身初斩，魂躯初凝：弹幕伤害减免+" + (mp.bodyBloodGas / 5000 + 20) + "%(初始20%，每5000+1)");
                modlabel_level.TextColor = Color.SkyBlue;
                modbuffpanel.children.Add(new LayoutElementWrapperUIElement(modlabel_level));
                modlabel_level = new UIText("近乎灵体，形若鬼魅：移动速度+66%，免疫摔落伤害");
                modlabel_level.TextColor = Color.SkyBlue;
                modbuffpanel.children.Add(new LayoutElementWrapperUIElement(modlabel_level));
                modlabel_level = new UIText("掌控万灵，魔力亲和：召唤栏位+" + (mp.bodyBloodGas / 10000 + 3) + "（初始3，每10000气血+1）最大法力值+" + (mp.bodyBloodGas / 200 + 200) + "(初始200，每200+1)");
                modlabel_level.TextColor = Color.SkyBlue;
                modbuffpanel.children.Add(new LayoutElementWrapperUIElement(modlabel_level));

                if (!mp.boughtbuffList[3])
                    praticeText = "【未修炼】";
                else if (mp.practiceFoot)
                    praticeText = "【修炼中】";
                else
                    praticeText = "【暂停修炼】";
                modlabel_level = new UIText("神魂之腿" + praticeText + " 气血值：" + mp.footBloodGas);
                modlabel_level.TextColor = Color.Orange;
                modbuffpanel.children.Add(new LayoutElementWrapperUIElement(modlabel_level));
                modlabel_level = new UIText("灵魂漫步：跳跃速度+" + (mp.footBloodGas / 500 + 100) + "%(初始100%，每400气血+1)");
                modlabel_level.TextColor = Color.SkyBlue;
                modbuffpanel.children.Add(new LayoutElementWrapperUIElement(modlabel_level));
                modlabel_level = new UIText("天涯海角，一步跨之：飞行时间+" + (mp.footBloodGas/1000 + 10) + "秒(初始10，每1000气血+1)");
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
                                        if(currentBuffIndex == 2)
                                        {
                                            Item.NewItem(mp.player.Center, ModContent.ItemType<SoulCrystal>());
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
