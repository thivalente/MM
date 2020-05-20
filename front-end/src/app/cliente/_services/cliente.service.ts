import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { SyncRequestClient } from 'ts-sync-request/dist'

declare const obterDataFormatada_ddMMyyyy: any;
declare const obterDataJavascript: any;

@Injectable({ providedIn: 'root' })
export class ClienteService
{
    constructor() { }

    public listarDiasUteis(dataInicioString, dataFimString)
    {
        var result = [];
        var dataInicio = obterDataJavascript(dataInicioString);
        var dataFim = obterDataJavascript(dataFimString);

        var feriados = this.getJSONFeriados();
        var currentDate = dataInicio;

        while (currentDate <= dataFim)
        {
            var currDateString = currentDate.toISOString().substring(0, currentDate.toISOString().indexOf('T')) + 'T00:00:00';
            var weekDay = currentDate.getDay();

            if(weekDay !== 0 && weekDay != 6 && !feriados.includes(currDateString))
                result.push(obterDataFormatada_ddMMyyyy(currentDate));

            currentDate.setDate(currentDate.getDate() + 1); 
        }

        return result;
    }

    private getJSONFeriados() : Array<string>
    {
        return new SyncRequestClient().get("assets/json/feriados.json");
    }
}
