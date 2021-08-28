using SummonHeart.costvalues;
using System.Collections.Generic;
using Terraria.ID;
using Terraria.ModLoader;

namespace SummonHeart.body
{

    public class VanilaBuffs
    {
        
        public static List<BuffValue> getGongFa()
        {
            List<BuffValue> modBuffValues = new List<BuffValue>();
            BuffValue buff1 = new BuffValue(ItemID.LifeCrystal, 5, "修炼魔神之眼");
            modBuffValues.Add(buff1);
            BuffValue buff2 = new BuffValue(ItemID.LifeCrystal, 5, "修炼魔神之手");
            modBuffValues.Add(buff2);
            BuffValue buff3 = new BuffValue(ItemID.LifeCrystal, 5, "修炼魔神之躯");
            modBuffValues.Add(buff3);
            BuffValue buff4 = new BuffValue(ItemID.LifeCrystal, 5, "修炼魔神之腿");
            modBuffValues.Add(buff4);
            BuffValue buff5 = new BuffValue(ItemID.LifeCrystal, 5, "无限法则：点亮获得物品【魔神的无限法则】，可以献祭灵魂提高无限法则上限");
            buff5.cost = new CostValue[] {new MoneyCostValue(10000) };
            modBuffValues.Add(buff5);
            return modBuffValues;
        }

        public static List<BuffValue> getVanilla()
        {
            List<BuffValue> modBuffValues = new List<BuffValue>();
            modBuffValues.AddRange(getGongFa());
            modBuffValues.Add(new BuffValue(ItemID.CopperBar,       6,  "不灭铜躯,肉身强度+6",       "铜锭"));
            modBuffValues.Add(new BuffValue(ItemID.TinBar,          7,  "不灭锡躯,肉身强度+7",       "锡锭"));
            modBuffValues.Add(new BuffValue(ItemID.IronBar,         9,  "不灭铁躯,肉身强度+9",       "铁锭"));
            modBuffValues.Add(new BuffValue(ItemID.LeadBar,         11, "不灭铅躯,肉身强度+11",      "铅锭"));
            modBuffValues.Add(new BuffValue(ItemID.SilverBar,       13, "不灭银躯,肉身强度+13",      "银锭"));
            modBuffValues.Add(new BuffValue(ItemID.TungstenBar,     15, "不灭钨躯,肉身强度+15",      "钨锭"));
            modBuffValues.Add(new BuffValue(ItemID.GoldBar,         16, "不灭金躯,肉身强度+16",      "金锭"));
            modBuffValues.Add(new BuffValue(ItemID.PlatinumBar,     20, "不灭铂金躯,肉身强度+20",    "铂金锭"));
            modBuffValues.Add(new BuffValue(ItemID.MeteoriteBar,    16, "不灭陨石躯,肉身强度+16",    "陨石锭"));
            modBuffValues.Add(new BuffValue(ItemID.DemoniteBar,     19, "不灭暗影躯,肉身强度+19",    "暗影锭"));
            modBuffValues.Add(new BuffValue(ItemID.CrimtaneBar,     19, "不灭猩红躯,肉身强度+19",    "猩红锭"));
            modBuffValues.Add(new BuffValue(ItemID.HellstoneBar,    25, "不灭狱石躯,肉身强度+25",    "狱石锭"));

            modBuffValues.Add(new BuffValue(ItemID.CobaltBar,       28, "不灭钴蓝躯,肉身强度+28",    "钴蓝锭"));
            modBuffValues.Add(new BuffValue(ItemID.PalladiumBar,    32, "不灭钯金躯,肉身强度+32",    "钯金锭"));
            modBuffValues.Add(new BuffValue(ItemID.MythrilBar,      37, "不灭秘银躯,肉身强度+37",    "秘银锭"));
            modBuffValues.Add(new BuffValue(ItemID.OrichalcumBar,   42, "不灭山铜躯,肉身强度+42",    "山铜锭"));
            modBuffValues.Add(new BuffValue(ItemID.AdamantiteBar,   46, "不灭精金躯,肉身强度+46",    "精金锭"));
            modBuffValues.Add(new BuffValue(ItemID.TitaniumBar,     49, "不灭钛金躯,肉身强度+49",    "钛金锭"));
            modBuffValues.Add(new BuffValue(ItemID.HallowedBar,     50, "不灭神圣躯,肉身强度+50",    "神圣锭"));
            modBuffValues.Add(new BuffValue(ItemID.ChlorophyteBar,  56, "不灭叶绿躯,肉身强度+56",    "叶绿锭"));
            modBuffValues.Add(new BuffValue(ItemID.LunarBar,        78, "不灭月冥躯,肉身强度+78",    "月冥锭"));
            return modBuffValues;
        }
    }
}
