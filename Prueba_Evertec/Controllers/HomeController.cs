using Microsoft.AspNetCore.Mvc;
using Prueba_Evertec.Models;
using Prueba_Evertec.Models.DAO;
using System.Diagnostics;

namespace Prueba_Evertec.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ICrearsesion crearSesion;

        //le paso  el servicio ICrearsesion para poder tener acceso a sus funcionces
        public HomeController(ILogger<HomeController> logger, ICrearsesion crearSesion)
        {
            _logger = logger;
            this.crearSesion = crearSesion;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult RealizarPago()
        {
            return View();
        }

        /// <summary>
        /// Recibo  los datos del  formularios para luego procesarlos y crear la sesion de pago
        /// </summary>
        /// <param name="datosFactura"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult RealizarPago(DatosFactura datosFactura)
        {
            //variable  para obtener el user_agent
            string user_agent = Request.Headers.UserAgent.ToString();
            string host = Request.Host.Host + ":" + Request.Host.Port;

            //variable que contendrá el resultado de crear la sesion de pago
            var crear_pago = crearSesion.CrearSesion(datosFactura, user_agent, host);

            //si la sesion se creó correctamente redirreciono a la url que me proporcionan
            if (crear_pago.Status.IsSuccessful())
            {
                //Guardo el requesid en un datotemporal ya que no estoy haciendo uso de base de datos
                TempData["RequesId"] = crear_pago.RequestId;
                return Redirect(crear_pago.ProcessUrl);
            }
            else
            {
                //en caso de que no se haya creado la sesion paso los datos de de la sesion al ErrorViewModel y poder mostrar la informacion en la  vista Error
                var error = new ErrorViewModel()
                {
                    Estado = crear_pago.Status.status,
                    Mensaje = crear_pago.Status.Message,
                    Fecha = crear_pago.Status.Date
                };

                return  View("Error", error);
            }



        }

        /// <summary>
        /// Vista que  retornará cuando el pago finalice
        /// </summary>
        /// <returns></returns>
        public IActionResult ResumenPago()
        {
            ///recupero el  resquestid para poder obtener la informacion del pago
            var requestid = TempData["RequesId"].ToString();
            //variable que contendrá  el  resultado de la busqueda de informacion del pago
            var resultado = crearSesion.BuscarInformacion(requestid);

            return View(resultado);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        //[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        //public IActionResult Error()
        //{
        //    return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        //}

        public IActionResult Error()
        {
            return View();
        }

    }
}