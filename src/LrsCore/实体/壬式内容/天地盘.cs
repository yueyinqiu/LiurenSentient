using LrsCore.实体.起课信息;
using YiJingFramework.PrimitiveTypes;

namespace LrsCore.实体.壬式内容;
public sealed class 天地盘
{
    private readonly int 偏移;

    public Dizhi 取临地(Dizhi 天盘支)
    {
        return 天盘支.Next(-this.偏移);
    }

    public Dizhi 取乘神(Dizhi 地盘支)
    {
        return 地盘支.Next(this.偏移);
    }

    private 天地盘(年月日时 年月日时)
    {
        this.偏移 = (int)年月日时.将 - (int)年月日时.时.Dizhi;
    }
    public static 天地盘 月上加时(年月日时 年月日时)
    {
        return new(年月日时);
    }
}
