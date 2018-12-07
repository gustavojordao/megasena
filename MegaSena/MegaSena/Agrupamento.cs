using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MegaSena
{
    public class Agrupamento
    {
        private int numClusters;
        private int numIteracoes;
        private List<Sorteio> sorteios;
        private List<List<Sorteio>> clusters;

        public Agrupamento(int numClusters, int numIteracoes, List<Sorteio> sorteios)
        {
            this.numClusters = numClusters;
            this.numIteracoes = numIteracoes;
            this.sorteios = sorteios;
            this.clusters = new List<List<Sorteio>>();
        }

        public void EscolheClusters(bool random)
        {
            List<int> numConcursos = new List<int>();
            numConcursos.AddRange(sorteios.Select(s => s.Concurso));

            if (random) {
                // Escolhe centros dos clusters randomicamente
                Random r = new Random();
                for (int i = 0; i < numClusters; i++)
                {
                    int sorteado = r.Next(numConcursos.Count());
                    clusters.Add(new List<Sorteio> { sorteios[numConcursos[sorteado]-1] });
                    numConcursos.Remove(sorteado);
                }
            }
            else
            {
                // Escolhe centros dos clusters de acordo com a menor distância total para cada cluster
                for (int i = 0; i < clusters.Count; i++)
                {
                    int clusterIndex = -1;
                    double distancia = 0;
                    for (int j = 0; j < clusters[i].Count; j++)
                    {
                        List<Sorteio> clusterTemp = new List<Sorteio>();
                        clusterTemp.AddRange(clusters[i]);
                        clusterTemp.RemoveAt(j);
                        clusterTemp.Insert(0, clusters[i][j]);

                        double distanciaTemp = CalculaDistanciaTotalCluster(clusterTemp);
                        if (clusterIndex == -1 || distanciaTemp < distancia)
                        {
                            clusterIndex = j;
                            distancia = distanciaTemp;
                        }
                    }
                    Sorteio tempCluster = clusters[i][clusterIndex];
                    clusters[i].RemoveAt(clusterIndex);
                    clusters[i].Insert(0, tempCluster);
                }
            }
        }

        public void Classifica()
        {
            List<Sorteio> tempSorteios = this.sorteios;

            // Limpa clusters
            for(int j=0; j<clusters.Count; j++)
            {
                clusters[j] = new List<Sorteio> { clusters[j][0] };
                tempSorteios.RemoveAll(c => c.Concurso == clusters[j][0].Concurso);
            }

            // Preenche clusters novamente
            for (int i = 0; i < tempSorteios.Count; i++)
            {
                int clusterIndex = -1;
                double distancia = 0;
                for (int j = 0; j < clusters.Count; j++)
                {
                    double distanciaTemp = CalculaDistancia(sorteios[i], clusters[j][0]);
                    if (clusterIndex == -1 || distanciaTemp < distancia)
                    {
                        clusterIndex = j;
                        distancia = distanciaTemp;
                    }
                }

                clusters[clusterIndex].Add(sorteios[i]);
            }
        }

        public double CalculaDistancia(Sorteio sorteio1, Sorteio sorteio2)
        {
            double soma = 0.0;
            for(int i=0; i<60; i++)
            {
                soma += 100.0/Math.Pow(2, sorteio1.Numeros[i] * sorteio2.Numeros[i]);
            }

            return soma;
        }

        public double CalculaDistanciaTotalCluster(List<Sorteio> cluster)
        {
            double soma = 0.0;
            for (int i = 1; i < cluster.Count; i++)
            {
                soma += CalculaDistancia(cluster[0], cluster[i]);
            }

            return soma;
        }

        public void Processa()
        {
            EscolheClusters(true);
            Classifica();

            for (int i=0; i<this.numIteracoes; i++)
            {
                EscolheClusters(false);
                Classifica();
            }

            OrdenaClusters();
        }

        public void OrdenaClusters()
        {
            for (int i = 0; i < clusters.Count; i++)
            {
                clusters[i].Sort(
                    delegate(Sorteio s1, Sorteio s2) {
                        double distC1 = CalculaDistancia(clusters[i][0], s1);
                        double distC2 = CalculaDistancia(clusters[i][0], s2);

                        return distC1 < distC2 ? -1 : distC1 > distC2 ? 1 : 0;
                    }
                );
            }

            // Ordena por ordem reversa
            clusters.Sort(
                delegate(List<Sorteio> c1, List<Sorteio> c2)
                {
                    return c1.Count < c2.Count ? 1 : c1.Count > c2.Count ? -1 : 0;
                }
            );
        }

        public void Imprime(string nomeArquivo)
        {
            string str = string.Empty;

            for (int i=0; i < clusters.Count; i++)
            {
                str += $"Cluster {i + 1} - {clusters[i].Count} sorteios{Environment.NewLine}";
                for (int j = 0; j < clusters[i].Count; j++)
                {
                    str += $"Concurso {clusters[i][j].Concurso} - Dezenas";
                    for (int k = 0; k < 6; k++)
                    {
                        str += $" {clusters[i][j].Dezenas[k]}";
                    }
                    str += $"{Environment.NewLine}";
                }
                str += $"{Environment.NewLine}";
            }

            File.WriteAllText(nomeArquivo, str);
        }
    }
}
