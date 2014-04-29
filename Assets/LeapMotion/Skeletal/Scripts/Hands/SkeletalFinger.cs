/******************************************************************************\
* Copyright (C) Leap Motion, Inc. 2011-2014.                                   *
* Leap Motion proprietary and  confidential.  Not for distribution.            *
* Use subject to the terms of the Leap Motion SDK Agreement available at       *
* https://developer.leapmotion.com/sdk_agreement, or another agreement between *
* Leap Motion and you, your company or other organization.                     *
* Author: Matt Tytel
\******************************************************************************/

using UnityEngine;
using System.Collections;
using Leap;

// The finger model for our skeletal hand made out of various polyhedra.
public class SkeletalFinger : FingerModel {

  public Transform[] bones = new Transform[NUM_BONES];

  protected void JumpToPosition(Transform deviceTransform,
                                Vector3 palm_normal, Vector3 palm_direction) {
    Vector3 last_bone_normal = palm_normal;
    Vector3 last_bone_direction = palm_direction;

    for (int i = 0; i < NUM_BONES; ++i) {
      if (bones[i] != null) {
        // Set position.
        bones[i].transform.position = deviceTransform.TransformPoint(GetBonePosition(i));

        // Set rotation.
        Vector3 bone_direction = deviceTransform.TransformDirection(GetBoneDirection(i));
        last_bone_normal = Quaternion.FromToRotation(last_bone_direction, bone_direction) * last_bone_normal;
        bones[i].transform.rotation = Quaternion.LookRotation(bone_direction, -last_bone_normal);
        last_bone_direction = bone_direction;
      }
    }
  }

  public override void InitFinger(Transform deviceTransform,
                                  Vector3 palm_normal, Vector3 palm_direction) {
    JumpToPosition(deviceTransform, palm_normal, palm_direction);
  }

  public override void UpdateFinger(Transform deviceTransform,
                                    Vector3 palm_normal, Vector3 palm_direction) {
    JumpToPosition(deviceTransform, palm_normal, palm_direction);
  }
}
