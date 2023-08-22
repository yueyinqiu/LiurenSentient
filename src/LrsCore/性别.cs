using System.Diagnostics.CodeAnalysis;
using System.Numerics;

namespace LrsCore;

public readonly struct 性别 :
    IComparable<性别>,
    IEquatable<性别>,
    IEqualityOperators<性别, 性别, bool>
{
    public bool 为男 { get; }
    public 性别(bool 为男)
    {
        this.为男 = 为男;
    }
    public static 性别 男 => new(true);
    public static 性别 女 => new(false);
    public int CompareTo(性别 另一性别)
    {
        return this.为男.CompareTo(另一性别.为男);
    }
    public bool Equals(性别 另一性别)
    {
        return this.为男.Equals(另一性别.为男);
    }

    public override bool Equals([NotNullWhen(true)] object? 另一对象)
    {
        if (另一对象 is 性别 性别)
            return this.为男.Equals(性别.为男);
        return false;
    }

    public override string ToString()
    {
        return this.为男 ? "男" : "女";
    }

    public static explicit operator bool(性别 性别)
    {
        return 性别.为男;
    }

    public static explicit operator 性别(bool 值)
    {
        return new(值);
    }

    public override int GetHashCode()
    {
        return this.为男.GetHashCode();
    }

    public static bool operator ==(性别 左, 性别 右)
    {
        return 左.为男 == 右.为男;
    }
    public static bool operator !=(性别 左, 性别 右)
    {
        return 左.为男 != 右.为男;
    }
}