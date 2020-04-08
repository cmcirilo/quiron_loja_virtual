﻿using System.Web.Mvc;
using Quiron.LojaVirtual.Dominio.Repositorio;
using Quiron.LojaVirtual.Web.V2.Models;

namespace Quiron.LojaVirtual.Web.V2.Controllers
{
    public class NavController : Controller
    {
        private ProdutoModeloRepositorio _repositorio;
        private ProdutosViewModel _model;
        private MenuRepositorio _menuRepositorio;

        public ActionResult Index()
        {
            _repositorio = new ProdutoModeloRepositorio();
            var produtos = _repositorio.ObterProdutosVitrine();
            _model = new ProdutosViewModel { Produtos = produtos };
            return View(_model);
        }

        [Route("nav/{id}/{marca}")]
        public ActionResult ObterProdutosPorMarcas(string id, string marca)
        {
            _repositorio = new ProdutoModeloRepositorio();
            var produtos = _repositorio.ObterProdutosVitrine(marca: id);
            _model = new ProdutosViewModel { Produtos = produtos, Titulo = marca };
            return View("Navegacao", _model);
        }

        [Route("nav/times/{id}/{clube}")]
        public ActionResult ObterProdutosPorClubes(string id, string clube)
        {
            _repositorio = new ProdutoModeloRepositorio();
            var produtos = _repositorio.ObterProdutosVitrine(linha: id);
            _model = new ProdutosViewModel { Produtos = produtos, Titulo = clube };
            return View("Navegacao", _model);
        }

        [Route("nav/genero/{id}/{genero}")]
        public ActionResult ObterProdutosPorGenero(string id, string genero)
        {
            _repositorio = new ProdutoModeloRepositorio();
            var produtos = _repositorio.ObterProdutosVitrine(genero: id);
            _model = new ProdutosViewModel { Produtos = produtos, Titulo = genero };
            return View("Navegacao", _model);
        }

        #region [Tenis por Categoria]

        /// <summary>
        /// Obtem categoria de tenis exibido no menu
        /// </summary>
        /// <returns></returns>
        [ChildActionOnly]
        [OutputCache(Duration = 3600, VaryByParam = "*")]
        public ActionResult TenisCategoria()
        {
            _menuRepositorio = new MenuRepositorio();
            var categorias = _menuRepositorio.ObterTenisCategoria();
            var subGrupo = _menuRepositorio.SubGrupoTenis();

            SubGrupoCategoriasViewModel model = new SubGrupoCategoriasViewModel
            {
                Categorias = categorias,
                SubGrupo = subGrupo
            };
            return PartialView("_TenisCategoria", model);
        }

        /// <summary>
        /// Retorna uma vitrine com tenis por categoria
        /// </summary>
        /// <param name="subGrupoCodigo"></param>
        /// <param name="categoriaCodigo"></param>
        /// <param name="categoriaDescricao"></param>
        /// <returns></returns>
        [Route("calcados/{subGrupoCodigo}/tenis/{categoriaCodigo}/{categoriaDescricao}")]
        public ActionResult ObterTenisPorCategoria(string subGrupoCodigo, string categoriaCodigo, string categoriaDescricao)
        {
            _repositorio = new ProdutoModeloRepositorio();
            var produtos = _repositorio.ObterProdutosVitrine(categoriaCodigo, subGrupo: subGrupoCodigo);
            _model = new ProdutosViewModel { Produtos = produtos, Titulo = categoriaDescricao.UpperCaseFirst() };
            return View("Navegacao", _model);

        }

        #endregion

        #region [Casual]

        [ChildActionOnly]
        [OutputCache(Duration = 3600, VaryByParam = "*")]
        public ActionResult CasualSubGrupo()
        {
            _menuRepositorio = new MenuRepositorio();
            var casual = _menuRepositorio.ObterModalidadeCasual();
            var subGrupo = _menuRepositorio.ObterCasualSubGrupo();

            var model = new ModalidadeSubGrupoViewModel
            {
                Modalidade = casual,
                SubGrupos = subGrupo
            };

            return PartialView("_CasualSubGrupo", model);

        }

        [Route("{modalidadeCodigo}/casual/{subGrupoCodigo}/{subGrupoDescricao}")]
        public ActionResult ObterModalidadeSubGrupo(string modalidadeCodigo,
            string subGrupoCodigo, string subGrupoDescricao)
        {
            _repositorio = new ProdutoModeloRepositorio();
            var produtos = _repositorio.ObterProdutosVitrine(
                modalidade: modalidadeCodigo, subGrupo: subGrupoCodigo);

            _model = new ProdutosViewModel
            {
                Produtos = produtos,
                Titulo = subGrupoDescricao.UpperCaseFirst()
            };

            return View("Navegacao", _model);
        }

        #endregion

        #region [Suplementos]

        [ChildActionOnly]
        [OutputCache(Duration = 3600, VaryByParam = "*")]
        public ActionResult Suplementos()
        {
            _menuRepositorio = new MenuRepositorio();
            var categoria = _menuRepositorio.Suplemento();
            var subGrupos = _menuRepositorio.ObterSuplementos();

            CategoriaSubGruposViewModel model = new CategoriaSubGruposViewModel
            {
                Categoria = categoria,
                SubGrupos = subGrupos
            };

            return PartialView("_Suplementos", model);
        }

        [Route("{categoriaCodigo}/suplementos/{subGrupoCodigo}/{subGrupoDescricao}")]
        public ActionResult ObterCategoriaSubGrupos(string categoriaCodigo,
            string subGrupoCodigo, string subGrupoDescricao)
        {
            _repositorio = new ProdutoModeloRepositorio();
            var produtos = _repositorio.ObterProdutosVitrine(categoria: categoriaCodigo,
                subGrupo: subGrupoCodigo);
            _model = new ProdutosViewModel
            {
                Produtos = produtos,
                Titulo = subGrupoDescricao.UpperCaseFirst()
            };

            return View("Navegacao", _model);
        }
        #endregion

        #region [ Consulta - Busca por Produto ]

        public ActionResult ConsultarProduto(string termo)
        {
            _repositorio = new ProdutoModeloRepositorio();
            var produtos = _repositorio.ObterProdutosVitrine(busca: termo);
            _model = new ProdutosViewModel
            {
                Produtos = produtos,
                Titulo = termo.UpperCaseFirst()
            };
            return View("Navegacao", _model);
        }

        #endregion [ Consulta - Busca por Produto ]
    }
}