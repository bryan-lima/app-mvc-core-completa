﻿using DevIO.Business.Notificacoes;
using System.Collections.Generic;

namespace DevIO.Business.Interfaces
{
    public interface INotificador
    {
        #region Public Methods

        bool TemNotificacao();

        List<Notificacao> ObterNotificacoes();

        void Handle(Notificacao notificacao);

        #endregion Public Methods
    }
}
