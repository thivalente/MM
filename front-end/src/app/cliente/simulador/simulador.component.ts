import { Component, OnInit } from '@angular/core';

@Component({ selector: 'app-simulador', templateUrl: './simulador.component.html', styleUrls: ['./simulador.component.css'] })

export class SimuladorComponent implements OnInit
{
  public cards: any;
  constructor() { }

  ngOnInit(): void
  {
    this.cards = { investido: 0, acumulado: 0, cdi: 0, poupanca: 0, visivel: false };
  }
}
