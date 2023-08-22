using System.Diagnostics;
using System.Reflection;
using YiJingFramework.EntityRelations.DizhiRelations.Extensions;
using YiJingFramework.EntityRelations.EntityCharacteristics.Extensions;
using YiJingFramework.EntityRelations.TianganJigongs.Extensions;
using YiJingFramework.EntityRelations.WuxingRelations;
using YiJingFramework.EntityRelations.WuxingRelations.Extensions;
using YiJingFramework.PrimitiveTypes;

namespace LrsCore.实体.壬式内容;

public partial class 课体表
{
    public static 课体表 六壬辨疑卷二(四课 四课)
    {
        return new 课体表(六壬辨疑卷二提供类.判断课体(四课));
    }

    private static class 六壬辨疑卷二提供类
    {
        private sealed record 四课之一(
            Tiangan? 干下, Dizhi 支下或干下之寄宫, Wuxing 下五行, Yinyang 下阴阳,
            Dizhi 上, Wuxing 上五行, Yinyang 上阴阳)
        {
            internal 四课之一(Dizhi 下, Dizhi 上) :
                this(null, 下, 下.Wuxing(), 下.Yinyang(), 上, 上.Wuxing(), 上.Yinyang())
            { }
            internal 四课之一(Tiangan 下, Dizhi 上) :
                this(下, 下.Jigong(), 下.Wuxing(), 下.Yinyang(), 上, 上.Wuxing(), 上.Yinyang())
            { }
        }

        public static IEnumerable<string> 判断课体(四课 四课)
        {
            var 新四课 = new[] {
                new 四课之一(四课.日, 四课.日阳),
                new 四课之一(四课.日阳, 四课.日阴),
                new 四课之一(四课.辰, 四课.辰阳),
                new 四课之一(四课.辰阳, 四课.辰阴),
            };
            var (克课, 为贼) = 取克课(新四课);
            var 去重克课 = 克课.DistinctBy(课 => 课.上).ToArray();
            if (去重克课.Length is 1)
            {
                if (为贼)
                    yield return "重审";
                else
                    yield return "元首";
            }
            else if (去重克课.Length is 2)
            {
                if (去重克课[0].上阴阳 == 去重克课[1].上阴阳)
                    yield return "涉害";
                else
                    yield return "知一";
            }
            else if (去重克课.Length is 0)
            {
                var 遥克课 = 取遥克课(新四课);
                if (遥克课.Length is not 0)
                    yield return "遥克";
                else
                {
                    var 不重复四课 = 新四课.DistinctBy(课 => 课.上).ToArray();
                    if (不重复四课.Length is 4)
                        yield return "昂星";
                    else if (不重复四课.Length is 3)
                        yield return "别责";
                    if (新四课[0].支下或干下之寄宫 == 新四课[2].支下或干下之寄宫)
                        yield return "八专";
                }
            }
            if (新四课[0].上 == 新四课[0].支下或干下之寄宫)
                yield return "伏吟";
            if (新四课[0].上 == 新四课[0].支下或干下之寄宫.Liuchong())
                yield return "反吟";
        }

        private static (IReadOnlyList<四课之一> 课, bool 为贼) 取克课(四课之一[] 四课)
        {
            var 下贼上课 = new List<四课之一>(4);
            var 上克下课 = new List<四课之一>(4);
            foreach (var 课 in 四课)
            {
                switch (课.下五行.GetRelation(课.上五行))
                {
                    case WuxingRelation.OvercameByMe:
                        下贼上课.Add(课);
                        break;
                    case WuxingRelation.OvercomingMe:
                        上克下课.Add(课);
                        break;
                }
            }
            var 有贼否 = 下贼上课.Count is not 0;
            return (有贼否 ? 下贼上课 : 上克下课, 有贼否);
        }

        private static 四课之一[] 取遥克课(四课之一[] 四课)
        {
            var 干五行 = 四课[0].下五行;
            return 四课.Where(课 => 干五行.GetRelation(课.上五行) is
                WuxingRelation.OvercameByMe or WuxingRelation.OvercomingMe)
                .ToArray();
        }
    }
}
