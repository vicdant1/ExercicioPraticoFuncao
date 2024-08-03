using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FI.AtividadeEntrevista.Extension
{
    public static class StringExtensions
    {
        public static string LimparCPF(this string cpf)
        {
            return cpf.Replace("/", "").Replace("-", "").Replace(".", "").Trim();
        }
    }
}
