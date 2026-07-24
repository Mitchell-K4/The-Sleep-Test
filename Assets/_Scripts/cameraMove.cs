using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles the moving of the camera to the different panels.
/// </summary>
public class cameraMove : MonoBehaviour
{
    // Camera Speed
    public float speedFactor = 0.75f;

    // Anchor to move to 
    public Transform anchorToMoveTo;

    // Link to button press audio
    public AudioSource slide;

    // Update is called once per frame
    void Update()
    {
        // Move camera
        transform.position = Vector3.Lerp(transform.position, anchorToMoveTo.position, speedFactor);
    }

    /// <summary>
    /// Sets the anchor that the camera will move to.
    /// </summary>
    /// <param name="anchor">The camera anchor to move to.</param>
    public void setAnchor(Transform anchor)
    {
        // Play transition audio
        slide.Play();

        // Set new anchor point to move to
        anchorToMoveTo = anchor;
    }
}
