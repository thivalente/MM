import { Movimentacao } from './../../../_models/movimentacao';
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { SettingsService } from 'src/app/_services/settings.service';

import { Usuario } from 'src/app/_models/usuario';

declare const isEmpty: any;

@Injectable({ providedIn: 'root' })
export class AdminUsuarioService
{
    constructor(private http: HttpClient, private config: SettingsService) { }

    public listaUsuarios: Usuario[] = [];

    public obter(usuario_id) : Observable<Usuario>
    {
        return this.http.get<any>(`${this.config.getApiUrl()}admin/usuario/` + usuario_id);
    }

    public listar() : Observable<Usuario[]>
    {
        return this.http.get<any>(`${this.config.getApiUrl()}admin/usuario`);
    }

    public salvarUsuario(usuario: Usuario)
    {
        return this.http.post(this.config.getApiUrl() + 'admin/usuario/salvar', usuario);
    }

    public salvarMovimentacao(movimentacao: Movimentacao)
    {
        if (isEmpty(movimentacao.id))
            movimentacao.id = '00000000-0000-0000-0000-000000000000';

        return this.http.post(this.config.getApiUrl() + 'admin/usuario/salvarMovimentacao', movimentacao);
    }
}
