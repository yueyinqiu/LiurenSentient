using YiJingFramework.PrimitiveTypes;

namespace LrsCore.实体.壬式内容;
public sealed partial class 三传
{
    private 三传(Dizhi 初传, Dizhi 中传, Dizhi 末传)
    {
        this.初传 = 初传;
        this.中传 = 中传;
        this.末传 = 末传;
    }

    public Dizhi 初传 { get; }
    public Dizhi 中传 { get; }
    public Dizhi 末传 { get; }
}
