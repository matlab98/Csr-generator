using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Org.BouncyCastle.Asn1.Crmf;
using RestSharp;
using RestSharp.Authenticators;
using RestSharp.Authenticators.OAuth;
using System.Text;
using System.Text.RegularExpressions;
using CsrGenerator.Business;
using CsrGenerator.Entities;
using CsrGenerator.Entities.Enum;

namespace CsrGenerator.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GeneradorCsr : ControllerBase
    {
        /// <summary>
        /// Instancia de la interfaz para llamar el service
        /// </summary>
        private readonly IPKI _pki;

        public GeneradorCsr(IPKI pki)
        {
            _pki = pki;
        }

        [HttpPost("/generarCsr")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> generarCsr([FromBody] KeyStoreRequest request)
        {
            Pki pki = await _pki.GenerarPKI(request);

            return Ok(pki);
        }
    }
}