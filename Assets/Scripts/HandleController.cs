using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleController : MonoBehaviour
{
    public float sensivisity = 0.1f;
    public float visibleSensivisity = 0.5f;
    public bool isHandling = false;
    private Vector2 lastTouchPos;
    public float rot;

    private Camera mainCam;

    private void Awake()
    {
        mainCam = Camera.main;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = mainCam.ViewportToWorldPoint(new Vector3(0.5f, 0.15f, 10));

        RotateHandle();
    }

    private void RotateHandle()
    {
        if(Input.GetMouseButtonDown(0))
        {
            isHandling = true;
            lastTouchPos = Input.mousePosition;
        }
        else if(Input.GetMouseButtonUp(0))
        {
            isHandling = false;
            StartCoroutine(EndHandling());
        }

        if(isHandling == true)
        {
            float direction = lastTouchPos.x - Input.mousePosition.x;

            transform.eulerAngles = new Vector3(0, 0, direction * (sensivisity * visibleSensivisity));
            rot = direction * sensivisity;
        }
    }

    private IEnumerator EndHandling()
    {
        float t = 0;
        float duration = 0.15f;
        Vector3 startAngle = transform.eulerAngles;

        while(t < 1)
        {
            t += Time.deltaTime / duration;

            transform.eulerAngles = new Vector3(0, 0, Mathf.LerpAngle(startAngle.z, 0, t)); //Vector3.Lerp(startAngle, Vector3.zero, t);

            yield return null;
        }
        
        transform.eulerAngles = Vector3.zero;
    }
}
