#Send SMS with Sinch nuget package.

First, you will need to [create a Sinch account](https://www.sinch.com/dashboard/#/signup) to get your app key and secret.

Then in package manager console, run to following command
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

And that's it. Any questions, email us at [support@sinch.com](mailto:support@sinch.com)
