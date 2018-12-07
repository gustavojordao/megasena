using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MegaSena
{
    public class Sorteio
    {
        private int concurso;
        private DateTime data;
        private int[] dezenas;
        private int[] numeros;

        public int Concurso
        {
            get
            {
                return concurso;
            }
            set
            {
                concurso = value;
            }
        }

        public DateTime Data
        {
            get
            {
                return data;
            }
            set
            {
                data = value;
            }
        }

        public int[] Dezenas
        {
            get
            {
                return dezenas;
            }
            set
            {
                dezenas = value;
            }
        }

        public int[] Numeros
        {
            get
            {
                return numeros;
            }
            set
            {
                numeros = value;
            }
        }

    }
}
