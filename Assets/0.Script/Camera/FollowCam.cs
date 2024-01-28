using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : Singleton<FollowCam>
{
    [SerializeField] Transform target;

    private Vector3 velocity = Vector3.zero;
    public float smoothTime = 0.3F;

    private void Awake()
    {
        if (Instance != null)
        {
            DontDestroyOnLoad(gameObject);
        }
        var fc = FindObjectsOfType<FollowCam>();
        if (fc.Length == 2)
        {
            Destroy(gameObject);
        }

    }
    // Update is called once per frame
    void Update()
    {
        if (target == null)
            return;

        Vector3 damp = Vector3.SmoothDamp(transform.position, target.position, ref velocity, smoothTime);
        damp.z = -10f;

        float x, y = 0f;
        switch(Define.GetCurrentSceneIndex)
        {
            case 1:
                transform.position = new Vector3(Mathf.Clamp(damp.x, -0.5f, 5f), Mathf.Clamp(damp.y, 2.3f, 4.2f), -10f);
                break;
            case 2:
                transform.position = new Vector3(Mathf.Clamp(damp.x, -3.7f, 5.5f), Mathf.Clamp(damp.y, 0.9f, 5f), -10f);
                break;
            case 3:
                transform.position = new Vector3(Mathf.Clamp(damp.x, -6.4f, 12.5f), Mathf.Clamp(damp.y, -2.5f, 12f), -10f);
                break;
            case 4:
                transform.position = new Vector3(Mathf.Clamp(damp.x, -5.4f, 5.4f), Mathf.Clamp(damp.y, -2f, 15.45f), -10f);
                break;
            case 5:
                transform.position = new Vector3(Mathf.Clamp(damp.x, -5f, 7f), Mathf.Clamp(damp.y, 8f, 25f), -10f);
                break;
            case 6:
                transform.position = new Vector3(Mathf.Clamp(damp.x, -14f, 14.9f), Mathf.Clamp(damp.y, 1.9f, 3.7f), -10f);
                break;
            case 7:
                transform.position = new Vector3(Mathf.Clamp(damp.x, -0.5f, -0.5f), Mathf.Clamp(damp.y, -0.1f, -0.1f), -10f);
                break;
            default:
                transform.position = damp;
                break;
        }
    }
}
