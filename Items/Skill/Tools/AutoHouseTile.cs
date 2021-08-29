using Terraria;
using Terraria.ModLoader;

namespace SummonHeart.Items.Skill.Tools
{
    public class AutoHouseTile : ModTile
    {
        public override void SetDefaults()
        {
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = true;
            Main.tileBlockLight[Type] = true;
            Main.tileLighted[Type] = true;
        }

        public override void PlaceInWorld(int i, int j, Item item)
        {
            Projectile.NewProjectile(i * 16 + 8, (j + 2) * 16, 0f, 0f, mod.ProjectileType("AutoHouseProj"), 0, 0, Main.myPlayer);
        }
    }
}