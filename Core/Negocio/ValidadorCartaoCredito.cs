using Core.Core;
using Dominio;
using Dominio.Cliente;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace Core.Negocio
{
    public class ValidadorCartaoCredito : IStrategy
    {
        public string processar(EntidadeDominio entidade)
        {
            StringBuilder sb = new StringBuilder();
            if (entidade.GetType() == typeof(CartaoCredito))
            {
                CartaoCredito cc = (CartaoCredito)entidade;

                // verifica se nome está vazio ou nulo
                if (String.IsNullOrEmpty(cc.NomeImpresso))
                {
                    sb.Append("NOME IMPRESSO É UM CAMPO OBRIGATÓRIO! <br />");
                }

                // verifica se número está vazio ou nulo
                if (String.IsNullOrEmpty(cc.NumeroCC))
                {
                    sb.Append("NÚMERO DO CARTÃO É UM CAMPO OBRIGATÓRIO! <br />");
                }
                else
                {
                    // expressão regular para verificar se contém 16 dígitos numéricos
                    Regex Rgx = new Regex(@"^\d{16}$");

                    // Verifica se o numero do cartao tem 16 dígitos
                    if (!Rgx.IsMatch(cc.NumeroCC))
                    {
                        sb.Append("NÚMERO DO CARTÃO INCORRETO! OBRIGATÓRIO 16 DÍGITOS NUMÉRICOS! <br />");
                    }
                }

                // verifica se bandeira foi selecionado
                if (cc.Bandeira.ID == 0)
                {
                    sb.Append("BANDEIRA É UM CAMPO OBRIGATÓRIO! <br />");
                }

                // verifica se código está vazio ou nulo
                if (String.IsNullOrEmpty(cc.CodigoSeguranca))
                {
                    sb.Append("CÓDIGO DE SEGURANÇA É UM CAMPO OBRIGATÓRIO! <br />");
                }

                // verifica se data de vencimento está vazio ou nulo
                if (cc.DataVencimento == null)
                {
                    sb.Append("DATA DE VENCIMENTO É UM CAMPO OBRIGATÓRIO! <br />");
                }

            }
            else
            {
                sb.Append("CARTÃO DE CRÉDITO NÃO PODE SER VALIDADO, POIS ENTIDADE NÃO É CARTÃO DE CRÉDITO! <br />");
            }

            if (sb.Length != 0)
            {
                return sb.ToString();
            }

            return null;
        }
    }
}
