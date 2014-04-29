/******************************************************************************\
* Copyright (C) Leap Motion, Inc. 2011-2014.                                   *
* Leap Motion proprietary and  confidential.  Not for distribution.            *
* Use subject to the terms of the Leap Motion SDK Agreement available at       *
* https://developer.leapmotion.com/sdk_agreement, or another agreement between *
* Leap Motion and you, your company or other organization.                     *
\******************************************************************************/

using UnityEngine;
using System.Collections;
using Leap;

namespace Leap {

  // Extension to the unity vector class. Provides automatic scaling into unity scene space.
  // Leap coordinates are in mm and Unity is in meters. So scale by 1000.
  public static class UnityVectorExtension
  {
    public const float INPUT_SCALE = 0.001f;

    // For directions.
    public static Vector3 ToUnity(this Vector lv) {
      return FlippedZ(lv);
    }

    // For acceleration/velocity.
    public static Vector3 ToUnityScaled(this Vector lv) {
      return INPUT_SCALE * FlippedZ(lv);
    }

    // For head mounted displays.
    public static Vector3 ToUnityHMD(this Vector lv) {
      return INPUT_SCALE * FlippedHMD(lv);
    }

    private static Vector3 FlippedZ(Vector v) {
      return new Vector3(v.x, v.y, -v.z);
    }

    private static Vector3 FlippedHMD(Vector v) {
      return new Vector3(-v.x, -v.z, v.y);
    }
  }
}
