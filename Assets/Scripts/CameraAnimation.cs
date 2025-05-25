 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class CameraAnimation : MonoBehaviour
{
    public List<GameObject> cameras;
    public int index;
    public int cena;
    public bool finished;

    private void Start()
    {
        DisableAll();
        cameras[0].SetActive(true);
        MouseEventHelper.Clickable();
    }

    private void DisableAll()
    {
        foreach (var item in cameras)
        {
            item.SetActive(false);
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && finished == false)
        {
            index++;
            if (index >= cameras.Count)
            {
                SceneController.LoadScene(cena, 1, 1);
                finished = true;
                return;

            }
            DisableAll();
            cameras[index].SetActive(true);
        }
    }
}
