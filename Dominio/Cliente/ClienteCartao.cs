using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Cliente
{
    public class ClienteCartao : EntidadeDominio
    {
        private CartaoCredito _cc;

        public CartaoCredito CC
        {
            get { return _cc; }
            set { _cc = value; }
        }

        public ClienteCartao()
        {
            _cc = new CartaoCredito();
        }

    }
}
