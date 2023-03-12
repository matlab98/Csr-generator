
using CsrGenerator.Entities;

namespace CsrGenerator.Business
{
    public interface IPKI
    {
        Task<Pki> GenerarPKI(KeyStoreRequest request);
    }
}
