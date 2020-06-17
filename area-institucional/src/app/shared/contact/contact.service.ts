import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import { Mensagem } from 'src/app/_models/mensagem';

@Injectable({ providedIn: 'root' })
export class ContactService
{
    url: string;

    constructor(private http: HttpClient)
    {
        this.url = 'https://api.mminvestimentos.com.br/api/v1.0/';
        //this.url = 'https://localhost:44323/api/v1.0/';
    }

    public enviar(mensagem: Mensagem)
    {
        const body = { email: mensagem.email, nome: mensagem.nome, assunto: mensagem.assunto, mensagem: mensagem.mensagem };

        return this.http.post(this.url + 'conta/contato', body);
    }
}