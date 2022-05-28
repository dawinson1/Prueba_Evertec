using P2P = PlacetoPay.Integrations.Library.CSharp.PlacetoPay;
using PlacetoPay.Integrations.Library.CSharp.Contracts;
using PlacetoPay.Integrations.Library.CSharp.Entities;
using PlacetoPay.Integrations.Library.CSharp.Message;

namespace Prueba_Evertec.Models.DAO
{
    public interface ICrearsesion
    {
        RedirectInformation BuscarInformacion(string requestId);
        RedirectResponse CrearSesion(DatosFactura factura, string user_agent, string host);
    }


    public class CrearSesionDAO : ICrearsesion
    {

        private readonly ILogger<CrearSesionDAO> logger;
        private readonly IConfiguration configuration;

        public CrearSesionDAO(ILogger<CrearSesionDAO> logger, IConfiguration configuration)
        {
            this.logger = logger;
            this.configuration = configuration;
        }

        public RedirectResponse CrearSesion(DatosFactura factura, string user_agent, string host)
        {
            //recupero las  credenciales de acceso al API por  medio de variables de entorno que se encuentran en el archivo appsettings.Development.json
            var creden = configuration.GetSection("Placetopay_Credenciales").Get<Credenciales_Api>();
            //Creo una referencia con el prefijo REF + formato datetime
            var referencia = "REF" + DateTime.Now.ToString("TyyyyMMddHHmmssFFFFFFF");
            var redireccion = "https://" + host + "/Home/ResumenPago";
            /// Variable con la fecha de expiración de un día
            var Expiracion = DateTime.Now.AddDays(1).ToString("yyyy-MM-ddTHH\\:mm\\:sszzz");

            //Variable  que me  permitirá el acceso al api, recibiendo como parametro las  variables de entorno que cree
            Gateway gateway = new P2P(creden.Login, creden.SecretKey, new Uri(creden.Url_Base), Gateway.TP_REST);

            //completo  las  esquemas necesarias para poder crear la sesion
            Person person = new Person(factura.NumDoc.ToString(), factura.TipoDocumento, factura.Nombres, factura.Apellidos, factura.Correo, null, null, factura.NumCelular);
            Amount amount = new Amount(int.Parse(factura.ValorPago.ToString()), "COP");
            Payment payment = new Payment(referencia, "Este es un servicio de cobro para pruebas", amount);

            //esta clase recibe todas las esquemas para poder crear la se sion
            RedirectRequest rq = new RedirectRequest(payment, redireccion, factura.wclip, user_agent, Expiracion, null, null, false, false, false, person);

            //recibo los datos de respuesta de la creacion de sesion
            RedirectResponse response = gateway.Request(rq);

            //retnorno la respuesta
            return response;

        }

        public RedirectInformation BuscarInformacion(string requestId)
        {
            //recupero las  credenciales de acceso al API por  medio de variables de entorno que se encuentran en el archivo appsettings.Development.json
            var creden = configuration.GetSection("Placetopay_Credenciales").Get<Credenciales_Api>();

            Gateway gateway = new P2P(creden.Login, creden.SecretKey, new Uri(creden.Url_Base), Gateway.TP_REST);

            //recupero la  informacion del  pago con el resquest id
            var response = gateway.Query(requestId);

            return response;

        }
    }
}
