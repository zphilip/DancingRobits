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

// Interface for all fingers.
public abstract class FingerModel : MonoBehaviour {

  public const int NUM_BONES = 3;
  public const int NUM_JOINTS = 4;

  public Finger.FingerType fingerType = Finger.FingerType.TYPE_INDEX;

  private Hand hand_;
  private Finger finger_;

  // Returns the location of the given joint on the finger.
  protected Vector3 GetJointPosition(int joint) {
    return finger_.JointPosition((Finger.FingerJoint)joint).ToUnityScaled();
  }

  // Returns the center of the given bone on the finger.
  protected Vector3 GetBonePosition(int bone) {
    return (finger_.JointPosition((Finger.FingerJoint)(bone + 1)).ToUnityScaled() +
            finger_.JointPosition((Finger.FingerJoint)(bone)).ToUnityScaled()) * 0.5f;
  }

  // Returns the direction the given bone is facing on the finger.
  protected Vector3 GetBoneDirection(int bone) {
    Vector3 difference = finger_.JointPosition((Finger.FingerJoint)(bone + 1)).ToUnity() -
                         finger_.JointPosition((Finger.FingerJoint)(bone)).ToUnity();
    difference.Normalize();
    return difference;
  }

  public void SetLeapHand(Hand hand) {
    hand_ = hand;
    finger_ = hand.Fingers[(int)fingerType];
  }

  public Hand GetLeapHand() { return hand_; }
  public Finger GetLeapFinger() { return finger_; }

  public abstract void InitFinger(Transform deviceTransform,
                                  Vector3 palm_normal, Vector3 palm_direction);

  public abstract void UpdateFinger(Transform deviceTransform,
                                    Vector3 palm_normal, Vector3 palm_direction);
}
