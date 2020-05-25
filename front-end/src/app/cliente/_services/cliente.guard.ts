import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, Router, CanDeactivate, Route } from '@angular/router';

import { BaseGuard } from 'src/app/_services/base.guard';

@Injectable()
export class ClienteGuard extends BaseGuard implements CanActivate //, CanDeactivate<NovoComponent>
{
    constructor(protected router: Router) { super(router); }

    // canDeactivate(component: NovoComponent)
    // {
    //     if(component.mudancasNaoSalvas)
    //     {
    //         return window.confirm('Tem certeza que deseja abandonar o preenchimento do formulario?');
    //     }        
    //     return true
    // }

    canActivate(routeAc: ActivatedRouteSnapshot)
    {
        return super.validarClaims(routeAc);
    }

    canLoad(route: Route)
    {
        this.verificarLogado();

        return true;
    }

    private verificarLogado()
    {
        let user = this.localStorageUtils.obterUsuario();

        if (!user || user.is_admin)
            this.navegarAcessoNegado();
    }
}