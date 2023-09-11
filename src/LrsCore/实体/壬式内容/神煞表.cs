using System.Collections.Immutable;
using YiJingFramework.PrimitiveTypes;

namespace LrsCore.实体.壬式内容;
public sealed partial class 神煞表
{
    private readonly ImmutableDictionary<Dizhi, List<string>> 神煞字典;

    public IEnumerable<string> 取神煞(Dizhi dizhi)
    {
        return this.神煞字典[dizhi].AsReadOnly();
    }

    private 神煞表(IEnumerable<(string, Dizhi?)> 神煞表)
    {
        this.神煞字典 = Enumerable.Range(1, 12)
            .ToImmutableDictionary(Dizhi.FromIndex, _ => new List<string>());

        foreach (var (神煞名, 地支) in 神煞表)
        {
            if (地支.HasValue)
                this.神煞字典[地支.Value].Add(神煞名);
        }
    }
}
