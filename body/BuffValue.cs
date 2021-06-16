using Microsoft.Xna.Framework.Graphics;
using SummonHeart.costvalues;
using Terraria;

namespace SummonHeart.body
{
    public class BuffValue
    {

        public delegate void BuffFunction(Player player);

        public int id;
        public int def;
        public string effect;
        public string name;
        public string mod;
        public CostValue[] cost;
        public Texture2D texture;

        public BuffValue(int id, int def, string effect, string name)
        {
            this.id = id;
            this.def = def;
            this.effect = effect;
            this.name = name;
            this.cost = new CostValue[] { new ItemCostValue(id, 99, name), new MoneyCostValue(def * 100) };
        }
    }
}
