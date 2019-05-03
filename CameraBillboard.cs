using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBillboard : MonoBehaviour
{
    public Camera m_Camera;

    private float scale;

    public bool MoonToggle;

    private SpriteRenderer sprite;

    public void OnEnable()
    {
        sprite = gameObject.GetComponent(typeof(SpriteRenderer)) as SpriteRenderer;
    }

    public void Update()
    {
        if (MoonToggle)
        {
            scale = Vector3.Distance(transform.position, m_Camera.transform.position);
            if (scale > 4000)
            {
                sprite.color = new Color(1f, 1f, 1f, 0f);
            }
            else
            {
                float alpha = Mathf.Clamp(800f / scale, 0f, 0.7f);
                
                sprite.color = new Color(1f, 1f, 1f, alpha);
            }
        }
    }

    //Orient the camera after all movement is completed this frame to avoid jittering
    void LateUpdate()
    {
        transform.LookAt(transform.position + m_Camera.transform.rotation * Vector3.forward,
            m_Camera.transform.rotation * Vector3.up);

        scale = Vector3.Distance(transform.position, m_Camera.transform.position) / 70f;
        transform.localScale = new Vector3(1f, 1f, 1f) * scale;

    }
}