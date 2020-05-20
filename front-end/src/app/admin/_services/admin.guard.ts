import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, Router, CanDeactivate } from '@angular/router';

import { BaseGuard } from 'src/app/_services/base.guard';

@Injectable()
export class AdminGuard extends BaseGuard implements CanActivate //, CanDeactivate<NovoComponent>
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

    canLoad(routeAc: ActivatedRouteSnapshot)
    {
        return false;
    }  

    canActivate(routeAc: ActivatedRouteSnapshot)
    {
        return super.validarClaims(routeAc);
    }  
}