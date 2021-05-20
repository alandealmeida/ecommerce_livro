using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Cliente
{
    public class PedidoCupom : EntidadeDominio
    {
        private Cupom _cupom;

        public Cupom Cupom
        {
            get { return _cupom; }
            set { _cupom = value; }
        }

        public PedidoCupom()
        {
            _cupom = new Cupom();
        }

    }
}
