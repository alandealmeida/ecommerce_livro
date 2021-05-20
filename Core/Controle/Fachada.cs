using Core.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Aplicacao;
using Dominio;
using Core.DAO;
using Core.Negocio;
using Dominio.Cliente;
using Dominio.Livro;
using Dominio.Venda;
using Dominio.Analise;

namespace Core.Controle
{
    public sealed class Fachada : IFachada
    {
        private Dictionary<string, IDAO> daos;

        private Dictionary<string, Dictionary<string, List<IStrategy>>> rns;
        private Resultado resultado;
        private static readonly Fachada Instance = new Fachada();

        /*
         * INÍCIO do CONSTRUTOR da Fachada 
         * ------------------------------------------------------
         */
        private Fachada()
        {
            daos = new Dictionary<string, IDAO>();
            rns = new Dictionary<string, Dictionary<string, List<IStrategy>>>();

            // instâncias das Strategys
            ComplementoDtCadastro complementoDtCadastro = new ComplementoDtCadastro();
            DeleteCartao deleteCartao = new DeleteCartao();
            DeleteEndereco deleteEndereco = new DeleteEndereco();
            DeleteClienteCartoes deleteClienteXCartoes = new DeleteClienteCartoes();
            DeleteClienteEnderecos deleteClienteXEnderecos = new DeleteClienteEnderecos();
            ValidadorClienteCartao validadorClienteCC = new ValidadorClienteCartao();
            ValidadorClienteEndereco validadorClienteEndereco = new ValidadorClienteEndereco();
            ValidadorCartaoCredito validadorCartaoCredito = new ValidadorCartaoCredito();
            ValidadorDadosCliente valDadosClientePessoaFisica = new ValidadorDadosCliente();
            ValidadorEndereco valEndereco = new ValidadorEndereco();
            ParametroExcluir paramExcluir = new ParametroExcluir();
            ValidadorExistenciaCPF valExistenciaCPF = new ValidadorExistenciaCPF();
            ValidadorExistenciaEmail valExistenciaEmail = new ValidadorExistenciaEmail();
            ValidadorAtivacaoInativacaoLivro valAtivacaoInativacaoLivro = new ValidadorAtivacaoInativacaoLivro();
            ValidadorDadosEstoque valDadosEstoque = new ValidadorDadosEstoque();
            ValidadorDadosPedido valDadosPedido = new ValidadorDadosPedido();
            ValidadorStatusPedido valStatusPedido = new ValidadorStatusPedido();
            ValidadorAtualizaPedido valAtualizaPedido = new ValidadorAtualizaPedido();


            // instâncias das DAOs
            EnderecoDAO enderecoDAO = new EnderecoDAO();
            CidadeDAO cidadeDAO = new CidadeDAO();
            EstadoDAO estadoDAO = new EstadoDAO();
            PaisDAO paisDAO = new PaisDAO();
            ClienteCartaoDAO clientePFXCartaoDAO = new ClienteCartaoDAO();
            ClienteEnderecoDAO ClienteEnderecoDAO = new ClienteEnderecoDAO();
            CartaoCreditoDAO ccDAO = new CartaoCreditoDAO();
            BandeiraDAO bandeiraDAO = new BandeiraDAO();
            TipoTelefoneDAO tipoTelefoneDAO = new TipoTelefoneDAO();
            TipoResidenciaDAO tipoResidenciaDAO = new TipoResidenciaDAO();
            TipoLogradouroDAO tipoLogradouroDAO = new TipoLogradouroDAO();
            ClienteDAO clientePFDAO = new ClienteDAO();
            CategoriaMotivoDAO categoriaMotivoDAO = new CategoriaMotivoDAO();
            CategoriaLivroDAO categoriaLivroDAO = new CategoriaLivroDAO();
            LivroCategoriaDAO livroCategoriaDAO = new LivroCategoriaDAO();
            ImagemLivroDAO imagemLivroDAO = new ImagemLivroDAO();
            EditoraDAO editoraDAO = new EditoraDAO();
            LivroDAO livroDAO = new LivroDAO();
            EstoqueDAO estoqueDAO = new EstoqueDAO();
            FornecedorDAO fornecedorDAO = new FornecedorDAO();
            CupomDAO cupomDAO = new CupomDAO();
            TipoCupomDAO tipoCupomDAO = new TipoCupomDAO();
            PedidoCupomDAO clientePFXCupomDAO = new PedidoCupomDAO();
            StatusPedidoDAO statusPedidoDAO = new StatusPedidoDAO();
            PedidoDetalheDAO pedidoDetalheDAO = new PedidoDetalheDAO();
            CartaoPedidoDAO ccPedidoDAO = new CartaoPedidoDAO();
            PedidoDAO pedidoDAO = new PedidoDAO();
            AnaliseDAO analiseDAO = new AnaliseDAO();

            // adicionando as DAOs ao Mapa daos já indicando o indice (nome da classe domínio) de cada um
            daos.Add(typeof(Endereco).Name, enderecoDAO);
            daos.Add(typeof(Cidade).Name, cidadeDAO);
            daos.Add(typeof(Estado).Name, estadoDAO);
            daos.Add(typeof(Pais).Name, paisDAO);
            daos.Add(typeof(ClienteCartao).Name, clientePFXCartaoDAO);
            daos.Add(typeof(ClienteEndereco).Name, ClienteEnderecoDAO);
            daos.Add(typeof(CartaoCredito).Name, ccDAO);
            daos.Add(typeof(Bandeira).Name, bandeiraDAO);
            daos.Add(typeof(TipoTelefone).Name, tipoTelefoneDAO);
            daos.Add(typeof(TipoResidencia).Name, tipoResidenciaDAO);
            daos.Add(typeof(TipoLogradouro).Name, tipoLogradouroDAO);
            daos.Add(typeof(Cliente).Name, clientePFDAO);
            daos.Add(typeof(CategoriaMotivo).Name, categoriaMotivoDAO);
            daos.Add(typeof(Categoria).Name, categoriaLivroDAO);
            daos.Add(typeof(LivroCategoria).Name, livroCategoriaDAO);
            daos.Add(typeof(ImagemLivro).Name, imagemLivroDAO);
            daos.Add(typeof(Editora).Name, editoraDAO);
            daos.Add(typeof(Livro).Name, livroDAO);
            daos.Add(typeof(Estoque).Name, estoqueDAO);
            daos.Add(typeof(Fornecedor).Name, fornecedorDAO);
            daos.Add(typeof(Cupom).Name, cupomDAO);
            daos.Add(typeof(TipoCupom).Name, tipoCupomDAO);
            daos.Add(typeof(PedidoCupom).Name, clientePFXCupomDAO);
            daos.Add(typeof(StatusPedido).Name, statusPedidoDAO);
            daos.Add(typeof(PedidoDetalhe).Name, pedidoDetalheDAO);
            daos.Add(typeof(CartaoCreditoPedido).Name, ccPedidoDAO);
            daos.Add(typeof(Pedido).Name, pedidoDAO);
            daos.Add(typeof(Analise).Name, analiseDAO);

            #region CRIAÇÃO DA LISTA DE STRATEGYS

            /*
             * CLIENTE X ENDEREÇO - COMEÇO DA CRIAÇÃO DA LISTA DE STRATEGYS----------------------------------
             */
            List<IStrategy> rnsSalvarClienteEndereco = new List<IStrategy>();
            rnsSalvarClienteEndereco.Add(validadorClienteEndereco);
            List<IStrategy> rnsConsultarClienteEndereco = new List<IStrategy>();
            /*
             * CLIENTE X ENDEREÇO - FIM ---------------------------------------------------------------------
             */

            // criando as listas que conterão as Strategys referente a cada classe
            // e adicionando as strategy nas listas
            /*
             * ENDEREÇO - COMEÇO DA CRIAÇÃO DA LISTA DE STRATEGYS----------------------------------
             */
            List<IStrategy> rnsAlterarEndereco = new List<IStrategy>();
            rnsAlterarEndereco.Add(valEndereco);
            List<IStrategy> rnsExcluirEndereco = new List<IStrategy>();
            rnsExcluirEndereco.Add(deleteEndereco);
            rnsExcluirEndereco.Add(paramExcluir);
            List<IStrategy> rnsConsultarEndereco = new List<IStrategy>();
            /*
             * ENDEREÇO - FIM ---------------------------------------------------------------------
             */

            /*
             * CIDADE - COMEÇO DA CRIAÇÃO DA LISTA DE STRATEGYS----------------------------------
             */
            List<IStrategy> rnsConsultarCidade = new List<IStrategy>();
            /*
             * CIDADE - FIM ---------------------------------------------------------------------
             */

            /*
             * ESTADO - COMEÇO DA CRIAÇÃO DA LISTA DE STRATEGYS----------------------------------
             */
            List<IStrategy> rnsConsultarEstado = new List<IStrategy>();
            /*
             * ESTADO - FIM ---------------------------------------------------------------------
             */

            /*
             * PAIS - COMEÇO DA CRIAÇÃO DA LISTA DE STRATEGYS----------------------------------
             */
            List<IStrategy> rnsConsultarPais = new List<IStrategy>();
            /*
             * PAIS - FIM ---------------------------------------------------------------------
             */

            /*
             * ClientePF - COMEÇO DA CRIAÇÃO DA LISTA DE STRATEGYS----------------------------------
             */
            List<IStrategy> rnsSalvarClientePF = new List<IStrategy>();
            rnsSalvarClientePF.Add(valDadosClientePessoaFisica);
            rnsSalvarClientePF.Add(valExistenciaCPF);
            rnsSalvarClientePF.Add(valExistenciaEmail);
            rnsSalvarClientePF.Add(complementoDtCadastro);
            List<IStrategy> rnsAlterarClientePF = new List<IStrategy>();
            rnsAlterarClientePF.Add(valDadosClientePessoaFisica);
            List<IStrategy> rnsExcluirClientePF = new List<IStrategy>();
            rnsExcluirClientePF.Add(deleteClienteXCartoes);
            rnsExcluirClientePF.Add(deleteClienteXEnderecos);
            rnsExcluirClientePF.Add(paramExcluir);
            List<IStrategy> rnsConsultarClientePF = new List<IStrategy>();
            /*
             * ClientePF - FIM ---------------------------------------------------------------------
             */

            /*
             * CLIENTE X CARTÃO - COMEÇO DA CRIAÇÃO DA LISTA DE STRATEGYS----------------------------------
             */
            List<IStrategy> rnsSalvarClienteCartao = new List<IStrategy>();
            rnsSalvarClienteCartao.Add(validadorClienteCC);
            List<IStrategy> rnsConsultarClienteCartao = new List<IStrategy>();
            /*
             * CLIENTE X CARTÃO - FIM ---------------------------------------------------------------------
             */

            /*
             * CARTÃO - COMEÇO DA CRIAÇÃO DA LISTA DE STRATEGYS----------------------------------
             */
            List<IStrategy> rnsAlterarCartao = new List<IStrategy>();
            rnsAlterarCartao.Add(validadorCartaoCredito);
            List<IStrategy> rnsExcluirCartao = new List<IStrategy>();
            rnsExcluirCartao.Add(deleteCartao);
            rnsExcluirCartao.Add(paramExcluir);
            List<IStrategy> rnsConsultarCartao = new List<IStrategy>();
            /*
             * CARTÃO - FIM ---------------------------------------------------------------------
             */

            /*
             * Bandeira - COMEÇO DA CRIAÇÃO DA LISTA DE STRATEGYS----------------------------------
             */
            List<IStrategy> rnsConsultarBandeira = new List<IStrategy>();
            /*
             * Bandeira - FIM ---------------------------------------------------------------------
             */

            /*
             * TipoTelefone - COMEÇO DA CRIAÇÃO DA LISTA DE STRATEGYS----------------------------------
             */
            List<IStrategy> rnsConsultarTipoTelefone = new List<IStrategy>();
            /*
             * TipoTelefone - FIM ---------------------------------------------------------------------
             */

            /*
             * TipoResidencia - COMEÇO DA CRIAÇÃO DA LISTA DE STRATEGYS----------------------------------
             */
            List<IStrategy> rnsConsultarTipoResidencia = new List<IStrategy>();
            /*
             * TipoResidencia - FIM ---------------------------------------------------------------------
             */

            /*
             * TipoLogradouro - COMEÇO DA CRIAÇÃO DA LISTA DE STRATEGYS----------------------------------
             */
            List<IStrategy> rnsConsultarTipoLogradouro = new List<IStrategy>();
            /*
             * TipoLogradouro - FIM ---------------------------------------------------------------------
             */

            /*
             * CategoriaMotivo - COMEÇO DA CRIAÇÃO DA LISTA DE STRATEGYS----------------------------------
             */
            List<IStrategy> rnsConsultarCategoriaMotivo = new List<IStrategy>();
            /*
             * CategoriaMotivo - FIM ---------------------------------------------------------------------
             */

            /*
             * CategoriaLivro - COMEÇO DA CRIAÇÃO DA LISTA DE STRATEGYS----------------------------------
             */
            List<IStrategy> rnsConsultarCategoriaLivro = new List<IStrategy>();
            /*
             * CategoriaLivro - FIM ---------------------------------------------------------------------
             */

            /*
             * LivroXCategoria - COMEÇO DA CRIAÇÃO DA LISTA DE STRATEGYS----------------------------------
             */
            List<IStrategy> rnsConsultarLivroCategoria = new List<IStrategy>();
            /*
             * LivroXCategoria - FIM ---------------------------------------------------------------------
             */

            /*
             * ImagemLivro - COMEÇO DA CRIAÇÃO DA LISTA DE STRATEGYS----------------------------------
             */
            List<IStrategy> rnsConsultarImagemLivro = new List<IStrategy>();
            /*
             * ImagemLivro - FIM ---------------------------------------------------------------------
             */

            /*
             * EDITORA - COMEÇO DA CRIAÇÃO DA LISTA DE STRATEGYS----------------------------------
             */
            List<IStrategy> rnsConsultarEditora = new List<IStrategy>();
            /*
             * EDITORA - FIM ---------------------------------------------------------------------
             */

            /*
             * LIVRO - COMEÇO DA CRIAÇÃO DA LISTA DE STRATEGYS----------------------------------
             */
            List<IStrategy> rnsAlterarLivro = new List<IStrategy>();
            rnsAlterarLivro.Add(valAtivacaoInativacaoLivro);
            List<IStrategy> rnsConsultarLivro = new List<IStrategy>();
            /*
             * LIVRO - FIM ---------------------------------------------------------------------
             */

            /*
             * ESTOQUE - COMEÇO DA CRIAÇÃO DA LISTA DE STRATEGYS----------------------------------
             */
            List<IStrategy> rnsSalvarEstoque = new List<IStrategy>();
            rnsSalvarEstoque.Add(valDadosEstoque);
            rnsSalvarEstoque.Add(complementoDtCadastro);
            List<IStrategy> rnsAlterarEstoque = new List<IStrategy>();
            rnsAlterarEstoque.Add(valDadosEstoque);
            rnsAlterarEstoque.Add(complementoDtCadastro);
            List<IStrategy> rnsExcluirEstoque = new List<IStrategy>();
            List<IStrategy> rnsConsultarEstoque = new List<IStrategy>();
            /*
             * ESTOQUE - FIM ---------------------------------------------------------------------
             */

            /*
             * FORNECEDOR - COMEÇO DA CRIAÇÃO DA LISTA DE STRATEGYS----------------------------------
             */
            List<IStrategy> rnsConsultarFornecedor = new List<IStrategy>();
            /*
             * FORNECEDOR - FIM ---------------------------------------------------------------------
             */

            /*
             * CUPOM - COMEÇO DA CRIAÇÃO DA LISTA DE STRATEGYS----------------------------------
             */
            List<IStrategy> rnsSalvarCupom = new List<IStrategy>();
            List<IStrategy> rnsAlterarCupom = new List<IStrategy>();
            List<IStrategy> rnsConsultarCupom = new List<IStrategy>();
            /*
             * CUPOM - FIM ---------------------------------------------------------------------
             */

            /*
             * TipoCupom - COMEÇO DA CRIAÇÃO DA LISTA DE STRATEGYS----------------------------------
             */
            List<IStrategy> rnsConsultarTipoCupom = new List<IStrategy>();
            /*
             * TipoCupom - FIM ---------------------------------------------------------------------
             */

            /*
             * CLIENTE X CUPOM - COMEÇO DA CRIAÇÃO DA LISTA DE STRATEGYS----------------------------------
             */
            List<IStrategy> rnsSalvarClienteCupom = new List<IStrategy>();
            List<IStrategy> rnsConsultarClienteCupom = new List<IStrategy>();
            /*
             * CLIENTE X CUPOM - FIM ---------------------------------------------------------------------
             */

            /*
             * StatusPedido - COMEÇO DA CRIAÇÃO DA LISTA DE STRATEGYS----------------------------------
             */
            List<IStrategy> rnsConsultarStatusPedido = new List<IStrategy>();
            /*
             * StatusPedido - FIM ---------------------------------------------------------------------
             */

            /*
             * CCPedido - COMEÇO DA CRIAÇÃO DA LISTA DE STRATEGYS----------------------------------
             */
            List<IStrategy> rnsConsultarCCPedido = new List<IStrategy>();
            /*
             * CCPedido - FIM ---------------------------------------------------------------------
             */

            /*
             * PedidoDetalhe - COMEÇO DA CRIAÇÃO DA LISTA DE STRATEGYS----------------------------------
             */
            List<IStrategy> rnsConsultarPedidoDetalhe = new List<IStrategy>();
            /*
             * PedidoDetalhe - FIM ---------------------------------------------------------------------
             */

            /*
             * Pedido - COMEÇO DA CRIAÇÃO DA LISTA DE STRATEGYS----------------------------------
             */
            List<IStrategy> rnsSalvarPedido = new List<IStrategy>();
            rnsSalvarPedido.Add(complementoDtCadastro);
            rnsSalvarPedido.Add(valDadosPedido);
            rnsSalvarPedido.Add(valStatusPedido);
            List<IStrategy> rnsAlterarPedido = new List<IStrategy>();
            rnsAlterarPedido.Add(complementoDtCadastro);
            rnsAlterarPedido.Add(valStatusPedido);
            rnsAlterarPedido.Add(valAtualizaPedido);
            List<IStrategy> rnsConsultarPedido = new List<IStrategy>();
            /*
             * Pedido - FIM ---------------------------------------------------------------------
             */

            /*
             * Analise - COMEÇO DA CRIAÇÃO DA LISTA DE STRATEGYS----------------------------------
             */
            List<IStrategy> rnsConsultarAnalise = new List<IStrategy>();
            /*
             * Analise - FIM ---------------------------------------------------------------------
             */

            #endregion


            #region CRIAÇÃO DA LISTA DE REGRAS PARA CADA OPERAÇÂO

            // criando mapa indicando o indice (operação) e a lista das Stategys(regras) de cada operação
            /*
             * CIDADE - COMEÇO DA CRIAÇÃO DA LISTA DE REGAS PARA CADA OPERAÇÂO -------------------------
             */
            Dictionary<string, List<IStrategy>> rnsCidade = new Dictionary<string, List<IStrategy>>();
            rnsCidade.Add("CONSULTAR", rnsConsultarCidade);
            /*
             * CIDADE - FIM ----------------------------------------------------------------------------
             */

            /*
             * ESTADO - COMEÇO DA CRIAÇÃO DA LISTA DE REGAS PARA CADA OPERAÇÂO -------------------------
             */
            Dictionary<string, List<IStrategy>> rnsEstado = new Dictionary<string, List<IStrategy>>();
            rnsEstado.Add("CONSULTAR", rnsConsultarEstado);
            /*
             * ESTADO - FIM ----------------------------------------------------------------------------
             */

            /*
             * PAIS - COMEÇO DA CRIAÇÃO DA LISTA DE REGAS PARA CADA OPERAÇÂO -------------------------
             */
            Dictionary<string, List<IStrategy>> rnsPais = new Dictionary<string, List<IStrategy>>();
            rnsPais.Add("CONSULTAR", rnsConsultarPais);
            /*
             * PAIS - FIM ----------------------------------------------------------------------------
             */

            /*
             * CLIENTE X ENDEREÇO - COMEÇO DA CRIAÇÃO DA LISTA DE REGAS PARA CADA OPERAÇÂO -------------------------
             */
            Dictionary<string, List<IStrategy>> rnsClienteEndereco = new Dictionary<string, List<IStrategy>>();
            rnsClienteEndereco.Add("SALVAR", rnsSalvarClienteEndereco);
            rnsClienteEndereco.Add("CONSULTAR", rnsConsultarClienteEndereco);
            /*
             * CLIENTE X ENDEREÇO - FIM ----------------------------------------------------------------------------
             */

            /*
             * ENDEREÇO - COMEÇO DA CRIAÇÃO DA LISTA DE REGAS PARA CADA OPERAÇÂO -------------------------
             */
            Dictionary<string, List<IStrategy>> rnsEndereco = new Dictionary<string, List<IStrategy>>();
            rnsEndereco.Add("ALTERAR", rnsAlterarEndereco);
            rnsEndereco.Add("EXCLUIR", rnsExcluirEndereco);
            rnsEndereco.Add("CONSULTAR", rnsConsultarEndereco);
            /*
             * ENDEREÇO - FIM ----------------------------------------------------------------------------
             */

            /*
             * ClientePF - COMEÇO DA CRIAÇÃO DA LISTA DE REGAS PARA CADA OPERAÇÂO -------------------------
             */
            Dictionary<string, List<IStrategy>> rnsClientePF = new Dictionary<string, List<IStrategy>>();
            rnsClientePF.Add("SALVAR", rnsSalvarClientePF);
            rnsClientePF.Add("ALTERAR", rnsAlterarClientePF);
            rnsClientePF.Add("EXCLUIR", rnsExcluirClientePF);
            rnsClientePF.Add("CONSULTAR", rnsConsultarClientePF);
            /*
             * ClientePF - FIM ----------------------------------------------------------------------------
             */

            /*
             * CLIENTE X CARTÃO - COMEÇO DA CRIAÇÃO DA LISTA DE REGAS PARA CADA OPERAÇÂO -------------------------
             */
            Dictionary<string, List<IStrategy>> rnsClienteCartao = new Dictionary<string, List<IStrategy>>();
            rnsClienteCartao.Add("SALVAR", rnsSalvarClienteCartao);
            rnsClienteCartao.Add("CONSULTAR", rnsConsultarClienteCartao);
            /*
             * CLIENTE X CARTÃO - FIM ----------------------------------------------------------------------------
             */

            /*
             * CARTÃO - COMEÇO DA CRIAÇÃO DA LISTA DE REGAS PARA CADA OPERAÇÂO -------------------------
             */
            Dictionary<string, List<IStrategy>> rnsCartao = new Dictionary<string, List<IStrategy>>();
            rnsCartao.Add("ALTERAR", rnsAlterarCartao);
            rnsCartao.Add("EXCLUIR", rnsExcluirCartao);
            rnsCartao.Add("CONSULTAR", rnsConsultarCartao);
            /*
             * CARTÃO - FIM ----------------------------------------------------------------------------
             */

            /*
             * Bandeira - COMEÇO DA CRIAÇÃO DA LISTA DE REGAS PARA CADA OPERAÇÂO -------------------------
             */
            Dictionary<string, List<IStrategy>> rnsBandeira = new Dictionary<string, List<IStrategy>>();
            rnsBandeira.Add("CONSULTAR", rnsConsultarBandeira);
            /*
             * Bandeira - FIM ----------------------------------------------------------------------------
             */

            /*
             * TipoTelefone - COMEÇO DA CRIAÇÃO DA LISTA DE REGAS PARA CADA OPERAÇÂO -------------------------
             */
            Dictionary<string, List<IStrategy>> rnsTipoTelefone = new Dictionary<string, List<IStrategy>>();
            rnsTipoTelefone.Add("CONSULTAR", rnsConsultarTipoTelefone);
            /*
             * TipoTelefone - FIM ----------------------------------------------------------------------------
             */

            /*
             * TipoResidencia - COMEÇO DA CRIAÇÃO DA LISTA DE REGAS PARA CADA OPERAÇÂO -------------------------
             */
            Dictionary<string, List<IStrategy>> rnsTipoResidencia = new Dictionary<string, List<IStrategy>>();
            rnsTipoResidencia.Add("CONSULTAR", rnsConsultarTipoResidencia);
            /*
             * TipoResidencia - FIM ----------------------------------------------------------------------------
             */

            /*
             * TipoLogradouro - COMEÇO DA CRIAÇÃO DA LISTA DE REGAS PARA CADA OPERAÇÂO -------------------------
             */
            Dictionary<string, List<IStrategy>> rnsTipoLogradouro = new Dictionary<string, List<IStrategy>>();
            rnsTipoLogradouro.Add("CONSULTAR", rnsConsultarTipoLogradouro);
            /*
             * TipoLogradouro - FIM ----------------------------------------------------------------------------
             */
            /*
             * CategoriaMotivo - COMEÇO DA CRIAÇÃO DA LISTA DE REGAS PARA CADA OPERAÇÂO -------------------------
             */
            Dictionary<string, List<IStrategy>> rnsCategoriaMotivo = new Dictionary<string, List<IStrategy>>();
            rnsCategoriaMotivo.Add("CONSULTAR", rnsConsultarCategoriaMotivo);
            /*
             * CategoriaMotivo - FIM ----------------------------------------------------------------------------
             */

            /*
             * CategoriaLivro - COMEÇO DA CRIAÇÃO DA LISTA DE REGAS PARA CADA OPERAÇÂO -------------------------
             */
            Dictionary<string, List<IStrategy>> rnsCategoriaLivro = new Dictionary<string, List<IStrategy>>();
            rnsCategoriaLivro.Add("CONSULTAR", rnsConsultarCategoriaLivro);
            /*
             * CategoriaLivro - FIM ----------------------------------------------------------------------------
             */

            /*
             * LivroXCategoria - COMEÇO DA CRIAÇÃO DA LISTA DE REGAS PARA CADA OPERAÇÂO -------------------------
             */
            Dictionary<string, List<IStrategy>> rnsLivroCategoria = new Dictionary<string, List<IStrategy>>();
            rnsLivroCategoria.Add("CONSULTAR", rnsConsultarLivroCategoria);
            /*
             * LivroXCategoria - FIM ----------------------------------------------------------------------------
             */

            /*
             * ImagemLivro - COMEÇO DA CRIAÇÃO DA LISTA DE REGAS PARA CADA OPERAÇÂO -------------------------
             */
            Dictionary<string, List<IStrategy>> rnsImagemLivro = new Dictionary<string, List<IStrategy>>();
            rnsImagemLivro.Add("CONSULTAR", rnsConsultarImagemLivro);
            /*
             * ImagemLivro - FIM ----------------------------------------------------------------------------
             */

            /*
             * EDITORA - COMEÇO DA CRIAÇÃO DA LISTA DE REGAS PARA CADA OPERAÇÂO -------------------------
             */
            Dictionary<string, List<IStrategy>> rnsEditora = new Dictionary<string, List<IStrategy>>();
            rnsEditora.Add("CONSULTAR", rnsConsultarEditora);
            /*
             * EDITORA - FIM ----------------------------------------------------------------------------
             */

            /*
             * LIVRO - COMEÇO DA CRIAÇÃO DA LISTA DE REGAS PARA CADA OPERAÇÂO -------------------------
             */
            Dictionary<string, List<IStrategy>> rnsLivro = new Dictionary<string, List<IStrategy>>();
            rnsLivro.Add("ALTERAR", rnsAlterarLivro);
            rnsLivro.Add("CONSULTAR", rnsConsultarLivro);
            /*
             * LIVRO - FIM ----------------------------------------------------------------------------
             */

            /*
             * ESTOQUE - COMEÇO DA CRIAÇÃO DA LISTA DE REGAS PARA CADA OPERAÇÂO -------------------------
             */
            Dictionary<string, List<IStrategy>> rnsEstoque = new Dictionary<string, List<IStrategy>>();
            rnsEstoque.Add("SALVAR", rnsSalvarEstoque);
            rnsEstoque.Add("ALTERAR", rnsAlterarEstoque);
            rnsEstoque.Add("EXCLUIR", rnsExcluirEstoque);
            rnsEstoque.Add("CONSULTAR", rnsConsultarEstoque);
            /*
             * ESTOQUE - FIM ----------------------------------------------------------------------------
             */

            /*
             * FORNECEDOR - COMEÇO DA CRIAÇÃO DA LISTA DE REGAS PARA CADA OPERAÇÂO -------------------------
             */
            Dictionary<string, List<IStrategy>> rnsFornecedor = new Dictionary<string, List<IStrategy>>();
            rnsFornecedor.Add("CONSULTAR", rnsConsultarFornecedor);
            /*
             * FORNECEDOR - FIM ----------------------------------------------------------------------------
             */

            /*
             * Cupom - COMEÇO DA CRIAÇÃO DA LISTA DE REGAS PARA CADA OPERAÇÂO -------------------------
             */
            Dictionary<string, List<IStrategy>> rnsCupom = new Dictionary<string, List<IStrategy>>();
            rnsCupom.Add("SALVAR", rnsSalvarCupom);
            rnsCupom.Add("ALTERAR", rnsAlterarCupom);
            rnsCupom.Add("CONSULTAR", rnsConsultarCupom);
            /*
             * Cupom - FIM ----------------------------------------------------------------------------
             */

            /*
             * TipoCupom - COMEÇO DA CRIAÇÃO DA LISTA DE REGAS PARA CADA OPERAÇÂO -------------------------
             */
            Dictionary<string, List<IStrategy>> rnsTipoCupom = new Dictionary<string, List<IStrategy>>();
            rnsTipoCupom.Add("CONSULTAR", rnsConsultarTipoCupom);
            /*
             * TipoCupom - FIM ----------------------------------------------------------------------------
             */

            /*
             * CLIENTE X CUPOM - COMEÇO DA CRIAÇÃO DA LISTA DE REGAS PARA CADA OPERAÇÂO -------------------------
             */
            Dictionary<string, List<IStrategy>> rnsClienteCupom = new Dictionary<string, List<IStrategy>>();
            rnsClienteCupom.Add("SALVAR", rnsSalvarClienteCupom);
            rnsClienteCupom.Add("CONSULTAR", rnsConsultarClienteCupom);
            /*
             * CLIENTE X CUPOM - FIM ----------------------------------------------------------------------------
             */

            /*
             * StatusPedido - COMEÇO DA CRIAÇÃO DA LISTA DE REGAS PARA CADA OPERAÇÂO -------------------------
             */
            Dictionary<string, List<IStrategy>> rnsStatusPedido = new Dictionary<string, List<IStrategy>>();
            rnsStatusPedido.Add("CONSULTAR", rnsConsultarStatusPedido);
            /*
             * StatusPedido - FIM ----------------------------------------------------------------------------
             */

            /*
             * CCPedido - COMEÇO DA CRIAÇÃO DA LISTA DE REGAS PARA CADA OPERAÇÂO -------------------------
             */
            Dictionary<string, List<IStrategy>> rnsCCPedido = new Dictionary<string, List<IStrategy>>();
            rnsCCPedido.Add("CONSULTAR", rnsConsultarCCPedido);
            /*
             * CCPedido - FIM ----------------------------------------------------------------------------
             */

            /*
             * PedidoDetalhe - COMEÇO DA CRIAÇÃO DA LISTA DE REGAS PARA CADA OPERAÇÂO -------------------------
             */
            Dictionary<string, List<IStrategy>> rnsPedidoDetalhe = new Dictionary<string, List<IStrategy>>();
            rnsPedidoDetalhe.Add("CONSULTAR", rnsConsultarPedidoDetalhe);
            /*
             * PedidoDetalhe - FIM ----------------------------------------------------------------------------
             */

            /*
             * Pedido - COMEÇO DA CRIAÇÃO DA LISTA DE REGAS PARA CADA OPERAÇÂO -------------------------
             */
            Dictionary<string, List<IStrategy>> rnsPedido = new Dictionary<string, List<IStrategy>>();
            rnsPedido.Add("SALVAR", rnsSalvarPedido);
            rnsPedido.Add("ALTERAR", rnsAlterarPedido);
            rnsPedido.Add("CONSULTAR", rnsConsultarPedido);
            /*
             * Pedido - FIM ----------------------------------------------------------------------------
             */
            /*
            * Analise - COMEÇO DA CRIAÇÃO DA LISTA DE REGAS PARA CADA OPERAÇÂO -------------------------
            */
            Dictionary<string, List<IStrategy>> rnsAnalise = new Dictionary<string, List<IStrategy>>();
            rnsAnalise.Add("CONSULTAR", rnsConsultarAnalise);
            /*
             * Analise - FIM ----------------------------------------------------------------------------
             */

            #endregion

            // adicionando ao mapa geral que conterá todos os mapas
            rns.Add(typeof(ClienteEndereco).Name, rnsClienteEndereco);
            rns.Add(typeof(Endereco).Name, rnsEndereco);
            rns.Add(typeof(Cidade).Name, rnsCidade);
            rns.Add(typeof(Estado).Name, rnsEstado);
            rns.Add(typeof(Pais).Name, rnsPais);
            rns.Add(typeof(ClienteCartao).Name, rnsClienteCartao);
            rns.Add(typeof(CartaoCredito).Name, rnsCartao);
            rns.Add(typeof(Bandeira).Name, rnsBandeira);
            rns.Add(typeof(TipoTelefone).Name, rnsTipoTelefone);
            rns.Add(typeof(TipoResidencia).Name, rnsTipoResidencia);
            rns.Add(typeof(TipoLogradouro).Name, rnsTipoLogradouro);
            rns.Add(typeof(Cliente).Name, rnsClientePF);
            rns.Add(typeof(CategoriaMotivo).Name, rnsCategoriaMotivo);
            rns.Add(typeof(Categoria).Name, rnsCategoriaLivro);
            rns.Add(typeof(LivroCategoria).Name, rnsLivroCategoria);
            rns.Add(typeof(ImagemLivro).Name, rnsImagemLivro);
            rns.Add(typeof(Editora).Name, rnsEditora);
            rns.Add(typeof(Livro).Name, rnsLivro);
            rns.Add(typeof(Estoque).Name, rnsEstoque);
            rns.Add(typeof(Fornecedor).Name, rnsFornecedor);
            rns.Add(typeof(Cupom).Name, rnsCupom);
            rns.Add(typeof(TipoCupom).Name, rnsTipoCupom);
            rns.Add(typeof(PedidoCupom).Name, rnsClienteCupom);
            rns.Add(typeof(StatusPedido).Name, rnsStatusPedido);
            rns.Add(typeof(CartaoCreditoPedido).Name, rnsCCPedido);
            rns.Add(typeof(PedidoDetalhe).Name, rnsPedidoDetalhe);
            rns.Add(typeof(Pedido).Name, rnsPedido);
            rns.Add(typeof(Analise).Name, rnsAnalise);

        }
        // FIM do CONSTRUTOR da Fachada -------------------------

