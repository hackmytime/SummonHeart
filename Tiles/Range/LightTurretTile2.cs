using Microsoft.Xna.Framework;
using SummonHeart.Extensions.TurretSystem;
using SummonHeart.Items.Range.Turret;
using SummonHeart.NPCs.Range;
using Terraria.ModLoader;

namespace SummonHeart.Tiles.Range
{
    public class LightTurretTile2 : TurretTile
    {
        public override void ModifyLight(int i, int j, ref float R, ref float G, ref float B)
        {
            R = 0.6f;
            G = 0.6f;
            B = 0.6f;
        }

        public override int GetHead()
        {
            return ModContent.NPCType<LightTurretHead2>();
        }

        public override TurretTileEntity GetTE()
        {
            return ModContent.GetInstance<TELightTurretTile2>();
        }

        public override int GetItem()
        {
            return ModContent.ItemType<LightTurretItem2>();
        }

        public override string GetName()
        {
            return "Light Turret";
        }

        public override Color GetMapColor()
        {
            return new Color(64, 28, 73);
        }
    }
}
