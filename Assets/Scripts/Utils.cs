using UnityEngine;

public struct QuaternionUtils {

    public static Quaternion Divide(Quaternion q1, Quaternion q2) {
        return q1 * Quaternion.Inverse(q2);
    }

    public static Quaternion Multiply(Quaternion q, float f) {
        return Quaternion.SlerpUnclamped(Quaternion.identity, q, f);
    }

    public static Quaternion Divide(Quaternion q, float f) {
        return Multiply(q, 1 / f);
    }

    public static Quaternion SmoothDamp(Quaternion current, Quaternion target, ref Vector3 currentVelocity, float smoothTime) {
        Vector3 c = current.eulerAngles;
        Vector3 t = target.eulerAngles;
        return Quaternion.Euler(
            Mathf.SmoothDampAngle(c.x, t.x, ref currentVelocity.x, smoothTime),
            Mathf.SmoothDampAngle(c.y, t.y, ref currentVelocity.y, smoothTime),
            Mathf.SmoothDampAngle(c.z, t.z, ref currentVelocity.z, smoothTime)
        );
    }

}
