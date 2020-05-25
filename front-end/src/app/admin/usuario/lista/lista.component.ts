import { Component, OnInit } from '@angular/core';
import { NgxUiLoaderService } from 'ngx-ui-loader';
import { Router } from '@angular/router';

import { AdminUsuarioService } from '../usuario.service';

import { Usuario } from './../../../_models/usuario';

@Component({ selector: 'admin-usuario-lista', templateUrl: './lista.component.html', styleUrls: ['./lista.component.css']})
export class AdminUsuarioListaComponent implements OnInit
{
  paginaAtual: number = 1;
  usuarios: Usuario[];

  constructor(private usuarioService: AdminUsuarioService, private router: Router, private ngxLoader: NgxUiLoaderService) { }

  ngOnInit(): void
  {
    this.usuarios = [];
    this.carregarTela(0);
  }

  carregarTela(nAttempts: number)
  {
    this.ngxLoader.startLoader('loader-principal');

    this.usuarioService.listar().subscribe(response =>
      {
          if (response != null)
          {
            this.usuarios = response;
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

  goToEditar(id: string)
  {
    this.router.navigate(['/admin/usuario/editar/' + id]);
  }
}
