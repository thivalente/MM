import { Movimentacao } from './../../../_models/movimentacao';
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { BaseService } from 'src/app/_services/base.service';
import { SettingsService } from 'src/app/_services/settings.service';

import { Usuario } from 'src/app/_models/usuario';

declare const isEmpty: any;

@Injectable({ providedIn: 'root' })
export class AdminUsuarioService extends BaseService
{
    constructor(private http: HttpClient, private config: SettingsService) { super(); }

    public listaUsuarios: Usuario[] = [];

    public excluirMovimentacao(movimentacao_id)
    {
        return this.http.delete(this.config.getApiUrl() + 'admin/usuario/excluirMovimentacao/' + movimentacao_id, super.ObterAuthHeaderJson());
    }

    public obter(usuario_id) : Observable<Usuario>
    {
        return this.http.get<any>(`${this.config.getApiUrl()}admin/usuario/` + usuario_id, super.ObterAuthHeaderJson());
    }

    public listar() : Observable<Usuario[]>
    {
        return this.http.get<any>(`${this.config.getApiUrl()}admin/usuario`, super.ObterAuthHeaderJson());
    }

    public salvarUsuario(usuario: Usuario)
    {
        return this.http.post(this.config.getApiUrl() + 'admin/usuario/salvar', usuario, super.ObterAuthHeaderJson());
    }

    public salvarMovimentacao(movimentacao: Movimentacao) : Observable<any>
    {
        if (isEmpty(movimentacao.id))
            movimentacao.id = '00000000-0000-0000-0000-000000000000';

        return this.http.post(this.config.getApiUrl() + 'admin/usuario/salvarMovimentacao', movimentacao, super.ObterAuthHeaderJson());
    }
}
