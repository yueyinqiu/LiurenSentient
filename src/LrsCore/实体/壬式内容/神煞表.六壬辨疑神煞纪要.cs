using LrsCore.实体.起课信息;
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
            int 所行 = (int)所至 - 某日月;
            return (Dizhi)(起某 + 所行 * 步长);
        }

        public static IEnumerable<(string, Dizhi?)> 取神煞(年月日时 年月日时)
        {
            #region 干煞
            {
                // 阳干同禄神，阴干从官星之禄神。如乙以庚为官用申之类。
                var 日干 = 年月日时.日.Tiangan;
                if (!日干.Yinyang().IsYang)
                    日干 = 日干.Wuhe();
                yield return ("日德", 日干.ShierZhangsheng(ShierZhangsheng.Linguan));
            }
            {
                // 日干对宫之神。如甲用己之类。
                yield return ("日合", 年月日时.日.Tiangan.Wuhe().Jigong());
            }
            Dizhi 游都;
            {
                // 甲己在丑，乙庚在子，丙辛在寅，丁壬在巳，戊癸在申。
                游都 = (int)年月日时.日.Tiangan switch
                {
                    1 or 6 => Dizhi.Chou,
                    2 or 7 => Dizhi.Zi,
                    3 or 8 => Dizhi.Yin,
                    4 or 9 => Dizhi.Si,
                    _ => Dizhi.Shen
                };
                yield return ("游都", 游都);
            }
            {
                // 即游都对宫之神。
                yield return ("鲁都", 游都.Liuchong());
            }
            {
                // 午巳辰卯寅丑未申酉戌
                var 结果 = (int)年月日时.日.Tiangan switch
                {
                    1 => Dizhi.Wu,
                    2 => Dizhi.Si,
                    3 => Dizhi.Chen,
                    4 => Dizhi.Mao,
                    5 => Dizhi.Yin,
                    6 => Dizhi.Chou,
                    7 => Dizhi.Wei,
                    8 => Dizhi.Shen,
                    9 => Dizhi.You,
                    _ => Dizhi.Xu
                };
                yield return ("干奇", 结果);
            }
            #endregion

            #region 支煞
            {
                // 子日起巳，顺行十二支。
                yield return ("支德", 某日月起某某行十二支(1, 6, 年月日时.日.Dizhi));
            }
            {
                // 用五行起例。
                yield return ("支墓", 年月日时.日.Dizhi.Wuxing().ShierZhangsheng(ShierZhangsheng.Mu));
            }
            {
                // 阳支后三神，阴支前三神。
                // 子 丑 寅 卯 辰 巳 午 未 申 酉 戌 亥
                // 戌 卯 子 丑 寅 未 辰 酉 午 亥 申 丑
                yield return ("支破", 年月日时.日.Dizhi.Liupo());
            }
            {
                // 三合之第三位神。
                yield return ("华盖", 年月日时.日.Dizhi.SanheRelation().DizhiOfMu);
            }
            {
                // 三合之第二位神。
                yield return ("将星", 年月日时.日.Dizhi.SanheRelation().DizhiOfDiwang);
            }
            {
                // 三合第一位冲神。
                yield return ("驿马", 年月日时.日.Dizhi.SanheRelation().DizhiOfZhangsheng.Liuchong());
            }
            {
                // 三合第三位之前一位神。
                yield return ("劫煞", 年月日时.日.Dizhi.SanheRelation().DizhiOfMu.Next());
            }
            {
                // 孟日用酉，仲日用巳，季日用丑。即金神，又名红砂。
                var 结果 = 年月日时.日.Dizhi.MengZhongJi() switch
                {
                    MengZhongJi.Meng => Dizhi.You,
                    MengZhongJi.Zhong => Dizhi.Si,
                    _ => Dizhi.Chou,
                };
                yield return ("破碎", 结果);
            }
            {
                // 午 辰 寅 未 酉 亥
                // 巳 卯 丑 申 戌 子
                var 结果 = (int)年月日时.日.Dizhi switch
                {
                    1 => Dizhi.Wu,
                    2 => Dizhi.Si,
                    3 => Dizhi.Chen,
                    4 => Dizhi.Mao,
                    5 => Dizhi.Yin,
                    6 => Dizhi.Chou,
                    7 => Dizhi.Wei,
                    8 => Dizhi.Shen,
                    9 => Dizhi.You,
                    10 => Dizhi.Xu,
                    11 => Dizhi.Hai,
                    _ => Dizhi.Zi
                };
                yield return ("支仪", 结果);
            }
            #endregion

            #region 月煞
            {
                // 正 二 三 四 五 六 七 八 九 十 十一 十二
                // 丁 坤 壬 辛 干 甲 癸 艮 丙 乙 巽  庚
                var 结果 = (Dizhi)((int)年月日时.月.Dizhi switch
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
            Dizhi 月德;
            {
                // 巳寅亥申三轮，即三合之禄神。
                var 月支 = 年月日时.月.Dizhi;
                var 三合五行 = 月支.SanheRelation().DizhiOfDiwang.Wuxing();
                月德 = 三合五行.ShierZhangsheng(ShierZhangsheng.Linguan);
                yield return ("月德", 月德);
            }
            {
                // 正月起子顺行。
                yield return ("生气", 某日月起某某行十二支(3, 1, 年月日时.月.Dizhi));
            }
            Dizhi 死气;
            {
                // 正月起午顺行。
                死气 = 某日月起某某行十二支(3, 7, 年月日时.月.Dizhi);
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
                死神 = 某日月起某某行十二支(3, 6, 年月日时.月.Dizhi);
                yield return ("死神", 死神);
            }
            {
                // 同死神。
                yield return ("火烛", 死神);
            }
            {
                // 正月起辰顺行。
                yield return ("天医", 某日月起某某行十二支(3, 5, 年月日时.月.Dizhi));
            }
            {
                // 正月起戌顺行。
                yield return ("地医", 某日月起某某行十二支(3, 11, 年月日时.月.Dizhi));
            }
            Dizhi 天诏;
            {
                // 正月起亥顺行。
                天诏 = 某日月起某某行十二支(3, 12, 年月日时.月.Dizhi);
                yield return ("天诏", 天诏);
            }
            {
                // 同天诏。
                yield return ("飞魂", 天诏);
            }
            {
                // 正月起酉顺行。
                yield return ("信神", 某日月起某某行十二支(3, 10, 年月日时.月.Dizhi));
            }
            Dizhi 血支;
            {
                // 正月起丑顺行。
                血支 = 某日月起某某行十二支(3, 2, 年月日时.月.Dizhi);
                yield return ("血支", 血支);
            }
            {
                // 同血支。
                yield return ("坑煞", 血支);
            }
            {
                // 正月起寅逆行。
                yield return ("风煞", 某日月起某某行十二支(3, 3, 年月日时.月.Dizhi, -1));
            }
            {
                // 正月起申逆行，天解同。
                var 风伯 = 某日月起某某行十二支(3, 9, 年月日时.月.Dizhi, -1);
                yield return ("风伯", 风伯);
                yield return ("天解", 风伯);
            }
            Dizhi 月厌;
            {
                // 正月起戌逆行，对宫即厌对。
                月厌 = 某日月起某某行十二支(3, 11, 年月日时.月.Dizhi, -1);
                yield return ("月厌", 月厌);
                yield return ("厌对", 月厌.Liuchong());
            }
            {
                // 同月厌。
                yield return ("火光", 月厌);
            }
            {
                // 正月起卯逆行。
                yield return ("烛命", 某日月起某某行十二支(3, 4, 年月日时.月.Dizhi, -1));
            }
            {
                // 正月起酉逆行。
                yield return ("天鸡", 某日月起某某行十二支(3, 10, 年月日时.月.Dizhi, -1));
            }
            {
                // 正七月起午，顺行六阳。
                yield return ("天马", 某日月起某某行十二支(9, 7, 年月日时.月.Dizhi, 2));
            }
            {
                // 正七月起未，顺行六阴。
                yield return ("皇恩", 某日月起某某行十二支(9, 8, 年月日时.月.Dizhi, 2));
            }
            {
                // 正七月起辰，顺行六阳。
                yield return ("天财", 某日月起某某行十二支(9, 5, 年月日时.月.Dizhi, 2));
            }
            {
                // 阳月起丑顺行，阴月起未顺行。
                var 结果 = (Dizhi)((int)年月日时.月.Dizhi switch
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
                var 结果 = (Dizhi)((int)年月日时.月.Dizhi switch
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
                勾神 = (Dizhi)((int)年月日时.月.Dizhi switch
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
            {
                // 未 寅 酉 丑 巳 申
                // 戌 亥 子 午 卯 辰
                var 结果 = (int)年月日时.月.Dizhi switch
                {
                    3 => Dizhi.Wei,
                    4 => Dizhi.Xu,
                    5 => Dizhi.Yin,
                    6 => Dizhi.Hai,
                    7 => Dizhi.You,
                    8 => Dizhi.Zi,
                    9 => Dizhi.Chou,
                    10 => Dizhi.Wu,
                    11 => Dizhi.Si,
                    12 => Dizhi.Mao,
                    1 => Dizhi.Shen,
                    _ => Dizhi.Chen
                };
                yield return ("会神", 结果);
            }
            var 月马 = 年月日时.月.Dizhi.SanheRelation().DizhiOfZhangsheng.Liuchong();
            {
                // 驿马合神。如正五九月马在申，巳与申合即是。
                yield return ("成神", 月马.Liuhe());
            }
            Dizhi 天鬼;
            {
                // 驿马前一位神。
                天鬼 = 月马.Next();
                yield return ("天鬼", 天鬼);
            }
            Dizhi 悬索;
            {
                // 天鬼对宫。
                悬索 = 天鬼.Liuchong();
                yield return ("悬索", 悬索);
            }
            {
                // 同悬索。
                yield return ("桃花", 悬索);
            }
            {
                // 阳月用驿马，阴月用马对宫。
                var 阳月 = 年月日时.月.Dizhi.Yinyang().IsYang;
                yield return ("产煞", 阳月 ? 月马 : 月马.Liuchong());
            }
            {
                // 月德前一位。
                yield return ("大煞", 月德.Next());
            }
            {
                // 月德前二位。
                yield return ("丧魄", 月德.Next(2));
            }
            #endregion

            #region 旬煞
            var 旬 = 年月日时.旬所在();
            {
                // 甲子甲戌旬在丑，甲申甲午旬在子，甲辰甲寅旬在亥。
                Dizhi? 结果 = (int)旬.旬首 switch
                {
                    1 or 11 => Dizhi.Chou,
                    9 or 7 => Dizhi.Zi,
                    5 or 3 => Dizhi.Hai,
                    _ => null
                };
                yield return ("三奇", 结果);
            }
            {
                // 旬首之神。
                yield return ("六仪", 旬.旬首);
            }
            {
                // 六丁之神。
                yield return ("丁马", 旬.获取对应地支((Tiangan)4));
            }
            #endregion

            #region 时煞
            var 四时 = 年月日时.月.Dizhi.SanhuiRelation();
            var 四时孟数 = (int)四时.DizhiOfMeng; // 寅3 巳6 申9 亥12
            {
                // 戊寅、甲午、戊申、甲子。
#warning 此戊、甲为何意？
                var 结果 = 四时孟数 switch
                {
                    3 => Dizhi.Yin,
                    6 => Dizhi.Wu,
                    9 => Dizhi.Shen,
                    _ => Dizhi.Zi,
                };
                yield return ("天赦", 结果);
            }
            {
                // 四时临官之神，如春木临官在寅之类。
                yield return ("皇书", 四时.DizhiOfMeng.Wuxing().ShierZhangsheng(ShierZhangsheng.Linguan));
            }
            Dizhi 孤辰;
            {
                // 四时前一位。
                孤辰 = 四时.DizhiOfJi.Next();
                yield return ("孤辰", 孤辰);
            }
            {
                // 四时后一位，关神同。
                var 结果 = 四时.DizhiOfMeng.Next(-1);
                yield return ("寡宿", 结果);
                yield return ("关神", 结果);
            }
            {
                // 喝散、钥神：同孤辰。
                yield return ("喝散", 孤辰);
                yield return ("钥神", 孤辰);
            }
            {
                // 午酉子卯。
                var 结果 = 四时孟数 switch
                {
                    3 => Dizhi.Wu,
                    6 => Dizhi.You,
                    9 => Dizhi.Zi,
                    _ => Dizhi.Mao,
                };
                yield return ("火鬼", 结果);
            }
            var 天喜 = 四时孟数 switch
            {
                3 => Dizhi.Xu,
                6 => Dizhi.Chou,
                9 => Dizhi.Chen,
                _ => Dizhi.Wei,
            };
            {
                // 天喜后一位。
                yield return ("丧车", 天喜.Next(-1));
            }
            {
                // 戌丑辰未。
                yield return ("天喜", 天喜);
            }
            {
                // 同天喜。
                yield return ("天耳", 天喜);
            }
            Dizhi 浴盆;
            {
                // 天喜冲位。
                浴盆 = 天喜.Liuchong();
                yield return ("浴盆", 浴盆);
            }
            {
                // 同浴盆。
                yield return ("天目", 浴盆);
            }
            Dizhi 哭神;
            {
                // 未戌丑辰。
                哭神 = 四时孟数 switch
                {
                    3 => Dizhi.Wei,
                    6 => Dizhi.Xu,
                    9 => Dizhi.Chou,
                    _ => Dizhi.Chen,
                };
                yield return ("哭神", 哭神);
            }
            {
                // 同哭神。
                yield return ("五墓", 哭神);
            }
            {
                // 丑子亥戌。
                var 结果 = 四时孟数 switch
                {
                    3 => Dizhi.Chou,
                    6 => Dizhi.Zi,
                    9 => Dizhi.Hai,
                    _ => Dizhi.Xu,
                };
                yield return ("游神", 结果);
            }
            {
                // 巳子酉辰。
                var 结果 = 四时孟数 switch
                {
                    3 => Dizhi.Si,
                    6 => Dizhi.Zi,
                    9 => Dizhi.You,
                    _ => Dizhi.Chen,
                };
                yield return ("戏神", 结果);
            }
            #endregion

            #region 岁煞
            {
                // 岁后一位。
                yield return ("大耗", 年月日时.年.Dizhi.Next(-1));
            }
            {
                // 岁前二位。
                yield return ("丧门", 年月日时.年.Dizhi.Next(2));
            }
            {
                // 岁后二位。
                yield return ("吊客", 年月日时.年.Dizhi.Next(-2));
            }
            Dizhi 岁墓;
            {
                // 岁后五位。
                岁墓 = 年月日时.年.Dizhi.Next(-5);
                yield return ("岁墓", 岁墓);
            }
            {
                // 岁墓前一位。
                yield return ("岁虎", 岁墓.Next(1));
            }
            #endregion
        }
    }
}
