using UnityEngine;
using System.Collections;

public class Roller : MonoBehaviour {

  public Transform upright;

	void Start () {
    rigidbody.maxAngularVelocity = 100.0f;
    Physics.IgnoreCollision(collider, upright.collider, true);
	}
	
	void Update () {
    float rotation_angle = 0;
    Vector3 rotation_axis;
    Quaternion force_quaternion = Quaternion.FromToRotation(Vector3.up,
                                                            upright.transform.up);
    force_quaternion.ToAngleAxis(out rotation_angle, out rotation_axis);

    // You get knocked down, don't get up again.
    if (rotation_angle > 60)
      return;

    // Vector3 upright_angular_vel = upright.rigidbody.angularVelocity;
    Vector3 velocity_diff = rigidbody.velocity - upright.rigidbody.velocity;
    Vector3 velocity_diff_rotation = Vector3.Cross(velocity_diff, Vector3.up);
    Vector3 velocity_rotation = Vector3.Cross(rigidbody.velocity, Vector3.up);

    // This is pretty much hacked together. Change these but beware.
    Vector3 force = 80.0f * Mathf.Tan(rotation_angle / 180.0f * Mathf.PI) * rotation_axis +
                    80.0f * velocity_diff_rotation -
                    6.0f * velocity_rotation;
    rigidbody.AddTorque(force);
	}
}
