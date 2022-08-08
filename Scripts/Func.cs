using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.IO;
using System;
using KeyAuth;

public class Func : MonoBehaviour
{
    [Header("Login")] // the input fields for login (username and password)
    public TMP_InputField 
        loginUsername,
        loginPassword;

    [Header("Register")] // the input fields for register (username, password, key/license)
    public TMP_InputField 
        registerUsername,
        registerPassword,
        registerKey;

    [Header("Login/Register W/ License")] // the input field for login/register with a license only
    public TMP_InputField
        logRegLicense;

    [Header("Status")] // the status labels for login, register, and login/register with license only
    public TMP_Text
        loginStatusLbl,
        registerStatusLbl,
        licenseOnlyStatusLbl;

    [Header("Register No Key")]
    public TextMeshProUGUI statusLbl;
    public TMP_InputField usernameBox;
    public TMP_InputField passwordBox;

    [Header("User Information")] // the display labels for all of the user information
    public TextMeshProUGUI
        currentSessionLbl,
        usernameLbl,
        expiryLbl,
        subscriptionsLbl,
        //ipAddressLbl,
        //hwidLbl,
        creationDateLbl,
        lastLoginLbl,
        expiresInLbl,
        usersLbl,
        onlineUsersLbl,
        licenseLbl,
        versionLbl,
        customerPanelLink;

    [Header("Chatroom Features")]
    private float timeToGatherMsgs = 10f; // the countdown time for when to grab messages again

    public TMP_InputField testBox; // the object the messages get displayed on
    public TMP_InputField chat_MsgToSend; // the input_field that contains the message you want to send
    public TMP_InputField userToMute; // the username of the user that you want to mute
    private bool getMessages = false; // status to check if you should be getting messages or not
    private string chatchannel = "testing"; // the name of the chatroom (name is created on https://keyauth.win

    public void displayUserInformation() // when the user successfully logs in, it will display this information on the user information tab
    {
        currentSessionLbl.text = $"Current Session: " + KeyAuthApp.response.success;
        usernameLbl.text = "Username: " + KeyAuthApp.user_data.username;
        expiryLbl.text = "Expiry: " + UnixTimeToDateTime(long.Parse(KeyAuthApp.user_data.subscriptions[0].expiry));
        subscriptionsLbl.text = "Subscriptions: " + KeyAuthApp.user_data.subscriptions[0].subscription;
        //ipAddressLbl.text = "IP Address: " + KeyAuthApp.user_data.ip;
        //hwidLbl.text = "HWID: " + KeyAuthApp.user_data.hwid;
        creationDateLbl.text = "Creation Date: " + UnixTimeToDateTime(long.Parse(KeyAuthApp.user_data.createdate));
        lastLoginLbl.text = "Last Login: " + UnixTimeToDateTime(long.Parse(KeyAuthApp.user_data.lastlogin));
        expiresInLbl.text = "Expires In: " + expirydaysleft();
        usersLbl.text = "Users: " + KeyAuthApp.app_data.numUsers;
        onlineUsersLbl.text = "Online Users: " + KeyAuthApp.app_data.numOnlineUsers;
        licenseLbl.text = "Licenses: " + KeyAuthApp.app_data.numKeys;
        versionLbl.text = "Version: " + KeyAuthApp.app_data.version;
        customerPanelLink.text = "Customer Panel: " + KeyAuthApp.app_data.customerPanelLink;
    }

    public static api KeyAuthApp = new api(
    name: "",
    ownerid: "",
    secret: "",
    version: "1.0"
    );

    public DateTime UnixTimeToDateTime(long unixtime) // make sure that this is included so that it can properly get the expirations etc 
    {
        System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Local);
        dtDateTime = dtDateTime.AddSeconds(unixtime).ToLocalTime();
        return dtDateTime;
    }

    public string expirydaysleft() // make sure that this is included so that it can properly get the expirations etc 
    {
        System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Local);
        dtDateTime = dtDateTime.AddSeconds(long.Parse(KeyAuthApp.user_data.subscriptions[0].expiry)).ToLocalTime();
        TimeSpan difference = dtDateTime - DateTime.Now;
        return Convert.ToString(difference.Days + " Days " + difference.Hours + " Hours Left");
    }

    public void Awake() // you can do Awake or you can use Start(), just make sure you have KeyAuthApp.init(); inside so that it can init the application so that it's useable 
    {
        KeyAuthApp.init();
    }
    
    private void LateUpdate()
    {
        if (getMessages == true)
        {
            retrieveMessages();
        }
    }

    public void ExitApplication() // this quits the application
    {
        Application.Quit(); 
    }

    public void noKeyRegister()
    {
        KeyAuthApp.webhook("WEBHOOKIDEHERE", "&type=adduser&user=" + usernameBox.text + "&sub=default&expiry=1&pass=" + passwordBox.text);
        statusLbl.text = KeyAuthApp.response.message;
    }

    public void loginType() // method to login with username and password
    {
        KeyAuthApp.login(loginUsername.text, loginPassword.text);
        if (KeyAuthApp.response.success)
        {
            loginStatusLbl.text = "Welcome " + KeyAuthApp.user_data.username;
            displayUserInformation();
            loginUsername.text = null;
            loginPassword.text = null;
        }
        else
        {
            loginStatusLbl.text = KeyAuthApp.response.message;
        }
    }

    public void registerType() // method to register with username, password and license
    {
        KeyAuthApp.register(registerUsername.text, registerPassword.text, registerKey.text);
        if (KeyAuthApp.response.success)
        {
            registerStatusLbl.text = "Success, welcome " + registerUsername.text;
            registerUsername.text = null;
            registerPassword.text = null;
            registerKey.text = null;
        }
        else
        {
            registerStatusLbl.text = KeyAuthApp.response.message;
        }
    }

    public void licenseType() // method to login and/or register with only a license
    {
        KeyAuthApp.license(logRegLicense.text);
        if (KeyAuthApp.response.success)
        {
            licenseOnlyStatusLbl.text = "Success";
            logRegLicense.text = null;
        }
        else
        {
            licenseOnlyStatusLbl.text = KeyAuthApp.response.message;
        }
    }
    
    public void sendMsg() // method to send a message in the chatroom
    {
        if (KeyAuthApp.chatsend(chat_MsgToSend.text, chatchannel))
        {
            testBox.text += KeyAuthApp.user_data.username + ": " + chat_MsgToSend + "\n\n";
            chat_MsgToSend.text = null;
        }
    }

    public void retrieveMessages() // method to gather all messages and place in the chatroom
    {
        timeToGatherMsgs--; 
        if (timeToGatherMsgs == 0)
        {
            testBox.text = null; 
            if (!String.IsNullOrEmpty(chatchannel)) 
            {
                var messages = KeyAuthApp.chatget(chatchannel);
                if (messages == null || messages[0].message == "not_found")
                {
                    Debug.Log("No Messages");
                }
                else
                {
                    foreach (var message in messages)
                    {
                        testBox.text += message.author + ":   " + message.message + "\n\n";
                        timeToGatherMsgs = 10f;
                    }
                }
            }
            else
            {
                Debug.Log("No msgs");
            }
        }
    }

    public void clearAllMessages()
    {
        KeyAuthApp.webhook("WEBHOOKIDHERE", "&type=clearchannel&name=channelnamehere");
    }

    public void muteUserInChat()
    {
        KeyAuthApp.webhook("WEBHOOKDIDHERE", "&type=muteuser&user=" + userToMute.text + "&time=60");
    }
}
