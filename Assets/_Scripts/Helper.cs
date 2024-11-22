using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Helper
{
    public static bool[] CheckSides(Transform transform, float offsetFromCenter, float rayDistance, LayerMask groundLayerMask) // Checks two sides of the gameObject to not fall from the platform
    {
        Vector3 OffsetR = new Vector3(offsetFromCenter, offsetFromCenter, 0);
        Vector3 OffsetL = new Vector3(-offsetFromCenter, offsetFromCenter, 0);

        bool rightSideGround = Physics.Raycast(transform.position + OffsetR, Vector3.down, rayDistance, groundLayerMask);
        bool leftSideGround = Physics.Raycast(transform.position + OffsetL, Vector3.down, rayDistance, groundLayerMask);

        //Debug.DrawRay(transform.position + OffsetL, Vector3.down, Color.green);
        //Debug.DrawRay(transform.position + OffsetR, Vector3.down, Color.green);

        bool[] sidesOnGround = { rightSideGround, leftSideGround };
        return sidesOnGround;
    }
}
