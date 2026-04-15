using UnityEngine;

public enum HandSide
{
    Left,
    Right,
    Both
}

public static class EnumExtensions
{
    public static HandSide Opposite(this HandSide handSide)
    {
        return handSide switch
        {
            HandSide.Left => HandSide.Right,
            HandSide.Right => HandSide.Left,
            _ => handSide
        };
    }
}