using LrsCore;
using LrsCore.实体.壬式内容;
using LrsCore.实体.起课信息;
using System.Diagnostics;
using System.Drawing;
using System.Reflection;
using YiJingFramework.PrimitiveTypes;
using 三传表生成器;

#region 检验
{
    // 癸酉 辰加子
    // 己卯 辰加子

    using StreamWriter 此次结果 = new StreamWriter($"./此次结果.txt", false);
    using StreamReader 正确三传 = new StreamReader($"./正确三传.txt");
    using StreamWriter 此次有误 = new StreamWriter($"./此次有误.txt", false);

    var 时支 = Dizhi.Zi;
    foreach (var (日干, 日支) in Enumerable.Range(1, 60).Select(x => (new Tiangan(x), new Dizhi(x))))
    {
        foreach (var 月将 in Enumerable.Range(1, 12).Select(x => new Dizhi(x)))
        {
            var 时 = new 年月日时(
                default, default, default, default, 日干, 日支, default, 时支, default, 月将);

            var 天地盘 = typeof(天地盘)
                .GetConstructor(BindingFlags.NonPublic | BindingFlags.Instance, new[] { typeof(年月日时) })
                ?.Invoke(new object[] { 时 })
                as 天地盘;
            Debug.Assert(天地盘 is not null);
            var 四课 = typeof(四课)
                .GetConstructor(BindingFlags.NonPublic | BindingFlags.Instance, new[] { typeof(年月日时), typeof(天地盘) })
                ?.Invoke(new object[] { 时, 天地盘 })
                as 四课;
            Debug.Assert(四课 is not null);

            var 三传 = new 三传涉害深浅计算(四课, 天地盘);

            var str1 = $"{时.日干:C}{时.日支:C} {天地盘.取乘神(时支):C}加{时支:C}";
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
    static int 生成键(Tiangan 日, Dizhi 辰, Dizhi 子所乘)
    {
        Debug.Assert(日.Index * 10000L + 辰.Index * 100L + 子所乘.Index < int.MaxValue);
        return 日.Index * 10000 + 辰.Index * 100 + 子所乘.Index;
    }

    using StreamWriter 输出 = new StreamWriter("成表.txt", false);
    输出.WriteLine("        private static (int 初, int 中, int 末) 获取三传(int 键)");
    输出.WriteLine("        {");
    输出.WriteLine("            return 键 switch");
    输出.WriteLine("            {");

    var 子 = Dizhi.Zi;
    foreach (var (日干, 日支) in Enumerable.Range(1, 60).Select(x => (new Tiangan(x), new Dizhi(x))))
    {
        foreach (var 子上 in Enumerable.Range(1, 12).Select(x => new Dizhi(x)))
        {
            var 时 = new 年月日时(
                default, default, default, default, 日干, 日支, default, 子, default, 子上);

            var 天地盘 = typeof(天地盘)
                .GetConstructor(BindingFlags.NonPublic | BindingFlags.Instance, new[] { typeof(年月日时) })
                ?.Invoke(new object[] { 时 })
                as 天地盘;
            Debug.Assert(天地盘 is not null);
            var 四课 = typeof(四课)
                .GetConstructor(BindingFlags.NonPublic | BindingFlags.Instance, new[] { typeof(年月日时), typeof(天地盘) })
                ?.Invoke(new object[] { 时, 天地盘 })
                as 四课;
            Debug.Assert(四课 is not null);

            var 三传 = new 三传涉害深浅计算(四课, 天地盘);
            输出.Write("                ");
            输出.Write(生成键(四课.日, 四课.辰, 天地盘.取乘神(子)));
            输出.Write(" => (");
            输出.Write(三传.初传.Index);
            输出.Write(", ");
            输出.Write(三传.中传.Index);
            输出.Write(", ");
            输出.Write(三传.末传.Index);
            输出.WriteLine("),");
        }
    }
    输出.WriteLine("                _ => (0, 0, 0)");
    输出.WriteLine("            };");
    输出.WriteLine("        }");
}
#endregion