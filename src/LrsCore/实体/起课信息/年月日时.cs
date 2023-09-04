using YiJingFramework.PrimitiveTypes;

namespace LrsCore.实体.起课信息;
public sealed record 年月日时(
    Tiangan 年干, Dizhi 年支,
    Tiangan 月干, Dizhi 月支,
    Tiangan 日干, Dizhi 日支,
    Tiangan 时干, Dizhi 时支,
    昼夜 昼夜, Dizhi 月将)
{
    public 旬 旬所在()
    {
        return new 旬(this.日支 - (int)this.日干 + 1);
    }

    public sealed record 旬(Dizhi 旬首)
    {
        public (Dizhi, Dizhi) 旬空亡 => (this.旬首.Next(-2), this.旬首.Next(-1));

        public Tiangan? 获取对应天干(Dizhi 地支)
        {
            var (空亡一, 空亡二) = this.旬空亡;
            if (地支 == 空亡一 || 地支 == 空亡二)
                return null;
            return (Tiangan)(((int)地支 - (int)this.旬首 + 13) % 12);
        }
        public Dizhi 获取对应地支(Tiangan 天干)
        {
            return (Dizhi)(this.旬首 + (int)天干 - 1);
        }
    }
}