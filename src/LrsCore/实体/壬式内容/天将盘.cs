using LrsCore.实体.起课信息;
using YiJingFramework.PrimitiveTypes;

namespace LrsCore.实体.壬式内容;
public sealed class 天将盘
{
    private readonly Dizhi 贵人;
    private readonly bool 顺行;
    private 天将盘(Dizhi 贵人, bool 顺行)
    {
        this.贵人 = 贵人;
        this.顺行 = 顺行;
    }

    public static 天将盘 甲戊庚牛羊(年月日时 年月日时, 天地盘 天地盘)
    {
        var 贵人 = (Dizhi)((int)年月日时.日干 switch
        {
            // 甲戊庚牛羊
            1 or 5 or 7 => 年月日时.昼夜.为昼 ? 2 : 8,
            // 乙己鼠猴乡
            2 or 6 => 年月日时.昼夜.为昼 ? 1 : 9,
            // 丙丁猪鸡位
            3 or 4 => 年月日时.昼夜.为昼 ? 12 : 10,
            // 壬癸蛇兔藏
            9 or 10 => 年月日时.昼夜.为昼 ? 6 : 4,
            // 六辛逢马虎
            _ => 年月日时.昼夜.为昼 ? 7 : 3 // 7 or 8
            // 此是贵人方
        });
        var 临神 = 天地盘.取临地(贵人);
        var 顺行 = (int)临神 is 12 or 1 or 2 or 3 or 4 or 5;
        return new(贵人, 顺行);
    }

    public 天将 取乘将(Dizhi 地支)
    {
        int 方向 = this.顺行 ? 1 : -1;
        var 差异 = (int)地支 - (int)this.贵人;
        return (天将)(方向 * 差异);
    }

    public Dizhi 取乘神(天将 天将)
    {
        int 方向 = this.顺行 ? 1 : -1;
        return this.贵人.Next(方向 * (int)天将);
    }
}
