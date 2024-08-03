using FI.AtividadeEntrevista.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FI.AtividadeEntrevista.Util
{
    public static class CPFHelper
    {
        /// <summary>
        /// Verificar se o CPF é válido de acordo com o cálculo padrão
        /// </summary>
        /// <param name="CPF"></param>
        /// <returns></returns>

        public static bool VerificarCPFValido(string CPF)
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
