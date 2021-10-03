using Microsoft.Xna.Framework;
using Paradigm.Library;
using SummonHeart.Extensions;
using SummonHeart.Items.Range;
using SummonHeart.Items.Range.Loot;
using SummonHeart.ui.UIComponents;
using System;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.UI;

namespace SummonHeart.ui.DinoUI
{
    public class DinoUI : UIState
    {
        public void UnloadUI()
        {
        }

        public override void OnInitialize()
        {
            panel = new UIBuilder<UIDragableImage>(panel).Init(new UIDragableImage(GFX.GFX.NOTHING, true)).Left(600f).Top(200f).Width(270f).Height(200f).Append(this);
            shelfImage = new UIBuilder<UIImage>(shelfImage).Init(new UIImage(GFX.GFX.SHELF)).Left(30f).Top(115f).Append(panel);
            DNASlot = new UIBuilder<UIItemSlot>(DNASlot).Init(new UIItemSlot(null, 1f, 0)).Left(16f).Top(20f).Extra(delegate (UIItemSlot e)
            {
                e.validItem = (Item item) => item.IsAir || (item.damage > 0 && item.stack == 1);
                e.PostItemChange += this.DNASlotItemChange;
                e.OnMouseHover += this.DNAHover;
            }).Append(panel);
            DNASlot.backgroundTexture = GFX.GFX.DINO_SLOT;
            fossilSlot = new UIBuilder<UIItemSlot>(fossilSlot).Init(new UIItemSlot(null, 1f, 0)).Left(90f).Top(20f).Extra(delegate (UIItemSlot e)
            {
                e.validItem = (Item item) => item.IsAir || item.Is<LootItem>();
                e.PostItemChange += this.FossilSlotItemChange;
                e.OnMouseHover += this.FossilHover;
            }).Append(panel);
            fossilSlot.backgroundTexture = GFX.GFX.DINO_SLOT;
            outputSlot = new UIBuilder<UIItemSlot>(outputSlot).Init(new UIItemSlot(null, 1f, 0)).Left(55f).Top(75f).Width(60f).Height(60f).Extra(delegate (UIItemSlot e)
            {
                e.validItem = (Item item) => item.IsAir;
                e.OnMouseHover += this.OutputHover;
            }).Append(panel);
            outputSlot.backgroundTexture = GFX.GFX.NOTHING;
            infoButton = new UIBuilder<UITextHoverImageButton>(infoButton).Init(new UITextHoverImageButton(GFX.GFX.INFO_BUTTON, 
                "-在左侧物品栏放置要强化的武器; 右侧显示强化成功率." +
                "\n-在右侧物品栏放置对应等级的生物材料; " +
                "\n1级科技武器和稀有度0-2武器强化级别为1" +
                "\n2级科技武器和稀有度3-4武器强化级别为2" +
                "\n3级科技武器和稀有度5-6武器强化级别为3" +
                "\n4级科技武器和稀有度7-8武器强化级别为4" +
                "\n5级科技武器和稀有度9-10武器强化级别为5" +
                "\n6级科技武器和稀有度11-12和-12武器强化级别为6"
                )).Left(235f).Top(20f).Size(GFX.GFX.INFO_BUTTON).Append(panel);
            button = new UIBuilder<UITextHoverImageButton>(button).Init(new UITextHoverImageButton(GFX.GFX.DINO_EXPORT_BUTTON, 
                btnMsg)).Left(35f).Top(95f).OnClick(new MouseEvent(ExportButtonPressed)).Append(panel);
            successBar = new UIBuilder<SuccessChanceBar>(successBar).Init(new SuccessChanceBar(GFX.GFX.DINO_CHANCE_BAR)).Left(165f).Top(15f).Append(panel);
        }

        private void ExportButtonPressed(UIMouseEvent evt, UIElement listeningElement)

        {
            bool canRoll = true;
            if (DNASlot.item.IsAir)
            {
                Main.NewText("还没有放置武器.", byte.MaxValue, 50, 50, false);
                canRoll = false;
            }
            if (fossilSlot.item.IsAir)
            {
                Main.NewText("还没有放置生物材料.", byte.MaxValue, 50, 50, false);
                canRoll = false;
            }
            if (!outputSlot.item.IsAir)
            {
                Main.NewText("请取走强化成功的武器.", byte.MaxValue, 50, 50, false);
                canRoll = false;
            }
            if (!HasLoot())
            {
                PowerGItem powerGItem = DNASlot.item.GetGlobalItem<PowerGItem>();
                int costCount = powerGItem.powerLevel;
                if (costCount == 0)
                    costCount = 1;
                if (fossilSlot.item.stack >= costCount)
                    canRoll = true;
                Main.NewText($"至少需要{costCount}个{powerGItem.itemRare}级生物材料", byte.MaxValue, 50, 50, false);
                canRoll = false;
            }
            if (canRoll)
            {
                PowerGItem powerGItem = DNASlot.item.GetGlobalItem<PowerGItem>();
                int costCount = powerGItem.powerLevel;
                if (costCount == 0)
                    costCount = 1;
                fossilSlot.item.stack -= costCount;
                if (fossilSlot.item.stack <= 0)
                {
                    fossilSlot.item.TurnToAir();
                }
                if (PowerSuccess())
                {
                    Main.PlaySound((int)SoundID.Item4.Type, Main.LocalPlayer.Center, 0);
                    powerGItem.powerLevel++;
                    /*outputSlot.item = DNASlot.item;
                    DNASlot.item.TurnToAir();*/
                }
                else
                {
                    //强化失败
                    Main.NewText("强化失败，你是个非酋.", byte.MaxValue, 50, 50, false);
                }
            }
        }

