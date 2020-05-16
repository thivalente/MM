import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import { ContaService } from 'src/app/conta/conta.service';
import { SettingsService } from 'src/app/_services/settings.service';

@Injectable({ providedIn: 'root' })
export class DashboardService
{
    constructor(private contaService: ContaService, private http: HttpClient, private config: SettingsService) { }

    public obter()
    {
        var usuario_id = this.contaService.usuarioLogado.id;
        return this.http.get<any>(`${this.config.getApiUrl()}dashboard/` + usuario_id);
    }
}
