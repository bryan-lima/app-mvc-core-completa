using AutoMapper;
using DevIO.App.Extensions;
using DevIO.App.ViewModels;
using DevIO.Business.Interfaces;
using DevIO.Business.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace DevIO.App.Controllers
{
    [Authorize]
    public class ProdutosController : BaseController
    {
        private readonly IFornecedorRepository _fornecedorRepository;
        private readonly IMapper _mapper;
        private readonly IProdutoRepository _produtoRepository;
        private readonly IProdutoService _produtoService;

        public ProdutosController(IFornecedorRepository fornecedorRepository,
                                  IMapper mapper,
                                  INotificador notificador,
                                  IProdutoRepository produtoRepository,
                                  IProdutoService produtoService) : base(notificador)
        {
            _fornecedorRepository = fornecedorRepository;
            _mapper = mapper;
            _produtoRepository = produtoRepository;
            _produtoService = produtoService;
        }

        [AllowAnonymous]
        [Route("lista-de-produtos")]
        public async Task<IActionResult> Index()
        {
            return View(_mapper.Map<IEnumerable<ProdutoViewModel>>(await _produtoRepository.ObterProdutosFornecedores()));
        }

        [AllowAnonymous]
        [Route("dados-do-produto/{id:guid}")]
        public async Task<IActionResult> Details(Guid id)
        {
            ProdutoViewModel _produtoViewModel = await ObterProduto(id);

            if (_produtoViewModel == null)
                return NotFound();

            return View(_produtoViewModel);
        }

        [ClaimsAuthorize("Produto", "Adicionar")]
        [Route("novo-produto")]
        public async Task<IActionResult> Create()
        {
            return View(await PopularFornecedores(new ProdutoViewModel()));
        }

        [ClaimsAuthorize("Produto", "Adicionar")]
        [Route("novo-produto")]
        [HttpPost]
        public async Task<IActionResult> Create(ProdutoViewModel produtoViewModel)
        {
            produtoViewModel = await PopularFornecedores(produtoViewModel);

            if (!ModelState.IsValid)
                return View(produtoViewModel);

            string _prefixo = $"{Guid.NewGuid()}_";

            if (!await UploadArquivo(produtoViewModel.ImagemUpload, _prefixo))
                return View(produtoViewModel);

            produtoViewModel.Imagem = _prefixo + produtoViewModel.ImagemUpload.FileName;

            await _produtoService.Adicionar(_mapper.Map<Produto>(produtoViewModel));

            if (!OperacaoValida())
                return View(produtoViewModel);

            return RedirectToAction(nameof(Index));
        }

        [ClaimsAuthorize("Produto", "Editar")]
        [Route("editar-produto/{id:guid}")]
        public async Task<IActionResult> Edit(Guid id)
        {
            ProdutoViewModel _produtoViewModel = await ObterProduto(id);

            if (_produtoViewModel == null)
                return NotFound();

            return View(_produtoViewModel);
        }

        [ClaimsAuthorize("Produto", "Editar")]
        [Route("editar-produto/{id:guid}")]
        [HttpPost]
        public async Task<IActionResult> Edit(Guid id, ProdutoViewModel produtoViewModel)
        {
            if (id != produtoViewModel.Id)
                return NotFound();

            ProdutoViewModel _produtoAtualizacao = await ObterProduto(id);

            produtoViewModel.Fornecedor = _produtoAtualizacao.Fornecedor;
            produtoViewModel.Imagem = _produtoAtualizacao.Imagem;

            if (!ModelState.IsValid)
                return View(produtoViewModel);

            if (produtoViewModel.ImagemUpload != null)
            {
                string _prefixo = $"{Guid.NewGuid()}_";

                if (!await UploadArquivo(produtoViewModel.ImagemUpload, _prefixo))
                    return View(produtoViewModel);

                _produtoAtualizacao.Imagem = _prefixo + produtoViewModel.ImagemUpload.FileName;
            }

            _produtoAtualizacao.Nome = produtoViewModel.Nome;
            _produtoAtualizacao.Descricao = produtoViewModel.Descricao;
            _produtoAtualizacao.Valor = produtoViewModel.Valor;
            _produtoAtualizacao.Ativo = produtoViewModel.Ativo;

            await _produtoService.Atualizar(_mapper.Map<Produto>(_produtoAtualizacao));

            if (!OperacaoValida())
                return View(produtoViewModel);

            return RedirectToAction(nameof(Index));
        }

        [ClaimsAuthorize("Produto", "Excluir")]
        [Route("excluir-produto/{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            ProdutoViewModel _produtoViewModel = await ObterProduto(id);

            if (_produtoViewModel == null)
                return NotFound();

            return View(_produtoViewModel);
        }

        [ClaimsAuthorize("Produto", "Excluir")]
        [Route("excluir-produto/{id:guid}")]
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            ProdutoViewModel _produtoViewModel = await ObterProduto(id);

            if (_produtoViewModel == null)
                return NotFound();

            await _produtoService.Remover(id);

            if (!OperacaoValida())
                return View(_produtoViewModel);

            TempData["Sucesso"] = "Produto excluído com sucesso!";

            return RedirectToAction(nameof(Index));
        }

        private async Task<ProdutoViewModel> ObterProduto(Guid id)
        {
            ProdutoViewModel _produto = _mapper.Map<ProdutoViewModel>(await _produtoRepository.ObterProdutoFornecedor(id));
            _produto.Fornecedores = _mapper.Map<IEnumerable<FornecedorViewModel>>(await _fornecedorRepository.ObterTodos());

            return _produto;
        }

        private async Task<ProdutoViewModel> PopularFornecedores(ProdutoViewModel produto)
        {
            produto.Fornecedores = _mapper.Map<IEnumerable<FornecedorViewModel>>(await _fornecedorRepository.ObterTodos());

            return produto;
        }

        private async Task<bool> UploadArquivo(IFormFile arquivo, string prefixo)
        {
            if (arquivo.Length <= 0)
                return false;

            string _path = Path.Combine(path1: Directory.GetCurrentDirectory(),
                                        path2: "wwwroot/imagens",
                                        path3: $"{prefixo}{arquivo.FileName}");

            if (System.IO.File.Exists(_path))
            {
                ModelState.AddModelError(key: string.Empty,
                                         errorMessage: "Já existe um arquivo com este nome!");

                return false;
            }

            // Gravar a imagem em disco
            using (FileStream stream = new FileStream(_path, FileMode.Create))
            {
                await arquivo.CopyToAsync(stream);
            }

            return true;
        }
    }
}
