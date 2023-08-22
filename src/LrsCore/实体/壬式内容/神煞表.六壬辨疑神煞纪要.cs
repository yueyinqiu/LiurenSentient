using LrsCore.实体.起课信息;
using System.Diagnostics;
using YiJingFramework.EntityRelations.DizhiMengZhongJis;
using YiJingFramework.EntityRelations.DizhiMengZhongJis.Extensions;
using YiJingFramework.EntityRelations.DizhiRelations.Extensions;
using YiJingFramework.EntityRelations.EntityCharacteristics.Extensions;
using YiJingFramework.EntityRelations.ShierZhangshengs;
using YiJingFramework.EntityRelations.ShierZhangshengs.Extensions;
using YiJingFramework.EntityRelations.TianganJigongs.Extensions;
using YiJingFramework.EntityRelations.TianganRelations.Extensions;
using YiJingFramework.PrimitiveTypes;

namespace LrsCore.实体.壬式内容;

public partial class 神煞表
{
    public static 神煞表 六壬辨疑神煞纪要(年月日时 年月日时)
    {
        return new 神煞表(六壬辨疑神煞纪要提供类.取神煞(年月日时));
    }

    private static class 六壬辨疑神煞纪要提供类
    {
        private static Dizhi 某日月起某某行十二支(
            int 某日月, int 起某, Dizhi 所至, int 步长 = 1)
        {
            int 所行 = 所至.Index - 某日月;
            return new(起某 + 所行 * 步长);
        }
        private static Dizhi 马(Dizhi 支)
        {
            return 支.SanheRelation().DizhiOfZhangsheng.Liuchong();
        }
        public static IEnumerable<(string, Dizhi?)> 取神煞(年月日时 年月日时)
        {
            #region 干煞
            {
                // 阳干同禄神，阴干从官星之禄神。如乙以庚为官用申之类。
                var 日干 = 年月日时.日干;
                if (!日干.Yinyang().IsYang)
                    日干 = 日干.Wuhe();
                yield return ("日德", 日干.ShierZhangsheng(ShierZhangsheng.Linguan));
            }
            {
                // 日干对宫之神。如甲用己之类。
                yield return ("日合", 年月日时.日干.Wuhe().Jigong());
            }
            Dizhi 游都;
            {
                // 甲己在丑，乙庚在子，丙辛在寅，丁壬在巳，戊癸在申。
                游都 = new Dizhi(
                    年月日时.日干.Index switch
                    {
                        1 or 6 => 2,
                        2 or 7 => 1,
                        3 or 8 => 3,
                        4 or 9 => 6,
                        _ => 9
                    });
                yield return ("游都", 游都);
            }
            {
                // 即游都对宫之神。
                yield return ("鲁都", 游都.Liuchong());
            }
            {
                // 午巳辰卯寅丑未申酉戌
                var 结果 = new Dizhi(
                    年月日时.日干.Index switch
                    {
                        1 => 7,
                        2 => 6,
                        3 => 5,
                        4 => 4,
                        5 => 3,
                        6 => 2,
                        7 => 8,
                        8 => 9,
                        9 => 10,
                        _ => 11
                    });
                yield return ("干奇", 结果);
            }
            #endregion

            #region 支煞
            {
                // 子日起巳，顺行十二支。
                yield return ("支德", 某日月起某某行十二支(1, 6, 年月日时.日支));
            }
            {
                // 用五行起例。
                yield return ("支墓", 年月日时.日支.Wuxing().ShierZhangsheng(ShierZhangsheng.Mu));
            }
            {
                // 阳支后三神，阴支前三神。
                // 子 丑 寅 卯 辰 巳 午 未 申 酉 戌 亥
                // 戌 卯 子 丑 寅 未 辰 酉 午 亥 申 丑
                yield return ("支破", 年月日时.日支.Liupo());
            }
            {
                // 三合之第三位神。
                yield return ("华盖", 年月日时.日支.SanheRelation().DizhiOfMu);
            }
            {
                // 三合之第二位神。
                yield return ("将星", 年月日时.日支.SanheRelation().DizhiOfDiwang);
            }
            {
                // 三合第一位冲神。
                yield return ("驿马", 马(年月日时.日支));
            }
            {
                // 三合第三位之前一位神。
                yield return ("劫煞", 年月日时.日支.SanheRelation().DizhiOfMu.Next());
            }
            {
                // 孟日用酉，仲日用巳，季日用丑。即金神，又名红砂。
                var 结果 = new Dizhi(年月日时.日支.MengZhongJi() switch
                {
                    MengZhongJi.Meng => 10,
                    MengZhongJi.Zhong => 6,
                    _ => 2,
                });
                yield return ("破碎", 结果);
            }
            {
                // 午 辰 寅 未 酉 亥
                // 巳 卯 丑 申 戌 子
                var 结果 = new Dizhi(年月日时.日支.Index switch
                {
                    1 => 7,
                    2 => 6,
                    3 => 5,
                    4 => 3,
                    5 => 2,
                    6 => 8,
                    7 => 9,
                    8 => 10,
                    9 => 11,
                    10 => 12,
                    _ => 1
                });
                yield return ("支仪", 结果);
            }
            #endregion

            #region 月煞
            {
                // 正 二 三 四 五 六 七 八 九 十 十一 十二
                // 丁 坤 壬 辛 干 甲 癸 艮 丙 乙 巽  庚
                var 结果 = new Dizhi(年月日时.月支.Index switch
                {
                    3 => 8,
                    4 => 9,
                    5 => 12,
                    6 => 11,
                    7 => 12,
                    8 => 3,
                    9 => 2,
                    10 => 3,
                    11 => 6,
                    12 => 5,
                    1 => 6,
                    _ => 9
                });
                yield return ("天德", 结果);
            }
            {
                // 巳寅亥申三轮，即三合之禄神。
                var 月支 = 年月日时.月支;
                var 三合五行 = 月支.SanheRelation().DizhiOfDiwang.Wuxing();
                yield return ("月德", 三合五行.ShierZhangsheng(ShierZhangsheng.Linguan));
            }
            {
                // 正月起子顺行。
                yield return ("生气", 某日月起某某行十二支(3, 1, 年月日时.月支));
            }
            Dizhi 死气;
            {
                // 正月起午顺行。
                死气 = 某日月起某某行十二支(3, 7, 年月日时.月支);
                yield return ("死气", 死气);
            }
            {
                // 官符、孝服、谩语：俱同死气。
                yield return ("官符", 死气);
                yield return ("孝服", 死气);
                yield return ("谩语", 死气);
            }
            Dizhi 死神;
            {
                // 正月起巳顺行。
                死神 = 某日月起某某行十二支(3, 6, 年月日时.月支);
                yield return ("死神", 死神);
            }
            {
                // 同死神。
                yield return ("火烛", 死神);
            }
            {
                // 正月起辰顺行。
                yield return ("天医", 某日月起某某行十二支(3, 5, 年月日时.月支));
            }
            {
                // 正月起戌顺行。
                yield return ("地医", 某日月起某某行十二支(3, 11, 年月日时.月支));
            }
            Dizhi 天诏;
            {
                // 正月起亥顺行。
                天诏 = 某日月起某某行十二支(3, 12, 年月日时.月支);
                yield return ("天诏", 天诏);
            }
            {
                // 同天诏。
                yield return ("飞魂", 天诏);
            }
            {
                // 正月起酉顺行。
                yield return ("信神", 某日月起某某行十二支(3, 10, 年月日时.月支));
            }
            Dizhi 血支;
            {
                // 正月起丑顺行。
                血支 = 某日月起某某行十二支(3, 2, 年月日时.月支);
                yield return ("血支", 血支);
            }
            {
                // 同血支。
                yield return ("坑煞", 血支);
            }
            {
                // 正月起寅逆行。
                yield return ("风煞", 某日月起某某行十二支(3, 3, 年月日时.月支, -1));
            }
            {
                // 正月起申逆行，天解同。
                var 风伯 = 某日月起某某行十二支(3, 9, 年月日时.月支, -1);
                yield return ("风伯", 风伯);
                yield return ("天解", 风伯);
            }
            Dizhi 月厌;
            {
                // 正月起戌逆行，对宫即厌对。
                月厌 = 某日月起某某行十二支(3, 11, 年月日时.月支, -1);
                yield return ("月厌", 月厌);
                yield return ("厌对", 月厌.Liuchong());
            }
            {
                // 同月厌。
                yield return ("火光", 月厌);
            }
            {
                // 正月起卯逆行。
                yield return ("烛命", 某日月起某某行十二支(3, 4, 年月日时.月支, -1));
            }
            {
                // 正月起酉逆行。
                yield return ("天鸡", 某日月起某某行十二支(3, 10, 年月日时.月支, -1));
            }
            {
                // 正七月起午，顺行六阳。
                yield return ("天马", 某日月起某某行十二支(9, 7, 年月日时.月支, 2));
            }
            {
                // 正七月起未，顺行六阴。
                yield return ("皇恩", 某日月起某某行十二支(9, 8, 年月日时.月支, 2));
            }
            {
                // 正七月起辰，顺行六阳。
                yield return ("天财", 某日月起某某行十二支(9, 5, 年月日时.月支, 2));
            }
            {
                // 阳月起丑顺行，阴月起未顺行。
                var 结果 = new Dizhi(年月日时.月支.Index switch
                {
                    3 => 2,
                    5 => 3,
                    7 => 4,
                    9 => 5,
                    11 => 6,
                    1 => 7,
                    4 => 8,
                    6 => 9,
                    8 => 10,
                    10 => 11,
                    12 => 12,
                    _ => 1,
                });
                yield return ("血忌", 结果);
            }
            {
                // 卯月起巳，午月起寅，酉月起亥，子月起申，俱顺行。
                var 结果 = new Dizhi(年月日时.月支.Index switch
                {
                    4 => 6,
                    5 => 7,
                    6 => 8,
                    7 => 3,
                    8 => 4,
                    9 => 5,
                    10 => 12,
                    11 => 1,
                    12 => 2,
                    1 => 9,
                    2 => 10,
                    _ => 11,
                });
                yield return ("飞廉", 结果);
            }
            Dizhi 勾神;
            {
                // 阳月起卯，隔月顺行六阴神。阴月起戌，隔月顺行六阳神。
                勾神 = new Dizhi(年月日时.月支.Index switch
                {
                    3 => 4,
                    5 => 6,
                    7 => 8,
                    9 => 10,
                    11 => 12,
                    1 => 2,
                    4 => 11,
                    6 => 1,
                    8 => 3,
                    10 => 5,
                    12 => 7,
                    _ => 9
                });
                yield return ("勾神", 勾神);
            }
            {
                // 勾神对宫。
                yield return ("绞神", 勾神.Liuchong());
            }
            #endregion
        }
    }

