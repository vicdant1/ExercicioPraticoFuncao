﻿using FI.AtividadeEntrevista.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FI.AtividadeEntrevista.BLL
{
    public class BoCliente
    {
        /// <summary>
        /// Inclui um novo cliente
        /// </summary>
        /// <param name="cliente">Objeto de cliente</param>
        public long Incluir(DML.Cliente cliente)
        {
            DAL.DaoCliente cli = new DAL.DaoCliente();
            return cli.Incluir(cliente);
        }

        /// <summary>
        /// Altera um cliente
        /// </summary>
        /// <param name="cliente">Objeto de cliente</param>
        public void Alterar(DML.Cliente cliente)
        {
            DAL.DaoCliente cli = new DAL.DaoCliente();
            cli.Alterar(cliente);
        }

        /// <summary>
        /// Consulta o cliente pelo id
        /// </summary>
        /// <param name="id">id do cliente</param>
        /// <returns></returns>
        public DML.Cliente Consultar(long id)
        {
            DAL.DaoCliente cli = new DAL.DaoCliente();
            return cli.Consultar(id);
        }

        /// <summary>
        /// Excluir o cliente pelo id
        /// </summary>
        /// <param name="id">id do cliente</param>
        /// <returns></returns>
        public void Excluir(long id)
        {
            DAL.DaoCliente cli = new DAL.DaoCliente();
            cli.Excluir(id);
        }

        /// <summary>
        /// Lista os clientes
        /// </summary>
        public List<DML.Cliente> Listar()
        {
            DAL.DaoCliente cli = new DAL.DaoCliente();
            return cli.Listar();
        }

        /// <summary>
        /// Lista os clientes
        /// </summary>
        public List<DML.Cliente> Pesquisa(int iniciarEm, int quantidade, string campoOrdenacao, bool crescente, out int qtd)
        {
            DAL.DaoCliente cli = new DAL.DaoCliente();
            return cli.Pesquisa(iniciarEm,  quantidade, campoOrdenacao, crescente, out qtd);
        }

        /// <summary>
        /// VerificaExistencia
        /// </summary>
        /// <param name="CPF"></param>
        /// <returns></returns>
        public bool VerificarExistencia(string CPF)
        {
            DAL.DaoCliente cli = new DAL.DaoCliente();
            return cli.VerificarExistencia(CPF);
        }

        /// <summary>
        /// Verificar se o CPF é válido de acordo com o cálculo padrão
        /// </summary>
        /// <param name="CPF"></param>
        /// <returns></returns>

        public bool VerificarCPFValido(string CPF)
        {
            int[] multiplicadorInterno = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicadorExterno = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

            CPF = CPF.LimparCPF();
            if (CPF.Length != 11)
                return false;

            for (int j = 0; j < 10; j++)
            {
                if (j.ToString().PadLeft(11, char.Parse(j.ToString())) == CPF)
                    return false;
            }

            string subCpf = CPF.Substring(0, 9);
            int somaDigitos = 0;

            for (int i = 0; i < 9; i++)
                somaDigitos += int.Parse(subCpf[i].ToString()) * multiplicadorInterno[i];

            int resto = somaDigitos % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            string digitoValidador = resto.ToString();
            subCpf = subCpf + digitoValidador;
            somaDigitos = 0;

            for (int i = 0; i < 10; i++)
            {
                somaDigitos += int.Parse(subCpf[i].ToString()) * multiplicadorExterno[i];
            }

            resto = somaDigitos % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            digitoValidador = digitoValidador + resto.ToString();

            return CPF.EndsWith(digitoValidador);
        }
    }
}
