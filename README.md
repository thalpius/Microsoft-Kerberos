# Kerberos

I have created a small C# project that requests a Ticket Granting Service (TGS) ticket using KerberosSecurityTokenProvider to use for Kerberoasting and an option to request an Azure AD SSO TGS. For more information about these attacks, see the links below.

I started the project for educational purposes only, but the tool works fine **and is not detected by Microsoft Defender for Identity** (tested with release 2.133 of Microsoft Defender for Identity).

For more information about Azure AD SSO, please check my blog post:  
https://thalpius.com/2020/12/14/microsoft-on-premises-to-the-cloud-using-seamless-single-sign-on/

For more information about Kerberoasting, please check my blog post:  
https://thalpius.com/2020/11/30/microsoft-defender-for-identity-kerberoasting/

# Usage Azure AD SSO

Request a TGS for Azure AD SSO using the following command:  
```Batchfile
Kerberos.exe /azureadsso
```

# Usage Kerberoast

First search for an account with a service principal name you want to Kerberoast:  
```Batchfile
setspn -Q */*
```

Once you've found an SPN, use it as a parameter to get the TGS hash which you can use to crack:  
```Batchfile
Kerberos.exe /kerberoast:MSSQLSERVER/SQL-Server.thalpius.demo:1433
```

# Screenshots

Getting a TGS for Azure AD SSO:  

![Alt text](/Screenshots/Kerberos01.jpg?raw=true "Azure AD SSO")

Getting a Ticket Granting Service (TGS) ticket to Kerberoast:  

![Alt text](/Screenshots/Kerberos02.jpg?raw=true "Kerberoasting")
