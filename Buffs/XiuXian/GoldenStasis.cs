using Terraria;
using Terraria.ModLoader;
using Terraria.Localization;
using SummonHeart;

namespace SummonHeart.Buffs.XiuXian
{
    public class GoldenStasis : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("修炼中");
            Description.SetDefault("吸收天地灵气,但无法移动");
            Main.buffNoSave[Type] = true;
            canBeCleared = true;
            DisplayName.AddTranslation(GameCulture.Chinese, "修练中");
            Description.AddTranslation(GameCulture.Chinese, "吸收天地灵气,但无法移动");
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.GetModPlayer<SummonHeartPlayer>().XiuLian = true;
            player.controlJump = false;
            player.controlDown = false;
            player.controlLeft = false;
            player.controlRight = false;
            player.controlUp = false;
            player.controlUseItem = false;
            player.controlUseTile = false;
            player.controlThrow = false;
            player.controlMount = false;
            player.velocity = player.oldVelocity;
            player.position = player.oldPosition;
        }
    }
}