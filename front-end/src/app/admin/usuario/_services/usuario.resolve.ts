import { Injectable } from '@angular/core';
import { Resolve, ActivatedRouteSnapshot } from '@angular/router';

import { Usuario } from 'src/app/_models/usuario';
import { AdminUsuarioService } from './usuario.service';

declare const isEmpty: any;

@Injectable()
export class AdminUsuarioResolve implements Resolve<Usuario> {

    constructor(private usuarioService: AdminUsuarioService) { }

    resolve(route: ActivatedRouteSnapshot)
    {
        var listaAtualUsuarios = this.usuarioService.listaUsuarios;
        var usuariosSelecionados = isEmpty(listaAtualUsuarios) ? null : listaAtualUsuarios.filter(u => u.id == route.params['id']);

        return !isEmpty(usuariosSelecionados) && usuariosSelecionados.length > 0 ? usuariosSelecionados[0] : this.usuarioService.obter(route.params['id']);
    }
}