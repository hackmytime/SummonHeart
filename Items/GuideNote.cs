using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Terraria.Localization;

namespace SummonHeart.Items
{
    public class GuideNote : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("GuideNote");
            Tooltip.SetDefault("'I know that Sasscade Yo-Yo exists. But how do you make it?"
            + "\nI am pretty sure that the [c/00FF00:Terrarian Yo-Yo] is the first component."
            + "\nAn [c/00FF00:Alchemical Bundle] should be the second component..."
            + "\nBut what could be the third one? I think that it is something, related to the Sass..."
            + "\nMaybe inner part of the [c/00FF00:Rod of Discord] can be it?"
            + "\nAnd the final component... Is the [c/00FF00:Yo-yo Bag]. I am 100% sure about this.'");
            DisplayName.AddTranslation(GameCulture.Russian, "Запись исследования №1");
            Tooltip.AddTranslation(GameCulture.Russian, "'Я знаю, что йо-йо Сасскад существует. Но как именно его сделать?\nЯ уверена, что первый компонент - [c/00FF00:Йо-Йо Террариан].\n[c/00FF00:Алхимический Набор] может быть вторым компонентом...\nНо что насчёт третьего? Я думаю, это что-то относящееся к ссорам...\nМожет внутренняя часть [c/00FF00:Жезла Раздора] подойдёт?\nИ последний компонент... Это [c/00FF00:Сумка для Йо-Йо]. Я на 100% в этом уверена.'");

            DisplayName.AddTranslation(GameCulture.Chinese, "魔王的信");
            Tooltip.AddTranslation(GameCulture.Chinese, "'欢迎你，第100万任魔神之子，你是我们最后的希望了。" +
                "\n我正在躲避女神教的追杀，预知了你的到来，所以抽空写了这封信给与你一些指导" +
                "\n1、不要相信女神教说的任何一句话，他们的目标就是消灭每一任魔神之子" +
                "\n2、生命水晶，史莱姆的凝胶，魔神的力量传承被我隐藏在这两件物品之中，请尽快将其融合获得魔神的完整传承！" +
                "\n3、魔神炼体法是魔神之子的强大之本，建议你尽快修炼，注意灵魂消耗，不要贪，上一任就是贪死的" +
                "\n4、我会为你缠住女神教，为你争取发育的时间，在你足够强大之前，不要去打破女神封印，引起女神注意" +
                "\n5、如果我死了，不用为我报仇，你是我们最后的希望了，我不希望你出事，毕竟，魔族就剩我们两个了" +
                "\n6、我这个魔王当的还真是失败呢，连自己的部下都保护不了，而且连个像样的礼物都没有。" +
                "\n7、你可以把这张纸卖了，就当做我送给你的礼物吧。这是魔龙皮做的，仅此一份，挺值钱的哟，不用担心字被别人看到，只有魔族的人能看" +
                "\n8、呜呜呜，求你了，求你了，已经死太多人了，一定要活下去！一定要好好的活下去！！如果你不喜欢纷争，隐世安稳的活下去也不错" +
                "\n不是魔王的笔迹：不卖有好处，卖了别后悔'");
        }

        public override void SetDefaults()
        {
            item.width = 32;
            item.height = 32;
            item.maxStack = 1;
            item.value = Item.sellPrice(250,0,0,0); ;
            item.rare = 10;
        }
    }
}