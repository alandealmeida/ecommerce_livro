using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Core.Aplicacao;
using Dominio;

namespace elivro.Command
{
    public class ExcluirCommand : AbstractCommand
    {
        public override Resultado execute(EntidadeDominio entidade)
        {
            return fachada.excluir(entidade);
        }
    }
}