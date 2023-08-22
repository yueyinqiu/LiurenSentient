using System.Collections.Immutable;
using YiJingFramework.PrimitiveTypes;

namespace LrsCore.实体.壬式内容;
public sealed partial class 课体表
{
    public IReadOnlyList<string> 课体 { get; }

    private 课体表(IEnumerable<string> 课体表)
    {
        this.课体 = 课体表.ToImmutableList();
    }
}
