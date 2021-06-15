using SummonHeart.costvalues;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SummonHeart.body
{

    public class VanilaBuffs
    {
        public static List<BuffValue> getVanilla()
        {
            List<BuffValue> modBuffValues = new List<BuffValue>();
            modBuffValues.Add(new BuffValue(ItemID.CopperBar,4, "铜锭", null, new CostValue[] { new ItemCostValue(ItemID.CopperBar, 30) }, "不灭铜躯,防御+4", null));
            modBuffValues.Add(new BuffValue(ItemID.IronBar,6, "铁锭", null, new CostValue[] { new ItemCostValue(ItemID.IronBar, 30) }, "不灭铁躯,防御+6", null));
            return modBuffValues;
        }
    }
}
