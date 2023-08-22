using LrsCore.实体.壬式内容;
using LrsCore.实体.起课信息;
using System.Collections.Immutable;

namespace LrsCore;
public sealed class 壬式
{
    public 天地盘 天地盘 { get; }
    public 四课 四课 { get; }
    public 三传 三传 { get; }
    public 天将盘 天将盘 { get; }
    public 神煞表 神煞表 { get; }
    public 课体表 课体表 { get; }
    public IReadOnlyList<年命> 年命表 { get; }

    private 壬式(年月日时 年月日时, IEnumerable<本命信息> 本命信息表)
    {
        this.天地盘 = 天地盘.月上加时(年月日时);
        this.四课 = 四课.创建(年月日时, this.天地盘);
        this.三传 = 三传.依涉害深浅(this.天地盘, this.四课);
        this.天将盘 = 天将盘.甲戊庚牛羊(年月日时, this.天地盘);
        this.年命表 = 本命信息表
            .Select(本命 => 年命.创建(年月日时, 本命))
            .ToImmutableArray();
        this.神煞表 = 神煞表.六壬辨疑神煞纪要(年月日时);
        this.课体表 = 课体表.六壬辨疑卷二(this.四课);
    }

    public static 壬式 起课(年月日时 年月日时, IEnumerable<本命信息> 本命信息表)
    {
        return new 壬式(年月日时, 本命信息表);
    }
}
