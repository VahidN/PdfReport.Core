using System;
using System.IO;
using System.Linq;
using Org.BouncyCastle.Pkcs;
using Org.BouncyCastle.X509;

namespace PdfRpt.Core.Security
{
    /// <summary>
    /// A Personal Information Exchange File Reader
    /// </summary>
    public static class PfxReader
    {
        /// <summary>
        /// Reads A Personal Information Exchange File.
        /// </summary>
        /// <param name="pfxPath">Certificate file's path</param>
        /// <param name="pfxPassword">Certificate file's password</param>
        public static PfxData ReadCertificate(string pfxPath, string pfxPassword)
        {
            using (var stream = new FileStream(pfxPath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                var pkcs12Store = new Pkcs12Store(stream, pfxPassword.ToCharArray());
                var alias = findThePublicKey(pkcs12Store);
                var asymmetricKeyParameter = pkcs12Store.GetKey(alias).Key;
                var chain = constructChain(pkcs12Store, alias);
                return new PfxData { X509PrivateKeys = chain, PublicKey = asymmetricKeyParameter };
            }
        }

        private static X509Certificate[] constructChain(Pkcs12Store pkcs12Store, string alias)
        {
            var certificateChains = pkcs12Store.GetCertificateChain(alias);
            var chain = new X509Certificate[certificateChains.Length];

            for (var k = 0; k < certificateChains.Length; ++k)
                chain[k] = certificateChains[k].Certificate;

            return chain;
        }

        private static string findThePublicKey(Pkcs12Store pkcs12Store)
        {
            var alias = string.Empty;
            foreach (var entry in pkcs12Store.Aliases.Cast<string>().Where(entry => pkcs12Store.IsKeyEntry(entry) && pkcs12Store.GetKey(entry).Key.IsPrivate))
            {
                alias = entry;
                break;
            }

            if (string.IsNullOrEmpty(alias))
                throw new InvalidOperationException("Provided certificate is invalid.");

            return alias;
        }
    }
}
