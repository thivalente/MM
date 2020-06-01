import { Injectable } from '@angular/core';
import { BaseGuard } from 'src/app/_services/base.guard';
import { CanActivate, Router, ActivatedRouteSnapshot } from '@angular/router';

declare const isEmpty: any;

@Injectable()
export class ContaTrocaSenhaGuard extends BaseGuard implements CanActivate
{
    constructor(protected router: Router) { super(router); }

    canActivate(routeAc: ActivatedRouteSnapshot)
    {
        var usuarioLogado = this.localStorageUtils.obterUsuario();
        return !isEmpty(usuarioLogado);
    }
}