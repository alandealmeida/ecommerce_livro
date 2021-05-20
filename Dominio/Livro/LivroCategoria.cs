using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Livro
{
    public class LivroCategoria : EntidadeDominio
    {
		private Categoria _categoria;

		public Categoria Categoria
		{
			get { return _categoria; }
			set { _categoria = value; }
		}

		public LivroCategoria()
		{
			_categoria = new Categoria();
		}
	}
}
