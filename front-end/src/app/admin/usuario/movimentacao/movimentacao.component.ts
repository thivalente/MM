import { Component, OnInit } from '@angular/core';

import { ActivatedRoute, Router } from '@angular/router';

import { Usuario } from 'src/app/_models/usuario';

@Component({ selector: 'app-movimentacao', templateUrl: './movimentacao.component.html', styleUrls: ['./movimentacao.component.css']})
export class AdminUsuarioMovimentacaoComponent implements OnInit
{
  paginaAtual: number = 1;
  usuario: Usuario;

  constructor(private route: ActivatedRoute, private router: Router)
  {
    this.usuario = this.route.snapshot.data['usuario'];
  }

  ngOnInit(): void
  {

  }

  goToCadastro()
  {
    this.router.navigate(['/admin/usuario/editar/' + this.usuario.id]);
  }
}
