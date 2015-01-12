#Send SMS with Sinch nuget package.

In package manager console run to following command
```PM> Install-Package Sinch.SMS```

To send an SMS use  
```
var client = new Sinch.SMS.Client("yourkey", "yoursecret");
var messageid = await client.SendSMS("+46722223355", "hello from sinch");

```
To check a status of an SMS use 
```
var client = new Sinch.SMS.Client("yourkey", "yoursecret");
var result = await client.CheckStatus(messageid);

```

Don't forget, to send an SMS you need an account with Sinch, sign up [here](#signup).
