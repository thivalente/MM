import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { ContaService } from 'src/app/conta/_services/conta.service';
import { SettingsService } from 'src/app/_services/settings.service';

import { Dashboard } from 'src/app/_models/dashboard';

@Injectable({ providedIn: 'root' })
export class DashboardService
{
    constructor(private contaService: ContaService, private http: HttpClient, private config: SettingsService) { }

    public aceitarTermos()
    {
        var usuario_id = this.contaService.usuarioLogado.id;
        const body = { usuario_id };

        return this.http.put<any>(`${this.config.getApiUrl()}aceitarTermos`, body);
    }

    public obter() : Observable<Dashboard[]>
    {
        var usuario_id = this.contaService.usuarioLogado.id;
        return this.http.get<any>(`${this.config.getApiUrl()}dashboard/` + usuario_id);
    }
}
