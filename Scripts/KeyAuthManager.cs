using UnityEngine;
using TMPro;
using UnityEngine.UI;
using KeyAuth;
using System;
using System.IO;

public class KeyAuthManager : MonoBehaviour
{
    // KeyAuth website - https://keyauth.win or https://keyauth.cc
    // KeyAuth Github - https://github.com/keyauth
    // KeyAuth docs - https://docs.keyauth.cc

    [Header("Login Section")]
    public TMP_InputField loginUsernameBox;  // needed - to retrieve your username
    public TMP_InputField loginPasswordBox;  // needed - to retrieve your password
    public Toggle loginRememberMeToggle;     // optional - remember me function so you do not need to keep entering your username and password every time you login
    public TextMeshProUGUI welcomeLbl;       // optional, but recommended - welcome your users
    public static bool loggedIn;             // needed, however you can switch it for a different method if you'd like - simple bool to check if you are logged in or not, it comes in handy with a lot of features

    [Header("Register Section")]
    public TMP_InputField registerUsernameBox;   // needed - to retrieve your username
    public TMP_InputField registerPasswordBox;   // needed - to retrieve your password 
    public TMP_InputField registerConfirmPasswordBox;   // optional, this will not be checked by KeyAuth - confirm your password 
    public TMP_InputField registerLicenseBox;   // needed, if you do not want to require your users to enter a license you will need to purchase the seller subscription for $19.99 a year

    [Header("Logs")]
    public TextMeshProUGUI logsLbl; // optional - logs all the errors or successes
    public TextMeshProUGUI statusLbl; // optional - shows the current status of the process/request you run (logged in, true, logged out, false, etc)

    [Header("Panels")]
    public GameObject loginPanel; // optional, but recommended - the panel for the login feature
    public GameObject registerPanel; // optional, but recommended - the panel for the register feature 
    public GameObject sectionPanel; // optional, but recommended - the panel for the section that explains how to navigate

    [Header("User Data")]
    public TextMeshProUGUI userDataBox; // needed - will display all information about you (username, hwid, ip, subscription, end date, etc) 
    public TextMeshProUGUI onlineUsersDisplay; // needed - will display all the active SESSIONS! this is not the number of people currently using the app, rather than the number of active sessions. You can change the session time to expire though if you'd like via the KeyAuth settings

    [Header("Chatroom")]
    public TMP_InputField chatroomInputField; // needed - the message that you want to send
    public TextMeshProUGUI chatroomMessageDisplay; // needed - the label or whatever you choose, that will display all of the messages. it's recommened you use just a label, but if you are good with Instantiating then you can do that as well :) 
    public float timeToGatherMessages = 10f; // needed - the time it will take to retrieve messages, you can change this to whatever you'd like, or you can remove it and use an IEnumerator, you're choice. 

    public string chatroomChannel = "testing"; // enter the name of your chatroom here, it must match on the KeyAuth dashboard or it won't work.
    public KeyCode SendKeyAuthMessage = KeyCode.Return; // return is {ENTER}

    /// <summary>
    /// You can get all of this information from visiting the KeyAuth dashboard. You can copy and paste it all from the c# section, make sure you change all of this information to YOUR app details. This is a test app that was deleted!
    /// </summary>
    public static api KeyAuthApp = new api(
     name: "UnityTutorial",
     ownerid: "7AvflSMyig",
     secret: "fb358a2e161466312939a6d711394420fffe3ffbad8fb43a7e91d20e2c46873d",
     version: "1.0"
    );

    private void Start()
    {
        #region Remember Me Function
        if (PlayerPrefs.GetString("login_username") != null && PlayerPrefs.GetString("login_password") != null)
        {
            loginUsernameBox.text = PlayerPrefs.GetString("login_username");
            loginPasswordBox.text = PlayerPrefs.GetString("login_password");
            loginRememberMeToggle.isOn = true;
        }
        else
        {
            loginRememberMeToggle.isOn = false;
        }
        #endregion

        KeyAuthApp.init(); // this is needed on Start in order for KeyAuth to work
        if (KeyAuthApp.response.success)
        {
            logsLbl.text = logsLbl.text + "\n <color=green> ! <color=white>Successfully Initialized";
        }
    }

