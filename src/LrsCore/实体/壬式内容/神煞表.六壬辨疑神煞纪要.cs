﻿using LrsCore.实体.起课信息;
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
        static IEnumerable<(string, Dizhi?)> 取神煞法提供类的神煞(年月日时 年月日时)
        {
            var 壬式信息 = new 六壬辨疑神煞纪要提供类.壬式信息(年月日时);
            foreach (var 属性 in typeof(六壬辨疑神煞纪要提供类).GetProperties())
            {
                var 属性名 = 属性.Name;
                var 属性值 = 属性.GetValue(null);
                if (属性值 is 六壬辨疑神煞纪要提供类.取神煞法 单)
                {
                    var 地支 = 单.Invoke(壬式信息);
                    yield return (属性名, 地支);
                }
                else if (属性值 is 六壬辨疑神煞纪要提供类.取多神煞法 多)
                {
                    bool 没有此神煞 = true;
                    foreach (var 地支 in 多.Invoke(壬式信息))
                    {
                        没有此神煞 = false;
                        yield return (属性名, 地支);
                    }
                    if (没有此神煞)
                        yield return (属性名, null);
                }
            }
        }
        return new 神煞表(取神煞法提供类的神煞(年月日时));
    }

    private static class 六壬辨疑神煞纪要提供类
    {
        public record 壬式信息(年月日时 年月日时);

        public delegate Dizhi? 取神煞法(壬式信息 式);
        public delegate IEnumerable<Dizhi> 取多神煞法(壬式信息 式);

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
        #region 各神煞

        #region 干煞
        public static 取神煞法 日德 => (式) =>
        {
            // 阳干同禄神，阴干从官星之禄神。如乙以庚为官用申之类。
            var 日干 = 式.年月日时.日干;
            if (!日干.Yinyang().IsYang)
                日干 = 日干.Wuhe();
            return 日干.ShierZhangsheng(ShierZhangsheng.Linguan);
        };
        public static 取神煞法 日合 => (式) =>
        {
            // 日干对宫之神。如甲用己之类。
            var 日干 = 式.年月日时.日干;
            return 日干.Wuhe().Jigong();
        };
        public static 取神煞法 游都 => (式) =>
        {
            // 甲己在丑，乙庚在子，丙辛在寅，丁壬在巳，戊癸在申。
            return new Dizhi(
                式.年月日时.日干.Index switch
                {
                    1 or 6 => 2,
                    2 or 7 => 1,
                    3 or 8 => 3,
                    4 or 9 => 6,
                    _ => 9
                });
        };
        public static 取神煞法 鲁都 => (式) =>
        {
            // 即游都对宫之神。
            var 游 = 游都(式);
            Debug.Assert(游.HasValue);
            return 游.Value.Liuchong();
        };
        public static 取神煞法 干奇 => (式) =>
        {
            // 午巳辰卯寅丑未申酉戌
            return new Dizhi(
                式.年月日时.日干.Index switch
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
        };
        #endregion

        #region 支煞
        public static 取神煞法 支德 => (式) =>
        {
            // 子日起巳，顺行十二支。
            var 日支 = 式.年月日时.日支;
            return 某日月起某某行十二支(1, 6, 日支);
        };
        /*
        public static 取神煞法 六合 => (式) =>
        {
            // 子丑合，寅亥合，卯戌合，辰酉合，巳申合，午未合。
            var 日支 = 式.年月日时.日支;
            return 日支.取六合();
        };
        public static 取多神煞法 三合 => (式) =>
        {
            // 即五行生旺墓三宫之神。
            var 日支 = 式.年月日时.日支;
            return 日支.取所在三合局().ToArray();
        };
        public static 取多神煞法 三刑 => (式) =>
        {
            // 寅刑巳，巳刑申，申刑寅；丑刑戌，戌刑未，未刑丑，此为月刑。
            // 子刑卯，卯刑子，此为互刑。
            // 辰刑辰，亥刑亥，酉刑酉，午刑午，此为自刑。
            var 日支 = 式.年月日时.日支;
            return (new[] { 日支.取所刑(), 日支.取被所刑() }).Distinct().ToArray();
        };
        public static 取神煞法 六害 => (式) =>
        {
            // 子未害，午丑害，寅巳害，卯辰害，申亥害，酉戌害。
            var 日支 = 式.年月日时.日支;
            return 日支.取害();
        };
        */
        public static 取神煞法 支墓 => (式) =>
        {
            // 用五行起例。
            var 日支 = 式.年月日时.日支;
            return 日支.Wuxing().ShierZhangsheng(ShierZhangsheng.Mu);
        };
        public static 取神煞法 支破 => (式) =>
        {
            // 阳支后三神，阴支前三神。
            // 子 丑 寅 卯 辰 巳 午 未 申 酉 戌 亥
            // 戌 卯 子 丑 寅 未 辰 酉 午 亥 申 丑
            var 日支 = 式.年月日时.日支;
            return 日支.Liupo();
        };
        public static 取神煞法 华盖 => (式) =>
        {
            // 三合之第三位神。
            var 日支 = 式.年月日时.日支;
            return 日支.SanheRelation().DizhiOfMu;
        };
        public static 取神煞法 将星 => (式) =>
        {
            // 三合之第二位神。
            var 日支 = 式.年月日时.日支;
            return 日支.SanheRelation().DizhiOfDiwang;
        };
        public static 取神煞法 驿马 => (式) =>
        {
            // 三合第一位冲神。
            var 日支 = 式.年月日时.日支;
            return 马(日支);
        };
        public static 取神煞法 劫煞 => (式) =>
        {
            // 三合第三位之前一位神。
            var 日支 = 式.年月日时.日支;
            return 日支.SanheRelation().DizhiOfMu.Next();
        };
        public static 取神煞法 破碎 => (式) =>
        {
            // 孟日用酉，仲日用巳，季日用丑。即金神，又名红砂。
            var 日支 = 式.年月日时.日支;
            return new(日支.MengZhongJi() switch
            {
                MengZhongJi.Meng => 10,
                MengZhongJi.Zhong => 6,
                _ => 2,
            });
        };
        public static 取神煞法 支仪 => (式) =>
        {
            // 午 辰 寅 未 酉 亥
            // 巳 卯 丑 申 戌 子
            var 日支 = 式.年月日时.日支;
            return new(日支.Index switch
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
        };
        #endregion

        #region 月煞
        public static 取神煞法 天德 => (式) =>
        {
            // 正 二 三 四 五 六 七 八 九 十 十一 十二
            // 丁 坤 壬 辛 干 甲 癸 艮 丙 乙 巽  庚
            var 月支 = 式.年月日时.月支;
            return new(月支.Index switch
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
        };
        public static 取神煞法 月德 => (式) =>
        {
            // 巳寅亥申三轮，即三合之禄神。
            var 月支 = 式.年月日时.月支;
            return 月支.SanheRelation().DizhiOfDiwang.Wuxing().ShierZhangsheng(ShierZhangsheng.Linguan);
        };
        public static 取神煞法 生气 => (式) =>
        {
            // 正月起子顺行。
            var 月支 = 式.年月日时.月支;
            return 某日月起某某行十二支(3, 1, 月支);
        };
        public static 取神煞法 死气 => (式) =>
        {
            // 正月起午顺行。
            var 月支 = 式.年月日时.月支;
            return 某日月起某某行十二支(3, 7, 月支);
        };
        public static 取神煞法 官符 => (式) =>
        {
            // 官符、孝服、谩语：俱同死气。
            return 死气(式);
        };
        public static 取神煞法 孝服 => (式) =>
        {
            return 死气(式);
        };
        public static 取神煞法 谩语 => (式) =>
        {
            return 死气(式);
        };
        public static 取神煞法 死神 => (式) =>
        {
            // 正月起巳顺行。
            var 月支 = 式.年月日时.月支;
            return 某日月起某某行十二支(3, 6, 月支);
        };
        public static 取神煞法 火烛 => (式) =>
        {
            // 同死神。
            return 死神(式);
        };
        public static 取神煞法 天医 => (式) =>
        {
            // 正月起辰顺行。
            var 月支 = 式.年月日时.月支;
            return 某日月起某某行十二支(3, 5, 月支);
        };
        public static 取神煞法 地医 => (式) =>
        {
            // 正月起戌顺行。
            var 月支 = 式.年月日时.月支;
            return 某日月起某某行十二支(3, 11, 月支);
        };
        public static 取神煞法 天诏 => (式) =>
        {
            // 正月起亥顺行。
            var 月支 = 式.年月日时.月支;
            return 某日月起某某行十二支(3, 12, 月支);
        };
        public static 取神煞法 飞魂 => (式) =>
        {
            // 同天诏。
            return 天诏(式);
        };
        public static 取神煞法 信神 => (式) =>
        {
            // 正月起酉顺行。
            var 月支 = 式.年月日时.月支;
            return 某日月起某某行十二支(3, 10, 月支);
        };
        public static 取神煞法 血支 => (式) =>
        {
            // 正月起丑顺行。
            var 月支 = 式.年月日时.月支;
            return 某日月起某某行十二支(3, 2, 月支);
        };
        public static 取神煞法 坑煞 => (式) =>
        {
            // 同血支。
            return 血支(式);
        };
        public static 取神煞法 风煞 => (式) =>
        {
            // 正月起寅逆行。
            var 月支 = 式.年月日时.月支;
            return 某日月起某某行十二支(3, 3, 月支, -1);
        };
        public static 取神煞法 风伯 => (式) =>
        {
            // 正月起申逆行，天解同。
            var 月支 = 式.年月日时.月支;
            return 某日月起某某行十二支(3, 9, 月支, -1);
        };
        public static 取神煞法 天解 => (式) =>
        {
            return 风伯(式);
        };
        public static 取神煞法 月厌 => (式) =>
        {
            // 正月起戌逆行，对宫即厌对。
            var 月支 = 式.年月日时.月支;
            return 某日月起某某行十二支(3, 11, 月支, -1);
        };
        public static 取神煞法 厌对 => (式) =>
        {
            var 厌 = 月厌(式);
            Debug.Assert(厌.HasValue);
            return 厌.Value.Liuchong();
        };
        public static 取神煞法 火光 => (式) =>
        {
            // 同月厌。
            return 月厌(式);
        };
        public static 取神煞法 烛命 => (式) =>
        {
            // 正月起卯逆行。
            var 月支 = 式.年月日时.月支;
            return 某日月起某某行十二支(3, 4, 月支, -1);
        };
        public static 取神煞法 天鸡 => (式) =>
        {
            // 正月起酉逆行。
            var 月支 = 式.年月日时.月支;
            return 某日月起某某行十二支(3, 10, 月支, -1);
        };
        public static 取神煞法 天马 => (式) =>
        {
            // 正七月起午，顺行六阳。
            var 月支 = 式.年月日时.月支;
            return 某日月起某某行十二支(9, 7, 月支, 2);
        };
        public static 取神煞法 皇恩 => (式) =>
        {
            // 正七月起未，顺行六阴。
            var 月支 = 式.年月日时.月支;
            return 某日月起某某行十二支(9, 8, 月支, 2);
        };
        public static 取神煞法 天财 => (式) =>
        {
            // 正七月起辰，顺行六阳。
            var 月支 = 式.年月日时.月支;
            return 某日月起某某行十二支(9, 5, 月支, 2);
        };
        public static 取神煞法 血忌 => (式) =>
        {
            // 阳月起丑顺行，阴月起未顺行。
            var 月支 = 式.年月日时.月支;
            return new(月支.Index switch
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
        };
        public static 取神煞法 飞廉 => (式) =>
        {
            // 卯月起巳，午月起寅，酉月起亥，子月起申，俱顺行。
            var 月支 = 式.年月日时.月支;
            return new(月支.Index switch
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
        };
        public static 取神煞法 勾神 => (式) =>
        {
            // 阳月起卯，隔月顺行六阴神。阴月起戌，隔月顺行六阳神。
            var 月支 = 式.年月日时.月支;
            return new(月支.Index switch
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
        };
        public static 取神煞法 绞神 => (式) =>
        {
            // 勾神对宫。
            var 勾 = 勾神(式);
            Debug.Assert(勾.HasValue);
            return 勾.Value.Liuchong();
        };
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
        /*
        public static 取多神煞法 旬空 => (式) => {
            // 十干不到之处。
            var 旬 = 式.年月日时.旬所在();
            var (空亡一, 空亡二) = 旬.旬空亡;
            return new[] { 空亡一, 空亡二 };
        };
        */
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
        #endregion 各神煞
    }
}
