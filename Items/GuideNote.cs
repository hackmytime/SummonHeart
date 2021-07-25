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
            DisplayName.SetDefault("The Devil's Letter");
            Tooltip.SetDefault("'Welcome, the one million son of the demon God. You are our last hope." +
                 "\nI'm avoiding the pursuit of the goddess cult. I foresee your arrival, so I've taken the time to write this letter to give you some guidance" +
                 "\n1、Don't believe any of the words of Goddess religion. Their goal is to destroy every son of demon God" +
                 "\n2、The crystal of life, the glue of slime, and the power of magic are hidden in these two objects. Please integrate them as soon as possible." +
                 "\n3、The body refining method of the demon God is the powerful foundation of the son of the demon God. " +
                 "\n   I suggest you practice it as soon as possible, pay attention to the consumption of soul, and don't be greedy. The last one was greedy to death" +
                 "\n4、I will entangle the goddess cult for you and fight for the time for your development. Before you are strong enough," +
                 "\n   don't break the seal of Goddess and attract the attention of goddess" +
                 "\n5、If I die, don't take revenge for me. You are our last hope. I don't want you to have an accident. After all, we are the only two left in the demon clan" +
                 "\n6、I'm really a failure as a demon. I can't even protect my subordinates, and I don't even have a decent gift." +
                 "\n7、You can sell this piece of paper as a gift. It's made of magic dragon skin. This is the only one. It's very valuable." +
                 "\n   You don't have to worry about being seen by others. Only the demons can read it" +
                 "\n8、Wuwuwu, please, please, too many people have died, we must live! Must live well!! If you don't like conflict, it's good to live in seclusion" +
                 "\nIt's not the handwriting of the devil: it's good not to sell it. Don't regret it'");
           
            DisplayName.AddTranslation(GameCulture.Chinese, "魔王的信");
            Tooltip.AddTranslation(GameCulture.Chinese, "欢迎你，第100万任魔神之子，你是我们最后的希望了。" +
                "\n我正在躲避女神教的追杀，预知了你的到来，所以抽空写了这封信给与你一些指导" +
                "\n1、不要相信女神教说的任何一句话，他们的目标就是消灭每一任魔神之子" +
                "\n2、生命水晶，史莱姆的凝胶，魔神的力量传承被我隐藏在这两件物品之中，请尽快将其融合获得魔神的完整传承！" +
                "\n3、魔神炼体法是魔神之子的强大之本，建议你尽快修炼，注意灵魂消耗，不要贪，上一任就是贪死的" +
                "\n4、我会为你缠住女神教，为你争取发育的时间，在你足够强大之前，不要去打破女神封印，引起女神注意" +
                "\n5、如果我死了，不用为我报仇，你是我们最后的希望了，我不希望你出事，毕竟，魔族就剩我们两个了" +
                "\n6、我这个魔王当的还真是失败呢，连自己的部下都保护不了，而且连个像样的礼物都没有。" +
                "\n7、你可以把这张纸卖了，就当做我送给你的礼物吧。这是魔龙皮做的，仅此一份，挺值钱的哟，不用担心字被别人看到，只有魔族的人能看" +
                "\n8、呜呜呜，求你了，求你了，已经死太多人了，一定要活下去！一定要好好的活下去！！如果你不喜欢纷争，隐世安稳的活下去也不错" +
                "\n不是魔王的笔迹：不卖有好处，卖了别后悔。加群:1136043760可窥探天机，获得更多指引");
        }

        public override void SetDefaults()
        {
            item.width = 32;
            item.height = 32;
            item.maxStack = 1;
            item.value = Item.sellPrice(250,0,0,0);
            item.rare = 10;
        }
    }
}