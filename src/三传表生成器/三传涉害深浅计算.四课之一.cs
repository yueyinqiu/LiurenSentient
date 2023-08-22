
using YiJingFramework.EntityRelations.EntityCharacteristics.Extensions;
using YiJingFramework.EntityRelations.TianganJigongs.Extensions;
using YiJingFramework.PrimitiveTypes;

namespace 三传表生成器;
internal partial class 三传涉害深浅计算
{
    private sealed class 四课之一
    {
        internal Tiangan? 干下 { get; }
        internal Dizhi 支下或干下之寄宫 { get; }
        internal Wuxing 下五行 { get; }
        internal Yinyang 下阴阳 { get; }
        internal Dizhi 上 { get; }
        internal Wuxing 上五行 { get; }
        internal Yinyang 上阴阳 { get; }
        internal 四课之一(Dizhi 下, Dizhi 上)
        {
            this.上 = 上;
            this.上五行 = 上.Wuxing();
            this.上阴阳 = 上.Yinyang();

            this.干下 = null;
            this.支下或干下之寄宫 = 下;
            this.下五行 = 下.Wuxing();
            this.下阴阳 = 下.Yinyang();
        }
        internal 四课之一(Tiangan 下, Dizhi 上)
        {
            this.上 = 上;
            this.上五行 = 上.Wuxing();
            this.上阴阳 = 上.Yinyang();

            this.干下 = 下;
            this.支下或干下之寄宫 = 下.Jigong();
            this.下五行 = 下.Wuxing();
            this.下阴阳 = 下.Yinyang();
        }
    }
}
