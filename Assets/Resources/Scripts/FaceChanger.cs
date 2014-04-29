using UnityEngine;
using System.Collections;

public class FaceChanger : MonoBehaviour {

  const float SAD_IMPACT_VELOCITY = 3.0f;
  const int SAD_WAIT_TIME = 100;

  public GameObject[] faces;
  public Texture2D happy;
  public Texture2D sad;
  public Texture2D falling;
  public Texture2D dead;

  private int sad_count_ = 0;
  private bool is_dead_ = false;

  void SetFace(Texture2D face) {
    if (!is_dead_) {
      for (int i = 0; i < faces.Length; ++i)
        faces[i].renderer.material.mainTexture = face;
    }
  }

  void OnJointBreak(float breakForce) {
    SetFace(dead);
    is_dead_ = true;
  }

  void OnCollisionEnter(Collision collision) {
    if (collision.relativeVelocity.magnitude > SAD_IMPACT_VELOCITY) {
      SetFace(sad);
      sad_count_ = SAD_WAIT_TIME;
    }
  }

  void Start () {

  }

  void Update () {
    if (sad_count_ == 0)
      SetFace(happy);

    sad_count_--;
  }
}
