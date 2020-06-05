import { Component, OnInit, TemplateRef, ViewChild, AfterViewInit } from '@angular/core';

import { ChartDataSets } from 'chart.js';
import { Label } from 'ng2-charts';
import { BsModalService, BsModalRef, ModalOptions } from 'ngx-bootstrap/modal'; 
import { NgxUiLoaderService } from 'ngx-ui-loader';

import { ClienteService } from '../_services/cliente.service';
import { ContaService } from 'src/app/conta/_services/conta.service';
import { DashboardService } from './dashboard.service';
import { LocalStorageUtils } from 'src/app/utils/localstorage';

declare const isEmpty: any;
declare const obterNomeMesAno: any;
declare const obterNomeMesAnoReduzido: any;
declare const sortByKey: any;
declare const sortByKey_Date: any;

@Component({ selector: 'app-dashboard', templateUrl: './dashboard.component.html', styleUrls: ['./dashboard.component.css'] })

export class DashboardComponent implements OnInit, AfterViewInit
{
  private localStorage: LocalStorageUtils = new LocalStorageUtils();
  private cdiChartLine: ChartDataSets;
  private poupancaChartLine: ChartDataSets;
  private userChartLine: ChartDataSets;

  public lineChartData: ChartDataSets[];
  public lineChartLabels: Label[];
  public lineChartType = 'line';
  public lineChartOptions;

  public cards: any;
  public extratos = [];
  public listaPeriodos = [];
  public periodoSelecionado: string;
  public saldo: number;
  public taxa_di_mensal: number;

  public aceitou: boolean;

  modalRef: BsModalRef;

  @ViewChild('templateDisclaimer') templateDisclaimer : TemplateRef<any>;

  constructor(private contaService: ContaService, private clienteService: ClienteService, 
    private dashboardService: DashboardService, private modalService: BsModalService, 
    private ngxLoader: NgxUiLoaderService) { }

  ngOnInit(): void 
  {
    // Inicializa o gráfico
    this.lineChartOptions = { responsive: true, 
      elements: { line: { fill: false } },
      scales:
      {
        yAxes:
        [
          { ticks: { callback: function(value, index, values) { return 'R$ ' + Intl.NumberFormat().format((value)); } } }
        ]
      },
      tooltips:
      {
        callbacks: {
          label: function(tooltipItem, chart)
          {
              var datasetLabel = chart.datasets[tooltipItem.datasetIndex].label || '';
              return datasetLabel + ': R$ ' + Intl.NumberFormat().format((tooltipItem.yLabel));
          }
        }
      }
    };

    this.lineChartLabels = [];
    this.userChartLine = { data: [], label: this.contaService.usuarioLogado.primeiro_nome, borderColor: '#45235F', pointBackgroundColor: '#742688' };
    this.cdiChartLine = { data: [], label: 'CDI', borderColor: '#808080', pointBackgroundColor: '#808080' };
    this.poupancaChartLine = { data: [], label: 'Poupança', borderColor: '#CCC', pointBackgroundColor: '#CCC' };
    this.lineChartData = [ this.userChartLine, this.cdiChartLine, this.poupancaChartLine ];

    this.cards = { hoje: 0, primeiro: 0, segundo: 0 };
    this.taxa_di_mensal = 0;
    this.carregarTela(0);
  }

  ngAfterViewInit(): void
  {
    if (!this.contaService.usuarioLogado.aceitou_termos)
      this.disclaimerAbrirModal();
  }

  disclaimerAbrirModal()
  {
    let options: ModalOptions = new ModalOptions();
    options.class = 'termos-de-uso modal-lg';
    options.backdrop = 'static';
    options.keyboard = false;

    this.modalRef = this.modalService.show(this.templateDisclaimer, options);
  }

  disclaimerAceitarTermos(nAttempts)
  {
    this.dashboardService.aceitarTermos().subscribe(response =>
      {
          if (response != null)
          {
            let usuarioLogado = this.contaService.usuarioLogado;
            usuarioLogado.aceitou_termos = true;
            usuarioLogado.data_aceitou_termos = new Date();

            this.contaService.atualizarDadosUsuarioLogado(usuarioLogado);
            this.modalRef.hide();
          }
      },
      error =>
      {
        nAttempts = nAttempts || 1;
        console.log(error, nAttempts);

        if (nAttempts >= 5)
        {
            return;
        }

        this.disclaimerAceitarTermos(++nAttempts);
      });
  }

