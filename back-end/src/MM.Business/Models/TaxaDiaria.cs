using System;

namespace MM.Business.Models
{
    public class TaxaDiaria
    {
        public TaxaDiaria() { }

        public TaxaDiaria(DateTime data_criacao, decimal taxa_di, decimal taxa_poupanca)
        {
            this.data_criacao = data_criacao;
            this.taxa_di = taxa_di;
            this.taxa_poupanca = taxa_poupanca;
        }

        public DateTime data_criacao    { get; private set; }
        public decimal taxa_di          { get; private set; }
        public decimal taxa_poupanca    { get; private set; }
        public bool ativo               { get; private set; }
    }
}
