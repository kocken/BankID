# BankID-Implementation
BankID API implementation written in C# using .NET Framework. 

## The solution includes two simple demo projects for testing purposes:
### BankID.WebDemo
Includes a Web API project to communicate with the BankID service through the BankID.Client library. Also includes a MVC project, whereas the MVC frontend works as a basic SPA which modifies the view and communicates to the API using JavaScript.

### BankID.ConsoleDemo
Console application that directly interacts with the BankID.Client library.

These projects do not include .NET authorization or authentication, the demo projects were created to solely interact with the BankID service through the BankID.Client library. Only swedish user messages are supported.

This implementation expects and requires the BankID certificates to be installed and stored locally. Install the RP and server certificate to your local key store.
