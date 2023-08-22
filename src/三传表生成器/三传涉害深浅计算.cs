using LrsCore.实体.壬式内容;
using YiJingFramework.PrimitiveTypes;

namespace 三传表生成器;
internal sealed partial class 三传涉害深浅计算
{
    private readonly 四课 四课;
    private readonly 天地盘 天地盘;
    internal 三传涉害深浅计算(四课 四课, 天地盘 天地盘)
    {
        this.四课 = 四课;
        this.天地盘 = 天地盘;
        this.重新计算();
    }

    public Dizhi 初传 { get; private set; }
    public Dizhi 中传 { get; private set; }
    public Dizhi 末传 { get; private set; }
}
