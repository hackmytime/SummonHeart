using Microsoft.Xna.Framework;
using SummonHeart.Extensions;
using SummonHeart.Items.Skill.Tools;
using SummonHeart.Utilities;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace SummonHeart.Items.Range.Tools
{
    public class AutoHouseTool : ModItem
    {
        private Player owner
        {
            get
            {
                return Main.player[item.owner];
            }
        }

        public override void SetStaticDefaults()
        {
            DisplayName.AddTranslation(GameCulture.Chinese, "自动房屋建筑装置");
            Tooltip.SetDefault("在鼠标位置为起点以玩家朝向为方向自动建筑房屋，一次最多建造10个\n1个房屋需要消耗76木头+2凝胶");
        }

        public override void SetDefaults()
        {
            item.height = 38;
            item.width = 38;
            item.maxStack = 1;
            item.rare = 2;
            item.value = 100000;
            item.useTime = 30;
            item.useAnimation = 30;
            item.useStyle = 1;
        }

        public override void HoldItem(Player player)
        {
            
            base.HoldItem(player);
        }

        public override bool UseItem(Player player)
        {
            Vector2 mousePosition = Main.MouseWorld;

            if (player.whoAmI == Main.myPlayer)
            {
                SummonHeartPlayer mp = player.SH();
                int tileX = (int)(mousePosition.X / 16f);
                int tileY = (int)(mousePosition.Y / 16f);
                if (Main.netMode == 0)
                {
                    AutoHouseTool.HandleBuilding(tileX, tileY, player.whoAmI);
                }
                else
                {
                    MsgUtils.BuildHousePacket((int)mousePosition.X, (int)mousePosition.Y, player.whoAmI);
                }
            }
            return true;
        }

        public static void HandleBuilding(int tileX, int tileY, int whoami)
        {
            Player player = Main.player[whoami];
            for (int i = 0; i < 10; i++)
            {
                int direction = player.direction;
                int newTileX = tileX + i * 4 * direction;
                if (direction == -1)
                    newTileX -= 4;
                if (Builder.BuildHouse(newTileX, tileY, 0, true))
                {
                }
            }
        }

        public static void HandleBuilding2(int tileX, int tileY, int whoami)
        {
            Player player = Main.player[whoami];
            for (int i = 0; i < 10; i++)
            {
                int direction = player.direction;
                int newTileX = tileX + i * 4 * direction;
                if (direction == -1)
                    newTileX -= 4;
                if (Builder.BuildHouse(newTileX, tileY, 0, false))
                {
                }
            }
        }



        public override void AddRecipes()
        {
            ModRecipe modRecipe = new ModRecipe(mod);
            modRecipe.AddIngredient(ItemID.Wood, 99);
            modRecipe.AddTile(TileID.WorkBenches);
            modRecipe.SetResult(this, 1);
            modRecipe.AddRecipe();
        }
    }
}
