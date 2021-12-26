﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria.GameContent.UI.Elements;
using Terraria.UI;
using SummonHeart.RPGModule.Enum;

namespace SummonHeart.Utilities
{
    class AdditionalInfo
    {
        static public string GetAdditionalStatInfo(Stat stat)
        {
            string Text = "";
            switch (stat)
            {
                case Stat.Vit:
                    Text = "Vitality : Increase Health\n Health regeneration\n and Armor.";
                    break;
                case Stat.Foc:
                    Text = "Focus : Increase Mana,\n Summon Damage\n and critical Chance.";
                    break;
                case Stat.Cons:
                    Text = "Constitution : Increase Armor,\n Health,\n Health regeneration.";
                    break;
                case Stat.Str:
                    Text = "Strenght : Increase Melee,\n Throw\n and Critical Damage.";
                    break;
                case Stat.Agi:
                    Text = "Agility : Increase Ranged,\n Melee\nand Critical Damage.";
                    break;
                case Stat.Dex:
                    Text = "Dexterity : Increase Throw\nRanged Damage\nArmor Penetration\n Critical Chance.";
                    break;
                case Stat.Int:
                    Text = "Inteligence : Increase Magic Damage,\n Mana Regeneration\n and Mana.";
                    break;
                case Stat.Spr:
                    Text = "Spirit : Increase Magic\n and Summon Damage,\n Increase Mana Regeneration.";
                    break;
            }

            return Text;
        }
    }
}