        public static Fachada UniqueInstance
        {
            get { return Instance; }
        }

        public Resultado salvar(EntidadeDominio entidade)
        {
            resultado = new Resultado();
            string nmClasse = entidade.GetType().Name;
            string msg = ExecutarRegras(entidade, "SALVAR");

            if (string.IsNullOrEmpty(msg))
            {
                IDAO dao = daos[nmClasse];
                dao.Salvar(entidade);
                List<EntidadeDominio> entidades = new List<EntidadeDominio>();
                entidades.Add(entidade);
                resultado.Entidades = entidades;
            }
            else
            {
                resultado.Msg = msg;
            }
            return resultado;
        }

        public Resultado alterar(EntidadeDominio entidade)
        {
            resultado = new Resultado();
            string nmClasse = entidade.GetType().Name;
            string msg = ExecutarRegras(entidade, "ALTERAR");

            if (string.IsNullOrEmpty(msg))
            {
                IDAO dao = daos[nmClasse];
                dao.Alterar(entidade);
                List<EntidadeDominio> entidades = new List<EntidadeDominio>();
                entidades.Add(entidade);
                resultado.Entidades = entidades;
            }
            else
            {
                resultado.Msg = msg;
            }

            return resultado;
        }

        public Resultado consultar(EntidadeDominio entidade)
        {
            resultado = new Resultado();
            string nmClasse = entidade.GetType().Name;
            string msg = ExecutarRegras(entidade, "CONSULTAR");

            if (string.IsNullOrEmpty(msg))
            {
                IDAO dao = daos[nmClasse];
                try
                {
                    resultado.Entidades = dao.Consultar(entidade);
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
            else
            {
                resultado.Msg = msg;
            }
            return resultado;
        }

        public Resultado excluir(EntidadeDominio entidade)
        {
            resultado = new Resultado();
            string nmClasse = entidade.GetType().Name;
            string msg = ExecutarRegras(entidade, "EXCLUIR");

            if (string.IsNullOrEmpty(msg))
            {
                IDAO dao = daos[nmClasse];
                dao.Excluir(entidade);
                List<EntidadeDominio> entidades = new List<EntidadeDominio>();
                entidades.Add(entidade);
                resultado.Entidades = entidades;
            }
            else
            {
                resultado.Msg = msg;
            }

            return resultado;
        }

        /*
         * Método que percorrerá a lista que contém as regra que devem ser 
         * executadas para validações e verificações "obrigatórias"
         */
        private string ExecutarRegras(EntidadeDominio entidade, string operacao)
        {
            // pega nome da classe
            string nmClasse = entidade.GetType().Name;
            StringBuilder msg = new StringBuilder();

            // pegando o mapa específico que contém todas as listas das regras indicando o indice (nmClasse)
            Dictionary<string, List<IStrategy>> regrasOperacao = rns[nmClasse];

            if (regrasOperacao != null)
            {
                // pegando a lista específica indicando a operação que será feita
                List<IStrategy> regras = regrasOperacao[operacao];

                if (regras != null)
                {
                    // percorre a lista para execução das Strategys
                    foreach (IStrategy s in regras)
                    {
                        // chama método que fará a validação/verificação
                        string m = s.processar(entidade);

                        if (!string.IsNullOrEmpty(m))
                        {
                            msg.Append(m);
                            msg.Append("\n");
                        }
                    }
                }
            }

            if (msg.Length > 0)
                return msg.ToString();
            else
                return null;
        }
    }
}