        private bool HasLoot()
        {
            PowerGItem powerGItem = DNASlot.item.GetGlobalItem<PowerGItem>();
            powerGItem.setItemRare(DNASlot.item);
            int itemRare = powerGItem.itemRare;
            if(itemRare == 6)
            {
                if (fossilSlot.item.rare == -12)
                    return true;
            }
            else
            {
                if (itemRare + 4 == fossilSlot.item.rare)
                    return true;
            }
            return false;
        }

        private bool PowerSuccess()
        {
            return Utils.NextFloat(Main.rand) <= this.GetChance();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (Vector2.Distance(Main.LocalPlayer.Center, tileCenter) > 128f)
            {
                ModUIHandler.dinoLoader.HideUI();
            }
            btnMsg = "消耗生物材料进行强化";
            if (!DNASlot.item.IsAir)
            {
                PowerGItem powerGItem = DNASlot.item.GetGlobalItem<PowerGItem>();
                int costCount = powerGItem.powerLevel;
                if (costCount == 0)
                    costCount = 1;
                Main.hoverItemName = $"消耗{costCount}个{powerGItem.itemRare}生物材料进行强化" +
                    $"\n强化成功率{GetChance() * 100}%";
            }
        }

        public void PutSlotItemsInInventory()
        {
            if (!DNASlot.item.IsAir)
            {
                Main.LocalPlayer.QuickSpawnClonedItem(DNASlot.item, DNASlot.item.stack);
                DNASlot.item.TurnToAir();
            }
            if (!fossilSlot.item.IsAir)
            {
                Main.LocalPlayer.QuickSpawnClonedItem(fossilSlot.item, fossilSlot.item.stack);
                fossilSlot.item.TurnToAir();
            }
            if (!outputSlot.item.IsAir)
            {
                Main.LocalPlayer.QuickSpawnClonedItem(outputSlot.item, outputSlot.item.stack);
                outputSlot.item.TurnToAir();
            }
        }

        public void DNASlotItemChange(Item item)
        {
            if (!item.IsAir)
            {
                successBar.SuccessChance = GetChance();
                return;
            }
            successBar.SuccessChance = 0f;
        }

        private float GetChance()
        {
            PowerGItem powerGItem = DNASlot.item.GetGlobalItem<PowerGItem>();
            int powerLevel = powerGItem.powerLevel;
            float chance = 1 - powerLevel * 0.01f;
            if (chance < 0.05f)
                chance = 0.05f;
            return chance;
        }

        public void FossilSlotItemChange(Item item)
        {
            if (!item.IsAir)
            {
                successBar.FossilValue = 1f;
                return;
            }
            successBar.FossilValue = 0f;
        }

        public void OutputHover()
        {
            if (outputSlot.item.IsAir)
            {
                Main.hoverItemName = $"强化成功率{GetChance() * 100}%";
            }
        }

        public void FossilHover()
        {
            if (fossilSlot.item.IsAir)
            {
                Main.hoverItemName = "放置生物材料.";
            }
        }

        public void DNAHover()
        {
            if (DNASlot.item.IsAir)
            {
                Main.hoverItemName = "放置要强化的武器.";
            }
        }

        // Token: 0x04000168 RID: 360
        private UIDragableImage panel;

        // Token: 0x04000169 RID: 361
        public UIItemSlot DNASlot;

        // Token: 0x0400016A RID: 362
        public UIItemSlot fossilSlot;

        // Token: 0x0400016B RID: 363
        public UIItemSlot outputSlot;

        // Token: 0x0400016C RID: 364
        public UIImage shelfImage;

        // Token: 0x0400016D RID: 365
        public UITextHoverImageButton button;

        // Token: 0x0400016E RID: 366
        public UITextHoverImageButton infoButton;

        // Token: 0x0400016F RID: 367
        public SuccessChanceBar successBar;

        // Token: 0x04000170 RID: 368
        public Vector2 tileCenter;

        // Token: 0x04000171 RID: 369
        public bool visible;
        public string btnMsg;
    }
}
