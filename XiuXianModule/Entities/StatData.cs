using SummonHeart.Utilities;

namespace SummonHeart.XiuXianModule.Entities
{
    class StatData
    {
        private int level;
        private int xp;

        public int AddLevel { get { return level; } }
        public int GetLevel { get { return level; } }
        public int GetXP { get { return xp; } }

        public int XpForLevel()
        {
            return Mathf.CeilInt(level / 3) + 1;
        }
        public void AddXp(int _xp)
        {
            xp += _xp;
            while (xp >= XpForLevel())
            {
                xp -= XpForLevel();
                level = level + 1;
            }
        }
        public StatData(int _level = 0, int _xp = 0)
        {
            xp = _xp;
            level = _level;
        }
        public void LevelUp()
        {
            level++;
        }

    }
}
