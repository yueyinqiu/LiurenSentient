using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;

namespace LrsCore;
public readonly struct 天将 :
    IComparable<天将>,
    IEquatable<天将>,
    IEqualityOperators<天将, 天将, bool>
{
    private readonly int 序数;
    private 天将(int 已经检查的序数)
    {
        Debug.Assert(已经检查的序数 is >= 0 and < 12);
        this.序数 = 已经检查的序数;
    }
    public static 天将 贵人 => new(0);
    public static 天将 螣蛇 => new(1);
    public static 天将 朱雀 => new(2);
    public static 天将 六合 => new(3);
    public static 天将 勾陈 => new(4);
    public static 天将 青龙 => new(5);
    public static 天将 天空 => new(6);
    public static 天将 白虎 => new(7);
    public static 天将 太常 => new(8);
    public static 天将 玄武 => new(9);
    public static 天将 太阴 => new(10);
    public static 天将 天后 => new(11);

    public int CompareTo(天将 另一天将)
    {
        return this.序数.CompareTo(另一天将.序数);
    }

    public bool Equals(天将 另一天将)
    {
        return this.序数.Equals(另一天将.序数);
    }

    public override bool Equals([NotNullWhen(true)] object? 另一对象)
    {
        if (另一对象 is 天将 将)
            return this.序数.Equals(将.序数);
        return false;
    }

    public override string ToString()
    {
        return this.序数 switch
        {
            0 => "贵人",
            1 => "螣蛇",
            2 => "朱雀",
            3 => "六合",
            4 => "勾陈",
            5 => "青龙",
            6 => "天空",
            7 => "白虎",
            8 => "太常",
            9 => "玄武",
            10 => "太阴",
            _ => "天后",
        };
    }

    public static explicit operator int(天将 天将)
    {
        return 天将.序数;
    }

    public static explicit operator 天将(int 值)
    {
        值 = (值 % 12 + 12) % 12;
        return new(值);
    }

    public override int GetHashCode()
    {
        return this.序数.GetHashCode();
    }

    public static bool operator ==(天将 左, 天将 右)
    {
        return 左.序数 == 右.序数;
    }
    public static bool operator !=(天将 左, 天将 右)
    {
        return 左.序数 != 右.序数;
    }
}
