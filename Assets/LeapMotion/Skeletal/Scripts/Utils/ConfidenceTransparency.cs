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

// Set's the alpha of the hand based on the hand's self confidence.
public class ConfidenceTransparency : MonoBehaviour {

  private Material material;

  void Start () {
    material = new Material(Shader.Find("Transparent/Diffuse"));
    Renderer[] renderers = GetComponentsInChildren<Renderer>();
    
    for (int i = 0; i < renderers.Length; ++i)
      renderers[i].material = material;
  }

  void Update () {
    Hand leap_hand = GetComponent<HandModel>().GetLeapHand();

    if (leap_hand != null) {
      Color new_color = material.color;
      new_color.a = leap_hand.Confidence;
      material.color = new_color;
    }
  }
}
