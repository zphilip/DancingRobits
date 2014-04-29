using UnityEngine;
using System.Collections;
using Leap;

public class RiggedHand : HandModel {

  public string handedness;

  protected readonly string[] finger_names = {"Thumb", "Index", "Middle", "Ring", "Pinky"};

  Transform GetArm() {
    return transform.Find("Root").Find(handedness + "_Arm");
  }
  
  void RescaleHand() {
    Hand hand = GetLeapHand();

    Transform wrist = GetArm().Find(handedness + "_Wrist");
    Vector3 leap_index_mcp = hand.Fingers[1].JointPosition(Finger.FingerJoint.JOINT_MCP).ToUnityScaled();
    Vector3 leap_index_dip = hand.Fingers[1].JointPosition(Finger.FingerJoint.JOINT_PIP).ToUnityScaled();
    Vector3 index0 = wrist.Find(handedness + "_" + finger_names[1] + "A")
                          .Find(handedness + "_" + finger_names[1] + "B")
                          .Find(handedness + "_" + finger_names[1] + "C").localPosition;
    float scale = (leap_index_mcp - leap_index_dip).magnitude / index0.magnitude;
    GetArm().localScale = new Vector3(scale, scale, scale);
  }


  void UpdateThumb(Transform deviceTransform) {
    Hand hand = GetLeapHand();
    Finger thumb = hand.Fingers[0];
    Transform wrist = GetArm().Find(handedness + "_Wrist");

    // Get all the joint positions in unity space.
    Vector3 mcpPos = thumb.JointPosition(Finger.FingerJoint.JOINT_MCP).ToUnityScaled();
    Vector3 pipPos = thumb.JointPosition(Finger.FingerJoint.JOINT_PIP).ToUnityScaled();
    Vector3 dipPos = thumb.JointPosition(Finger.FingerJoint.JOINT_DIP).ToUnityScaled();
    Vector3 tipPos = thumb.JointPosition(Finger.FingerJoint.JOINT_TIP).ToUnityScaled();

    Transform mcp = wrist.Find(handedness + "_ThumbA");
    Transform pip = mcp.Find(handedness + "_ThumbB");
    Transform dip = pip.Find(handedness + "_ThumbC");

    Vector3 mcp_position = deviceTransform.TransformPoint(mcpPos);
    Vector3 pip_position = deviceTransform.TransformPoint(pipPos);
    Vector3 dip_position = deviceTransform.TransformPoint(dipPos);
    Vector3 tip_position = deviceTransform.TransformPoint(tipPos);

    Vector3 curl_vector = Vector3.Cross(pip_position - mcp_position, dip_position - pip_position);
    if (curl_vector.magnitude <= 0.0001f)
      curl_vector = Vector3.Cross(dip_position - pip_position, tip_position - dip_position);
    if (curl_vector.magnitude <= 0.0001f)
      return;
    
    if (Vector3.Dot(curl_vector, hand.PalmNormal.ToUnityScaled()) < 0)
      curl_vector = -curl_vector;

    mcp.rotation = Quaternion.LookRotation(Vector3.Cross(mcp_position - pip_position, curl_vector), pip_position - mcp_position);
    pip.rotation = Quaternion.LookRotation(Vector3.Cross(pip_position - dip_position, curl_vector), dip_position - pip_position);
    dip.rotation = Quaternion.LookRotation(Vector3.Cross(dip_position - tip_position, curl_vector), tip_position - dip_position);
  }

  public override void InitHand(Transform deviceTransform) {
    RescaleHand();

    Hand hand = GetLeapHand();
    GetArm().localRotation = Quaternion.LookRotation(hand.PalmNormal.ToUnity(),
                                                      -hand.Direction.ToUnity());
    GetArm().localPosition = hand.PalmPosition.ToUnityScaled();

    Transform wrist = GetArm().Find(handedness + "_Wrist");

    for (int i = 1; i < hand.Fingers.Count; ++i) {
      Finger finger = hand.Fingers[i];

      // Get all the joint positions in unity space.
      Vector3 mcpPos = finger.JointPosition(Finger.FingerJoint.JOINT_MCP).ToUnityScaled();
      Vector3 pipPos = finger.JointPosition(Finger.FingerJoint.JOINT_PIP).ToUnityScaled();
      Vector3 dipPos = finger.JointPosition(Finger.FingerJoint.JOINT_DIP).ToUnityScaled();
      Vector3 tipPos = finger.JointPosition(Finger.FingerJoint.JOINT_TIP).ToUnityScaled();

      Transform attachment = wrist.Find(handedness + "_" + finger_names[i] + "A");
      Transform mcp = attachment.Find(handedness + "_" + finger_names[i] + "B");
      Transform pip = mcp.Find(handedness + "_" + finger_names[i] + "C");
      Transform dip = pip.Find(handedness + "_" + finger_names[i] + "D");

      Vector3 mcp_position = deviceTransform.TransformPoint(mcpPos);
      Vector3 pip_position = deviceTransform.TransformPoint(pipPos);
      Vector3 dip_position = deviceTransform.TransformPoint(dipPos);
      Vector3 tip_position = deviceTransform.TransformPoint(tipPos);

      Vector3 curl_vector = Vector3.Cross(pip_position - mcp_position, dip_position - pip_position);
      if (curl_vector.magnitude <= 0.0001f)
        curl_vector = Vector3.Cross(dip_position - pip_position, tip_position - dip_position);
      if (curl_vector.magnitude <= 0.0001f)
        curl_vector = Vector3.Cross(-hand.PalmNormal.ToUnity(), hand.Direction.ToUnity());

      mcp.rotation = Quaternion.LookRotation(Vector3.Cross(mcp_position - pip_position, curl_vector), pip_position - mcp_position);
      pip.rotation = Quaternion.LookRotation(Vector3.Cross(pip_position - dip_position, curl_vector), dip_position - pip_position);
      dip.rotation = Quaternion.LookRotation(Vector3.Cross(dip_position - tip_position, curl_vector), tip_position - dip_position);
    }

    UpdateThumb(deviceTransform);
  }

  public override void UpdateHand(Transform deviceTransform) {
    InitHand(deviceTransform);
  }
}
