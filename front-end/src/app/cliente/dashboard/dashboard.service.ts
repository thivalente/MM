import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, } from 'rxjs';
import { catchError, map } from "rxjs/operators";

import { ContaService } from 'src/app/conta/_services/conta.service';
import { SettingsService } from 'src/app/_services/settings.service';

import { Dashboard } from 'src/app/_models/dashboard';
import { BaseService } from 'src/app/_services/base.service';

@Injectable({ providedIn: 'root' })
export class DashboardService extends BaseService
{
    constructor(private contaService: ContaService, private http: HttpClient, private config: SettingsService) { super(); }

    public aceitarTermos()
    {
        var usuario_id = this.contaService.usuarioLogado.id;
        const body = { usuario_id };

        return this.http.put<any>(`${this.config.getApiUrl()}aceitarTermos`, body, super.ObterAuthHeaderJson());
    }

    public obter() : Observable<Dashboard[]>
    {
        var usuario_id = this.contaService.usuarioLogado.id;
        
        return this.http.get<any>(`${this.config.getApiUrl()}dashboard/` + usuario_id, super.ObterAuthHeaderJson())
        .pipe(
            map(super.extractData),
            catchError(super.serviceError));
    }
}