    private void Update()
    {
        statusLbl.text = "Status: " + KeyAuthApp.response.success;

        if (loggedIn == true)
        {
            if (Input.GetKeyDown(SendKeyAuthMessage))
            {
                SendMessage();
            }

            RetrieveMessages();
        }
    }

    public void Login() // the login function 
    {
        KeyAuthApp.login(loginUsernameBox.text, loginPasswordBox.text); // notice you make the request, and then check if it was successful or if it failed by using if/else statement(s). 
        if (KeyAuthApp.response.success)
        {
            logsLbl.text = logsLbl.text + "\n <color=green> ! <color=white>Successfully logged in.";
            RememberMe();
            UserData();
            loginUsernameBox.text = null;
            loginPasswordBox.text = null;
            welcomeLbl.enabled = false;
            statusLbl.enabled = false;
            sectionPanel.SetActive(true);
            loginPanel.SetActive(false);
            logsLbl.enabled = false;
            loggedIn = true;
        }
        else
        {
            logsLbl.text = logsLbl.text + "\n Failed to log in";
            loginUsernameBox.text = null;
            loginPasswordBox.text = null;
        }
    }

    public void UserData() // this is the function to gather all the information about you and display it
    {
        userDataBox.text =
            "Username: " + KeyAuthApp.user_data.username +
            "\nExpiry: " + UnixTimeToDateTime(long.Parse(KeyAuthApp.user_data.subscriptions[0].expiry)) +
            "\nSubscription: " + KeyAuthApp.user_data.subscriptions[0].subscription +
            "\nIP: " + KeyAuthApp.user_data.ip +
            "\nHWID: " + KeyAuthApp.user_data.hwid +
            "\nCreation Date: " + UnixTimeToDateTime(long.Parse(KeyAuthApp.user_data.createdate)) +
            "\nLast Login: " + UnixTimeToDateTime(long.Parse(KeyAuthApp.user_data.lastlogin)) +
            "\nTime Left: " + KeyAuthApp.expirydaysleft() + // this is in days
            "\nTotal Users: " + KeyAuthApp.app_data.numUsers +
            "\nOnline Users: " + KeyAuthApp.app_data.numOnlineUsers +
            "\nLicenses: " + KeyAuthApp.app_data.numKeys +
            "\nVersion: " + KeyAuthApp.app_data.version +
            "\nCustomer Panel: " + KeyAuthApp.app_data.customerPanelLink;

        var onlineUsers = KeyAuthApp.fetchOnline();

        foreach (var user in onlineUsers)
        {
            onlineUsersDisplay.text += "\n " + user.credential;
        }
    }

    public void SendMessage() // the function to actually send a message
    {
        if (KeyAuthApp.chatsend(chatroomInputField.text, chatroomChannel))
        {
            chatroomMessageDisplay.text += KeyAuthApp.user_data.username + "     > " + chatroomInputField.text + "          " + UnixTimeToDateTime(DateTimeOffset.Now.ToUnixTimeSeconds()).ToString();
            chatroomInputField.text = null;
        }
        else
        {
            logsLbl.text = logsLbl.text + "\n" + KeyAuthApp.response.message;
            Debug.LogError(KeyAuthApp.response.message);
        }
    }

    private void RetrieveMessages() // the function used to display all the messages sent
    {
        timeToGatherMessages--;
        if (timeToGatherMessages <= 0)
        {
            chatroomMessageDisplay.text = null;
            if (!string.IsNullOrEmpty(chatroomChannel))
            {
                var messages = KeyAuthApp.chatget(chatroomChannel);
                if (messages == null || messages[0].message == "not_found")
                {
                    Debug.Log("No messages found");
                }
                else
                {
                    foreach (var message in messages)
                    {
                        chatroomMessageDisplay.text += "\n" + message.author + "     > " + message.message + "          " + UnixTimeToDateTime(long.Parse(message.timestamp));
                        timeToGatherMessages = 10f;
                    }
                }
            }
            else
            {
                Debug.Log("No messages");
            }
        }
    }

