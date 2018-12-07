using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MegaSena
{
    public class PreProcessamento
    {
        private string nomeArquivo;
        private string nomeNovoArquivo;

        public PreProcessamento(string nomeArquivo, string nomeNovoArquivo)
        {
            this.nomeArquivo = nomeArquivo;
            this.nomeNovoArquivo = nomeNovoArquivo;
        }

        public string[] LeArquivo()
        {
            return File.ReadAllLines(nomeArquivo);
        }

        public List<Sorteio> Processa()
        {
            string[] linhas = LeArquivo();

            List<Sorteio> sorteios = new List<Sorteio>();

            //Linha 0 correponde ao cabeçalho
            for (int i = 1; i < linhas.Count(); i++)
            {
                string[] colunas = linhas[i].Split(';');

                Sorteio sorteio = new Sorteio
                {
                    Concurso = int.Parse(colunas[0]),
                    Data = DateTime.Parse(colunas[1]),
                    Dezenas = new int[]{
                        int.Parse(colunas[2]),
                        int.Parse(colunas[3]),
                        int.Parse(colunas[4]),
                        int.Parse(colunas[5]),
                        int.Parse(colunas[6]),
                        int.Parse(colunas[7])
                    },
                    Numeros = new int[60]
                };

                for (int n = 1; n <= 60; n++)
                {
                    sorteio.Numeros[n - 1] = 0;
                    for (int j = 0; j < 6; j++)
                    {
                        if (sorteio.Dezenas[j] == n)
                        {
                            sorteio.Numeros[n - 1] = 1;
                            break;
                        }
                    }
                }

                sorteios.Add(sorteio);
            }

            return sorteios;
        }
    }
}
