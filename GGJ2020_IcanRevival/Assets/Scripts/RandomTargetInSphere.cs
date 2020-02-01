using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomTargetInSphere : MoveTargetFinder
{
    public float Radius;

    public override Vector2 GetTarget()
    {
        float angle = Random.value * Mathf.PI * 2;
        float radius = Random.value * Radius;

        return new Vector2(transform.position.x + Mathf.Cos(angle) * radius, transform.position.y + Mathf.Sin(angle) * radius);
    }
}
