using YiJingFramework.Nongli.Solar;
using YiJingFramework.PrimitiveTypes;

namespace LrsCore.实体.起课信息;
public sealed record 年月日时(
    Ganzhi 年, Ganzhi 月, Ganzhi 日, Ganzhi 时, 
    Dizhi 将, 昼夜 昼夜)
{
    public 旬 旬所在()
    {
        return new 旬(this.日.Dizhi - this.日.Tiangan.Index + 1);
    }

    public sealed record 旬(Dizhi 旬首)
    {
        public (Dizhi, Dizhi) 旬空亡 => (this.旬首.Next(-2), this.旬首.Next(-1));

        public Tiangan? 获取对应天干(Dizhi 地支)
        {
            var (空亡一, 空亡二) = this.旬空亡;
            if (地支 == 空亡一 || 地支 == 空亡二)
                return null;
            return Tiangan.FromIndex((地支.Index - this.旬首.Index + 13) % 12);
        }
        public Dizhi 获取对应地支(Tiangan 天干)
        {
            return this.旬首 + 天干.Index - 1;
        }
    }
}