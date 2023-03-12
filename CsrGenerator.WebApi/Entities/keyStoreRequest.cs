using CsrGenerator.Entities.Enum;

namespace CsrGenerator.Entities
{
    public class KeyStoreRequest
    {
        public CertificateData certificateData { get; set; }

        public string certificateType { get; set; }

        public int securityLevel { get; set; }

        public int sizeKeys { get; set; }

        public SignatureAlgorithm certificateAlgorithm { get; set; }

    }
}
