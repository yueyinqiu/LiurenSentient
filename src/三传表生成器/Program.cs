using LrsCore.实体.壬式内容;
using LrsCore.实体.起课信息;
using System.Diagnostics;
using YiJingFramework.Nongli.Solar;
using YiJingFramework.PrimitiveTypes;
using 三传表生成器;

#region 检验
{
    // 癸酉 辰加子
    // 己卯 辰加子

    using StreamWriter 此次结果 = new StreamWriter($"./此次结果.txt", false);
    using StreamReader 正确三传 = new StreamReader($"./正确三传.txt");
    using StreamWriter 此次有误 = new StreamWriter($"./此次有误.txt", false);

    var 时干支 = Ganzhi.FromGanzhi(default, Dizhi.Zi);
    foreach (var 日 in Enumerable.Range(1, 60).Select(x => (Ganzhi)x))
    {
        foreach (var 月将 in Enumerable.Range(1, 12).Select(x => (Dizhi)x))
        {
            var 时 = new 年月日时(default, default, 日, 时干支, 月将, default);

            var 天地 = 天地盘.月上加时(时);
            var 课 = 四课.创建(时, 天地);
            var 三传 = new 三传涉害深浅计算(课, 天地);

            var str1 = $"{时.日.Tiangan:C}{时.日.Dizhi:C} {天地.取乘神(Dizhi.Zi):C}加{Dizhi.Zi:C}";
            var str2 = $"{三传.初传:C}{三传.中传:C}{三传.末传:C}";
            var str3 = "";

            此次结果.WriteLine(str1);
            此次结果.WriteLine(str2);
            此次结果.WriteLine(str3);

            if (str1 != 正确三传.ReadLine()
                | str2 != 正确三传.ReadLine()
                | str3 != 正确三传.ReadLine())
            {
                Console.WriteLine(str1);
                此次有误.WriteLine(str1);
                此次有误.WriteLine(str2);
                此次有误.WriteLine(str3);
            }
        }
    }
}
#endregion

Console.Write("检验已完成，要继续成表吗？");
_ = Console.ReadLine();

#region 成表
{
    static int 生成键(Ganzhi 日, Dizhi 子所乘)
    {
        Debug.Assert((int)日 * 100L + (int)子所乘 < int.MaxValue);
        return (int)日 * 100 + (int)子所乘;
    }

    using StreamWriter 输出 = new StreamWriter("成表.txt", false);
    输出.WriteLine("        private static (Dizhi 初, Dizhi 中, Dizhi 末) 获取三传(int 键)");
    输出.WriteLine("        {");
    输出.WriteLine("            return 键 switch");
    输出.WriteLine("            {");

    var 时干支 = Ganzhi.FromGanzhi(default, Dizhi.Zi);
    foreach (var 日 in Enumerable.Range(1, 60).Select(x => (Ganzhi)x))
    {
        foreach (var 子上 in Enumerable.Range(1, 12).Select(x => (Dizhi)x))
        {
            var 时 = new 年月日时(default, default, 日, 时干支, 子上, default);

            var 天地 = 天地盘.月上加时(时);
            var 课 = 四课.创建(时, 天地);
            var 三传 = new 三传涉害深浅计算(课, 天地);

            输出.Write("                ");
            输出.Write(生成键(时.日, 天地.取乘神(Dizhi.Zi)));
            输出.Write(" => (Dizhi.");
            输出.Write(三传.初传);
            输出.Write(", Dizhi.");
            输出.Write(三传.中传);
            输出.Write(", Dizhi.");
            输出.Write(三传.末传);
            输出.WriteLine("),");
        }
    }
    输出.WriteLine("                _ => (default, default, default)");
    输出.WriteLine("            };");
    输出.WriteLine("        }");
}
#endregion