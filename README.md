# BankID-Implementation
BankID API implementation written in C# using .NET Framework. 

The library (BankID.Client) includes all BankID service end points, for authenticating, signing, collecting status/information of an authoriation attempt, as well as cancelling an authorization attempt. The solution includes two simple demo projects for testing purposes.

### BankID.WebDemo
Consisting of a MVC project and a Web API project. The MVC project functions as a basic SPA, the frontend communicates with the API and uses return values to modify the view using JavaScript.

### BankID.ConsoleDemo
Console application that with hardcoded actions & values directly interacts with the BankID.Client library.

### Notes
These projects do not include .NET authorization/authentication, the demo projects were created to solely interact with the BankID service through the BankID.Client library. Only swedish user messages are supported.

This implementation expects and requires the BankID certificates to be installed and stored locally. Install the RP and server certificate to your local key store.
