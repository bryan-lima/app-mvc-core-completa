using DevIO.Business.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DevIO.App.Controllers
{
    public abstract class BaseController : Controller
    {
        #region Private Fields

        private readonly INotificador _notificador;

        #endregion Private Fields

        #region Protected Constructors

        protected BaseController(INotificador notificador)
        {
            _notificador = notificador;
        }

        #endregion Protected Constructors

        #region Protected Methods

        protected bool OperacaoValida()
        {
            return !_notificador.TemNotificacao();
        }

        #endregion Protected Methods
    }
}
