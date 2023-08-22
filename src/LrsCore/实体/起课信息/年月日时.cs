using YiJingFramework.PrimitiveTypes;

namespace LrsCore.实体.起课信息;
public sealed record 年月日时(
    Tiangan 年干, Dizhi 年支,
    Tiangan 月干, Dizhi 月支,
    Tiangan 日干, Dizhi 日支,
    Tiangan 时干, Dizhi 时支,
    昼夜 昼夜, Dizhi 月将);
