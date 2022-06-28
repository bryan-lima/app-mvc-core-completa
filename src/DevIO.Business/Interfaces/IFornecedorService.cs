using DevIO.Business.Models;
using System;
using System.Threading.Tasks;

namespace DevIO.Business.Interfaces
{
    public interface IFornecedorService : IDisposable
    {
        #region Public Methods

        Task Adicionar(Fornecedor fornecedor);

        Task Atualizar(Fornecedor fornecedor);

        Task Remover(Guid id);

        Task AtualizarEndereco(Endereco endereco);

        #endregion Public Methods
    }
}
