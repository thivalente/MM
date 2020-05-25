import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, Router, CanDeactivate, Route } from '@angular/router';

import { BaseGuard } from 'src/app/_services/base.guard';

@Injectable()
export class ContaGuard extends BaseGuard implements CanActivate
{
    constructor(protected router: Router) { super(router); }

    canActivate(routeAc: ActivatedRouteSnapshot)
    {
        var usuarioLogado = this.localStorageUtils.obterUsuario();

        if (!usuarioLogado)
            return true;

        var pagina_inicial = usuarioLogado.pagina_inicial;
        this.router.navigate([pagina_inicial]);
    }
}