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

    [Header("User Information")] // the display labels for all of the user information
    public TextMeshProUGUI
        currentSessionLbl,
        usernameLbl,
        expiryLbl,
        subscriptionsLbl,
        ipAddressLbl,
        hwidLbl,
        creationDateLbl,
        lastLoginLbl,
        expiresInLbl,
        usersLbl,
        onlineUsersLbl,
        licenseLbl,
        versionLbl,
        customerPanelLink;


    public void displayUserInformation() // when the user successfully logs in, it will display this information on the user information tab
    {
        currentSessionLbl.text = $"Current Session: " + KeyAuthApp.response.success;
        usernameLbl.text = "Username: " + KeyAuthApp.user_data.username;
        expiryLbl.text = "Expiry: " + UnixTimeToDateTime(long.Parse(KeyAuthApp.user_data.subscriptions[0].expiry));
        subscriptionsLbl.text = "Subscriptions: " + KeyAuthApp.user_data.subscriptions[0].subscription;
        ipAddressLbl.text = "IP Address: " + KeyAuthApp.user_data.ip;
        hwidLbl.text = "HWID: " + KeyAuthApp.user_data.hwid;
        creationDateLbl.text = "Creation Date: " + UnixTimeToDateTime(long.Parse(KeyAuthApp.user_data.createdate));
        lastLoginLbl.text = "Last Login: " + UnixTimeToDateTime(long.Parse(KeyAuthApp.user_data.lastlogin));
        expiresInLbl.text = "Expires In: " + expirydaysleft();
        usersLbl.text = "Users: " + KeyAuthApp.app_data.numUsers;
        onlineUsersLbl.text = "Online Users: " + KeyAuthApp.app_data.numOnlineUsers;
        licenseLbl.text = "Licenses: " + KeyAuthApp.app_data.numKeys;
        versionLbl.text = "Version: " + KeyAuthApp.app_data.version;
        customerPanelLink.text = "Customer Panel: " + KeyAuthApp.app_data.customerPanelLink;
    }

    private static api KeyAuthApp = new api(
    name: "UnityTest", // the name of your keyauth application
    ownerid: "7AvflSMyig", // your ownerID can be found in your keyauth settings
    secret: "39f456f430a9c83de489ddc49f7f989fdfc9d9629e8bb469d12201d9e779aab8", // the secret can be found right under your application name
    version: "1.0" // the version of your application. By default the version will be 1.0, if you can it in your keyauth settings make sure you change it here or it won't work
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

    public void ExitApplication() // this quits the application
    {
        Application.Quit(); 
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
}
