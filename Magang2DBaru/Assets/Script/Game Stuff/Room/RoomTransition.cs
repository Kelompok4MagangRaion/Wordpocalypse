using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTransition : MonoBehaviour
{
    [Header ("Change Stuff")]
    public Vector2 camChange;
    public Vector3 playerChange;

    private CameraMovement cam;

    [Header("Finish")]
    public bool isFinish;
    public GameObject finishPanel;

    void Start()
    {
        cam = Camera.main.GetComponent<CameraMovement>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            if (!isFinish)
            {
                cam.minPosition += camChange;
                cam.maxPosition += camChange;
                collision.transform.position += playerChange;
            }
            else
            {
                finishPanel.SetActive(true);
                UIManager.isFinish = true;
                Time.timeScale = 0;
            }
        }
    }
}
