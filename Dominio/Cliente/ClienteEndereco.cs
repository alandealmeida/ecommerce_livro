using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Cliente
{
    public class ClienteEndereco : EntidadeDominio
    {
        private Endereco _endereco;

        public Endereco Endereco
        {
            get { return _endereco; }
            set { _endereco = value; }
        }

        public ClienteEndereco()
        {
            _endereco = new Endereco();
        }

    }
}
