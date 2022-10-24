using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Navigation : MonoBehaviour
{
    [Header("Panels")]
    public GameObject chatroomSection;
    public GameObject userdataSection;
    public GameObject selectionSection;

    [Header("KeyCodes")]
    public KeyCode NavLeft = KeyCode.LeftArrow;
    public KeyCode NavLeftAlt = KeyCode.Alpha1;
    public KeyCode NavRight = KeyCode.RightArrow;
    public KeyCode NavRightAlt = KeyCode.Alpha2;

    private void QuitApplication()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    private void CloseAll()
    {
        chatroomSection.SetActive(false);
        userdataSection.SetActive(false);
        selectionSection.SetActive(false);
    }

    private void navChatroomSection()
    {
        CloseAll();
        chatroomSection.SetActive(true);
    }

    private void openUserDataSection()
    {
        CloseAll();
        userdataSection.SetActive(true);
    }

    private void Update()
    {
        QuitApplication();

        if (KeyAuthManager.loggedIn == true)
        {
            if (chatroomSection.activeSelf == true && Input.GetKeyDown(NavLeft) || Input.GetKeyDown(NavLeftAlt))
            {
                CloseAll();
                userdataSection.SetActive(true);
            }
            else if (userdataSection.activeSelf == true && Input.GetKeyDown(NavRight) || Input.GetKeyDown(NavRightAlt))
            {
                CloseAll();
                chatroomSection.SetActive(true);
            }
        }
    }
}
