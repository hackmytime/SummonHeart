using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;

namespace SummonHeart.Buffs
{
    public class llPetBuff : ModBuff
    {
        public override void SetDefaults()
        {
            // DisplayName and Description are automatically set from the .lang files, but below is how it is done normally.
            DisplayName.SetDefault("Lotanyi loves you");
            Description.SetDefault("\"It is said that lotanyi's body and mind are reduced by magic\"");
            DisplayName.AddTranslation(GameCulture.Chinese, "洛天依爱着你");
            Description.AddTranslation(GameCulture.Chinese, "\"据说洛天依的身心被魔法变小了\"");
            Main.buffNoTimeDisplay[Type] = true;
            Main.vanityPet[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.buffTime[buffIndex] = 18000;
            player.GetModPlayer<SummonHeartPlayer>().llPet = true;
            bool petProjectileNotSpawned = player.ownedProjectileCounts[mod.ProjectileType("llPet")] <= 0;
            if (petProjectileNotSpawned && player.whoAmI == Main.myPlayer)
            {
                Projectile.NewProjectile(player.position.X + player.width / 2, player.position.Y + player.height / 2, 0f, 0f, mod.ProjectileType("llPet"), 0, 0f, player.whoAmI, 0f, 0f);
            }
        }
    }
}