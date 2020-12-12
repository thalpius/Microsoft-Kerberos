# Kerberos

I have created a small C# project that requests a Ticket Granting Service ticket using KerberosSecurityTokenProvider to use for Kerberoasting. I started the project for educational purposes only, but the tool works fine **and is not detected by Microsoft Defender for Identity** (tested with release 2.133 of Microsoft Defender for Identity).

For more information about Kerberoasting, please check my blog post:  
https://thalpius.com/2020/11/30/microsoft-defender-for-identity-kerberoasting/

# Usage

First search for an account with a service principal name you want to Kerberoast:  
```setspn -Q */*```

Once you've found an SPN, use it as a parameter to get the TGS hash which you can use to crack:  
```Kerberoasting.exe MSSQLSERVER/SQL-Server.thalpius.demo:1433```

# Screenshots

Getting all Service Principal Names within the domain:  

![Alt text](/Screenshots/Kerberoasting_01.jpg?raw=true "Get SPNs")

Using Kerberoasting.exe to get the Ticket Granting Service ticket to Kerberoast:  

![Alt text](/Screenshots/Kerberoasting_02.jpg?raw=true "Get TGS")
