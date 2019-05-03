using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        Move();
	}
    [SerializeField][Range(0.01f, 2.0f)]
    private float speed = 1;
    public Camera cam;
    private float _y;

    public void Move()
    {
        if (Input.GetKeyDown("q"))
        {
            _y = 0.2f;
        }
        if (Input.GetKeyDown("e"))
        {
            _y = -0.2f;
        }
        if (Input.GetKeyUp("q"))
        {
            _y = 0f;
        }
        if (Input.GetKeyUp("e"))
        {
            _y = 0f;
        }
        if (Input.GetKey("q"))
        {
            _y += 0.2f;
        }
        if (Input.GetKey("e"))
        {
            _y -= 0.2f;
        }

        _y = Mathf.Clamp(_y, -1f, 1f);

        Vector3 Movement = new Vector3(Input.GetAxis("Horizontal"), _y, Input.GetAxis("Vertical"));
        float _speed = speed * Mathf.Abs(cam.transform.position.y);

        if (_speed < 0.001f)
        {
            _speed = 0.001f;
        }

        this.transform.position += Movement * _speed * Time.deltaTime;

    }
}