    #region 月煞
    public static 取神煞法 会神 => (式) =>
    {
        // 未 寅 酉 丑 巳 申
        // 戌 亥 子 午 卯 辰
        var 月支 = 式.年月日时.月支;
        return new(月支.Index switch
        {
            3 => 8,
            4 => 11,
            5 => 3,
            6 => 12,
            7 => 10,
            8 => 1,
            9 => 2,
            10 => 7,
            11 => 6,
            12 => 4,
            1 => 9,
            _ => 5
        });
    };
    public static 取神煞法 成神 => (式) =>
    {
        // 驿马合神。如正五九月马在申，巳与申合即是。
        var 月支 = 式.年月日时.月支;
        return 马(月支).Liuhe();
    };
    public static 取神煞法 天鬼 => (式) =>
    {
        // 驿马前一位神。
        var 月支 = 式.年月日时.月支;
        return 马(月支).Next();
    };
    public static 取神煞法 悬索 => (式) =>
    {
        // 天鬼对宫。
        var 鬼 = 天鬼(式);
        Debug.Assert(鬼.HasValue);
        return 鬼.Value.Liuchong();
    };
    public static 取神煞法 桃花 => (式) =>
    {
        // 同悬索。
        return 悬索(式);
    };
    public static 取神煞法 产煞 => (式) =>
    {
        // 阳月用驿马，阴月用马对宫。
        var 月支 = 式.年月日时.月支;
        return 月支.Yinyang().IsYang ? 马(月支) : 马(月支).Liuchong();
    };
    public static 取神煞法 大煞 => (式) =>
    {
        // 月德前一位。
        var 德 = 月德(式);
        Debug.Assert(德.HasValue);
        return 德.Value.Next();
    };
    public static 取神煞法 丧魄 => (式) =>
    {
        // 月德前二位。
        var 德 = 月德(式);
        Debug.Assert(德.HasValue);
        return 德.Value.Next(2);
    };
    #endregion

