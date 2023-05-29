using System.Collections.Generic;
using UnityEngine;

public static class PositionUtils
{
    public static List<Vector2> GetPositionsAround(Vector2 position, int count)
    {
        const int FirstRingCount = 5;
        const float BetweenUnitDistance = 1.25f;

        var positions = new List<Vector2>() { position };
        var ringCount = Mathf.CeilToInt(((float)count) / FirstRingCount);

        for (var i = 0; i < ringCount; i++)
        {
            var positionsInRing = GetPositionsAround(
                position, (i + 1) * FirstRingCount, (i + 1) * BetweenUnitDistance);
            positions.AddRange(positionsInRing);
        }

        return positions;
    }

    private static List<Vector2> GetPositionsAround(
        Vector2 position, int count, float distance)
    {
        var positions = new List<Vector2>();

        for (var i = 0; i < count; i++)
        {
            var angle = i * (360.0f / count);
            var direction = RotateVector(Vector2.right, angle);
            positions.Add(position + direction * distance);
        }

        return positions;
    }

    private static Vector2 RotateVector(Vector2 vector, float angle)
    {
        return Quaternion.Euler(0, 0, angle) * vector;
    }
}
