import { Component } from '@angular/core';
import { Router } from '@angular/router';

import { LocalStorageUtils } from 'src/app/utils/localstorage';

declare const isEmpty: any;

@Component({ selector: 'app-menu', templateUrl: './menu.component.html', styleUrls: ['./menu.component.css'] })
export class MenuComponent
{
  public isCollapsed: boolean;
  public router: Router;

  usuario_logado: boolean;
  usuario_is_admin: boolean;
  localStorageUtils = new LocalStorageUtils();

  constructor(private routerService: Router)
  {
    this.isCollapsed = true;
    this.router = routerService;

    var usuario = this.localStorageUtils.obterUsuario();
    this.usuario_logado = !isEmpty(usuario);
    this.usuario_is_admin = this.usuario_logado && usuario.is_admin;
  }
}
