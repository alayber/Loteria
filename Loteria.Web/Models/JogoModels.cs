using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Loteria.Web.Models
{
    public class JogoModels
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public int QtdeNumeroSorteados { get; set; }
        public int QtdeMinNumeroMarcadoVolante { get; set; }
        public int QtdeMaxNumeroMarcadoVolante { get; set; }
        public int NumeroInicial { get; set; }
        public int QuantidadeNumeros { get; set; }

        public int QtdeLinhas
        {
            get {
                if (QuantidadeNumeros > 25)
                    return (int)(QuantidadeNumeros / 10);
                else
                    return (int)(QuantidadeNumeros / 5);
            }
        }

        public int QtdeColunas
        {
            get
            {
                if (QuantidadeNumeros > 25)
                    return 10;
                else
                    return  5;
            }
        }
        public List<ApostaModels> ApostasFeitas { get; set; }
        public ResultadoModels Resultado { get; set; }

    }
}