  carregarTela(nAttempts: number): void
  {
      if (this.contaService.usuarioLogado.aceitou_termos)
        this.ngxLoader.startLoader('loader-principal');

      var taxas = this.localStorage.obterTaxasAtualizadas();
      this.taxa_di_mensal = !isEmpty(taxas) ? taxas.taxa_mensal_di / 100 : 0;

      this.dashboardService.obter().subscribe(response =>
        {
            if (response != null)
            {
              var listaMovimentacoes = response;

              this.extratos = sortByKey_Date(listaMovimentacoes, 'data', false);
              console.log(this.extratos);
              this.saldo = this.extratos.map(e => e.valor).reduce((total, item) => total + item);
          
              this.montarCards();
          
              this.periodoSelecionado = Math.max.apply(Math, this.extratos.map(function(e) { return e.periodo; })).toString();
              this.listaPeriodos = this.montarPeriodos(this.extratos.map(e => e.periodo).filter((value, index, self) => self.indexOf(value) === index));
          
              this.montarSeries(listaMovimentacoes);
            }

            this.ngxLoader.stopLoader('loader-principal');
        },
        error =>
        {
          nAttempts = nAttempts || 1;
          console.log(error, nAttempts);

          if (nAttempts >= 5)
          {
              this.ngxLoader.stopLoader('loader-principal');
              return;
          }

          this.carregarTela(++nAttempts);
        });
  }

  montarCards(): void
  {
    var taxa_cliente = this.taxa_di_mensal * 2;
    this.cards.hoje = this.saldo;
    this.cards.primeiro = this.cards.hoje * Math.pow((1 + taxa_cliente), 3); // 90 dias
    this.cards.segundo = this.cards.hoje * Math.pow((1 + taxa_cliente), 6); // 180 dias
  }

  montarPeriodos(periodos): Array<any>
  {
    var lista = [];

    periodos.forEach(p =>
      {
        var ano = p.toString().substring(0, 4);
        var mes = { ano_mes: p, nome: obterNomeMesAno(p) };

        var itemLista = lista.filter(l => l.ano.toString() === ano);

        // Se não existir este ano na lista, adiciona o ano. Se não, apenas atualiza o mês
        if (itemLista.length === 0)
          lista.push({ ano: ano, meses: [mes] });
        else // Se existir, adiciona o mês
          itemLista.map(l => l.meses.push(mes));
      });

    return lista;
  }

  montarSeries(movimentacoes: Array<any>) : void
  {
    var result = sortByKey(this.montarMovimentacoesMensais(movimentacoes), 'periodo', true);

    this.userChartLine.data = result.map(r => r.serieCliente);
    this.cdiChartLine.data = result.map(r => r.serieDI);
    this.poupancaChartLine.data = result.map(r => r.seriePoupanca);
    this.lineChartLabels = result.map(r => r.label);
  }

  montarMovimentacoesMensais(movimentacoes)
  {
      var result = [];

      if (movimentacoes.length === 0)
        return result;

      let movimentacoesOrdenadas = sortByKey_Date(movimentacoes.map(m => Object.assign({}, m)), 'data', true);

      let pm = movimentacoesOrdenadas[0];
      let acumulado = 0;
      let acumulado_di = 0;
      let acumulado_poupanca = 0;
      result.push({ serieCliente: 0, serieDI: 0, seriePoupanca: 0, label: obterNomeMesAnoReduzido(pm.periodo), periodo: pm.periodo });

      movimentacoesOrdenadas.forEach(element =>
      {
          // Se não tiver o período, adiciona com o acumulado atual
          if (result.filter(r => r.periodo === element.periodo).length <= 0)
          {
            result.push({ serieCliente: acumulado, serieDI: acumulado_di, seriePoupanca: acumulado_poupanca, label: obterNomeMesAnoReduzido(element.periodo), periodo: element.periodo });
          }

          if (element.rendimento)
          {
            acumulado += element.valor;
            acumulado_di += element.valor_di;
            acumulado_poupanca += element.valor_poupanca;
          }
      });

      return result;
  }
}
