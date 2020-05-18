import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { HttpClient, HttpHeaders } from '@angular/common/http';

import { Usuario } from '../_models/usuario';

@Injectable({ providedIn: 'root' })
export class ContaService
{
    private usuarioLogadoSubject: BehaviorSubject<Usuario>;

    constructor() { }

    public get usuarioLogado(): Usuario
    {
        this.usuarioLogadoSubject = new BehaviorSubject<Usuario>({ id: '13226661-4927-46F3-969A-2A3919183747', nome: 'Elói Gonçalves', primeiro_nome: 'Elói', aceitou_termos: false, data_aceitou_termos: null, taxa_acima_cdi: 2 });
        return this.usuarioLogadoSubject.value;
    }
}
