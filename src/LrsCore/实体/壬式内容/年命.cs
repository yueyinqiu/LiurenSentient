using LrsCore.实体.起课信息;
using YiJingFramework.PrimitiveTypes;

namespace LrsCore.实体.壬式内容;
public sealed class 年命
{
    public 性别 性别 { get; }
    public Dizhi 本命 { get; }
    public Dizhi 行年 { get; }

    private 年命(年月日时 年月日时, 本命信息 本命信息)
    {
        this.性别 = 本命信息.性别;
        this.本命 = 本命信息.本命;

        this.行年 = 本命信息.性别.为男 ?
            Dizhi.Yin.Next((int)年月日时.年支 - (int)this.本命) :
            Dizhi.Shen.Next((int)this.本命 - (int)年月日时.年支);
    }

    public static 年命 创建(年月日时 年月日时, 本命信息 本命信息)
    {
        return new(年月日时, 本命信息);
    }
}
