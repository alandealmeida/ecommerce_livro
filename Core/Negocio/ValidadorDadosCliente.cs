using Core.Core;
using Dominio;
using Dominio.Cliente;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Negocio
{
    public class ValidadorDadosCliente : IStrategy
    {
        public string processar(EntidadeDominio entidade)
        {
            StringBuilder sb = new StringBuilder();
            if (entidade.GetType() == typeof(Cliente))
            {
                Cliente pessoa = (Cliente)entidade;

                // verifica se nome está vazio ou nulo
                if (String.IsNullOrEmpty(pessoa.Nome))
                {
                    sb.Append("NOME É UM CAMPO OBRIGATÓRIO! <br />");
                }
                else
                {
                    // verifica se nome é maior que 100
                    if (pessoa.Nome.Length > 100)
                    {
                        sb.Append("NOME TEM QUE TER TAMANHO DE NO MÁXIMO 100 LETRAS! <br />");
                    }
                }

                // vefica se telefone está vazio ou nulo
                if (pessoa.Telefone == null)
                {
                    sb.Append("TELEFONE É UM CAMPO OBRIGATÓRIO! <br />");
                }
                else
                {
                    ValidadorTelefone valTelefone = new ValidadorTelefone();
                    String msg = valTelefone.processar(pessoa.Telefone);
                    if (msg != null)
                    {
                        sb.Append(msg);
                    }
                }

                // verifica se email está vazio ou nulo
                if (String.IsNullOrEmpty(pessoa.Email))
                {
                    sb.Append("E-MAIL É UM CAMPO OBRIGATÓRIO! <br />");
                }
                else
                {
                    ValidadorEmail valEmail = new ValidadorEmail();
                    String msg = valEmail.processar(pessoa);
                    if (msg != null)
                    {
                        sb.Append(msg);
                    }
                }

                // verifica se endereço está vazio ou nulo
                if (pessoa.Enderecos == null)
                {
                    sb.Append("ENDEREÇO É UM CAMPO OBRIGATÓRIO! <br />");
                }
                else
                {
                    ValidadorEndereco valEndereco = new ValidadorEndereco();
                    foreach (var endereco in pessoa.Enderecos)
                    {
                        String msg = valEndereco.processar(endereco);
                        if (msg != null)
                        {
                            sb.Append(msg);
                        }
                    }
                }


                // verifica se CPF está vazio ou nulo
                if (String.IsNullOrEmpty(pessoa.CPF))
                {
                    sb.Append("CPF É UM CAMPO OBRIGATÓRIO! <br />");
                }
                else
                {
                    ValidadorCPF valCPF = new ValidadorCPF();
                    String msg = valCPF.processar(pessoa);
                    if (msg != null)
                    {
                        sb.Append(msg);
                    }
                }

                // vefica se gênero está vazio ou nulo ou se não foi selecionado
                if (pessoa.Genero == '\0' || pessoa.Genero == '0')
                {
                    sb.Append("GÊNERO É UM CAMPO OBRIGATÓRIO! <br />");
                }

                // verifica se data de nascimento está vazio ou nulo
                if (pessoa.DataNascimento == null)
                {
                    sb.Append("DATA DE NASCIMENTO É UM CAMPO OBRIGATÓRIO! <br />");
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
