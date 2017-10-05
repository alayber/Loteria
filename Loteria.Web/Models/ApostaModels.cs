using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Loteria.Web.Models
{
    public class ApostaModels
    {
        public int Id { get; set; }        
        public List<int> NumerosSelecionados { get; set; }
        public DateTime DataCadastro { get; set; }
        public int QuantidadeAcertos(ResultadoModels pResultado)
        {
            int qtdades = 0;
            foreach (var numero in pResultado.NumerosSorteados)
            {
                if (NumerosSelecionados.Where(p => p == numero).Count() > 0)
                    qtdades++;
            }
            return qtdades;
        }
    }
}