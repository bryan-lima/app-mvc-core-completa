using DevIO.Business.Interfaces;
using DevIO.Business.Notificacoes;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevIO.App.Extensions
{
    public class SummaryViewComponent : ViewComponent
    {
        private readonly INotificador _notificador;

        public SummaryViewComponent(INotificador notificador)
        {
            _notificador = notificador;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            List<Notificacao> _notificacoes = await Task.FromResult(_notificador.ObterNotificacoes());

            _notificacoes.ForEach(notificacao => ViewData.ModelState.AddModelError(key: string.Empty,
                                                                                   errorMessage: notificacao.Mensagem));

            return View();
        }
    }
}
