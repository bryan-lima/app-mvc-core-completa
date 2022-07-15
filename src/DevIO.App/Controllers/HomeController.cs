using DevIO.App.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DevIO.App.Controllers
{
    public class HomeController : Controller
    {
        #region Private Fields

        private readonly ILogger<HomeController> _logger;

        #endregion Private Fields

        #region Public Constructors

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        #endregion Public Constructors

        #region Public Methods

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [Route("erro/{id:length(3,3)}")]
        public IActionResult Errors(int id)
        {
            ErrorViewModel _modelErro = new();

            switch (id)
            {
                case 500:
                    _modelErro.Mensagem = "Ocorreu um erro! Tente novamente mais tarde ou contate nosso suporte.";
                    _modelErro.Titulo = "Ocorreu um erro!";
                    _modelErro.ErrorCode = id;
                    break;
                case 404:
                    _modelErro.Mensagem = "A página que está procurando não existe! <br />Em caso de dúvida entre em contato com nosso suporte.";
                    _modelErro.Titulo = "Ops! Página não encontrada";
                    _modelErro.ErrorCode = id;
                    break;
                case 403:
                    _modelErro.Mensagem = "Você não tem permissão para fazer isto.";
                    _modelErro.Titulo = "Acesso negado";
                    _modelErro.ErrorCode = id;
                    break;
                default:
                    return StatusCode(500);
            }

            return View("Error", _modelErro);
        }

        #endregion Public Methods
    }
}
