using Org.BouncyCastle.Asn1.Pkcs;
using Org.BouncyCastle.Asn1.X509;
using System.Collections;
using CsrGenerator.Entities;

using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Operators;
using Org.BouncyCastle.Crypto.Prng;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Pkcs;
using Org.BouncyCastle.Security;
using System.Text;
using CsrGenerator.Entities.Enum;

namespace CsrGenerator.Business
{
    public class PKI : IPKI
    {
        public async Task<Pki> GenerarPKI(KeyStoreRequest request)
        {

            #region Determine Signature Algorithm

            string signatureAlgorithmStr;
            switch (request.certificateAlgorithm)
            {
                case SignatureAlgorithm.SHA1:
                    signatureAlgorithmStr = PkcsObjectIdentifiers.Sha1WithRsaEncryption.Id;
                    break;

                case SignatureAlgorithm.SHA256:
                    signatureAlgorithmStr = PkcsObjectIdentifiers.Sha256WithRsaEncryption.Id;
                    break;

                case SignatureAlgorithm.SHA512:
                    signatureAlgorithmStr = PkcsObjectIdentifiers.Sha512WithRsaEncryption.Id;
                    break;

                default:
                    signatureAlgorithmStr = PkcsObjectIdentifiers.Sha256WithRsaEncryption.Id;
                    break;
            }

            #endregion

            #region Cert Info

            IDictionary attrs = new Hashtable();

            attrs.Add(X509Name.CN, request.certificateData.CN);
            //attrs.Add(X509Name.O, request.certificateData.);
            //attrs.Add(X509Name.OU, request.certificateData.);
            attrs.Add(X509Name.C, request.certificateData.C);
            attrs.Add(X509Name.SerialNumber, request.certificateData.SerialNumber);

            X509Name subject = new X509Name(new ArrayList(attrs.Keys), attrs);

            #endregion

            #region Key Generator

            RsaKeyPairGenerator rsaKeyPairGenerator = new RsaKeyPairGenerator();
            rsaKeyPairGenerator.Init(new KeyGenerationParameters(new SecureRandom(new CryptoApiRandomGenerator()), request.sizeKeys));
            AsymmetricCipherKeyPair pair = rsaKeyPairGenerator.GenerateKeyPair();

            #endregion

            #region CSR Generator

            Asn1SignatureFactory signatureFactory = new Asn1SignatureFactory(signatureAlgorithmStr, pair.Private);

            Pkcs10CertificationRequest csr = new Pkcs10CertificationRequest(signatureFactory, subject, pair.Public, null, pair.Private);

            #endregion

            #region Convert to PEM and Output

            #region Private Key

            StringBuilder privateKeyStrBuilder = new StringBuilder();
            PemWriter privateKeyPemWriter = new PemWriter(new StringWriter(privateKeyStrBuilder));
            privateKeyPemWriter.WriteObject(pair.Private);
            privateKeyPemWriter.Writer.Flush();

            #endregion Private Key

            #region Public Key

            StringBuilder publicKeyStrBuilder = new StringBuilder();
            PemWriter publicKeyPemWriter = new PemWriter(new StringWriter(publicKeyStrBuilder));
            publicKeyPemWriter.WriteObject(pair.Private);
            publicKeyPemWriter.Writer.Flush();

            #endregion Public Key

            #region CSR

            StringBuilder csrStrBuilder = new StringBuilder();
            PemWriter csrPemWriter = new PemWriter(new StringWriter(csrStrBuilder));
            csrPemWriter.WriteObject(csr);
            csrPemWriter.Writer.Flush();

            #endregion CSR

            #endregion

            return new Pki()
            {
                Csr = csrStrBuilder.ToString().Replace("-----BEGIN CERTIFICATE REQUEST-----", "-----BEGIN NEW CERTIFICATE REQUEST-----").Replace("-----END CERTIFICATE REQUEST-----", "-----END NEW CERTIFICATE REQUEST-----"),
                PublicKey = publicKeyStrBuilder.ToString(),
                PrivateKey = privateKeyStrBuilder.ToString()

            };
        }
    }
}
