Here you will find all the extras as well as the proper way to setup the example. It is really simple to setup and you can always ask for help on https://community.keyauth.win 

Optional Functions:
[Webhooks]
KeyAuthApp.webhook("lfvbBrbFhIr", "?sellerkey=CUqDqlCIgl&type=resethash");
> send secure request to webhook which is impossible to crack into. the base link set on the website is https://keyauth.win/api/seller/, which nobody except you can see, so the final request is https://keyauth.win/api/seller/?sellerkey=CUqDqlCIgl&type=resethash


[Download Files]
byte[] result = KeyAuthApp.download("902901"); 
File.WriteAllBytes("C:\\Users\\Username\\Downloads\\KeyAuth-CSHARP-Example-main (5)\\KeyAuth-CSHARP-Example-main\\ConsoleExample\\bin\\Debug\\countkeys.txt", result);
> downloads application file


[Show Variable(s)]
MessageBox.Show(KeyAuthApp.var("123456"));
> retrieve application variable



[Chatrooms]
Not yet available in this example ... coming soon. The code is included in api.cs however there is not example code to show you how to set it up



[LeaderBoards]
Not yet available in this example ... coming soon. The code is included in api.cs however there is not example code to show you how to set it up