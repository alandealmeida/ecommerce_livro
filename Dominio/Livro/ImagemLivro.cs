using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio.Livro
{
    public class ImagemLivro : EntidadeDominio
    {
		private int _idLivro;

		public int IdLivro
		{
			get { return _idLivro; }
			set { _idLivro = value; }
		}

		private byte[] _imagem;

		public byte[] Imagem
		{
			get { return _imagem; }
			set { _imagem = value; }
		}

		public ImagemLivro()
		{
			_idLivro = 0;
			_imagem = null;
		}
	}
}
