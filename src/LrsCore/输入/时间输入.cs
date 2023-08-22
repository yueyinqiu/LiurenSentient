using YiJingFramework.PrimitiveTypes;

namespace LrsCore.输入;
public sealed record 时间输入(
    Tiangan 年干, Dizhi 年支,
    Tiangan 月干, Dizhi 月支,
    Tiangan 日干, Dizhi 日支,
    Tiangan 时干, Dizhi 时支,
    日夜 昼夜, Dizhi 月将);
