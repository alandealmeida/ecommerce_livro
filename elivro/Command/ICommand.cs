﻿using Core.Aplicacao;
using Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace elivro.Command
{
    public interface ICommand
    {
        Resultado execute(EntidadeDominio entidade);
    }
}