    #region 旬煞
    public static 取神煞法 三奇 => (式) =>
    {
        // 甲子甲戌旬在丑，甲申甲午旬在子，甲辰甲寅旬在亥。
        var 旬 = 式.年月日时.旬所在();
        return 旬.旬首.Index switch
        {
            1 or 11 => new(2),
            9 or 7 => new(1),
            5 or 3 => new(12),
            _ => null
        };
    };
    public static 取神煞法 六仪 => (式) =>
    {
        // 旬首之神。
        var 旬 = 式.年月日时.旬所在();
        return 旬.旬首;
    };
    public static 取神煞法 丁马 => (式) =>
    {
        // 六丁之神。
        var 旬 = 式.年月日时.旬所在();
        return 旬.获取对应地支(new Tiangan(4));
    };
    #endregion

    #region 时煞
    public static 取神煞法 天赦 => (式) =>
    {
        // 戊寅、甲午、戊申、甲子。
#warning 此戊、甲为何意？
        var 时 = 式.年月日时.月支.SanhuiRelation().DizhiOfMeng.Wuxing();
        return new((int)时 switch
        {
            0 => 3,
            1 => 7,
            3 => 9,
            _ => 1,
        });
    };
    public static 取神煞法 皇书 => (式) =>
    {
        // 四时临官之神，如春木临官在寅之类。
        var 时 = 式.年月日时.月支.SanhuiRelation();
        return 时.DizhiOfMeng.Wuxing().ShierZhangsheng(ShierZhangsheng.Linguan);
    };
    public static 取神煞法 孤辰 => (式) =>
    {
        // 四时前一位。
        var 时 = 式.年月日时.月支.SanhuiRelation();
        return 时.DizhiOfJi.Next();
    };
    public static 取神煞法 寡宿 => (式) =>
    {
        // 四时后一位，关神同。
        var 时 = 式.年月日时.月支.SanhuiRelation();
        return 时.DizhiOfMeng.Next(-1);
    };
    public static 取神煞法 关神 => (式) =>
    {
        return 寡宿(式);
    };
    public static 取神煞法 喝散 => (式) =>
    {
        // 喝散、钥神：同孤辰。
        return 孤辰(式);
    };
    public static 取神煞法 钥神 => (式) =>
    {
        return 孤辰(式);
    };
    public static 取神煞法 火鬼 => (式) =>
    {
        // 午酉子卯。
        var 时 = 式.年月日时.月支.SanhuiRelation().DizhiOfMeng.Wuxing();
        return new((int)时 switch
        {
            0 => 7,
            1 => 10,
            3 => 1,
            _ => 4,
        });
    };
    public static 取神煞法 丧车 => (式) =>
    {
        // 天喜后一位。
        var 喜 = 天喜(式);
        Debug.Assert(喜.HasValue);
        return 喜.Value.Next(-1);
    };
    public static 取神煞法 天喜 => (式) =>
    {
        // 戌丑辰未。
        var 时 = 式.年月日时.月支.SanhuiRelation().DizhiOfMeng.Wuxing();
        return new((int)时 switch
        {
            0 => 11,
            1 => 2,
            3 => 5,
            _ => 8,
        });
    };
    public static 取神煞法 天耳 => (式) =>
    {
        // 同天喜。
        return 天喜(式);
    };
    public static 取神煞法 浴盆 => (式) =>
    {
        // 天喜冲位。
        var 喜 = 天喜(式);
        Debug.Assert(喜.HasValue);
        return 喜.Value.Liuchong();
    };
    public static 取神煞法 天目 => (式) =>
    {
        // 同浴盆。
        return 浴盆(式);
    };
    public static 取神煞法 哭神 => (式) =>
    {
        // 未戌丑辰。
        var 时 = 式.年月日时.月支.SanhuiRelation().DizhiOfMeng.Wuxing();
        return new((int)时 switch
        {
            0 => 8,
            1 => 11,
            3 => 2,
            _ => 5,
        });
    };
    public static 取神煞法 五墓 => (式) =>
    {
        // 同哭神。
        return 哭神(式);
    };
    public static 取神煞法 游神 => (式) =>
    {
        // 丑子亥戌。
        var 时 = 式.年月日时.月支.SanhuiRelation().DizhiOfMeng.Wuxing();
        return new((int)时 switch
        {
            0 => 2,
            1 => 1,
            3 => 12,
            _ => 11,
        });
    };
    public static 取神煞法 戏神 => (式) =>
    {
        // 巳子酉辰。
        var 时 = 式.年月日时.月支.SanhuiRelation().DizhiOfMeng.Wuxing();
        return new((int)时 switch
        {
            0 => 6,
            1 => 1,
            3 => 10,
            _ => 5,
        });
    };
    #endregion

    #region 岁煞
    public static 取神煞法 大耗 => (式) =>
    {
        // 岁后一位。
        var 年支 = 式.年月日时.年支;
        return 年支.Next(-1);
    };
    public static 取神煞法 丧门 => (式) =>
    {
        // 岁前二位。
        var 年支 = 式.年月日时.年支;
        return 年支.Next(2);
    };
    public static 取神煞法 吊客 => (式) =>
    {
        // 岁后二位。
        var 年支 = 式.年月日时.年支;
        return 年支.Next(-2);
    };
    public static 取神煞法 岁墓 => (式) =>
    {
        // 岁后五位。
        var 年支 = 式.年月日时.年支;
        return 年支.Next(-5);
    };
    public static 取神煞法 岁虎 => (式) =>
    {
        // 岁墓前一位。
        var 墓 = 岁墓(式);
        Debug.Assert(墓.HasValue);
        return 墓.Value.Next(1);
    };
    #endregion
}
