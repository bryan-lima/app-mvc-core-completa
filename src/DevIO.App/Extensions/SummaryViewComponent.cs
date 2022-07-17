using DevIO.Business.Interfaces;
using DevIO.Business.Notificacoes;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevIO.App.Extensions
{
    public class SummaryViewComponent : ViewComponent
    {
        #region Private Fields

        private readonly INotificador _notificador;

        #endregion Private Fields

        #region Public Constructors

        public SummaryViewComponent(INotificador notificador)
        {
            _notificador = notificador;
        }

        #endregion Public Constructors

        #region Public Methods

        public async Task<IViewComponentResult> InvokeAsync()
        {
            List<Notificacao> _notificacoes = await Task.FromResult(_notificador.ObterNotificacoes());

            _notificacoes.ForEach(notificacao => ViewData.ModelState.AddModelError(key: string.Empty,
                                                                                   errorMessage: notificacao.Mensagem));

            return View();
        }

        #endregion Public Methods
    }
}
