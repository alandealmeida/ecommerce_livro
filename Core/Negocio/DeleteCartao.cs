﻿using Core.Core;
using Core.DAO;
using Dominio;
using Dominio.Cliente;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Negocio
{
    public class DeleteCartao : IStrategy
    {
        public string processar(EntidadeDominio entidade)
        {
            StringBuilder sb = new StringBuilder();
            if (entidade.GetType() == typeof(CartaoCredito))
            {
                CartaoCredito cartao = (CartaoCredito)entidade;

                if(cartao.ID == 0)
                {
                    return "PARÂMETRO PARA EXCLUSÃO DA ENTIDADE INCORRETO(ID)! <br />";
                } else
                {
                    // necessário para excluir na tabela n-n antes de fazer a exclusão na tabela dos cartões
                    ClienteCartaoDAO clienteXCartaoDAO = new ClienteCartaoDAO("id_cartao");
                    clienteXCartaoDAO.Excluir(cartao);
                }

            }
            else
            {
                sb.Append("CLIENTE PESSOA FÍSICA NÃO PODE SER VALIDADA, POIS ENTIDADE NÃO É CLIENTE PESSOA FÍSICA! <br />");
            }

            if (sb.Length != 0)
            {
                return sb.ToString();
            }

            return null;
        }
    }
}
