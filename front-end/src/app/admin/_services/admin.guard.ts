import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, Router, CanDeactivate, Route } from '@angular/router';

import { BaseGuard } from 'src/app/_services/base.guard';

import { AdminUsuarioCadastroComponent } from './../usuario/cadastro/cadastro.component';

@Injectable()
export class AdminGuard extends BaseGuard implements CanActivate, CanDeactivate<AdminUsuarioCadastroComponent>
{
    constructor(protected router: Router) { super(router); }

    canActivate(routeAc: ActivatedRouteSnapshot)
    {
        return super.validarClaims(routeAc);
    }

    canDeactivate(component: AdminUsuarioCadastroComponent)
    {
        if(component.mudancasNaoSalvas)
            return window.confirm('Tem certeza que deseja abandonar o preenchimento do formulario?');

        return true
    }

    canLoad(route: Route)
    {
        this.verificarAdmin(route);

        return true;
    }

    private verificarAdmin(route)
    {
        let user = this.localStorageUtils.obterUsuario();
        var isPathAdmin = route.path === 'admin';

        if (!user || (!user.is_admin && isPathAdmin))
            this.navegarAcessoNegado();
    }
}