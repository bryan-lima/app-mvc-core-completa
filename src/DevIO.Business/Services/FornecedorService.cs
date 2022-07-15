using DevIO.Business.Interfaces;
using DevIO.Business.Models;
using DevIO.Business.Models.Validations;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DevIO.Business.Services
{
    public class FornecedorService : BaseService, IFornecedorService
    {
        #region Private Fields

        private readonly IEnderecoRepository _enderecoRepository;
        private readonly IFornecedorRepository _fornecedorRepository;

        #endregion Private Fields

        #region Public Constructors

        public FornecedorService(IEnderecoRepository enderecoRepository,
                                 IFornecedorRepository fornecedorRepository,
                                 INotificador notificador) : base(notificador)
        {
            _enderecoRepository = enderecoRepository;
            _fornecedorRepository = fornecedorRepository;
        }

        #endregion Public Constructors

        #region Public Methods

        public async Task Adicionar(Fornecedor fornecedor)
        {
            if (!ExecutarValidacao(validacao: new FornecedorValidation(), entidade: fornecedor) || !ExecutarValidacao(validacao: new EnderecoValidation(), entidade: fornecedor.Endereco))
                return;

            if (_fornecedorRepository.Buscar(f => f.Documento.Equals(fornecedor.Documento)).Result.Any())
            {
                Notificar("Já existe fornecedor com este documento informado");
                return;
            }

            await _fornecedorRepository.Adicionar(fornecedor);
        }

        public async Task Atualizar(Fornecedor fornecedor)
        {
            if (!ExecutarValidacao(new FornecedorValidation(), fornecedor))
                return;

            if (_fornecedorRepository.Buscar(f => f.Documento == fornecedor.Documento && f.Id != fornecedor.Id).Result.Any())
            {
                Notificar("Já existe um fornecedor com este documento informado");
                return;
            }

            await _fornecedorRepository.Atualizar(fornecedor);
        }

        public async Task AtualizarEndereco(Endereco endereco)
        {
            if (!ExecutarValidacao(new EnderecoValidation(), endereco))
                return;

            await _enderecoRepository.Atualizar(endereco);
        }

        public async Task Remover(Guid id)
        {
            if (_fornecedorRepository.ObterFornecedorProdutosEndereco(id).Result.Produtos.Any())
            {
                Notificar("O fornecedor possui produtos cadastrados");
                return;
            }

            await _fornecedorRepository.Remover(id);
        }

        public void Dispose()
        {
            _fornecedorRepository?.Dispose();
            _enderecoRepository?.Dispose();
        }

        #endregion Public Methods
    }
}