    public DateTime UnixTimeToDateTime(long unixtime) // keyAuth uses UnixTime... in order to get the correct sub, expiry time to display you will need this function :) 
    {
        System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Local);
        dtDateTime = dtDateTime.AddSeconds(unixtime).ToLocalTime();
        return dtDateTime;
    }

    public void Register() // the register function 
    {
        if (registerPasswordBox.text != registerConfirmPasswordBox.text) // again, this is optional and you do not need the confirm password if you do not want to have it. 
        {
            logsLbl.text = logsLbl.text + "\n <color=red> ! <color=white> Passwords do not match!";
        }
        else
        {
            KeyAuthApp.register(registerUsernameBox.text, registerPasswordBox.text, registerLicenseBox.text); // notice you make the request, and then check if it was successful or if it failed by using if/else statement(s). 
            if (KeyAuthApp.response.success)
            {
                logsLbl.text = logsLbl.text + "\n Successfully registered.";
                registerUsernameBox.text = null;
                registerPasswordBox.text = null;
                registerConfirmPasswordBox.text = null;
                registerLicenseBox.text = null;
                registerPanel.SetActive(false);
                loginPanel.SetActive(true);
            }
            else
            {
                logsLbl.text = logsLbl.text + "\n Failed To register";
                registerUsernameBox.text = null;
                registerPasswordBox.text = null;
                registerConfirmPasswordBox.text = null;
                registerLicenseBox.text = null;
            }
        }
    }

    public void RememberMe() // the remember me function 
    {
        if (loginRememberMeToggle.isOn)
        {
            PlayerPrefs.SetString("login_username", loginUsernameBox.text);
            PlayerPrefs.SetString("login_password", loginPasswordBox.text);
            PlayerPrefs.Save();
        }
        else
        {
            PlayerPrefs.SetString("login_username", null);
            PlayerPrefs.SetString("login_password", null);
            PlayerPrefs.Save();
        }
    }


    /// <summary>
    /// Additional features -------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    /// You do not need to keep them inside the functions created. As long as you have the code you can use it anywhere you'd like. 
    /// </summary>

    private void SendWebhook()
    {
        KeyAuthApp.webhook("WebhookID", "param");
        // send secure request to webhook which is impossible to crack into. the base link set on the website is https://keyauth.win/api/seller/?sellerkey=sellerkeyhere&type=black, which nobody except you can see, so the final request is https://keyauth.win/api/seller/?sellerkey=sellerkeyhere&type=black&ip=1.1.1.1&hwid=abc
    }

    private void Download()
    {
        byte[] result = KeyAuthApp.download("fileID");
        File.WriteAllBytes("PathOfYourChoosing", result);
        // you can add a direct download link on KeyAuth and you then can add the FileID that is provided to you on the site here and it will download anything you want the users to be able to download
    }

    private void ShowVariable()
    {
        Debug.Log(KeyAuthApp.var("VarID"));
        // you can enter text on the site, and it will come back here by you using the varid that is provided to you after you create a var
    }

    private void CheckBlacklist()
    {
        if (KeyAuthApp.checkblack())
        {
            Debug.Log("User is blacklisted");
            Application.Quit();
        }
        else
        {
            Debug.Log("User is not blacklisted");
            // continue whatever
        }
        // this will check if the user that is trying to use the application is blacklisted or not via hwid/ip
    }

    private void CheckSession()
    {
        KeyAuthApp.check();
        if (KeyAuthApp.response.success)
        {
            Debug.Log("Session is valid");
        }
        else
        {
            Debug.Log("Session is not valid");
        }
        // this will check if the session is validated or not
    }

    private void Upgrade()
    {
        KeyAuthApp.upgrade("KeyAuthUsernameThatYouWantToUpgrade", "LicenseWithTheSameLevelAsTheSubYouWantToGiveTheUser");
        // if you would like to upgrade a user you can do so with this code, you do not need to login to use this upgrade feature either
    }

    private void Log()
    {
        KeyAuthApp.log("LogYouWantToSend");
        // if you would like to send a log after certain features you can. Say a user logs in, accesses a certain area of a map/program etc and you want an alert. Just call this function :)
    }

    private void LoginRegisterWithLicenseOnly()
    {
        KeyAuthApp.license("license");
        if (KeyAuthApp.response.success)
        {
            Debug.Log("Success!");
            // if it's the first time the user used the license then they have successfully registered and logged in, if it's there second time then successful login 
        }
        else
        {
            Debug.Log("Failed!");
        }
        // if you would like to have your users only register/login with a license then you can remove the login/register functions and just use this instead.
    }
}