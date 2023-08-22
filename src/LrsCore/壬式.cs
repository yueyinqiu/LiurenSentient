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
    public IReadOnlyList<年命> 年命 { get; }

    private 壬式(年月日时 年月日时, IEnumerable<本命信息> 本命信息)
    {
        this.天地盘 = 天地盘.月上加时(年月日时);
        this.四课 = 四课.取四课(年月日时, this.天地盘);
        this.三传 = 三传.依涉害深浅(this.天地盘, this.四课);
        this.天将盘 = 天将盘.甲戊庚牛羊(年月日时, this.天地盘);
        this.年命 = 本命信息
            .Select(本命 => 实体.壬式内容.年命.取年命(年月日时, 本命))
            .ToImmutableArray();
    }

    public static 壬式 起课(年月日时 年月日时, IEnumerable<本命信息> 本命信息)
    {
        return new 壬式(年月日时, 本命信息);
    }
}
