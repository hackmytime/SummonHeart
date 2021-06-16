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
            modBuffValues.Add(new BuffValue(ItemID.CopperBar,   6,  "不灭铜躯,防御+6,反伤+6",       "铜锭"));
            modBuffValues.Add(new BuffValue(ItemID.TinBar,      7,  "不灭锡躯,防御+7,反伤+7",       "锡锭"));
            modBuffValues.Add(new BuffValue(ItemID.IronBar,     9,  "不灭铁躯,防御+9,反伤+9",       "铁锭"));
            modBuffValues.Add(new BuffValue(ItemID.LeadBar,     11, "不灭铅躯,防御+11,反伤+11",     "铅锭"));
            modBuffValues.Add(new BuffValue(ItemID.SilverBar,   13, "不灭银躯,防御+13,反伤+13",     "银锭"));
            modBuffValues.Add(new BuffValue(ItemID.TungstenBar, 15, "不灭钨躯,防御+15,反伤+15",     "钨锭"));
            modBuffValues.Add(new BuffValue(ItemID.GoldBar,     16, "不灭金躯,防御+16,反伤+16",     "金锭"));
            modBuffValues.Add(new BuffValue(ItemID.PlatinumBar, 20, "不灭铂金躯,防御+20,反伤+20",   "铂金锭"));
            return modBuffValues;
        }
    }
}
