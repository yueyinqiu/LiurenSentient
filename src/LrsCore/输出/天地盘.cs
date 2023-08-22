using LrsCore.输入;
using YiJingFramework.PrimitiveTypes;

namespace LrsCore.输出;
public sealed class 天地盘
{
    private readonly int offset;

    public 天地盘(时间输入 时间)
    {
        this.offset = 时间.月将.Index - 时间.时支.Index;
    }

    public Dizhi Below(Dizhi zhi)
    {
        return zhi.Next(-this.offset);
    }

    public Dizhi Above(Dizhi zhi)
    {
        return zhi.Next(this.offset);
    }
}
