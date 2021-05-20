using Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Core
{
    public interface IStrategy
    {
        string processar(EntidadeDominio entidade);
    }
}
