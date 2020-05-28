import { Component, OnInit } from '@angular/core';
import { ContaService } from 'src/app/conta/_services/conta.service';

import { LocalStorageUtils } from 'src/app/utils/localstorage';

declare const isEmpty: any;

@Component({ selector: 'app-simulador', templateUrl: './simulador.component.html', styleUrls: ['./simulador.component.css'] })
export class SimuladorComponent implements OnInit
{
  public taxa_cdi: number;
  public taxa_poupanca: number;
  public dados: any;

  private localStorage: LocalStorageUtils = new LocalStorageUtils();

  constructor(private contaService: ContaService) { }

  ngOnInit(): void
  {
    this.dados =
    {
      inicial: 0,
      mensal: 0,
      prazo: 0,
      aplicado_total: 0,
      acumulado: { total: 0, cdi: 0, poupanca: 0 },
      porcentagem_cdi: this.contaService.usuarioLogado.taxa_acima_cdi * 100,
      rendimento: { total: 0, cdi: 0, poupanca: 0 }
    };

    var taxas = this.localStorage.obterTaxasAtualizadas();

    this.taxa_cdi = !isEmpty(taxas) ? taxas.taxa_mensal_di / 100 : 0;
    this.taxa_poupanca = !isEmpty(taxas) ? taxas.taxa_mensal_poupanca / 100 : 0;
  }

  atualizarCalculos(): void
  {
    // F = P.(1+i)n + M.[(1+i)n - 1]/i
    let periodo = this.dados.prazo;
    let aporte_mensal = this.dados.mensal;
    let taxa_cdi = this.taxa_cdi;
    let taxa_cliente = this.contaService.usuarioLogado.taxa_acima_cdi * taxa_cdi;
    let taxa_poupanca = this.taxa_poupanca;
    let valor_futuro = this.calcular_valor_futuro(this.dados.inicial, aporte_mensal, taxa_cliente, periodo);
    let cdi_futuro = this.calcular_valor_futuro(this.dados.inicial, aporte_mensal, taxa_cdi, periodo);
    let poupanca_futuro = this.calcular_valor_futuro(this.dados.inicial, aporte_mensal, taxa_poupanca, periodo);

    this.dados.aplicado_total = this.dados.inicial + (aporte_mensal * periodo);
    this.dados.acumulado.total = valor_futuro;
    this.dados.acumulado.cdi = cdi_futuro;
    this.dados.acumulado.poupanca = poupanca_futuro;
    this.dados.rendimento.total = this.dados.acumulado.total - this.dados.aplicado_total;
    this.dados.rendimento.cdi = cdi_futuro - this.dados.aplicado_total;
    this.dados.rendimento.poupanca = poupanca_futuro - this.dados.aplicado_total;
  }

  calcular_valor_futuro(valor_presente, aporte_mensal, taxa, periodo) : number
  {
    return (valor_presente * Math.pow((1 + taxa), periodo)) + (aporte_mensal * (Math.pow((1 + taxa), periodo) - 1)) / taxa;
  }
}
