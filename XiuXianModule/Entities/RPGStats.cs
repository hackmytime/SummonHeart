using SummonHeart.Utilities;
using System;
using System.Collections.Generic;
using SummonHeart.XiuXianModule.EnumType;

namespace SummonHeart.XiuXianModule.Entities
{
    class RPGStats
    {

        readonly int Default = 0;
        private Dictionary<Stat, StatData> ActualStat;

        string[] lingenTexts = new string[] { "废品","凡品","下品","中品","上品","良品","超品","极品","完美","先天","凡仙","仙品" };
        string[] meiliTexts = new string[] { "憎恶", "反感", "丑陋", "怪异", "普通", "不凡", "出众", "非凡", "惊人", "超凡", "完美", "仙姿" };
        string[] qiyunTexts = new string[] { "天道弃子","恶运缠身","多灾多难","时运不济","平平淡淡","颇有好运","时来运转","顺风顺水","逢凶化吉","鸿运当头","气运缠身","天道之子" };
        string[] daoxinTexts = new string[] { "咸鱼","不亢不卑","坚守本心","自命不凡","向天问道","百折不挠","坚韧不拔","看破红尘","人定胜天","超凡入圣","肝帝证道" };

        

        public RPGStats()
        {
            ActualStat = new Dictionary<Stat, StatData>(8);
            for (int i = 0; i <= 7; i++)
            {
                ActualStat.Add((Stat)i, new StatData(Default));
            }
        }

        public void SetStats(Stat _stat, int _level, int _xp)
        {
            ActualStat[_stat] = new StatData(_level, _xp);
        }

        public int GetLevelStat(Stat a)
        {
            return ActualStat[a].AddLevel;
        }
        public int GetStat(Stat a)
        {
            return ActualStat[a].GetLevel;
        }
        public string GetStatText(Stat a)
        {
            int level = ActualStat[a].GetLevel;
            
            if (a == Stat.道心)
            {
                level = Mathf.Clamp(level, 0, 10);
                return daoxinTexts[level];
            }
            int v = 0;
            if (level != 0)
                v = (int)Math.Log(level, 2) - 2;
            v = Mathf.Clamp(v, 0, 11);
            if (a == Stat.灵根 || a == Stat.悟性)
            {
                return lingenTexts[v];
            }else if (a == Stat.魅力)
            {
                return meiliTexts[v];
            }
            else if (a == Stat.气运)
            {
                return qiyunTexts[v];
            }
            else
            {
                return lingenTexts[v];
            }
        }

        public void UpgradeStat(Stat statname, int value = 1)
        {
            ActualStat[statname].AddXp(value);
        }
        public int GetStatXP(Stat statname)
        {
            return ActualStat[statname].GetXP;
        }
        public int GetStatXPMax(Stat statname)
        {
            return ActualStat[statname].XpForLevel();
        }

        public void Reset()
        {
            for (int i = 0; i <= 7; i++)
            {
                if ((Stat)i == Stat.道心)
                    continue;
                ActualStat[(Stat)i] = new StatData(Default);
            }
        }

        public void OnLevelUp()
        {
            ActualStat[(Stat.体质)].LevelUp();
            ActualStat[(Stat.力量)].LevelUp();
        }
    }
}
