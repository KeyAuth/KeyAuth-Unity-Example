using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Keybind : MonoBehaviour
{
    [Header("Cameras")]
    public Camera
        loginCam,
        registerCam,
        userInfoCam,
        registerNoKeyCam;

    [Header("Panels")]
    public GameObject
        loginPanel,
        registerPanel,
        userInfoPanel,
        registerNoKeyPanel;

    private void Start()
    {
        loginCam.enabled = true;
        registerCam.enabled = false;
        userInfoCam.enabled = false;
        registerNoKeyCam.enabled = false;

        loginPanel.SetActive(true);
        registerPanel.SetActive(false);
        userInfoPanel.SetActive(false);
        registerNoKeyPanel.SetActive(false);
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            loginCam.enabled = true;
            registerCam.enabled = false;
            userInfoCam.enabled = false;
            registerNoKeyCam.enabled = false;

            loginPanel.SetActive(true);
            registerPanel.SetActive(false);
            userInfoPanel.SetActive(false);
            registerNoKeyPanel.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            loginCam.enabled = false;
            registerCam.enabled = true;
            userInfoCam.enabled = false;
            registerNoKeyCam.enabled = false;

            loginPanel.SetActive(false);
            registerPanel.SetActive(true);
            userInfoPanel.SetActive(false);
            registerNoKeyPanel.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            loginCam.enabled = false;
            registerCam.enabled = false;
            userInfoCam.enabled = true;
            registerNoKeyCam.enabled = false;

            loginPanel.SetActive(false);
            registerPanel.SetActive(false);
            registerNoKeyPanel.SetActive(false);
            userInfoPanel.SetActive(true);
        }

        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            loginCam.enabled = false;
            registerCam.enabled = false;
            userInfoCam.enabled = false;
            registerNoKeyCam.enabled = true;

            loginPanel.SetActive(false);
            registerPanel.SetActive(false);
            registerNoKeyPanel.SetActive(true);
            userInfoPanel.SetActive(false);
        }
    }
}
