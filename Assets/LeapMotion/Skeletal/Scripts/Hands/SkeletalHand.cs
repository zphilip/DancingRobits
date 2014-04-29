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

// The model for our skeletal hand made out of various polyhedra.
public class SkeletalHand : HandModel {

  protected const float PALM_CENTER_OFFSET = 21.0f;

  public GameObject palm;

  void Start() {
    IgnoreCollisionsWithSelf();
  }

  protected Vector3 GetPalmCenter(Hand hand) {
    return (hand.PalmPosition.ToUnityScaled() -
            hand.Direction.ToUnityScaled() * PALM_CENTER_OFFSET);
  }

  public override void InitHand(Transform deviceTransform) {
    Hand leap_hand = GetLeapHand();

    // Initialize all the fingers and palm.
    Vector3 palm_normal = deviceTransform.TransformDirection(leap_hand.PalmNormal.ToUnity());
    Vector3 palm_direction = deviceTransform.TransformDirection(leap_hand.Direction.ToUnity());
    Vector3 palm_center = deviceTransform.TransformPoint(GetPalmCenter(leap_hand));

    for (int f = 0; f < fingers.Length; ++f) {
      if (fingers[f] != null)
        fingers[f].InitFinger(deviceTransform, palm_normal, palm_direction);
    }

    if (palm != null) {
      palm.transform.position = palm_center;
      palm.transform.rotation = Quaternion.LookRotation(palm_direction, -palm_normal);
    }
  }

  public override void UpdateHand(Transform deviceTransform) {
    InitHand(deviceTransform);
  }
}
