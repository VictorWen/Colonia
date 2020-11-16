using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceIconScript : MonoBehaviour
{
    public SpriteRenderer resourceicon;
    public float scaleInertia;

    private void Update()
    {
        // change scale when zooming in and out
        float cameraSize = Camera.main.orthographicSize;
        float scale = 0.5f * cameraSize / 5f;
        Vector3 target = new Vector3(scale, scale);
        transform.localScale += (target - transform.localScale) / scaleInertia * Time.deltaTime;
    }

    public void SetResource(Sprite resourceSprite) // TODO: change to string resourceID
    {
        resourceicon.sprite = resourceSprite;
    }
}
