using System.Diagnostics.CodeAnalysis;
using System.Numerics;

namespace LrsCore;

public readonly struct 昼夜 :
    IComparable<昼夜>,
    IEquatable<昼夜>,
    IEqualityOperators<昼夜, 昼夜, bool>
{
    public bool 为昼 { get; }
    public 昼夜(bool 为昼)
    {
        this.为昼 = 为昼;
    }
    public static 昼夜 昼 => new(true);
    public static 昼夜 夜 => new(false);
    public int CompareTo(昼夜 另一昼夜)
    {
        return this.为昼.CompareTo(另一昼夜.为昼);
    }
    public bool Equals(昼夜 另一昼夜)
    {
        return this.为昼.Equals(另一昼夜.为昼);
    }

    public override bool Equals([NotNullWhen(true)] object? 另一对象)
    {
        if (另一对象 is 昼夜 昼夜)
            return this.为昼.Equals(昼夜.为昼);
        return false;
    }

    public override string ToString()
    {
        return this.为昼 ? "昼" : "夜";
    }

    public static explicit operator bool(昼夜 昼夜)
    {
        return 昼夜.为昼;
    }

    public static explicit operator 昼夜(bool 值)
    {
        return new(值);
    }

    public override int GetHashCode()
    {
        return this.为昼.GetHashCode();
    }

    public static bool operator ==(昼夜 左, 昼夜 右)
    {
        return 左.为昼 == 右.为昼;
    }
    public static bool operator !=(昼夜 左, 昼夜 右)
    {
        return 左.为昼 != 右.为昼;
    }
}