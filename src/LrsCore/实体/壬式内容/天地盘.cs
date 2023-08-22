using LrsCore.实体.起课信息;
using YiJingFramework.PrimitiveTypes;

namespace LrsCore.实体.壬式内容;
public sealed class 天地盘
{
    private readonly int 偏移;

    internal 天地盘(年月日时 年月日时)
    {
        偏移 = 年月日时.月将.Index - 年月日时.时支.Index;
    }

    public Dizhi 取临地(Dizhi 天盘支)
    {
        return 天盘支.Next(-偏移);
    }

    public Dizhi 取乘神(Dizhi 地盘支)
    {
        return 地盘支.Next(偏移);
    }
}
