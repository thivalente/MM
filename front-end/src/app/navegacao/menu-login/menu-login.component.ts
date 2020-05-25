import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { LocalStorageUtils } from 'src/app/utils/localstorage';

@Component({ selector: 'app-menu-login', templateUrl: './menu-login.component.html', styleUrls:['./menu-login.component.css'] })
export class MenuLoginComponent
{
  token: string = "";
  user: any;
  primeiro_nome: string = "";
  localStorageUtils = new LocalStorageUtils();

  constructor(private router: Router) {  }

  usuarioLogado(): boolean
  {
    this.token = this.localStorageUtils.obterTokenUsuario();
    this.user = this.localStorageUtils.obterUsuario();

    if (this.user)
      this.primeiro_nome = this.user.primeiro_nome;

    return this.token !== null;
  }

  logout()
  {
    this.localStorageUtils.limparDadosLocaisUsuario();
    this.router.navigate(['/conta/login']);
  }
}
