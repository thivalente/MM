import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { ContaService } from 'src/app/conta/conta.service';
import { SettingsService } from 'src/app/_services/settings.service';

import { Dashboard } from 'src/app/_models/dashboard';

@Injectable({ providedIn: 'root' })
export class DashboardService
{
    constructor(private contaService: ContaService, private http: HttpClient, private config: SettingsService) { }

    public obter() : Observable<Dashboard[]>
    {
        var usuario_id = this.contaService.usuarioLogado.id;
        return this.http.get<any>(`${this.config.getApiUrl()}dashboard/` + usuario_id);
    }
}
