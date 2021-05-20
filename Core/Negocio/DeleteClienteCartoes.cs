using Core.Core;
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
    public class DeleteClienteCartoes : IStrategy
    {
        public string processar(EntidadeDominio entidade)
        {
            StringBuilder sb = new StringBuilder();
            if (entidade.GetType() == typeof(Cliente))
            {
                Cliente pessoa = (Cliente)entidade;

                if(pessoa.ID == 0)
                {
                    return "PARÂMETRO PARA EXCLUSÃO DA ENTIDADE INCORRETO(ID)! <br />";
                } else
                {
                    ClienteCartaoDAO clienteXCartaoDAO = new ClienteCartaoDAO();
                    clienteXCartaoDAO.Excluir(pessoa);
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
