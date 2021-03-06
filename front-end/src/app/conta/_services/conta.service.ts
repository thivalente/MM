import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { HttpClient } from '@angular/common/http';

import { LocalStorageUtils } from 'src/app/utils/localstorage';
import { Usuario } from '../../_models/usuario';

import { BaseService } from 'src/app/_services/base.service';
import { SettingsService } from '../../_services/settings.service';

declare const isEmpty: any;

@Injectable({ providedIn: 'root' })
export class ContaService extends BaseService
{
    public LocalStorage: LocalStorageUtils;
    private usuarioLogadoSubject: BehaviorSubject<Usuario>;

    constructor(private config: SettingsService, private http: HttpClient)
    {
        super();
        this.LocalStorage = new LocalStorageUtils();
    }

    public get usuarioLogado(): Usuario
    {
        let usuarioLogado = this.LocalStorage.obterUsuario();

        if (!usuarioLogado)
            return null;

        this.usuarioLogadoSubject = new BehaviorSubject<Usuario>(usuarioLogado);
        return this.usuarioLogadoSubject.value;
    }

    public atualizarDadosUsuarioLogado(usuario: any)
    {
        this.LocalStorage.salvarUsuario(usuario);
    }

    public efetuarLogin(email: string, senha: string)
    {
        const body = { email, senha };
        return this.http.post(this.config.getApiUrl() + 'conta/login', body);
    }

    public obterPaginaInicial(usuario: Usuario = null) : string
    {
        if (!usuario)
            usuario = this.usuarioLogado;

        if (!usuario)
            return '/login';

        if (!isEmpty(usuario.pagina_inicial))
            return usuario.pagina_inicial;

        return (usuario.is_admin) ? '/admin/usuario' : '/cliente/dashboard';
    }

    public recuperarSenha(email: string)
    {
        const body = { email, senha: 'senha' };
        return this.http.post(this.config.getApiUrl() + 'conta/recuperarsenha', body);
    }

    public trocarSenha(senhaAtual: string, novaSenha: string)
    {
        let usuarioLogado = this.LocalStorage.obterUsuario();

        const body = { email: usuarioLogado.email, senhaAtual, novaSenha };
        return this.http.post(this.config.getApiUrl() + 'conta/trocarsenha', body, super.ObterAuthHeaderJson());
    }
}
