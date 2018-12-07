using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MegaSena
{
    public class Program
    {
        static void Main(string[] args)
        {
            string nomeArquivo = string.Empty;
            if(args == null || args.Count() < 2)
            {
                nomeArquivo = "../../Sorteios.csv";
            }
            else
            {
                nomeArquivo = args[1];
            }

            string nomeNovoArquivo = string.Empty;
            if (args == null || args.Count() < 3)
            {
                nomeNovoArquivo = "../../saida.txt";
            }
            else
            {
                nomeNovoArquivo = args[2];
            }

            PreProcessamento preProcessamento = new PreProcessamento(nomeArquivo, nomeNovoArquivo);
            List<Sorteio> sorteios = preProcessamento.Processa();
            Agrupamento agrupamento = new Agrupamento(10, 100, sorteios);
            agrupamento.Processa();
            agrupamento.Imprime(nomeNovoArquivo);
        }
    }
}
