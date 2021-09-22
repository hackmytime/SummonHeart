using SummonHeart.Items.Range.AmmoSkill;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace SummonHeart.Items.Range.Tools.Bomb
{
    public class C4Item2 : ExplosiveItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("C4Item2");
            Tooltip.SetDefault("C4Item2");
            DisplayName.AddTranslation(GameCulture.Chinese, "3级科技造物·爆破工程炸弹【立即爆炸型号】");
            Tooltip.AddTranslation(GameCulture.Chinese, "爆炸半径：20码" +
                "\n爆炸伤害：1000" +
                "\n接触物块和敌人立即爆炸");
        }

        public override void SafeSetDefaults()
        {
            item.damage = 1000;  //The damage stat for the Weapon.
            item.width = 32;    //sprite width
            item.height = 32;   //sprite height
            item.maxStack = 999;   //This defines the items max stack
            item.consumable = true;  //Tells the game that this should be used up once fired
            item.useStyle = 1;   //The way your item will be used, 1 is the regular sword swing for example
            item.rare = 9;   //The color the title of your item when hovering over it ingame
            item.UseSound = SoundID.Item1; //The sound played when using this item
            item.useAnimation = 50;  //How long the item is used for.
            item.useTime = 50;   //How fast the item is used.
            item.value = Item.sellPrice(1, 0, 0, 0);   //How much the item is worth, in copper coins, when you sell it to a merchant. It costs 1/5th of this to buy it back from them. An easy way to remember the value is platinum, gold, silver, copper or PPGGSSCC (so this item price is 3 silver)
            item.noUseGraphic = true;
            item.noMelee = true;      //Setting to True allows the weapon sprite to stop doing damage, so only the projectile does the damge
            item.shoot = mod.ProjectileType("C4Projectile2"); //This defines what type of projectile this item will shoot
            item.shootSpeed = 5f; //This defines the projectile speed when shot
                                  //item.createTile = mod.TileType("ExplosiveTile");
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.Dynamite, 500);
            recipe.AddIngredient(ItemID.Gel, 100);
            recipe.AddIngredient(ModContent.ItemType<HotUnit>(), 5);
            recipe.SetResult(this, 100);
            recipe.AddRecipe();
            recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<C4Item>(), 10);
            recipe.SetResult(this, 10);
            recipe.AddRecipe();
        }
    }
}