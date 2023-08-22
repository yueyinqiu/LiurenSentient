using LrsCore.实体.起课信息;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YiJingFramework.EntityRelations.TianganJigongs.Extensions;
using YiJingFramework.PrimitiveTypes;

namespace LrsCore.实体.壬式内容;
public sealed class 四课
{
    internal 四课(年月日时 年月日时, 天地盘 天地盘)
    {
        this.日 = 年月日时.日干;
        this.日阳 = 天地盘.取乘神(this.日.Jigong());
        this.日阴 = 天地盘.取乘神(this.日阳);

        this.辰 = 年月日时.日支;
        this.辰阳 = 天地盘.取乘神(this.辰);
        this.辰阴 = 天地盘.取乘神(this.辰阳);
    }

    public Tiangan 日 { get; }
    public Dizhi 日阳 { get; }
    public Dizhi 日阴 { get; }
    public Dizhi 辰 { get; }
    public Dizhi 辰阳 { get; }
    public Dizhi 辰阴 { get; }
}
