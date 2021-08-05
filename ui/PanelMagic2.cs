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
    class PanelMagic2 : UIState
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

                var modlabel = new UIText("魔神淬法诀-魔神法师·控法者传承：魔神领悟道之本源用杀戮法则所创，掠夺万物法力练体");
                modlabel.TextColor = new Color(232, 181, 16);
                modbuffpanel.children.Add(new LayoutElementWrapperUIElement(modlabel));
                modlabel = new UIText("炼至巅峰，成就不灭法体。可充能叠加魔法，万法不侵");
                modlabel.TextColor = new Color(232, 181, 16);
                modbuffpanel.children.Add(new LayoutElementWrapperUIElement(modlabel));
                modlabel = new UIText("杀戮众生淬法体，月华灵眸破妄虚。意起身随凌空至，掌过星流化荒芜！");
                modlabel.TextColor = new Color(232, 181, 16);
                modbuffpanel.children.Add(new LayoutElementWrapperUIElement(modlabel));

                int totalBoodGas = mp.eyeBloodGas + mp.handBloodGas + mp.bodyBloodGas + mp.footBloodGas;

                var modlabel_level = new UIText("控法者被动：解锁充能魔法系统，每80点气血增加1点最大法力值，");
                modlabel_level.TextColor = Color.SkyBlue;
                modbuffpanel.children.Add(new LayoutElementWrapperUIElement(modlabel_level));
                modlabel_level = new UIText("控法者被动：原版法力回复无效，获得独立基础回蓝8点每秒。");
                modlabel_level.TextColor = Color.SkyBlue;
                modbuffpanel.children.Add(new LayoutElementWrapperUIElement(modlabel_level));
                modlabel_level = new UIText("魔法上限+" + totalBoodGas/10 + " 总气血：" + totalBoodGas + " 死亡次数：" + mp.deathCount + " 战斗力：" + mp.getPower() + " 当前暴击伤害" + mp.MyCritDmageMult * 100 + "%");
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
                modlabel_level = new UIText("魔神法眼" + praticeText + " 气血值：" + mp.eyeBloodGas);
                modlabel_level.TextColor = Color.Orange;
                modbuffpanel.children.Add(new LayoutElementWrapperUIElement(modlabel_level));
                modlabel_level = new UIText("洞破妄虚：魔法暴击伤害+" + (mp.eyeBloodGas / 1000)+ "%(每1000气血+1)");
                modlabel_level.TextColor = Color.SkyBlue;
                modbuffpanel.children.Add(new LayoutElementWrapperUIElement(modlabel_level));
                modlabel_level = new UIText("万法归宗：击杀敌人回复" + (mp.eyeBloodGas / 1000 + 10) + "点法力(初始10，每1000气血+1)");
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
                modlabel_level = new UIText("充能魔法：弹幕类魔法可以充能。当前充能上限：" + (mp.magicChargeCountMax) + "(初始10，点亮+10，每2500气血+1)");
                modlabel_level.TextColor = Color.SkyBlue;
                modbuffpanel.children.Add(new LayoutElementWrapperUIElement(modlabel_level));
                modlabel_level = new UIText("快速充能：充能速度+" + (mp.handBloodGas / 2500 + 20) + "%(初始20%，每2500气血+1%)");
                modlabel_level.TextColor = Color.SkyBlue;
                modbuffpanel.children.Add(new LayoutElementWrapperUIElement(modlabel_level));
                modlabel_level = new UIText("魔法掌控：充能魔法伤害+" + (mp.handBloodGas / 200) + "%(每200气血+1%) ");
                modlabel_level.TextColor = Color.SkyBlue;
                modbuffpanel.children.Add(new LayoutElementWrapperUIElement(modlabel_level));

                if (!mp.boughtbuffList[2])
                    praticeText = "【未修炼】";
                else if (mp.practiceBody)
                    praticeText = "【修炼中】";
                else
                    praticeText = "【暂停修炼】";
                modlabel_level = new UIText("不灭法躯" + praticeText + " 气血值：" + mp.bodyBloodGas);
                modlabel_level.TextColor = Color.Orange;
                modbuffpanel.children.Add(new LayoutElementWrapperUIElement(modlabel_level));
                modlabel_level = new UIText("法力之源：每秒回复自身" + (mp.bodyBloodGas / 100000 + 1) + "%法力值(初始1%，每100000气血+1)");
                modlabel_level.TextColor = Color.SkyBlue;
                modbuffpanel.children.Add(new LayoutElementWrapperUIElement(modlabel_level));
                modlabel_level = new UIText("不灭法躯：免疫击退，受到伤害的+" + (mp.bodyBloodGas / 10000 + 75) + "%变为消耗法力值(初始75%，每10000+1)");
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
                modlabel_level = new UIText("风驰电掣，雷厉风行：移动速度+" + (mp.footBloodGas / 5000 + 20) + "%(初始20%，每5000气血+1) 跳跃速度+" + (mp.footBloodGas / 1000 + 60) + "%(初始60%，每1000气血+1)");
                modlabel_level.TextColor = Color.SkyBlue;
                modbuffpanel.children.Add(new LayoutElementWrapperUIElement(modlabel_level));
                modlabel_level = new UIText("天涯海角，一步跨之：飞行时间+" + (mp.footBloodGas / 2222 + 10) + "秒(初始10，每2222气血+1)免疫摔落伤害，20W气血时无限飞行");
                modlabel_level.TextColor = Color.SkyBlue;
                modbuffpanel.children.Add(new LayoutElementWrapperUIElement(modlabel_level));
                modlabel_level = new UIText("空间传送：可以消耗当前5%魔力（最低100）使用强力空间传送（无视地形），需要设置按键");
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
