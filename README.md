# Puship examples for API access

---

## DESCRIPTION

Puship api are REST API, so it is possible to integrate any technology: php, python, java, angular, c # ...

Using [Swagger] (http://api.puship.com) you have the possibility to test REST calls directly.

Remember that you will first need to generate a token by calling the authentication service like this:

```
curl --location --request POST 'https://auth.puship.com/connect/token' \
--header 'Content-Type: application/x-www-form-urlencoded' \
--data-urlencode 'grant_type=client_credentials' \
--data-urlencode 'client_id=<you client id here>' \
--data-urlencode 'client_secret=<your client secret here>'
```

The examples of this project are in asp.net mvc but they just want to be a guideline for the integration between puship and other external systems.


##<a name="license"></a> LICENSE

	The MIT License

	Copyright (c) 2012 Adobe Systems, inc.
	portions Copyright (c) 2012 Olivier Louvignes

	Permission is hereby granted, free of charge, to any person obtaining a copy
	of this software and associated documentation files (the "Software"), to deal
	in the Software without restriction, including without limitation the rights
	to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
	copies of the Software, and to permit persons to whom the Software is
	furnished to do so, subject to the following conditions:

	The above copyright notice and this permission notice shall be included in
	all copies or substantial portions of the Software.

	THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
	IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
	FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
	AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
	LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
	OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
	THE SOFTWARE.




##<a name="automatic_installation"></a>Installation and execution

Open the ApiSample.sln solution with visual studio and build it.

### Project Sample.Machine2Machine

This is the main project and contains an example for each API puship. This is using machine2machine connection mode

To test this connection mode, run the "Sample.Machine2Machine" project

Once started, enter the clientid and client secret you have created from the puship dashboard.

***Be aware that some features may be limited based on your user's plan.

### WebApp example

This project makes only one sample API call because its main purpose is to demonstrate how to make a connection in WebApp mode

To test this, modify the clientid and client_secret in the program.cs file.

```
    options.ClientId = "<you client id here>";
    options.ClientSecret = "<your client secret here>";
```

Then start the project and login with your credentials.

By pressing "Query the GetApps API" you should see the number of apps in your account.

### Shared library

This is a shared project between the two previous projects and contains the DTO classes used to map the results of calls to the Puship API.


This example is also available online at this [link] (http://example1.puship.com)...enjoy!
