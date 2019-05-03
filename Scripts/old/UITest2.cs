using UnityEngine;
using System.Collections;

public class UITest2 : MonoBehaviour
{

    private float orthoOrg;
    private float orthoCurr;
    private Vector3 scaleOrg;
    // private Vector3 posOrg;
    public Camera _cam;

    void Start()
    {
        if (_cam == null)
        {
            _cam = Camera.main;
        }
        orthoOrg = _cam.orthographicSize;
        orthoCurr = orthoOrg;
        this.transform.localScale = new Vector3 (1f, 1f, 1f) * orthoCurr / 60f;
        scaleOrg = transform.localScale;

        // posOrg = _cam.WorldToViewportPoint(transform.position);
    }
    void LateUpdate()
    {
        var osize = _cam.orthographicSize;
        if (orthoCurr != osize)
        {
            transform.localScale = scaleOrg * osize / orthoOrg;
            orthoCurr = osize;
            // transform.position = Camera.main.ViewportToWorldPoint(posOrg);
        }

    }
}