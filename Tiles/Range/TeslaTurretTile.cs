using Microsoft.Xna.Framework;
using SummonHeart.Extensions.TurretSystem;
using SummonHeart.Items.Range.Turret;
using SummonHeart.NPCs.Range;
using Terraria.ModLoader;

namespace SummonHeart.Tiles.Range
{
    public class TeslaTurretTile : TurretTile
    {
        public override void ModifyLight(int i, int j, ref float R, ref float G, ref float B)
        {
            R = 0.6f;
            G = 0.6f;
            B = 0.6f;
        }

        public override int GetHead()
        {
            return ModContent.NPCType<TeslaTurretHead>();
        }

        public override TurretTileEntity GetTE()
        {
            return ModContent.GetInstance<TETeslaTurretTile>();
        }

        public override int GetItem()
        {
            return ModContent.ItemType<TeslaTurretItem>();
        }

        public override string GetName()
        {
            return "Tesla Turret";
        }

        public override Color GetMapColor()
        {
            return new Color(64, 28, 73);
        }
    }
}
