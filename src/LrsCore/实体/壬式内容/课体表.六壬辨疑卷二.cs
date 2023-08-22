using LrsCore.实体.起课信息;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Reflection;
using YiJingFramework.EntityRelations.DizhiRelations.Extensions;
using YiJingFramework.EntityRelations.EntityCharacteristics.Extensions;
using YiJingFramework.EntityRelations.TianganJigongs.Extensions;
using YiJingFramework.EntityRelations.WuxingRelations;
using YiJingFramework.EntityRelations.WuxingRelations.Extensions;
using YiJingFramework.PrimitiveTypes;

namespace LrsCore.实体.壬式内容;
partial class 课体表
{
    public static 课体表 六壬辨疑神煞纪要(四课 四课, 三传 三传, 天将盘 天将盘)
    {
        var 提供类 = new 六壬辨疑卷二提供类(四课, 三传, 天将盘);
        return new 课体表(提供类.判断课体());
    }

    private sealed class 六壬辨疑卷二提供类
    {
        [AttributeUsage(AttributeTargets.Method)]
        private sealed class 判课体法Attribute : Attribute { }

        private readonly 四课 传入四课;
        private readonly 天将盘 传入天将盘;
        private readonly 三传 传入三传;
        public 六壬辨疑卷二提供类(四课 四课, 三传 三传, 天将盘 天将盘)
        {
            this.传入四课 = 四课;
            this.传入天将盘 = 天将盘;
            this.传入三传 = 三传;
        }

        private IReadOnlyList<四课之一>? 四课;
        private (IReadOnlyList<四课之一> 课, bool 为贼)? 克课;
        private IReadOnlyList<四课之一>? 遥克课;
        private IReadOnlyList<四课之一>? 不重复四课;
        private (bool 遵循顺逆, bool 为顺)? 天将顺逆;

        public IEnumerable<string> 判断课体()
        {
            foreach(var 方法 in this.GetType().GetMethods())
            {
                if(方法.GetCustomAttributes(typeof(判课体法Attribute)).Any())
                {
                    var 结果 = 方法.Invoke(this, null);
                    Debug.Assert(结果 is not null);
                    if ((bool)结果)
                    {
                        yield return 方法.Name;
                    }
                }
            }
        }
        private sealed class 四课之一
        {
            public Tiangan? 干下 { get; }
            public Dizhi 支下或干下之寄宫 { get; }
            public Wuxing 下五行 { get; }
            public Yinyang 下阴阳 { get; }
            public Dizhi 上 { get; }
            public Wuxing 上五行 { get; }
            public Yinyang 上阴阳 { get; }
            internal 四课之一(Dizhi 下, Dizhi 上)
            {
                this.上 = 上;
                this.上五行 = 上.Wuxing();
                this.上阴阳 = 上.Yinyang();

                this.干下 = null;
                this.支下或干下之寄宫 = 下;
                this.下五行 = 下.Wuxing();
                this.下阴阳 = 下.Yinyang();
            }
            internal 四课之一(Tiangan 下, Dizhi 上)
            {
                this.上 = 上;
                this.上五行 = 上.Wuxing();
                this.上阴阳 = 上.Yinyang();

                this.干下 = 下;
                this.支下或干下之寄宫 = 下.Jigong();
                this.下五行 = 下.Wuxing();
                this.下阴阳 = 下.Yinyang();
            }
        }

        private IReadOnlyList<四课之一> 取四课()
        {
            if (四课 is null)
            {
                四课 = new 四课之一[4] {
                        new 四课之一(传入四课.日, 传入四课.日阳),
                        new 四课之一(传入四课.日阳, 传入四课.日阴),
                        new 四课之一(传入四课.辰, 传入四课.辰阳),
                        new 四课之一(传入四课.辰阳, 传入四课.辰阴),
                    };
            }
            return 四课;
        }

        private IReadOnlyList<四课之一> 取不重复四课()
        {
            if (不重复四课 is null)
            {
                不重复四课 = 取四课().DistinctBy(课 => 课.上).ToArray();
            }
            return 不重复四课;
        }
        private (IReadOnlyList<四课之一> 课, bool 为贼) 取克课()
        {
            if (!克课.HasValue)
            {
                var 四课 = 取四课();
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
                克课 = (有贼否 ? 下贼上课 : 上克下课, 有贼否);
            }
            return 克课.Value;
        }
        private IReadOnlyList<四课之一> 取遥克课()
        {
            if (遥克课 is null)
            {
                var 四课 = 取四课();
                var 干五行 = 四课[0].下五行;
                遥克课 = 四课
                    .Where(课 => 干五行.GetRelation(课.上五行) is
                    WuxingRelation.OvercameByMe or WuxingRelation.OvercomingMe)
                    .ToArray();
            }
            return 遥克课;
        }
        private (bool 遵循顺逆, bool 为顺) 取天将顺逆()
        {
            if (!天将顺逆.HasValue)
            {
                IEnumerable<int> 顺行预期(int 起始)
                {
                    for (int 序号 = 起始 + 12; 序号 < 起始 + 24; 序号++)
                    {
                        yield return 序号 % 12;
                    }
                }
                IEnumerable<int> 逆行预期(int 起始)
                {
                    for (int 序号 = 起始 + 12; 序号 > 起始; 序号--)
                    {
                        yield return 序号 % 12;
                    }
                }
                IEnumerable<int> 取实际()
                {
                    for (int 序号 = 1; 序号 <= 12; 序号++)
                    {
                        yield return (int)传入天将盘.取乘将(new(序号));
                    }
                }

                var 实际 = 取实际();
                var 起 = 实际.First();
                if (实际.SequenceEqual(顺行预期(起)))
                    天将顺逆 = (true, true);
                else if (实际.SequenceEqual(逆行预期(起)))
                    天将顺逆 = (true, false);
                else
                    天将顺逆 = (false, true);
            }
            return 天将顺逆.Value;
        }
        private IEnumerable<Dizhi> 迭代三传()
        {
            yield return 传入三传.初传;
            yield return 传入三传.中传;
            yield return 传入三传.末传;
        }
        private IEnumerable<Dizhi> 迭代四课(bool 含日 = true)
        {
            if (含日)
                yield return 传入四课.日.Jigong();
            yield return 传入四课.日阳;
            yield return 传入四课.日阴;
            yield return 传入四课.辰;
            yield return 传入四课.辰阳;
            yield return 传入四课.辰阴;
        }
        #region 卷二
        [判课体法]
        public bool 元首()
        {
            var (课, 为贼) = 取克课();
            if (为贼)
                return false;
            return 课.DistinctBy(课 => 课.上).Count() is 1;
        }

        [判课体法]
        public bool 重审()
        {
            var (课, 为贼) = 取克课();
            if (!为贼)
                return false;
            return 课.DistinctBy(课 => 课.上).Count() is 1;
        }

        [判课体法]
        public bool 知一()
        {
            var (课, _) = 取克课();
            var 去重课 = 课.DistinctBy(课 => 课.上).ToArray();
            if (去重课.Length is not 2)
                return false;
            return 去重课[0].上阴阳 != 去重课[1].上阴阳;
        }

        [判课体法]
        public bool 涉害()
        {
            var (课, _) = 取克课();
            var 去重课 = 课.DistinctBy(课 => 课.上).ToArray();
            if (去重课.Length is not 2)
                return false;
            return 去重课[0].上阴阳 == 去重课[1].上阴阳;
        }

        [判课体法]
        public bool 遥克()
        {
            var (克课, _) = 取克课();
            if (克课.Count is not 0)
                return false;
            return 取遥克课().Count is not 0;
        }

        [判课体法]
        public bool 昂星()
        {
            var (克课, _) = 取克课();
            if (克课.Count is not 0)
                return false;

            if (取遥克课().Count is not 0)
                return false;

            var 四课 = 取不重复四课();
            return 四课.Count is 4;
        }

        [判课体法]
        public bool 别责()
        {
            var (克课, _) = 取克课();
            if (克课.Count is not 0)
                return false;

            if (取遥克课().Count is not 0)
                return false;

            var 四课 = 取不重复四课();
            return 四课.Count is 3;
        }

        [判课体法]
        public bool 八专()
        {
            var (克课, _) = 取克课();
            if (克课.Count is not 0)
                return false;

            if (取遥克课().Count is not 0)
                return false;

#warning 伏吟八专可兼否？
            if (伏吟())
                return false;

            var 四课 = 取四课();
            return 四课[0].支下或干下之寄宫 == 四课[2].支下或干下之寄宫;
        }

        [判课体法]
        public bool 伏吟()
        {
            var 四课 = 取四课();
            return 四课[0].上 == 四课[0].支下或干下之寄宫;
        }

        [判课体法]
        public bool 反吟()
        {
            var 四课 = 取四课();
            return 四课[0].上 == 四课[0].支下或干下之寄宫.Liuchong();
        }
        #endregion

        /*
        #region 卷三
        [判课体法]
        public static bool 三光(壬式信息 式, 缓存 存)
        {
#warning 曰吉神在中，然怎得判断天将如何？
            var 月 = 式.起课信息.年月日时.月支;
            return
                式.起课信息.年月日时.日干.取旺相状态(月) is 旺相状态.旺 or 旺相状态.相 &&
                式.起课信息.年月日时.日支.取旺相状态(月) is 旺相状态.旺 or 旺相状态.相 &&
                式.三传.初传.取旺相状态(月) is 旺相状态.旺 or 旺相状态.相;
        }

        [判课体法]
        public static bool 三阳(壬式信息 式, 缓存 存)
        {
            var (符合顺逆, 顺) = 取天将顺逆(式, 存);
            if (!符合顺逆)
                return false;
            if (!顺)
                return false;
#warning 何谓有气？何谓居前？
            var 月 = 式.起课信息.年月日时.月支;
            bool 有气居贵神前(EarthlyBranch 支)
            {
                return 支.取旺相状态(月) is 旺相状态.旺 or 旺相状态.相 &&
                    ((int)式.天将盘.取乘将(支)) is > 0 and <= 6;
            }
            return
                (有气居贵神前(式.起课信息.年月日时.日干.寄宫()) ||
                有气居贵神前(式.起课信息.年月日时.日支)) &&
                式.三传.初传.取旺相状态(月) is 旺相状态.旺 or 旺相状态.相;
        }

        [判课体法]
        public static bool 三奇(壬式信息 式, 缓存 存)
        {
            EarthlyBranch 旬奇 = new(
                式.起课信息.年月日时.旬所在.旬首支.Index switch {
                    1 or 11 => 2,
                    9 or 7 => 1,
                    _ => 12 // 5 or 3
                });
            if (迭代三传(式, 存).Contains(旬奇))
                return true;
            return false;
        }

        [判课体法]
        public static bool 六仪(壬式信息 式, 缓存 存)
        {
            return 迭代三传(式, 存).Contains(式.起课信息.年月日时.旬所在.旬首支);
        }

        [判课体法]
        public static bool 时泰(壬式信息 式, 缓存 存)
        {
            if (!迭代三传(式, 存).Contains(式.起课信息.年月日时.年支))
                return false;
            if (!迭代三传(式, 存).Contains(式.起课信息.年月日时.月支))
                return false;
#warning 还未写完
            return false;
        }
        #endregion
        */
    }
}
